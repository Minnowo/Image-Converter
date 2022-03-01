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

using Nyan.Imaging;
using Nyan.Imaging.Helpers;

namespace Image_Converter
{
    public enum ImgFormat
    {
        png,
        jpg,
        gif,
        bmp,
        tif,
        webp,
        nil = -1
    }

    public static class ImageHelper
    {
        public static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Png;

        /// <summary>
        /// Load a wemp image. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="path"> The path to the image. </param>
        /// <returns> A bitmap object if the image is loaded, otherwise null. </returns>
        public static Bitmap LoadWebP(string path)
        {
            if (!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            try
            {
                using (Webp webp = new Webp())
                {
                    return webp.Load(path);
                }
            }
            catch
            {
            }
            return null;
        }

        /// <summary>
        /// Returns the image format based off the extension of the filepath.
        /// </summary>
        /// <param name="filePath">The filepath to check the extension of.</param>
        /// <returns></returns>
        public static ImgFormat GetPathImageFormat(string filePath)
        {
            string ext = filePath.GetFilenameExtension();

            switch (ext)
            {
                case "png":
                    return ImgFormat.png;
                case "jpg":
                case "jpeg":
                case "jpe":
                case "jfif":
                    return ImgFormat.jpg;
                case "gif":
                    return ImgFormat.gif;
                case "bmp":
                    return ImgFormat.bmp;
                case "tif":
                case "tiff":
                    return ImgFormat.tif;
                case "webp":
                    return ImgFormat.webp;
            }
            return ImgFormat.nil;
        }

        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="path"> The path to the image. </param>
        /// <returns> A bitmap object if the image is loaded, otherwise null. </returns>
        public static Bitmap LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            try
            {
                if (InternalSettings.WebP_Plugin_Exists)
                    if (GetPathImageFormat(path) == ImgFormat.webp)
                    {
                        return LoadWebP(path);
                    }
                return (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            }
            catch
            {
                if (InternalSettings.WebP_Plugin_Exists)
                {
                    // in case the file doesn't have proper extension there is no harm in trying to load as webp
                    Bitmap tryLoadWebP;
                    if ((tryLoadWebP = LoadWebP(path)) != null)
                        return tryLoadWebP;
                }
            }
            return null;
        }

        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Image img, string filePath, bool collectGarbage = true)
        {
            if (img == null || string.IsNullOrEmpty(filePath))
                return false;

            Helpers.CreateDirectoryFromFilePath(filePath);

            try
            {
                switch (GetPathImageFormat(filePath))
                {
                    default:
                    case ImgFormat.png:
                        img.Save(filePath, ImageFormat.Png);
                        return true;
                    case ImgFormat.jpg:
                        img.Save(filePath, ImageFormat.Jpeg);
                        return true;
                    case ImgFormat.bmp:
                        img.Save(filePath, ImageFormat.Bmp);
                        return true;
                    case ImgFormat.gif:
                        img.Save(filePath, ImageFormat.Gif);
                        return true;
                    case ImgFormat.tif:
                        img.Save(filePath, ImageFormat.Tiff);
                        return true;
                    case ImgFormat.webp:
                        return SaveWebp(img, filePath, WebPQuality.Default);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                if (collectGarbage)
                {
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Bitmap img, string filePath, bool collectGarbage = true)
        {
            return SaveImage((Image)img, filePath, collectGarbage);
        }

        /// <summary>
        /// Save a bitmap as a webp file. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="img"> The bitmap to encode. </param>
        /// <param name="filePath"> The path to save the bitmap. </param>
        /// <param name="q"> The webp quality args. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the bitmap was saved successfully, else false </returns>
        public static bool SaveWebp(Bitmap img, string filePath, WebPQuality q, bool collectGarbage = true)
        {
            if (!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(filePath) || img == null)
                return false;

            if (q == WebPQuality.empty)
                q = WebPQuality.Default;

            try
            {
                using (Webp webp = new Webp())
                {
                    byte[] rawWebP;

                    switch (q.Format)
                    {
                        default:
                        case WebpEncodingFormat.EncodeLossless:
                            rawWebP = webp.EncodeLossless(img, q.Speed);
                            break;
                        case WebpEncodingFormat.EncodeNearLossless:
                            rawWebP = webp.EncodeNearLossless(img, q.Quality, q.Speed);
                            break;
                        case WebpEncodingFormat.EncodeLossy:
                            rawWebP = webp.EncodeLossy(img, q.Quality, q.Speed);
                            break;
                    }

                    File.WriteAllBytes(filePath, rawWebP);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (collectGarbage)
                {
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Save an image as a webp file. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="img"> The image to encode. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="q"> The webp quality args. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveWebp(Image img, string filePath, WebPQuality q, bool collectGarbage = true)
        {
            return SaveWebp((Bitmap)img, filePath, q, collectGarbage);
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
