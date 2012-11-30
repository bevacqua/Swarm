using System;
using System.Web.Security;
using Castle.Windsor;

namespace Swarm.Common.Mvc.IoC.Membership
{
    public abstract class WindsorMembershipProvider : MembershipProvider
    {
        protected internal abstract Lazy<IWindsorContainer> Container { get; }

        private MembershipProvider GetProvider()
        {
            MembershipProvider provider = Container.Value.Resolve<MembershipProvider>();
            return provider;
        }

        private T WithProvider<T>(Func<MembershipProvider, T> action)
        {
            MembershipProvider provider = GetProvider();
            try
            {
                return action(provider);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        private void WithProvider(Action<MembershipProvider> action)
        {
            MembershipProvider provider = GetProvider();
            try
            {
                action(provider);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            MembershipProvider provider = GetProvider();
            try
            {
                return provider.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, providerUserKey, out status);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return WithProvider(p => p.ChangePasswordQuestionAndAnswer(username, password, newPasswordAnswer, newPasswordAnswer));
        }

        public override string GetPassword(string username, string answer)
        {
            return WithProvider(p => p.GetPassword(username, answer));
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return WithProvider(p => p.ChangePassword(username, oldPassword, newPassword));
        }

        public override string ResetPassword(string username, string answer)
        {
            return WithProvider(p => p.ResetPassword(username, answer));
        }

        public override void UpdateUser(MembershipUser user)
        {
            WithProvider(p => p.UpdateUser(user));
        }

        public override bool ValidateUser(string username, string password)
        {
            return WithProvider(p => p.ValidateUser(username, password));
        }

        public override bool UnlockUser(string userName)
        {
            return WithProvider(p => p.UnlockUser(userName));
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(providerUserKey, userIsOnline));
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            return WithProvider(p => p.GetUser(username, userIsOnline));
        }

        public override string GetUserNameByEmail(string email)
        {
            return WithProvider(p => p.GetUserNameByEmail(email));
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return WithProvider(p => p.DeleteUser(username, deleteAllRelatedData));
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipProvider provider = GetProvider();
            try
            {
                return provider.GetAllUsers(pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            return WithProvider(p => p.GetNumberOfUsersOnline());
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipProvider provider = GetProvider();
            try
            {
                return provider.FindUsersByName(usernameToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var provider = GetProvider();
            try
            {
                return provider.FindUsersByEmail(emailToMatch, pageIndex, pageSize, out totalRecords);
            }
            finally
            {
                Container.Value.Release(provider);
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return WithProvider(p => p.EnablePasswordRetrieval); }
        }

        public override bool EnablePasswordReset
        {
            get { return WithProvider(p => p.EnablePasswordReset); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return WithProvider(p => p.RequiresQuestionAndAnswer); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return WithProvider(p => p.MaxInvalidPasswordAttempts); }
        }

        public override int PasswordAttemptWindow
        {
            get { return WithProvider(p => p.PasswordAttemptWindow); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return WithProvider(p => p.RequiresUniqueEmail); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return WithProvider(p => p.PasswordFormat); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return WithProvider(p => p.MinRequiredPasswordLength); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return WithProvider(p => p.MinRequiredNonAlphanumericCharacters); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return WithProvider(p => p.PasswordStrengthRegularExpression); }
        }

        public override string ApplicationName
        {
            get { return WithProvider(p => p.ApplicationName); }
            set { WithProvider(p => p.ApplicationName = value); }
        }
    }
}
