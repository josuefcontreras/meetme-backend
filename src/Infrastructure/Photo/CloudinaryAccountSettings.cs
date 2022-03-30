using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photo
{
    public class CloudinaryAccountSettings
    {
        public string CloudName { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string ProfilePhotoFolderName { get; set; }
        public string AppBaseFolderName { get; set; }
    }
}
