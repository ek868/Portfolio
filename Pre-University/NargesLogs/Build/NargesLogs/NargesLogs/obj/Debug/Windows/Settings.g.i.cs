﻿#pragma checksum "..\..\..\Windows\Settings.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "261C98AD4EC49345F9FC5896EAFD655CC5E8D400"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NargesLogs;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace NargesLogs {
    
    
    /// <summary>
    /// Settings
    /// </summary>
    public partial class Settings : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Windows\Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtIP;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Windows\Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPort;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Windows\Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Windows\Settings.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSave;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NargesLogs;component/windows/settings.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Windows\Settings.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txtIP = ((System.Windows.Controls.TextBox)(target));
            
            #line 22 "..\..\..\Windows\Settings.xaml"
            this.txtIP.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txtIP_MouseEnter);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Windows\Settings.xaml"
            this.txtIP.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txtIP_MouseLeave);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Windows\Settings.xaml"
            this.txtIP.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtIP_KeyUp);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Windows\Settings.xaml"
            this.txtIP.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtIP_KeyDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtPort = ((System.Windows.Controls.TextBox)(target));
            
            #line 24 "..\..\..\Windows\Settings.xaml"
            this.txtPort.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txtPort_MouseEnter);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\Windows\Settings.xaml"
            this.txtPort.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txtPort_MouseLeave);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\Windows\Settings.xaml"
            this.txtPort.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtPort_KeyUp);
            
            #line default
            #line hidden
            
            #line 24 "..\..\..\Windows\Settings.xaml"
            this.txtPort.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtPort_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Windows\Settings.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnSave = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\Windows\Settings.xaml"
            this.btnSave.Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

