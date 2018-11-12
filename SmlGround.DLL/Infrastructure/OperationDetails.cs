using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmlGround.DLL.Infrastructure
{
    public class OperationDetails
    {
        public OperationDetails(bool succeed, string message, string prop)
        {
            Succeed = succeed;
            Message = message;
            Property = prop;
        }

        public Boolean Succeed { get; private set; }
        public String Message { get; private set; }
        public String Property { get; set; }
    }
}
