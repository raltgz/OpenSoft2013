﻿#pragma checksum "..\..\FavoritesResult.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F5ACC8A0981973690F050AD855F78709"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Citeseer {
    
    
    /// <summary>
    /// FavoritesResult
    /// </summary>
    public partial class FavoritesResult : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBoxItem listItem;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grid2;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock queryString;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock queryParams;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock queryType;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnViewSearch;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\FavoritesResult.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDelete;
        
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
            System.Uri resourceLocater = new System.Uri("/Rescite;component/favoritesresult.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\FavoritesResult.xaml"
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
            this.listItem = ((System.Windows.Controls.ListBoxItem)(target));
            
            #line 9 "..\..\FavoritesResult.xaml"
            this.listItem.MouseEnter += new System.Windows.Input.MouseEventHandler(this.addShade);
            
            #line default
            #line hidden
            
            #line 9 "..\..\FavoritesResult.xaml"
            this.listItem.MouseLeave += new System.Windows.Input.MouseEventHandler(this.removeShade);
            
            #line default
            #line hidden
            
            #line 9 "..\..\FavoritesResult.xaml"
            this.listItem.Selected += new System.Windows.RoutedEventHandler(this.listItem_Selected);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grid2 = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.queryString = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.queryParams = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.queryType = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btnViewSearch = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\FavoritesResult.xaml"
            this.btnViewSearch.Click += new System.Windows.RoutedEventHandler(this.btnSearch);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnDelete = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\FavoritesResult.xaml"
            this.btnDelete.Click += new System.Windows.RoutedEventHandler(this.delete);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

