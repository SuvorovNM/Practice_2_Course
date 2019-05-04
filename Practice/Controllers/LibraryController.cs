﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Practice.Models;
using System.Text.RegularExpressions;

namespace Practice.Controllers
{
    public class LibraryController : Controller
    {
        static string patEmail = @"^(?(')('.+?(?<!\\)'@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        //Регулярное выражение для поля "Город":
        static string patCity = @"^[a-zA-Zа-яА-Я]+(?:[\s-][a-zA-Zа-яА-Я]+)*$";
        Regex rgEmail = new Regex(patEmail);
        Regex rgCity = new Regex(patCity);
        Librarian CurLib;
        private DataManager DM;
        public LibraryController(DataManager _DM)
        {
            DM = _DM;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReadersCollection()
        {
            Librarian CurLib = (Librarian)Session["CurUsr"];
            if (CurLib != null)
            {
                ViewData["Readers"] = DM.Rd.readers();
                ViewData["Id"] = 0;
                return View();
            }
            else return RedirectToAction("SignIn");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadersCollection(Reader gotten, bool CB_Library_Card = false, bool CB_FIO = false, bool CB_Birth = false, bool CB_Phone = false, bool CB_Reg = false)
        {
            int LT = gotten.Library_Card;
            //IEnumerable<Reader> readers = (from t in DM.Rd.readers() where t.Library_Card == LT select t);
            IEnumerable<Reader> readers = DM.Rd.readers();
            if (CB_Library_Card)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Library_Card == LT select t);
            if (CB_FIO && gotten.FIO != null)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.FIO.Contains(gotten.FIO) select t);
            if (CB_Birth && gotten.Birthday != null)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Birthday == gotten.Birthday select t);
            if (CB_Phone && gotten.Phone_Number != null)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Phone_Number == gotten.Phone_Number select t);
            if (CB_Reg && gotten.Registration_Date != null)
                readers = readers.Intersect(from t in DM.Rd.readers() where t.Registration_Date == gotten.Registration_Date select t);
            ViewData["Readers"] = readers;
            return View();
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReadersSearch(int tmp)
        {
            ViewData["Readers"] = DM.Rd.readers();
            return View();
        }
        public ActionResult LibsCollection()
        {
            ViewData["Librarians"] = DM.Lib.librarians();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BooksCollection()
        {
            Librarian CurLib = (Librarian)Session["CurUsr"];
            if (CurLib != null)
            {
                ViewData["Books"] = DM.Book.publications();
                return View();
            }
            else return RedirectToAction("SignIn");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BooksCollection(Publication gotten, bool CB_Id = false, bool CB_Name = false, bool CB_Author = false, bool CB_BBK = false, bool CB_Pub = false, bool CB_Avail = false, string AvailableT = "")
        {
            IEnumerable<Publication> pubs = DM.Book.publications();
            if (CB_Id)
            {
                pubs = pubs.Intersect(DM.Book.publicationsById(gotten.Id));
            }
            if (CB_Name && gotten.Name != null)
            {
                pubs = pubs.Intersect(DM.Book.publicationsByName(gotten.Name));
            }
            if (CB_Author && gotten.Author != null)
            {
                pubs = pubs.Intersect(DM.Book.publicationsByAuthor(gotten.Author));
            }
            if (CB_BBK && gotten.BBK != null)
            {
                pubs = pubs.Intersect(DM.Book.publicationsByBBK(gotten.BBK));
            }
            if (CB_Pub && gotten.Publisher != null && gotten.Publisher.Name != null)
            {
                pubs = pubs.Intersect(DM.Book.publicationsByPub(gotten.Publisher.Name));
            }
            if (CB_Avail)
            {
                bool Avail = true;
                if (AvailableT == "Нет в наличии") Avail = false;
                pubs = pubs.Intersect(DM.Book.publicationsByAvail(Avail));
            }
            ViewData["Books"] = pubs;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddBook()
        {
            var info = DM.Pub.publishers()
                .Select(s => new
                {
                    Publishers = s.Id,
                    Descr = s.City + ": " + s.Name
                }).ToList();
            ViewData["Publishers"] = new SelectList(info, "Publishers", "Descr");
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddBook(Publication p, int Publishers)
        {
            if (string.IsNullOrWhiteSpace(p.Name))
                ModelState.AddModelError("Name", "Укажите название книги!");
            if (string.IsNullOrWhiteSpace(p.Author))
                ModelState.AddModelError("Author", "Укажите название книги!");
            if (string.IsNullOrWhiteSpace(p.BBK))
                ModelState.AddModelError("BBK", "Укажите ББК книги!");
            if (string.IsNullOrWhiteSpace(p.UDK))
                ModelState.AddModelError("UDK", "Укажите UDK книги!");
            if (string.IsNullOrWhiteSpace(p.ISBN))
                ModelState.AddModelError("ISBN", "Укажите ISBN книги!");
            if (p.Page_Count < 1)
                ModelState.AddModelError("Page_Count", "Некорректное количество страниц!");
            if (p.Year < 1800)
                ModelState.AddModelError("Year", "Некорректный год!");
            if (ModelState.IsValid)
            {
                try
                {
                    p.Available = true;
                    DM.Book.Add(p, Publishers);
                    return RedirectToAction("BooksCollection");
                }
                catch
                {
                    var info = DM.Pub.publishers()
                                .Select(s => new
                                {
                                    Publishers = s.Id,
                                    Descr = s.City + ": " + s.Name
                                }).ToList();
                    ViewData["Publishers"] = new SelectList(info, "Publishers", "Descr");
                    return View();
                }
            }
            else
            {
                var info = DM.Pub.publishers()
                                .Select(s => new
                                {
                                    Publishers = s.Id,
                                    Descr = s.City + ": " + s.Name
                                }).ToList();
                ViewData["Publishers"] = new SelectList(info, "Publishers", "Descr");
                return View();
            }
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
            if (string.IsNullOrWhiteSpace(rd.FIO))
                ModelState.AddModelError("FIO", "Укажите имя!");
            if (string.IsNullOrWhiteSpace(rd.Phone_Number) || rd.Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(rd.Email) || !rgEmail.IsMatch(rd.Email))
                ModelState.AddModelError("Email", "Введите корректный Email!");
            /*if (rd.Birthday==null||rd.Birthday.Year<1900)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");*/
            if (string.IsNullOrWhiteSpace(rd.Address.City) || !rgCity.IsMatch(rd.Address.City))
                ModelState.AddModelError("Address.City", "Введите корректный город!");
            if (string.IsNullOrWhiteSpace(rd.Address.Region))
                ModelState.AddModelError("Address.Region", "Укажите регион!");
            if (string.IsNullOrWhiteSpace(rd.Address.Street))
                ModelState.AddModelError("Address.Street", "Укажите улицу!");
            if (string.IsNullOrWhiteSpace(rd.Address.House))
                ModelState.AddModelError("Address.House", "Укажите дом!");
            if (rd.Address.Flat < 1)
                ModelState.AddModelError("Address.Flat", "Укажите квартиру!");
            if (ModelState.IsValid)
            {
                DM.Rd.Edit(id, rd);
                return RedirectToAction("ReadersCollection");
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddReader(Reader t)
        {
            if (string.IsNullOrWhiteSpace(t.FIO))
                ModelState.AddModelError("FIO", "Укажите имя!");
            if (string.IsNullOrWhiteSpace(t.Phone_Number) || t.Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(t.Email) || !rgEmail.IsMatch(t.Email))
                ModelState.AddModelError("Email", "Введите корректный Email!");
            if (t.Birthday == null || t.Birthday.Year < 1900)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");
            if (string.IsNullOrWhiteSpace(t.Address.City) || !rgCity.IsMatch(t.Address.City))
                ModelState.AddModelError("Address.City", "Введите корректный город!");
            if (string.IsNullOrWhiteSpace(t.Address.Region))
                ModelState.AddModelError("Address.Region", "Укажите регион!");
            if (string.IsNullOrWhiteSpace(t.Address.Street))
                ModelState.AddModelError("Address.Street", "Укажите улицу!");
            if (string.IsNullOrWhiteSpace(t.Address.House))
                ModelState.AddModelError("Address.House", "Укажите дом!");
            if (t.Address.Flat < 1)
                ModelState.AddModelError("Address.Flat", "Укажите квартиру!");
            if (ModelState.IsValid)
            {
                try
                {
                    DM.Rd.Add(t);
                    return RedirectToAction("ReadersCollection");
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddReader()
        {
            Reader t = new Reader();
            t.Birthday = new DateTime(1900, 01, 01);
            return View();
        }
        public ActionResult DeleteReader(int id)
        {
            DM.Rd.DeleteReader(id);
            return RedirectToAction("ReadersCollection");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SignIn(int Staff_Number = 0, string Password = "")
        {
            //if (lib!=null)            
            Librarian lib = DM.Lib.LoginIn(Staff_Number, Password);
            CurLib = lib;
            if (CurLib != null)
            {
                Session["CurUsr"] = lib;
                return RedirectToAction("ReadersCollection");
            }
            else
            {
                ModelState.AddModelError("Password", "Неправильный логин или пароль!");
                return View();
            }
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SignIn()
        {
            //if (lib!=null)
            Session["CurUsr"] = null;
            return View();
        }
    }
}