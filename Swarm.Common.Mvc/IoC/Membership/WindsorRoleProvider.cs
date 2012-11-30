using System;
using System.Web.Security;
using Castle.Windsor;

namespace Swarm.Common.Mvc.IoC.Membership
{
    public abstract class WindsorRoleProvider : RoleProvider
    {
        protected internal abstract Lazy<IWindsorContainer> Container { get; }

        private RoleProvider GetProvider()
        {
            RoleProvider provider = Container.Value.Resolve<RoleProvider>();
            return provider;
        }

        private T WithProvider<T>(Func<RoleProvider, T> action)
        {
            RoleProvider provider = GetProvider();
            try
            {
                return action(provider);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        private void WithProvider(Action<RoleProvider> action)
        {
            RoleProvider provider = GetProvider();
            try
            {
                action(provider);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return WithProvider(p => p.IsUserInRole(username, roleName));
        }

        public override string[] GetRolesForUser(string username)
        {
            return WithProvider(p => p.GetRolesForUser(username));
        }

        public override void CreateRole(string roleName)
        {
            WithProvider(p => p.CreateRole(roleName));
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return WithProvider(p => p.DeleteRole(roleName, throwOnPopulatedRole));
        }

        public override bool RoleExists(string roleName)
        {
            return WithProvider(p => p.RoleExists(roleName));
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.AddUsersToRoles(usernames, roleNames));
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            WithProvider(p => p.RemoveUsersFromRoles(usernames, roleNames));
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return WithProvider(p => p.GetUsersInRole(roleName));
        }

        public override string[] GetAllRoles()
        {
            return WithProvider(p => p.GetAllRoles());
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return WithProvider(p => p.FindUsersInRole(roleName, usernameToMatch));
        }

        public override string ApplicationName
        {
            get { return WithProvider(p => p.ApplicationName); }
            set { WithProvider(p => p.ApplicationName = value); }
        }
    }
}
