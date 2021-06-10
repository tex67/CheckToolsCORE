
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CheckToolsCORE.Models
{
    public class CheckListViewModel
    {
       
        public string CodiceArticolo { get; set; }
        
        public string Pn { get; set; }
        
        [Required]
        [Display(Name = "Data Documento")]        
        public DateTime DataDocumento { get; set; }
       
        [Required(ErrorMessage = "campo obbligatorio")]
        public string  Riferimento { get; set; }
    
    }
}