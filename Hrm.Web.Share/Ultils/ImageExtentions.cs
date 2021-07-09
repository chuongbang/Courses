using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Course.Web.Share.Ultils
{
    public static class ImageExtentions
    {
        public static void GetAllFileInFolder(this List<string> data, string folder)
        {
            if (data == null || folder == null)
            {
                return;
            }
            data.Clear();
            string selectedDirectory = Path.Combine(GlobalVariants.ResourceFolderPath, folder.Trim('\\'));
            if (Directory.Exists(selectedDirectory))
            {
                var files = new DirectoryInfo(selectedDirectory).GetFiles();
                data.AddRange(files.Select(c => $"/{GlobalVariants.ResourceFolderName}{(folder + "\\" + c.Name).ToUrl()}"));
            }
        }

        public static string ToUrl(this string localUrl)
        {
            if (localUrl == null)
            {
                return string.Empty;
            }
            return localUrl.Replace('\\', '/');
        }

        public static string ToFullUrl(this string localUrl)
        {
            if (localUrl == null)
            {
                return string.Empty;
            }
            return $"/{GlobalVariants.ResourceFolderName}/{localUrl.Replace('\\', '/').TrimStart('/')}";
        }
    }
}
