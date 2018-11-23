using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SmlGround.SMTP;

namespace SmlGround.DLL.Interfaces
{
    public interface IMessage
    {
        (string,string) FormMessage();
    }

    public class ConfirmEmail : IMessage
    {
        private string url; 
        public ConfirmEmail(string url)
        {
            this.url = url;
        }

        public (string,string) FormMessage()
        {
            string subject = "Подтверждение email";
            string messege = string.Format("Для завершения регистрации перейдите по ссылке:" +
                                           "<a href=\"{0}\" title=\"Подтвердить регистрацию\">{0}</a>",
                url);
            return (subject, messege);
        }
    }
}
