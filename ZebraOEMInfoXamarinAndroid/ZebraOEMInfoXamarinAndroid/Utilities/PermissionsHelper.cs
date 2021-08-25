
using Android.App;
using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace Utilities
{

    public class PermissionsHelper
    {

        // Constants
        public static int PERMISSIONS_REQUEST_CODE = 1000;
        private static readonly string[] PERMISSIONS = { };

        // Variables
        private readonly Activity mActivity;
        private readonly OnPermissionsResultListener mOnPermissionsResultListener;

        // Interfaces
        public interface OnPermissionsResultListener
        {
            void OnPermissionsGranted();
        }

        public PermissionsHelper(Activity activity, OnPermissionsResultListener onPermissionsResultListener)
        {
            this.mActivity = activity;
            this.mOnPermissionsResultListener = onPermissionsResultListener;
            ForcePermissionsUntilGranted();
        }

        private void ForcePermissionsUntilGranted()
        {
            if (CheckStandardPermissions())
            {
                mOnPermissionsResultListener.OnPermissionsGranted();
            }
            else
            {
                RequestStandardPermission();
            }
        }

        private bool CheckStandardPermissions()
        {
            bool permissionsGranted = true;
            foreach (string permission in PERMISSIONS)
            {
                if (ContextCompat.CheckSelfPermission(mActivity, permission) != Permission.Granted)
                {
                    permissionsGranted = false;
                    break;
                }
            }
            return permissionsGranted;
        }

        private void RequestStandardPermission()
        {
            ActivityCompat.RequestPermissions(mActivity, PERMISSIONS, PERMISSIONS_REQUEST_CODE);
        }

        public void OnRequestPermissionsResult()
        {
            ForcePermissionsUntilGranted();
        }

    }
}