﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TBIControl.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int pmdbaudrate {
            get {
                return ((int)(this["pmdbaudrate"]));
            }
            set {
                this["pmdbaudrate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public global::System.IO.Ports.Parity pmdparity {
            get {
                return ((global::System.IO.Ports.Parity)(this["pmdparity"]));
            }
            set {
                this["pmdparity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int pmddatabits {
            get {
                return ((int)(this["pmddatabits"]));
            }
            set {
                this["pmddatabits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("One")]
        public global::System.IO.Ports.StopBits pmdstopbits {
            get {
                return ((global::System.IO.Ports.StopBits)(this["pmdstopbits"]));
            }
            set {
                this["pmdstopbits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public global::System.IO.Ports.Handshake pmdflowcontrol {
            get {
                return ((global::System.IO.Ports.Handshake)(this["pmdflowcontrol"]));
            }
            set {
                this["pmdflowcontrol"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int elecbaudrate {
            get {
                return ((int)(this["elecbaudrate"]));
            }
            set {
                this["elecbaudrate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public global::System.IO.Ports.Parity elecparity {
            get {
                return ((global::System.IO.Ports.Parity)(this["elecparity"]));
            }
            set {
                this["elecparity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int elecdatabits {
            get {
                return ((int)(this["elecdatabits"]));
            }
            set {
                this["elecdatabits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("One")]
        public global::System.IO.Ports.StopBits elecstopbits {
            get {
                return ((global::System.IO.Ports.StopBits)(this["elecstopbits"]));
            }
            set {
                this["elecstopbits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public global::System.IO.Ports.Handshake elecflowcontrol {
            get {
                return ((global::System.IO.Ports.Handshake)(this["elecflowcontrol"]));
            }
            set {
                this["elecflowcontrol"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string pmdport {
            get {
                return ((string)(this["pmdport"]));
            }
            set {
                this["pmdport"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool pmdconnected {
            get {
                return ((bool)(this["pmdconnected"]));
            }
            set {
                this["pmdconnected"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.IO.Ports.SerialPort pmdserobj {
            get {
                return ((global::System.IO.Ports.SerialPort)(this["pmdserobj"]));
            }
            set {
                this["pmdserobj"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string elecport {
            get {
                return ((string)(this["elecport"]));
            }
            set {
                this["elecport"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool elecconnected {
            get {
                return ((bool)(this["elecconnected"]));
            }
            set {
                this["elecconnected"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.IO.Ports.SerialPort elecserobj {
            get {
                return ((global::System.IO.Ports.SerialPort)(this["elecserobj"]));
            }
            set {
                this["elecserobj"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D")]
        public string pmdinitializecmd {
            get {
                return ((string)(this["pmdinitializecmd"]));
            }
            set {
                this["pmdinitializecmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("E")]
        public string pmdpositioncmd {
            get {
                return ((string)(this["pmdpositioncmd"]));
            }
            set {
                this["pmdpositioncmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("I")]
        public string pmdspeedcmd {
            get {
                return ((string)(this["pmdspeedcmd"]));
            }
            set {
                this["pmdspeedcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("B")]
        public string pmdgotoabscmd {
            get {
                return ((string)(this["pmdgotoabscmd"]));
            }
            set {
                this["pmdgotoabscmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C")]
        public string pmdgotorelcmd {
            get {
                return ((string)(this["pmdgotorelcmd"]));
            }
            set {
                this["pmdgotorelcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("H")]
        public string pmddriveoutcmd {
            get {
                return ((string)(this["pmddriveoutcmd"]));
            }
            set {
                this["pmddriveoutcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("G")]
        public string pmddriveincmd {
            get {
                return ((string)(this["pmddriveincmd"]));
            }
            set {
                this["pmddriveincmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("STA")]
        public string elecstartcmd {
            get {
                return ((string)(this["elecstartcmd"]));
            }
            set {
                this["elecstartcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HLD")]
        public string elecholdcmd {
            get {
                return ((string)(this["elecholdcmd"]));
            }
            set {
                this["elecholdcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("RES")]
        public string elecresetcmd {
            get {
                return ((string)(this["elecresetcmd"]));
            }
            set {
                this["elecresetcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("NUL")]
        public string elecnullcmd {
            get {
                return ((string)(this["elecnullcmd"]));
            }
            set {
                this["elecnullcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D1")]
        public string elecreadchratecmd {
            get {
                return ((string)(this["elecreadchratecmd"]));
            }
            set {
                this["elecreadchratecmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D0")]
        public string elecreadchcmd {
            get {
                return ((string)(this["elecreadchcmd"]));
            }
            set {
                this["elecreadchcmd"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0.5")]
        public string electempres {
            get {
                return ((string)(this["electempres"]));
            }
            set {
                this["electempres"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public string elecdataindex {
            get {
                return ((string)(this["elecdataindex"]));
            }
            set {
                this["elecdataindex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(";")]
        public string elecdatadivchar {
            get {
                return ((string)(this["elecdatadivchar"]));
            }
            set {
                this["elecdatadivchar"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D1")]
        public string elecdataid {
            get {
                return ((string)(this["elecdataid"]));
            }
            set {
                this["elecdataid"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public string elecdataidindex {
            get {
                return ((string)(this["elecdataidindex"]));
            }
            set {
                this["elecdataidindex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1E-7")]
        public string elecdatacutoff {
            get {
                return ((string)(this["elecdatacutoff"]));
            }
            set {
                this["elecdatacutoff"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public double minRS232updtime {
            get {
                return ((double)(this["minRS232updtime"]));
            }
            set {
                this["minRS232updtime"] = value;
            }
        }
    }
}
