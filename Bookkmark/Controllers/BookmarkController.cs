using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using HtmlAgilityPack;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace Bookmark.Controllers
{
    public class BookmarkController : BaseController
    {
        private BookmarkCls bmrk = new BookmarkCls();
        private Domain domain = new Domain(); 

        public ActionResult Show(string did, string lnk, string t)
        {
            Domain _domain = new Bookmark.Domain();
            did = Utilities.DecryptString(did);
            _domain = _domain.GetDomains(did);

            if(string.IsNullOrEmpty(lnk))
            {
                lnk = Request.UrlReferrer.ToString();
            }

            ViewData["Bookmark"] = lnk;
            ViewData["ht"] = _domain.Height;
            ViewData["wd"] = _domain.Width;
            ViewData["title"] = t;

            if (!ValidateBookmarkRequest(_domain, lnk))
                return null;


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
                ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmark.jpg";
            }
            else
            {
                Users user = (Users)Session["User"];
                BookmarkCls bookmark = new BookmarkCls();
                bookmark.URL = lnk;
                if (bookmark.BookmarkExists(user.UserId.ToString()))
                {
                    ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmarked.jpg";
                }
                else
                {
                    ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmark.jpg"; //temporary
                    //ViewData["bookmarkImgSrc"] = "../../Images/Bookmark.jpg"; //actual later
                }
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

        public string addbm(string Bookmark, string title, string ipAddr, string bookmarkImgSrc)
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
                if (bmrk.BookmarkExists(user.UserId.ToString()))
                {
                    resp = "../../Bookmark/Images/Bookmarked.jpg,Edit," + bmrk.Id;
                }
                else
                {
                    bmrk.URL = Bookmark;
                    bmrk.Name = title;
                    bmrk.IpAddr = ipAddr;
                    Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);
                    bmrk.OptID = 1;                 //Insert, Create
                    bmrk.FolderId = 0;              //Default Folder
                    bmrk.IsFolder = false;
                    bmrk.CreatedDate = DateTime.Now.ToString();
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.City = strCity;
                    bmrk.Country = strState;
                    bmrk.ManageBookmark();
                    resp = "../../Bookmark/Images/Bookmarked.jpg";
                }
            }
            return resp;
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
            }
            else if (Request.Form["btnSave"] != null)
            {
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

                          

                _domain.Script = @"<div id=""divBqmrq""/><script id=""bqmrq"" src=""http://booqmarqs.com/Bookmark.1.249.js?did=" + Utilities.EncryptString(domainId) + "\"></script>";
                _domain.OptID = 5;
                _domain.Id = double.Parse(domainId);
                _domain.ManageDomain(out domainId);
                ViewData["ScriptCode"] = _domain.Script;

                ViewData["txtHeight"] = txtHeight;
                ViewData["txtWidth"] = txtWidth;

                if (bool.Parse(chkShowCount))
                    ViewData["chkShowCount"] = true;
                else
                    ViewData["chkShowCount"] = false;
                ViewBag.Ack = "Domain registered and Script saved successfully. Use the generated script in your application to make user bookmark you website.";
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
                    }
                    else
                    {
                        ViewData["txtExistingDomain"] = lstDomains[0].Name;
                        ViewData["ScriptCode"] = lstDomains[0].Script;
                        ViewData["txtHeight"] = lstDomains[0].Height;
                        ViewData["txtWidth"] = lstDomains[0].Width;
                        ViewData["chkShowCount"] = lstDomains[0].ShowCount;
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

        public ActionResult MyBookmarks()
        {
            //string strCity = string.Empty;
            //string strState = string.Empty;
            //string strCountry = string.Empty;
            //GetLocation("69.248.7.33", ref strCity, ref strState, ref strCountry);
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(null, null, null, user.UserId.ToString(), null, null);
            return View(lstBookmarks);
        }

        public ActionResult ExtBookmarks()
        {
            //string strCity = string.Empty;
            //string strState = string.Empty;
            //string strCountry = string.Empty;
            //GetLocation("69.248.7.33", ref strCity, ref strState, ref strCountry);
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(null, null, null, user.UserId.ToString(), null, null);
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


                
                var varBookmarks = lstBookmarks.AsQueryable();

                if (pageSize > 0)
                {
                    varBookmarks = varBookmarks.Skip(skip).Take(pageSize);
                }

                return varBookmarks.ToList();
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
            return View();
        }


        public ActionResult Reports(int page =1, string sort = "Name", string sortdir = "asc", string search = "", string txtCreatedFrom = "", string txtCreatedTo = "", string ddDomains = "", string ddScaleType = "")
        {
            double domainId = 0;
            int loopCount = 0;
            string[] strLabels = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            Dictionary<string, string> bmrkDict = new Dictionary<string, string>();
            List<BookmarkCls> lstBmrk = new List<BookmarkCls>();

            Users user = (Users)Session["User"];
            List<Domain> lstDomains = new List<Domain>();
            lstDomains = domain.GetDomains(null, null, null, user.UserId.ToString());
            ViewData["domains"] = lstDomains;
            Session["ChartData"] = null;
            Session["bmrkDict"] = null;

            int pageSize = 10;
            string totalRecord = string.Empty;
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

            var data = GetReports(search, sort, sortdir, skip, pageSize, out totalRecord, txtCreatedFrom, txtCreatedTo, domainId.ToString());
            lstBmrk = data.ToList();

            if (lstBmrk.Count > 10)
                loopCount = 10;
            else
                loopCount = lstBmrk.Count;

            for (int count = 0; count < loopCount; count++)
            {
                bmrkDict.Add(strLabels[count], lstBmrk[count].URL);
            }

            if (string.IsNullOrEmpty(ddScaleType) || ddScaleType == "1")
            {
               ddScaleType = "1";
                ViewBag.Dictionary = bmrkDict;
            }

            ViewBag.TotalBookmarks = totalRecord;
            ViewBag.TotalRows = data.ToList().Count;
            Session["ChartData"] = data.ToList();
            Session["ScaleType"] = ddScaleType;
            Session["bmrkDict"] = bmrkDict;
            ViewBag.search = search;
            LoadChartScaleDropDown();
            return View(data);
        }



        public ActionResult Manage()
        {
            Users user = (Users)Session["User"];
            ViewBag.BookmarkId = Request.QueryString["id"];

            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;

            if (Request.QueryString["action"].ToString().Equals("AddB") )
            {
                ViewBag.AddType = "Bookmark";
                return View("../Bookmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("AddF"))
            {
                return View("../Bookmark/NewBMFolder");
            }
            else if (Request.QueryString["action"].ToString().Equals("Move"))
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
                if(!bmrk.IsFolder)
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
                return View("../Bookmark/Delete", bmrk);
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

            if (!string.IsNullOrEmpty(txtLink))
            {
                bmrk.URL = txtLink;
                bmrk.IsFolder = false;
            }
            else
            {
                bmrk.IsFolder = true;
            }
            bmrk.Name = txtName;
            bmrk.FolderId = double.Parse(ddFolder);
            bmrk.CreatedDate = DateTime.Now.ToString();
            bmrk.CreatedUserId = user.UserId.ToString();
            bmrk.City = strCity;
            bmrk.Country = strState;
            bmrk.ManageBookmark();

            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult EditBMFolder(BookmarkCls bmrk)
        {
            Users user = (Users)Session["User"];
            //BookmarkCls bmrk = new BookmarkCls();
            //string strCity = string.Empty;
            //string strState = string.Empty;
            //string strCountry = string.Empty;
            //string ipAddr = Utilities.GetIPAddress();
            //Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);

            //if (!string.IsNullOrEmpty(bmrk.URL))
            //{
            //    //bmrk.URL = txtLink;
            //    bmrk.IsFolder = false;
            //}
            //else
            //{
            //    bmrk.IsFolder = true;
            //}
            bmrk.OptID = 3;  //Update
            //bmrk.Name = txtName;
            //bmrk.FolderId = double.Parse(ddFolder);
            bmrk.ModifiedDate = DateTime.Now.ToString();
            bmrk.CreatedUserId = user.UserId.ToString();
            //bmrk.City = strCity;
            //bmrk.Country = strState;
            bmrk.ManageBookmark();

            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult Delete(string HFBookmarkId)
        {
            if (Request.Form["btnYes"] != null)
            {
                Users user = (Users)Session["User"];
                BookmarkCls bmrk = new BookmarkCls();
                bmrk.Id = double.Parse(HFBookmarkId);
                bmrk.OptID = 4; //Delete
                bmrk.CreatedUserId = user.UserId.ToString();
                bmrk.ManageBookmark();               
            }
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult AddDomain(string txtDomain)
        {

            Users user = (Users)Session["User"];
            Domain _domain = new Domain();
            string domId = string.Empty;
            _domain.OptID = 1; //Delete
            _domain.Name = txtDomain;
            _domain.CreatedUserId = user.UserId.ToString();
            _domain.ManageDomain(out domId);
            ViewBag.Ack = "Domain Added Successfully";
            return View();
        }


        public ActionResult Import(HttpPostedFileBase fileImport)
        {
            if (fileImport != null)
            {
                StreamReader reader = new StreamReader(fileImport.InputStream);
                string fileText = reader.ReadToEnd();

                //var regexImage = new Regex(@"data:(?<mime>[\w/\-\.]+);(?<encoding>\w+),(?<data>.>)", RegexOptions.Compiled);

                //var regexComment = new Regex(@"(?=<!--)([\s\S]*?)-->", RegexOptions.Compiled);      //<!--(.*?)-->
                //var match = regexImage.Match(fileText);


                //fileText = regexImage.Replace(fileText, "");
                //fileText = regexComment.Replace(fileText, "");


                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(fileText);

                List<HtmlNode> htmlNodes = new List<HtmlNode>();
                htmlNodes = doc.DocumentNode.Descendants().ToList();
                Users user = (Users)Session["User"];

                foreach (HtmlNode node in htmlNodes)
                {
                    if (node.Attributes.Count > 0)
                    {
                        if (node.Name.Trim().ToUpper().Equals("H3"))
                        {
                            //Create Folder
                            bmrk.IsFolder = true;
                            bmrk.Name = node.InnerText;
                        }
                        else if (node.Name.Trim().ToUpper().Equals("A") && node.Attributes[0].Value.ToString().StartsWith("http"))
                        {
                            //Create Bookmark
                            bmrk.IsFolder = false;
                            bmrk.Name = node.InnerText;
                            bmrk.URL = node.Attributes[0].Value;
                        }
                        else
                        {
                            continue;
                        }

                        bmrk.Name = node.InnerText;
                        bmrk.OptID = 1;
                        bmrk.CreatedUserId = user.UserId.ToString();
                        bmrk.CreatedDate = DateTime.Now.ToString();
                       // bmrk.ManageBookmark();
                    }
                }


                //List<string> hrefTags = new List<string>();

                //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                //{
                //    HtmlAttribute att = link.Attributes["href"];
                //    hrefTags.Add(att.Value);
                //}


                //var linkedPages = doc.DocumentNode.Descendants("a").Select(a=> a.Attributes);

                //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                //{
                //    //HtmlAttribute att = link["href"];
                //    //att.Value = FixLink(att);
                //}

             

            }
            return View();
        }

        public ActionResult MoveBMFolder(string txtLink, string txtName, string ddFolder, string HFBookmarkId)
        {
            Users user = (Users)Session["User"];
            BookmarkCls bmrk = new BookmarkCls();
            //string strCity = string.Empty;
            //string strState = string.Empty;
            //string strCountry = string.Empty;
            //string ipAddr = Utilities.GetIPAddress();
            //Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);

            //if (!string.IsNullOrEmpty(txtLink))
            //{
            //    bmrk.URL = txtLink;
            //    bmrk.IsFolder = false;
            //}
            //else
            //{
            //    bmrk.IsFolder = true;
            //}
            bmrk.OptID = 5; //Update folder
            //bmrk.Name = txtName;
            bmrk.FolderId = double.Parse(ddFolder);
            //bmrk.CreatedDate = DateTime.Now.ToString();
            bmrk.CreatedUserId = user.UserId.ToString();
            //bmrk.City = strCity;
            //bmrk.Country = strState;
            bmrk.ManageBookmark();

            List<BookmarkCls> lstFolders = bmrk.GetFolders(null, null, null, user.UserId.ToString());
            ViewData["ddFolders"] = lstFolders;
            ViewData["Refresh"] = "true";
            return View();
        }

        public ActionResult Extensions()
        {
            return View();
        }

        private void LoadChartData()
        {
            List<BookmarkCls> lstBmrk = new List<BookmarkCls>();
            lstBmrk = Session["ChartData"] as List<BookmarkCls>;
            Dictionary<string, string> bmrkDict  = Session["bmrkDict"] as Dictionary<string, string>;

            if (Session["ScaleType"].ToString() == "1")
            {
                ViewBag.MyXAxisValues = bmrkDict.Keys.ToArray();
                ViewBag.MyYAxisValues = lstBmrk.Select(x => x.Count).ToArray();
            }
            else if (Session["ScaleType"].ToString() == "2")
            {
                ViewBag.MyXAxisValues = lstBmrk.Select(x => x.Country).Distinct().ToArray();
                ViewBag.MyYAxisValues = lstBmrk.GroupBy(x => x.Country).Select(y => y.Sum(z => Convert.ToInt64(z.Count)).ToString()).ToArray();
            }
            //if (Session["ScaleType"].ToString() == "2")
            //{
            //    ViewBag.MyXAxisValues = bmrkDict.Keys.ToArray();
            //    ViewBag.MyYAxisValues = lstBmrk.Select(x => x.Country).ToArray();
            //}           
        }


        public ActionResult BarCharts()
        {
            LoadChartData();
            return View();
        }

        public ActionResult PieCharts()
        {
            LoadChartData();
            return View();
        }

        public ActionResult LineCharts()
        {
            LoadChartData();
            return View();
        }

        public ActionResult BarChartsHorizon()
        {
            LoadChartData();
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
            sli.Text = "Country Vs Count";
            sli.Value = "2";
            lstSacleTypes.Add(sli);
            //sli = new SelectListItem();
            //sli.Text = "URL Vs Country";
            //sli.Value = "3";
            //lstSacleTypes.Add(sli);
            ViewBag.ddScaleType = lstSacleTypes;
        }

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
