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
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult getAll()
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                var contactList = contactsData.Contacts.ToList();
                return Json(contactList, JsonRequestBehavior.AllowGet);
            }
        }

        public string addContact(Contact Con)
        {
            if (Con != null)
            {
                using (ContactDBEntities contactsData = new ContactDBEntities())
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

        public string deleteContact (int ConId)
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                int id_int = Convert.ToInt32(ConId);
                var contactById = contactsData.Contacts.Where(x => x.Id == id_int).FirstOrDefault();
                contactsData.Contacts.Remove(contactById);
                contactsData.SaveChanges();
                return "Contact deleted.";
            }
        }

        public string updateContact (Contact Con)
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                int id_int = Convert.ToInt32(Con.Id);
                var toBeUpdated = contactsData.Contacts.Where(x => x.Id == id_int).FirstOrDefault();
                toBeUpdated.Name = Con.Name;
                toBeUpdated.Surname = Con.Surname;
                toBeUpdated.Address = Con.Address;
                toBeUpdated.Email = Con.Email;
                toBeUpdated.Telephone = Con.Telephone;
                toBeUpdated.Tags = Con.Tags;
                contactsData.SaveChanges();
                return "Contact updated.";
            }
        }

        public JsonResult searchByName (string searchString)
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                var filteredList = contactsData.Contacts.Where(x => x.Name.IndexOf(searchString)>-1).ToList();
                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult searchBySurname(string searchString)
        {
            Debug.WriteLine(searchString);
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                var filteredList = contactsData.Contacts.Where(x => x.Surname.IndexOf(searchString) > -1).ToList();
                Debug.WriteLine(filteredList);
                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult searchByTag(string searchString)
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                var filteredList = contactsData.Contacts.Where(x => x.Tags.IndexOf(searchString) > -1).ToList();
                return Json(filteredList, JsonRequestBehavior.AllowGet);
            }
        }
        public string fillDB ()
        {
            using (ContactDBEntities contactsData = new ContactDBEntities())
            {
                contactsData.Database.ExecuteSqlCommand("TRUNCATE TABLE [Contacts]");
            }
            
            Contact Con1 = new Contact {
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