using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SmlGround.Models
{
    public class EditProfileViewModel
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Display(Name = "День рождение")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
        [Display(Name = "Город")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Display(Name = "Место учёбы")]
        [DataType(DataType.Text)]
        public string PlaceOfStudy { get; set; }
        [DataType(DataType.Text)]
        public string Skype { get; set; }
        
    }
}