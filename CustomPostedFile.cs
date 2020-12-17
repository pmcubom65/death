using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Clases.SmartChat
{
    public class CustomPostedFile : System.Web.HttpPostedFileBase
    {
        private readonly byte[] fileBytes;
        MemoryStream stream;

        public CustomPostedFile(byte[] fileBytes, string filename = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = filename;
            this.ContentType = "image/png";
            this.stream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;
        public override string FileName { get; }
        public override Stream InputStream
        {
            get { return stream; }
        }
        public override string ContentType { get; }

        public override void SaveAs(string filename)
        {
            using (var file = File.Open(filename, FileMode.CreateNew))
                stream.WriteTo(file);
        }
    }
}
