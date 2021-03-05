using ImageResizer;
using System;
using System.Drawing;
using System.IO;

namespace ImageFileResizer
{
    public class ImageManipulator : IImageManipulator
    {
        private long maxImageBytes = (512 * 1024); //512KB
        public ImageManipulator()
        {

        }
        /// <summary>
        /// Set the max image size in bytes (long)
        /// </summary>
        /// <param name="newMax"></param>
        public void MaxImageBytes(long newMax)
        {
            maxImageBytes = newMax;
        }


        public void ResizeImageBytes(string source, string target)
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
                Width = img.Width,
                Height = img.Height
            };

            //load MemoryStream to determine size in bytes
            MemoryStream sourceStream = new MemoryStream(File.ReadAllBytes(source));

            ResizeImageBytes(sourceStream, target, resizeSetting);
        }

        /// <summary>
        /// Overloaded method to handle MemoryStream data taken from clipboard etc.
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="target"></param>
        /// <param name="resizeSetting"></param>
        public void ResizeImageBytes(MemoryStream sourceStream, string target, ResizeSettings resizeSetting)
        {
            //backup sourceStream
            MemoryStream sourceBackup = new MemoryStream();
            sourceStream.CopyTo(sourceBackup);
            sourceStream.Seek(0, SeekOrigin.Begin);
            sourceBackup.Seek(0, SeekOrigin.Begin);

            MemoryStream targetStream = new MemoryStream();

            //Execute initial Image builder as the different compression may reduce file size
            ImageBuilder.Current.Build(sourceStream, targetStream, resizeSetting);

            //Determine ResizeSettings required to meet file size criteria
            resizeSetting = GetMaxImageSettings(targetStream, resizeSetting);

            //Perform final Image resize and move to target.
            ImageBuilder.Current.Build(sourceBackup, target, resizeSetting);
        }

        /// <summary>
        /// Private function to determine image dimentions to meet file size restrictions.
        /// </summary>
        /// <param name="imageStream"></param>
        /// <param name="currentSettings"></param>
        /// <returns>ResizeSettings</returns>
        private ResizeSettings GetMaxImageSettings(MemoryStream imageStream, ResizeSettings currentSettings)
        {
            //keep reducing the height and width by 5% until bytes below threshold
            while (imageStream.Length > maxImageBytes)
            {
                //make sure MemoryStream is at the begining
                imageStream.Seek(0, SeekOrigin.Begin);
                //Reduce dimensions by 5%
                currentSettings.Width = (int)(currentSettings.Width * 0.95);
                currentSettings.Height = (int)(currentSettings.Height * 0.95);
                //resize the image stream with new dimensions.
                imageStream = ResizeImageStream(imageStream, currentSettings);

            }
            //once complete then return the new dimensions.
            return new ResizeSettings
            {
                Width = currentSettings.Width,
                Height = currentSettings.Height
            };
        }

        /// <summary>
        /// Takes a byte stream image and resizes it based on dimentions in settings
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="settings"></param>
        /// <returns>A new MemoryStream</returns>
        private MemoryStream ResizeImageStream(MemoryStream sourceStream, ResizeSettings settings)
        {
            MemoryStream targetStream = new MemoryStream();
            ImageBuilder.Current.Build(sourceStream, targetStream, settings);
            return targetStream;
        }
    }
}
