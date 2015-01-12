using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Bazam.APIs
{
    public class Amazonian
    {
        public string BucketName { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public Amazonian(string bucketName, string publicKey, string privateKey)
        {
            this.BucketName = bucketName;
            this.PrivateKey = privateKey;
            this.PublicKey = publicKey;
        }
        public byte[] DownloadFile(string amazonFileName)
        {
            string request = BuildRequest(amazonFileName);
            return new WebClient().DownloadData(request);
        }

        public string DownloadFileContents(string fileName)
        {
            string request = BuildRequest(fileName);
            return new WebClient().DownloadString(request);
        }

        private string BuildRequest(string fileName)
        {
            string expirationValue = Math.Floor(((TimeSpan)(DateTime.UtcNow.AddMinutes(10) - new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string stringToSign = string.Format(
                "GET\n\n\n{0}\n/{1}/{2}",
                expirationValue,
                BucketName,
                fileName
            );
            string signature = GetSignature(stringToSign);
            string amazonBucketUrl = string.Format("http://{0}.s3.amazonaws.com/", BucketName);

            return string.Format(
                "{0}{1}?AWSAccessKeyId={2}&Expires={3}&Signature={4}",
                amazonBucketUrl,
                fileName,
                PublicKey,
                expirationValue,
                signature
            );
        }

        private string GetSignature(string toBeSigned)
        {
            byte[] encodedShit = new UTF8Encoding().GetBytes(toBeSigned);
            HMACSHA1 sha = new HMACSHA1(new UTF8Encoding().GetBytes(this.PrivateKey));
            string base64 = Convert.ToBase64String(sha.ComputeHash(encodedShit));

            return Uri.EscapeDataString(base64);
        }
    }
}