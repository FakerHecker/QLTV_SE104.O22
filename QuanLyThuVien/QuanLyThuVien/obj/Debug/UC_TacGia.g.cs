﻿#pragma checksum "..\..\UC_TacGia.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "37EAAF2EFAFAF6411BFFD45BFFC1FEE3D642A9665E6934A492779FB871A5CCD4"
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
    /// UC_TacGia
    /// </summary>
    public partial class UC_TacGia : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 36 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblMaTacGia;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbTenTacGia;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnThemMoi;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCapNhat;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnLuu;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnXoa;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\UC_TacGia.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgvTacGia;
        
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
            System.Uri resourceLocater = new System.Uri("/QuanLyThuVien;component/uc_tacgia.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UC_TacGia.xaml"
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
            this.tblMaTacGia = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.txbTenTacGia = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btnThemMoi = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\UC_TacGia.xaml"
            this.btnThemMoi.Click += new System.Windows.RoutedEventHandler(this.btnThemMoi_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnCapNhat = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\UC_TacGia.xaml"
            this.btnCapNhat.Click += new System.Windows.RoutedEventHandler(this.btnCapNhat_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnLuu = ((System.Windows.Controls.Button)(target));
            
            #line 45 "..\..\UC_TacGia.xaml"
            this.btnLuu.Click += new System.Windows.RoutedEventHandler(this.btnLuu_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.btnXoa = ((System.Windows.Controls.Button)(target));
            
            #line 46 "..\..\UC_TacGia.xaml"
            this.btnXoa.Click += new System.Windows.RoutedEventHandler(this.btnXoa_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.dgvTacGia = ((System.Windows.Controls.DataGrid)(target));
            
            #line 52 "..\..\UC_TacGia.xaml"
            this.dgvTacGia.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dgvTacGia_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

