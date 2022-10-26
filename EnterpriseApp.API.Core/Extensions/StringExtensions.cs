using System;
using SD = System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EnterpriseApp.API.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static string RemoveNewLines(this string stringVal)
        {
            string formattedText = string.Empty;

            formattedText = stringVal?.Replace(System.Environment.NewLine, string.Empty);

            return formattedText;
        }

        public static string ToHashString(this string input)
        {
            var sBuilder = new StringBuilder();

            using (SHA256 hashAlgorithm = SHA256.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }

        public static long ToNumber(this string stringVal)
        {
            bool canParse = long.TryParse(stringVal, out long numberValue);

            return numberValue;
        }

        public static bool Search(this string stringVal, string text)
        {
            if (string.IsNullOrEmpty(stringVal) || string.IsNullOrEmpty(text))
            {
                return false;
            }

            bool contains = stringVal.ToLower().Contains(text.ToLower());

            return contains;
        }

        public static bool Match(this string stringVal, string text)
        {
            if (string.IsNullOrEmpty(stringVal) || string.IsNullOrEmpty(text))
            {
                return false;
            }

            bool equals = stringVal.Equals(text, StringComparison.InvariantCultureIgnoreCase);

            return equals;
        }

        public static bool IsURI(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            Uri uriResult;
            bool isURI = Uri.TryCreate(text, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isURI;
        }

        public static string ResizeBase64(this string stringVal, double desiredWidth, double desiredHeight)
        {
            if (string.IsNullOrEmpty(stringVal) || desiredWidth <= 0 || desiredHeight <= 0)
            {
                return string.Empty;
            }

            stringVal = stringVal.Replace("data:image/png;base64,", string.Empty);

            byte[] imageBytes = Convert.FromBase64String(stringVal);

            using (var ms = new MemoryStream(imageBytes))
            {
                var image = SD.Image.FromStream(ms);

                var ratioX = (double)desiredWidth / image.Width;
                var ratioY = (double)desiredHeight / image.Height;

                var ratio = Math.Min(ratioX, ratioY);

                var width = (int)(image.Width * ratio);
                var height = (int)(image.Height * ratio);

                var newImage = new SD.Bitmap(width, height);

                SD.Graphics.FromImage(newImage).DrawImage(image, 0, 0, width, height);

                SD.Bitmap bmp = new SD.Bitmap(newImage);

                SD.ImageConverter converter = new SD.XImageConverter();

                var data = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                return "data:image/*;base64," + Convert.ToBase64String(data);
            }
        }
    }
}
