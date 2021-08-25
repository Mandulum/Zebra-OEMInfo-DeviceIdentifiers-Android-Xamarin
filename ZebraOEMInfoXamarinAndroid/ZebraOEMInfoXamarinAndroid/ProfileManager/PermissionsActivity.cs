using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using ProfileManager;
using Symbol.XamarinEMDK;
using Utilities;
using ZebraOEMInfoXamarinAndroid;

public class PermissionsActivity : AppCompatActivity, OnProfileApplied, EMDKManager.IEMDKListener
{

    // Debugging
    private static readonly string TAG = "PermissionsActivity";

    // Global Variables
    public static string PERMISSIONS_GRANTED_EXTRA = "permissions-granted-extra";
    public static string PERMISSIONS_STATUS_CODE = "permissions-status-code-extra";
    public static string PERMISSIONS_EXTENDED_STATUS_CODE = "permissions-extended-status-code-extra";

    // Private Variables
    private EMDKManager mEmdkManager = null;
    private Symbol.XamarinEMDK.ProfileManager mProfileManager = null;

    // UI
    private Android.App.AlertDialog mProgressDialog;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_permissions);

        // Init Progress Dialog
        mProgressDialog = CustomDialog.BuildLoadingDialog(this, "Applying MX XML to Grant Permissions...", false);
        mProgressDialog.Show();

        // Init EMDK
        EMDKResults emdkManagerResults = EMDKManager.GetEMDKManager(this, this);

        // Verify EMDK Manager
        if (emdkManagerResults == null || emdkManagerResults.StatusCode != EMDKResults.STATUS_CODE.Success)
        {
            // Log Error
            Android.Util.Log.Error(TAG, "onCreate: Failed to get EMDK Manager -> " + (emdkManagerResults == null ? "No Results Returned" : emdkManagerResults.StatusCode.ToString()));
            Toast.MakeText(this, "Failed to get EMDK Manager!", ToastLength.Long).Show();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // Release EMDK Manager Instance
        if (mEmdkManager != null)
        {
            mEmdkManager.Release();
            mEmdkManager = null;
        }

        // Remove Progress
        if (mProgressDialog != null && mProgressDialog.IsShowing)
        {
            mProgressDialog.Dismiss();
        }
    }

    /**
     * EMDK Methods
     */
    public void OnOpened(EMDKManager emdkManager)
    {
        // Assign EMDK Reference
        mEmdkManager = emdkManager;

        // Get Profile & Version Manager Instances
        mProfileManager = (Symbol.XamarinEMDK.ProfileManager)mEmdkManager.GetInstance(EMDKManager.FEATURE_TYPE.Profile);

        // Apply Profile
        if (mProfileManager != null)
        {
            try
            {
                // Init XML
                XML permissionXml = new XML(this);

                // Process
                new ProcessProfile(XML.GRANT_SERIAL_PERMISSION_NAME, mProfileManager, this).Execute(permissionXml.GetSerialPermissionXml());

                // Process
                new ProcessProfile(XML.GRANT_IMEI_PERMISSION_NAME, mProfileManager, this).Execute(permissionXml.GetImeiPermissionXml());

            }
            catch (Exception e)
            {
                Android.Util.Log.Error(TAG, e.StackTrace);
            }
        }
        else
        {
            Android.Util.Log.Error(TAG, "Error Obtaining ProfileManager!");
            Toast.MakeText(this, "Error Obtaining ProfileManager!", ToastLength.Long).Show();
        }
    }

    public void OnClosed()
    {
        // Release EMDK Manager Instance
        if (mEmdkManager != null)
        {
            mEmdkManager.Release();
            mEmdkManager = null;
        }
    }

    /**
     * Callback
     */

    // Holder - this is needed because we can't apply two access manager permissions in a single profile
    private int numberOfResults = 0;
    private readonly int numberOfPermissionsToGrant = 2;

    // Return Intent for StartActivityForResult
    private readonly Intent resultIntent = new Intent();

    public void ProfileApplied(string statusCode, string extendedStatusCode)
    {
        // Update Results Holder
        if (++numberOfResults == numberOfPermissionsToGrant)
        {
            resultIntent.PutExtra(PERMISSIONS_GRANTED_EXTRA, true);
            resultIntent.PutExtra(PERMISSIONS_STATUS_CODE, statusCode);
            resultIntent.PutExtra(PERMISSIONS_EXTENDED_STATUS_CODE, extendedStatusCode);
            SetResult(Result.Ok, resultIntent);
            Finish();
        }
    }

    public void ProfileError(string statusCode, string extendedStatusCode)
    {
        resultIntent.PutExtra(PERMISSIONS_GRANTED_EXTRA, false);
        resultIntent.PutExtra(PERMISSIONS_STATUS_CODE, statusCode);
        resultIntent.PutExtra(PERMISSIONS_EXTENDED_STATUS_CODE, extendedStatusCode);
        SetResult(Result.Ok, resultIntent);
        Finish();
    }
}