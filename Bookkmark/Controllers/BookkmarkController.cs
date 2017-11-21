using System.Web.Mvc;
using Bookkmark.Models;
using System.Collections.Generic;
using System.Linq;

namespace Bookkmark.Controllers
{
    public class BookkmarkController : BaseController
    {
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

　
            if (Session["User"] == null && Request.Cookies["BookmaqLogin"] == null)
            {
                ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Images/Bookmark.jpg";
            }
            else
            {
                Bookkmark bookkmark = new Bookkmark();
                bookkmark.URL = lnk;
                if (BookkmarkExist(bookkmark))
                {
                    ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Imagess/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookkmarkImgSrc"] = "../../Bookkmark/Images/Bookmarked.jpg"; //temporary
                    //ViewData["bookkmarkImgSrc"] = "../../Images/Bookmark.jpg"; //actual later
                }
            }
            return View();
        }

        private object GetBookmarkCount(string lnk)
        {
            Bookkmark bookmark = new Bookkmark();
            string bmrkCount = bookmark.GetBookkmarkCount(lnk);
            return bmrkCount;
            //return "24";
        }

        public string addbm(string Bookkmark, string title, string ipAddr, string bookkmarkImgSrc)
        {
            string resp = null;
            Bookkmark bookkmark = new Bookkmark();
            bookkmark.URL = Bookkmark;
            bookkmark.Name = title;
            bookkmark.IpAddr = ipAddr;

            Session["bookkmark"] = null;
            Session["bookkmark"] = bookkmark;
            //return null;    // which will open Login Window


            if (Session["User"] == null && Request.Cookies["BookmaqLogin"] == null)
            {
                Session["bookkmark"] = null;
                Session["bookkmark"] = bookkmark;
                
                resp = null;    // which will open Login Window
            }
            else
            {
                Users user = (Users)Session["User"];
                if (BookkmarkExist(bookkmark, user))
                {
                    EditBookkmark(bookkmark, user);
                    resp = "../../Bookkmark/Bookkmark/Images/Bookmarked.jpg";
                }
                else
                {
                    InsertBookkmark(bookkmark, user);
                    resp = "../../Bookkmark/Bookkmark/Images/Bookmarked.jpg";
                }
            }
            return resp;
        }

        private void InsertBookkmark(Bookkmark bookmark, Users user)
        {
            //get location details from IP
            //Session["user"]
            //ViewData["Bookkmark"] = bookkmark.URL;
            //ViewData["ipAddr"] = bookkmark.IpAddr;            
        }

        private bool BookkmarkExist(Bookkmark bookmark, Users user)
        {

           // bookkmark.Crea

                //ViewData["Bookkmark"] = bookkmark;
            return false;
        }

        private void EditBookkmark(Bookkmark bookkmark, Users user)
        {
            //Session["User"] &&  Session["bookkmark"]
            //ViewData["Bookkmark"] = bookkmark;
        }

        public ActionResult ClientPage()
        {
            return View();
        }

        public ActionResult ScriptCode(string ddDomains, string txtHeight, string txtWidth, string chkShowCount)
        {
            List<Models.Domain> lstDomains = GetDomainsFromDB(null, null, null);
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

　
            if (Session["User"] != null)
            {
                //Get ScriptCode from DB and assign to ViewState
                //ViewData["ScriptCode"] = 
                ViewData["ScriptCode"] = @"<div id=""divBookkmark""/><script id=""bookkmark"" src=""http://bookkmark.com/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "\"></script>";
            }
            else
            { 
                ViewData["ScriptCode"] = @"<div id=""divBookkmark""/><script id=""bookkmark"" src=""http://bookkmark.com/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "\"></script>";
            }

            //SaveScript();

            return View(lstDomains);

        }

        public ActionResult MyBookkmarks()
        {
            List<BookkmarkModel> lstBookkmarks = GetBookkmarksFromDB(null, null, null);
            return View(lstBookkmarks);
        }

        private List<global::Bookkmark.Models.BookkmarkModel> GetBookkmarksFromDB(string search, string sort, string sortdir)
        {
            List<BookkmarkModel> lstBookkmarks = new List<BookkmarkModel>();
            global::Bookkmark.Models.BookkmarkModel _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1;
            _BookkmarkModel.Name = "Default";
            _BookkmarkModel.URL = "";
            _BookkmarkModel.ParentID = 0;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "12";
            lstBookkmarks.Add(_BookkmarkModel);

            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 102;
            _BookkmarkModel.Name = "Social";
            _BookkmarkModel.URL = "";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "San Francisco";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "10/7/2017 12:24 PM";
            _BookkmarkModel.Count = "9";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 103;
            _BookkmarkModel.Name = "General";
            _BookkmarkModel.URL = "";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Chicago";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "09/21/2017 10:24 AM";
            _BookkmarkModel.Count = "20";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 104;
            _BookkmarkModel.Name = "MyFolder";
            _BookkmarkModel.URL = "";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Texas";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "08/20/2017 10:24 AM";
            _BookkmarkModel.Count = "4";
            lstBookkmarks.Add(_BookkmarkModel);


            //Forums
            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1001;
            _BookkmarkModel.Name = "Facebook";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.facebook.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Bengaluru";
            _BookkmarkModel.Country = "India";
            _BookkmarkModel.CreatedDateTime = "07/25/2017 10:24 AM";
            _BookkmarkModel.Count = "21";
            lstBookkmarks.Add(_BookkmarkModel);



            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1003;
            _BookkmarkModel.Name = "Stats - Quora";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.quora.com/stats";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Los Angeles";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "05/02/2017 10:24 AM";
            _BookkmarkModel.Count = "15";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1005;
            _BookkmarkModel.Name = "LinkedIn";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.linkedin.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "North Carolina";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "03/05/2017 10:24 AM";
            _BookkmarkModel.Count = "24";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1006;
            _BookkmarkModel.Name = "Twitter";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.twitter.com";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Vegas";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "02/10/2017 10:24 AM";
            _BookkmarkModel.Count = "28";
            lstBookkmarks.Add(_BookkmarkModel);

            //CDS
            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1009;
            _BookkmarkModel.Name = "Google Plus";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://plus.google.com/discover";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "27";
            lstBookkmarks.Add(_BookkmarkModel);


            //News
            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1004;
            _BookkmarkModel.Name = "Wikipedia";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.wikipedia.org/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Florida";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "04/21/2017 10:24 AM";
            _BookkmarkModel.Count = "18";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1008;
            _BookkmarkModel.Name = "Google";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.google.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "17";
            lstBookkmarks.Add(_BookkmarkModel);

            //My Folder
            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1007;
            _BookkmarkModel.Name = "Yahoo";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.yahoo.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Boston";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "01/12/2017 10:24 AM";
            _BookkmarkModel.Count = "31";
            lstBookkmarks.Add(_BookkmarkModel);



            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1008;
            _BookkmarkModel.Name = "Bing";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.bing.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "17";
            lstBookkmarks.Add(_BookkmarkModel);




            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1002;
            _BookkmarkModel.Name = "Amazon";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.amazon.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New Jersey";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "06/11/2017 10:24 AM";
            _BookkmarkModel.Count = "19";

            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1010;
            _BookkmarkModel.Name = "ESPN";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.espn.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "25";
            lstBookkmarks.Add(_BookkmarkModel);
            return lstBookkmarks;
        }

        public ActionResult Reports(int page =1, string sort = "Name", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetBookkmarks(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        //public ActionResult Domains()
        //{
        //    return View();
        //}


        public ActionResult Manage()
        {
            if (Request.QueryString["action"].ToString().Equals("Add"))
            {
                List<BookkmarkModel> lstFolders = GetFoldersFromDB();
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Move"))
            {
                List<BookkmarkModel> lstFolders = GetFoldersFromDB();
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/EditBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Edit"))
            {
                List<BookkmarkModel> lstFolders = GetFoldersFromDB();
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
            List<BookkmarkModel> lstFolders = GetFoldersFromDB();
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult NewBMFolder()
        {
            List<BookkmarkModel> lstFolders = GetFoldersFromDB();
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


        public ActionResult BMAdded()
        {
            return View();
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


        public List<global::Bookkmark.Models.BookkmarkModel> GetBookkmarks(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            List<BookkmarkModel> lstBookkmarks = GetBookkmarksFromDB(search, sort, sortdir);            
            totalRecord = lstBookkmarks.Count();
            var varBookkmarks = lstBookkmarks.AsQueryable();

            if (pageSize > 0)
            {
                varBookkmarks = varBookkmarks.Skip(skip).Take(pageSize);
            }

            return varBookkmarks.ToList();
        }

        public List<global::Bookkmark.Models.Domain> GetDomains(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            List<Models.Domain> lstDomains = GetDomainsFromDB(search, sort, sortdir);
            totalRecord = lstDomains.Count();
            var varBookkmarks = lstDomains.AsQueryable();

            if (pageSize > 0)
            {
                varBookkmarks = varBookkmarks.Skip(skip).Take(pageSize);
            }

            return varBookkmarks.ToList();
        }

        private List<global::Bookkmark.Models.BookkmarkModel> GetFoldersFromDB()
        {
            List<BookkmarkModel> lstBookkmarks = new List<BookkmarkModel>();
            global::Bookkmark.Models.BookkmarkModel _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 1;
            _BookkmarkModel.Name = "Default";
            _BookkmarkModel.URL = "Forums";
            _BookkmarkModel.ParentID = 0;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "12";
            lstBookkmarks.Add(_BookkmarkModel);

            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 102;
            _BookkmarkModel.Name = "Forums";
            _BookkmarkModel.URL = "Forums";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "San Francisco";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "10/7/2017 12:24 PM";
            _BookkmarkModel.Count = "9";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 103;
            _BookkmarkModel.Name = "News";
            _BookkmarkModel.URL = "News";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Chicago";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "09/21/2017 10:24 AM";
            _BookkmarkModel.Count = "20";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new global::Bookkmark.Models.BookkmarkModel();
            _BookkmarkModel.BookkmarkID = 104;
            _BookkmarkModel.Name = "MyFolder";
            _BookkmarkModel.URL = "MyFolder";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Texas";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "08/20/2017 10:24 AM";
            _BookkmarkModel.Count = "4";
            lstBookkmarks.Add(_BookkmarkModel);
         
            return lstBookkmarks;
        }

        private List<global::Bookkmark.Models.Domain> GetDomainsFromDB(string search, string sort, string sortdir)
        {
            List<Models.Domain> lstDomains = new List<Models.Domain>();
            global::Bookkmark.Models.Domain _Domain = new global::Bookkmark.Models.Domain();
            _Domain.Id = 1;
            _Domain.Name = "google.com";
            _Domain.Script = "sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd";
            lstDomains.Add(_Domain);

            _Domain = new global::Bookkmark.Models.Domain();
            _Domain.Id = 2;
            _Domain.Name = "facebook.com";
            _Domain.Script = "sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd";
            lstDomains.Add(_Domain);


            _Domain = new global::Bookkmark.Models.Domain();
            _Domain.Id = 3;
            _Domain.Name = "twitter.com";
            _Domain.Script = "sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd sdfsdf ssfddsfsd";
            lstDomains.Add(_Domain);

            return lstDomains;

        }

    }
}
