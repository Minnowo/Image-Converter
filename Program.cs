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
        public static string outputDir = "";
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
                Console.WriteLine("saving " + Path.Combine(outputDir, string.Format("{0}.{1}", fileName, ext)));
                ImageHelper.SaveImage(im, Path.Combine(outputDir, string.Format("{0}.{1}", fileName, ext)));
            }
        }

        static void Main(string[] args)
        {
            string convertTo = "png";
            string[] types = new string[] { "jpg", "png", "jpeg", "jpe", "jfij", "gif", "bmp", "tif", "tiff", "" };
            if (InternalSettings.WebP_Plugin_Exists)
                types[9] = "webp";

            string dir = "";
            string filePath = "";

            if (args != null)
                if (args.Length > 0)
                {
                    if (Directory.Exists(args[0]))
                        dir = args[0];
                    if (File.Exists(args[0]))
                        filePath = args[0];
                }

            if (string.IsNullOrEmpty(dir) && string.IsNullOrEmpty(filePath))
            {
                while (true)
                {
                    Console.WriteLine("enter directory or file path");
                    string input = Console.ReadLine().Trim();

                    if (Directory.Exists(input))
                    {
                        dir = input;
                        Console.WriteLine(dir);
                        break;
                    }
                    if (File.Exists(input))
                    {
                        filePath = input;
                        Console.WriteLine(filePath);
                        break;
                    }
                }
            }

            
            while (true)
            {
                if (InternalSettings.WebP_Plugin_Exists)
                    Console.WriteLine("convert to?  png jpg jpeg gif bmp tif tiff webp");
                else
                    Console.WriteLine("convert to?  png jpg jpeg gif bmp tif tiff");

                convertTo = Console.ReadLine().Trim().ToLower();

                if (!types.Contains(convertTo))
                    continue;
                
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

                if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                {
                    outputDir = Path.Combine(Path.GetDirectoryName(filePath), "ConvertOut");
                    Helpers.CreateDirectory(outputDir);
                    ConvertImage(filePath, ImageHelper.GetPathImageFormat("." + convertTo));
                    break;
                }

                if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                {
                    outputDir = Path.Combine(dir, "ConvertOut");
                    Helpers.CreateDirectory(outputDir);
                    List<string> files = new DirectoryInfo(dir).EnumerateFiles(".", SearchOption.TopDirectoryOnly).Select(x => x.Name).ToList();
                    foreach (string file in files)
                    {
                        ConvertImage(Path.Combine(dir, file), ImageHelper.GetPathImageFormat("." + convertTo));
                    }
                    break ;
                }
            }
            Console.WriteLine("\ndone.");
            Console.ReadLine();
        }
    }
}
