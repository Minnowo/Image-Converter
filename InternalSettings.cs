using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Converter
{
    public static class InternalSettings
    {
        public static string Image_Dialog_Default = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";

        public static string WebP_File_Dialog = "WebP (*.webp)|*.webp";


        public static List<string> Readable_Image_Formats_Dialog_Options = new List<string>
        { "*.png", "*.jpg", "*.jpeg", "*.jpe", "*.jfif", "*.gif", "*.bmp", "*.tif", "*.tiff" };

        public static List<string> Readable_Image_Formats = new List<string>()
        { "png", "jpg", "jpeg", "jpe", "jfif", "gif", "bmp", "tif", "tiff" };

        public static bool WebP_Plugin_Exists = CheckWebpPlugin();
        public static bool CPU_Type_x64 = IntPtr.Size == 8;

        private static bool CheckWebpPlugin()
        {
            if (CPU_Type_x64)
            {
                if (File.Exists(Path.Combine(AppContext.BaseDirectory, Nyan.Imaging.Helpers.UnsafeNativeMethods.libwebP_x64)))
                {
                    Readable_Image_Formats_Dialog_Options.Add("*.webp");
                    Readable_Image_Formats.Add("webp");
                    return true;
                }
            }
            else
            {
                if (File.Exists(Path.Combine(AppContext.BaseDirectory, Nyan.Imaging.Helpers.UnsafeNativeMethods.libwebP_x86)))
                {
                    Readable_Image_Formats_Dialog_Options.Add("*.webp");
                    Readable_Image_Formats.Add("webp");
                    return true;
                }
            }
            return false;
        }

        public static string GetFilenameExtension(this string input, bool includeDot = false)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            int pos = input.LastIndexOf('.');

            if (pos < 0)
                return string.Empty;

            if (includeDot)
                return "." + input.Substring(pos + 1).ToLower();

            return input.Substring(pos + 1).ToLower();
        }
    }
}
