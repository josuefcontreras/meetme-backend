using Application;
using Application.Common.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photo
{
    public class CloudinaryPhotoService<T> : IPhotoService<T> where T : IPhotoFolder
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _baseFolderName;
        public CloudinaryPhotoService(IOptions<CloudinaryAccountSettings> cloudinarySettings, IOptions<FileStorageFolders> fileStorageFolders)
        {
            Account account = new Account(cloudinarySettings.Value.CloudName, cloudinarySettings.Value.APIKey, cloudinarySettings.Value.APISecret);
            _cloudinary = new Cloudinary(account);
            _baseFolderName = fileStorageFolders.Value.AppBaseFolderName;
        }
        public async Task<PhotoUploadResult> AddPhoto(IFormFile file, T folder)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var id = Guid.NewGuid().ToString();
                
                var uploadParams = new ImageUploadParams
                {
                    PublicId = $"{_baseFolderName}/{folder.FolderName}/{id}",
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return new PhotoUploadResult
                {
                    Id = id,
                    Folder = $"{_baseFolderName}/{folder.FolderName}",
                    Url = uploadResult.SecureUrl.ToString()
                };
            }

            return null;
        }
        public async Task<string> DeletPhoto(Domain.Photo photo)
        {
            var deleteParams = new DeletionParams($"{photo.Folder}/{photo.Id}");
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok" ? result.Result : null;
        }
    }
}
