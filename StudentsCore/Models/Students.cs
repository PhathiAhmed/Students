using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentsCore.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required]
        [MaxLength(200)]
        [Display(Name = " student's Name")]
        public string Name { get; set; }
        [Required]

        [Display(Name = " student's Age")]
        public int Age { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = " student's Date Of Birth")]
        public DateTime DateBirth { get; set; }
        [Required]
        [Display(Name = " student's Gender")]
        public Gender Sex { get; set; }
        [Required]
        [MaxLength(300)]
        [Display(Name = " student's Adress")]
        public string Address { get; set; }

        [Display(Name = " student's Image")]
        public string Image { get; set; }
        //[NotMapped]
        //public IFormFile File { get; set; }

    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
