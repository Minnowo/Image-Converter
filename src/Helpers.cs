using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Image_Converter
{
    public static class Helpers
    {
        public static string GetFilenameExtension(string filePath, bool includeDot = false)
        {
            if (string.IsNullOrEmpty(filePath))
                return "";
            
            int pos = filePath.LastIndexOf('.');

            if (pos < 0)
                return "";

            if (includeDot)
                return "." + filePath.Substring(pos + 1);            

            return filePath.Substring(pos + 1);
        }

        public static void CreateDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath))
                return;
            
            try
            {
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CreateDirectoryFromFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;
            
            CreateDirectory(Path.GetDirectoryName(filePath));
        }
    }
}
