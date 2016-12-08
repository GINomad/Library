using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LibraryApp.ViewModels
{
  public  class RegisterModel
    {
        [HiddenInput]
        public int ID { set; get; }
        [Required]
        public string Name { set; get; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { set; get; }
    }
}
