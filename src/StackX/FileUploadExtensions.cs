using System;
using System.IO;
using System.Linq;
using ServiceStack;

namespace StackX.ServiceInterface
{
    public static class FileUploadExtensions
    {
        /// <summary>
        /// This return the file full path on the default upload folder ex. /uploads/filename.png
        /// </summary>
        /// <param name="_"></param>
        /// <param name="fileName">filename.png</param>
        /// <param name="subFolder">Sub folder on which the file is going to be stored inside default upload path, it must be allowed on AllowedUploadSubFolders</param>
        /// <returns>ex.  /uploads/filename.png</returns>
        public static string GetDefaultUploadFullFilePath(this ServiceConfig config , string fileName, string subFolder = null)
        {
            var root = config.DefaultUploadsPath.Replace("~", "").MapServerPath();
            var invalidParts = new[] {"../", "..\\", "~", "./", ".\\"};

            if (subFolder is null)
            {
                return $"{root}/{fileName}";
            }
            
            if (invalidParts.Any(subFolder.Contains))
            {
                throw new ArgumentException("Specified sub folder contains not allowed chars");
            }

            if (!config.DisableSubFolderCheck && !config.AllowedUploadSubFolders.Contains(subFolder))
            {
                throw new ArgumentException("Specified sub folder not allowed");
            }
            root = Path.Combine(root, subFolder);
            return $"{root}/{fileName}";
        }
    }
}