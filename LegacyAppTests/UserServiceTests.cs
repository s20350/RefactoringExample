using System;
using LegacyApp;
using Xunit;

namespace LegacyAppTests
{
    public class UserServiceTests
    {
        [Fact]
        public void AddUser_Should_Return_False_If_FirstName_Is_Missing()
        {
            var service = new UserService();
            
            var result = service.AddUser(null, "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);
            
            Assert.False(result);
        }

        [Fact]
        public void AddUser_Should_Return_False_If_Email_Is_Invalid()
        {
            var service = new UserService();
            
            var result = service.AddUser("John", "Doe", "johndoeatexampledotcom", new DateTime(1980, 1, 1), 1);
            
            Assert.False(result);
        }

        [Fact]
        public void AddUser_Should_Return_False_If_User_Is_Under_21()
        {
            var service = new UserService();
            
            var result = service.AddUser("John", "Doe", "john.doe@example.com", DateTime.Today.AddYears(-20), 1);
            
            Assert.False(result);
        }

        [Fact]
        public void AddUser_Should_Return_True_When_All_Conditions_Are_Met()
        {
            var service = new UserService();
            
            var result = service.AddUser("John", "Doe", "john.doe@example.com", new DateTime(1980, 1, 1), 1);
            
            Assert.True(result);
        }
    }
}