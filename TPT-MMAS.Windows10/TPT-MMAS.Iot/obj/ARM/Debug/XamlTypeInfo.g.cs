﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace TPT_MMAS.Iot
{
    public partial class App : global::Windows.UI.Xaml.Markup.IXamlMetadataProvider
    {
    private global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider _provider;

        /// <summary>
        /// GetXamlType(Type)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(global::System.Type type)
        {
            if(_provider == null)
            {
                _provider = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByType(type);
        }

        /// <summary>
        /// GetXamlType(String)
        /// </summary>
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlType(string fullName)
        {
            if(_provider == null)
            {
                _provider = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider();
            }
            return _provider.GetXamlTypeByName(fullName);
        }

        /// <summary>
        /// GetXmlnsDefinitions()
        /// </summary>
        public global::Windows.UI.Xaml.Markup.XmlnsDefinition[] GetXmlnsDefinitions()
        {
            return new global::Windows.UI.Xaml.Markup.XmlnsDefinition[0];
        }
    }
}

namespace TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal partial class XamlTypeInfoProvider
    {
        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByType.TryGetValue(type, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByType(type);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            var userXamlType = xamlType as global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType;
            if(xamlType == null || (userXamlType != null && userXamlType.IsReturnTypeStub && !userXamlType.IsLocalType))
            {
                global::Windows.UI.Xaml.Markup.IXamlType libXamlType = CheckOtherMetadataProvidersForType(type);
                if (libXamlType != null)
                {
                    if(libXamlType.IsConstructible || xamlType == null)
                    {
                        xamlType = libXamlType;
                    }
                }
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlType GetXamlTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlType xamlType;
            if (_xamlTypeCacheByName.TryGetValue(typeName, out xamlType))
            {
                return xamlType;
            }
            int typeIndex = LookupTypeIndexByName(typeName);
            if(typeIndex != -1)
            {
                xamlType = CreateXamlType(typeIndex);
            }
            var userXamlType = xamlType as global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType;
            if(xamlType == null || (userXamlType != null && userXamlType.IsReturnTypeStub && !userXamlType.IsLocalType))
            {
                global::Windows.UI.Xaml.Markup.IXamlType libXamlType = CheckOtherMetadataProvidersForName(typeName);
                if (libXamlType != null)
                {
                    if(libXamlType.IsConstructible || xamlType == null)
                    {
                        xamlType = libXamlType;
                    }
                }
            }
            if (xamlType != null)
            {
                _xamlTypeCacheByName.Add(xamlType.FullName, xamlType);
                _xamlTypeCacheByType.Add(xamlType.UnderlyingType, xamlType);
            }
            return xamlType;
        }

        public global::Windows.UI.Xaml.Markup.IXamlMember GetMemberByLongName(string longMemberName)
        {
            if (string.IsNullOrEmpty(longMemberName))
            {
                return null;
            }
            global::Windows.UI.Xaml.Markup.IXamlMember xamlMember;
            if (_xamlMembers.TryGetValue(longMemberName, out xamlMember))
            {
                return xamlMember;
            }
            xamlMember = CreateXamlMember(longMemberName);
            if (xamlMember != null)
            {
                _xamlMembers.Add(longMemberName, xamlMember);
            }
            return xamlMember;
        }

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByName = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>
                _xamlTypeCacheByType = new global::System.Collections.Generic.Dictionary<global::System.Type, global::Windows.UI.Xaml.Markup.IXamlType>();

        global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>
                _xamlMembers = new global::System.Collections.Generic.Dictionary<string, global::Windows.UI.Xaml.Markup.IXamlMember>();

        string[] _typeNameTable = null;
        global::System.Type[] _typeTable = null;

        private void InitTypeTables()
        {
            _typeNameTable = new string[15];
            _typeNameTable[0] = "TPT_MMAS.Shared.Converter.NullToVisibilityConverter";
            _typeNameTable[1] = "Object";
            _typeNameTable[2] = "TPT_MMAS.Shared.Converter.BooleanToVisibilityConverter";
            _typeNameTable[3] = "TPT_MMAS.Shared.Converter.DateTimeConverter";
            _typeNameTable[4] = "TPT_MMAS.Iot.ViewModel.ViewModelLocator";
            _typeNameTable[5] = "TPT_MMAS.Iot.ViewModel.MainViewModel";
            _typeNameTable[6] = "GalaSoft.MvvmLight.ViewModelBase";
            _typeNameTable[7] = "GalaSoft.MvvmLight.ObservableObject";
            _typeNameTable[8] = "TPT_MMAS.Iot.ViewModel.ShellViewModel";
            _typeNameTable[9] = "TPT_MMAS.Iot.ViewModel.DebuggingViewModel";
            _typeNameTable[10] = "TPT_MMAS.Iot.Shell";
            _typeNameTable[11] = "Windows.UI.Xaml.Controls.Page";
            _typeNameTable[12] = "Windows.UI.Xaml.Controls.UserControl";
            _typeNameTable[13] = "TPT_MMAS.Iot.Views.DebuggingPage";
            _typeNameTable[14] = "TPT_MMAS.Iot.Views.MainPage";

            _typeTable = new global::System.Type[15];
            _typeTable[0] = typeof(global::TPT_MMAS.Shared.Converter.NullToVisibilityConverter);
            _typeTable[1] = typeof(global::System.Object);
            _typeTable[2] = typeof(global::TPT_MMAS.Shared.Converter.BooleanToVisibilityConverter);
            _typeTable[3] = typeof(global::TPT_MMAS.Shared.Converter.DateTimeConverter);
            _typeTable[4] = typeof(global::TPT_MMAS.Iot.ViewModel.ViewModelLocator);
            _typeTable[5] = typeof(global::TPT_MMAS.Iot.ViewModel.MainViewModel);
            _typeTable[6] = typeof(global::GalaSoft.MvvmLight.ViewModelBase);
            _typeTable[7] = typeof(global::GalaSoft.MvvmLight.ObservableObject);
            _typeTable[8] = typeof(global::TPT_MMAS.Iot.ViewModel.ShellViewModel);
            _typeTable[9] = typeof(global::TPT_MMAS.Iot.ViewModel.DebuggingViewModel);
            _typeTable[10] = typeof(global::TPT_MMAS.Iot.Shell);
            _typeTable[11] = typeof(global::Windows.UI.Xaml.Controls.Page);
            _typeTable[12] = typeof(global::Windows.UI.Xaml.Controls.UserControl);
            _typeTable[13] = typeof(global::TPT_MMAS.Iot.Views.DebuggingPage);
            _typeTable[14] = typeof(global::TPT_MMAS.Iot.Views.MainPage);
        }

        private int LookupTypeIndexByName(string typeName)
        {
            if (_typeNameTable == null)
            {
                InitTypeTables();
            }
            for (int i=0; i<_typeNameTable.Length; i++)
            {
                if(0 == string.CompareOrdinal(_typeNameTable[i], typeName))
                {
                    return i;
                }
            }
            return -1;
        }

        private int LookupTypeIndexByType(global::System.Type type)
        {
            if (_typeTable == null)
            {
                InitTypeTables();
            }
            for(int i=0; i<_typeTable.Length; i++)
            {
                if(type == _typeTable[i])
                {
                    return i;
                }
            }
            return -1;
        }

        private object Activate_0_NullToVisibilityConverter() { return new global::TPT_MMAS.Shared.Converter.NullToVisibilityConverter(); }
        private object Activate_2_BooleanToVisibilityConverter() { return new global::TPT_MMAS.Shared.Converter.BooleanToVisibilityConverter(); }
        private object Activate_3_DateTimeConverter() { return new global::TPT_MMAS.Shared.Converter.DateTimeConverter(); }
        private object Activate_4_ViewModelLocator() { return new global::TPT_MMAS.Iot.ViewModel.ViewModelLocator(); }
        private object Activate_5_MainViewModel() { return new global::TPT_MMAS.Iot.ViewModel.MainViewModel(); }
        private object Activate_7_ObservableObject() { return new global::GalaSoft.MvvmLight.ObservableObject(); }
        private object Activate_8_ShellViewModel() { return new global::TPT_MMAS.Iot.ViewModel.ShellViewModel(); }
        private object Activate_9_DebuggingViewModel() { return new global::TPT_MMAS.Iot.ViewModel.DebuggingViewModel(); }
        private object Activate_13_DebuggingPage() { return new global::TPT_MMAS.Iot.Views.DebuggingPage(); }
        private object Activate_14_MainPage() { return new global::TPT_MMAS.Iot.Views.MainPage(); }

        private global::Windows.UI.Xaml.Markup.IXamlType CreateXamlType(int typeIndex)
        {
            global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlSystemBaseType xamlType = null;
            global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType userType;
            string typeName = _typeNameTable[typeIndex];
            global::System.Type type = _typeTable[typeIndex];

            switch (typeIndex)
            {

            case 0:   //  TPT_MMAS.Shared.Converter.NullToVisibilityConverter
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_0_NullToVisibilityConverter;
                xamlType = userType;
                break;

            case 1:   //  Object
                xamlType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 2:   //  TPT_MMAS.Shared.Converter.BooleanToVisibilityConverter
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_2_BooleanToVisibilityConverter;
                xamlType = userType;
                break;

            case 3:   //  TPT_MMAS.Shared.Converter.DateTimeConverter
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_3_DateTimeConverter;
                xamlType = userType;
                break;

            case 4:   //  TPT_MMAS.Iot.ViewModel.ViewModelLocator
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_4_ViewModelLocator;
                userType.AddMemberName("Main");
                userType.AddMemberName("Shell");
                userType.AddMemberName("Debugging");
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 5:   //  TPT_MMAS.Iot.ViewModel.MainViewModel
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GalaSoft.MvvmLight.ViewModelBase"));
                userType.SetIsReturnTypeStub();
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 6:   //  GalaSoft.MvvmLight.ViewModelBase
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GalaSoft.MvvmLight.ObservableObject"));
                xamlType = userType;
                break;

            case 7:   //  GalaSoft.MvvmLight.ObservableObject
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Object"));
                userType.Activator = Activate_7_ObservableObject;
                xamlType = userType;
                break;

            case 8:   //  TPT_MMAS.Iot.ViewModel.ShellViewModel
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GalaSoft.MvvmLight.ViewModelBase"));
                userType.SetIsReturnTypeStub();
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 9:   //  TPT_MMAS.Iot.ViewModel.DebuggingViewModel
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("GalaSoft.MvvmLight.ViewModelBase"));
                userType.SetIsReturnTypeStub();
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 10:   //  TPT_MMAS.Iot.Shell
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 11:   //  Windows.UI.Xaml.Controls.Page
                xamlType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 12:   //  Windows.UI.Xaml.Controls.UserControl
                xamlType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlSystemBaseType(typeName, type);
                break;

            case 13:   //  TPT_MMAS.Iot.Views.DebuggingPage
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_13_DebuggingPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;

            case 14:   //  TPT_MMAS.Iot.Views.MainPage
                userType = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType(this, typeName, type, GetXamlTypeByName("Windows.UI.Xaml.Controls.Page"));
                userType.Activator = Activate_14_MainPage;
                userType.SetIsLocalType();
                xamlType = userType;
                break;
            }
            return xamlType;
        }

        private global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider> _otherProviders;
        private global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider> OtherProviders
        {
            get
            {
                if(_otherProviders == null)
                {
                    var otherProviders = new global::System.Collections.Generic.List<global::Windows.UI.Xaml.Markup.IXamlMetadataProvider>();
                    global::Windows.UI.Xaml.Markup.IXamlMetadataProvider provider;
                    provider = new global::TPT_MMAS.Shared.TPT_MMAS_Shared_XamlTypeInfo.XamlMetaDataProvider() as global::Windows.UI.Xaml.Markup.IXamlMetadataProvider;
                    otherProviders.Add(provider); 
                    _otherProviders = otherProviders;
                }
                return _otherProviders;
            }
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CheckOtherMetadataProvidersForName(string typeName)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            global::Windows.UI.Xaml.Markup.IXamlType foundXamlType = null;
            foreach(global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xmp in OtherProviders)
            {
                xamlType = xmp.GetXamlType(typeName);
                if(xamlType != null)
                {
                    if(xamlType.IsConstructible)    // not Constructible means it might be a Return Type Stub
                    {
                        return xamlType;
                    }
                    foundXamlType = xamlType;
                }
            }
            return foundXamlType;
        }

        private global::Windows.UI.Xaml.Markup.IXamlType CheckOtherMetadataProvidersForType(global::System.Type type)
        {
            global::Windows.UI.Xaml.Markup.IXamlType xamlType = null;
            global::Windows.UI.Xaml.Markup.IXamlType foundXamlType = null;
            foreach(global::Windows.UI.Xaml.Markup.IXamlMetadataProvider xmp in OtherProviders)
            {
                xamlType = xmp.GetXamlType(type);
                if(xamlType != null)
                {
                    if(xamlType.IsConstructible)    // not Constructible means it might be a Return Type Stub
                    {
                        return xamlType;
                    }
                    foundXamlType = xamlType;
                }
            }
            return foundXamlType;
        }

        private object get_0_ViewModelLocator_Main(object instance)
        {
            var that = (global::TPT_MMAS.Iot.ViewModel.ViewModelLocator)instance;
            return that.Main;
        }
        private object get_1_ViewModelLocator_Shell(object instance)
        {
            var that = (global::TPT_MMAS.Iot.ViewModel.ViewModelLocator)instance;
            return that.Shell;
        }
        private object get_2_ViewModelLocator_Debugging(object instance)
        {
            var that = (global::TPT_MMAS.Iot.ViewModel.ViewModelLocator)instance;
            return that.Debugging;
        }

        private global::Windows.UI.Xaml.Markup.IXamlMember CreateXamlMember(string longMemberName)
        {
            global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlMember xamlMember = null;
            global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType userType;

            switch (longMemberName)
            {
            case "TPT_MMAS.Iot.ViewModel.ViewModelLocator.Main":
                userType = (global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType)GetXamlTypeByName("TPT_MMAS.Iot.ViewModel.ViewModelLocator");
                xamlMember = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlMember(this, "Main", "TPT_MMAS.Iot.ViewModel.MainViewModel");
                xamlMember.Getter = get_0_ViewModelLocator_Main;
                xamlMember.SetIsReadOnly();
                break;
            case "TPT_MMAS.Iot.ViewModel.ViewModelLocator.Shell":
                userType = (global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType)GetXamlTypeByName("TPT_MMAS.Iot.ViewModel.ViewModelLocator");
                xamlMember = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlMember(this, "Shell", "TPT_MMAS.Iot.ViewModel.ShellViewModel");
                xamlMember.Getter = get_1_ViewModelLocator_Shell;
                xamlMember.SetIsReadOnly();
                break;
            case "TPT_MMAS.Iot.ViewModel.ViewModelLocator.Debugging":
                userType = (global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlUserType)GetXamlTypeByName("TPT_MMAS.Iot.ViewModel.ViewModelLocator");
                xamlMember = new global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlMember(this, "Debugging", "TPT_MMAS.Iot.ViewModel.DebuggingViewModel");
                xamlMember.Getter = get_2_ViewModelLocator_Debugging;
                xamlMember.SetIsReadOnly();
                break;
            }
            return xamlMember;
        }
    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlSystemBaseType : global::Windows.UI.Xaml.Markup.IXamlType
    {
        string _fullName;
        global::System.Type _underlyingType;

        public XamlSystemBaseType(string fullName, global::System.Type underlyingType)
        {
            _fullName = fullName;
            _underlyingType = underlyingType;
        }

        public string FullName { get { return _fullName; } }

        public global::System.Type UnderlyingType
        {
            get
            {
                return _underlyingType;
            }
        }

        virtual public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name) { throw new global::System.NotImplementedException(); }
        virtual public bool IsArray { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsCollection { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsConstructible { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsDictionary { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsMarkupExtension { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsBindable { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsReturnTypeStub { get { throw new global::System.NotImplementedException(); } }
        virtual public bool IsLocalType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType ItemType { get { throw new global::System.NotImplementedException(); } }
        virtual public global::Windows.UI.Xaml.Markup.IXamlType KeyType { get { throw new global::System.NotImplementedException(); } }
        virtual public object ActivateInstance() { throw new global::System.NotImplementedException(); }
        virtual public void AddToMap(object instance, object key, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void AddToVector(object instance, object item)  { throw new global::System.NotImplementedException(); }
        virtual public void RunInitializer()   { throw new global::System.NotImplementedException(); }
        virtual public object CreateFromString(string input)   { throw new global::System.NotImplementedException(); }
    }
    
    internal delegate object Activator();
    internal delegate void AddToCollection(object instance, object item);
    internal delegate void AddToDictionary(object instance, object key, object item);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlUserType : global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlSystemBaseType
    {
        global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider _provider;
        global::Windows.UI.Xaml.Markup.IXamlType _baseType;
        bool _isArray;
        bool _isMarkupExtension;
        bool _isBindable;
        bool _isReturnTypeStub;
        bool _isLocalType;

        string _contentPropertyName;
        string _itemTypeName;
        string _keyTypeName;
        global::System.Collections.Generic.Dictionary<string, string> _memberNames;
        global::System.Collections.Generic.Dictionary<string, object> _enumValues;

        public XamlUserType(global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider provider, string fullName, global::System.Type fullType, global::Windows.UI.Xaml.Markup.IXamlType baseType)
            :base(fullName, fullType)
        {
            _provider = provider;
            _baseType = baseType;
        }

        // --- Interface methods ----

        override public global::Windows.UI.Xaml.Markup.IXamlType BaseType { get { return _baseType; } }
        override public bool IsArray { get { return _isArray; } }
        override public bool IsCollection { get { return (CollectionAdd != null); } }
        override public bool IsConstructible { get { return (Activator != null); } }
        override public bool IsDictionary { get { return (DictionaryAdd != null); } }
        override public bool IsMarkupExtension { get { return _isMarkupExtension; } }
        override public bool IsBindable { get { return _isBindable; } }
        override public bool IsReturnTypeStub { get { return _isReturnTypeStub; } }
        override public bool IsLocalType { get { return _isLocalType; } }

        override public global::Windows.UI.Xaml.Markup.IXamlMember ContentProperty
        {
            get { return _provider.GetMemberByLongName(_contentPropertyName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType ItemType
        {
            get { return _provider.GetXamlTypeByName(_itemTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlType KeyType
        {
            get { return _provider.GetXamlTypeByName(_keyTypeName); }
        }

        override public global::Windows.UI.Xaml.Markup.IXamlMember GetMember(string name)
        {
            if (_memberNames == null)
            {
                return null;
            }
            string longName;
            if (_memberNames.TryGetValue(name, out longName))
            {
                return _provider.GetMemberByLongName(longName);
            }
            return null;
        }

        override public object ActivateInstance()
        {
            return Activator(); 
        }

        override public void AddToMap(object instance, object key, object item) 
        {
            DictionaryAdd(instance, key, item);
        }

        override public void AddToVector(object instance, object item)
        {
            CollectionAdd(instance, item);
        }

        override public void RunInitializer() 
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(UnderlyingType.TypeHandle);
        }

        override public object CreateFromString(string input)
        {
            if (_enumValues != null)
            {
                int value = 0;

                string[] valueParts = input.Split(',');

                foreach (string valuePart in valueParts) 
                {
                    object partValue;
                    int enumFieldValue = 0;
                    try
                    {
                        if (_enumValues.TryGetValue(valuePart.Trim(), out partValue))
                        {
                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                        }
                        else
                        {
                            try
                            {
                                enumFieldValue = global::System.Convert.ToInt32(valuePart.Trim());
                            }
                            catch( global::System.FormatException )
                            {
                                foreach( string key in _enumValues.Keys )
                                {
                                    if( string.Compare(valuePart.Trim(), key, global::System.StringComparison.OrdinalIgnoreCase) == 0 )
                                    {
                                        if( _enumValues.TryGetValue(key.Trim(), out partValue) )
                                        {
                                            enumFieldValue = global::System.Convert.ToInt32(partValue);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        value |= enumFieldValue; 
                    }
                    catch( global::System.FormatException )
                    {
                        throw new global::System.ArgumentException(input, FullName);
                    }
                }

                return value; 
            }
            throw new global::System.ArgumentException(input, FullName);
        }

        // --- End of Interface methods

        public Activator Activator { get; set; }
        public AddToCollection CollectionAdd { get; set; }
        public AddToDictionary DictionaryAdd { get; set; }

        public void SetContentPropertyName(string contentPropertyName)
        {
            _contentPropertyName = contentPropertyName;
        }

        public void SetIsArray()
        {
            _isArray = true; 
        }

        public void SetIsMarkupExtension()
        {
            _isMarkupExtension = true;
        }

        public void SetIsBindable()
        {
            _isBindable = true;
        }

        public void SetIsReturnTypeStub()
        {
            _isReturnTypeStub = true;
        }

        public void SetIsLocalType()
        {
            _isLocalType = true;
        }

        public void SetItemTypeName(string itemTypeName)
        {
            _itemTypeName = itemTypeName;
        }

        public void SetKeyTypeName(string keyTypeName)
        {
            _keyTypeName = keyTypeName;
        }

        public void AddMemberName(string shortName)
        {
            if(_memberNames == null)
            {
                _memberNames =  new global::System.Collections.Generic.Dictionary<string,string>();
            }
            _memberNames.Add(shortName, FullName + "." + shortName);
        }

        public void AddEnumValue(string name, object value)
        {
            if (_enumValues == null)
            {
                _enumValues = new global::System.Collections.Generic.Dictionary<string, object>();
            }
            _enumValues.Add(name, value);
        }
    }

    internal delegate object Getter(object instance);
    internal delegate void Setter(object instance, object value);

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    internal class XamlMember : global::Windows.UI.Xaml.Markup.IXamlMember
    {
        global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider _provider;
        string _name;
        bool _isAttachable;
        bool _isDependencyProperty;
        bool _isReadOnly;

        string _typeName;
        string _targetTypeName;

        public XamlMember(global::TPT_MMAS.Iot.TPT_MMAS_Iot_XamlTypeInfo.XamlTypeInfoProvider provider, string name, string typeName)
        {
            _name = name;
            _typeName = typeName;
            _provider = provider;
        }

        public string Name { get { return _name; } }

        public global::Windows.UI.Xaml.Markup.IXamlType Type
        {
            get { return _provider.GetXamlTypeByName(_typeName); }
        }

        public void SetTargetTypeName(string targetTypeName)
        {
            _targetTypeName = targetTypeName;
        }
        public global::Windows.UI.Xaml.Markup.IXamlType TargetType
        {
            get { return _provider.GetXamlTypeByName(_targetTypeName); }
        }

        public void SetIsAttachable() { _isAttachable = true; }
        public bool IsAttachable { get { return _isAttachable; } }

        public void SetIsDependencyProperty() { _isDependencyProperty = true; }
        public bool IsDependencyProperty { get { return _isDependencyProperty; } }

        public void SetIsReadOnly() { _isReadOnly = true; }
        public bool IsReadOnly { get { return _isReadOnly; } }

        public Getter Getter { get; set; }
        public object GetValue(object instance)
        {
            if (Getter != null)
                return Getter(instance);
            else
                throw new global::System.InvalidOperationException("GetValue");
        }

        public Setter Setter { get; set; }
        public void SetValue(object instance, object value)
        {
            if (Setter != null)
                Setter(instance, value);
            else
                throw new global::System.InvalidOperationException("SetValue");
        }
    }
}

