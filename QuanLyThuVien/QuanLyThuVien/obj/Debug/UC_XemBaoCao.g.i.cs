﻿#pragma checksum "..\..\UC_XemBaoCao.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EB8033A1A481A799584C4650639DA04DD120195F313FD9CCC506CA28140F3D8F"
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
    /// UC_XemBaoCao
    /// </summary>
    public partial class UC_XemBaoCao : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 35 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gr;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cbLoaiBaoCao;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataGridView;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbTongSoLuotMuon;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblTongSoLuotMuon;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnInBaoCao;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\UC_XemBaoCao.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnXoa;
        
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
            System.Uri resourceLocater = new System.Uri("/QuanLyThuVien;component/uc_xembaocao.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\UC_XemBaoCao.xaml"
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
            this.gr = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.cbLoaiBaoCao = ((System.Windows.Controls.ComboBox)(target));
            
            #line 46 "..\..\UC_XemBaoCao.xaml"
            this.cbLoaiBaoCao.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cbLoaiBaoCao_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.dataGridView = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 4:
            this.lbTongSoLuotMuon = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.tblTongSoLuotMuon = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnInBaoCao = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\UC_XemBaoCao.xaml"
            this.btnInBaoCao.Click += new System.Windows.RoutedEventHandler(this.btnInBaoCao_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnXoa = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\UC_XemBaoCao.xaml"
            this.btnXoa.Click += new System.Windows.RoutedEventHandler(this.btnXoa_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

