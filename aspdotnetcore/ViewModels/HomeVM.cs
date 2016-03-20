using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.ViewModels
{
    public class HomeVM
    {
        [Display(Name = "DateTimeLabel")]
        public string DateTime { get; set; }

        [Display(Name = "NameLabel")]
        public string Name { get; set; }
    }
}
