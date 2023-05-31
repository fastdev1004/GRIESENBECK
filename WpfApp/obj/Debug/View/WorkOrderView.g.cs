﻿#pragma checksum "..\..\..\View\WorkOrderView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4F4EE2DC8D2D3C5ABECC7BBF270F54351B804725"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using WpfApp.View;


namespace WpfApp.View {
    
    
    /// <summary>
    /// WorkOrderView
    /// </summary>
    public partial class WorkOrderView : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox projectContacts;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid WOListView;
        
        #line default
        #line hidden
        
        
        #line 163 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Crew_ComBo;
        
        #line default
        #line hidden
        
        
        #line 169 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Supt_ComBo;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid WorkOrderNoteGrid;
        
        #line default
        #line hidden
        
        
        #line 286 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ProjectWorkOrderList;
        
        #line default
        #line hidden
        
        
        #line 351 "..\..\..\View\WorkOrderView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ProjectLaborList;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfApp;component/view/workorderview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\WorkOrderView.xaml"
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
            
            #line 34 "..\..\..\View\WorkOrderView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.goBack);
            
            #line default
            #line hidden
            return;
            case 2:
            this.projectContacts = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.WOListView = ((System.Windows.Controls.DataGrid)(target));
            
            #line 71 "..\..\..\View\WorkOrderView.xaml"
            this.WOListView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.SelectWorkOrdersList);
            
            #line default
            #line hidden
            return;
            case 4:
            this.Crew_ComBo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.Supt_ComBo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.WorkOrderNoteGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.ProjectWorkOrderList = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.ProjectLaborList = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

