using Swim_Feedback.Services;
using Swim_Feedback.Models;
using Swim_Feedback.Data;
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Swim_Feedback_Tests.Services
{
    [TestClass]
    public class TestAccountService
    {
      //  private AccountService accountService = new();

     /*   [Inject]
        private IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        //Login Tests
        [TestMethod]
        public void Login_with_correct_credentials_possible()
        {
            ResponseMessage response = accountService.Login("admin", "admin");
            Assert.IsTrue(response.Status == HttpStatusCode.OK && response.Message.Contains("Token: "));
        }

        [TestMethod]
        public void Login_with_incorrect_password_not_possible()
        {
            ResponseMessage response = accountService.Login("admin", "......");
            Assert.IsTrue(response.Status == HttpStatusCode.BadRequest && response.Message.Equals("Incorrect password"));
        }

        [TestMethod]
        public void Login_with_empty_data_not_possible()
        {
            ResponseMessage response = accountService.Login("admin", "");

            ResponseMessage response2 = accountService.Login("", "admin");

            Assert.IsTrue(response.Status == HttpStatusCode.BadRequest && response.Message.Equals("Unable to login with empty data"));
            Assert.IsTrue(response.Status == HttpStatusCode.BadRequest && response.Message.Equals("Unable to login with empty data"));
        }

        [TestMethod]
        public void Login_with_non_existing_user_not_possible()
        {
            ResponseMessage response = accountService.Login("nonexist", "nonexist");
            Assert.IsTrue(response.Status == HttpStatusCode.BadRequest && response.Message.Equals("User does not exist"));
        }


        //Create account tests
        
        [TestMethod]
        public void Create_account_with_correct_data_possible()
        {
            string username = "Testing";
            ResponseMessage response = accountService.CreateAccount(username, "Test@gmail.com", "testPassword");

            bool check = checkIfUserExists(username);
            Assert.IsTrue(response.Status == HttpStatusCode.Created && response.Message.Equals("User created") && check == true);
        }

        [TestMethod]
        public void Create_account_with_empty_data_not_possible()
        {
            ResponseMessage response = accountService.CreateAccount("", "Test@gmail.com", "testPassword");
            ResponseMessage response2 = accountService.CreateAccount("Testing", "", "testPassword");
            ResponseMessage response3 = accountService.CreateAccount("Testing", "Test@gmail.com", "");

            Assert.IsTrue(response.Status == HttpStatusCode.BadRequest && response.Message.Equals("Unable to create a account with empty data"));
            Assert.IsTrue(response2.Status == HttpStatusCode.BadRequest && response2.Message.Equals("Unable to create a account with empty data"));
            Assert.IsTrue(response3.Status == HttpStatusCode.BadRequest && response3.Message.Equals("Unable to create a account with empty data"));
        }


        [TestMethod]
        public void Create_account_that_already_exists_not_possible()
        {
            ResponseMessage response = accountService.CreateAccount("Testing", "Test@gmail.com", "testPassword");
            Assert.IsTrue(response.Status == HttpStatusCode.Created && response.Message.Equals("User created"));

            ResponseMessage response2 = accountService.CreateAccount("Testing", "Test@gmail.com", "testPassword");
            Assert.IsTrue(response2.Status == HttpStatusCode.Conflict && response2.Message.Equals("User already exists"));
        }

        private Boolean checkIfUserExists(string username)
        {
            var dbContext = DbContextFactory.CreateDbContext();
            Account account = dbContext.Accounts.FirstOrDefault(Account => Account.Username == username);
            
            if(account == null)
            {
                return false;
            }
            return true;
        }*/
    }
}
