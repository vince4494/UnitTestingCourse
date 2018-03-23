using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTestingCourse;
using System.Configuration;
using System.IO;

namespace UnitTestingCourse_Tests
{
    [TestClass]
    public class FileProcessTests
    {
        private const string BAD_FILE_NAME = @"C:\Windows\Regedit.exe";
        private string _GoodFileName;

        //allows to write test output that gets logged that can be used as confirmation that certain areas of the test are successful
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void FileNameDoesExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            SetGoodFileName();
            TestContext.WriteLine("Creating the file: " + _GoodFileName);
            File.AppendAllText(_GoodFileName, "Some Text");
            TestContext.WriteLine("Testing the file: " + _GoodFileName);
            fromCall = fp.FileExists(_GoodFileName);
            TestContext.WriteLine("Deleting the file: " + _GoodFileName);
            File.Delete(_GoodFileName);

            Assert.IsTrue(fromCall);
        }

        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if(_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }

        }

        [TestMethod]
        public void FileNameDoesNotExist()
        {
            FileProcess fp = new FileProcess();
            bool fromCall;

            fromCall = fp.FileExists(@"C:\BadFileName.bad");

            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            FileProcess fp = new FileProcess();

            fp.FileExists("");

        }

        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UseTryCatch()
        {
            FileProcess fp = new FileProcess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentNullException)
            {
                //the test was a success
                return;
            }

            Assert.Fail("Call to FileExists did NOT throw an Argument Null Exception");
        }
    }
}
