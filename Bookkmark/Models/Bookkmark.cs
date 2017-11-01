using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bookkmark.Models
{
    public class Bookkmark
    {
        public Int64 BookkmarkID { get; set; } 
        public string URL { get; set; } 
        public string Name { get; set; } 
        public string FolderId { get; set; } 
        public string CreatedUser { get; set; } 
        public string CreatedDateTime { get; set; }
   
    }

    public class Folder
    {
        public Int64 FolderID { get; set; }
        public Int64 ParentID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedDateTime { get; set; }
    }

    public class VwBookkmark
    {
        public Int64 BookkmarkID { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public string CreatedUser { get; set; }
        public string CreatedDateTime { get; set; }

    }

}