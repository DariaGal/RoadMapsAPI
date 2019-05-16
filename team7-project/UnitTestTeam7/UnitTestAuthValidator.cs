using NUnit.Framework;
using team7_project.Auth;

namespace Tests
{
    public class UnitTestAuthValidator
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Daria22w!")]
        [TestCase("lalaF1dd")]
        [TestCase("helloIt'sme12KKKKK56_+=")]
        public void TestCorrectPassword(string password)
        {
            var isValid = AuthValidator.Password(password,out var error);
            Assert.IsTrue(isValid);
        }
        
        [TestCase("lF2")]
        [TestCase("Allo5")]
        [TestCase("")]
        public void TestShortPassword(string password)
        {
            var isValid = AuthValidator.Password(password, out var error);
            Assert.IsFalse(isValid);
        }
        
        [TestCase("hello12345ggg")]
        [TestCase("&@)0rueoghjfg$")]
        public void TestPasswordWithoutUpperLetter(string password)
        {
            var isValid = AuthValidator.Password(password, out var error);
            Assert.IsFalse(isValid);
        }
        
        [TestCase("hellFGggg")]
        [TestCase("DariaGaliullina!#")]
        public void TestPasswordWithoutNumber(string password)
        {
            var isValid = AuthValidator.Password(password, out var error);
            Assert.IsFalse(isValid);
        }


        [TestCase("DARIA")]
        [TestCase("daria_gal")]
        [TestCase("-")]
        public void TestCorrectLogin(string login)
        {
            var isValid = AuthValidator.Login(login, out var error);
            Assert.IsTrue(isValid);
        }
    }

}