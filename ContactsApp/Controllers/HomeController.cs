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
        
        //vraca iz baze kontakt sa zadanim id-em
        public JsonResult getContactById(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var tempContact = contactsData.Contacts.Where(x => x.Id == ConId).Single();
                return Json(tempContact, JsonRequestBehavior.AllowGet);

            }

        }

        //vraca listu svih kontakata iz baze
        public JsonResult getAll()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                var contactList = contactsData.Contacts.ToList();
                return Json(contactList, JsonRequestBehavior.AllowGet);
            }
        }
        
        //funkcija dodaje kontakt u bazu
        public string addContact(Contact Con)
        {
            if (Con != null)
            {
                using (ContactsDBEntities contactsData = new ContactsDBEntities())
                {
                    contactsData.Contacts.Add(Con);
                    contactsData.SaveChanges();
                    return "Contact added.";
                }
            }
            else
            {
                return "Contact to be added is not valid.";
            }
        }

        //brisanje kontakta iz baze
        public string deleteContact(int ConId)
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
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

        //funkcija popunjava bazu
        //ne koristi se, potrebna je samo za pocetno popunjavanje baze
        public string fillDB()
        {
            using (ContactsDBEntities contactsData = new ContactsDBEntities())
            {
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contacts]");
            }

            Contact Con1 = new Contact
            {
                Name = "Ana",
                Surname = "Anić",
                Address = "Amruševa 99",
                Email = "ana.an@gmail.com, a.anic@yahoo.com",
                Telephone = "098 9790 858, 01 2990 512",
                Tags = "srednja"
            };

            Contact Con2 = new Contact
            {
                Name = "Berislav",
                Surname = "Borić",
                Address = "Borska 12",
                Email = "be.bo@gmail.com, bboric@hotmail.com",
                Telephone = "091 5525 552, 01 4741 444",
                Tags = "posao_ekipa"
            };

            Contact Con3 = new Contact
            {
                Name = "Cvijeta",
                Surname = "Cavrić",
                Address = "Ulica ciklama 52",
                Email = "cvijeta123@gmail.com, ccavric@pmf.hr",
                Telephone = "095 833 9077, 091 5082 190, 01 7172 121",
                Tags = "faks"
            };

            Contact Con4 = new Contact
            {
                Name = "Doris",
                Surname = "Denić",
                Address = "Dobri dol 18",
                Email = "dorka_d@gmail.com",
                Telephone = "099 2716 520",
                Tags = "obitelj"
            };

            Contact Con5 = new Contact
            {
                Name = "Ela",
                Surname = "Elezović",
                Address = "Eugena Kvaternika 11",
                Email = "ela_elez@gmail.com, elita@hotmail.com",
                Telephone = "091 5534 551",
                Tags = "srednja"
            };

            Contact Con6 = new Contact
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
            };

            addContact(Con1);
            addContact(Con2);
            addContact(Con3);
            addContact(Con4);
            addContact(Con5);
            addContact(Con6);
            addContact(Con7);
            addContact(Con8);
            addContact(Con9);
            addContact(Con10);
            return "OK";
        }
    }
}