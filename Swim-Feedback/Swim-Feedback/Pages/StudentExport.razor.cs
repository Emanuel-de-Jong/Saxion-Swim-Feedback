using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.JSInterop;
using OfficeOpenXml;
using Swim_Feedback.Data;
using Swim_Feedback.Models;
using System.Net;
using System.Security.Claims;

namespace Swim_Feedback.Pages
{

    public partial class StudentExport : ComponentBase
    {
        [Parameter]
        public bool IsReload { get; set; } = false;

        [Inject]
        private NavigationManager navigationManager { get; set; }
        [Inject]
        private IJSRuntime? js { get; set; }

        [Inject]
        private AuthenticationStateProvider? authenticationStateProvider { get; set; }

        [Inject]
        public IDbContextFactory<ApplicationDbContext>? dbContextFactory { get; set; }

        [Inject]
        private UserManager<IdentityUser> userManager { get; set; }

        private IdentityUser? currentUser;
        private List<Student>? Students;
        private Student? selectedStudent;

        protected override async Task OnInitializedAsync()
        {
            ClaimsPrincipal claimsPrincipal = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
            if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
            {
                currentUser = await userManager.GetUserAsync(claimsPrincipal);
            }

            ApplicationDbContext dbContext = await dbContextFactory.CreateDbContextAsync();

            Students = dbContext.Students.ToList();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        await js.InvokeAsync<string>("studentExport.init");

        //        if (!IsReload)
        //        {
        //            navigationManager.NavigateTo(navigationManager.Uri + "/true", forceLoad: true);
        //        }
        //    }
        //}

        private void SetSelectedStudent(Student student)
        {
            selectedStudent = student;
        }

        //This function exports all feedback to exel files.
        //The exported data in anonymized
        public async Task ExportAllFeedback()
        {
            ApplicationDbContext dbContext = dbContextFactory.CreateDbContext();

            List<StudentFeedback> feedbackdata = AnonomizeStudentFeedbackList(dbContext.StudentFeedbacks.ToList());

            List<TeacherFeedback> teacherFeedbacks = AnonomizeTeacherFeedbackList(dbContext.TeacherFeedbacks.ToList());

            byte[] studentFeedbackExcel = ExportToExcel<StudentFeedback>(feedbackdata, "studentFeedbackData");
            byte[] teacherFeeback = ExportToExcel<TeacherFeedback>(teacherFeedbacks, "teacherFeedbackData");

            await js.InvokeAsync<string>("saveAsFile", "StudentFeedbackData.xlsx", Convert.ToBase64String(studentFeedbackExcel));
            await js.InvokeAsync<string>("saveAsFile", "TeacherFeedbackData.xlsx", Convert.ToBase64String(teacherFeeback));
        }

        //This function anonomyzes a list of studentFeedback
        public List<StudentFeedback> AnonomizeStudentFeedbackList(List<StudentFeedback> studentFeedbacks)
        {
            foreach (StudentFeedback studentFeedback in studentFeedbacks)
            {
                studentFeedback.Student = null;
                studentFeedback.StudentId = null;
            }
            return studentFeedbacks;
        }

        //This function anonomyzes a list of teacherFeedback
        public List<TeacherFeedback> AnonomizeTeacherFeedbackList(List<TeacherFeedback> teacherFeedbacks)
        {
            foreach (TeacherFeedback teacher in teacherFeedbacks)
            {
                teacher.Student = null;
                teacher.StudentId = null;
            }
            return teacherFeedbacks;
        }

        //This function retrieves all data from a student. It then uses the ExporToExcel function to transform the lists of data to excel files. And then it uses a js script to download the files.
        public async Task ExportPersonalInfo(Student student)
        {
            ApplicationDbContext dbContext = dbContextFactory.CreateDbContext();

            List<StudentFeedback> feedbackdata = dbContext.StudentFeedbacks
            .Where(feedback => feedback.Student == student)
            .ToList();


            List<Student> studentdata = new List<Student>();
            studentdata.Add(student);

            List<TeacherFeedback> teacherFeedbacks = dbContext.TeacherFeedbacks
                .Where(feedback => feedback.Student == student)
                .ToList();

            byte[] studentFeedbackExcel = ExportToExcel<StudentFeedback>(feedbackdata, "studentFeedbackData");
            byte[] studentExcel = ExportToExcel<Student>(studentdata, "studentData");
            byte[] teacherFeeback = ExportToExcel<TeacherFeedback>(teacherFeedbacks, "teacherFeedbackData");

            await js.InvokeAsync<string>("saveAsFile", student.CustomerNumber + "_studentFeedbackData.xlsx", Convert.ToBase64String(studentFeedbackExcel));
            await js.InvokeAsync<string>("saveAsFile", student.CustomerNumber + "_studentData.xlsx", Convert.ToBase64String(studentExcel));
            await js.InvokeAsync<string>("saveAsFile", student.CustomerNumber + "_teacherFeedbackData.xlsx", Convert.ToBase64String(teacherFeeback));
        }

        //This function transforms a list of data to a excel file.
        private byte[] ExportToExcel<T>(List<T> data, string filename)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(filename);

                worksheet.Cells.LoadFromCollection(data, true);

                return package.GetAsByteArray();
            }
        }

        //This function anonymizes a student by setting their name and customer id to "" and 0 so that it cant be traced back to a student.
        public ResponseMessage<string> Anonymizedata(Student student)
        {
            var dbContext = dbContextFactory.CreateDbContext();

            var selectedStudent = dbContext.Students.Find(student.StudentId);

            if (selectedStudent == null)
            {
                return new ResponseMessage<string>(HttpStatusCode.BadRequest, "Student does not exist", "");
            }

            selectedStudent.Name = "";
            selectedStudent.CustomerNumber = 0;
            dbContext.SaveChanges();

            if (navigationManager != null) 
            {
                navigationManager.NavigateTo(navigationManager.Uri, forceLoad: true);
            }
            
            return new ResponseMessage<string>(HttpStatusCode.OK, "Data has been anonymized", "");
        }

    }
}