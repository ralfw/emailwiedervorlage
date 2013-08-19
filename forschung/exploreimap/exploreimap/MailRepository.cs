using System.Collections.Generic;
using System.Linq;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Security;

namespace exploreimap
{
    public class MailRepository
    {
        public MailRepository(string mailServer, int port, bool useSSL, string username, string password)
        {
            if (useSSL)
                Client.ConnectSsl(mailServer, port);
            else
                Client.Connect(mailServer, port);
            Client.Login(username, password);
        }
 

        public IEnumerable<Message> GetAllMails(string mailBox)
        {
            return GetMails(mailBox, "ALL").Cast<Message>();
        }
 
        public IEnumerable<Message> GetUnreadMails(string mailBox)
        {
            return GetMails(mailBox, "UNSEEN").Cast<Message>();
        }
 
        private MessageCollection GetMails(string mailBox, string searchPhrase)
        {
            var mails = Client.SelectMailbox(mailBox);
            var messages = mails.SearchParse(searchPhrase);
            return messages;
        }


        public void DeleteMail(int index)
        {
            var mails = Client.SelectMailbox("Inbox");
            var fetch = mails.Fetch;
            var m = fetch.MessageObject(1);
            //var messages = mails.SearchParse("ALL");
            mails.DeleteMessage(index, true);
        }

        
        private Imap4Client _client = null;
        private Imap4Client Client
        {
            get { return _client ?? (_client = new Imap4Client()); }
        }
    }
}