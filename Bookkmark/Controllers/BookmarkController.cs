using System.Web.Mvc;
using Bookmark.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bookmark.Controllers
{
    public class BookmarkController : BaseController
    {
        private BookmarkCls bmrk = new BookmarkCls();
        private Domain domain = new Domain();

        public ActionResult Show(string lnk, string h, string w, string sd, string t)
        {
            ViewData["Bookmark"] = lnk;
            ViewData["ht"] = h;
            ViewData["wd"] = w;
            ViewData["title"] = t;
　
            if (sd == "true")
            {
                ViewData["BookmarkCount"] = GetBookmarkCount(lnk);
            }
            else
            {
                ViewData["BookmarkCount"] = null;
            }

　
            if (Session["User"] == null)
            {
                ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmark.jpg";
            }
            else
            {
                Users user = (Users)Session["User"];
                BookmarkCls bookmark = new BookmarkCls();
                bookmark.URL = lnk;
                if (bookmark.BookmarkExists(user.UserId.ToString()))
                {
                    ViewData["bookmarkImgSrc"] = "../../Bookmark/Imagess/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmark.jpg"; //temporary
                    //ViewData["bookmarkImgSrc"] = "../../Images/Bookmark.jpg"; //actual later
                }
            }
            return View();
        }

        private object GetBookmarkCount(string lnk)
        {
            return "24";
            string bmrkCount = bmrk.GetBookmarkCount(lnk);
            return bmrkCount;
        }

        public string addbm(string Bookmark, string title, string ipAddr, string bookmarkImgSrc)
        {
            string resp = null;
            bmrk.URL = Bookmark;
            bmrk.Name = title;
            bmrk.IpAddr = ipAddr;

            Session["bookmark"] = null;
            Session["bookmark"] = bmrk;


            if (Session["User"] == null && Request.Cookies["BookmaqLogin"] == null)
            {
                Session["bookmark"] = null;
                Session["bookmark"] = bmrk;                
                resp = null;    // which will open Login Window
            }
            else
            {
                Users user = (Users)Session["User"];
                if (bmrk.BookmarkExists(user.UserId.ToString()))
                {
                    resp = "../../Bookmark/Bookmark/Images/Bookmarked.jpg,Edit," + bmrk.Id;
                }
                else
                {
                    bmrk.OptID = 1;  //Insert, Create
                    bmrk.FolderId = 0;  //Default Folder
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.ManageBookmark();
                    resp = "../../Bookmark/Bookmark/Images/Bookmarked.jpg";
                }
            }
            return resp;
        }

        public ActionResult ClientPage()
        {
            return View();
        }

        public ActionResult ScriptCode(string ddDomains, string txtHeight, string txtWidth, string chkShowCount)
        {
            Users user = (Users)Session["User"];
            List<Domain> lstDomains = domain.GetDomains(null, null, null, user.UserId.ToString());
            ViewData["domains"] = lstDomains;

            if (!string.IsNullOrEmpty(txtHeight))
                ViewData["txtHeight"] = txtHeight;
            else
                ViewData["txtHeight"] = "60";

            if (!string.IsNullOrEmpty(txtWidth))
                ViewData["txtWidth"] = txtWidth;
            else
                ViewData["txtWidth"] = "50";

            if (!string.IsNullOrEmpty(chkShowCount))
            {
                ViewData["chkShowCount"] = chkShowCount;
                ViewData["BookmarkCount"] = "24";
            }
            else
                ViewData["chkShowCount"] = "false";


            //Get ScriptCode from DB and assign to ViewState
            ViewData["ScriptCode"] = domain.GetDomainScript(user.UserId.ToString(), ddDomains);


            //SaveScript();
            ViewData["ScriptCode"] = @"<div id=""divBookmark""/><script id=""bookmark"" src=""http://bookmark.com/Bookmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "\"></script>";

            return View(lstDomains);
        }

        public ActionResult MyBookmarks()
        {
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(null, null, null, user.UserId.ToString());
            return View(lstBookmarks);
        }

        public ActionResult Domains(int page = 1, string sort = "Name", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetDomains(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        public List<BookmarkCls> GetReports(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            BookmarkCls bmrk = new BookmarkCls();
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(search, sort, sortdir, user.UserId.ToString());
            totalRecord = lstBookmarks.Count();
            var varBookmarks = lstBookmarks.AsQueryable();

            if (pageSize > 0)
            {
                varBookmarks = varBookmarks.Skip(skip).Take(pageSize);
            }

            return varBookmarks.ToList();
        }

        public List<Domain> GetDomains(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            Domain domain = new Domain();
            Users user = (Users)Session["User"];
            List<Domain> lstDomains = domain.GetDomains(search, sort, sortdir, user.UserId.ToString());
            totalRecord = lstDomains.Count();
            var varBookmarks = lstDomains.AsQueryable();

            if (pageSize > 0)
            {
                varBookmarks = varBookmarks.Skip(skip).Take(pageSize);
            }

            return varBookmarks.ToList();
        }

        public ActionResult BMAdded()
        {
            return View();
        }

        public ActionResult Reports(int page =1, string sort = "Name", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetReports(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }






        public ActionResult Manage()
        {
           
            if (Request.QueryString["action"].ToString().Equals("Add"))
            {
                List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Move"))
            {
                List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookmark/EditBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Edit"))
            {
                List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookmark/EditBMFolder");
            }         
            else
            {
                //Process(Request.QueryString["id"])
                return View("../Bookmark/Delete");
            }
        }

        public ActionResult EditBMFolder()
        {
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null);
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult NewBMFolder()
        {
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null);
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult Delete()
        {
            ViewData["Refresh"] = "true";
            return View();
        }


        public ActionResult Import()
        {
            return View();
        }





    }
}
