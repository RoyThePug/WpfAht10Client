using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WpfAht10Client.Views.Converters
{
    public class ByteArrayToBitmapSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] source && source.Length > 0)
            {
                return ToBitmapSource(source);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ToByteArray(value as BitmapSource);
        }

        public static BitmapSource ToBitmapSource(byte[] buffer)
        {
            var bmp = new BitmapImage();

            using (MemoryStream strm = new MemoryStream())
            using (MemoryStream ms = new MemoryStream())
            {
                strm.Write(buffer, 0, buffer.Length);
                strm.Position = 0;

                if (strm.Length>1)
                {
                    System.Drawing.Image img = System.Drawing.Image.FromStream(strm);

                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms.Seek(0, SeekOrigin.Begin);

                    bmp.BeginInit();
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.StreamSource = ms;
                    bmp.EndInit();
                    bmp.Freeze();   

                    return bmp;
                }

                return null;
            }
        }

        public static byte[] ToByteArray(BitmapSource bitmap)
        {
            byte[] buffer = null;

            if (bitmap != null)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    buffer = stream.ToArray();
                }
            }

            return buffer;
        }
    }
}
