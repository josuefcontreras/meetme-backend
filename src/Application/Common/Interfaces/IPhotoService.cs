using Application.Photos;
using Domain;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IPhotoService<T> where T : IPhotoFolder
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file, T folder);
        Task<string> DeletPhoto(Photo photo);

    }
}
