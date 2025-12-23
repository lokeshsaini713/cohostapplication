using System.Security.Cryptography;
using System.Text;

namespace Shared.Common
{
    public static class EncryptDecryptExtensions
    {
        public static string ToEncrypt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            var tripleDES = TripleDES.Create();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(SiteKeys.EncryptDecryptKey ?? string.Empty)
;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            string base64 = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return base64.Replace("/", "-$$-$$-");
        }

        public static string ToDecrypt(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            byte[] inputArray = Convert.FromBase64String(input.Replace("-$$-$$-", "/"));
            var tripleDES = TripleDES.Create();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(SiteKeys.EncryptDecryptKey ?? string.Empty);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);

        }

    }
}
