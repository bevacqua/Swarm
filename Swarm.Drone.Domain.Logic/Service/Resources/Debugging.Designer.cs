﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Swarm.Drone.Domain.Logic.Service.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Debugging {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Debugging() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Swarm.Drone.Domain.Logic.Service.Resources.Debugging", typeof(Debugging).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Waiting for {0:N2} seconds to attain better synchronization..
        /// </summary>
        internal static string DroneService_Idle {
            get {
                return ResourceManager.GetString("DroneService_Idle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mapping {0} data rows as REST requests..
        /// </summary>
        internal static string DroneService_Mapping {
            get {
                return ResourceManager.GetString("DroneService_Mapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mapping data row headers..
        /// </summary>
        internal static string DroneService_MappingHeader {
            get {
                return ResourceManager.GetString("DroneService_MappingHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Drone won&apos;t sleep. Executing immediatly..
        /// </summary>
        internal static string DroneService_NoSleep {
            get {
                return ResourceManager.GetString("DroneService_NoSleep", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Received request to perform a Load Test..
        /// </summary>
        internal static string DroneService_Received {
            get {
                return ResourceManager.GetString("DroneService_Received", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Drone awaken after sleep time. Execution started..
        /// </summary>
        internal static string DroneService_Resumed {
            get {
                return ResourceManager.GetString("DroneService_Resumed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Returning a response from the WCF drone service..
        /// </summary>
        internal static string DroneService_Returned {
            get {
                return ResourceManager.GetString("DroneService_Returned", resourceCulture);
            }
        }
    }
}
