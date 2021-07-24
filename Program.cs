using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Nyan.Imaging;
using Nyan.Imaging.Helpers;

namespace Image_Converter
{
    class Program
    {
        static void ConvertImage(string image, ImgFormat format)
        {
            if (string.IsNullOrEmpty(image) || !File.Exists(image))
                return;

            if (format == ImgFormat.nil)
                format = ImgFormat.png;

            string fileName = Path.GetFileNameWithoutExtension(image);
            string ext = format.ToString().ToLower().Trim();

            using(Bitmap im = ImageHelper.LoadImage(image))
            {
                if (im == null)
                    return;
                Console.WriteLine("saving " + Directory.GetCurrentDirectory() + "\\ConvertOut\\" + fileName + "." + ext);
                ImageHelper.SaveImage(im, Directory.GetCurrentDirectory() + "\\ConvertOut\\" + fileName + "." + ext);
            }
        }

        static void Main(string[] args)
        {
            string filePath;
            string convertTo = "png";
            string[] types = new string[] { "jpg", "png", "jpeg", "jpe", "jfij", "gif", "bmp", "tif", "tiff", "" };
            if (InternalSettings.WebP_Plugin_Exists)
                types[9] = "webp";
            switch (args.Length)
            {
                case 0:
                    return;

                case 1:
                    filePath = args[0];
                    Console.WriteLine(filePath);
                    while (true)
                    {
                        Console.WriteLine("convert to?  png jpg jpeg jpe jfij gif bmp tif tiff ");
                        if (InternalSettings.WebP_Plugin_Exists)
                            Console.Write("webp");
                        convertTo = Console.ReadLine();

                        if (types.Contains(convertTo))
                        {
                            if(convertTo == "webp")
                            {
                                WebpEncodingFormat fm = WebpEncodingFormat.EncodeLossless;
                                int speed = 0;
                                int quality = 65;
                                while (true)
                                {
                                    Console.WriteLine("please enter the webp encoding format\n  0 : Encode Lossless\n  1 : Encode Near Lossless\n  2 : Encode Lossy\n");

                                    if(int.TryParse(Console.ReadLine(), out int val))
                                    {
                                        if (val < 0)
                                            continue;
                                        if (val > 2)
                                            continue;
                                        fm = (WebpEncodingFormat)val;
                                        break;
                                    }
                                }
                                while (true)
                                {
                                    Console.WriteLine("please enter the webp quality (0 - 100) (low - high)\n");

                                    if (int.TryParse(Console.ReadLine(), out int val))
                                    {
                                        if (val < 0)
                                            continue;
                                        if (val > 100)
                                            continue;
                                        quality = val;
                                        break;
                                    }
                                }
                                while (true)
                                {
                                    Console.WriteLine("please enter the webp speed (0 - 9) (fast - slow)\n");

                                    if (int.TryParse(Console.ReadLine(), out int val))
                                    {
                                        if (val < 0)
                                            continue;
                                        if (val > 9)
                                            continue;
                                        speed = val;
                                        break;
                                    }
                                }
                                WebPQuality.Default = new WebPQuality(fm, quality, speed);
                            }
                            Helpers.CreateDirectory(Directory.GetCurrentDirectory() + "\\ConvertOut");
                            if (File.Exists(args[0]))
                            {
                                ConvertImage(args[0], ImageHelper.GetPathImageFormat("." + convertTo));
                            }
                            else if (Directory.Exists(args[0]))
                            {
                                List<string> files = new DirectoryInfo(args[0]).EnumerateFiles(".", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToList();
                                foreach (string file in files)
                                {
                                    ConvertImage(args[0] + "\\" + file, ImageHelper.GetPathImageFormat("." + convertTo));
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
                            ConvertImage(args[0], ImageHelper.GetPathImageFormat(args[1]));
                        }
                        else if (Directory.Exists(args[0]))
                        {
                            List<string> files = new DirectoryInfo(args[0]).EnumerateFiles(".", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToList();
                            foreach(string file in files)
                            {
                                ConvertImage(args[0] + "\\" + file, ImageHelper.GetPathImageFormat(args[1]));
                            }
                        }
                        return;
                    }
                    return;
            }
        }
    }
}
