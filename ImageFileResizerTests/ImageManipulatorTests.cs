using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageFileResizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ImageFileResizer.Tests
{
    [TestClass()]
    public class ImageManipulatorTests
    {
        [TestMethod()]
        public void ResizeImageBytesTest_ThrowArgumentException()
        {
            string imageFilePath = "target";

            IImageManipulator imageManipulator = new ImageManipulator();

            Assert.ThrowsException<ArgumentException>(() => imageManipulator.ResizeImageBytes(imageFilePath, imageFilePath));
        }

        [TestMethod()]
        public void ResizeImageBytesTest_ThrowsArgumentNullException()
        {
            IImageManipulator imageManipulator = new ImageManipulator();

            Assert.ThrowsException<ArgumentNullException>(() => imageManipulator.ResizeImageBytes("", null));
        }

        [TestMethod()]
        public void ResizeImageBytesTest_SuccessResizeFile()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string sourcePath = basePath + "\\images\\testSourceImage.jpg";
            string targetPath = basePath + "\\images\\testTargetImage.jpg";

            IImageManipulator imageManipulator = new ImageManipulator();
            imageManipulator.ResizeImageBytes(sourcePath, targetPath);

            Assert.IsTrue(File.Exists(targetPath));
            File.Delete(targetPath);
        }
    }
}