
using System;
using System.Linq;
using Android.OS;
using Symbol.XamarinEMDK;

namespace ProfileManager
{
    public class ProcessProfile : AsyncTask
    {
        // Debugging
        private static readonly string TAG = "ProcessProfile";

        // Constants


        // Static Variables


        // Non-Static Variables
        private readonly string mProfileName;
        private readonly Symbol.XamarinEMDK.ProfileManager mProfileManager;
        private readonly OnProfileApplied mOnProfileApplied;

        public ProcessProfile(String profileName, Symbol.XamarinEMDK.ProfileManager profileManager, OnProfileApplied onProfileApplied)
        {
            this.mProfileName = profileName;
            this.mProfileManager = profileManager;
            this.mOnProfileApplied = onProfileApplied;
        }

        protected override Java.Lang.Object? DoInBackground(params Java.Lang.Object[]? @parameters)
        {
            // Execute Profile
            string[] stringParams = @parameters.Select(x => x.ToString()).ToArray();
            return mProfileManager.ProcessProfile(mProfileName, Symbol.XamarinEMDK.ProfileManager.PROFILE_FLAG.Set, stringParams);
        }

        protected override void OnPostExecute(Java.Lang.Object? result)
        {
            base.OnPostExecute(result);

            EMDKResults results = (EMDKResults)result;

            // Log Result
            Android.Util.Log.Info(TAG, "Profile Manager Result: " + results.StatusCode + " | " + results.ExtendedStatusCode);

            // Notify Class
            if (results.StatusCode == EMDKResults.STATUS_CODE.CheckXml || results.StatusCode == EMDKResults.STATUS_CODE.Success)
            {
                Android.Util.Log.Info(TAG, "XML: " + results.StatusString);
                mOnProfileApplied.ProfileApplied(results.StatusCode.ToString(), results.ExtendedStatusCode.ToString());
            }
            else if (results.StatusCode == EMDKResults.STATUS_CODE.Failure)
            {
                mOnProfileApplied.ProfileError(results.StatusCode.ToString(), results.ExtendedStatusCode.ToString());
            }
        }
    }
}
