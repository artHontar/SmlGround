using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enum;

namespace SmlGround.DLL.DTO
{
    public class ProfileDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public string City { get; set; }
        public string PlaceOfStudy { get; set; }
        public string Skype { get; set; }
        public FriendStatus FriendFlag { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (!(obj is ProfileDTO))
                throw new ArgumentException("obj is not an ProfileDTO");
            var usr = obj as ProfileDTO;

            return Id.Equals(usr.Id);
        }
    }
}
