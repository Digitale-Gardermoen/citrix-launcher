﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace citrix_launcher.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("citrix_launcher.Properties.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to No.
        /// </summary>
        internal static string buttonNo {
            get {
                return ResourceManager.GetString("buttonNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        internal static string buttonYes {
            get {
                return ResourceManager.GetString("buttonYes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Click OK to exit the application..
        /// </summary>
        internal static string popupErrorBottomText {
            get {
                return ResourceManager.GetString("popupErrorBottomText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid configuration file..
        /// </summary>
        internal static string popupErrorCfgFileInvalid {
            get {
                return ResourceManager.GetString("popupErrorCfgFileInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find configuration file..
        /// </summary>
        internal static string popupErrorCfgFileMissing {
            get {
                return ResourceManager.GetString("popupErrorCfgFileMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Group based configuration not found..
        /// </summary>
        internal static string popupErrorCfgFileNoGroupConfig {
            get {
                return ResourceManager.GetString("popupErrorCfgFileNoGroupConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to read configuration file..
        /// </summary>
        internal static string popupErrorCfgFileNotReadable {
            get {
                return ResourceManager.GetString("popupErrorCfgFileNotReadable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Citrix Launcher: ERROR.
        /// </summary>
        internal static string popupErrorTitle {
            get {
                return ResourceManager.GetString("popupErrorTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Example text, replace me in Strings.resx
        ///Click Yes to launch Citrix session..
        /// </summary>
        internal static string popupText {
            get {
                return ResourceManager.GetString("popupText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Citrix Launcher: Regex match.
        /// </summary>
        internal static string popupWindowTitle {
            get {
                return ResourceManager.GetString("popupWindowTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Would you like to start Citrix Launcher?.
        /// </summary>
        internal static string promptText {
            get {
                return ResourceManager.GetString("promptText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Citrix Launcher.
        /// </summary>
        internal static string promptWindowTitle {
            get {
                return ResourceManager.GetString("promptWindowTitle", resourceCulture);
            }
        }
    }
}
