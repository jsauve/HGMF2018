using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace HGMF2018.Core
{
    public static class Settings
    {
        static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        const string UBER_CLIENT_ID = "UBER_CLIENT_ID";
        static readonly string UBER_CLIENT_ID_Default = "UBER_CLIENT_ID_VALUE";

        const string LYFT_CLIENT_ID = "LYFT_CLIENT_ID";
        static readonly string LYFT_CLIENT_ID_Default = "LYFT_CLIENT_ID_VALUE";

        const string APP_CENTER_IOS_KEY = "APP_CENTER_IOS_KEY";
        static readonly string APP_CENTER_IOS_KEY_Default = "APP_CENTER_IOS_KEY_VALUE";

        const string APP_CENTER_ANDROID_KEY = "APP_CENTER_ANDROID_KEY";
        static readonly string APP_CENTER_ANDROID_KEY_Default = "APP_CENTER_ANDROID_KEY_VALUE";

        const string PYZE_IOS_KEY = "PYZE_IOS_KEY";
        static readonly string PYZE_IOS_KEY_Default = "PYZE_IOS_KEY_VALUE";

        const string PYZE_ANDROID_KEY = "PYZE_ANDROID_KEY";
        static readonly string PYZE_ANDROID_KEY_Default = "PYZE_ANDROID_KEYVALUE";

        const string AZURE_FUNCTION_ROOTURL_API_KEY = "AZURE_FUNCTION_ROOTURL_API_KEY";
        static readonly string AZURE_FUNCTION_ROOTURL_API_KEY_Default = "AZURE_FUNCTION_ROOTURL_API_KEY_VALUE";

        const string AZURE_FUNCTION_NAVBARTEXT_API_KEY = "AZURE_FUNCTION_NAVBARTEXT_API_KEY";
        static readonly string AZURE_FUNCTION_NAVBARTEXT_API_KEY_Default = "AZURE_FUNCTION_NAVBARTEXT_API_KEY_VALUE";

        const string AZURE_FUNCTION_BANNEDDOMAINS_API_KEY = "AZURE_FUNCTION_BANNEDDOMAINS_API_KEY";
        static readonly string AZURE_FUNCTION_BANNEDDOMAINS_API_KEY_Default = "AZURE_FUNCTION_BANNEDDOMAINS_API_KEY_VALUE";

        const string AZURE_FUNCTION_IOSVERSION_API_KEY = "AZURE_FUNCTION_IOSVERSION_API_KEY";
        static readonly string AZURE_FUNCTION_IOSVERSION_API_KEY_Default = "AZURE_FUNCTION_IOSVERSION_API_KEY_VALUE";

        const string AZURE_FUNCTION_ANDROIDVERSION_API_KEY = "AZURE_FUNCTION_ANDROIDVERSION_API_KEY";
        static readonly string AZURE_FUNCTION_ANDROIDVERSION_API_KEY_Default = "AZURE_FUNCTION_ANDROIDVERSION_API_KEY_VALUE";

        const string AZURE_NOTIFICATION_HUB_NAME_SANDBOX = "Azure_Notification_Hub_Name_Sandbox";
        static readonly string AZURE_NOTIFICATION_HUB_NAME_SANDBOX_Default = "AZURE_NOTIFICATION_HUB_NAME_SANDBOX_VALUE";

        const string AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_SANDBOX = "Azure_Notification_HubConnectionString_Sandbox";
        static readonly string AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_SANDBOX_Default = "AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_SANDBOX_VALUE";

        const string AZURE_NOTIFICATION_HUB_NAME_PROD = "Azure_Notification_Hub_Name_Prod";
        static readonly string AZURE_NOTIFICATION_HUB_NAME_PROD_Default = "AZURE_NOTIFICATION_HUB_NAME_PROD_VALUE";

        const string AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_PROD = "Azure_Notification_Hub_ConnectionString_Prod";
        static readonly string AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_PROD_Default = "AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_PROD_VALUE";


        #endregion


        public static string UberClientId
        {
            get => AppSettings.GetValueOrDefault(UBER_CLIENT_ID, UBER_CLIENT_ID_Default);
            //set => AppSettings.AddOrUpdateValue(UBER_CLIENT_ID, value);
        }

        public static string LyftClientId
        {
            get => AppSettings.GetValueOrDefault(LYFT_CLIENT_ID, LYFT_CLIENT_ID_Default);
            //set => AppSettings.AddOrUpdateValue(LYFT_CLIENT_ID, value);
        }

        public static string AppCenteriOSKey
        {
            get => AppSettings.GetValueOrDefault(APP_CENTER_IOS_KEY, APP_CENTER_IOS_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(UBER_CLIENT_ID, value);
        }

        public static string AppCenterAndroidKey
        {
            get => AppSettings.GetValueOrDefault(APP_CENTER_ANDROID_KEY, APP_CENTER_ANDROID_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(LYFT_CLIENT_ID, value);
        }

        public static string PyzeiOSKey
        {
            get => AppSettings.GetValueOrDefault(PYZE_IOS_KEY, PYZE_IOS_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(UBER_CLIENT_ID, value);
        }

        public static string PyzeAndroidKey
        {
            get => AppSettings.GetValueOrDefault(PYZE_ANDROID_KEY, PYZE_ANDROID_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(LYFT_CLIENT_ID, value);
        }

        public static string AzureFunctionRootUrlApiKey
        {
            get => AppSettings.GetValueOrDefault(AZURE_FUNCTION_ROOTURL_API_KEY, AZURE_FUNCTION_ROOTURL_API_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(AZURE_FUNCTION_IOSVERSION_API_KEY, value);
        }

        public static string AzureFunctionNavBarTextApiKey
        {
            get => AppSettings.GetValueOrDefault(AZURE_FUNCTION_NAVBARTEXT_API_KEY, AZURE_FUNCTION_NAVBARTEXT_API_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(AZURE_FUNCTION_IOSVERSION_API_KEY, value);
        }

        public static string AzureFunctionBannedDomainsApiKey
        {
            get => AppSettings.GetValueOrDefault(AZURE_FUNCTION_BANNEDDOMAINS_API_KEY, AZURE_FUNCTION_BANNEDDOMAINS_API_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(AZURE_FUNCTION_IOSVERSION_API_KEY, value);
        }

        public static string AzureFunctioniOSVersionApiKey
        {
            get => AppSettings.GetValueOrDefault(AZURE_FUNCTION_IOSVERSION_API_KEY, AZURE_FUNCTION_IOSVERSION_API_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(AZURE_FUNCTION_IOSVERSION_API_KEY, value);
        }

        public static string AzureFunctionAndroidVersionApiKey
        {
            get => AppSettings.GetValueOrDefault(AZURE_FUNCTION_ANDROIDVERSION_API_KEY, AZURE_FUNCTION_ANDROIDVERSION_API_KEY_Default);
            //set => AppSettings.AddOrUpdateValue(AZURE_FUNCTION_ANDROIDVERSION_API_KEY, value);
        }

        public static string AzureNotificationHubNameSandbox
        {
            get => AppSettings.GetValueOrDefault(AZURE_NOTIFICATION_HUB_NAME_SANDBOX, AZURE_NOTIFICATION_HUB_NAME_SANDBOX_Default);
            //set => AppSettings.AddOrUpdateValue(Azure_Notification_Hub_Name_Sandbox, value);
        }

        public static string AzureNotificationHubConnectionStringSandbox
        {
            get => AppSettings.GetValueOrDefault(AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_SANDBOX, AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_SANDBOX_Default);
            //set => AppSettings.AddOrUpdateValue(Azure_Notification_HubConnectionString_Sandbox, value);
        }

        public static string AzureNotificationHubNameProd
        {
            get => AppSettings.GetValueOrDefault(AZURE_NOTIFICATION_HUB_NAME_PROD, AZURE_NOTIFICATION_HUB_NAME_PROD_Default);
            //set => AppSettings.AddOrUpdateValue(Azure_Notification_Hub_Name_Prod, value);
        }

        public static string AzureNotificationHubConnectionStringProd
        {
            get => AppSettings.GetValueOrDefault(AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_PROD, AZURE_NOTIFICATION_HUB_CONNECTIONSTRING_PROD_Default);
            //set => AppSettings.AddOrUpdateValue(Azure_Notification_Hub_ConnectionString_Prod, value);
        }
    }
}
