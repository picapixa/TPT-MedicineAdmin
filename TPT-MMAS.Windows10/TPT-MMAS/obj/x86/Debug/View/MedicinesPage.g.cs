﻿#pragma checksum "C:\Users\jrpal_000\Source\Repos\TPT-MMAS\TPT-MMAS\TPT-MMAS\View\MedicinesPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3C6857D0FE12C6B572564E09F4B27647"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TPT_MMAS.View
{
    partial class MedicinesPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        internal class XamlBindingSetters
        {
            public static void Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(global::Windows.UI.Xaml.Controls.ItemsControl obj, global::System.Object value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = (global::System.Object) global::Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(global::System.Object), targetNullValue);
                }
                obj.ItemsSource = value;
            }
            public static void Set_Windows_UI_Xaml_Controls_TextBlock_Text(global::Windows.UI.Xaml.Controls.TextBlock obj, global::System.String value, string targetNullValue)
            {
                if (value == null && targetNullValue != null)
                {
                    value = targetNullValue;
                }
                obj.Text = value ?? global::System.String.Empty;
            }
        };

        private class MedicinesPage_obj8_Bindings :
            global::Windows.UI.Xaml.IDataTemplateExtension,
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMedicinesPage_Bindings
        {
            private global::TPT_MMAS.Shared.Model.MedicineInventory dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);
            private global::Windows.UI.Xaml.ResourceDictionary localResources;
            private global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement> converterLookupRoot;
            private bool removedDataContextHandler = false;

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.TextBlock obj9;
            private global::Windows.UI.Xaml.Controls.TextBlock obj10;
            private global::Windows.UI.Xaml.Controls.TextBlock obj11;
            private global::Windows.UI.Xaml.Controls.TextBlock obj12;
            private global::Windows.UI.Xaml.Controls.TextBlock obj13;

            private MedicinesPage_obj8_BindingsTracking bindingsTracking;

            public MedicinesPage_obj8_Bindings()
            {
                this.bindingsTracking = new MedicinesPage_obj8_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 9:
                        this.obj9 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 10:
                        this.obj10 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 11:
                        this.obj11 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 12:
                        this.obj12 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    case 13:
                        this.obj13 = (global::Windows.UI.Xaml.Controls.TextBlock)target;
                        break;
                    default:
                        break;
                }
            }

            public void DataContextChangedHandler(global::Windows.UI.Xaml.FrameworkElement sender, global::Windows.UI.Xaml.DataContextChangedEventArgs args)
            {
                 global::TPT_MMAS.Shared.Model.MedicineInventory data = args.NewValue as global::TPT_MMAS.Shared.Model.MedicineInventory;
                 if (args.NewValue != null && data == null)
                 {
                    throw new global::System.ArgumentException("Incorrect type passed into template. Based on the x:DataType global::TPT_MMAS.Shared.Model.MedicineInventory was expected.");
                 }
                 this.SetDataRoot(data);
                 this.Update();
            }

            // IDataTemplateExtension

            public bool ProcessBinding(uint phase)
            {
                throw new global::System.NotImplementedException();
            }

            public int ProcessBindings(global::Windows.UI.Xaml.Controls.ContainerContentChangingEventArgs args)
            {
                int nextPhase = -1;
                switch(args.Phase)
                {
                    case 0:
                        nextPhase = -1;
                        this.SetDataRoot(args.Item as global::TPT_MMAS.Shared.Model.MedicineInventory);
                        if (!removedDataContextHandler)
                        {
                            removedDataContextHandler = true;
                            ((global::Windows.UI.Xaml.Controls.Grid)args.ItemContainer.ContentTemplateRoot).DataContextChanged -= this.DataContextChangedHandler;
                        }
                        this.initialized = true;
                        break;
                }
                this.Update_((global::TPT_MMAS.Shared.Model.MedicineInventory) args.Item, 1 << (int)args.Phase);
                return nextPhase;
            }

            public void ResetTemplate()
            {
                this.bindingsTracking.ReleaseAllListeners();
            }

            // IMedicinesPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // MedicinesPage_obj8_Bindings

            public void SetDataRoot(global::TPT_MMAS.Shared.Model.MedicineInventory newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }
            public void SetConverterLookupRoot(global::Windows.UI.Xaml.FrameworkElement rootElement)
            {
                this.converterLookupRoot = new global::System.WeakReference<global::Windows.UI.Xaml.FrameworkElement>(rootElement);
            }

            public global::Windows.UI.Xaml.Data.IValueConverter LookupConverter(string key)
            {
                if (this.localResources == null)
                {
                    global::Windows.UI.Xaml.FrameworkElement rootElement;
                    this.converterLookupRoot.TryGetTarget(out rootElement);
                    this.localResources = rootElement.Resources;
                    this.converterLookupRoot = null;
                }
                return (global::Windows.UI.Xaml.Data.IValueConverter) (this.localResources.ContainsKey(key) ? this.localResources[key] : global::Windows.UI.Xaml.Application.Current.Resources[key]);
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::TPT_MMAS.Shared.Model.MedicineInventory obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_Dosage(obj.Dosage, phase);
                    }
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_StocksLeft(obj.StocksLeft, phase);
                        this.Update_TimeLastAdded(obj.TimeLastAdded, phase);
                    }
                    if ((phase & (NOT_PHASED | (1 << 0))) != 0)
                    {
                        this.Update_GenericName(obj.GenericName, phase);
                        this.Update_BrandName(obj.BrandName, phase);
                    }
                }
            }
            private void Update_Dosage(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj9, obj, null);
                }
            }
            private void Update_StocksLeft(global::System.Int32 obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj10, obj.ToString(), null);
                }
            }
            private void Update_TimeLastAdded(global::System.DateTime obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj11, (global::System.String)this.LookupConverter("DateTimeConverter").Convert(obj, typeof(global::System.String), "MMMM dd, yyyy h:mm tt", null), null);
                }
            }
            private void Update_GenericName(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj12, obj, null);
                }
            }
            private void Update_BrandName(global::System.String obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED )) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_TextBlock_Text(this.obj13, obj, null);
                }
            }

            private class MedicinesPage_obj8_BindingsTracking
            {
                global::System.WeakReference<MedicinesPage_obj8_Bindings> WeakRefToBindingObj; 

                public MedicinesPage_obj8_BindingsTracking(MedicinesPage_obj8_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<MedicinesPage_obj8_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                }

            }
        }

        private class MedicinesPage_obj1_Bindings :
            global::Windows.UI.Xaml.Markup.IComponentConnector,
            IMedicinesPage_Bindings
        {
            private global::TPT_MMAS.View.MedicinesPage dataRoot;
            private bool initialized = false;
            private const int NOT_PHASED = (1 << 31);
            private const int DATA_CHANGED = (1 << 30);

            // Fields for each control that has bindings.
            private global::Windows.UI.Xaml.Controls.ListView obj3;

            private MedicinesPage_obj1_BindingsTracking bindingsTracking;

            public MedicinesPage_obj1_Bindings()
            {
                this.bindingsTracking = new MedicinesPage_obj1_BindingsTracking(this);
            }

            // IComponentConnector

            public void Connect(int connectionId, global::System.Object target)
            {
                switch(connectionId)
                {
                    case 3:
                        this.obj3 = (global::Windows.UI.Xaml.Controls.ListView)target;
                        break;
                    default:
                        break;
                }
            }

            // IMedicinesPage_Bindings

            public void Initialize()
            {
                if (!this.initialized)
                {
                    this.Update();
                }
            }
            
            public void Update()
            {
                this.Update_(this.dataRoot, NOT_PHASED);
                this.initialized = true;
            }

            public void StopTracking()
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.initialized = false;
            }

            // MedicinesPage_obj1_Bindings

            public void SetDataRoot(global::TPT_MMAS.View.MedicinesPage newDataRoot)
            {
                this.bindingsTracking.ReleaseAllListeners();
                this.dataRoot = newDataRoot;
            }

            public void Loading(global::Windows.UI.Xaml.FrameworkElement src, object data)
            {
                this.Initialize();
            }

            // Update methods for each path node used in binding steps.
            private void Update_(global::TPT_MMAS.View.MedicinesPage obj, int phase)
            {
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_VM(obj.VM, phase);
                    }
                }
            }
            private void Update_VM(global::TPT_MMAS.ViewModel.MedicinesViewModel obj, int phase)
            {
                this.bindingsTracking.UpdateChildListeners_VM(obj);
                if (obj != null)
                {
                    if ((phase & (NOT_PHASED | DATA_CHANGED | (1 << 0))) != 0)
                    {
                        this.Update_VM_MedicineInventories(obj.MedicineInventories, phase);
                    }
                }
            }
            private void Update_VM_MedicineInventories(global::System.Collections.ObjectModel.ObservableCollection<global::TPT_MMAS.Shared.Model.MedicineInventory> obj, int phase)
            {
                if((phase & ((1 << 0) | NOT_PHASED | DATA_CHANGED)) != 0)
                {
                    XamlBindingSetters.Set_Windows_UI_Xaml_Controls_ItemsControl_ItemsSource(this.obj3, obj, null);
                }
            }

            private class MedicinesPage_obj1_BindingsTracking
            {
                global::System.WeakReference<MedicinesPage_obj1_Bindings> WeakRefToBindingObj; 

                public MedicinesPage_obj1_BindingsTracking(MedicinesPage_obj1_Bindings obj)
                {
                    WeakRefToBindingObj = new global::System.WeakReference<MedicinesPage_obj1_Bindings>(obj);
                }

                public void ReleaseAllListeners()
                {
                    UpdateChildListeners_VM(null);
                }

                public void PropertyChanged_VM(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MedicinesPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::TPT_MMAS.ViewModel.MedicinesViewModel obj = sender as global::TPT_MMAS.ViewModel.MedicinesViewModel;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                            if (obj != null)
                            {
                                    bindings.Update_VM_MedicineInventories(obj.MedicineInventories, DATA_CHANGED);
                            }
                        }
                        else
                        {
                            switch (propName)
                            {
                                case "MedicineInventories":
                                {
                                    if (obj != null)
                                    {
                                        bindings.Update_VM_MedicineInventories(obj.MedicineInventories, DATA_CHANGED);
                                    }
                                    break;
                                }
                                default:
                                    break;
                            }
                        }
                    }
                }
                private global::TPT_MMAS.ViewModel.MedicinesViewModel cache_VM = null;
                public void UpdateChildListeners_VM(global::TPT_MMAS.ViewModel.MedicinesViewModel obj)
                {
                    if (obj != cache_VM)
                    {
                        if (cache_VM != null)
                        {
                            ((global::System.ComponentModel.INotifyPropertyChanged)cache_VM).PropertyChanged -= PropertyChanged_VM;
                            cache_VM = null;
                        }
                        if (obj != null)
                        {
                            cache_VM = obj;
                            ((global::System.ComponentModel.INotifyPropertyChanged)obj).PropertyChanged += PropertyChanged_VM;
                        }
                    }
                }
                public void PropertyChanged_VM_MedicineInventories(object sender, global::System.ComponentModel.PropertyChangedEventArgs e)
                {
                    MedicinesPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        string propName = e.PropertyName;
                        global::System.Collections.ObjectModel.ObservableCollection<global::TPT_MMAS.Shared.Model.MedicineInventory> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::TPT_MMAS.Shared.Model.MedicineInventory>;
                        if (global::System.String.IsNullOrEmpty(propName))
                        {
                        }
                        else
                        {
                            switch (propName)
                            {
                                default:
                                    break;
                            }
                        }
                    }
                }
                public void CollectionChanged_VM_MedicineInventories(object sender, global::System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
                {
                    MedicinesPage_obj1_Bindings bindings;
                    if(WeakRefToBindingObj.TryGetTarget(out bindings))
                    {
                        global::System.Collections.ObjectModel.ObservableCollection<global::TPT_MMAS.Shared.Model.MedicineInventory> obj = sender as global::System.Collections.ObjectModel.ObservableCollection<global::TPT_MMAS.Shared.Model.MedicineInventory>;
                    }
                }
            }
        }
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2:
                {
                    this.grid_tableHeader = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 4:
                {
                    this.fl_medInventory = (global::Windows.UI.Xaml.Controls.Flyout)(target);
                    #line 56 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Flyout)this.fl_medInventory).Closed += this.OnStockFlyoutClosed;
                    #line 56 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Flyout)this.fl_medInventory).Opened += this.OnStockFlyoutOpened;
                    #line default
                }
                break;
            case 5:
                {
                    this.btn_flyoutAdd = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 58 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_flyoutAdd).Click += this.OnStockFlyoutAddButtonClick;
                    #line default
                }
                break;
            case 6:
                {
                    this.tb_flyoutStocks = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 59 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.tb_flyoutStocks).TextChanged += this.OnStocksTextBoxChanged;
                    #line default
                }
                break;
            case 7:
                {
                    this.btn_flyoutMinus = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 60 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_flyoutMinus).Click += this.OnStockButtonFlyoutMinusButtonClick;
                    #line default
                }
                break;
            case 8:
                {
                    global::Windows.UI.Xaml.Controls.Grid element8 = (global::Windows.UI.Xaml.Controls.Grid)(target);
                    #line 73 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Grid)element8).RightTapped += this.OnInventoryRightTapped;
                    #line default
                }
                break;
            case 14:
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element14 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    #line 21 "..\..\..\View\MedicinesPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element14).Click += this.OnAddMedicineClickAsync;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)target;
                    MedicinesPage_obj1_Bindings bindings = new MedicinesPage_obj1_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot(this);
                    this.Bindings = bindings;
                    element1.Loading += bindings.Loading;
                }
                break;
            case 8:
                {
                    global::Windows.UI.Xaml.Controls.Grid element8 = (global::Windows.UI.Xaml.Controls.Grid)target;
                    MedicinesPage_obj8_Bindings bindings = new MedicinesPage_obj8_Bindings();
                    returnValue = bindings;
                    bindings.SetDataRoot((global::TPT_MMAS.Shared.Model.MedicineInventory) element8.DataContext);
                    bindings.SetConverterLookupRoot(this);
                    element8.DataContextChanged += bindings.DataContextChangedHandler;
                    global::Windows.UI.Xaml.DataTemplate.SetExtensionInstance(element8, bindings);
                }
                break;
            }
            return returnValue;
        }
    }
}

