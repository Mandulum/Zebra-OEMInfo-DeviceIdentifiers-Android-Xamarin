using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Utilities;
using ZebraOEMInfoXamarinAndroid;
using AndroidUri = Android.Net.Uri;

namespace DeviceIdentifierManager
{
    public class RetrieveOemInfo : AsyncTask
    {
        // debugging
        private static readonly string TAG = "RetrieveOemInfo";

        // Main Thread Handler
        private readonly Handler mHandler = new Handler(Looper.MainLooper);

        // Variables
        private readonly WeakReference<Context> mContextWeakRef;
        private readonly AndroidUri[] mContentProviderUris;
        private readonly OnOemInfoRetrievedListener mOnOemInfoRetrievedListener;
        private readonly AlertDialog mProgressDialog;

        public RetrieveOemInfo(Context context, AndroidUri[] contentProviderUris,
                               OnOemInfoRetrievedListener onOemInfoRetrievedListener)
        {
            this.mContextWeakRef = new WeakReference<Context>(context);
            this.mContentProviderUris = contentProviderUris;
            this.mOnOemInfoRetrievedListener = onOemInfoRetrievedListener;

            mContextWeakRef.TryGetTarget(out Context cx);
            this.mProgressDialog = CustomDialog.BuildLoadingDialog(cx, "Obtaining OEM Info...", false);
        }

        protected override void OnPreExecute()
        {
            base.OnPreExecute();
            mHandler.Post(() => mProgressDialog.Show());
        }

        protected override Java.Lang.Object? DoInBackground(params Java.Lang.Object[]? @params)
        {
            mContextWeakRef.TryGetTarget(out Context cx);
            if (cx != null)
            {
                // Init Holder
                Dictionary<string, string> oemIdentifiers = new Dictionary<string, string>();

                // Loop Content Providers
                foreach (AndroidUri contentProviderUri in mContentProviderUris)
                {
                    // Grab Cursor using URI
                    ICursor cursor = cx.ContentResolver.Query(contentProviderUri, null, null, null, null);

                    // Validate Permissions
                    if (cursor == null || cursor.Count < 1)
                    {
                        mHandler.Post(() => mOnOemInfoRetrievedListener.OnPermissionError(cx.GetString(Resource.String.permissions_error_message)));
                        return null;
                    }

                    // Loop Cursor
                    while (cursor.MoveToNext())
                    {
                        // Validate Cursor
                        if (cursor.Count == 0)
                        {
                            mHandler.Post(() => mOnOemInfoRetrievedListener.OnUnknownError(cx.GetString(Resource.String.permissions_dialog_message)));
                        }
                        else
                        {
                            // Loop Cursor Columns
                            for (int i = 0; i < cursor.ColumnCount; i++)
                            {
                                try
                                {
                                    oemIdentifiers.Add(cursor.GetColumnName(i), cursor.GetString(cursor.GetColumnIndex(cursor.GetColumnName(i))));
                                }
                                catch (Exception e)
                                {
                                    mHandler.Post(() => mOnOemInfoRetrievedListener.OnUnknownError(e.Message));
                                }
                            }
                        }
                    }

                    // Close Cursor
                    cursor.Close();
                }

                // Return Values
                mHandler.Post(() => mOnOemInfoRetrievedListener.OnDetailsRetrieved(oemIdentifiers));
            }
            else
            {
                Android.Util.Log.Error(TAG, "Could not access valid context, quitting.");
            }
            return null;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            base.OnPostExecute(result);
            mHandler.Post(() => mProgressDialog.Dismiss());
        }

        public interface OnOemInfoRetrievedListener
        {
            void OnDetailsRetrieved(Dictionary<string, string> oemIdentifiers);
            void OnPermissionError(string e);
            void OnUnknownError(string e);
        }
    }
}