using System;
using System.Collections;
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
            var messages = Nachrichten_abholen();
            messages = Dubletten_aussieben(messages);
            return Für_jede_Einplanungsadresse_auf_Email_mappen(messages);
        }

        private IEnumerable<Message> Nachrichten_abholen()
        {
            var mailbox = _imap.SelectMailbox("Inbox");
            var messages = mailbox.SearchParse("UNSEEN").Cast<Message>().ToArray();
            return messages;
        }
        
        private static IEnumerable<Message> Dubletten_aussieben(IEnumerable<Message> messages)
        {
            // Falls eine Nachricht mehrere Wiedervorlageadressen enthält, entstehen mehrere Kopien in der Inbox.
            // BCC-Empfänger sind nur in der ersten Kopie enthalten!
            var uniqueMessages = new Dictionary<string, Message>();
            foreach (var msg in messages) 
                if (!uniqueMessages.ContainsKey(msg.MessageId))
                    uniqueMessages.Add(msg.MessageId, msg);
            return uniqueMessages.Values;
        }

        private IEnumerable<Email> Für_jede_Einplanungsadresse_auf_Email_mappen(IEnumerable<Message> messages)
        {
            return from msg in messages
                   from einplanungsadresse in Einplanungsemailadressen_sammeln(msg)
                   select new Email {
                       An = einplanungsadresse,
                       Von = msg.From.Email,
                       Betreff = msg.Subject,
                       Text = msg.BodyText.Text
                   };
        }

        private IEnumerable<string> Einplanungsemailadressen_sammeln(Message msg)
        {
            return msg.To
                      .Union(msg.Cc)
                      .Union(msg.Bcc)
                      .Where(empfänger => empfänger.Email.ToLower().EndsWith("@emailwiedervorlage.de"))
                      .Select(empfänger => empfänger.Email);
        }


        public void Dispose()
        {
            _imap.Close();
            _imap.Disconnect();
        }
    }
}