using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NTS.Common.Utils
{
    public static class Common
    {
        public static string GenStatus(int stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {

                    case 1:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status1; break;
                    case 2:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status2; break;
                    case 3:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status3; break;
                    case 4:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status4; break;
                    case 5:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status5; break;
                    case 6:
                        rs = InformationHub.Resource.Resource.ReportProfile_Status6; break;
                    default:
                        break;
                }
            }
            catch (Exception)
            { }
            return rs;
        }
        public static string GenLevel(int? stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {
                    case 0:
                        rs = InformationHub.Resource.Resource.ReportProfile_Low; break;
                    case 1:
                        rs = InformationHub.Resource.Resource.ReportProfile_Nomal; break;
                    case 2:
                        rs = InformationHub.Resource.Resource.ReportProfile_Hight; break;
                    default:
                        break;
                }
            }
            catch (Exception)
            { }
            return rs;
        }
        public static string GenSource(int stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {
                    case 0:
                        rs = InformationHub.Resource.Resource.Phone_Title; break;
                    case 1:
                        rs = InformationHub.Resource.Resource.Live_Title; break;
                    case 2:
                        rs = InformationHub.Resource.Resource.Other_Title; break;
                    default:
                        break;
                }
            }
            catch (Exception)
            { }
            return rs;
        }
        public static string GenGender(int? stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {
                    case 0:
                        rs = InformationHub.Resource.Resource.ReportProfile_Unknow; break;
                    case 1:
                        rs = InformationHub.Resource.Resource.ReportProfile_Male; break;
                    case 2:
                        rs = InformationHub.Resource.Resource.ReportProfile_FeMale; break;
                    default:
                        break;
                }
            }
            catch (Exception)
            { }
            return rs;
        }
        public static string GenUserType(string stt)
        {
            string rs = "";
            try
            {
                switch (stt)
                {
                    case "0":
                        rs = InformationHub.Resource.Resource.GroupUser_Type_Admin; break;
                    case "1":
                        rs = InformationHub.Resource.Resource.GroupUser_Type_Province; break;
                    case "2":
                        rs = InformationHub.Resource.Resource.GroupUser_Type_District; break;
                    case "3":
                        rs = InformationHub.Resource.Resource.GroupUser_Type_Ward; break;
                    default:
                        break;
                }
            }
            catch (Exception)
            { }
            return rs;
        }

        #region Name To Tag
        public static string StripHTML(string HTMLCode, int count)
        {
            try
            {
                // Remove new lines since they are not visible in HTML
                HTMLCode = HTMLCode.Replace("\n", " ");

                // Remove tab spaces
                HTMLCode = HTMLCode.Replace("\t", " ");

                // Remove multiple white spaces from HTML
                HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

                // Remove HEAD tag
                HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                    , RegexOptions.IgnoreCase | RegexOptions.Singleline);

                // Remove any JavaScript
                HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
                  , RegexOptions.IgnoreCase | RegexOptions.Singleline);

                // Replace special characters like &, <, >, " etc.
                StringBuilder sbHTML = new StringBuilder(HTMLCode);
                // Note: There are many more special characters, these are just
                // most common. You can add new characters in this arrays if needed
                string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"};
                string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
                for (int i = 0; i < OldWords.Length; i++)
                {
                    sbHTML.Replace(OldWords[i], NewWords[i]);
                }

                // Check if there are line breaks (<br>) or paragraph (<p>)
                sbHTML.Replace("<br>", "\n<br>");
                sbHTML.Replace("<br ", "\n<br ");
                sbHTML.Replace("<p ", "\n<p ");

                // Finally, remove all HTML tags and return plain text
                var rs = System.Text.RegularExpressions.Regex.Replace(
                        sbHTML.ToString(), "<[^>]*>", "");
                return FormatContentNews(rs, count);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public static string ConvertNameToTag(string strName)
        {
            string strReturn = "";
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            strReturn = Regex.Replace(strName, "[^\\w\\s]", string.Empty).Replace(" ", "-").ToLower();
            string strFormD = strReturn.Normalize(System.Text.NormalizationForm.FormD);
            return regex.Replace(strFormD, string.Empty).Replace("đ", "d");
        }
        #endregion
        public static string FormatContentNews(string value, int count)
        {
            try
            {
                string _value = value;
                if (_value.Length >= count)
                {
                    string ValueCut = _value.Substring(0, count - 3);
                    string[] valuearray = ValueCut.Split(' ');
                    string valuereturn = "";
                    if (valuearray.Length > 1)
                    {
                        for (int i = 0; i < valuearray.Length - 1; i++)
                        {
                            valuereturn = valuereturn + " " + valuearray[i];
                        }
                    }
                    else
                    {
                        valuereturn = valuearray[0];
                    }

                    return valuereturn + "..";
                }
                else
                {
                    return _value;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string PhanTrang(int numItems, int curpage, int numOfNews, string url)
        {
            //int numItems = 10; // so san pham tren 1 trang
            //int numOfNews = 0; // tong so san pham da goi len duoc tu db
            int numpages = 0;   //tong so trang co duoc khi tien hanh phan trang
            string showpage = "";
            numpages = numOfNews / numItems;
            if (numOfNews % numItems > 0)
            {
                numpages += 1;
            }
            if (curpage < 0)
            {
                curpage = 0;
            }
            if (numOfNews > 0)
            {
                if (curpage == 0)
                {
                    showpage = "<ul  class=\"pagination\">";
                }
                else
                {
                    showpage = "<ul  class=\"pagination\">";
                }
                if (numpages == 1)
                {
                    showpage += "<li><span>1</span></li>";
                }
                else if (numpages < 10)
                {
                    if (curpage == 0)
                    {
                        for (int i = 0; i < numpages; i++)
                        {

                            if (i == curpage)
                            {
                                showpage += "<li><span>" + (i + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\">" + (i + 1) + "</a></li>";
                            }
                        }
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\">></a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\" >>></a></li>";
                    }
                    else if (curpage == numpages - 1)
                    {
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == numpages - 1)
                            {
                                showpage += "<li><span>" + (curpage + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\" >" + (i + 1) + "</a></li>";
                            }
                        }
                    }
                    else if (numpages < curpage + 2)
                    {
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\"><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<li><span>" + (i + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\" >" + (i + 1) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\" >>></a></li>";
                    }
                    else
                    {
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "1')\"><<</a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (curpage) + "')\"  ><</a></li>";
                        for (int i = 0; i < numpages; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<li><span>" + (i + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\" >" + (i + 1) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\">></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\" >>></a></li>";
                    }
                }
                else if (numpages >= 10)
                {
                    if (curpage == 0)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == curpage)
                            {
                                showpage += "<li><span>" + (i + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\" >" + (i + 1) + "</a></li>";
                            }
                        }
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\" >>></a></li>";
                    }
                    else if (curpage == numpages - 1)
                    {
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\"><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 8)
                            {
                                showpage += "<li><span>" + (curpage + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\" >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                    }
                    else if (curpage == 1)
                    {
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\"><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 1)
                            {
                                showpage += "<li><span>" + (i + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (i + 1) + "')\"  >" + (i + 1) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + numpages + "')\"  >>></a></li>";
                    }
                    else if (numpages == curpage + 2)
                    {
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 7)
                            {
                                showpage += "<li><span>" + (numpages - 8 + i) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\" >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\">></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\">>></a></li>";
                    }
                    else if (numpages == curpage + 3)
                    {
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 6)
                            {
                                showpage += "<li><span>" + (numpages - 8 + i) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\"  >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\"   >></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\"  >>></a></li>";
                    }
                    else if (numpages == curpage + 4)
                    {
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\"  ><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 5)
                            {
                                showpage += "<li><span>" + (numpages - 8 + i) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\" >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\">>></a></li>";
                    }
                    else if (numpages == curpage + 5)
                    {
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 4)
                            {
                                showpage += "<li><span>" + (numpages - 8 + i) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\" >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + numpages + "')\" >>></a></li>";
                    }
                    else if (numpages == curpage + 6)
                    {
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (curpage) + "')\" ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 3)
                            {
                                showpage += "<li><span>" + (numpages - 8 + i) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a  href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (numpages - 8 + i) + "')\" >" + (numpages - 8 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a href=\"" + url + "" + (curpage + 2) + "\">></a></li>";
                        showpage += "<li><a href=\"" + url + "" + numpages + "\">>></a></li>";
                    }
                    else
                    {
                        showpage += "<li><a href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "1')\" ><<</a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + curpage + "')\"  ><</a></li>";
                        for (int i = 0; i < 9; i++)
                        {
                            if (i == 2)
                            {
                                showpage += "<li><span>" + (curpage + 1) + "</span></li>";
                            }
                            else
                            {
                                showpage += "<li><a href=\"javascript:void(0)\"  onclick=\"phantrang('" + url + "" + (curpage - 1 + i) + "')\"  >" + (curpage - 1 + i) + "</a></li>";
                            }
                        }
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + (curpage + 2) + "')\" >></a></li>";
                        showpage += "<li><a href=\"javascript:void(0)\" onclick=\"phantrang('" + url + "" + numpages + "')\"  >>></a></li>";
                    }
                }
            }
            else
            {
                if (curpage == 0)
                {
                    showpage = "<ul  class=\"pagination\"><li>";
                }
            }
            showpage += "</ul>";
            return showpage;
        }


        static readonly string _stripHTMLRegex = "<.+?>";
        static readonly string _stripHTMLRegexConditionalFormat = "<(?!({0})\\b)[^>]*>";

        /// <summary>
        /// Counts number of words in a string
        /// </summary>
        /// <param name="str">The string to parse</param>
        /// <returns>An integer of the number of words found</returns>
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Uppercases the first character of a string
        /// </summary>
        /// <param name="input">The string which first character should be uppercased</param>
        /// <returns>The input string with it'input first character uppercased</returns>
        public static string FirstCharToUpper(this string input)
        {
            return String.IsNullOrEmpty(input) ? "" : String.Concat(input.Substring(0, 1).ToUpper(), input.Substring(1));
        }

        /// <summary>
        /// Highlights specified keywords in the input string with the specified class name by using a &lt;span /&gt;
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="keywords">The keywords to highlight</param>
        /// <param name="className">The class name</param>
        /// <returns>The input string with highlighted keywords</returns>
        public static string HighlightKeywords(this string input, IEnumerable<string> keywords, string className)
        {
            if (string.IsNullOrEmpty(input) || keywords == null || !keywords.Any())
                return input;

            foreach (string keyword in keywords)
            {
                input = Regex.Replace(input, keyword, string.Format("<span class=\"{1}\">{0}</span>", "$0", className), RegexOptions.IgnoreCase);
            }
            return input;
        }

        /// <summary>
        /// Strips HTML from a string with the specified options
        /// </summary>
        /// <param name="input">The HTML formatted input string</param>
        /// <param name="ignoreParagraphs">Indicates if paragraphs should be remained</param>
        /// <param name="ignoreItalic">Indicates if italic tags should be remained</param>
        /// <param name="ignoreUnderline">Indicates if underline tags should be remained</param>
        /// <param name="ignoreBold">Indicates if bold tags should be remained</param>
        /// <param name="ignoreLinebreak">Indicates if linebreaks should be remained</param>
        /// <param name="otherTagsToIgnore">A list of other tag names (without the brackets, like 'div') to ignore</param>
        /// <returns>The HTML stripped result</returns>
        public static string StripHtml(this string input, bool ignoreParagraphs = true, bool ignoreItalic = true, bool ignoreUnderline = true, bool ignoreBold = true, bool ignoreLinebreak = true, List<string> otherTagsToIgnore = null)
        {
            if (ignoreParagraphs || ignoreItalic || ignoreUnderline || ignoreBold || ignoreLinebreak || (otherTagsToIgnore != null && otherTagsToIgnore.Any()))
            {
                string conditions = string.Empty;

                if (ignoreParagraphs)
                    conditions += "/?p|";
                if (ignoreItalic)
                    conditions += "/?i|/?em|";
                if (ignoreUnderline)
                    conditions += "/?u|";
                if (ignoreBold)
                    conditions += "/?b|/?strong|";
                if (ignoreLinebreak)
                    conditions += "br|";
                if (otherTagsToIgnore != null && otherTagsToIgnore.Any())
                {
                    otherTagsToIgnore.ForEach((x) =>
                    {
                        conditions += string.Concat("/?", x, "|");
                    });
                }

                conditions = conditions.Substring(0, conditions.Length - 1); // Remove last '|'

                string regex = string.Format(_stripHTMLRegexConditionalFormat, conditions);
                Regex rgx = new Regex(regex, RegexOptions.Singleline);

                return rgx.Replace(input, string.Empty);
            }
            else
                if (input != null)
            {
                return new Regex(_stripHTMLRegex, RegexOptions.Singleline).Replace(input, string.Empty);
            }
            else
            {
                return String.Empty;
            }
        }
        

        /// Returns a particular sentence by splitting by fullstop, question mark and exclamation mark
        /// </summary>
        /// <param name="textToSplit">The string</param>
        /// <param name="sentenceIndex">The index of the sentence to return</param>
        /// <returns>The single sentence requested</returns>
        public static string GetSentence(this string textToSplit, int sentenceIndex)
        {
            string[] delim = { ".", "!", "?" };
            string[] splitText = textToSplit.StripHtml(false, false, false, false, false).Split(delim, StringSplitOptions.None);
            var sentanceCount = splitText.Length;
            if (sentanceCount >= sentenceIndex)
            {
                return splitText[sentenceIndex] + ".";
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns a particular paragraph by splitting by p tag
        /// </summary>
        /// <param name="textToSplit">The HTML formatted string</param>
        /// <param name="paragraphIndex">The index of the paragraph to return</param>
        /// <returns>The single paragraph requested</returns>
        public static string GetParagraph(this string textToSplit, int paragraphIndex)
        {
            string[] delim = { "<p>" };
            string[] splitText = textToSplit.RemoveHtmlComments().StripHtml(true, true, true, true, true, new List<string> { "a" }).Split(delim, StringSplitOptions.RemoveEmptyEntries);
            int paragraphCount = splitText.Length;

            if (paragraphCount - 1 >= paragraphIndex)
            {
                var endPos = splitText[paragraphIndex].IndexOf("</p>");
                return "<p>" + splitText[paragraphIndex].Substring(0, endPos + 4);
            }
            return "<p></p>";
        }

        /// <summary>
        /// Strips out html comment tags
        /// </summary>
        /// <param name="input">The HTML formatted string</param>
        /// <returns>The html without any comments tags</returns>
        private static string RemoveHtmlComments(this string input)
        {
            if (input != null)
            {
                string output = string.Empty;
                string[] temp = Regex.Split(input, "<!--");
                foreach (string s in temp)
                {
                    var str = !s.Contains("-->") ? s : s.Substring(s.IndexOf("-->") + 3);
                    if (str.Trim() != string.Empty)
                    {
                        output = output + str.Trim();
                    }
                }
                return output;
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Safely truncate any string to a predetermined length and preserve whole words when truncating
        /// </summary>
        /// <param name="textToTruncate">Text to truncate</param>
        /// <param name="length">Length</param>
        /// <returns></returns>
        public static string TruncateAtWord(this string textToTruncate, int length)
        {
            if (textToTruncate == null || textToTruncate.Length < length)
                return textToTruncate;
            int iNextSpace = textToTruncate.LastIndexOf(" ", length);
            return string.Format("{0}...", textToTruncate.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim());
        }

        /// <summary>
        /// Inverts the case of each character in the given string and returns the new string
        /// </summary>
        /// <param name="input">The given string</param>
        /// <returns>The converted string</returns>
        public static string InvertCase(this string input)
        {
            return new string(
            input.Select(c => char.IsLetter(c) ? (char.IsUpper(c) ?
                      char.ToLower(c) : char.ToUpper(c)) : c).ToArray());
        }

        /// <summary>
        /// Returns the substring of string1 before the occurrence of string2.
        /// </summary>
        /// <param name="string1">
        /// The string 1.
        /// </param>
        /// <param name="string2">
        /// The string 2.
        /// </param>
        /// <returns>
        /// The substring
        /// </returns>
        public static string SubstringBefore(this string string1, string string2)
        {
            var posA = string1.IndexOf(string2);
            if (posA != -1)
            {
                return string1.Substring(0, posA);
            }
            return string.Empty;
        }

        /// <summary>
        /// Returns the substring of string1 after the occurrence of string2.
        /// </summary>
        /// <param name="string1">
        /// The string 1.
        /// </param>
        /// <param name="string2">
        /// The string 2.
        /// </param>
        /// <returns>
        /// The substring
        /// </returns>
        public static string SubstringAfter(this string string1, string string2)
        {
            var posA = string1.IndexOf(string2);
            if (posA != -1)
            {
                return string1.Substring(posA + 1);
            }
            return string.Empty;
        }

    }
}
