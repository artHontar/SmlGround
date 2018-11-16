using System.IO;
using System.Web;

namespace SmlGround.DLL.Service
{
    public class CastBinaryFormatter
    {
        private HttpPostedFileBase uploadImage;

        public CastBinaryFormatter(HttpPostedFileBase uploadImage)
        {
            this.uploadImage = uploadImage;
        }

        public byte[] Convert()
        {
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(uploadImage.InputStream))
            {
                imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
            }

            return imageData;
        }
    }
}
