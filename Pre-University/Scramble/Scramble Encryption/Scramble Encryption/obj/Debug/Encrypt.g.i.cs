﻿#pragma checksum "..\..\Encrypt.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CE8616721CF465B457324007CEC4FECA"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Scramble_Encryption;
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


namespace Scramble_Encryption {
    
    
    /// <summary>
    /// Encrypt
    /// </summary>
    public partial class Encrypt : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 27 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lsvDnD;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtDropFilesHere;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHelp;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnBack;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtDestination;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbBruteKey;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbPassword;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEncrypt;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\Encrypt.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgPlusFile;
        
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
            System.Uri resourceLocater = new System.Uri("/Scramble Encryption;component/encrypt.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Encrypt.xaml"
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
            
            #line 8 "..\..\Encrypt.xaml"
            ((Scramble_Encryption.Encrypt)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_PreviewMouseDown);
            
            #line default
            #line hidden
            
            #line 8 "..\..\Encrypt.xaml"
            ((Scramble_Encryption.Encrypt)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lsvDnD = ((System.Windows.Controls.ListView)(target));
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.MouseEnter += new System.Windows.Input.MouseEventHandler(this.lsvDnD_MouseEnter);
            
            #line default
            #line hidden
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.MouseLeave += new System.Windows.Input.MouseEventHandler(this.lsvDnD_MouseLeave);
            
            #line default
            #line hidden
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.AddHandler(System.Windows.DragDrop.DropEvent, new System.Windows.DragEventHandler(this.ListView_Drop));
            
            #line default
            #line hidden
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.lsvDnD_MouseDown);
            
            #line default
            #line hidden
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.AddHandler(System.Windows.DragDrop.DragEnterEvent, new System.Windows.DragEventHandler(this.lsvDnD_DragEnter));
            
            #line default
            #line hidden
            
            #line 27 "..\..\Encrypt.xaml"
            this.lsvDnD.AddHandler(System.Windows.DragDrop.DragLeaveEvent, new System.Windows.DragEventHandler(this.lsvDnD_DragLeave));
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtDropFilesHere = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.btnHelp = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\Encrypt.xaml"
            this.btnHelp.Click += new System.Windows.RoutedEventHandler(this.btnHelp_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnBack = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\Encrypt.xaml"
            this.btnBack.Click += new System.Windows.RoutedEventHandler(this.btnBack_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtDestination = ((System.Windows.Controls.TextBlock)(target));
            
            #line 39 "..\..\Encrypt.xaml"
            this.txtDestination.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.txtDestination_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 39 "..\..\Encrypt.xaml"
            this.txtDestination.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txtDestination_MouseEnter);
            
            #line default
            #line hidden
            
            #line 39 "..\..\Encrypt.xaml"
            this.txtDestination.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txtDestination_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txbBruteKey = ((System.Windows.Controls.TextBox)(target));
            
            #line 40 "..\..\Encrypt.xaml"
            this.txbBruteKey.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.txbBruteKey_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 40 "..\..\Encrypt.xaml"
            this.txbBruteKey.GotFocus += new System.Windows.RoutedEventHandler(this.txbBruteKey_GotFocus);
            
            #line default
            #line hidden
            
            #line 40 "..\..\Encrypt.xaml"
            this.txbBruteKey.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txbBruteKey_MouseEnter);
            
            #line default
            #line hidden
            
            #line 40 "..\..\Encrypt.xaml"
            this.txbBruteKey.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txbBruteKey_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txbPassword = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\Encrypt.xaml"
            this.txbPassword.GotFocus += new System.Windows.RoutedEventHandler(this.txbPassword_GotFocus);
            
            #line default
            #line hidden
            
            #line 41 "..\..\Encrypt.xaml"
            this.txbPassword.MouseEnter += new System.Windows.Input.MouseEventHandler(this.txbPassword_MouseEnter);
            
            #line default
            #line hidden
            
            #line 41 "..\..\Encrypt.xaml"
            this.txbPassword.MouseLeave += new System.Windows.Input.MouseEventHandler(this.txbPassword_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnEncrypt = ((System.Windows.Controls.Button)(target));
            
            #line 42 "..\..\Encrypt.xaml"
            this.btnEncrypt.Click += new System.Windows.RoutedEventHandler(this.btnEncrypt_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.imgPlusFile = ((System.Windows.Controls.Image)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

