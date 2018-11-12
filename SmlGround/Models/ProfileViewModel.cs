using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmlGround.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Skype { get; set; }

    }
}