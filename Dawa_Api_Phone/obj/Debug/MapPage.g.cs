﻿

#pragma checksum "C:\Users\Daniel Kaas\Documents\Visual Studio 2015\Projects\Dawa_Api_Phone\Dawa_Api_Phone\MapPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6FE094CE43C2B3565E3CAB37364839D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dawa_Api_Phone
{
    partial class MapPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 34 "..\..\MapPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.MapControl_Loaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 35 "..\..\MapPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.button_Mail_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


