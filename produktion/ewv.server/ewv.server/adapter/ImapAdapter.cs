using System;
using System.Collections.Generic;
using System.Linq;
using ActiveUp.Net.Mail;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    internal class ImapAdapter : IDisposable
    {
        private readonly Imap4Client _imap;

        public ImapAdapter(KonfigurationAdapter config)
        {
            _imap = new Imap4Client();
            _imap.Connect(config["mailserver_receive"]);
            _imap.Login(config["mailserver_user"], config["mailserver_password"]);
        }


        public IEnumerable<Email> Einplanungen_abholen()
        {
            var mailbox = _imap.SelectMailbox("Inbox");
            var messages = mailbox.SearchParse("UNSEEN").Cast<Message>().ToArray();

            Console.WriteLine("Einplanungen abgeholt: {0}", messages.Count());

            return messages.Select(msg => new Email
            {
                An = msg.To[0].Email,
                Von = msg.From.Email,
                Betreff = msg.Subject,
                Text = msg.BodyText.Text
            }).ToArray();
        }


        public void Dispose()
        {
            _imap.Close();
            _imap.Disconnect();
        }
    }
}