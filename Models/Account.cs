using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stacsnet.Models {

    [Bind("email, uname, pwhash")]
    public class Account {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int id { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [DisplayName("Email: ")]
        [RegularExpression("^[a-z0-9]+$", ErrorMessage = "Email address is invalid")]
        public string email { get; set;}

        [Required(ErrorMessage = "Please enter a username")]
        [DisplayName("Username: ")]
        [RegularExpression("^[a-z0-9]{2,6}$", ErrorMessage="Your username must be between 2 and 8 characters, and can only contain lower case alphanumeric characters")]
        public string uname { get; set; }

        [Required(ErrorMessage = "Please enter a password")]
        [DisplayName("Password: ")]
        [DataType(DataType.Password)]
        [RegularExpression("[^\\s]{4,}", ErrorMessage="Your password must be at least 4 characters, and can contain any characters other than whitespace.")]
        public string pwhash {get; set; }

        [ScaffoldColumn(false)]
        public string key { get; set; }

        [ScaffoldColumn(false)]
        public bool verified {get; set; }
    }
}