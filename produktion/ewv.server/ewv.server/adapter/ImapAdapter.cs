﻿using System;
using System.Collections.Generic;
using System.Linq;
using ActiveUp.Net.Mail;
using ewv.server.kontrakt;

namespace ewv.server.adapter
{
    // Imap-API: http://mailsystem.codeplex.com/

    internal class ImapAdapter : IDisposable
    {
        private readonly KonfigurationAdapter _config;
        private Imap4Client _imap;

        public ImapAdapter(KonfigurationAdapter config)
        {
            _config = config;
        }


        public void Verbinden()
        {
            _imap = new Imap4Client();
            _imap.Connect(_config["mailserver_receive"]);
            _imap.Login(_config["mailserver_user"], _config["mailserver_password"]);
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
            var messages = mailbox.SearchParse("ALL").Cast<Message>().ToArray();
            return messages;
        }
        
        internal static IEnumerable<Message> Dubletten_aussieben(IEnumerable<Message> messages)
        {
            // Falls eine Nachricht mehrere Wiedervorlageadressen enthält, entstehen mehrere Kopien in der Inbox.
            // BCC-Empfänger sind nur in der ersten Kopie enthalten!
            var uniqueMessages = new Dictionary<string, Message>();
            foreach (var msg in messages) 
                if (!uniqueMessages.ContainsKey(msg.MessageId))
                    uniqueMessages.Add(msg.MessageId, msg);
            return uniqueMessages.Values;
        }

        internal IEnumerable<Email> Für_jede_Einplanungsadresse_auf_Email_mappen(IEnumerable<Message> messages)
        {
            return from msg in messages
                   from einplanungsadresse in Einplanungsemailadressen_sammeln(msg)
                   select new Email {
                       MessageId = msg.MessageId,
                       VersandzeitpunktUTC = msg.Date,
                       An = msg.To[0].Email,
                       AnWiedervorlage = einplanungsadresse,
                       Von = msg.From.Email,
                       Betreff = msg.Subject,
                       Text = msg.BodyText.Text
                   };
        }

        internal IEnumerable<string> Einplanungsemailadressen_sammeln(Message msg)
        {
            return msg.To
                      .Union(msg.Cc) 
                      .Union(msg.Bcc)
                      .Where(empfänger => empfänger.Email.ToLower().EndsWith("@" + _config["mailserver_domain"]))
                      .Select(empfänger => empfänger.Email);
        }


        public void Einplanungen_löschen(IEnumerable<string> messageIds)
        {
            var uids = Uids_der_zu_löschenden_Emails_sammeln(messageIds.ToArray());
            Emails_löschen(uids);
        }

        private IEnumerable<int> Uids_der_zu_löschenden_Emails_sammeln(string[] messageIds)
        {
            var mailbox = _imap.SelectMailbox("Inbox");
            var fetch = mailbox.Fetch;

            var uids = new List<int>();
            for (var i = 1; i <= mailbox.MessageCount; i++)
            {
                var msg = fetch.MessageObject(i);
                if (messageIds.Contains(msg.MessageId)) uids.Add(fetch.Uid(i));
            }
            return uids;
        }

        private void Emails_löschen(IEnumerable<int> uids)
        {
            var mailbox = _imap.SelectMailbox("Inbox");
            foreach(var uid in uids)
                mailbox.UidDeleteMessage(uid, true);
        }


        public void Dispose()
        {
            if (_imap == null) return;

            _imap.Close();
            _imap.Disconnect();
        }

    }
}