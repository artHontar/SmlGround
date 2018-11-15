using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmlGround.DLL.Service
{
    public class BinaryFormater
    {
        private string uploadImage;
        public BinaryFormater(string uploadImage)
        {
            this.uploadImage = uploadImage;
        }
        public byte[] Convert()
        {
            return System.Text.Encoding.Unicode.GetBytes(uploadImage);
        }
    }
}
