using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace Image_Converter
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        public static ImageFormat ImageFormatFromString(this string format)
        {
            Type type = typeof(System.Drawing.Imaging.ImageFormat);
            BindingFlags flags = BindingFlags.GetProperty;
            object o = type.InvokeMember(format, flags, null, type, null);
            return (ImageFormat)o;
        }


        public static T CloneSafe<T>(this T obj) where T : class, ICloneable
        {
            try
            {
                if (obj != null)
                {
                    return obj.Clone() as T;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public static T _Clamp<T>(T num, T min, T max) where T : IComparable<T>
        {
            if (num.CompareTo(min) <= 0) return min;
            if (num.CompareTo(max) >= 0) return max;
            return num;
        }

        public static T Clamp<T>(this T input, T min, T max) where T : IComparable<T>
        {
            return _Clamp(input, min, max);
        }

        public static bool IsValid(this Rectangle rect)
        {
            return rect.Width > 0 && rect.Height > 0;
        }
    }
}
