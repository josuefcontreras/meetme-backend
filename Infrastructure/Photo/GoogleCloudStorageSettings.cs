using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Photo
{
    public class GoogleCloudStorageSettings
    {
        public string PhotoBucketName { get; set; } = "";
        public string RequestEndPoint { get; set; } = "";
    }
}
