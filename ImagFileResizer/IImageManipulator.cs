using ImageResizer;
using System.IO;

namespace ImageFileResizer
{
    public interface IImageManipulator
    {
        /// <summary>
        /// Set max image size in bytes for this instance.
        /// </summary>
        /// <param name="newMax"></param>
        void MaxImageBytes(long newMax);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="target"></param>
        /// <param name="resizeSetting"></param>
        void ResizeImageBytes(MemoryStream sourceStream, string target, ResizeSettings resizeSetting);

        /// <summary>
        /// Resizes and copies a source image from one location to another.
        /// Source and Target must be different.
        /// Uses ImageResizer library
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void ResizeImageBytes(string source, string target);
    }
}