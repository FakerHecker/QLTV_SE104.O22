﻿#pragma checksum "..\..\UC_TraCuuSach.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6D22083E10EB047B55D864D4E4888AB7C3B0A77002C7B63A59FFD218539C8B51"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using QuanLyThuVien;
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


namespace QuanLyThuVien {
    
    
    /// <summary>
    /// UC_TraCuuSach
    /// </summary>
    public partial class UC_TraCuuSach : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 46 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTimSach;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHuy;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GroupBox gb;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbKey;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbValue;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblTongSoSach;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblSoSachTimThay;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\UC_TraCuuSach.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgvCuonSach;
        
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
            System.Uri resourceLocater = new System.Uri("/QuanLyThuVien;component/uc_tracuusach.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UC_TraCuuSach.xaml"
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
            this.btnTimSach = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\UC_TraCuuSach.xaml"
            this.btnTimSach.Click += new System.Windows.RoutedEventHandler(this.btnTimSach_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btnHuy = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\UC_TraCuuSach.xaml"
            this.btnHuy.Click += new System.Windows.RoutedEventHandler(this.btnHuy_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.gb = ((System.Windows.Controls.GroupBox)(target));
            return;
            case 4:
            this.cbKey = ((System.Windows.Controls.ComboBox)(target));
            
            #line 60 "..\..\UC_TraCuuSach.xaml"
            this.cbKey.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbKey_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cbValue = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.tblTongSoSach = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.tblSoSachTimThay = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.dgvCuonSach = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

