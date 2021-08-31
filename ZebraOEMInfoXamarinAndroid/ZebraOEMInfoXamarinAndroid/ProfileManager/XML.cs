using Android.Content;
using Utilities;

namespace ProfileManager
{
    public class XML
    {

        public static string GRANT_SERIAL_PERMISSION_NAME = "SerialPermission";
        public static string GRANT_IMEI_PERMISSION_NAME = "ImeiPermission";

        // Holders
        private readonly string mPackageSignatureHex;
        private readonly string mPackageName;

        public XML(Context context)
        {
            mPackageSignatureHex = PackageManagerHelper.GetSigningCertBase64(context);
            mPackageName = context.PackageName;
        }

        public string GetSerialPermissionXml()
        {
            return "<wap-provisioningdoc>" +
                    "  <characteristic type=\"Profile\">" +
                    "    <parm name=\"ProfileName\" value=\"SerialPermission\"/>" +
                    "    <characteristic version=\"8.3\" type=\"AccessMgr\">" +
                    "      <parm name=\"OperationMode\" value=\"1\" />" +
                    "      <parm name=\"ServiceAccessAction\" value=\"4\" />" +
                    "      <parm name=\"ServiceIdentifier\" value=\"content://oem_info/oem.zebra.secure/build_serial\" />" +
                    "      <parm name=\"CallerPackageName\" value=" + '"' + mPackageName + '"' + " />" +
                    "      <parm name=\"CallerSignature\" value=" + '"' + mPackageSignatureHex + '"' + "  />" +
                    "    </characteristic>" +
                    "  </characteristic>" +
                    "</wap-provisioningdoc>";
        }

        public string GetImeiPermissionXml()
        {
            return "<wap-provisioningdoc>" +
                    "  <characteristic type=\"Profile\">" +
                    "    <parm name=\"ProfileName\" value=\"ImeiPermission\"/>" +
                    "    <characteristic version=\"8.3\" type=\"AccessMgr\">" +
                    "      <parm name=\"OperationMode\" value=\"1\" />" +
                    "      <parm name=\"ServiceAccessAction\" value=\"4\" />" +
                    "      <parm name=\"ServiceIdentifier\" value=\"content://oem_info/wan/imei\" />" +
                    "      <parm name=\"CallerPackageName\" value=" + '"' + mPackageName + '"' + " />" +
                    "      <parm name=\"CallerSignature\" value=" + '"' + mPackageSignatureHex + '"' + "  />" +
                    "    </characteristic>" +
                    "  </characteristic>" +
                    "</wap-provisioningdoc>";
        }
    }
}