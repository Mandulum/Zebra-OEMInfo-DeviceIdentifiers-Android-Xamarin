using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using DeviceIdentifierManager;
using Utilities;
using AndroidUri = Android.Net.Uri;

namespace Zebra_OEMInfo_DeviceIdentifiers_Android_Xamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, RetrieveOemInfo.OnOemInfoRetrievedListener, PermissionsHelper.OnPermissionsResultListener
    {
        // Debugging
        private static readonly string TAG = "MainActivity";

        // Permissions Callback Code
        private PermissionsHelper mPermissionsHelper;
        private static readonly int PERMISSION_ACTIVITY_RESULT_CODE = 100;

        // Content URIs
        private static readonly string SERIAL_URI = "content://oem_info/oem.zebra.secure/build_serial";
        private static readonly string IMEI_URI = "content://oem_info/wan/imei";
        private static readonly AndroidUri[] CONTENT_PROVIDER_URIS = {
                AndroidUri.Parse(SERIAL_URI),
                AndroidUri.Parse(IMEI_URI)
        };

        // Content Provider Keys
        private static readonly string IMEI = "imei";
        private static readonly string BUILD_SERIAL = "build_serial";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            // Grab Permissions
            mPermissionsHelper = new PermissionsHelper(this, this);
        }

        /**
         *
         * OEM CALL BACKS
         *
        */

        public void OnDetailsRetrieved(Dictionary<string, string> oemIdentifiers)
        {
            // Init Writer & Holders
            String imei = null;
            String buildSerial = null;

            // Grab Values
            foreach (string key in oemIdentifiers.Keys)
            {
                Android.Util.Log.Info(TAG, "OEM Info | " + key + oemIdentifiers[key]);
                if (key == IMEI)
                {
                    imei = oemIdentifiers[key];
                }
                else if (key == BUILD_SERIAL)
                {
                    buildSerial = oemIdentifiers[key];
                }
            }

            CustomDialog.ShowCustomDialog(this, CustomDialog.DialogType.SUCCESS,
                    "Identifiers Saved",
                    $"Serial: {buildSerial}\nImei: {imei}",
                    "OK", (dialogInterface, i) => Finish(),
                    null, null);
        }

        public void OnPermissionError(string e)
        {
            Android.Util.Log.Error(TAG, "Permissions Error: " + e);
            CustomDialog.ShowCustomDialog(this, CustomDialog.DialogType.WARN,
                    GetString(Resource.String.permissions_dialog_title),
                    GetString(Resource.String.permissions_dialog_message),
                    "OK", (dialogInterface, i) => RequestOemInfoPermissions(),
                    "EXIT", (dialogInterface, i) => Finish());
        }

        public void OnUnknownError(string e)
        {
            Android.Util.Log.Error(TAG, "Unknown Error: " + e);
            CustomDialog.ShowCustomDialog(this, CustomDialog.DialogType.ERROR,
                    GetString(Resource.String.unknown_error_dialog_title), e,
                    "OK", (dialogInterface, i) => Finish(),
                    null, null);
        }

        /**
         * Permissions Management
         */
        public void OnPermissionsGranted()
        {
            Android.Util.Log.Info(TAG, "Permissions Granted");

            // Attempt to get Serial & IMEI from Content Providers
            new RetrieveOemInfo(this, CONTENT_PROVIDER_URIS, this).Execute();
        }

        private void RequestOemInfoPermissions()
        {
            StartActivityForResult(new Intent(this, typeof(PermissionsActivity)), PERMISSION_ACTIVITY_RESULT_CODE);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PERMISSION_ACTIVITY_RESULT_CODE)
            {
                if (data != null)
                {
                    bool permissionsGranted = data.GetBooleanExtra(PermissionsActivity.PERMISSIONS_GRANTED_EXTRA, false);
                    if (permissionsGranted)
                    {
                        new RetrieveOemInfo(this, CONTENT_PROVIDER_URIS, this).Execute();
                    }
                    else
                    {
                        String statusCode = data.GetStringExtra(PermissionsActivity.PERMISSIONS_STATUS_CODE);
                        String extendedStatusCode = data.GetStringExtra(PermissionsActivity.PERMISSIONS_EXTENDED_STATUS_CODE);
                        CustomDialog.ShowCustomDialog(this, CustomDialog.DialogType.ERROR,
                                "Failed to Grant Permissions",
                                "Status Code: " + statusCode + " | \n\n Extended Status Code: " + extendedStatusCode,
                                "RETRY", (dialogInterface, i) => StartActivityForResult(new Intent(this, typeof(PermissionsActivity)), PERMISSION_ACTIVITY_RESULT_CODE),
                                "QUIT", (dialogInterface, i) => Finish());
                    }
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            mPermissionsHelper.OnRequestPermissionsResult();
        }
    }
}
