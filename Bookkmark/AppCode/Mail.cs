using System.Net.Mail;
using System.IO;
using System.Configuration;

namespace Bookkmark
{
    public class Mail
    {
        private MailMessage MlMessage = new MailMessage();
        private string StrFromAdd;
        private string StrToAdd;
        private string StrCCAdds;
        private string StrSubject;
        private bool BlnIsBodyHtml;
        private string StrMailBody;
        private Attachment _attachment;
        private Attachment _SourceAttachment;


        public string FromAdd
        {
            get
            {
                return StrFromAdd;
            }
            set
            {
                StrFromAdd = value;
            }
        }
        public string ToAdd
        {
            get
            {
                return StrToAdd;
            }
            set
            {
                StrToAdd = value;
            }
        }
        public string CCAdds
        {
            get
            {
                return StrCCAdds;
            }
            set
            {
                StrCCAdds = value;
            }
        }
        public string Subject
        {
            get
            {
                return StrSubject;
            }
            set
            {
                StrSubject = value;
            }
        }
        public bool IsBodyHtml
        {
            get
            {
                return BlnIsBodyHtml;
            }
            set
            {
                BlnIsBodyHtml = value;
            }
        }
        public string Body
        {
            get
            {
                return StrMailBody;
            }
            set
            {
                StrMailBody = value;
            }
        }


        public Attachment FileAttachment
        {
            get
            {
                return _attachment;
            }
            set
            {
                _attachment = value;
            }
        }

        public Attachment SourceFileAttachment
        {
            get
            {
                return _SourceAttachment;
            }
            set
            {
                _SourceAttachment = value;
            }
        }

        public string SendMail()
        {
            string ErrDesc = "";
            MailAddress FrmMailAdd = new MailAddress(StrFromAdd);
            MlMessage.From = FrmMailAdd;
            MlMessage.To.Add(StrToAdd);
            if (!string.IsNullOrEmpty(StrCCAdds))
            {
                MlMessage.CC.Add(StrCCAdds);
            }
            MlMessage.Subject = StrSubject;
            MlMessage.IsBodyHtml = BlnIsBodyHtml;
            MlMessage.Body = StrMailBody;

            if (FileAttachment != null)
            {
                MlMessage.Attachments.Add(FileAttachment);
            }

            if (SourceFileAttachment != null)
            {
                MlMessage.Attachments.Add(SourceFileAttachment);
            }

            SmtpClient SMTPClnt = new SmtpClient("mail.codeanalyze.com", 25);
            //SMTPClnt.Host = "mail.codeanalyze.com";
            //SMTPClnt.Port = 26;
            //SMTPClnt.EnableSsl = false;
            //SMTPClnt.UseDefaultCredentials = false;
            //SMTPClnt.DeliveryMethod = SmtpDeliveryMethod.Network;
            SMTPClnt.Credentials = new System.Net.NetworkCredential("admin@codeanalyze.com", "Shamu@123");
            try
            {
                //if(!MlMessage.To.ToString().Contains("codeanalyze.com") &&
                //    !MlMessage.From.ToString().Contains("codeanalyze.com"))
                SMTPClnt.Send(MlMessage);

            }
            catch (System.Exception ex)
            {
                // throw ex;
            }
            return ErrDesc;
        }
    }
}