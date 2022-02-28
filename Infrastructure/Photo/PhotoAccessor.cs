using Application.Interfaces;
using Application.Photos;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photo
{
    public class PhotoAccessor : IPhotoAccessor
    {
        private readonly string _photoBucketName;
        private readonly string _requestEndPoint;

        public PhotoAccessor(IOptions<GoogleCloudStorageSettings> config)
        {
            _requestEndPoint = config.Value.RequestEndPoint;
            _photoBucketName = config.Value.PhotoBucketName;   
        }
        public async Task<PhotoUploadResult?> AddPhoto(IFormFile file)
        {
            var client = await GetClient();
            var bucketName = _photoBucketName;
            var destination = Guid.NewGuid() + "-" + file.FileName; // check extention is included
            var contentType = file.ContentType;

            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();

                try
                {
                    var obj = await client
                        .UploadObjectAsync(bucketName, destination, contentType, stream);
                    var result = new PhotoUploadResult() { PublicId = obj.Name, Url = _requestEndPoint + bucketName + "/" +obj.Name };
                    return result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return null;
        }

        public async Task<string> DeletPhoto(string publicId)
        {
            var client = StorageClient.Create();
            try
            {
                await client.DeleteObjectAsync(_photoBucketName, publicId);
                return "ok";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<StorageClient> GetClient()
        {
            var credential = await GoogleCredential.GetApplicationDefaultAsync();
            var client = await StorageClient.CreateAsync(credential);
            return client;
        }
    }
}
