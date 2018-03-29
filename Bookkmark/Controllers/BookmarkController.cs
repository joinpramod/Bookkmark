using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using HtmlAgilityPack;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace Bookmark.Controllers
{
    public class BookmarkController : BaseController
    {
        private BookmarkCls bmrk = new BookmarkCls();
        private Domain domain = new Domain();
        private string lclCity = string.Empty;
        private string lclState = string.Empty;
        private string lclCountry = string.Empty;
        private string lclipAddr = Utilities.GetIPAddress();

        public ActionResult Show(string did, string lnk, string t)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["did"]))
                    did = Request.QueryString["did"];
                if (!string.IsNullOrEmpty(Request.QueryString["lnk"]))
                    lnk = Request.QueryString["lnk"];
                if (!string.IsNullOrEmpty(Request.QueryString["t"]))
                    t = Request.QueryString["t"];

                Domain _domain = new Bookmark.Domain();
                did = Utilities.DecryptString(did);

                _domain = _domain.GetDomains(did);

                if (string.IsNullOrEmpty(lnk))
                {
                    lnk = Request.UrlReferrer.ToString();
                }

                ViewData["Bookmark"] = lnk;
                ViewData["ht"] = _domain.Height;
                ViewData["wd"] = _domain.Width;
                ViewData["title"] = t;

                if (!ValidateBookmarkRequest(_domain, lnk))
                    ViewData["bookmarkImgSrc"] = null;


                if (bool.Parse(_domain.ShowCount))
                {
                    ViewData["BookmarkCount"] = GetBookmarkCount(lnk);
                }
                else
                {
                    ViewData["BookmarkCount"] = null;
                }

                if (Session["User"] == null)
                {
                    if (!Request.Url.AbsoluteUri.ToString().Contains("localhost"))
                    {
                        ViewData["bookmarkImgSrc"] = "http://booqmarqs.com/Images/Bookmark.jpg";
                    }
                    else
                    {
                        ViewData["bookmarkImgSrc"] = "http://localhost/bookmark/Images/Bookmark.jpg";
                    }
                }
                else
                {
                    Users user = (Users)Session["User"];
                    BookmarkCls bookmark = new BookmarkCls();
                    bookmark.URL = lnk;
                    if (bookmark.BookmarkExists(user.UserId.ToString()))
                    {
                        if (!Request.Url.AbsoluteUri.ToString().Contains("localhost"))
                        {
                            ViewData["bookmarkImgSrc"] = "http://booqmarqs.com/Images/Bookmarked.jpg";
                        }
                        else
                        {
                            ViewData["bookmarkImgSrc"] = "http://localhost/bookmark/Images/Bookmarked.jpg";
                        }
                    }
                    else
                    {
                        if (!Request.Url.AbsoluteUri.ToString().Contains("localhost"))
                        {
                            ViewData["bookmarkImgSrc"] = "http://booqmarqs.com/Images/Bookmark.jpg"; //temporary
                        }
                        else
                        {
                            ViewData["bookmarkImgSrc"] = "http://localhost/bookmark/Images/Bookmark.jpg";
                        }                                                                    //ViewData["bookmarkImgSrc"] = "../../Images/Bookmark.jpg"; //actual later

                    }
                }
            }
            catch(Exception ex)
            {
                try
                {
                    Utilities.EmailException(ex);
                }
                catch
                {

                }
                ViewData["bookmarkImgSrc"] = null;
            }
            return View();
        }

        private bool ValidateBookmarkRequest(Domain _dom, string lnk)
        {
            var uri = new Uri(lnk);
            var host = uri.Host;
           // var parts = host.ToLowerInvariant().Split('.');
           // string lnkDom = string.Empty;

            if (_dom.Name == host)
            {
                if(!bool.Parse(_dom.IsOwner))
                {
                    _dom.OptID = 6;
                    _dom.IsOwner = "1";
                    _dom.ManageDomain();
                }
                return true;
            }
            else
            {
                return false;
            }
            //throw new NotImplementedException();
        }

        private object GetBookmarkCount(string lnk)
        {
            //return "24";
            string bmrkCount = bmrk.GetBookmarkCount(lnk);
            return bmrkCount;
        }

        //From Booqmarq Icon
        public string managebm(string Bookmark, string title, string ipAddr, string bookmarkImgSrc)
        {
            string strCity = string.Empty;
            string strState = string.Empty;
            string strCountry = string.Empty;
            string resp = null;

            if (Session["User"] == null)
            {
                resp = null;                    // which will open Login Window
            }
            else
            {
                Users user = (Users)Session["User"];
                bmrk.URL = Bookmark;
                if (bmrk.BookmarkExists(user.UserId.ToString()))
                {
                    if (!Request.Url.AbsoluteUri.ToString().Contains("localhost"))
                    {
                        resp = "http://booqmarqs.com/Images/Bookmarked.jpg,Edit," + bmrk.Id;
                    }
                    else
                    {
                        resp = "http://localhost/bookmark/Images/Bookmarked.jpg,Edit," + bmrk.Id;
                    }
                }
                else
                {
                    bmrk.URL = Bookmark;
                    bmrk.Name = title;
                    if(string.IsNullOrEmpty(ipAddr))
                    {
                        ipAddr = Utilities.GetIPAddress();
                    }
                    bmrk.IpAddr = ipAddr;
                    Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);
                    bmrk.OptID = 1;                 //Insert, Create
                    bmrk.FolderId = 27;              //Default Folder
                    bmrk.IsFolder = false;
                    bmrk.CreatedDate = DateTime.Now.ToString();
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.City = strCity;
                    bmrk.Country = strState;
                    bmrk.ManageBookmark();
                    if (!Request.Url.AbsoluteUri.ToString().Contains("localhost"))
                    {
                        resp = "http://booqmarqs.com/Images/Bookmarked.jpg";
                    }
                    else
                    {
                        resp = "http://localhost/bookmark/Images/Bookmarked.jpg";
                    }
                }
            }
            return resp;
        }

        //From Booqmarq Link
        public ActionResult add()
        {
            Users user = (Users)Session["User"];
            string did = Request.QueryString["did"];
            Domain _domain = new Bookmark.Domain();
            did = Utilities.DecryptString(did);
            _domain = _domain.GetDomains(did);

            if (!ValidateBookmarkRequest(_domain, Request.UrlReferrer.ToString()))
            {
                ViewBag.Ack = "Client Id does not match the domain verified";
                return View("../Home/Error");
            }
            ViewBag.URL = Request.UrlReferrer.ToString();
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewBag.AddType = "Bookmark";
            return View("../Bookmark/NewBMFolder");
        }


        //From Booqmarq Ext
        public ActionResult addBmrk()
        {
            Users user = (Users)Session["User"];
            string strURL = Request.QueryString["url"];
            string strName = Request.QueryString["name"];
            ViewBag.URL = strURL;
            ViewBag.Name = strName;
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewBag.AddType = "Bookmark";
            return View("../Bookmark/NewBMFolder");
        }


        public ActionResult ClientPage()
        {
            return View();
        }

        public ActionResult ScriptCode(string txtDomain, string txtExistingDomain, string ddDomains, string txtHeight, string txtWidth, string chkShowCount)
        {
            string domainId = string.Empty;
            Users user = (Users)Session["User"];
            List<Domain> lstDomains = new List<Domain>();
            lstDomains = domain.GetDomains(null, null, null, user.UserId.ToString());
            ViewData["domains"] = lstDomains;
            ViewData["Preview"] = null;
            if (Request.Form["btnPreview"] != null)
            {
                if (!string.IsNullOrEmpty(txtHeight))
                    ViewData["txtHeight"] = txtHeight;
                if (!string.IsNullOrEmpty(txtWidth))
                    ViewData["txtWidth"] = txtWidth;
                if (bool.Parse(chkShowCount))
                    ViewData["chkShowCount"] = true;
                else
                    ViewData["chkShowCount"] = false;
                ViewData["Preview"] = "1";
            }
            else if (Request.Form["btnSave"] != null)
            {
                ViewData["Preview"] = "1";
                Domain _domain = new Bookmark.Domain();
                _domain.Height = txtHeight;
                _domain.Width = txtWidth;
                _domain.ShowCount = chkShowCount;


                if (string.IsNullOrEmpty(txtDomain))
                {
                    _domain.Id = double.Parse(ddDomains);
                    _domain.OptID = 2;
                    _domain.ModifiedDate = DateTime.Now;
                    _domain.Name = txtExistingDomain;
                    _domain.ManageDomain(out domainId);
                }
                else
                {
                    _domain.Name = txtDomain;
                    _domain.OptID = 1;                    
                    _domain.CreatedDate = DateTime.Now;
                    _domain.CreatedUserId = user.UserId.ToString();
                    _domain.ManageDomain(out domainId);
                }                          

                _domain.Script = @"<div id=""divBqmrq""/><script id=""bqmrq"" src=""http://booqmarqs.com/Bookmark.1.249.js?h="+ txtHeight + "&w="+ txtWidth+"&did=" + Utilities.EncryptString(domainId) + "\"></script>";
                ViewData["ScriptCode"] = _domain.Script;
                ViewData["DirectLink"] = @"http://booqmarqs.com/bookmark/add?did=" + Utilities.EncryptString(domainId);     // + "&URL=[YourUrlToBeBookmarked]"
                _domain.OptID = 5;
                _domain.Id = double.Parse(domainId);
                _domain.ManageDomain(out domainId);
                

                ViewData["txtHeight"] = txtHeight;
                ViewData["txtWidth"] = txtWidth;

                if (bool.Parse(chkShowCount))
                    ViewData["chkShowCount"] = true;
                else
                    ViewData["chkShowCount"] = false;
                ViewBag.Ack = "Domain registered and script saved successfully.";
            }
            else
            {
                if (lstDomains.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ddDomains))
                    {
                        double domId = double.Parse(ddDomains);
                        var varDom = lstDomains.Where(x => x.Id == domId).ToList();
                        ViewData["txtExistingDomain"] = varDom[0].Name;
                        ViewData["ScriptCode"] = varDom[0].Script;
                        ViewData["txtHeight"] = varDom[0].Height;
                        ViewData["txtWidth"] = varDom[0].Width;
                        ViewData["chkShowCount"] = varDom[0].ShowCount;
                        ViewData["DirectLink"] = @"http://booqmarqs.com/bookmark/add?did=" + Utilities.EncryptString(domId.ToString());
                    }
                    else
                    {
                        ViewData["txtExistingDomain"] = lstDomains[0].Name;
                        ViewData["ScriptCode"] = lstDomains[0].Script;
                        ViewData["txtHeight"] = lstDomains[0].Height;
                        ViewData["txtWidth"] = lstDomains[0].Width;
                        ViewData["chkShowCount"] = lstDomains[0].ShowCount;
                        ViewData["DirectLink"] = @"http://booqmarqs.com/bookmark/add?did=" + Utilities.EncryptString(lstDomains[0].Id.ToString());
                    }
                }
                else  //First time, new
                {
                    ViewData["chkShowCount"] = false;
                }
            }

            return View(lstDomains);
        }

        public string GetDomainInfo(string ddDomains)
        {
            Users user = (Users)Session["User"];
            var lstDomains = domain.GetDomains(user.UserId.ToString(), ddDomains);
            return lstDomains[0].Name + "," + lstDomains[0].Height + "," + lstDomains[0].Width + "," + bool.Parse(lstDomains[0].ShowCount) + "," + lstDomains[0].Script;
        }

        public ActionResult MyBookmarks(string txtSearch)
        {
            Users user = (Users)Session["User"];
            Utilities.LogMessage("I", "MyBookmarks", user.Email);
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(txtSearch, null, null, user.UserId.ToString(), null, null);
            return View(lstBookmarks);
        }

        public ActionResult ExtBookmarks(string txtSearch)
        {
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(txtSearch, null, null, user.UserId.ToString(), null, null);
            if(lstBookmarks == null || lstBookmarks.Count<=1)
            {
                ViewBag.Count = 0;
            }
            Utilities.LogMessage("I", "ExtBookmarks", user.Email);
            return View(lstBookmarks);
        }

        public ActionResult Domains(string txtDomain, int page = 1, string sort = "Name", string sortdir = "asc", string search = "")
        {
            if (Request.Form["AddDomain"] != null)
            {
                Users user = (Users)Session["User"];
                Domain _domain = new Domain();
                string domId = string.Empty;
                _domain.OptID = 1;
                _domain.Name = txtDomain;
                _domain.ShowCount = false.ToString();
                _domain.CreatedUserId = user.UserId.ToString();
                _domain.ManageDomain(out domId);
                ViewBag.Ack = "Domain Added Successfully. Now generate script for the new domain in \"Script\"";
            }
            int pageSize = 5;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetDomains(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            return View(data);
        }

        public List<BookmarkCls> GetReports(string search, string sort, string sortdir, int skip, int pageSize, out string totalRecord, string txtCreatedFrom, string txtCreatedTo, string ddDomains)
        {
            if (!string.IsNullOrEmpty(ddDomains))
            {
                BookmarkCls bmrk = new BookmarkCls();
                Users user = (Users)Session["User"];
                List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
                string strQuery = string.Empty;

                if (string.IsNullOrEmpty(txtCreatedFrom) && string.IsNullOrEmpty(txtCreatedTo) && string.IsNullOrEmpty(search))
                    lstBookmarks = bmrk.GetReports(search, sort, sortdir, user.UserId.ToString(), txtCreatedFrom, txtCreatedTo, ddDomains, out totalRecord);
                else
                {

                    Domain _domain = new Domain();
                    List<Domain> lstDomains = new List<Domain>();
                    lstDomains = _domain.GetDomains(user.UserId.ToString(), ddDomains);

                    strQuery = " SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count from Bookmarks where IsFolder = 0 and URL like '%" + lstDomains[0].Name + "%' ";

                    if (!string.IsNullOrEmpty(search))
                    {
                        strQuery += " and URL like '%" + search + "%'";
                    } 
                    if (!string.IsNullOrEmpty(txtCreatedFrom) && !string.IsNullOrEmpty(txtCreatedTo))
                    {
                        strQuery += " and CreatedDateTime between '" + txtCreatedFrom + "' and '" + txtCreatedTo + "'";
                    }

                    strQuery += " GROUP BY url, createddatetime, city, Country, Id, FolderId ";

                    lstBookmarks = bmrk.GetReports(strQuery, out totalRecord);
                }

                return lstBookmarks;
            }
            else
            {
                totalRecord = "0";
                return null;
            }
        }

        public List<BookmarkCls> GetChartData(string search, string sort, string sortdir, int skip, int pageSize, out string totalRecord, string txtCreatedFrom, string txtCreatedTo, string ddDomains, string ddScaleType)
        {
            if (!string.IsNullOrEmpty(ddDomains))
            {
                BookmarkCls bmrk = new BookmarkCls();
                Users user = (Users)Session["User"];
                List<BookmarkCls> lstBookmarks = new List<BookmarkCls>();
                string strQuery = string.Empty;
                if(string.IsNullOrEmpty(ddScaleType))
                {
                    ddScaleType = "1";
                }

                totalRecord = string.Empty;
                if (ddScaleType.Equals("1") || string.IsNullOrEmpty(ddScaleType))
                {
                    if (string.IsNullOrEmpty(txtCreatedFrom) && string.IsNullOrEmpty(txtCreatedTo) && string.IsNullOrEmpty(search))
                        lstBookmarks = bmrk.GetCharts(search, sort, sortdir, user.UserId.ToString(), txtCreatedFrom, txtCreatedTo, ddDomains, out totalRecord, ddScaleType);
                    else
                    {
                        Domain _domain = new Domain();
                        List<Domain> lstDomains = new List<Domain>();
                        lstDomains = _domain.GetDomains(user.UserId.ToString(), ddDomains);

                        strQuery = "  SELECT url, COUNT(url) AS URLCount from ( SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count from Bookmarks where IsFolder = 0 and URL like '%" + lstDomains[0].Name + "%' ";

                        if (!string.IsNullOrEmpty(search))
                        {
                            strQuery += " and URL like '%" + search + "%'";
                        }
                        if (!string.IsNullOrEmpty(txtCreatedFrom) && !string.IsNullOrEmpty(txtCreatedTo))
                        {
                            strQuery += " and CreatedDateTime between '" + txtCreatedFrom + "' and '" + txtCreatedTo + "'";
                        }

                        strQuery += " GROUP BY url, createddatetime, city, Country, Id, FolderId) as bookmarks GROUP BY url";

                        lstBookmarks = bmrk.GetURLReports(strQuery, out totalRecord);
                    }
                }
                else if (ddScaleType.Equals("2"))
                {
                    if (string.IsNullOrEmpty(txtCreatedFrom) && string.IsNullOrEmpty(txtCreatedTo) && string.IsNullOrEmpty(search))
                        lstBookmarks = bmrk.GetCharts(search, sort, sortdir, user.UserId.ToString(), txtCreatedFrom, txtCreatedTo, ddDomains, out totalRecord, ddScaleType);
                    else
                    {
                        Domain _domain = new Domain();
                        List<Domain> lstDomains = new List<Domain>();
                        lstDomains = _domain.GetDomains(user.UserId.ToString(), ddDomains);

                        strQuery = "  SELECT city, COUNT(city) AS CityCount from ( SELECT url, createddatetime, city, country, Id, FolderId, COUNT(url) AS URLCount, SUM(COUNT(url)) OVER() AS total_count from Bookmarks where IsFolder = 0 and URL like '%" + lstDomains[0].Name + "%' ";

                        if (!string.IsNullOrEmpty(search))
                        {
                            strQuery += " and URL like '%" + search + "%'";
                        }
                        if (!string.IsNullOrEmpty(txtCreatedFrom) && !string.IsNullOrEmpty(txtCreatedTo))
                        {
                            strQuery += " and CreatedDateTime between '" + txtCreatedFrom + "' and '" + txtCreatedTo + "'";
                        }

                        strQuery += " GROUP BY url, createddatetime, city, Country, Id, FolderId) as bookmarks GROUP BY city";

                        lstBookmarks = bmrk.GetCityReports(strQuery, out totalRecord);
                    }
                }


                return lstBookmarks;
            }
            else
            {
                totalRecord = "0";
                return null;
            }
        }


        public List<Domain> GetDomains(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            Domain domain = new Domain();
            Users user = (Users)Session["User"];
            List<Domain> lstDomains = new List<Domain>();
            lstDomains = domain.GetDomains(search, sort, sortdir, user.UserId.ToString());
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
            ViewBag.Ack = "Bookmark saved successfully";
            return View();
        }

        public ActionResult Reports(int page =1, string sort = "Name", string sortdir = "asc", string search = "", string txtCreatedFrom = "", string txtCreatedTo = "", string ddDomains = "", string ddScaleType = "")
        {
            double domainId = 0;
            int loopCount = 0;
            string[] strLabels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            Dictionary<string, string> bmrkDict = new Dictionary<string, string>();
            List<BookmarkCls> lstBmrk = new List<BookmarkCls>();
            string totalRecord = "0";
            string temp = string.Empty;

            Users user = (Users)Session["User"];
            List<Domain> lstDomains = new List<Domain>();
            lstDomains = domain.GetDomains(null, null, null, user.UserId.ToString());
            ViewData["domains"] = lstDomains;
            Session["ChartData"] = null;
            Session["bmrkDict"] = null;

            if(lstDomains.Count <=0)
            {
                ViewData["chkShowCount"] = false;
                ViewData["domains"] = lstDomains;
                return View("../Bookmark/ScriptCode", lstDomains);
            }

            int pageSize = 10;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;

            if (string.IsNullOrEmpty(ddDomains))
            {
                if (lstDomains != null && lstDomains.Count > 0)
                {
                    ViewData["selectedIndex"] = lstDomains[0].Name;
                    domainId = lstDomains[0].Id;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ddDomains))
                {
                    double.TryParse(ddDomains.ToString(), out domainId);
                }
            }

            var gridData = GetReports(search, sort, sortdir, skip, pageSize, out totalRecord, txtCreatedFrom, txtCreatedTo, domainId.ToString());
            //lstBmrk = gridData.ToList();

            var varBookmarks = gridData.AsQueryable();

            if (pageSize > 0)
            {
                varBookmarks = varBookmarks.Skip(skip).Take(pageSize);
            }



            var chartData = GetChartData(search, sort, sortdir, skip, pageSize, out temp, txtCreatedFrom, txtCreatedTo, domainId.ToString(), ddScaleType).ToList();
            var varchartData = chartData.AsQueryable();

            if (chartData.Count > 10)
            {
                //loopCount = 10;
                varchartData = varchartData.Skip(skip).Take(pageSize);
            }
            else
                loopCount = chartData.Count;

            for (int count = 0; count < varchartData.ToList().Count; count++)
            {
                if (ddScaleType.Equals("1") || string.IsNullOrEmpty(ddScaleType))
                {
                    bmrkDict.Add(strLabels[count], varchartData.ToList()[count].URL);
                }
                else
                {
                    bmrkDict.Add(strLabels[count], varchartData.ToList()[count].City);
                }
            }

            if (string.IsNullOrEmpty(ddScaleType))
            {
                ddScaleType = "1";               
            }

            ViewBag.Dictionary = bmrkDict;
            ViewBag.TotalBookmarks = totalRecord;
            ViewBag.TotalRows = Int32.Parse(totalRecord);
            Session["ChartData"] = varchartData.ToList();
            Session["ScaleType"] = ddScaleType;
            Session["bmrkDict"] = bmrkDict;
            ViewBag.search = search;
            LoadChartScaleDropDown();
            return View(varBookmarks.ToList());
        }

        public ActionResult Manage()
        {
            Users user = (Users)Session["User"];
            ViewBag.BookmarkId = Request.QueryString["id"];

            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            BookmarkCls tempBmrk = new BookmarkCls();
            tempBmrk.Id = 27;
            tempBmrk.Name = "Default";
            lstFolders.Add(tempBmrk);

            ViewData["ddFolders"] = lstFolders;

            if (Request.QueryString["action"].ToString().Equals("AddB"))
            {
                ViewBag.AddType = "Bookmark";
                return View("../Bookmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("AddF"))
            {
                return View("../Bookmark/NewBMFolder");
            }

            else if (ViewBag.BookmarkId != "27")
            {
                if (Request.QueryString["action"].ToString().Equals("Move"))
                {
                    BookmarkCls bmrk = new BookmarkCls();
                    bmrk = bmrk.GetBookmarkFromId(ViewBag.BookmarkId, user.UserId);
                    if (!bmrk.IsFolder)
                        ViewBag.MoveType = "Bookmark";
                    ViewBag.Name = bmrk.Name;
                    return View("../Bookmark/MoveBMFolder", bmrk);
                }
                else if (Request.QueryString["action"].ToString().Equals("Edit"))
                {
                    BookmarkCls bmrk = new BookmarkCls();
                    bmrk = bmrk.GetBookmarkFromId(ViewBag.BookmarkId, user.UserId);
                    if (!bmrk.IsFolder)
                        ViewBag.EditType = "Bookmark";
                    return View("../Bookmark/EditBMFolder", bmrk);
                }
                else
                {
                    BookmarkCls bmrk = new BookmarkCls();
                    bmrk = bmrk.GetBookmarkFromId(ViewBag.BookmarkId, user.UserId);
                    if (!bmrk.IsFolder)
                        ViewBag.DeleteType = "Bookmark";
                    ViewBag.Name = bmrk.Name;
                    if (bmrk.IsFolder)
                    {
                        ViewBag.Info = "Deleting folder will delete all bookmarks within. Are you sure you want to delete folder? ";
                    }
                    else
                    {
                        ViewBag.Info = "Are you sure you want to delete ?";
                    }
                    return View("../Bookmark/Delete", bmrk);
                }
            }
            else
            {
                ViewBag.Ack = "Default Folder cannot be modified";
                return View("../Bookmark/BMAdded");
            }
        }

        public ActionResult NewBMFolder(string txtLink, string txtName, string ddFolder, string HFBookmarkId)
        {
            Users user = (Users)Session["User"];
            BookmarkCls bmrk = new BookmarkCls();
            string strCity = string.Empty;
            string strState = string.Empty;
            string strCountry = string.Empty;
            string ipAddr = Utilities.GetIPAddress();
            Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);
            List<BookmarkCls> lstFolders = new List<BookmarkCls>();

            if (!string.IsNullOrEmpty(txtLink))
            {
                if(!txtLink.StartsWith("http://") || !txtLink.StartsWith("https://"))
                {
                    txtLink = "http://" + txtLink;
                }
                bmrk.URL = txtLink;
                bmrk.IsFolder = false;
            }
            else
            {
                bmrk.IsFolder = true;
            }

            if (bmrk.IsFolder && !bmrk.FolderExists(user.UserId.ToString(), ddFolder, txtName))
            {
                bmrk.OptID = 1;
                bmrk.Name = txtName;
                bmrk.FolderId = double.Parse(ddFolder);
                bmrk.CreatedDate = DateTime.Now.ToString();
                bmrk.CreatedUserId = user.UserId.ToString();
                bmrk.City = strCity;
                bmrk.Country = strState;
                bmrk.ManageBookmark();
            }
            else
            {
                ViewBag.Ack = "Folder already exists";
                lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
                BookmarkCls tempBmrk = new BookmarkCls();
                tempBmrk.Id = 27;
                tempBmrk.Name = "Default";
                lstFolders.Add(tempBmrk);
                ViewData["ddFolders"] = lstFolders;
                return View("../Bookmark/NewBMFolder");
            }
            lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View("../Bookmark/BMAdded");
        }

        public ActionResult EditBMFolder(BookmarkCls bmrk)
        {
            Users user = (Users)Session["User"];
            bmrk.OptID = 2;  //Update
            bmrk.ModifiedDate = DateTime.Now.ToString();
            bmrk.ManageBookmark();
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;           
            ViewData["Refresh"] = "true";
            return View("../Bookmark/BMAdded");
        }

        public ActionResult Delete(string HFBookmarkId)
        {
            if (Request.Form["btnYes"] != null)
            {
                Users user = (Users)Session["User"];
                BookmarkCls bmrk = new BookmarkCls();
                bmrk= bmrk.GetBookmarkFromId(HFBookmarkId, user.UserId);
                if(bmrk.IsFolder)
                    bmrk.OptID = 4; //Delete  folder
                else
                    bmrk.OptID = 3; //Delete bookmark
                bmrk.CreatedUserId = user.UserId.ToString();
                bmrk.ManageBookmark();               
            }
            ViewData["Refresh"] = "true";
            return View("../Bookmark/BMAdded");
        }

        public ActionResult MoveBMFolder(BookmarkCls bmrk)
        {
            Users user = (Users)Session["User"];
            bmrk.OptID = 5; //Update folder
            bmrk.ModifiedDate = DateTime.Now.ToString();
            bmrk.ManageBookmark();
            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View("../Bookmark/BMAdded");
        }

        public ActionResult Import(HttpPostedFileBase fileImport)
        {
            string folderId = string.Empty;
            bool IfPartialSuccess = false;
            try
            {

                if (fileImport != null)
                {
                    string strCity = string.Empty;
                    string strState = string.Empty;
                    string strCountry = string.Empty;
                    string ipAddr = Utilities.GetIPAddress();
                    Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);

                    StreamReader reader = new StreamReader(fileImport.InputStream);
                    string fileText = reader.ReadToEnd();

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(fileText);

                    List<HtmlNode> htmlNodes = new List<HtmlNode>();
                    htmlNodes = doc.DocumentNode.Descendants().ToList();
                    Users user = (Users)Session["User"];

                    Utilities.LogMessage("I", "Import", user.Email);
                    try
                    {
                        fileImport.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~/Logs/" + fileImport.FileName));
                    }
                    catch
                    {

                    }

                    foreach (HtmlNode node in htmlNodes)
                    {
                        try
                        {
                            if (node.Attributes.Count > 0)
                            {
                                if (node.Name.Trim().ToUpper().Equals("H3"))
                                {
                                    //Create Folder
                                    bmrk.IsFolder = true;
                                    bmrk.FolderId = 27;
                                    bmrk.URL = string.Empty;
                                    folderId = string.Empty;
                                }
                                else if (node.Name.Trim().ToUpper().Equals("A") && node.Attributes[0].Value.ToString().StartsWith("http"))
                                {
                                    //Create Bookmark
                                    bmrk.IsFolder = false;
                                    bmrk.URL = node.Attributes[0].Value;
                                    bmrk.FolderId = double.Parse(folderId);
                                }
                                else
                                {
                                    continue;
                                }

                                bmrk.Name = node.InnerText;
                                bmrk.OptID = 1;
                                bmrk.CreatedUserId = user.UserId.ToString();
                                bmrk.CreatedDate = DateTime.Now.ToString();
                                bmrk.City = strCity;
                                bmrk.Country = strState;
                                bmrk.IpAddr = ipAddr;

                                if (bmrk.IsFolder)
                                {
                                    if (!bmrk.FolderExists(user.UserId.ToString(), bmrk.Name, out folderId))
                                    {
                                        bmrk.ManageBookmark(out folderId);
                                    }
                                }
                                else
                                {
                                    if (!bmrk.BookmarkUnderFolderExists(user.UserId.ToString()))
                                    {
                                        bmrk.ManageBookmark();
                                    }
                                }
                                IfPartialSuccess = true;

                            }
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    ViewBag.Ack = "Bookmarks imported successfully. Click on \"Bookmarks\" tab to view all your bookmarks.";
                }

            }
            catch (Exception ex)
            {
                if (IfPartialSuccess)
                {
                    ViewBag.Ack = "Bookmarks imported. Howere there were warnings, click on \"Bookmarks\" tab to view your bookmarks and make necessary updates if required.";
                }
                else
                {
                    Utilities.EmailException(ex);
                    ViewBag.Ack = "Please try again, there seem to be an error";
                }
            }


            return View();
        }

        public ActionResult Extensions()
        {
            return View();
        }

        private void LoadChartData(string ChartType)
        {
            List<BookmarkCls> lstBmrk = new List<BookmarkCls>();
            lstBmrk = Session["ChartData"] as List<BookmarkCls>;
            Dictionary<string, string> bmrkDict  = Session["bmrkDict"] as Dictionary<string, string>;

            if (Session["ScaleType"].ToString() == "1")
            {
                if (!string.IsNullOrEmpty(ChartType))
                {
                    ViewBag.MyXAxisValues = bmrkDict.Keys.ToArray();
                }
                else
                {
                    ViewBag.MyXAxisValues = lstBmrk.Select(x => x.URL).ToArray();
                }
                ViewBag.MyYAxisValues = lstBmrk.Select(x => x.Count).ToArray();
            }
            else if (Session["ScaleType"].ToString() == "2")
            {
                //ViewBag.MyXAxisValues = bmrkDict.Keys.ToArray();
                ViewBag.MyXAxisValues = lstBmrk.Select(x => x.City).ToArray();
                ViewBag.MyYAxisValues = lstBmrk.Select(x => x.Count).ToArray();
                //ViewBag.MyXAxisValues = lstBmrk.Select(x => x.City).Distinct().ToArray();
                //ViewBag.MyYAxisValues = lstBmrk.GroupBy(x => x.City).Select(y => y.Sum(z => Convert.ToInt64(z.Count)).ToString()).ToArray();
            }
            //if (Session["ScaleType"].ToString() == "2")
            //{
            //    ViewBag.MyXAxisValues = bmrkDict.Keys.ToArray();
            //    ViewBag.MyYAxisValues = lstBmrk.Select(x => x.Country).ToArray();
            //}           
        }

        public ActionResult BarCharts()
        {
            LoadChartData(null);
            return View();
        }

        public ActionResult PieCharts()
        {
            LoadChartData("Pie");
            return View();
        }

        public ActionResult LineCharts()
        {
            LoadChartData(null);
            return View();
        }

        public ActionResult BarChartsHorizon()
        {
            LoadChartData(null);
            return View();
        }

        private void LoadChartScaleDropDown()
        {
            List<SelectListItem> lstSacleTypes = new List<SelectListItem>();
            SelectListItem sli = new SelectListItem();
            sli.Text = "URL Vs Count";
            sli.Value = "1";
            sli.Selected = true;
            lstSacleTypes.Add(sli);
            sli = new SelectListItem();
            sli.Text = "City Vs Count";
            sli.Value = "2";
            lstSacleTypes.Add(sli);
            //sli = new SelectListItem();
            //sli.Text = "URL Vs Country";
            //sli.Value = "3";
            //lstSacleTypes.Add(sli);
            ViewBag.ddScaleType = lstSacleTypes;
        }

        public void extImport(string Id, string Bookmarks)
        {
            List<ChromeBookmark> lstChromeBmrks = JsonConvert.DeserializeObject<List<ChromeBookmark>>(Bookmarks);
            Utilities.LogMessage("I", "extImport", Bookmarks);
            string lclipAddr = Utilities.GetIPAddress();
            Utilities.GetLocation(lclipAddr, ref lclCity, ref lclState, ref lclCountry);
            Users user = new Users();
            user = user.GetUser(Id.Substring(6));

            Utilities.LogMessage("I", "extImport", user.Email);
            try
            {
                if (user.UserId != 0)
                {
                    if (lstChromeBmrks != null && lstChromeBmrks.Count > 0)
                    {
                        ProcessBookmarks(user.UserId.ToString(), lstChromeBmrks, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.EmailException(ex);
            }
            //return View();
        }

        private void ProcessBookmarks(string userId, List<ChromeBookmark> lstChromeBmrks, string folderId)
        {
            foreach (ChromeBookmark chromeBmrk in lstChromeBmrks)
            {
                try
                {
                    if (chromeBmrk.children != null && chromeBmrk.parentId != null)
                    {
                        //Create Folder
                        bmrk.IsFolder = true;
                        bmrk.FolderId = 27;
                        bmrk.URL = string.Empty;
                        folderId = string.Empty;
                    }
                    else if (chromeBmrk.url != null)
                    {
                        //Create Bookmark
                        bmrk.IsFolder = false;
                        bmrk.URL = chromeBmrk.url;
                        bmrk.FolderId = double.Parse(folderId);
                    }
                    else
                    {
                        if (chromeBmrk.children != null)
                        {
                            ProcessBookmarks(userId, chromeBmrk.children, folderId);
                        }
                    }

                    bmrk.Name = chromeBmrk.title;
                    bmrk.OptID = 1;
                    bmrk.CreatedUserId = userId.ToString();
                    bmrk.CreatedDate = DateTime.Now.ToString();
                    bmrk.City = lclCity;
                    bmrk.Country = lclState;
                    bmrk.IpAddr = lclipAddr;

                    if (bmrk.IsFolder)
                    {
                        if (!bmrk.FolderExists(userId.ToString(), bmrk.Name, out folderId))
                        {
                            bmrk.ManageBookmark(out folderId);
                        }
                        ProcessBookmarks(userId, chromeBmrk.children, folderId);
                    }
                    else
                    {
                        if (!bmrk.BookmarkUnderFolderExists(userId.ToString()))
                        {
                            bmrk.ManageBookmark();
                        }
                    }

                }

                catch
                {
                    continue;
                }
            }

        }
    }



    public class ChromeBookmark
    {
        public string title { get; set; }
        public string url { get; set; }
        public string id { get; set; }
        public string parentId { get; set; }

        public int index { get; set; }
        public double dateAdded { get; set; }
        public double dateGroupModified { get; set; }
        public List<ChromeBookmark> children { get; set; }

    }



    //    public static class ChartTheme
    //    {
    //        // Review: Need better names.

    //        public const string Blue = @"<Chart BackColor="" #D3DFF0"" BackGradientStyle="" TopBottom"" BackSecondaryColor="" White"" BorderColor="" 26, 59, 105"" BorderlineDashStyle="" Solid"" BorderWidth="" 2"" Palette="" BrightPastel"">
    //    	                            <ChartAreas>
    //        	                            <ChartArea Name="" Default"" _Template_="" All"" BackColor="" 64, 165, 191, 228"" BackGradientStyle="" TopBottom"" BackSecondaryColor="" White"" BorderColor="" 64, 64, 64, 64"" BorderDashStyle="" Solid"" ShadowColor="" Transparent"" />
    //                                        </ChartAreas>
    //    	                                <Legends>
    //        	                            <Legend _Template_="" All"" BackColor="" Transparent"" Font="" Trebuchet MS, 8.25pt, style=Bold"" IsTextAutoFit="" False"" />
    //                                        </Legends>
    //    	                                <BorderSkin SkinStyle="" Emboss"" />    
    //                                    </Chart>";

    //        public const string Green =
    //@"<Chart BackColor="" #C9DC87"" BackGradientStyle="" TopBottom"" BorderColor="" 181, 64, 1"" BorderWidth="" 2"" BorderlineDashStyle="" Solid"" Palette="" BrightPastel"">
    //    	  <ChartAreas>
    //        	    <ChartArea Name="" Default"" _Template_="" All"" BackColor="" Transparent"" BackSecondaryColor="" White"" BorderColor="" 64, 64, 64, 64"" BorderDashStyle="" Solid"" ShadowColor="" Transparent"">
    //           	      <AxisY LineColor="" 64, 64, 64, 64"">
    //                	        <MajorGrid Interval="" Auto"" LineColor="" 64, 64, 64, 64"" />
    //                	        <LabelStyle Font="" Trebuchet MS, 8.25pt, style=Bold"" />

    //            </AxisY>
    //            	      <AxisX LineColor="" 64, 64, 64, 64"">
    //                	        <MajorGrid LineColor="" 64, 64, 64, 64"" />
    //                	        <LabelStyle Font="" Trebuchet MS, 8.25pt, style=Bold"" />

    //            </AxisX>
    //            	      <Area3DStyle Inclination="" 15"" IsClustered="" False"" IsRightAngleAxes="" False"" Perspective="" 10"" Rotation="" 10"" WallWidth="" 0"" />

    //        </ChartArea>

    //    </ChartAreas>
    //      <Legends>
    //        	    <Legend _Template_="" All"" Alignment="" Center"" BackColor="" Transparent"" Docking="" Bottom"" Font="" Trebuchet MS, 8.25pt, style=Bold"" IsTextAutoFit="" False"" LegendStyle="" Row"">

    //        </Legend>

    //    </Legends>
    //    	  <BorderSkin SkinStyle="" Emboss"" />

    //</Chart>";

    //        public const string Vanilla =
    //@"<Chart Palette="" SemiTransparent"" BorderColor="" #000"" BorderWidth="" 2"" BorderlineDashStyle="" Solid"">
    //    	<ChartAreas>
    //        	    <ChartArea _Template_="" All"" Name="" Default"">
    //            	            <AxisX>
    //                	                <MinorGrid Enabled="" False"" />
    //                	                <MajorGrid Enabled="" False"" />

    //            </AxisX>
    //            	            <AxisY>
    //                	                <MajorGrid Enabled="" False"" />
    //                	                <MinorGrid Enabled="" False"" />

    //            </AxisY>

    //        </ChartArea>

    //    </ChartAreas>

    //</Chart>";

    //        public const string Vanilla3D =
    //@"<Chart BackColor="" #555"" BackGradientStyle="" TopBottom"" BorderColor="" 181, 64, 1"" BorderWidth="" 2"" BorderlineDashStyle="" Solid"" Palette="" SemiTransparent"" AntiAliasing="" All"">
    //    	    <ChartAreas>
    //        	        <ChartArea Name="" Default"" _Template_="" All"" BackColor="" Transparent"" BackSecondaryColor="" White"" BorderColor="" 64, 64, 64, 64"" BorderDashStyle="" Solid"" ShadowColor="" Transparent"">
    //            	            <Area3DStyle LightStyle="" Simplistic"" Enable3D="" True"" Inclination="" 30"" IsClustered="" False"" IsRightAngleAxes="" False"" Perspective="" 10"" Rotation="" -30"" WallWidth="" 0"" />

    //        </ChartArea>

    //    </ChartAreas>

    //</Chart>";

    //        public const string Yellow =
    //@"<Chart BackColor="" #FADA5E"" BackGradientStyle="" TopBottom"" BorderColor="" #B8860B"" BorderWidth="" 2"" BorderlineDashStyle="" Solid"" Palette="" EarthTones"">
    //    	  <ChartAreas>
    //        	    <ChartArea Name="" Default"" _Template_="" All"" BackColor="" Transparent"" BackSecondaryColor="" White"" BorderColor="" 64, 64, 64, 64"" BorderDashStyle="" Solid"" ShadowColor="" Transparent"">
    //            	      <AxisY>
    //                	        <LabelStyle Font="" Trebuchet MS, 8.25pt, style=Bold"" />

    //            </AxisY>
    //            	      <AxisX LineColor="" 64, 64, 64, 64"">
    //                	        <LabelStyle Font="" Trebuchet MS, 8.25pt, style=Bold"" />

    //            </AxisX>

    //        </ChartArea>

    //    </ChartAreas>
    //    	  <Legends>
    //        	    <Legend _Template_="" All"" BackColor="" Transparent"" Docking="" Bottom"" Font="" Trebuchet MS, 8.25pt, style=Bold"" LegendStyle="" Row"">

    //        </Legend>

    //    </Legends>
    //    	  <BorderSkin SkinStyle="" Emboss"" />

    //</Chart>";
    //    }
}
