using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Share.Ultils
{
    public class GlobalVariants
    {
        public const string ACCESSTOKEN = "data";
        public static string CategoriesFolder;
        public static Dictionary<string, string> TemplateKey;
        public static string HoanCanhDacBietKey = "HoanCanhDacBietKey";
        public static string ResourceFolderPath;
        public static string ResourceFolderName;
        public static string MaSoThue => "MaSoThue";
        public static string PrivateKey => ".gUju7KkmNPaF&vh+RmM_@yNyTx-LrwyA63_`yK4Wsp(}[AT@/Y9'T%^~;*su7/pevpZmA$d`K/<NPwa'Ns)EY<@95Tts`-yBJ>?9Eu=Sdn=JYEkQe<4J`&s-vV47";
        public static string JwtIssuer => "Hrm";
        private static string _fileVersion;
        public static string FileUploadPath;
        public static void InitFileVersion()
        {
            _fileVersion = DateTime.Now.Ticks.ToString();
        }
    }
}
