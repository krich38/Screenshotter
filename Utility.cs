using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace ScreenShifter
{
    class Utility
    {

        public static string FormatBytes(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        public static string GetTime()
        {
            return DateTime.Now.ToString("dd/MM/yy H:mm:ss");
        }

        public static Dictionary<int, WebClient> UploadingClients = new Dictionary<int, WebClient>();

        private static Uri imguri = new Uri("http://imgur.com/api/upload.xml");

        public static void DoUploadImage(Image img, string filename, FormMain evntfrm)
        {
            var UploaderClient = new WebClient();
            UploaderClient.Proxy = null; // fix bug http://bytes.com/topic/net/answers/792637-webclient-freezes-ten-seconds-first-connect
            ImageConverter converter = new ImageConverter();
            UploaderClient.UploadProgressChanged += new UploadProgressChangedEventHandler(evntfrm.UploaderClient_UploadProgressChanged);
            UploaderClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(evntfrm.UploaderClient_UploadValuesCompleted);
            String imagedata;
            using (var stream = new MemoryStream())
            {
                if (Settings.Instance.UploadImageFormat == Settings.Format.Png)
                {
                    img.Save(stream, ImageFormat.Png);
                }
                else
                {
                    var encparams = new EncoderParameters(1);
                    encparams.Param[0] = new EncoderParameter(Encoder.Quality, Settings.Instance.UploadImageQuality);
                    img.Save(stream, ImageCodecInfo.GetImageDecoders().FirstOrDefault(fmt => fmt.FormatID == ImageFormat.Jpeg.Guid), encparams);
                }
                imagedata = Convert.ToBase64String(stream.ToArray());
            }
            var values = new NameValueCollection
                {
                    { "key", Settings.Instance.ImgurApiKey },
                    { "name", filename + "." + Settings.Instance.UploadImageFormat },
                    { "image", imagedata }
                };
            string s = "Starting " + filename + " upload in format: " + Settings.Instance.UploadImageFormat + ", " + Utility.FormatBytes(imagedata.Length) + " - " + img.Width + "x" + img.Height;
            int i;
            if (evntfrm.listBoxImages.Items[0].ToString().StartsWith("Up"))
                i = 0;
            else
                i = evntfrm.listBoxImages.Items.Add(s);
            evntfrm.Log("(" + i + ") " + s);
            UploaderClient.UploadValuesAsync(imguri, "POST", values, i);
            UploadingClients.Add(i, UploaderClient);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Rectangle GetRectangleFromScreen(bool fullscreen = true)
        {
            Rectangle r = new Rectangle();
            if (fullscreen)
            {
                foreach (var scr in Screen.AllScreens)
                    r = Rectangle.Union(r, scr.Bounds);
            }
            else
            {
                var handle = GetForegroundWindow();
                Rect winrect = new Rect();
                GetWindowRect(handle, ref winrect);
                r.Y = winrect.Top;
                r.X = winrect.Left;
                r.Height = winrect.Bottom - winrect.Top;
                r.Width = winrect.Right - winrect.Left;
            }
            return r;
        }

        public static Image GetImageFromScreen(Rectangle rect)
        {
            var img = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.CopyFromScreen(rect.X, rect.Y, 0, 0, rect.Size);
            }
            return img;
        }

    }
}
