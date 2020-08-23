using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    class HousekeeperServiceTests
    {
        private Housekeeper _housekeeper;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private HousekeeperService service;

        private DateTime _statementDate = new DateTime(2020, 1, 1);
        private string _statementFileName;

        [SetUp]
        public void Setup()
        {
            _housekeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };

            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(s => s.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
                _housekeeper
            }.AsQueryable());

            _statementFileName = "filename";

            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator.Setup(v => v.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(() => _statementFileName);

            _emailSender = new Mock<IEmailSender>();
            _emailSender.Setup(v => v.EmailFile(It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>(),
                                                It.IsAny<string>()))
                .Throws<Exception>();

            _messageBox = new Mock<IXtraMessageBox>();

            service = new HousekeeperService(_unitOfWork.Object,
                                                 _statementGenerator.Object,
                                                 _emailSender.Object,
                                                 _messageBox.Object);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_ShouldGenerateStatements()
        {
            service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(v => v.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_EmailIsNull_ShouldNotGenerateStatements(string email)
        {
            _housekeeper.Email = email;

            service.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(v => v.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate), Times.Never);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_ShouldEmailStatement()
        {
            service.SendStatementEmails(_statementDate);

            _emailSender.Verify(v =>
                v.EmailFile(_housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<string>()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_StatementFileNameIsInvalid_ShouldNotEmailStatement(string filename)
        {
            _statementFileName = filename;

            service.SendStatementEmails(_statementDate);

            _emailSender.Verify(v =>
                v.EmailFile(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SendStatementEmails_ErrorSendingEmail_DisplayMessageBox()
        {
            service.SendStatementEmails(_statementDate);

            _messageBox.Verify(v => v.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK), Times.Once);
        }
    }
}
