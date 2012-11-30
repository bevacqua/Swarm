using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Dapper;

namespace Swarm.Overmind.Data.Dapper
{
    public static class SqlMapperExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IList<PropertyInfo>> TypeProperties = new ConcurrentDictionary<RuntimeTypeHandle, IList<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, string> GetQueries = new ConcurrentDictionary<RuntimeTypeHandle, string>();

        private static IEnumerable<PropertyInfo> TypePropertiesCache(Type type)
        {
            IList<PropertyInfo> cache;
            if (TypeProperties.TryGetValue(type.TypeHandle, out cache))
            {
                return cache;
            }
            IList<PropertyInfo> properties = type.GetProperties().Where(IsWritable).ToList();
            TypeProperties[type.TypeHandle] = properties;
            return properties;
        }

        public static bool IsWritable(PropertyInfo propertyInfo)
        {
            Type t = propertyInfo.PropertyType;
            bool primitive = t.IsPrimitive || t.IsValueType || t == typeof (string);
            return primitive;
        }

        /// <summary>
        /// Returns a single entity by a single id from table "Ts". T must be of interface type. 
        /// Created entity is tracked/intercepted for changes and used by the Update() extension. 
        /// </summary>
        /// <typeparam name="T">Interface type to create and populate</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="id">Id of the entity to get.</param>
        /// <param name="transaction">The transaction scope</param>
        /// <param name="commandTimeout">A command timeout</param>
        /// <returns>Entity of T</returns>
        public static T Get<T>(this IDbConnection connection, dynamic id, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Type type = typeof (T);
            string sql;
            if (!GetQueries.TryGetValue(type.TypeHandle, out sql))
            {
                sql = string.Format("SELECT [{0}].* FROM [{0}] WHERE [{0}].[Id] = @id", type.Name);
                GetQueries[type.TypeHandle] = sql;
            }

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@id", id);

            T proxy;

            if (type.IsInterface)
            {
                var result = connection.Query(sql, dynamicParameters).FirstOrDefault() as IDictionary<string, object>;
                if (result == null)
                {
                    return null;
                }
                proxy = ProxyGenerator.GetInterfaceProxy<T>();

                foreach (PropertyInfo property in TypePropertiesCache(type))
                {
                    object val = result[property.Name];
                    property.SetValue(proxy, val, null);
                }
                ((IProxy)proxy).IsDirty = false; // reset change tracking and return.
            }
            else
            {
                proxy = connection.Query<T>(sql, dynamicParameters, transaction, commandTimeout: commandTimeout).FirstOrDefault();
            }
            return proxy;
        }

        /// <summary>
        /// Inserts an entity into table "Ts" and returns identity id.
        /// </summary>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToInsert">Entity to insert</param>
        /// <param name="transaction">The transaction scope</param>
        /// <param name="commandTimeout">A command timeout</param>
        /// <returns>Identity of inserted entity</returns>
        public static long Insert<T>(this IDbConnection connection, T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            Type type = typeof (T);
            StringBuilder columnList = new StringBuilder();

            List<PropertyInfo> allProperties = TypePropertiesCache(type).ToList();
            PropertyInfo key = allProperties.First(prop => prop.Name.ToLowerInvariant() == "id");
            List<PropertyInfo> allPropertiesExceptKey = allProperties.Except(new[] {key}).ToList();

            for (int i = 0; i < allPropertiesExceptKey.Count; i++)
            {
                PropertyInfo property = allPropertiesExceptKey.ElementAt(i);
                columnList.Append(property.Name);
                if (i < allPropertiesExceptKey.Count - 1)
                {
                    columnList.Append(", ");
                }
            }

            StringBuilder parameterList = new StringBuilder();
            for (int i = 0; i < allPropertiesExceptKey.Count; i++)
            {
                PropertyInfo property = allPropertiesExceptKey.ElementAt(i);
                parameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKey.Count - 1)
                {
                    parameterList.Append(", ");
                }
            }
            ISqlAdapter adapter = GetFormatter(connection);
            int id = adapter.Insert(connection, transaction, commandTimeout, type.Name, columnList.ToString(), parameterList.ToString(), key, entityToInsert);
            return id;
        }

        /// <summary>
        /// Updates entity in table "Ts", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">Type to be updated</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction">The transaction scope</param>
        /// <param name="commandTimeout">A command timeout</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public static bool Update<T>(this IDbConnection connection, T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            IProxy proxy = entityToUpdate as IProxy;
            if (proxy != null && !proxy.IsDirty)
            {
                return false;
            }
            Type type = typeof (T);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE [{0}] SET ", type.Name);

            IEnumerable<PropertyInfo> allProperties = TypePropertiesCache(type).ToList();
            PropertyInfo key = allProperties.First(prop => prop.Name.ToLowerInvariant() == "id");
            List<PropertyInfo> allPropertiesExceptKey = allProperties.Except(new[] {key}).ToList();

            for (int i = 0; i < allPropertiesExceptKey.Count; i++)
            {
                PropertyInfo property = allPropertiesExceptKey.ElementAt(i);
                sb.AppendFormat("{0} = @{1}", property.Name, property.Name);
                if (i < allPropertiesExceptKey.Count - 1)
                {
                    sb.AppendFormat(", ");
                }
            }
            sb.AppendFormat(" WHERE [{0}].[Id] = @Id", type.Name);
            int updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }

        /// <summary>
        /// Delete entity in table "Ts".
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="connection">Open SqlConnection</param>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction scope</param>
        /// <param name="commandTimeout">A command timeout</param>
        /// <returns>true if deleted, false if not found</returns>
        public static bool Delete<T>(this IDbConnection connection, T entityToDelete, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
            if (entityToDelete == null)
            {
                throw new ArgumentNullException("entityToDelete");
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("DELETE FROM [{0}] WHERE [{0}].[Id] = @Id", typeof (T).Name);
            int deleted = connection.Execute(sb.ToString(), entityToDelete, transaction, commandTimeout);
            return deleted > 0;
        }

        public static ISqlAdapter GetFormatter(IDbConnection connection)
        {
            return new SqlServerAdapter();
        }

        private static class ProxyGenerator
        {
            private static readonly Dictionary<Type, object> TypeCache = new Dictionary<Type, object>();

            private static AssemblyBuilder GetAsmBuilder(string name)
            {
                AssemblyBuilder assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(new AssemblyName {Name = name}, AssemblyBuilderAccess.Run); // NOTE: to save, use RunAndSave
                return assemblyBuilder;
            }

            public static T GetInterfaceProxy<T>()
            {
                Type type = typeof (T);

                object cached;
                if (TypeCache.TryGetValue(type, out cached))
                {
                    return (T)cached;
                }
                AssemblyBuilder assemblyBuilder = GetAsmBuilder(type.Name);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("SqlMapperExtensions." + type.Name); // NOTE: to save, add "asdasd.dll" parameter

                Type interfaceType = typeof (IProxy);
                TypeBuilder typeBuilder = moduleBuilder.DefineType(type.Name + "_" + Guid.NewGuid(), TypeAttributes.Public | TypeAttributes.Class);
                typeBuilder.AddInterfaceImplementation(type);
                typeBuilder.AddInterfaceImplementation(interfaceType);

                // create our _isDirty field, which implements IProxy.
                MethodInfo setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);

                // generate a field for each property, which implements the T.
                foreach (PropertyInfo property in typeof (T).GetProperties())
                {
                    CreateProperty<T>(typeBuilder, property.Name, property.PropertyType, setIsDirtyMethod);
                }

                Type generatedType = typeBuilder.CreateType();

                // assemblyBuilder.Save(name + ".dll");  //NOTE: to save, uncomment.

                object generatedObject = Activator.CreateInstance(generatedType);

                TypeCache.Add(type, generatedObject);
                return (T)generatedObject;
            }

            private static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
            {
                Type propType = typeof (bool);
                FieldBuilder field = typeBuilder.DefineField("_" + "IsDirty", propType, FieldAttributes.Private);
                PropertyBuilder property = typeBuilder.DefineProperty("IsDirty", System.Reflection.PropertyAttributes.None, propType, new[] {propType});

                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.SpecialName | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig;

                // Define the "get" and "set" accessor methods
                MethodBuilder currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + "IsDirty", getSetAttr, propType, Type.EmptyTypes);
                ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, field);
                currGetIL.Emit(OpCodes.Ret);

                MethodBuilder currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + "IsDirty", getSetAttr, null, new[] {propType});
                ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, field);
                currSetIL.Emit(OpCodes.Ret);

                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);

                MethodInfo getMethod = typeof (IProxy).GetMethod("get_" + "IsDirty");
                MethodInfo setMethod = typeof (IProxy).GetMethod("set_" + "IsDirty");
                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);

                return currSetPropMthdBldr;
            }

            private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod)
            {
                // define the field and the property.
                FieldBuilder field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
                PropertyBuilder property = typeBuilder.DefineProperty(propertyName, System.Reflection.PropertyAttributes.None, propType, new[] {propType});

                const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;

                // define the "get" and "set" accessor methods.
                MethodBuilder currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName, getSetAttr, propType, Type.EmptyTypes);
                ILGenerator currGetIL = currGetPropMthdBldr.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, field);
                currGetIL.Emit(OpCodes.Ret);

                // store value in private field and set the IsDirty flag.
                MethodBuilder currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName, getSetAttr, null, new[] {propType});
                ILGenerator currSetIL = currSetPropMthdBldr.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, field);
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldc_I4_1);
                currSetIL.Emit(OpCodes.Call, setIsDirtyMethod);
                currSetIL.Emit(OpCodes.Ret);

                property.SetGetMethod(currGetPropMthdBldr);
                property.SetSetMethod(currSetPropMthdBldr);

                MethodInfo getMethod = typeof (T).GetMethod("get_" + propertyName);
                MethodInfo setMethod = typeof (T).GetMethod("set_" + propertyName);
                typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);
            }
        }

        private interface IProxy
        {
            bool IsDirty { get; set; }
        }
    }

    public interface ISqlAdapter
    {
        int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, PropertyInfo key, object entityToInsert);
    }

    public class SqlServerAdapter : ISqlAdapter
    {
        public int Insert(IDbConnection connection, IDbTransaction transaction, int? commandTimeout, String tableName, string columnList, string parameterList, PropertyInfo key, object entityToInsert)
        {
            string sql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, columnList, parameterList);

            connection.Execute(sql, entityToInsert, transaction, commandTimeout);

            // NOTE: would prefer to use IDENT_CURRENT('tablename') or IDENT_SCOPE but these are not available on SQLCE
            IEnumerable<dynamic> result = connection.Query("SELECT @@IDENTITY Id", transaction: transaction, commandTimeout: commandTimeout);
            int id = (int)result.First().Id;
            if (key != null)
            {
                key.SetValue(entityToInsert, id, null);
            }
            return id;
        }
    }
}
