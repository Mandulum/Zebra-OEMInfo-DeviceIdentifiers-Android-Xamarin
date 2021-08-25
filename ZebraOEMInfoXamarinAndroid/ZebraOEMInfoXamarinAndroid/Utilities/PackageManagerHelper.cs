using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace Utilities
{
    public class PackageManagerHelper
    {
        public static string GetSigningCertBase64(Context cx)
        {
            //convert hex sig to byte array (1st step)
            byte[] byteArray = GetSigningCertificateHex(cx)[0].ToByteArray();

            // The String decoded to Base64 (2nd step)
            // return Base64.encodeBase64String(byteArray); -> Throws error on Android 8
            return Base64.EncodeToString(byteArray, Base64Flags.NoWrap);
        }

        public static Signature[] GetSigningCertificateHex(Context cx)
        {
            Signature[] sigs;
            SigningInfo signingInfo;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                signingInfo = cx.PackageManager.GetPackageInfo(cx.PackageName, PackageInfoFlags.SigningCertificates).SigningInfo;
                sigs = signingInfo.GetApkContentsSigners();
            }
            else
            {
                sigs = cx.PackageManager.GetPackageInfo(cx.PackageName, PackageInfoFlags.Signatures).Signatures.ToArray();
            }
            return sigs;
        }
    }
}