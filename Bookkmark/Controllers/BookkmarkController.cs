﻿using System.Web.Mvc;

namespace Bookkmark.Controllers
{
    public class BookkmarkController : BaseController
    {
        public ActionResult Show(string lnk, string h, string w, string sd)
        {
            ViewData["Bookkmark"] = lnk;
            ViewData["ht"] = h;
            ViewData["wd"] = w;


            if (sd == "true")
            {
                @ViewData["BookkmarkCount"] = GetBookkmarkCount(lnk);
            }
            else
            {
                @ViewData["BookkmarkCount"] = null;
            }


            if (Session["User"] == null)
            {
                ViewData["bookkmarkImgSrc"] = "../../JavaScriptProgram/Images/Bookmark.jpg";
            }
            else
            {
                BookkmarkCls bookkmark = new BookkmarkCls();
                bookkmark.URL = lnk;
                if (BookkmarkExist(bookkmark))
                {
                    ViewData["bookkmarkImgSrc"] = "../../JavaScriptProgram/Imagess/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookkmarkImgSrc"] = "../../JavaScriptProgram/Images/Bookmark.jpg";
                }
            }
            return View();
        }

        private object GetBookkmarkCount(string lnk)
        {
            //GetCount
            return "24";
        }

        public string addbm(string Bookkmark, string title, string ipAddr, string bookkmarkImgSrc)
        {
            string resp = null;
            BookkmarkCls bookkmark = new BookkmarkCls();
            bookkmark.URL = Bookkmark;
            bookkmark.Name = title;
            bookkmark.IpAddr = ipAddr;

            if (Session["User"] == null)
            {
                Session["bookkmark"] = null;
                Session["bookkmark"] = bookkmark;
                resp = null;    // which will open Login Window
            }
            else
            {
                if (BookkmarkExist(bookkmark))
                {
                    EditBookkmark(bookkmark);
                    resp = "../../JavaScriptProgram/Images/Bookmarked.jpg";
                }
                else
                {
                    AddBookkmark(bookkmark);
                    resp = "../../JavaScriptProgram/Images/Bookmarked.jpg";
                }
            }
            return resp;
        }

        private void AddBookkmark(BookkmarkCls bookkmark)
        {
            //get location details from IP
            //Session["user"]
            //ViewData["Bookkmark"] = bookkmark.URL;
            //ViewData["ipAddr"] = bookkmark.IpAddr;            
        }


        private bool BookkmarkExist(BookkmarkCls bookkmark)
        {
            //Session["User"]
            //ViewData["Bookkmark"] = bookkmark;
            return false;
        }


        private void EditBookkmark(BookkmarkCls bookkmark)
        {
            //Session["User"] &&  Session["bookkmark"]
            //ViewData["Bookkmark"] = bookkmark;
        }

        public ActionResult ClientPage()
        {
            return View();
        }

        public ActionResult ScriptCode()
        {
            return View();
        }

        public ActionResult MyBookkmarks()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Domains()
        {
            return View();
        }
    }
}
