using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer;
using System.IO;
using System.Drawing;

namespace ImageFileResizer
{
    public class ImageManipulator
    {
        private long maxImageBytes = (512 * 1024); //512KB
        public ImageManipulator()
        {

        }

        public void MaxImageBytes(long newMax)
        {
            maxImageBytes = newMax;
        }

        public  void ResizeImageBytes(string source, string target)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException();
            }

            if (string.Compare(source, target, true) == 0)
            {
                throw new ArgumentException("Target and Source cannot be the same!");
            }

            //load bitmap to get image attributes
            Bitmap img = new Bitmap(source);

            //initialise resize settings
            ResizeSettings resizeSetting = new ResizeSettings
            {
                Width = img.Height,
                Height = img.Width,
                Format = "jpg"
            };

            //load MemoryStream to determine size in bytes
            MemoryStream sourceStream = new MemoryStream(File.ReadAllBytes(source));
            MemoryStream targetStream = new MemoryStream();
            ImageBuilder.Current.Build(sourceStream, targetStream, resizeSetting);

            resizeSetting = GetMaxImageSettings(targetStream, resizeSetting);
//            sourceStream.Close();
//            sourceStream.Dispose();
  
            targetStream.Seek(0, SeekOrigin.Begin);
            ImageBuilder.Current.Build(targetStream, target, resizeSetting);
        }


        private ResizeSettings GetMaxImageSettings(MemoryStream imageStream, ResizeSettings currentSettings)
        {
            //keep reducing the height and width by 5% until bytes below threshold
            while (imageStream.Length > maxImageBytes)
            {
                currentSettings.Width = (int)((double)currentSettings.Width * 0.95);
                currentSettings.Height = (int)((double)currentSettings.Height * 0.95);
                imageStream = ResizeImageStream(imageStream, currentSettings);
            }
            return new ResizeSettings
            {
                Width = currentSettings.Width,
                Height = currentSettings.Height,
                Format = currentSettings.Format
            };
        }

        private MemoryStream ResizeImageStream(MemoryStream sourceStream, ResizeSettings settings)
        {
            MemoryStream targetStream = new MemoryStream();
            ImageBuilder.Current.Build(sourceStream, targetStream, settings);
            return targetStream;
        }
    }
}
