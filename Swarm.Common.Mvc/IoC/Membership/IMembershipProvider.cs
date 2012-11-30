namespace Swarm.Common.Mvc.IoC.Membership
{
    public interface IMembershipProvider
    {
        bool ValidateUser(string username, string password);
    }
}
