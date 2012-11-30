using SquishIt.Framework.Base;
using Swarm.Common.Configuration;

namespace Swarm.Common.Mvc.Extensions
{
    public static class BundleBaseHelpers
    {
        public static T WithConfiguration<T>(this BundleBase<T> bundle) where T : BundleBase<T>
        {
            if (Config.Mvc.Debug.IgnoreMinification)
            {
                return bundle.ForceDebug();
            }
            return bundle.ForceRelease();
        }

        /// <summary>
        /// Add a list of file paths to the bundle.
        /// For some reason this was removed in SquishIt 0.9.
        /// </summary>
        public static T Add<T>(this BundleBase<T> bundle, string[] paths) where T : BundleBase<T>
        {
            foreach (string path in paths)
            {
                bundle.Add(path);
            }
            return (T)bundle;
        }
    }
}
