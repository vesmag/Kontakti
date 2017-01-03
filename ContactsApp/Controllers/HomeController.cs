using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactsApp.Models;
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

        public class Contact_Info
        {
            public Contact_Info ()
            {

            }

            public Contact_Info (string name, string surname, string address, List<string> emails, List<string> telephones, List<string> tags)
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
            public List<string> Emails = new List<string>();
            public List<string> Telephones = new List<string>();
            public List<string> Tags = new List<string>();
        }

        //vraca iz baze kontakt sa zadanim id-em
        public JsonResult getContactById(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                Contact_Info temp = new Contact_Info();
                var tempContact = contactsData.Contacts.Where(x => x.Id == ConId).Single();
                temp.Id = ConId;
                temp.Name = tempContact.Name;
                temp.Surname = tempContact.Surname;
                temp.Address = tempContact.Address;
                var tempEmail = contactsData.Emails.Where(x => x.PersonId == ConId).ToList();
                foreach (var mail in tempEmail)
                {
                    temp.Emails.Add(mail.Email1);
                }
                var tempTelephone = contactsData.Telephones.Where(x => x.PersonId == ConId).ToList();
                foreach (var tel in tempTelephone)
                {
                    temp.Telephones.Add(tel.Telephone1);
                }
                var tempTag = contactsData.Tags.Where(x => x.PersonId == ConId).ToList();
                foreach (var tag in tempTag)
                {
                    temp.Tags.Add(tag.Tag1);
                }
                return Json(temp, JsonRequestBehavior.AllowGet);

            }

        }

        //vraca listu svih kontakata iz baze
        public JsonResult getAll()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                List<Contact_Info> lista = new List<Contact_Info>();
                var contacts = contactsData.Contacts;
                var emails = contactsData.Emails;
                var telephones = contactsData.Telephones;
                var tags = contactsData.Tags;
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
                Debug.WriteLine(contactList3.Count());
                foreach(var contact in contactList3)
                {
                    Contact_Info temp = new Contact_Info();
                    temp.Id = contact.Id;
                    temp.Name = contact.Name;
                    temp.Surname = contact.Surname;
                    temp.Address = contact.Address;
                    foreach (var em in contact.Email)
                    {
                        temp.Emails.Add(em.Email1);
                    }
                    foreach (var tel in contact.Telephone)
                    {
                        temp.Telephones.Add(tel.Telephone1);
                    }
                    foreach (var tag in contact.Tag)
                    {
                        temp.Tags.Add(tag.Tag1);
                    }
                    lista.Add(temp);
                }
                return Json(lista, JsonRequestBehavior.AllowGet);
                
            }
        }

        public int addContact (Contact Con)
        {
            if (Con != null)
            {
                using (ContactsDBEntities contactsData = new ContactsDBEntities())
                {
                    contactsData.Contacts.Add(Con);
                    contactsData.SaveChanges();
                }
                return 1;
            }
            else
            {
                return 0;
            }
        }

        //funkcija dodaje kontakt u bazu
        public int addFullContact(Contact_Info Con)
        {
            if (Con != null)
            {
                using (ContactsDBEntities contactsData = new ContactsDBEntities())
                {
                    Contact ConBasic = new Contact(Con.Name, Con.Surname, Con.Address);
                    contactsData.Contacts.Add(ConBasic);
                    contactsData.SaveChanges();
                    foreach (var em in Con.Emails)
                    {
                        Email Em = new Email(ConBasic.Id, em);
                        addEmail(Em);
                    }
                    foreach (var tel in Con.Telephones)
                    {
                        Telephone Tel = new Telephone(ConBasic.Id, tel);
                        addTelephone(Tel);
                    }
                    foreach (var tag in Con.Tags)
                    {
                        Tag Tag = new Tag(ConBasic.Id, tag);
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
        
        public string addEmail(Email Em)
        {
            Debug.WriteLine("ALO");
            Debug.WriteLine(Em);
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Emails.Add(Em);
                contactsData.SaveChanges();
                return "E-mail added";
            }
        }

        public string addTelephone (Telephone Tel)
        { 
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Telephones.Add(Tel);
                contactsData.SaveChanges();
                return "Telephone added";
            }
        }

        public string addTag (Tag Tag)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Tags.Add(Tag);
                contactsData.SaveChanges();
                return "Tag added";
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
        /*
        //funckija prima objekt Contact kojim zamijenjuje trenutni objekt u bazi s tim id-em
        public string updateContact(Contact Con)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                int id_int = Convert.ToInt32(Con.Id);
                var toBeUpdated = contactsData.Contacts.Where(x => x.Id == id_int).Single();
                if (toBeUpdated != null)
                {
                    toBeUpdated.Name = Con.Name;
                    toBeUpdated.Surname = Con.Surname;
                    toBeUpdated.Address = Con.Address;
                    toBeUpdated.Email = Con.Email;
                    toBeUpdated.Telephone = Con.Telephone;
                    toBeUpdated.Tags = Con.Tags;
                    contactsData.SaveChanges();
                    return "Contact updated.";
                }
                else
                {
                    return "Contact not found.";
                }
            }
        }

        //trazi kontakte po imenu, prezimenu i tagovima te vracaju filtriranu listu
        //onih kontakata koji kao substring imena/prezimena/tagova imaju searchString
        public JsonResult search(string searchString)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var filteredList1 = contactsData.Contacts.Where(x => x.Name.IndexOf(searchString) > -1).ToList();
                var filteredList2 = contactsData.Contacts.Where(x => x.Surname.IndexOf(searchString) > -1).ToList();
                var filteredList3 = contactsData.Contacts.Where(x => x.Tags.IndexOf(searchString) > -1).ToList();
                var filteredList = filteredList1;
                filteredList.AddRange(filteredList2);
                filteredList.AddRange(filteredList3);
                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
        }
        */
        //funkcija popunjava bazu
        //ne koristi se, potrebna je samo za pocetno popunjavanje baze
        public string fillDB()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contacts]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Emails]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Telephones]");
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Tags]");
            }

            List<string> Emails1 = new List<string>() { "a.anic@yahoo.com", "ana.an@gmail.com" };
            List<string> Telephones1 = new List<string>() { "098/9790-858", "01/2990-512" };
            List<string> Tags1 = new List<string>() { "srednja" };
            Contact_Info Con1 = new Contact_Info("Ana", "Anić", "Amruševa 99", Emails1, Telephones1, Tags1);

            List<string> Emails2 = new List<string>() { "be.bo@gmail.com", "bboric@hotmail.com" };
            List<string> Telephones2 = new List<string>() { "091/5525-552", "01/4741-444" };
            List<string> Tags2 = new List<string>() { "posao_ekipa" };
            Contact_Info Con2 = new Contact_Info("Berislav", "Borić", "Borska 12", Emails2, Telephones2, Tags2);

            List<string> Emails3 = new List<string>() { "cvijeta123@gmail.com", "ccavric@pmf.hr" };
            List<string> Telephones3 = new List<string>() { "095/833-9077", "091/5082-190", "01/7172-121" };
            List<string> Tags3 = new List<string>() { "faks" };
            Contact_Info Con3 = new Contact_Info("Cvijeta", "Cavrić", "Ulica ciklama 52", Emails3, Telephones3, Tags3);

            List<string> Emails4 = new List<string>() { "dorka_d@gmail.com" };
            List<string> Telephones4 = new List<string>() { "099/2716-520" };
            List<string> Tags4 = new List<string>() { "obitelj" };
            Contact_Info Con4 = new Contact_Info("Doris", "Denić", "Dobri dol 18", Emails4, Telephones4, Tags4);

            List<string> Emails5 = new List<string>() { "ela_elez@gmail.com", "elita@hotmail.com" };
            List<string> Telephones5 = new List<string>() { "091/5534-551" };
            List<string> Tags5 = new List<string>() { "srednja" };
            Contact_Info Con5 = new Contact_Info("Ela", "Elezović", "Eugena Kvaternika 11", Emails5, Telephones5, Tags5);

            /* Contact Con6 = new Contact
             {
                 Name = "Franko",
                 Surname = "Favrić",
                 Address = "Francuske revolucije 8, Split",
                 Email = "franko.favric@tvrtka.hr",
                 Telephone = "099 1986 111",
                 Tags = "posao_kontakti"
             };

             Contact Con7 = new Contact
             {
                 Name = "Goran",
                 Surname = "Golubić",
                 Address = "Grižanska 19",
                 Email = "goran.gogs@gmail.com",
                 Telephone = "095 76 848 76, 01 2987 187",
                 Tags = "osnovna"
             };

             Contact Con8 = new Contact
             {
                 Name = "Hrvoje",
                 Surname = "Hrenović",
                 Address = "Heinzelova 21",
                 Email = "hrvoje.hrenovic@tvrtka.com, marketing@tvrtka.com",
                 Telephone = "091 1234 987, 01 4660 555",
                 Tags = "posao_kontakti"
             };

             Contact Con9 = new Contact
             {
                 Name = "Irena",
                 Surname = "Iljazović",
                 Address = "Iblerov trg 8",
                 Email = "iiljaz.math@pmf.hr, irena.iljazovic@gmail.com",
                 Telephone = "092 881 9921",
                 Tags = "faks"
             };

             Contact Con10 = new Contact
             {
                 Name = "Janja",
                 Surname = "Jakić",
                 Address = "Josipa Hamma 12",
                 Email = "",
                 Telephone = "098 8128 818",
                 Tags = "osnovna"
             };*/

            addFullContact(Con1);
            addFullContact(Con2);
            addFullContact(Con3);
            addFullContact(Con4);
            addFullContact(Con5);
            return "OK";
        }
    }
}