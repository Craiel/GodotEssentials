namespace Craiel.Essentials.Runtime.Utils;

using System.IO;
using System.Text;

public class UTF8StringWriter : StringWriter
{
    public override Encoding Encoding
    {
        get
        {
            return Encoding.UTF8;
        }
    }
}
