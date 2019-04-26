using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace stacsnet.Models {

    [Bind("Module, Year, Folder")]
    public class ModuleEntry {

        [DisplayName( "Module code: ") ]
        [Required( ErrorMessage = "Module code is required")]
        [RegularExpression("^[A-Z]{2}[1-9][0-9]{4}$", ErrorMessage = "Module code is invalid")]
        public string Module { get; set; }

        [DisplayName( "Year: ") ]
        [Required( ErrorMessage = "Year is required")]
        [RegularExpression("^(19|20)[0-9]{2}$", ErrorMessage = "Year is invalid")]
        public string Year { get; set; }

        [DisplayName( "Folder: ") ]
        [Required( ErrorMessage = "Folder is required")]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} character")]
        [StringLength(10, ErrorMessage = "{0} cannot exceed {1} characters")]
        public string Folder { get; set; }
    }
}