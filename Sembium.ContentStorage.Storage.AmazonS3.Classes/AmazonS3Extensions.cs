using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sembium.ContentStorage.Storage.AmazonS3
{
    public static class AmazonS3Extensions
    {
        public static bool GetObjectExists(this IAmazonS3 amazonS3, string bucketName, string key)
        {
            try
            {
                var request = new GetObjectMetadataRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                // If the object doesn't exist then a "NotFound" will be thrown
                amazonS3.GetObjectMetadataAsync(request).Wait();

                return true;
            }
            catch (AggregateException ae)
            {
                if ((ae.InnerException is AmazonS3Exception) &&
                    (string.Equals(((AmazonS3Exception)ae.InnerException).ErrorCode, "NotFound")))
                {
                    return false;
                }

                throw;
            }
        }
    }
}
