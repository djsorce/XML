
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Reflection.Metadata;
using UserCaptureXML.Controllers;
using Assert = Xunit.Assert;

namespace Test
{
    [TestClass]
    public class UsersUnitTest
    {
        [TestMethod]
        [Fact]
        public void Test1()
        {
            //Arrange
            //Create - the objects here for test case
            var UniqueTime = DateTime.Now.TimeOfDay.Seconds;
            UserDetail User = new UserDetail
            {
                cellphone = "073456458" + UniqueTime.ToString(),
                name = "name1", 
                surname = "surname1"
            };
            //Act
            var UserCapture = new CaptureController();
            var Response = UserCapture.NewUser(User);
            //Assert
            var result = Response.IsCompletedSuccessfully;
            Assert.NotNull(Response);
            Assert.True(result);
        }
    }
}