using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Swarm.Common.Extensions;

namespace Swarm.Common.Mvc.Core.Routing
{
    public class EnumRouteConstraint<T> : IRouteConstraint
        where T : struct
    {
        private static readonly Lazy<HashSet<string>> _enumNames; // this field being static in a generic type is actually exactly what we need.

        static EnumRouteConstraint()
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException(Resources.Error.EnumRouteConstraint.FormatWith(typeof (T).FullName));
            }

            string[] names = Enum.GetNames(typeof (T));
            _enumNames = new Lazy<HashSet<string>>(() => new HashSet<string>
                                                             (
                                                             names.Select(name => name), StringComparer.InvariantCultureIgnoreCase
                                                             ));
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            bool match = _enumNames.Value.Contains(values[parameterName].ToString());
            return match;
        }
    }
}
