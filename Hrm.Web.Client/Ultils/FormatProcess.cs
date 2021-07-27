using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Web.Client.Ultils
{
    public static class FormatProcess
    {
        static string[] mediaExtensions = {
            ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
            ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
            ".AVI", ".MP4", ".DIVX", ".WMV", //etc
        };

        public static string ToShortDate(this DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }
        public static int EnumToInt<T>(this T enumObj)
        {
            if (!typeof(T).IsEnum)
                throw new InvalidEnumArgumentException("Method is only supported enum");
            return Convert.ToInt32(enumObj);
        }

        public static string EnumToString<T>(this T enumObj)
        {
            if (!typeof(T).IsEnum)
                throw new InvalidEnumArgumentException("Method is only supported enum");
            return Convert.ToInt32(enumObj).ToString();
        }



        public static bool IsMediaFile(this string path)
        {
            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToUpperInvariant());
        }

    }
}
