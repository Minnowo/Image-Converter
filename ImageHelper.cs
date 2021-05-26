using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace Image_Converter
{
    public static class ImageHelper
    {
        public static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Png;

        public static Bitmap LoadImage(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        return (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
                    }

                }
            }
            catch
            {
                try
                {
                    File.AppendAllText("error.txt", "unable to load " + path + " the file is most likely not supported \n");
                }
                catch { }
            }
            return null;
        }

        public static bool Save(string imageName, Image img, ImageFormat format = null)
        {
            if (img == null || string.IsNullOrEmpty(imageName))
                return false;

            if (format == null)
                format = DEFAULT_IMAGE_FORMAT;

            try
            {
                img.Save(imageName, format);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public static string SaveImageFileDialog(Image img, string filePath = "")
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";
                sfd.DefaultExt = "png";

                if (!string.IsNullOrEmpty(filePath))
                {
                    sfd.FileName = Path.GetFileName(filePath);

                    string ext = Helpers.GetFilenameExtension(filePath);

                    if (!string.IsNullOrEmpty(ext))
                    {
                        ext = ext.ToLowerInvariant();

                        switch (ext)
                        {
                            case "jpg":
                            case "jpeg":
                            case "jpe":
                            case "jfif":
                                sfd.FilterIndex = 2;
                                break;

                            case "tif":
                            case "tiff":
                                sfd.FilterIndex = 5;
                                break;

                            case "png":
                                sfd.FilterIndex = 1;
                                break;

                            case "gif":
                                sfd.FilterIndex = 3;
                                break;

                            case "bmp":
                                sfd.FilterIndex = 4;
                                break;
                        }
                    }
                }

                if (sfd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                {
                    SaveImage(img, sfd.FileName);
                    return sfd.FileName;
                }
            }

            return null;
        }

        public static bool SaveImage(Image img, string filePath)
        {
            Helpers.CreateDirectoryFromFilePath(filePath);
            ImageFormat imageFormat = GetImageFormatFromFile(filePath);

            if (img == null)
                return false;

            try
            {
                img.Save(filePath, imageFormat);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return false;
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return ImageFormat.Png;

            switch (extension.Trim().ToLower())
            {
                case "png":
                    return ImageFormat.Png;

                case "jpg":
                case "jpeg":
                case "jpe":
                case "jfif":
                    return ImageFormat.Jpeg;
                    
                case "gif":
                    return ImageFormat.Gif;
                   
                case "bmp":
                    return ImageFormat.Bmp;
                   
                case "tif":
                case "tiff":
                    return ImageFormat.Tiff;
            }

            return ImageFormat.Png;
        }

        public static ImageFormat GetImageFormatFromFile(string filePath)
        {
            ImageFormat imageFormat = ImageFormat.Png;
            string ext = Helpers.GetFilenameExtension(filePath);
 
            return GetImageFormat(ext);
        }
    }
}
