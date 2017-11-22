using System.Web.Mvc;
using Bookkmark.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bookkmark.Controllers
{
    public class BookkmarkController : BaseController
    {
        private Bookkmark bmrk = new Bookkmark();
        private Domain domain = new Domain();

        public ActionResult Show(string lnk, string h, string w, string sd, string t)
        {
            ViewData["Bookkmark"] = lnk;
            ViewData["ht"] = h;
            ViewData["wd"] = w;
            ViewData["title"] = t;
　
            if (sd == "true")
            {
                ViewData["BookkmarkCount"] = GetBookmarkCount(lnk);
            }
            else
            {
                ViewData["BookkmarkCount"] = null;
            }

　
            if (Session["User"] == null)
            {
                ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Images/Bookmark.jpg";
            }
            else
            {
                Users user = (Users)Session["User"];
                Bookkmark bookkmark = new Bookkmark();
                bookkmark.URL = lnk;
                if (bookkmark.BookmarkExists(user.UserId.ToString()))
                {
                    ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Imagess/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Images/Bookmark.jpg"; //temporary
                    //ViewData["bookkmarkImgSrc"] = "../../Images/Bookmark.jpg"; //actual later
                }
            }
            return View();
        }

        private object GetBookmarkCount(string lnk)
        {
            return "24";
            string bmrkCount = bmrk.GetBookkmarkCount(lnk);
            return bmrkCount;
        }

        public string addbm(string Bookkmark, string title, string ipAddr, string bookkmarkImgSrc)
        {
            string resp = null;
            bmrk.URL = Bookkmark;
            bmrk.Name = title;
            bmrk.IpAddr = ipAddr;

            Session["bookkmark"] = null;
            Session["bookkmark"] = bmrk;


            if (Session["User"] == null && Request.Cookies["BookmaqLogin"] == null)
            {
                Session["bookkmark"] = null;
                Session["bookkmark"] = bmrk;                
                resp = null;    // which will open Login Window
            }
            else
            {
                Users user = (Users)Session["User"];
                if (bmrk.BookmarkExists(user.UserId.ToString()))
                {
                    resp = "../../Bookkmark/Bookkmark/Images/Bookmarked.jpg,Edit," + bmrk.Id;
                }
                else
                {
                    bmrk.OptID = 1;  //Insert, Create
                    bmrk.FolderId = 0;  //Default Folder
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.ManageBookkmark();
                    resp = "../../Bookkmark/Bookkmark/Images/Bookmarked.jpg";
                }
            }
            return resp;
        }

        public ActionResult ClientPage()
        {
            return View();
        }

        public ActionResult Domains()
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
                ViewData["BookkmarkCount"] = "24";
            }
            else
                ViewData["chkShowCount"] = "false";


            //Get ScriptCode from DB and assign to ViewState
            ViewData["ScriptCode"] = domain.GetDomainScript(user.UserId.ToString(), ddDomains);


            //SaveScript();
            ViewData["ScriptCode"] = @"<div id=""divBookkmark""/><script id=""bookkmark"" src=""http://bookkmark.com/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "\"></script>";

            return View(lstDomains);
        }

        public ActionResult MyBookkmarks()
        {
            Users user = (Users)Session["User"];
            List<Bookkmark> lstBookkmarks = bmrk.GetBookkmarks(null, null, null, user.UserId.ToString());
            return View(lstBookkmarks);
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

        public List<Bookkmark> GetReports(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            Bookkmark bmrk = new Bookkmark();
            Users user = (Users)Session["User"];
            List<Bookkmark> lstBookkmarks = bmrk.GetBookkmarks(search, sort, sortdir, user.UserId.ToString());
            totalRecord = lstBookkmarks.Count();
            var varBookkmarks = lstBookkmarks.AsQueryable();

            if (pageSize > 0)
            {
                varBookkmarks = varBookkmarks.Skip(skip).Take(pageSize);
            }

            return varBookkmarks.ToList();
        }

        public List<Domain> GetDomains(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            Domain domain = new Domain();
            Users user = (Users)Session["User"];
            List<Domain> lstDomains = domain.GetDomains(search, sort, sortdir, user.UserId.ToString());
            totalRecord = lstDomains.Count();
            var varBookkmarks = lstDomains.AsQueryable();

            if (pageSize > 0)
            {
                varBookkmarks = varBookkmarks.Skip(skip).Take(pageSize);
            }

            return varBookkmarks.ToList();
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
                List<Bookkmark> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Move"))
            {
                List<Bookkmark> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/EditBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Edit"))
            {
                List<Bookkmark> lstFolders = bmrk.GetFolders(null, null, null);
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/EditBMFolder");
            }         
            else
            {
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/Delete");
            }
        }

        public ActionResult EditBMFolder()
        {
            List<Bookkmark> lstFolders = bmrk.GetFolders(null, null, null);
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult NewBMFolder()
        {
            List<Bookkmark> lstFolders = bmrk.GetFolders(null, null, null);
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
