using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nyan.Imaging.Helpers
{
    public enum WebpEncodingFormat
    {
        EncodeLossless,
        EncodeNearLossless,
        EncodeLossy
    }

    public struct WebPQuality
    {
        public static readonly WebPQuality empty;
        public static WebPQuality Default = new WebPQuality(WebpEncodingFormat.EncodeLossy, 74, 6);

        /// <summary>
        /// The encoding format of the webp.
        /// </summary>
        [Description("The encoding format."), DisplayName("Encoding Format")]
        public WebpEncodingFormat Format { get; set; }

        /// <summary>
        /// Between 0 (lower quality, lowest file size) and 100 (highest quality, higher file size)
        /// </summary>
        [Description("The quality level."), DisplayName("Quality 0 - 100")]
        public int Quality
        {
            get
            {
                return quality;
            }
            set
            {
                if (value < 0)
                {
                    quality = 0;
                    return;
                }
                if (value > 100)
                {
                    quality = 100;
                    return;
                }
                quality = value;
            }
        }
        private int quality;

        /// <summary>
        /// Between 0 (fastest, lowest compression) and 9 (slower, best compression)
        /// </summary>
        [Description("The speed."), DisplayName("Speed 0 - 9 (fast - slow)")]
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                if (value < 0)
                {
                    speed = 0;
                    return;
                }
                if (value > 9)
                {
                    speed = 9;
                    return;
                }
                speed = value;
            }
        }
        private int speed;

        public WebPQuality(WebpEncodingFormat fmt, int quality, int speed) : this()
        {
            Format = fmt;
            Speed = speed;
            Quality = quality;
        }

        public static bool operator ==(WebPQuality left, WebPQuality right)
        {
            return (left.Format == right.Format) && (left.Speed == right.Speed) && (left.Quality == right.Quality);
        }

        public static bool operator !=(WebPQuality left, WebPQuality right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Format, quality, speed);
        }
    }
}
