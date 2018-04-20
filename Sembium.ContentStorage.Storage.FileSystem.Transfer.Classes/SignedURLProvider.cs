using Sembium.ContentStorage.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sembium.ContentStorage.Storage.FileSystem.Transfer
{
    public class SignedURLProvider : ISignedURLProvider
    {
        private readonly ISignatureProvider _signatureProvider;

        public SignedURLProvider(ISignatureProvider signatureProvider)
        {
            _signatureProvider = signatureProvider;
        }

        private string NormalizeUrl(string url)
        {
            var uri = new Uri(url);

            return uri.ToString();
        }

        public string GetSignedURL(string url, DateTimeOffset startMoment, DateTimeOffset expiryMoment)
        {
            var ub = new UriBuilder(NormalizeUrl(url));

            var query = System.Web.HttpUtility.ParseQueryString("");  // the only way to construct HttpValueCollection 

            if (startMoment != null)
            {
                query.Add("st", startMoment.ToString("u"));
            }

            if (expiryMoment != null)
            {
                query.Add("se", expiryMoment.ToString("u"));
            }

            var signature = GenerateSignature(url, query["st"], query["se"]);  

            query.Add("sig", signature);

            ub.Query = query.ToString();

            return NormalizeUrl(ub.ToString());
        }

        private void InvalidSignedURLError(string message, string url)
        {
            throw new UserException(message + " URL: " + url);
        }

        public void CheckSignedURL(string url)
        {
            var uri = new Uri(NormalizeUrl(url));

            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

            var startMoment = queryParams["st"];
            var expiryMoment = queryParams["se"];
            var signature = queryParams["sig"];

            if (string.IsNullOrEmpty(expiryMoment))
                InvalidSignedURLError("Expiry time required.", url);

            var fileUrl = url.Remove(url.IndexOf("?"));

            if (!VerifySignature(fileUrl, startMoment, expiryMoment, signature))
                InvalidSignedURLError("Invalid signature.", url);

            var now = DateTimeOffset.Now;

            if (!string.IsNullOrEmpty(startMoment))
            {
                var st = DateTimeOffset.ParseExact(startMoment, "u", CultureInfo.InvariantCulture);

                if (now < st)
                    InvalidSignedURLError("Start time not reached yet.", url);
            }

            var se = DateTimeOffset.ParseExact(expiryMoment, "u", CultureInfo.InvariantCulture);

            if (now > se)
                InvalidSignedURLError("Expired", url);
        }

        private string GenerateSignature(string url, string startMoment, string expiryMoment)
        {
            var data = string.Concat(url, startMoment, expiryMoment);
            return _signatureProvider.GenerateSignature(data);
        }

        private bool VerifySignature(string url, string startMoment, string expiryMoment, string signature)
        {
            if (string.IsNullOrEmpty(signature))
            {
                return false;
            }

            var data = string.Concat(url, startMoment, expiryMoment);
            return _signatureProvider.VerifySignature(data, signature);
        }
    }
}
