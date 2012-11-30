namespace Swarm.Common.Quartz
{
    public interface IJobAutoRunner
    {
        /// <summary>
        /// Fires jobs scheduled to run on application start using the AutoRun attribute.
        /// </summary>
        void Fire();
    }
}
