using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photo
{
    public class ProfilePhotoFolder : IPhotoFolder
    {
        public string FolderName { get; set; }
    }
}
