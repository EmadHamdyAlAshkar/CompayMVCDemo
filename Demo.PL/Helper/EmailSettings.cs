﻿using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("emadh732@gmail.com", "ghrptxocxpdqdcan");

            client.Send("emadh732@gmail.com", email.To, email.Title, email.Body);
        }
    }
}
