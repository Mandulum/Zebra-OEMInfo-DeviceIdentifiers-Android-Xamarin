

using System.Linq;
using System.Text;
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
            //convert String to char array (1st step)
            char[] charArray = GetSigningCertificateHex(cx)[0].ToChars();

            // decode the char array to byte[] (2nd step)
            //byte[] decodedHex = Hex.decodeHex(charArray);
            byte[] decodedHex = Encoding.ASCII.GetBytes(charArray);

            // The String decoded to Base64 (3rd step)
            // return Base64.encodeBase64String(decodedHex); -> Throws error on Android 8
            return Base64.EncodeToString(decodedHex, Base64Flags.NoWrap);
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