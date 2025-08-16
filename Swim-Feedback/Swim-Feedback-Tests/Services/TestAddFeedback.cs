using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.ContentModel;
using Swim_Feedback.Data;
using Microsoft.AspNetCore.Identity;
using Swim_Feedback.Models;
using Swim_Feedback.Pages;
using Swim_Feedback.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Swim_Feedback_Tests.Services
{
    [TestClass]
    public class TestAddFeedback
    {
        public TestAddFeedback() {
            DbContextFactory = new MyDbContextFactory();

            feedback = new() { DbContextFactory = DbContextFactory };
        }

        private IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        private UserManager<IdentityUser>? userManager { get; set; }


        private Feedback? feedback;


        [TestMethod]
        public void TestAddFeedbackFunction()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            Student student = dbContext.Students.First();

            int currentFeedbackAmountWithScore2 = dbContext.StudentFeedbacks
                                        .Where(f => f.StudentId == student.StudentId || f.Student == student)
                                        .Where(f => f.Score == 2)
                                        .Count();

            feedback.StudentId = student.StudentId;

            feedback.SetScore(1);

            int newFeedbackAmountWithScore2 = dbContext.StudentFeedbacks
                                        .Where(f => f.StudentId == student.StudentId || f.Student == student)
                                        .Where(f => f.Score == 2)
                                        .Count();

            Assert.IsTrue(newFeedbackAmountWithScore2 == currentFeedbackAmountWithScore2 + 1);
        }


    }
}
