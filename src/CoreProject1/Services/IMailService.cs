using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject1.Services
{
    public interface IMailService
    {
        void SendEmail(string to, string from, string subject, string body);
    }
}
