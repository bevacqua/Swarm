using System;
using System.Reflection;
using System.Text;
using Swarm.Common.Extensions;
using Swarm.Common.Interface;
using Swarm.Common.Resources;

namespace Swarm.Common
{
    public abstract class ResourceHelper<TRawString> : IResourceHelper<TRawString> where TRawString : class
    {
        // Convention: Resources are stored in the same path as the view in a Resources directory:
        //                 + Views
        //                   + Account
        //                     + Resources
        //                        o Register.resx (Class name: [NamespaceRoot].Views.[Controller].Resources.[View])
        //                     o Register.aspx (the view)
        //
        //
        // Convention: Shared resources are stored in the /Views/Shared/Resources directory:
        //                 + Views
        //                   + Shared
        //                     + Resources
        //                        o Buttons.resx  (view is "Buttons")
        //

        private readonly string namespaceRoot;

        protected ResourceHelper(string namespaceRoot)
        {
            if (namespaceRoot == null)
            {
                throw new ArgumentNullException("namespaceRoot");
            }
            this.namespaceRoot = namespaceRoot;
        }

        protected abstract Assembly GetReferenceAssembly();
        protected abstract string GetViewPath();
        protected abstract string GetSharedResourceNamespace();
        protected abstract TRawString RawConverter(string resource);

        /// <summary>
        /// Returns the value of the requested string resource from the local resource file. 
        /// </summary>
        internal string GetStringResource(string key)
        {
            string resource = null;

            string viewPath = GetViewPath();
            if (!viewPath.NullOrEmpty()) // expecting: "~/Views/User/LogOn.cshtml" or "Account/Register"
            {
                StringBuilder sb = new StringBuilder(namespaceRoot);
                string[] parts = viewPath.Split('/');
                foreach (string part in parts)
                {
                    if (part == "~")
                    {
                        continue;
                    }
                    if (part.Contains("."))
                    {
                        sb.Append(".Resources.");
                        sb.Append(part.Substring(0, part.IndexOf('.')));
                    }
                    else
                    {
                        sb.Append('.');
                        sb.Append(part);
                    }
                }
                sb.Append(", ");
                sb.Append(GetReferenceAssembly().FullName);

                string resourceTypeName = sb.ToString();

                resource = GetResourceStringFromType(key, resourceTypeName);
            }

            return resource ?? key; // return key if resource not found.
        }

        /// <summary>
        /// Returns the value of the specified string from the /Views/Shared/Resources directory
        /// </summary>
        /// <param name="file">Name (without extension) of the Shared resource file to read from.</param>
        /// <param name="key">Name of the string resource to get.</param>
        internal string GetSharedResource(string file, string key)
        {
            string assemblyName = GetReferenceAssembly().FullName;
            string resourceTypeName = "{0}.{1}.Resources.{2}, {3}".FormatWith(namespaceRoot, GetSharedResourceNamespace(), file, assemblyName);

            string resource = GetResourceStringFromType(key, resourceTypeName);
            return resource ?? key; // return key if resource is not found.
        }

        /// <summary>
        /// Returns a resource based on its resource maanger type and resource string name.
        /// </summary>
        internal string GetStringResource(Type resourceType, string resourceName, bool throwIfNotFound = false)
        {
            if (resourceType == null)
            {
                if (throwIfNotFound)
                {
                    throw new ArgumentNullException("resourceType");
                }
                return resourceName;
            }
            if (resourceName == null)
            {
                if (throwIfNotFound)
                {
                    throw new ArgumentNullException("resourceName");
                }
                return null;
            }
            PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
            if (property == null)
            {
                if (throwIfNotFound)
                {
                    throw new InvalidOperationException(Error.ResourceTypeNoProperty.FormatWith(resourceName));
                }
                return resourceName;
            }
            if (property.PropertyType != typeof (string))
            {
                if (throwIfNotFound)
                {
                    throw new InvalidOperationException(Error.ResourceTypeNotString.FormatWith(resourceName));
                }
                return resourceName;
            }
            return (string)property.GetValue(null, null);
        }

        /// <summary>
        /// Loads a resource from a resource file using reflection of the resource's type.
        /// </summary>
        protected internal string GetResourceStringFromType(string key, string resourceTypeName)
        {
            // Use reflection to get the ResourceManager property for requested resource file
            Type resourceType = Type.GetType(resourceTypeName, false, true);
            string resource = GetStringResource(resourceType, key);
            return resource ?? key;
        }

        /// <summary>
        /// Returns a formatted string using a string resource as the format string.
        /// </summary>
        /// <param name="key">The name of the string resource or the format string resource.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        /// <returns></returns>
        public string String(string key, params object[] args)
        {
            string formatString = GetStringResource(key);
            return formatString.FormatWith(args);
        }

        /// <summary>
        /// Returns a formatted string using a shared string resource from the /Views/Shared/Resources directory as the format string.
        /// </summary>
        /// <param name="file">Name of the Shared resource file to read from.</param>
        /// <param name="key">The name of the string resource or the format string resource.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public string Shared(string file, string key, params object[] args)
        {
            string formatString = GetSharedResource(file, key);
            return formatString.FormatWith(args);
        }

        public TRawString Raw(string key, params object[] args)
        {
            string resource = String(key, args);
            return RawConverter(resource);
        }

        public TRawString RawShared(string file, string key, params object[] args)
        {
            string resource = Shared(file, key, args);
            return RawConverter(resource);
        }
    }
}
