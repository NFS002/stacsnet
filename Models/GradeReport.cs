using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel; 
using stacsnet.Util;

namespace stacsnet.Models
{
    public enum GradeType
    {
        Practical,
        Exam,
        Project,
        Final,
        Other
    }

    [Bind("Week","Type","Grade","descr","Year","code")] 
    public class GradeReport {

        [RequiredIf("Type", GradeType.Practical)]
        [DisplayName("Week: (only required for Practicals)")]
        [Range(0, 13)]
        public int? Week { get; set; }

        [Required(ErrorMessage = "Please select the grade type")]
        [EnumDataType(typeof(GradeType))]
        [DisplayName("Grade type: ")]
        public GradeType Type { get; set; }
       
        [Key]
        [ScaffoldColumn(false)]
        public string ID { get; set; }

        [Required(ErrorMessage = "Please enter your grade")]
        [DisplayName("Grade: ")]
        [Range(0, 20)]
        public int Grade { get; set; }

        [Required(ErrorMessage = "Please enter the module code")]
        [DisplayName("Module code: ")]
        [RegularExpression("^[A-Z]{2}[0-9]{4}$", ErrorMessage = "Please enter your module code in the correct format (LLNNNN) ")]
        public string code { get; set; }

        [DisplayName("Additional comments: ")]
        [DataType(DataType.Text)]
        public string descr { get; set; }


        [DisplayName("Year: ")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Please enter the year you received this grade. (YYYY) ")]
        public string Year { get; set; }

    }
}