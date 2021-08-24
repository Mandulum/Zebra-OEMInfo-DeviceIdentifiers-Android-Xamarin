
using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Zebra_OEMInfo_DeviceIdentifiers_Android_Xamarin;

namespace Utilities
{

    public class CustomDialog
    {

        // Debugging
        private static readonly string TAG = "CustomDialog";

        // Constants
        public enum DialogType { SUCCESS, INFO, WARN, ERROR }

        // Static Variables


        // Variables

        public static void ShowCustomDialog(Context cx, DialogType type, string title, string message)
        {
            // Inflate View
            View customDialogView = LayoutInflater.From(cx).Inflate(Resource.Layout.layout_custom_dialog, null);

            // Get View Components
            RelativeLayout headerLayout = customDialogView.FindViewById<RelativeLayout>(Resource.Id.header_layout);
            ImageView headerIcon = customDialogView.FindViewById<ImageView>(Resource.Id.header_icon);
            TextView titleView = customDialogView.FindViewById<TextView>(Resource.Id.title);
            TextView messageView = customDialogView.FindViewById<TextView>(Resource.Id.message);

            // Set Component Values
            headerLayout.SetBackgroundColor(GetHeaderColor(cx, type));
            headerIcon.SetImageDrawable(GetHeaderIcon(cx, type));
            titleView.SetText(title, TextView.BufferType.Normal);
            messageView.SetText(message, TextView.BufferType.Normal);

            // Create Dialog
            AlertDialog customAlertDialog = new AlertDialog.Builder(cx)
                .SetView(customDialogView)
                .SetPositiveButton("OK", (IDialogInterfaceOnClickListener)null)
                .Create();

            // Show Dialog
            customAlertDialog.Show();
        }

        public static void ShowCustomDialog(Context cx, DialogType type, string title, string message,
            string positiveButtonText, EventHandler<DialogClickEventArgs> positiveClickListener,
            string negativeButtonText, EventHandler<DialogClickEventArgs> negativeClickListener)
        {

            // Inflate View
            View customDialogView = LayoutInflater.From(cx).Inflate(Resource.Layout.layout_custom_dialog, null);

            // Get View Components
            RelativeLayout headerLayout = customDialogView.FindViewById<RelativeLayout>(Resource.Id.header_layout);
            ImageView headerIcon = customDialogView.FindViewById<ImageView>(Resource.Id.header_icon);
            TextView titleView = customDialogView.FindViewById<TextView>(Resource.Id.title);
            TextView messageView = customDialogView.FindViewById<TextView>(Resource.Id.message);

            // Set Component Values
            headerLayout.SetBackgroundColor(GetHeaderColor(cx, type));
            headerIcon.SetImageDrawable(GetHeaderIcon(cx, type));
            titleView.SetText(title, TextView.BufferType.Normal);
            messageView.SetText(message, TextView.BufferType.Normal);

            // Create Dialog
            AlertDialog customAlertDialog = new AlertDialog.Builder(cx)
                .SetView(customDialogView)
                .SetPositiveButton(positiveButtonText, positiveClickListener)
                .SetNegativeButton(negativeButtonText, negativeClickListener)
                .Create();

            // Show Dialog
            customAlertDialog.Show();
        }

        public static AlertDialog BuildLoadingDialog(Context cx, string message, bool cancelable)
        {
            // Inflate View
            View customDialogView = LayoutInflater.From(cx).Inflate(Resource.Layout.layout_loading_dialog, null);

            // Get View Components
            ProgressBar progressBar = customDialogView.FindViewById<ProgressBar>(Resource.Id.progress_bar);
            TextView messageView = customDialogView.FindViewById<TextView>(Resource.Id.message);

            // Set View
            messageView.SetText(message, TextView.BufferType.Normal);

            // Create Dialog
            return new AlertDialog.Builder(cx)
                    .SetView(customDialogView)
                    .SetCancelable(cancelable)
                    .Create();
        }

        private static Color GetHeaderColor(Context cx, DialogType type)
        {
            int color = 0;
            switch (type)
            {
                case DialogType.SUCCESS:
                    color = cx.GetColor(Resource.Color.success);
                    break;
                case DialogType.INFO:
                    color = cx.GetColor(Resource.Color.info);
                    break;
                case DialogType.WARN:
                    color = cx.GetColor(Resource.Color.warn);
                    break;
                case DialogType.ERROR:
                    color = cx.GetColor(Resource.Color.error);
                    break;
            }
            return new Color(ContextCompat.GetColor(cx, color));
        }

        private static Drawable GetHeaderIcon(Context cx, DialogType type)
        {
            Drawable icon = null;
            switch (type)
            {
                case DialogType.SUCCESS:
                    icon = cx.GetDrawable(Resource.Drawable.ic_success);
                    break;
                case DialogType.INFO:
                    icon = cx.GetDrawable(Resource.Drawable.ic_info);
                    break;
                case DialogType.WARN:
                    icon = cx.GetDrawable(Resource.Drawable.ic_warning);
                    break;
                case DialogType.ERROR:
                    icon = cx.GetDrawable(Resource.Drawable.ic_error);
                    break;
            }
            return icon;
        }

    }
}