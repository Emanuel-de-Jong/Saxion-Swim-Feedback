using System.ComponentModel.DataAnnotations;

namespace Swim_Feedback.Data
{
    public class Student
    {
        [Key]
        public long StudentId { get; set; }
        public long? StudentImageId { get; set; }
        public StudentImage? StudentImage { get; set; }
        public long? SwimClassId { get; set; }
        public SwimClass? SwimClass { get; set; }
        public long? AvatarId { get; set; }
        public Avatar? Avatar { get; set; }
        public int CustomerNumber { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }

        public Student(int customerNumber, string name)
            : this(null, null, customerNumber, name) { }

        public Student(StudentImage studentImage, SwimClass swimClass, int customerNumber, string name)
        {
            StudentImage = studentImage;
            SwimClass = swimClass;
            CustomerNumber = customerNumber;
            Name = name;

            //Avatar = new Avatar();
        }
    }
}
