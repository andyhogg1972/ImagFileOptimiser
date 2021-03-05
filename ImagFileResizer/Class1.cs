using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageResizer;

namespace ImageFileResizer
{
    public class ImageManipulator
    {
        
        public static void ResizeImageBytes(string source, string target)
        {

            ResizeSettings resizeSetting = new ResizeSettings
            {
                Width = 150,
                Height = 100,
                Format = "png"
            };
            ImageBuilder.Current.Build(source, target, resizeSetting);
        }
    }
}
