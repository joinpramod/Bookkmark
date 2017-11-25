﻿using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Net;
using System.Xml;

/// <summary>
/// Summary description for Utilities
/// </summary>
namespace Bookmark
{
    public static class Utilities
{
    static string Target = "";

    public static string ExpandUrls(string inputText)
    {
        string pattern = @"[""'=]?(http://|ftp://|https://|www\.|ftp\.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
        System.Text.RegularExpressions.RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase;
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern, options);
        MatchEvaluator MatchEval = new MatchEvaluator(ExpandUrlsRegExEvaluator);
        return Regex.Replace(inputText, pattern, MatchEval);
    }

    private static string ExpandUrlsRegExEvaluator(System.Text.RegularExpressions.Match M)
    {

        string Href = M.Value;
        if (Href.StartsWith("=") || Href.StartsWith("'") || Href.StartsWith("\""))
            return Href;

        string Text = Href;
        if (Href.IndexOf("://") < 0)
        {
            if (Href.StartsWith("www."))
                Href = "http://" + Href;
            else if (Href.StartsWith("ftp"))
                Href = "ftp://" + Href;
            else if (Href.IndexOf("@") > -1)
                Href = "mailto:" + Href;
        }

        string Targ = !string.IsNullOrEmpty(Target) ? " target='" + Target + "'" : "";
        return "<a href='" + Href + "'" + Targ + ">" + Text + "</a>";

    }

    public static string GetUserIP()
    {
        string strIP = String.Empty;
        HttpRequest httpReq = HttpContext.Current.Request;

        //test for non-standard proxy server designations of client's IP
        if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
        {
            strIP = httpReq.ServerVariables["HTTP_CLIENT_IP"].ToString();
        }
        else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
        {
            strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        }
        //test for host address reported by the server
        else if
        (
            //if exists
            (httpReq.UserHostAddress.Length != 0)
            &&
            //and if not localhost IPV6 or localhost name
            ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost"))
        )
        {
            strIP = httpReq.UserHostAddress;
        }
        return strIP;
    }


        public static string GetGravatarUrlForAddress(string address)
        {
            return "http://www.gravatar.com/avatar/" + HashEmailAddress(address) + "?s=80&r=g&d=Identicon";
        }


        public static string HashEmailAddress(string address)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();

                var hasedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(address));

                var sb = new System.Text.StringBuilder();

                for (var i = 0; i < hasedBytes.Length; i++)
                {
                    sb.Append(hasedBytes[i].ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string GetIPAddress()
        {
            string varIPAddress = string.Empty;
            string varVisitorCountry = string.Empty;
            string varIpAddress = string.Empty;
            varIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(varIpAddress))
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    varIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
            }

            //varIPAddress = (System.Web.UI.Page)Request.ServerVariables["HTTP_X_FORWARDED_FOR"];    
            if (varIPAddress == "" || varIPAddress == null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    varIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            //varIPAddress = Request.ServerVariables["REMOTE_ADDR"];    
            return varIpAddress;
        }

        public static void GetLocation(string varIPAddress, ref string strCity, ref string strState, ref string strCountry)
        {
            WebRequest varWebRequest = WebRequest.Create("http://freegeoip.net/xml/" + varIPAddress);
            WebProxy px = new WebProxy("http://freegeoip.net/xml/" + varIPAddress, true);
            varWebRequest.Proxy = px;
            varWebRequest.Timeout = 2000;
            try
            {
                WebResponse rep = varWebRequest.GetResponse();
                XmlTextReader xtr = new XmlTextReader(rep.GetResponseStream());
                DataSet ds = new DataSet();
                ds.ReadXml(xtr);
                strCity = ds.Tables[0].Rows[0]["City"].ToString();
                strState = ds.Tables[0].Rows[0]["RegionName"].ToString();
                strCountry = ds.Tables[0].Rows[0]["CountryName"].ToString();
            }
            catch
            {
                //return null;
            }
        }

    }
}
