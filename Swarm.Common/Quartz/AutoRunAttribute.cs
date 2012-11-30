using System;

namespace Swarm.Common.Quartz
{
    /// <summary>
    /// Jobs decorated with this attribute should be fired when the application starts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AutoRunAttribute : Attribute
    {
        /// <summary>
        /// Set this flag to true if you want the Auto Run job instance to run just one time.
        /// </summary>
        public bool RunOnce { get; set; }

        /// <summary>
        /// Delay to start the job for the first time, specified in minutes.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Interval between jobs, specified in minutes.
        /// </summary>
        public int? Interval { get; set; }

        /// <summary>
        /// Default interval if no interval is set, specified in minutes.
        /// </summary>
        public const int DefaultInterval = 5;
    }
}
