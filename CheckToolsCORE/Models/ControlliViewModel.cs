using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckToolsCORE.Models
{
    public class ControlliViewModel
    {
       
        public string CodiceArticolo { get; set; }
        
        public string Pn { get; set; }
        
        [Required]
        [Display(Name = "Data Documento")]        
        public DateTime DataControllo { get; set; }
       
       
        public int IdCheckList { get; set; }

        public string RifChecklist { get; set; }

        public string DataChecklist { get; set; }


    }
}