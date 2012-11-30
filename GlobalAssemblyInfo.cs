using System.Reflection;
using System.Runtime.InteropServices;
using log4net.Config;

[assembly: AssemblyCompany("GroupCommerce")]
[assembly: AssemblyProduct("The Swarm")]
[assembly: AssemblyCopyright("Copyright © GroupCommerce 2012")]
[assembly: ComVisible(false)]
[assembly: AssemblyVersion("1.0.*")]

// NOTE: the log4net configuration must be at the specified relative path from the entry point project root

[assembly: XmlConfigurator(Watch = true, ConfigFile = "Xml/log4net.config")]