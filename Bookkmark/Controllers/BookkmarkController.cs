using System.Web.Mvc;
using Bookkmark.Models;
using System.Collections.Generic;

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

            List<Bookkmark.Models.Bookkmark> lstFolders = new List<Bookkmark.Models.Bookkmark>();
            Bookkmark.Models.Bookkmark _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1;
            _BookkmarkModel.Name = "Default";
            _BookkmarkModel.ParentID = 0;
            _BookkmarkModel.IsFolder = true;
            lstFolders.Add(_BookkmarkModel);

            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 102;
            _BookkmarkModel.Name = "Forums";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 103;
            _BookkmarkModel.Name = "News";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 104;
            _BookkmarkModel.Name = "MyFolder";
            _BookkmarkModel.ParentID = 1;
            _BookkmarkModel.IsFolder = true;
            lstFolders.Add(_BookkmarkModel);


            //Forums
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1001;
            _BookkmarkModel.Name = "ASP.NET Forums";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://forums.asp.net/";
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1002;
            _BookkmarkModel.Name = "Msdn forums";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://social.msdn.microsoft.com/Forums/en-US/home";
            lstFolders.Add(_BookkmarkModel);



            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1003;
            _BookkmarkModel.Name = "Stats - Quora";
            _BookkmarkModel.ParentID = 102;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "https://www.quora.com/stats";
            lstFolders.Add(_BookkmarkModel);




            //News
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1004;
            _BookkmarkModel.Name = "CNN";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.cnn.com/";
            lstFolders.Add(_BookkmarkModel);



            
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1005;
            _BookkmarkModel.Name = "Times Of India";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://timesofindia.indiatimes.com/international-home";
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1006;
            _BookkmarkModel.Name = "The KapilSharma Show (@TheKapilSShow) Twitter";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://twitter.com/thekapilsshow?lang=en";
            lstFolders.Add(_BookkmarkModel);





            //My Folder
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1007;
            _BookkmarkModel.Name = "Yahoo";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://yahoomail.com";
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1008;
            _BookkmarkModel.Name = "Gmail";
            _BookkmarkModel.ParentID = 103;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://gmail.com";
            lstFolders.Add(_BookkmarkModel);

            //CDS
            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1009;
            _BookkmarkModel.Name = "LabCorp Beacon ® Patient Overview ";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://patient.labcorp.com/patient/PatientWeb/default.aspx";
            lstFolders.Add(_BookkmarkModel);


            _BookkmarkModel = new Bookkmark.Models.Bookkmark();
            _BookkmarkModel.BookkmarkID = 1010;
            _BookkmarkModel.Name = "221(G) Tracker";
            _BookkmarkModel.ParentID = 104;
            _BookkmarkModel.IsFolder = false;
            _BookkmarkModel.URL = "http://www.immihelp.com/tracker/221g-tracker";
            lstFolders.Add(_BookkmarkModel);

            return View(lstFolders);
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Domains()
        {
            return View();
        }


        public ActionResult Manage()
        {
            //Bookkmark bkmrk = GetBookkmark(Request.QueryString["id"])
            if (Request.QueryString["action"] != null)
            {
                if (Request.QueryString["action"].ToString().Equals("Add"))
                {
                     //Process(Request.QueryString["id"])
                     return View("../Bookkmark/AddEditBM");
                }
                else if (Request.QueryString["action"].ToString().Equals("Edit"))
                {
                    //Process(Request.QueryString["id"])
                    return View("../Bookkmark/Rename");

                   // return View("../Bookkmark/AddEditBM");
                }
                else if (Request.QueryString["action"].ToString().Equals("NewFolder"))
                {

                    ViewData["Refresh"] = null;
                    //Process(Request.QueryString["id"])
                    return View("../Bookkmark/NewFolder");
                }
                else
                {
                    //Process(Request.QueryString["id"])
                    return View("../Bookkmark/Delete");
                }
            }
            else
            {
                ViewData["Refresh"] = "true";
                if (Request.Form["btnAdd"] != null)
                {
                    return View("../Bookkmark/AddEditBM");
                }
                else if (Request.Form["btnEdit"] != null)
                {
                    return View("../Bookkmark/Rename");
                   // return View("../Bookkmark/AddEditBM");
                }
                else if (Request.Form["btnNewFolder"] != null)
                {
                    return View("../Bookkmark/NewFolder");
                }
                else
                {
                    return View("../Bookkmark/Delete");
                }

            }
        }

        public ActionResult Rename()
        {
            return View();
        }

        public ActionResult NewFolder()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

    }
}
