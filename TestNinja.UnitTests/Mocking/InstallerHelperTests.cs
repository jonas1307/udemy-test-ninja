using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class InstallerHelperTests
    {
        private Mock<IFileDownloader> _fileDownloader;
        private InstallerHelper _installerHelper;

        [SetUp]
        public void Setup()
        {
            _fileDownloader = new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_fileDownloader.Object);
        }

        [Test]
        public void DownloadInstaller_WhenCalled_ReturnsTrue()
        {
            Assert.That(_installerHelper.DownloadInstaller("", ""), Is.True);
        }

        [Test]
        public void DownloadInstaller_WhenCalled_ReturnsFalse()
        {
            _fileDownloader.Setup(s => s.DownloadFile(It.IsAny<string>(), It.IsAny<string>())).Throws<WebException>();

            Assert.That(_installerHelper.DownloadInstaller("", ""), Is.False);
        }
    }
}
