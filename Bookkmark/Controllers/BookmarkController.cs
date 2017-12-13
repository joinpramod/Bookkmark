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

        public ActionResult Show(string lnk, string h, string w, string sd, string t)
        {
            ViewData["Bookmark"] = lnk;
            ViewData["ht"] = h;
            ViewData["wd"] = w;
            ViewData["title"] = t;

            //temp
            ViewData["BookmarkCount"] = GetBookmarkCount(lnk);
            ViewData["bookmarkImgSrc"] = "../../Bookmark/Images/Bookmark.jpg";
            return View();

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
            string strCity = string.Empty;
            string strState = string.Empty;
            string strCountry = string.Empty;
            string resp = null;
            Session["bookmark"] = null;
            Session["bookmark"] = bmrk;


            if (Session["User"] == null)
            {
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
                    bmrk.URL = Bookmark;
                    bmrk.Name = title;
                    bmrk.IpAddr = ipAddr;
                    Utilities.GetLocation(ipAddr, ref strCity, ref strState, ref strCountry);
                    bmrk.OptID = 1;  //Insert, Create
                    bmrk.FolderId = 0;  //Default Folder
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.IsFolder = false;
                    bmrk.CreatedDate = DateTime.Now.ToString();
                    bmrk.CreatedUserId = user.UserId.ToString();
                    bmrk.City = strCity;
                    bmrk.Country = strState;
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

        public ActionResult ScriptCode(string txtDomain, string ddDomains, string txtHeight, string txtWidth, string chkShowCount)
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
            ViewData["ScriptCode"] = @"<div id=""divBookmark""/><script id=""bookmark"" src=""http://booqmarqs.com/Bookmark.1.249.js?cid=2468&h=" + ViewData["txtHeight"].ToString() + "&w=" + ViewData["txtWidth"].ToString() + "&data=" + ViewData["chkShowCount"] + "\"></script>";


            if (Request.Form["btnSave"] != null)
            {
                Domain _domain = new Bookmark.Domain();
                if(string.IsNullOrEmpty(txtDomain))
                {
                    _domain.Id = double.Parse(ddDomains);
                    _domain.OptID = 3;
                    _domain.Script = ViewData["ScriptCode"].ToString();
                    _domain.ModifiedDate = DateTime.Now;
                }
                else
                {
                    _domain.Name = txtDomain;
                    _domain.OptID = 1;
                    _domain.Script = ViewData["ScriptCode"].ToString();
                    _domain.ModifiedDate = DateTime.Now;
                }
                _domain.CreateDomain();
                ViewBag.Ack = "Script saved successfully";
            }
                return View(lstDomains);
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

        public List<BookmarkCls> GetReports(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord, string txtCreatedFrom, string txtCreatedTo)
        {
            BookmarkCls bmrk = new BookmarkCls();
            Users user = (Users)Session["User"];
            List<BookmarkCls> lstBookmarks = bmrk.GetBookmarks(search, sort, sortdir, user.UserId.ToString(), txtCreatedFrom, txtCreatedTo);
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

        public ActionResult Reports(int page =1, string sort = "Name", string sortdir = "asc", string search = "", string txtCreatedFrom = "", string txtCreatedTo = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetReports(search, sort, sortdir, skip, pageSize, out totalRecord, txtCreatedFrom, txtCreatedTo);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
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
            _domain.OptID = 1; //Delete
            _domain.Name = txtDomain;
            _domain.CreatedUserId = user.UserId.ToString();
            _domain.CreateDomain();
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

        public ActionResult BarCharts()
        {
            ViewBag.MyXAxisValues = new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" };
            ViewBag.MyYAxisValues = new[] { "2", "6", "4", "5", "3" };
            return View();
        }

        public ActionResult PieCharts()
        {

            ViewBag.MyXAxisValues = new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" };
            ViewBag.MyYAxisValues = new[] { "2", "6", "4", "5", "3" };
            return View();
        }

        public ActionResult LineCharts()
        {
            ViewBag.MyXAxisValues = new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" };
            ViewBag.MyYAxisValues = new[] { "2", "6", "4", "5", "3" };
            return View();
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
