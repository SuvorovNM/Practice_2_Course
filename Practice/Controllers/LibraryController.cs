using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Models;
using System.Text.RegularExpressions;

namespace Practice.Controllers
{
    public class LibraryController:Controller
    {
        static string patEmail = @"^(?(')('.+?(?<!\\)'@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        //Регулярное выражение для поля "Город":
        static string patCity = @"^[a-zA-Zа-яА-Я]+(?:[\s-][a-zA-Zа-яА-Я]+)*$";
        Regex rgEmail = new Regex(patEmail);
        Regex rgCity = new Regex(patCity);
        private DataManager DM;
        public LibraryController(DataManager _DM)
        {
            DM = _DM;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReadersCollection()
        {
            ViewData["Readers"] = DM.Rd.readers();
            ViewData["Id"] = 0;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadersCollection(Reader gotten, bool CB_Library_Card=false, bool CB_FIO=false, bool CB_Birth=false, bool CB_Phone=false, bool CB_Reg=false)
        {
            int LT = gotten.Library_Card;
            //IEnumerable<Reader> readers = (from t in DM.Rd.readers() where t.Library_Card == LT select t);
            IEnumerable<Reader> readers = DM.Rd.readers();
            if (CB_Library_Card)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Library_Card == LT select t);
            if (CB_FIO)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.FIO.Contains(gotten.FIO) select t);
            if (CB_Birth)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Birthday == gotten.Birthday select t);
            if (CB_Phone)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Phone_Number == gotten.Phone_Number select t);
            if (CB_Reg)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Registration_Date == gotten.Registration_Date select t);
            ViewData["Readers"] = readers;
            return View();
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadersSearch(int tmp)
        {
            //string temp = VD.ToString();
            //string t = ViewData["Id"].ToString();
            // IEnumerable<Reader> readers = (from t in DM.Rd.readers() where t.Id == gotten.Id select t);
            //ViewData["Readers"] = readers;
            ViewData["Readers"] = DM.Rd.readers();
            return View();
            //ViewData["Readers"] = DM.Rd.readers().Where()
        }
        public ActionResult LibsCollection()
        {
            ViewData["Librarians"] = DM.Lib.librarians();
            return View();
        }
        public ActionResult BooksCollection()
        {
            ViewData["Books"] = DM.Book.publications();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeReader(int id)
        {
            Reader rd = DM.Rd.GetReader(id);
            //rd.Birthday=rd.Birthday.ToS
            ViewData.Model = rd;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeReader(int id, Reader rd)
        {
            /*if (string.IsNullOrWhiteSpace(FIO))
                ModelState.AddModelError("FIO", "Укажите имя");
            if (string.IsNullOrWhiteSpace(Phone_Number) || Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 5)
                ModelState.AddModelError("Email", "Введите корректный Email!");
            if (string.IsNullOrWhiteSpace(Birthday) || Birthday.Length < 5)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Parse(Birthday);
                int flat;
                Int32.TryParse(Flat, out flat);
                DM.Rd.Edit(id, FIO, Phone_Number, Email, dt, Address_Region, City, Street, House, flat);
                return RedirectToAction("CourseWorkCollection");
            }*/
            if (string.IsNullOrWhiteSpace(rd.FIO))
                ModelState.AddModelError("FIO", "Укажите имя!");
            if (string.IsNullOrWhiteSpace(rd.Phone_Number) || rd.Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(rd.Email) || !rgEmail.IsMatch(rd.Email))
                ModelState.AddModelError("Email", "Введите корректный Email!");
            /*if (rd.Birthday==null||rd.Birthday.Year<1900)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");*/
            if (string.IsNullOrWhiteSpace(rd.Address.City)||!rgCity.IsMatch(rd.Address.City))
                ModelState.AddModelError("Address.City", "Введите корректный город!");
            if (string.IsNullOrWhiteSpace(rd.Address.Region))
                ModelState.AddModelError("Address.Region", "Укажите регион!");
            if (string.IsNullOrWhiteSpace(rd.Address.Street))
                ModelState.AddModelError("Address.Street", "Укажите улицу!");
            if (string.IsNullOrWhiteSpace(rd.Address.House))
                ModelState.AddModelError("Address.House", "Укажите дом!");
            if (rd.Address.Flat<1)
                ModelState.AddModelError("Address.Flat", "Укажите квартиру!");
            if (ModelState.IsValid)
            {
                DM.Rd.Edit(id, rd);
                return RedirectToAction("ReadersCollection");
            }
            return View();
        }
    }
}