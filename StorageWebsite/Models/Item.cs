using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StorageWebsite.Models
{
    public class Item
    {
        public int ID { get; set; }

        [Display(Name = "Namn")]
        [Required]
        [StringLength(60, MinimumLength =3)]
        public string Name { get; set; }

        [Display(Name = "Kategori")]
        [Required]
        public string Category { get; set; }


        [Display(Name = "I lager")]
        public bool Instorage { get; set; }

        [Display(Name = "Pris")]
        [Required]
        public int Price { get; set; }

        [Display(Name = "Skapat")]
        public DateTime Added { get; set; } = DateTime.Now;
    }
}
