using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bazam.Modules
{
    public static class Amazonian
    {
        public static byte[] DownloadFile(string amazonFileName, string amazonBucketName, string publicKey, string privateKey)
        {
            string request = BuildRequest(amazonFileName, amazonBucketName, publicKey, privateKey);
            return new WebClient().DownloadData(request);
        }

        public static string DownloadFileContents(string fileName, string amazonBucketName, string publicKey, string privateKey)
        {
            string request = BuildRequest(fileName, amazonBucketName, publicKey, privateKey);
            return new WebClient().DownloadString(request);
        }

        private static string BuildRequest(string fileName, string amazonBucketName, string publicKey, string privateKey)
        {
            string expirationValue = Math.Floor(((TimeSpan)(DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string stringToSign = string.Format(
                "GET\n\n\n{0}\n/{1}/{2}",
                expirationValue,
                amazonBucketName,
                fileName
            );
            string signature = GetSignature(stringToSign, privateKey);
            string amazonBucketUrl = string.Format("http://{0}.s3.amazonaws.com/", amazonBucketName);

            return string.Format(
                "{0}{1}?AWSAccessKeyId={2}&Expires={3}&Signature={4}",
                amazonBucketUrl,
                fileName,
                publicKey,
                expirationValue,
                signature
            );
        }

        private static string GetSignature(string toBeSigned, string privateKey)
        {
            byte[] encodedShit = new UTF8Encoding().GetBytes(toBeSigned);
            HMACSHA1 sha = new HMACSHA1(new UTF8Encoding().GetBytes(privateKey));
            string base64 = Convert.ToBase64String(sha.ComputeHash(encodedShit));

            return HttpUtility.UrlEncode(base64);
        }
    }
}