// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("zxMncF2dIceRkDhDQU2mOYEn9Z5wwkFicE1GSWrGCMa3TUFBQUVAQ4S8LzUBEQy+60+w52MMMyuCQO7uoEvnwS1XgpDe51/sqlzfEyvtxZAmsaHXreZbkhs/i2BuEHixqa/Ido8CV6ELgwpPN7be7Sp6QnYdtiGzwkFPQHDCQUpCwkFBQPFbHNXx0/qtEiBAZq5Y0ccNngdvEsDP5AdKjTf6ld0Pcc0v9TfmD+GIEcCgNdxu7wcEd9B2AeITdKKTd0PDhVMao2f1vc45yMJPsIuijOY1hQ+ayYmCfIk5IPVs5d5CFIYH3iPa0WDzqRb14h5x5kumm5gsEZO9NrjFBdGoPyGWyJUHQr6mfyeP2qclmpD+pDnsXrFcRrzhiTDOQ0JDQUBB");
        private static int[] order = new int[] { 2,2,7,13,6,10,7,7,10,12,12,12,12,13,14 };
        private static int key = 64;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
