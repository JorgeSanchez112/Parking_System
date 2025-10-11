using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using ZXing;
using ZXing.Common;

namespace Parking.Utils
{
    public static class BarcodeHelper
    {

        public static String GenerateUniqueCodebar(String prefix = "TKT")
        {
            long ticks = DateTime.UtcNow.Ticks;
            string base36 = ToBase36(ticks);
            string shortCode = base36.Length > 5
            ? base36.Substring(base36.Length - 5)
            : base36.PadLeft(5, '0');

            return shortCode;
        }

        public static Bitmap GenerateBarcodeImage(String code, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Width = width,
                    Height = height,
                    Margin = 1
                }
            };

            return writer.Write(code);
        }

        private static string ToBase36(long value)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder();

            do
            {
                sb.Insert(0, chars[(int)(value % 36)]);
                value /= 36;
            } while (value > 0);

            return sb.ToString();
        }

    }

}
