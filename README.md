# Automatische Email-Wiedervorlage

Service zur automatischen Wiedervorlage von Emails à la [followup.cc](http://followup.cc).

Wer eine Email nach einer bestimmten Zeitspanne (Stunden, Tage, Wochen…) durch eine Erinnerungsemail in seinem Postfach wieder vorgelegt bekommen möchte, der schickt sie sich auf BCC an eine Wiedervorlage-Adresse, z.B. in10tagen@wiedervorlage.cc [1].

In diesem Repo soll über die Zeit alles zusammenkommen, was man braucht, um so einen Dienst selbst mit geringem Aufwand zu betreiben.

Für den persönlichen Gebrauch sollen nicht mehr Kosten als die für einen Domänennamen anfallen. Der Betrieb soll lokal möglich sein, d.h. auf heimischen Rechnern, oder im Rahmen kostenloser Kontingente von Cloud-Infrastrukturanbietern.

Mehr Informationen [im Wiki des Repo](https://github.com/ralfw/emailwiedervorlage/wiki).

### Endnoten
[1] Hier und im Weiteren ist die Annahme, dass zum Service eine solche Domäne wie wiedervorlage.cc gehört. Für sie muss ein catch-all Email-Postfach existieren, an das Emails mit ganz unterschiedlichen Benutzernamen (z.B. in10tagen@ oder in3monaten@) geschickt werden können und auf das Service-Bestandteile per IMAP Zugriff haben.
