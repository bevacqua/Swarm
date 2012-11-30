using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Web.Mvc;
using Swarm.Common.Mvc.Core.Attributes;
using Swarm.Common.Mvc.Core.Models;
using Swarm.Common.Mvc.Interface;
using Swarm.Common.Resources;
using Swarm.Common.Utility;

namespace Swarm.Common.Mvc.Core.Controllers
{
    public class ResourceController : ExtendedController
    {
        private readonly IList<ResourceAssemblyLocation> locations;
        private readonly ResourceAssemblyLocation sharedLocation;
        private readonly IResourceCompressor compressor;

        public ResourceController(IList<ResourceAssemblyLocation> locations, IResourceCompressor compressor)
        {
            if (locations == null)
            {
                throw new ArgumentNullException("locations");
            }
            if (compressor == null)
            {
                throw new ArgumentNullException("compressor");
            }
            this.locations = locations;
            this.compressor = compressor;

            sharedLocation = new ResourceAssemblyLocation
            {
                Assembly = Assembly.GetAssembly(typeof (FileSystemHelper)),
                Namespace = Constants.SharedResourceNamespaceRoot
            };
        }

        [HttpGet]
        [NotAjax]
        [OutputCache(Duration = 3600)]
        public ContentResult Localization()
        {
            CultureInfo culture = CultureInfo.InvariantCulture; // easily replaceable by user culture.
            IEnumerable<ResourceLocalizationModel> model = GetLocalizationModel(culture);

            string partial = PartialViewString(null, model);
            string minified = compressor.MinifyJavaScript(partial, false);

            return new ContentResult
            {
                Content = minified,
                ContentEncoding = Encoding.UTF8,
                ContentType = Constants.JavaScriptContentType
            };
        }

        private IEnumerable<ResourceLocalizationModel> GetLocalizationModel(CultureInfo culture)
        {
            var resourceManagers = GetResourceManagers();
            foreach (var resourceManager in resourceManagers)
            {
                ResourceSet resourceSet = resourceManager.Value.GetResourceSet(culture, true, false);
                ResourceLocalizationModel file = new ResourceLocalizationModel
                {
                    Title = resourceManager.Key,
                    Items = resourceSet.Cast<DictionaryEntry>()
                };
                yield return file;
            }
        }

        private IEnumerable<KeyValuePair<string, ResourceManager>> GetResourceManagers()
        {
            IEnumerable<Type> resourceTypes = locations
                .Concat(new[] {sharedLocation})
                .SelectMany(location =>
                            location.Assembly.GetTypes().Where(type => location.Namespace == type.Namespace));

            foreach (Type type in resourceTypes)
            {
                ResourceManager manager = null;
                PropertyInfo prop = type.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.Static);
                if (prop != null)
                {
                    manager = prop.GetValue(null, null) as ResourceManager;
                }
                if (manager != null)
                {
                    yield return new KeyValuePair<string, ResourceManager>(type.Name, manager);
                }
            }
        }
    }

    public class ResourceAssemblyLocation
    {
        public Assembly Assembly { get; set; }
        public string Namespace { get; set; }
    }
}
