using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmlGround.Models
{
    public class ProfileViewModel
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Avatar { get; set; }
        [Display(Name = "День рождение")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        [Display(Name = "Город")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Display(Name = "Место учёбы")]
        [DataType(DataType.Text)]
        public string PlaceOfStudy { get; set; }
        [DataType(DataType.Text)]
        public string Skype { get; set; }

        public Boolean IsCurrentUserProfile { get; set; }
    }
}