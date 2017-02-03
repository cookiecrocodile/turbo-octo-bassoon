using System;
using NUnit.Framework;
using HemtentaTdd2017.blog;
using HemtentaTdd2017;
using Moq;

namespace HemtentaTester
{
    [TestFixture]
    public class BlogTest
    {
        Blog blog;

        [SetUp]
        public void Setup()
        {
            blog = new Blog();
        }

        [Test]
        public void LoginUser_Success()
        {
            User u = new User("testUser");

            var mockAuth = new Mock<IAuthenticator>();
            mockAuth.Setup(x => x.GetUserFromDatabase(u.Name)).Returns(u);
            blog.SetAuthenticator(mockAuth.Object);

            blog.LoginUser(u);

            Assert.That(blog.UserIsLoggedIn, Is.True);
            mockAuth.Verify(x => x.GetUserFromDatabase(u.Name), Times.Once);

        }

        [Test]
        public void LoginUser_WrongPasswordFail()
        {
            User dbUser = new User("testUser");
            User u = new User(dbUser.Name);
            u.Password = "hejsansvejsan";

            var mockAuth = new Mock<IAuthenticator>();
            mockAuth.Setup(x => x.GetUserFromDatabase(u.Name)).Returns(dbUser);
            blog.SetAuthenticator(mockAuth.Object);

            blog.LoginUser(u);

            Assert.That(blog.UserIsLoggedIn, Is.False);
            mockAuth.Verify(x => x.GetUserFromDatabase(u.Name), Times.Once);
        }

        [Test]
        public void LoginUser_NullUserThrowsException()
        {
            Assert.That(() => blog.LoginUser(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void LoginUser_NoUserInDbThrowsException()
        {
            User uNull = null;
            User user = new User("username");

            var mockAuth = new Mock<IAuthenticator>();
            mockAuth.Setup(x => x.GetUserFromDatabase(user.Name)).Returns(uNull);
            blog.SetAuthenticator(mockAuth.Object);

            Assert.That(() => blog.LoginUser(user), Throws.TypeOf<NullReferenceException>());
            mockAuth.Verify(x => x.GetUserFromDatabase(user.Name), Times.Once);

        }

        [Test]
        public void LogoutUser_NullUserThrowsException()
        {
            Assert.That(() => blog.LogoutUser(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void LogoutUser_LogsOutUserWhoIsLoggedInSuccess()
        {
            User u = new User("LogMeOutUser");
            CreateAuthMockAndSignIn(u);

            blog.LogoutUser(u);

            Assert.That(blog.UserIsLoggedIn, Is.False);
        }


        [Test]
        public void PublishPage_NullPageThrowsException()
        {
            Assert.That(() => blog.PublishPage(null), Throws.TypeOf<ArgumentNullException>());
        }


        [Test]
        public void PublishPage_SuccessRetursTrue()
        {
            User u = new User("Publisher");

            CreateAuthMockAndSignIn(u);
            
            Page p = new Page();
            bool result = blog.PublishPage(p);
            Assert.That(result, Is.True);
        }

        [Test]
        public void PublishPage_FailsIfNotLoggedIn()
        {
            User u = new User("Publisher");

            Page p = new Page();
            bool result = blog.PublishPage(p);
            Assert.That(result, Is.False);
            
        }

        [TestCase(null, "Hello", "Message")]
        [TestCase("", "Hello", "Message")]
        [TestCase("NotAddress", "Hello", "Message")] //bestämde att det måste likna en mejladress

        [TestCase("hej@svejsan.se", null, "Message")]
        [TestCase("hej@svejsan.se", "", "Message")] //bestämde att en tom sträng inte är giltig

        [TestCase("hej@svejsan.se", "Hello", null)]
        [TestCase("hej@svejsan.se", "Hello", "")] //bestämde att en tom sträng inte är giltig
        public void SendEmail_InvalidParameterThrowsException(string address, string caption, string body)
        {
            Assert.That(() => blog.SendEmail(address, caption, body), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void SendEmail_ValidAndLoggedInSuccess()
        {
            User u = new User("Hannibal");
            CreateAuthMockAndSignIn(u);

            int result = blog.SendEmail("test@mail.net", "header", "body");
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void SendEmail_ValidParametersNotLoggedInFail()
        {
            int result = blog.SendEmail("test@mail.net", "header", "body");
            Assert.That(result, Is.EqualTo(0));
        }


        public void CreateAuthMockAndSignIn(User u)
        {
            var mockAuth = new Mock<IAuthenticator>();
            mockAuth.Setup(x => x.GetUserFromDatabase(u.Name)).Returns(u);
            blog.SetAuthenticator(mockAuth.Object);

            blog.LoginUser(u);
        }

    }
}
