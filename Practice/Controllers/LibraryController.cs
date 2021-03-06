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
        #region Downloads
        public FileResult Debtors_Download()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            string file_path = Server.MapPath("~/Files/Debt.xlsx");
            string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string file_name = "Список должников.xlsx";
            return File(file_path, file_type, file_name);
        }
        public FileResult CountBooks_Download()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            string file_path = Server.MapPath("~/Files/CountBooks.xlsx");
            string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string file_name = "Количество взятых книг.xlsx";
            return File(file_path, file_type, file_name);
        }
        public FileResult BooksPop_Download()
        {
            string file_path = Server.MapPath("~/Files/BooksPop.xlsx");
            string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string file_name = "Количество выдач.xlsx";
            return File(file_path, file_type, file_name);
        }
        public FileResult PenaltyDebts_Download()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            string file_path = Server.MapPath("~/Files/PenaltyDebt.xlsx");
            string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string file_name = "Должники.xlsx";
            return File(file_path, file_type, file_name);
        }
        public FileResult BooksByDate_Download()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            string file_path = Server.MapPath("~/Files/BooksByDate.xlsx");
            string file_type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string file_name = ((string)Session["ChosenDate_string"])+"_Выдачи.xlsx";
            return File(file_path, file_type, file_name);
        }
        #endregion
        #region BooksByDate
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BooksByDate()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            List<BooksByDate> BD = (from t in DM.BG.bookGivings()
                                    where t.Give_Date == DateTime.Today
                                    select new BooksByDate(t.Reader.Library_Card, t.Reader.FIO, t.Publication.Id, t.Publication.Name, t.Publication.Author, t.Librarian.FIO, t.Expected_Return_Date.ToShortDateString())).ToList();
            ViewData.Model = BD;
            string path = Server.MapPath("~/Files/BooksByDate.xlsx");
            ExcelExecuter.XLOutput_BooksByDate(path, BD);
            Session["ChosenDate"] = DateTime.Today;
            string str = DateTime.Today.ToString("s");
            Session["ChosenDate_string"] = str.Substring(0, str.IndexOf('T'));
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BooksByDate(DateTime date)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            List<BooksByDate> BD = (from t in DM.BG.bookGivings()
                                    where t.Give_Date == date
                                    select new BooksByDate(t.Reader.Library_Card, t.Reader.FIO, 
                                    t.Publication.Id, t.Publication.Name, t.Publication.Author, 
                                    t.Librarian.FIO, t.Expected_Return_Date.ToShortDateString())).ToList();
            ViewData.Model = BD;
            string path = Server.MapPath("~/Files/BooksByDate.xlsx");
            ExcelExecuter.XLOutput_BooksByDate(path, BD);
            Session["ChosenDate"] = date;
            string str = date.ToString("s");
            Session["ChosenDate_string"] = str.Substring(0, str.IndexOf('T'));
            return View();
        }
        #endregion
        public ActionResult PenaltyDebtors()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            List<PenaltyDebtors> PD = (from t in DM.Rd.readers()
                                       let sum = (from p in DM.Penalty.penalties()
                                                  where p.BookReturning.BookGiving.Reader.Id == t.Id && p.Sum > 0
                                                  select p.Sum).Sum()
                                       where sum>0
                                       orderby sum descending
                                       select new PenaltyDebtors(t.FIO, t.Birthday.ToShortDateString(), t.Phone_Number, t.Email, sum)).ToList();
            ViewData.Model = PD;
            string path = Server.MapPath("~/Files/PenaltyDebt.xlsx");
            ExcelExecuter.XLOutput_PenaltyDebts(path, PD);
            return View();
        }
        public ActionResult Debtors()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            List<NamePlusCountBooks> NP = (from t in DM.Rd.readers()
                                           let countB= (from p in DM.BG.bookGivings()
                                                        where (p.Reader.Id == t.Id && p.BookReturning == null && 
                                                        p.Expected_Return_Date < DateTime.Today)
                                                        select p).Count()
                                           where countB > 0
                                           select new NamePlusCountBooks(t.FIO, t.Library_Card, t.Phone_Number, t.Email, 
                                           t.Registration_Date, countB)).ToList();
            ViewData.Model = NP;
            ViewData["Debtors"] = ViewData.Model;
            string path = Server.MapPath("~/Files/Debt.xlsx");
            ExcelExecuter.XLOutput_NamePlusCountBooks(path, NP);
            return View();
        }
        public ActionResult BooksPopularity()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            List<BooksPopularity> BP = (from b in DM.Book.publications()
                                        let count = b.BookGiving.Count
                                        orderby count descending
                                        select new BooksPopularity(b.Name, b.Author, b.Publisher.Name, b.Year, count)).ToList();
            ViewData.Model = BP;
            string path = Server.MapPath("~/Files/BooksPop.xlsx");
            ExcelExecuter.XLOutput_BooksPop(path, BP);
            return View();
        }
        #region CountBooks
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CountBooks()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            ViewData["People"] = DM.Rd.readers().ToList();
            ViewData["Counts"] = (from t in DM.Rd.readers() select t.BookGiving.Count).ToList();
            var values = (from t in DM.Rd.readers()
                          select new NamePlusCountBooks(t.FIO, t.Library_Card, t.Phone_Number, t.Email, t.Registration_Date, t.BookGiving.Count)).
                          OrderBy(p=>p.Count).
                          Reverse().
                          ToList();
            ViewData["AllCounts"] = values;
            string path = Server.MapPath("~/Files/CountBooks.xlsx");
            ExcelExecuter.XLOutput_NamePlusCountBooks(path, values);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CountBooks(int min, int max)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            ViewData["People"] = DM.Rd.readers().ToList();
            ViewData["Counts"] = (from t in DM.Rd.readers() select t.BookGiving.Count).ToList();
            
            var values = (from t in DM.Rd.readers()
                          where t.BookGiving.Count>=min && t.BookGiving.Count<=max
                          select new NamePlusCountBooks( t.FIO,t.Library_Card,t.Phone_Number,t.
                          Email,t.Registration_Date, t.BookGiving.Count))
                          .OrderBy(p=>p.Count).Reverse().ToList();
            ViewData["AllCounts"] = values;
            string path = Server.MapPath("~/Files/CountBooks.xlsx");
            ExcelExecuter.XLOutput_NamePlusCountBooks(path, values);
            return View();
        }
        #endregion
        #region ReadersCollection
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
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
        public ActionResult ReadersSearch(int tmp)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            ViewData["Readers"] = DM.Rd.readers();
            return View();
        }
        #endregion
        public ActionResult LibsCollection()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("SignIn");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("ReadersCollection");
            }
            ViewData["Librarians"] = DM.Lib.librarians();
            return View();
        }
        #region BookReturn
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BookReturn(int id)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            ViewData["BG"] = DM.BG.GetBookGiving(id);
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BookReturn(int id, Penalty pn, int SumP, int SumK)
        {
            pn.Sum = SumP + (decimal)SumK / 100;
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            BookReturning BR = new BookReturning();
            if (pn.Info == null)
            {
                pn.Info = "";
            }
            BR.Penalty = pn;
            BR.Real_Return_Date = DateTime.Today;
            BR.Librarian = (Librarian)Session["CurUsr"];
            BookGiving bg = DM.BG.GetBookGiving(id);
            if (bg == null)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            DM.BR.Add(bg, BR);
            return RedirectToAction("ReaderInfo/" + bg.Reader.Id);
        }
        #endregion
        #region AddLib
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddLib()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddLib(Librarian t)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            if (string.IsNullOrWhiteSpace(t.FIO))
                ModelState.AddModelError("FIO", "Укажите имя!");
            if (string.IsNullOrWhiteSpace(t.Phone_Number) || t.Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(t.Email) || !rgEmail.IsMatch(t.Email))
                ModelState.AddModelError("Email", "Введите корректный Email!");
            if (t.Birthday == null || t.Birthday.Year < 1900)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");
            else
            {
                string t1 = t.Birthday.Date.ToString("s");
                ViewData["BD"] = t1.Substring(0, t1.IndexOf('T'));
            }
            if (string.IsNullOrWhiteSpace(t.Password))
                ModelState.AddModelError("Password", "Укажите пароль!");
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
                    DM.Lib.Add(t);
                    return RedirectToAction("LibsCollection");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
        #endregion
        #region LibChange
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LibChange(int id = -1)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            if (id == -1)
            {
                id = (int)Session["LibID"];
            }
            Librarian lib = DM.Lib.GetLibrarian(id);
            if (lib == null)
            {
                Response.Redirect("~/Library/LibsCollection");
            }
            ViewData["Lib"] = lib;
            ViewData.Model = lib;
            string t1 = lib.Birthday.Date.ToString("s");
            ViewData["BD"] = t1.Substring(0, t1.IndexOf('T'));
            string t2 = lib.Hiring_Date.Date.ToString("s");
            ViewData["HD"] = t2.Substring(0, t2.IndexOf('T'));
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LibChange(int id, Librarian t)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            Librarian lib = DM.Lib.GetLibrarian(id);
            if (lib == null)
            {
                Response.Redirect("~/Library/LibsCollection");
            }
            ViewData["Lib"] = lib;
            ViewData.Model = lib;
            string t1 = lib.Birthday.Date.ToString("s");
            ViewData["BD"] = t1.Substring(0, t1.IndexOf('T'));
            string t2 = lib.Hiring_Date.Date.ToString("s");
            ViewData["HD"] = t2.Substring(0, t2.IndexOf('T'));
            if (string.IsNullOrWhiteSpace(t.FIO))
                ModelState.AddModelError("FIO", "Укажите имя!");
            if (string.IsNullOrWhiteSpace(t.Phone_Number) || t.Phone_Number.Length < 11)
                ModelState.AddModelError("Phone_Number", "Введите корректный номер телефона!");
            if (string.IsNullOrWhiteSpace(t.Email) || !rgEmail.IsMatch(t.Email))
                ModelState.AddModelError("Email", "Введите корректный Email!");
            if (t.Birthday == null || t.Birthday.Year < 1900)
                ModelState.AddModelError("Birthday", "Введите корректный ДР!");
            if (t.Hiring_Date == null || t.Hiring_Date.Year < 1900)
                ModelState.AddModelError("Hiring_Date", "Введите корректную дату трудоустройства!");
            if (string.IsNullOrWhiteSpace(t.Password))
                ModelState.AddModelError("Password", "Укажите пароль!");
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
                    DM.Lib.Edit(id, t);
                    return RedirectToAction("LibsCollection");
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }
        #endregion
        #region ReaderInfo
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ReaderInfo(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            Session["ReaderID"] = id;
            Reader rd = DM.Rd.GetReader(id);
            if (rd != null)
            {
                ViewData["Reader"] = rd;
                ViewData.Model = rd;
                ViewData["BG"] = DM.BG.bookGivings(rd);
                ViewData["Penalty"] = DM.Rd.GetPenalty(rd);
            }
            else
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            return View();

        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ReaderInfo(int id, DateTime Give_Date, DateTime Exp_Return, bool CB_Book_ID = false, bool CB_Return = false, bool CB_Penalty = false, bool CB_Give_Date = false,
            bool CB_Exp_Return = false, int Book_ID = 0, string IsReturned = "", string IsPenalty = "")
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            Reader rd = DM.Rd.GetReader(id);
            if (rd == null)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            ViewData["Reader"] = rd;
            ViewData.Model = rd;
            ViewData["Penalty"] = DM.Rd.GetPenalty(rd);
            IEnumerable<BookGiving> bg = DM.BG.bookGivings(rd);
            if (CB_Book_ID)
            {
                bg = bg.Intersect(from t in DM.BG.bookGivings() where t.Publication.Id == Book_ID select t);
            }
            if (CB_Return)
            {
                if (IsReturned == "Возвращена")
                    bg = bg.Intersect(from t in DM.BG.bookGivings() where t.BookReturning != null select t);
                else
                {
                    bg = bg.Intersect(from t in DM.BG.bookGivings() where t.BookReturning == null select t);
                }
            }
            if (CB_Penalty)
            {
                if (IsPenalty == "Да")
                    bg = bg.Intersect(from t in DM.BG.bookGivings() where t.BookReturning != null && t.BookReturning.Penalty != null && t.BookReturning.Penalty.Sum > 0 select t);
                else
                {
                    bg = bg.Intersect(from t in DM.BG.bookGivings() where (t.BookReturning != null && t.BookReturning.Penalty != null && t.BookReturning.Penalty.Sum == 0) || t.BookReturning == null select t);
                }
            }
            if (CB_Give_Date && Give_Date != null)
            {
                bg = bg.Intersect(from t in DM.BG.bookGivings() where t.Give_Date == Give_Date select t);
            }
            if (CB_Exp_Return && Exp_Return != null)
            {
                bg = bg.Intersect(from t in DM.BG.bookGivings() where t.Expected_Return_Date == Exp_Return select t);
            }
            ViewData["BG"] = bg;
            return View();
        }
        #endregion
        public ActionResult BGInfo(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            ViewData.Model = DM.BG.GetBookGiving(id);
            return View();
        }
        public ActionResult PayPenalty(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            BookGiving b = DM.BG.GetBookGiving(id);
            if (b != null && b.BookReturning != null && b.BookReturning.Penalty != null)
            {
                int PenaltyID = b.BookReturning.Penalty.Id;
                DM.Penalty.PenaltyToZero(PenaltyID);
            }
            return RedirectToAction("ReaderInfo/" + b.Reader.Id);
        }
        public ActionResult PubInfo(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/PubsCollection");
            }
            ViewData.Model = DM.Pub.GetPublisher(id);
            if (ViewData.Model == null)
            {
                Response.Redirect("~/Library/PubsCollection");
            }
            ViewData["Books"] = (from t in DM.Book.publications() where t.Publisher.Id == id select t).ToList();
            return View();
        }
        public ActionResult BookInfo(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            ViewData.Model = DM.Book.GetPublication(id);
            if (ViewData.Model == null)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            return View();
        }
        #region GiveBook
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GiveBook(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            Reader rd = DM.Rd.GetReader(id);
            if (rd == null)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            BookGiving bg = new BookGiving();
            bg.Reader = rd;
            bg.Librarian = (Librarian)Session["CurUsr"];
            bg.Give_Date = DateTime.Today;
            ViewData.Model = bg;
            DateTime dt = DateTime.Today;
            dt = new DateTime(dt.Year, dt.Month, dt.Day + 1);
            string t = dt.ToString("s");
            ViewData["ERD"] = t.Substring(0, t.IndexOf('T'));
            Session["CurrentDate"] = t.Substring(0, t.IndexOf('T'));
            Session["Book_OK"] = null;//->false
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GiveBook(int id, BookGiving bg, int BookID = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            Publication p = null;
            bool OK = false;
            try
            {
                p = DM.Book.GetPublication(BookID);
            }
            catch { }
            if (p != null)
            {
                if (p.Available)
                {
                    OK = true;
                    bg.Publication = p;
                    ViewData["BookID"] = BookID;
                }
            }
            Reader rd = DM.Rd.GetReader(id);
            if (rd == null)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            bg.Reader = rd;
            bg.Librarian = (Librarian)Session["CurUsr"];
            bg.Give_Date = DateTime.Today;
            ViewData.Model = bg;
            try
            {
                if (Session["Book_OK"] != null && (bool)Session["Book_OK"])
                {
                    DM.BG.Add(bg);
                    return RedirectToAction("/ReaderInfo/" + id);
                }
            }
            catch { }
            Session["Book_OK"] = OK;
            string t = bg.Expected_Return_Date.ToString("s");
            ViewData["ERD"] = t.Substring(0, t.IndexOf('T'));
            return View();
        }
        #endregion
        #region LibInfo
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LibInfo(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/LibsCollection");
            }
            Librarian lib = DM.Lib.GetLibrarian(id);
            if (lib == null)
            {
                Response.Redirect("~/Library/LibsCollection");
            }
            ViewData["Lib"] = lib;
            ViewData.Model = lib;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LibInfo(int id, Librarian Clib)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            //RedirectToAction("LibChange", id);
            Session["LibID"] = id;
            return RedirectToAction("LibChange", id);
        }
        #endregion
        public ActionResult PubsCollection()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            ViewData["Pubs"] = DM.Pub.publishers();
            return View();
        }
        #region ChangePub
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangePub(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/PubsCollection");
            }
            Publisher p = DM.Pub.GetPublisher(id);
            if (p == null)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            ViewData.Model = p;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePub(int id, Publisher p)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (string.IsNullOrWhiteSpace(p.Name))
                ModelState.AddModelError("Name", "Укажите название издательства!");
            if (string.IsNullOrWhiteSpace(p.City))
                ModelState.AddModelError("City", "Укажите город издательства!");
            if (ModelState.IsValid)
            {
                try
                {
                    DM.Pub.Edit(id, p);
                    return RedirectToAction("PubsCollection");
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }
        #endregion
        #region AddPub
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddPub()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddPub(Publisher p)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (string.IsNullOrWhiteSpace(p.Name))
                ModelState.AddModelError("Name", "Укажите название издательства!");
            if (string.IsNullOrWhiteSpace(p.City))
                ModelState.AddModelError("City", "Укажите город издательства!");
            if (ModelState.IsValid)
            {
                try
                {
                    DM.Pub.Add(p);
                    return RedirectToAction("PubsCollection");
                }
                catch
                {
                    return View();
                }
            }
            else return View();
        }
        #endregion
        #region BooksCollection
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult BooksCollection()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
        #endregion
        #region Addbook
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult AddBook()
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
        #endregion
        #region ChangeReader
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeReader(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            Reader rd = DM.Rd.GetReader(id);
            if (rd == null)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            //rd.Birthday=rd.Birthday.ToS
            ViewData.Model = rd;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeReader(int id, Reader rd)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
        #endregion
        #region ChangeBook
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ChangeBook(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            Publication p = DM.Book.GetPublication(id);
            if (p == null)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            ViewData["Avail"] = p.Available;
            ViewData.Model = p;
            var info = DM.Pub.publishers()
               .Select(s => new
               {
                   Publishers = s.Id,
                   Descr = s.City + ": " + s.Name
               }).ToList();
            ViewData["Pubs"] = new SelectList(info, "Publishers", "Descr", p.Publisher.Id);
            ViewData["Chosen"] = p.Publisher.Id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeBook(int id, Publication p, int Publishers, string AvailableT)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            Publisher pub = DM.Pub.GetPublisher(Publishers);
            if (pub == null)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            bool Avail = false;
            Avail = AvailableT == "Есть в наличии";
            if (string.IsNullOrWhiteSpace(p.Name))
                ModelState.AddModelError("Name", "Укажите название!");
            if (string.IsNullOrWhiteSpace(p.UDK))
                ModelState.AddModelError("UDK", "Укажите УДК!");
            if (string.IsNullOrWhiteSpace(p.BBK))
                ModelState.AddModelError("BBK", "Укажите ББК!");
            if (string.IsNullOrWhiteSpace(p.Author))
                ModelState.AddModelError("Author", "Укажите автора!");
            if (string.IsNullOrWhiteSpace(p.ISBN))
                ModelState.AddModelError("ISBN", "Укажите ISBN!");
            if (p.Page_Count < 1)
                ModelState.AddModelError("Page_Count", "Некорректное количество страниц!");
            if (p.Year < 1800)
                ModelState.AddModelError("Year", "Некорректный год!");
            if (ModelState.IsValid)
            {
                try
                {
                    p.Publisher = pub;
                    p.Available = Avail;
                    DM.Book.Edit(id, p);
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
                    ViewData["Pubs"] = new SelectList(info, "Publishers", "Descr");
                    ViewData["Avail"] = Avail;
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
                ViewData["Pubs"] = new SelectList(info, "Publishers", "Descr");
                ViewData["Avail"] = Avail;
                return View();
            }
        }
        #endregion
        #region AddReader
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddReader(Reader t)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
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
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            Reader t = new Reader();
            t.Birthday = new DateTime(1900, 01, 01);
            return View();
        }
        #endregion
        #region deleteEntities
        public ActionResult DeleteReader(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            try
            {
                DM.Rd.DeleteReader(id);
            }
            catch { }
            return RedirectToAction("ReadersCollection");
        }
        public ActionResult DeleteBook(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/BooksCollection");
            }
            try
            {
                DM.Book.Delete(id);
            }
            catch { }
            return RedirectToAction("BooksCollection");
        }
        public ActionResult DeleteLib(int id = 0)
        {
            if (Session["CurUsr"] == null)
            {
                Response.Redirect("~/Library/SignIn");
            }
            if (id == 0)
            {
                Response.Redirect("~/Library/LibsCollection");
            }
            else if (((Librarian)Session["CurUsr"]).Privilege == 0)
            {
                Response.Redirect("~/Library/ReadersCollection");
            }
            try
            {
                DM.Lib.Delete(id);
            }
            catch { }
            return RedirectToAction("LibsCollection");
        }
        #endregion
        #region SignIn
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
        #endregion
    }
}