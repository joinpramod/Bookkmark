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
                    InsertBookkmark(bookkmark);
                    resp = "../../JavaScriptProgram/Images/Bookmarked.jpg";
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
            }
            else
            { 
                ViewData["ScriptCode"] = @"<script id=""bookkmark"" src=""http://bookkmark.com/JavaScriptProgram/Bookkmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "></script>";
            }

            //SaveScript();

            return View();

        }

        public ActionResult MyBookkmarks()
        {

            List<Bookkmark.Models.Folder> lstFolders = new List<Bookkmark.Models.Folder>();
            Bookkmark.Models.Folder _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 1;
            _FolderModel.Name = "Root1";
            _FolderModel.ParentID = 0;
            lstFolders.Add(_FolderModel);

            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 102;
            _FolderModel.Name = "Child102";
            _FolderModel.ParentID = 1;
            lstFolders.Add(_FolderModel);


            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 103;
            _FolderModel.Name = "Child103";
            _FolderModel.ParentID = 1;
            lstFolders.Add(_FolderModel);


            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 104;
            _FolderModel.Name = "Child104";
            _FolderModel.ParentID = 1;
            lstFolders.Add(_FolderModel);



            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 1001;
            _FolderModel.Name = "Child1001";
            _FolderModel.ParentID = 102;
            lstFolders.Add(_FolderModel);


            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 1002;
            _FolderModel.Name = "Child1002";
            _FolderModel.ParentID = 102;
            lstFolders.Add(_FolderModel);



            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 2;
            _FolderModel.Name = "Root2";
            _FolderModel.ParentID = 0;
            lstFolders.Add(_FolderModel);


            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 105;
            _FolderModel.Name = "Child105";
            _FolderModel.ParentID = 2;
            lstFolders.Add(_FolderModel);


            _FolderModel = new Bookkmark.Models.Folder();
            _FolderModel.FolderID = 106;
            _FolderModel.Name = "Child106";
            _FolderModel.ParentID = 2;
            lstFolders.Add(_FolderModel);

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
    }
}
