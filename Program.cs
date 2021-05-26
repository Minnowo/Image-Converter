using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Image_Converter
{
    class Program
    {
        static void ConvertImage(string image, ImageFormat format = null)
        {
            if (string.IsNullOrEmpty(image) || !File.Exists(image))
                return;

            if (format == null)
                format = ImageHelper.DEFAULT_IMAGE_FORMAT;

            string fileName = Path.GetFileNameWithoutExtension(image);
            string ext = format.ToString().ToLower().Trim();

            if (ext == "jpeg")
                ext = "jpg";

            using(Bitmap im = ImageHelper.LoadImage(image))
            {
                if (im == null)
                    return;
                Console.WriteLine("saving " + Directory.GetCurrentDirectory() + "\\ConvertOut\\" + fileName + "." + ext);
                ImageHelper.Save(Directory.GetCurrentDirectory() + "\\ConvertOut\\" + fileName + "." + ext, im, format);
            }
        }
        static void Main(string[] args)
        {
            string filePath;
            string convertTo = "png";
            string[] types = new string[] { "jpg", "png", "jpeg", "jpe", "jfij", "gif", "bmp", "tif", "tiff" };

            args = new string[1] { "D:\\Pictures\\Ω-Convert\\Random" };

            switch (args.Length)
            {
                case 0:
                    return;

                case 1:
                    filePath = args[0];
                    Console.WriteLine(filePath);
                    while (true)
                    {
                        Console.WriteLine("convert to? ( png jpg jpeg jpe jfij gif bmp tif tiff )");
                        convertTo = Console.ReadLine();
                        if (types.Contains(convertTo))
                        {
                            Helpers.CreateDirectory(Directory.GetCurrentDirectory() + "\\ConvertOut");
                            if (File.Exists(args[0]))
                            {
                                ConvertImage(args[0], ImageHelper.GetImageFormat(convertTo));
                            }
                            else if (Directory.Exists(args[0]))
                            {
                                List<string> files = new DirectoryInfo(args[0]).EnumerateFiles(".", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToList();
                                foreach (string file in files)
                                {
                                    ConvertImage(args[0] + "\\" + file, ImageHelper.GetImageFormat(convertTo));
                                }
                            }
                            return;
                        }
                    }

                case 2:
                default:
                    if (types.Contains(args[1]))
                    {
                        Helpers.CreateDirectory(Directory.GetCurrentDirectory() + "\\ConvertOut");
                        if (File.Exists(args[0]))
                        {
                            ConvertImage(args[0], ImageHelper.GetImageFormat(args[1]));
                        }
                        else if (Directory.Exists(args[0]))
                        {
                            List<string> files = new DirectoryInfo(args[0]).EnumerateFiles(".", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToList();
                            foreach(string file in files)
                            {
                                ConvertImage(args[0] + "\\" + file, ImageHelper.GetImageFormat(args[1]));
                            }
                        }
                        return;
                    }
                    return;
            }
        }
    }
}
