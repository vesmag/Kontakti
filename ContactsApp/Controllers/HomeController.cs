using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactsApp.Models;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace ContactsApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //klasa koja cuva sve informacije o jednom kontaktu: ime, prezime, adresu, emailove, telefonske brojeve i tagove
        public class Contact_Info
        {
            //konstruktori
            public Contact_Info () {}

            public Contact_Info (string name, string surname, string address, List<Email> emails, List<Telephone> telephones, List<Tag> tags)
            {
                Name = name;
                Surname = surname;
                Address = address;
                Emails = emails;
                Telephones = telephones;
                Tags = tags;
            }
            
            public int Id;
            public string Name;
            public string Surname;
            public string Address;
            public List<Email> Emails = new List<Email>();
            public List<Telephone> Telephones = new List<Telephone>();
            public List<Tag> Tags = new List<Tag>();
        }

        //vraca iz baze sve informacije o kontaktu sa zadanim id-em u obliku JSONa
        public JsonResult getContactById(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                //stvara se i puni temp, instanca klase Contact_Info
                Contact_Info temp = new Contact_Info();

                //id, ime, prezime i adresu vade se iz tablice "Contacts"
                var tempContact = contactsData.Contacts.Where(x => x.Id == ConId).Single();
                temp.Id = ConId;
                temp.Name = tempContact.Name;
                temp.Surname = tempContact.Surname;
                temp.Address = tempContact.Address;

                //lista emailova/telefonskih brojeva/tagova
                //(instanci klase Email/Telephone/Tag koja sadrzi id osobe, id unosa i sam email/broj/tag)
                //dohvaca se iz tablice "Emails"/"Telephones"/"Tags"
                var tempEmail = contactsData.Emails.Where(x => x.PersonId == ConId).ToList();
                foreach (var mail in tempEmail)
                {
                    temp.Emails.Add(mail);
                }

                var tempTelephone = contactsData.Telephones.Where(x => x.PersonId == ConId).ToList();
                foreach (var tel in tempTelephone)
                {
                    temp.Telephones.Add(tel);
                }

                var tempTag = contactsData.Tags.Where(x => x.PersonId == ConId).ToList();
                foreach (var tag in tempTag)
                {
                    temp.Tags.Add(tag);
                }

                //vracanje popunjene instance klase "Contact_Info"
                return Json(temp, JsonRequestBehavior.AllowGet);

            }

        }

        //vraca iz baze sve informacije o kontaktu sa zadanim id-em u obliku instance Contact_Info klase
        public Contact_Info getContactInfoById(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                //stvara se i puni temp, instanca klase Contact_Info
                Contact_Info temp = new Contact_Info();

                //id, ime, prezime i adresu vade se iz tablice "Contacts"
                var tempContact = contactsData.Contacts.Where(x => x.Id == ConId).Single();
                temp.Id = ConId;
                temp.Name = tempContact.Name;
                temp.Surname = tempContact.Surname;
                temp.Address = tempContact.Address;

                //lista emailova/telefonskih brojeva/tagova
                //(instanci klase Email/Telephone/Tag koja sadrzi id osobe, id unosa i sam email/broj/tag)
                //dohvaca se iz tablice "Emails"/"Telephones"/"Tags"
                var tempEmail = contactsData.Emails.Where(x => x.PersonId == ConId).ToList();
                foreach (var mail in tempEmail)
                {
                    temp.Emails.Add(mail);
                }

                var tempTelephone = contactsData.Telephones.Where(x => x.PersonId == ConId).ToList();
                foreach (var tel in tempTelephone)
                {
                    temp.Telephones.Add(tel);
                }

                var tempTag = contactsData.Tags.Where(x => x.PersonId == ConId).ToList();
                foreach (var tag in tempTag)
                {
                    temp.Tags.Add(tag);
                }

                //vracanje popunjene instance klase "Contact_Info"
                return temp;

            }

        }

        //vraca listu svih kontakata iz baze
        public JsonResult getAll()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                List<Contact_Info> completeList = new List<Contact_Info>();
                var contacts = contactsData.Contacts;
                var emails = contactsData.Emails;
                var telephones = contactsData.Telephones;
                var tags = contactsData.Tags;

                //sintaksa f-je GroupJoin:
                //Outer.GroupJoin(Inner, outer => key, inner => key, (outer, inner) => result)

                //prvi join radi se na contacts-emails tablici
                var contactList = contacts.GroupJoin(emails,
                    contact => contact.Id,
                    email => email.PersonId,
                    (contact, email) => new
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Address = contact.Address,
                        Email = email
                    });

                //u drugom joinu na contacts-emails dodajemo i telephones
                var contactList2 = contactList.GroupJoin(telephones,
                    contact => contact.Id,
                    telephone => telephone.PersonId,
                    (contact, telephone) => new
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Address = contact.Address,
                        Email = contact.Email,
                        Telephone = telephone
                    });

                //na kraju, treci join stvara contacts-emails-telephones-tags
                var contactList3 = contactList2.GroupJoin(tags,
                    contact => contact.Id,
                    tag => tag.PersonId,
                    (contact, tag) => new
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Surname = contact.Surname,
                        Address = contact.Address,
                        Email = contact.Email,
                        Telephone = contact.Telephone,
                        Tag = tag
                    });

                //prebacivanje iz contactList3 u completeList
                foreach(var contact in contactList3)
                {
                    Contact_Info temp = new Contact_Info();
                    temp.Id = contact.Id;
                    temp.Name = contact.Name;
                    temp.Surname = contact.Surname;
                    temp.Address = contact.Address;
                    foreach (var em in contact.Email)
                    {
                        temp.Emails.Add(em);
                    }
                    foreach (var tel in contact.Telephone)
                    {
                        temp.Telephones.Add(tel);
                    }
                    foreach (var tag in contact.Tag)
                    {
                        temp.Tags.Add(tag);
                    }
                    completeList.Add(temp);
                }

                return Json(completeList, JsonRequestBehavior.AllowGet);                
            }
        }

        //dodavnja osnovnih informacija kontakta (ime, prezime, adresa) u Contacts tablicu
        public int addContact (Contact Con)
        {
            if (Con != null)
            {
                using (ContactsDBEntities contactsData = new ContactsDBEntities())
                {
                    contactsData.Contacts.Add(Con);
                    contactsData.SaveChanges();
                }
                return Con.Id;
            }
            else
            {
                return 0;
            }
        }

        //funkcija dodaje novi kontakt u bazu, sa svim podacima (ime, prezime, adresa, emailovi, tel.brojevi, tagovi)
        public int addFullContact(Contact_Info Con)
        {
            if (Con != null)
            {
                using (ContactsDBEntities contactsData = new ContactsDBEntities())
                {
                    //prvo se dodaju osnovni podaci (ime, prezime i adresa) u tablicu "Contacts"
                    Contact ConBasic = new Contact(Con.Name, Con.Surname, Con.Address);
                    contactsData.Contacts.Add(ConBasic);
                    contactsData.SaveChanges();

                    //s dobivenim id-em osobe, dodaju se njeni mailovi, telefonski brojevi i tagovi,
                    //sve u svoje tablice
                    foreach (var em in Con.Emails)
                    {
                        Email Em = new Email(ConBasic.Id, em.Email1);
                        addEmail(Em);
                    }
                    foreach (var tel in Con.Telephones)
                    {
                        Telephone Tel = new Telephone(ConBasic.Id, tel.Telephone1);
                        addTelephone(Tel);
                    }
                    foreach (var tag in Con.Tags)
                    {
                        Tag Tag = new Tag(ConBasic.Id, tag.Tag1);
                        addTag(Tag);
                    }
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        //dodavanje emaila u bazu
        public int addEmail(Email Em)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Emails.Add(Em);
                contactsData.SaveChanges();
                return Em.EntryId;
            }
        }

        //dodavanje telefonskog broja u bazu
        public int addTelephone (Telephone Tel)
        { 
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Telephones.Add(Tel);
                contactsData.SaveChanges();
                return Tel.EntryId;
            }
        }

        //dodavanje taga u bazu
        public int addTag (Tag Tag)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Tags.Add(Tag);
                contactsData.SaveChanges();
                return Tag.EntryId;
            }
        }

        //osobi s id-em Id ime postavljamo na novodobiveni string Name
        public string updateName (int Id, string Name)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var toBeUpdated = contactsData.Contacts.Where(x => x.Id == Id).Single();
                toBeUpdated.Name = Name;
                contactsData.SaveChanges();
                return "Name updated.";
            }
        }

        //analogno fji updateName
        public string updateSurname(int Id, string Surname)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                Debug.WriteLine(Id);
                Debug.WriteLine(Surname);
                var toBeUpdated = contactsData.Contacts.Where(x => x.Id == Id).Single();
                toBeUpdated.Surname = Surname;
                contactsData.SaveChanges();
                return "Surname updated.";
            }
        }

        //analogno fji updateName
        public string updateAddress(int Id, string Address)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                Debug.WriteLine(Id);
                Debug.WriteLine(Address);
                var toBeUpdated = contactsData.Contacts.Where(x => x.Id == Id).Single();
                toBeUpdated.Address = Address;
                contactsData.SaveChanges();
                return "Address updated.";
            }
        }

        //funckcije primaju instancu klase Email/Telephone/Tag i po EntryIdu nalaze redak tablice koji treba osvjeziti
        //te stari unos zamijenjuju novodobivenim
        public string updateEmail (Email Em)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var toBeUpdated = contactsData.Emails.Where(x => x.EntryId == Em.EntryId).Single();
                toBeUpdated.Email1 = Em.Email1;
                contactsData.SaveChanges();
                return "Email updated.";
            }
        }
                   
        public string updateTelephone(Telephone Tel)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var toBeUpdated = contactsData.Telephones.Where(x => x.EntryId == Tel.EntryId).Single();
                toBeUpdated.Telephone1 = Tel.Telephone1;
                contactsData.SaveChanges();
                return "Telephone updated.";
            }
        }

        public string updateTag(Tag Tag)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var toBeUpdated = contactsData.Tags.Where(x => x.EntryId == Tag.EntryId).Single();
                toBeUpdated.Tag1 = Tag.Tag1;
                contactsData.SaveChanges();
                return "Tag updated.";
            }
        }
    

        //brisanje kontakta iz baze
        public string deleteContact(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var mailsById = contactsData.Emails.Where(x => x.PersonId == ConId).ToList();
                foreach (var mail in mailsById)
                {
                    contactsData.Emails.Remove(mail);
                }
                var telsById = contactsData.Telephones.Where(x => x.PersonId == ConId).ToList();
                foreach (var tel in telsById)
                {
                    contactsData.Telephones.Remove(tel);
                }
                var tagsById = contactsData.Tags.Where(x => x.PersonId == ConId).ToList();
                foreach (var tag in tagsById)
                {
                    contactsData.Tags.Remove(tag);
                }
                var contactById = contactsData.Contacts.Where(x => x.Id == ConId).Single();
                if (contactById != null)
                {
                    contactsData.Contacts.Remove(contactById);
                    contactsData.SaveChanges();
                    return "Contact deleted.";
                }
                else
                {
                    return "Contact not in database";
                }
            }
        }

        //fje za brisanje emaila/telefonskog broja/taga iz baze
        public string deleteEmail (Email Em)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var emailById = contactsData.Emails.Where(x => x.EntryId == Em.EntryId).Single();
                if (emailById != null)
                {
                    contactsData.Emails.Remove(emailById);
                    contactsData.SaveChanges();
                    return "Email deleted";
                }
                else
                {
                    return "Email not in database.";
                }
            }
        }

        public string deleteTelephone(Telephone Tel)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var telephoneById = contactsData.Telephones.Where(x => x.EntryId == Tel.EntryId).Single();
                if (telephoneById != null)
                {
                    contactsData.Telephones.Remove(telephoneById);
                    contactsData.SaveChanges();
                    return "Telephone deleted";
                }
                else
                {
                    return "Telephone not in database.";
                }
            }
        }

        public string deleteTag(Tag Tag)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var tagById = contactsData.Tags.Where(x => x.EntryId == Tag.EntryId).Single();
                if (tagById != null)
                {
                    contactsData.Tags.Remove(tagById);
                    contactsData.SaveChanges();
                    return "Tag deleted";
                }
                else
                {
                    return "Tag not in database.";
                }
            }
        }

        //trazi kontakte po imenu, prezimenu i tagovima te vracaju filtriranu listu
        //onih kontakata koji kao substring imena/prezimena/tagova imaju searchString
        public JsonResult search(string searchString)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                List<Contact_Info> filteredList = new List<Contact_Info>();

                //prvo gledamo u tablicu "Contacts" i vadimo sve unose koji kao substring imena ili prezimena
                //imaju searchString
                var filteredList1 = contactsData.Contacts.Where(x => x.Name.IndexOf(searchString) > -1).ToList();
                var filteredList2 = contactsData.Contacts.Where(x => x.Surname.IndexOf(searchString) > -1).ToList();
                filteredList1.AddRange(filteredList2);

                //za svaki takav naden unos u tablici funkcijom getContactInfoById u listu filteredList
                //unosimo Contact_Info tog kontakta
                foreach (var contact in filteredList1)
                {
                    filteredList.Add(getContactInfoById(contact.Id));
                }

                //istu stvar radimo na tablici "Tags"
                var filteredList3 = contactsData.Tags.Where(x => x.Tag1.IndexOf(searchString) > -1).ToList();
                foreach (var tag in filteredList3)
                {
                    filteredList.Add(getContactInfoById(tag.PersonId));
                }

                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
        }
        
        //funkcija popunjava bazu
        //ne koristi se, potrebna je samo za pocetno popunjavanje baze (pristup unosom URL-a /Home/fillDB)
       public string fillDB()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contacts]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Emails]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Telephones]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Tags]");
            }

            Contact Con1 = new Contact("Ana", "Anić", "Amruševa 99");
            int Id1 = addContact(Con1);
            Email Email11 = new Email(Id1, "a.anic@yahoo.com");
            Email Email12 = new Email(Id1, "ana.an@gmail.com");
            Telephone Telephone11 = new Telephone(Id1, "098/9790-858");
            Telephone Telephone12 = new Telephone(Id1, "01/2990-512");
            Tag Tag1 = new Tag(Id1, "srednja");

            addEmail(Email11);
            addEmail(Email11);
            addTelephone(Telephone11);
            addTelephone(Telephone12);
            addTag(Tag1);

            Contact Con2 = new Contact("Berislav", "Borić", "Borska 12");
            int Id2 = addContact(Con2);
            Email Email21 = new Email(Id2, "bboric@hotmail.com");
            Email Email22 = new Email(Id2, "be.bo@gmail.com");
            Telephone Telephone21 = new Telephone(Id2, "091/5525-552");
            Telephone Telephone22 = new Telephone(Id2, "01/4741-444");
            Tag Tag2 = new Tag(Id2, "posao_ekipa");

            addEmail(Email21);
            addEmail(Email22);
            addTelephone(Telephone21);
            addTelephone(Telephone22);
            addTag(Tag2);

            Contact Con3 = new Contact("Cvijeta", "Cavrić", "Ulica ciklama 52");
            int Id3 = addContact(Con3);
            Email Email31 = new Email(Id3, "cvijeta123@gmail.com");
            Email Email32 = new Email(Id3, "ccavric@pmf.hr");
            Telephone Telephone31 = new Telephone(Id3, "095/833-9077");
            Telephone Telephone32 = new Telephone(Id3, "091/5082-190");
            Telephone Telephone33 = new Telephone(Id3, "01/7172-121");
            Tag Tag3 = new Tag(Id3, "faks");

            addEmail(Email31);
            addEmail(Email32);
            addTelephone(Telephone31);
            addTelephone(Telephone32);
            addTelephone(Telephone33);
            addTag(Tag3);

            Contact Con4 = new Contact("Doris", "Denić", "Dobri dol 18");
            int Id4 = addContact(Con4);
            Email Email41 = new Email(Id4, "dorka_d@gmail.com");
            Telephone Telephone41 = new Telephone(Id4, "099/2716-520");
            Tag Tag4 = new Tag(Id4, "obitelj");

            addEmail(Email41);
            addTelephone(Telephone41);
            addTag(Tag4);

            Contact Con5 = new Contact("Ela", "Elezović", "Eugena Kvaternika 11");
            int Id5 = addContact(Con5);
            Email Email51 = new Email(Id5, "ela_elez@gmail.com");
            Email Email52 = new Email(Id5, "elita@hotmail.com");
            Telephone Telephone51 = new Telephone(Id5, "091/5534-551");
            Tag Tag5 = new Tag(Id5, "srednja");

            addEmail(Email51);
            addEmail(Email52);
            addTelephone(Telephone51);
            addTag(Tag3);

            Contact Con6 = new Contact("Franko", "Favrić", "Francuske revolucije 8, Split");
            int Id6 = addContact(Con6);
            Email Email6 = new Email(Id6, "franko.favric@tvrtka.hr");
            Telephone Telephone6 = new Telephone(Id6, "099/1986-111");
            Tag Tag6 = new Tag(Id6, "posao_kontakti");

            addEmail(Email6);
            addTelephone(Telephone6);
            addTag(Tag6);

            Contact Con7 = new Contact("Goran", "Golubić", "Grižanska 19");
            int Id7 = addContact(Con7);
            Email Email71 = new Email(Id7, "goran.gogs@gmail.com");
            Telephone Telephone71 = new Telephone(Id7, "095 76 848 76");
            Telephone Telephone72 = new Telephone(Id7, "01 2987 187");
            Tag Tag7 = new Tag(Id7, "osnovna");

            addEmail(Email71);
            addTelephone(Telephone71);
            addTelephone(Telephone72);
            addTag(Tag7);

            Contact Con8 = new Contact("Hrvoje", "Hrenović", "Heinzelova 21");
            int Id8 = addContact(Con8);
            Email Email81 = new Email(Id8, "hrvoje.hrenovic@tvrtka.com");
            Email Email82 = new Email(Id8, "marketing@tvrtka.com");
            Telephone Telephone81 = new Telephone(Id8, "091 1234 987");
            Telephone Telephone82 = new Telephone(Id8, "01 4660 555");
            Tag Tag8 = new Tag(Id8, "posao_kontakti");

            addEmail(Email81);
            addEmail(Email82);
            addTelephone(Telephone81);
            addTelephone(Telephone82);
            addTag(Tag8);

            Contact Con9 = new Contact("Irena", "Iljazović", "Iblerov trg 8");
            int Id9 = addContact(Con9);
            Email Email91 = new Email(Id9, "iiljaz.math@pmf.hr");
            Email Email92 = new Email(Id9, "irena.iljazovic@gmail.com");
            Telephone Telephone91 = new Telephone(Id9, "092 881 9921");
            Tag Tag9 = new Tag(Id9, "faks");

            addEmail(Email91);
            addEmail(Email92);
            addTelephone(Telephone91);
            addTag(Tag9);

            Contact Con10 = new Contact("Janja", "Jakić", "Josipa Hamma 12");
            int Id10 = addContact(Con10);
            Telephone Telephone10 = new Telephone(Id10, "098 8128 818");
            Tag Tag10 = new Tag(Id10, "osnovna");
            
            addTelephone(Telephone10);
            addTag(Tag10);

            return "OK";
        }
    }
}