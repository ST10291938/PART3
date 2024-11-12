using System.ComponentModel.DataAnnotations;

namespace PART3.Models
{
    public class Lecturer
    {
        [Key]
        public int Id { get; set; }

        public string lecture_ID { get; set; }
        public string lecturer_Name { get; set; }

        public string lecturer_Surname { get; set; }

        public string lecturer_FullName => $"{lecturer_Name} {lecturer_Surname}";
        public string lecturer_Email { get; set; }

        public string lecturer_Contact { get; set; }

        public string Program { get; set; }

        public string Module_Code { get; set; }

        public int Hours_Worked { get; set; } 

        public int Hourly_Rate { get; set; }
        public DateTime Date_Of_Session { get; set; }
        // public IFormFile UploadedFile { get; set; }
        public string UploadedFileName { get; set; }

        public string? Status { get; set; }
    }
}
