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
            return "<wap-provisioningdoc>\n" +
                    "  <characteristic type=\"Profile\">\n" +
                    "    <parm name=\"ProfileName\" value=\"SerialPermission\"/>\n" +
                    "    <characteristic version=\"8.3\" type=\"AccessMgr\">\n" +
                    "      <parm name=\"OperationMode\" value=\"1\" />\n" +
                    "      <parm name=\"ServiceAccessAction\" value=\"4\" />\n" +
                    "      <parm name=\"ServiceIdentifier\" value=\"content://oem_info/oem.zebra.secure/build_serial\" />\n" +
                    "      <parm name=\"CallerPackageName\" value=" + '"' + mPackageName + '"' + " />\n" +
                    "      <parm name=\"CallerSignature\" value=" + '"' + mPackageSignatureHex + '"' + "  />\n" +
                    "    </characteristic>\n" +
                    "  </characteristic>\n" +
                    "</wap-provisioningdoc>";
        }

        public string GetImeiPermissionXml()
        {
            return "<wap-provisioningdoc>\n" +
                    "  <characteristic type=\"Profile\">\n" +
                    "    <parm name=\"ProfileName\" value=\"ImeiPermission\"/>\n" +
                    "    <characteristic version=\"8.3\" type=\"AccessMgr\">\n" +
                    "      <parm name=\"OperationMode\" value=\"1\" />\n" +
                    "      <parm name=\"ServiceAccessAction\" value=\"4\" />\n" +
                    "      <parm name=\"ServiceIdentifier\" value=\"content://oem_info/wan/imei\" />\n" +
                    "      <parm name=\"CallerPackageName\" value=" + '"' + mPackageName + '"' + " />\n" +
                    "      <parm name=\"CallerSignature\" value=" + '"' + mPackageSignatureHex + '"' + "  />\n" +
                    "    </characteristic>\n" +
                    "  </characteristic>\n" +
                    "</wap-provisioningdoc>";
        }
    }
}