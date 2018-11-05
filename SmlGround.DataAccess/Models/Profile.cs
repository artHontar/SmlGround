using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DataAccess.Models
{
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Skype { get; set; }

        public User User { get; set; }
    }
}
