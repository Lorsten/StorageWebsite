using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StorageWebsite.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required]
        [StringLength(40, MinimumLength =3)]
        [Display(Name = "Användarnam")]
        public string Name { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}
