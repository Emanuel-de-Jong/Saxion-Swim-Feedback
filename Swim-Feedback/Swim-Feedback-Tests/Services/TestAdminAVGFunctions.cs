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
    public class TestAdminAVGFunctions
    {
        public TestAdminAVGFunctions() {
            DbContextFactory = new MyDbContextFactory();

            adminDashboard = new() { dbContextFactory = DbContextFactory };
            studentExport = new() { dbContextFactory = DbContextFactory };
        }

        private IDbContextFactory<ApplicationDbContext>? DbContextFactory { get; set; }

        private UserManager<IdentityUser>? userManager { get; set; }


        private AdminDashboard? adminDashboard;
        private StudentExport? studentExport;

        //Delete AVG Data tests
        [TestMethod]
        public void SucceedIfDataIsDeletedCorrectly()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            Student student = new Student(9999999, "test");
            StudentFeedback studentFeedback = new(student, "", 2, DateTimeOffset.Now);
            TeacherFeedback teacherFeedback = new TeacherFeedback("test", "test", DateTimeOffset.Now);
            teacherFeedback.Student = student;

            dbContext.Students.Add(student);
            dbContext.StudentFeedbacks.Add(studentFeedback);
            dbContext.TeacherFeedbacks.Add(teacherFeedback);
            dbContext.SaveChanges();

            adminDashboard.DeleteData(student);

            Assert.IsFalse(dbContext.Students.Contains(student));
            Assert.IsFalse(dbContext.StudentFeedbacks.Contains(studentFeedback));
            Assert.IsFalse(dbContext.TeacherFeedbacks.Contains(teacherFeedback));
        }

        [TestMethod]
        public void FailIfDeleteFunctionDoesNotDetectStudentInDatabase()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            Student student = new Student(9999998, "test");
            TeacherFeedback teacherFeedback = new TeacherFeedback("test", "test", DateTimeOffset.Now);
            teacherFeedback.Student = student;

            dbContext.Students.Add(student);
            dbContext.TeacherFeedbacks.Add(teacherFeedback);
            dbContext.SaveChanges();
            
            var response = adminDashboard.DeleteData(student);

            Assert.IsTrue(response.IsEqual(new ResponseMessage<string>(HttpStatusCode.OK, "Data has been removed", "")));
        }

        //Anonomize Data AVG tests

        [TestMethod]
        public void SucceedIfDataIsAnnomizedCorrectly()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            Student student = new Student(9999997, "test");
            StudentFeedback studentFeedback = new(student, "", 2, DateTimeOffset.Now);

            dbContext.Students.Add(student);
            dbContext.StudentFeedbacks.Add(studentFeedback);
            dbContext.SaveChanges();

            var response = studentExport.Anonymizedata(student);

            Assert.IsFalse(dbContext.Students
                .Where(s => s.CustomerNumber == student.CustomerNumber)
                .Any());
        }

        [TestMethod]

        public void FailIfAnonymizeFunctionDoesNotDetectStudentInDatabase()
        {
            var dbContext = DbContextFactory.CreateDbContext();

            Student student = new Student(9999996, "test");

            var response = studentExport.Anonymizedata(student);

            Assert.IsTrue(response.IsEqual(new ResponseMessage<string>(HttpStatusCode.BadRequest, "Student does not exist", "")));
        }

        //Give User Role Test

/*        [TestMethod]
        public async void SucceedIfUserIsGivenAdminRole()
        {
            Pages_RRegister register = new Pages_RRegister();
            string email = "test";
            string password = "test";
            register.onGetAs
            IdentityResult result = await userManager.CreateAsync(user, password);
        }*/
    }
}
