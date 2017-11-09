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
                @ViewData["BookkmarkCount"] = GetBookkmarkCount(lnk);
            }
            else
            {
                @ViewData["BookkmarkCount"] = null;
            }

　
            if (Session["User"] == null)
            {
                ViewData["bookkmarkImgSrc"] = "../../Images/Bookmark.jpg";
            }
            else
            {
                BookkmarkCls bookkmark = new BookkmarkCls();
                bookkmark.URL = lnk;
                if (BookkmarkExist(bookkmark))
                {
                    ViewData["bookkmarkImgSrc"] = "../../Imagess/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookkmarkImgSrc"] = "../../Images/Bookmark.jpg";
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

            //Session["bookkmark"] = null;
            //Session["bookkmark"] = bookkmark;
            //return null;    // which will open Login Window


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
                    resp = "../../Bookkmark/Images/Bookmarked.jpg";
                }
                else
                {
                    InsertBookkmark(bookkmark);
                    resp = "../../Bookkmark/Images/Bookmarked.jpg";
                }
            }
            return resp;
        }

        private void InsertBookkmark(BookkmarkCls bookkmark)
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

        public ActionResult ScriptCode(string txtHeight, string txtWidth, string chkShowCount)
        {

            if (!string.IsNullOrEmpty(txtHeight))
                ViewData["txtHeight"] = txtHeight + "px";            
            else
                ViewData["txtHeight"] = "60px";

            if (!string.IsNullOrEmpty(txtWidth))
                ViewData["txtWidth"] = txtWidth + "px";
            else
                ViewData["txtWidth"] = "50px";
            if (!string.IsNullOrEmpty(chkShowCount))
                ViewData["chkShowCount"] = chkShowCount;
            else
                ViewData["chkShowCount"] = "false";

　
            if (Session["User"] != null)
            {
                //Get ScriptCode from DB and assign to ViewState
                //ViewData["ScriptCode"] = 
                ViewData["ScriptCode"] = @"<script id=""bookkmark"" src=""http://bookkmark.com/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "></script>";
            }
            else
            { 
                ViewData["ScriptCode"] = @"<script id=""bookkmark"" src=""http://bookkmark.com/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "></script>";
            }

            //SaveScript();

            return View();

        }

        public ActionResult MyBookkmarks()
        {
            List<Bookkmark.Models.Bookkmark> lstBookkmarks = GetBookkmarksFromDB(null, null, null);
            return View(lstBookkmarks);
        }

        private List<Bookkmark.Models.Bookkmark> GetBookkmarksFromDB(string search, string sort, string sortdir)
        {
            List<Bookkmark.Models.Bookkmark> lstBookkmarks = new List<Bookkmark.Models.Bookkmark>();
            Bookkmark.Models.Bookkmark _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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

            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


            //Forums
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1001;
            _BookkmarkModel.Name = "ASP.NET Forums";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://forums.asp.net/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Bengaluru";
            _BookkmarkModel.Country = "India";
            _BookkmarkModel.CreatedDateTime = "07/25/2017 10:24 AM";
            _BookkmarkModel.Count = "21";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1002;
            _BookkmarkModel.Name = "Msdn forums";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://social.msdn.microsoft.com/Forums/en-US/home";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New Jersey";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "06/11/2017 10:24 AM";
            _BookkmarkModel.Count = "19";

            lstBookkmarks.Add(_BookkmarkModel);



            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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




            //News
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1004;
            _BookkmarkModel.Name = "CNN";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.cnn.com/";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Florida";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "04/21/2017 10:24 AM";
            _BookkmarkModel.Count = "18";
            lstBookkmarks.Add(_BookkmarkModel);




            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1005;
            _BookkmarkModel.Name = "Times Of India";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://timesofindia.indiatimes.com/international-home";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "North Carolina";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "03/05/2017 10:24 AM";
            _BookkmarkModel.Count = "24";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1006;
            _BookkmarkModel.Name = "The KapilSharma Show (@TheKapilSShow) Twitter";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://twitter.com/thekapilsshow?lang=en";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Vegas";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "02/10/2017 10:24 AM";
            _BookkmarkModel.Count = "28";
            lstBookkmarks.Add(_BookkmarkModel);





            //My Folder
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1007;
            _BookkmarkModel.Name = "Yahoo";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://yahoomail.com";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "Boston";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "01/12/2017 10:24 AM";
            _BookkmarkModel.Count = "31";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1008;
            _BookkmarkModel.Name = "Gmail";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://gmail.com";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "17";
            lstBookkmarks.Add(_BookkmarkModel);

            //CDS
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1009;
            _BookkmarkModel.Name = "LabCorp Beacon ® Patient Overview ";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://patient.labcorp.com/patient/PatientWeb/default.aspx";
            _BookkmarkModel.IpAddr = "121.0.0.1";
            _BookkmarkModel.City = "New York";
            _BookkmarkModel.Country = "USA";
            _BookkmarkModel.CreatedDateTime = "11/8/2017 10:24 AM";
            _BookkmarkModel.Count = "27";
            lstBookkmarks.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1010;
            _BookkmarkModel.Name = "221(G) Tracker";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.immihelp.com/tracker/221g-tracker";
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

        public ActionResult Domains()
        {
            return View();
        }


        public ActionResult Manage()
        {
            if (Request.QueryString["action"].ToString().Equals("Add"))
            {
                List<Bookkmark.Models.Bookkmark> lstFolders = GetFoldersFromDB();
                ViewData["ddFolders"] = lstFolders;
                //Process(Request.QueryString["id"])
                return View("../Bookkmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Edit"))
            {
                List<Bookkmark.Models.Bookkmark> lstFolders = GetFoldersFromDB();
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
            List<Bookkmark.Models.Bookkmark> lstFolders = GetFoldersFromDB();
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult NewBMFolder()
        {
            List<Bookkmark.Models.Bookkmark> lstFolders = GetFoldersFromDB();
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


        public List<Bookkmark.Models.Bookkmark> GetBookkmarks(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            List<Bookkmark.Models.Bookkmark>  lstBookkmarks = GetBookkmarksFromDB(search, sort, sortdir);            

            //var lstBookkmarks = lstFolders.AsEnumerable();
            //var v = (from a in lstBookkmarks
            //         where  a.Name.Contains(search) ||
            //                a.URL.Contains(search)
            //         //orderby a.Name ascending  
            //         select a);
            //v = v.OrderBy(sort + " " + sortdir);

            totalRecord = lstBookkmarks.Count();
            var varBookkmarks = lstBookkmarks.AsQueryable();

            if (pageSize > 0)
            {
                varBookkmarks = varBookkmarks.Skip(skip).Take(pageSize);
            }

            return varBookkmarks.ToList();
        }


        private List<Bookkmark.Models.Bookkmark> GetFoldersFromDB()
        {
            List<Bookkmark.Models.Bookkmark> lstBookkmarks = new List<Bookkmark.Models.Bookkmark>();
            Bookkmark.Models.Bookkmark _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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

            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
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


    }
}
