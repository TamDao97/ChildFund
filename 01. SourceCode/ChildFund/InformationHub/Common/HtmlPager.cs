using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace InformationHub.Common
{
    public class HtmlPager
    {
        /// <summary>
        /// tạo lnk phân trang
        /// </summary>
        /// <param name="strPathPage"></param>
        /// <param name="intCurrentPage"></param>
        /// <param name="intRowPerPage"></param>
        /// <param name="intTotalRecord"></param>
        /// <returns></returns>
        public static string GetPage(string strPathPage, int intCurrentPage, int intRowPerPage, int intTotalRecord)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (intTotalRecord >= 0)
            {
                List<int> ltsRowPerpage = new List<int>() { 5, 10, 15, 20, 25, 30, 35, 40};
                if (!ltsRowPerpage.Contains(intRowPerPage))
                    ltsRowPerpage.Add(intRowPerPage);
                ltsRowPerpage.Sort();

                int intTotalPage = (intTotalRecord % intRowPerPage == 0) ? intTotalRecord / intRowPerPage : ((intTotalRecord - (intTotalRecord % intRowPerPage)) / intRowPerPage) + 1; ;

                strBuilder.Append("<div class=\"bottom-pager\">\r\n");

                strBuilder.Append("    <div class=\"col-lg-3 col-md-3 col-sm-3 col-xs-3 text-left\">\r\n");
                strBuilder.Append("        <span>Tổng số</span>\r\n");
                strBuilder.Append("        <select id=\"RowPerPage\" onchange=\"ChangeSize()\">\r\n");
                foreach (var item in ltsRowPerpage)
                {
                    strBuilder.AppendFormat("<option value=\"{0}\"{1}>{2}</option>\r\n", item, (item == intRowPerPage) ? " selected" : "", item);
                }
                strBuilder.Append("        </select>\r\n");
                strBuilder.AppendFormat("        <span>bản ghi/trang. Tổng số: {0}</span>\r\n", intTotalRecord);
                strBuilder.Append("    </div>\r\n");
                strBuilder.Append("    <div class=\"col-lg-6 col-md-6 col-sm-6 col-xs-6 text-center\">\r\n");
                strBuilder.Append("    <ul class=\"pagination-sm pagination ng-isolate-scope ng-valid\">\r\n");
                if (intCurrentPage > 1)
                {
                    strBuilder.AppendFormat("        <li class=\"pagination-first\" ><a href=\"javascript:;\"onclick=\"pageOnclick({0}'1')\" >Trang đầu</a></li>\r\n", strPathPage);
                    strBuilder.AppendFormat("        <li class=\"pagination-prev\" ><a href=\"javascript:;\" onclick=\"pageOnclick('{0}{1}')\" >Trang trước</a></li>\r\n", strPathPage, intCurrentPage - 1);
                }
                else
                {
                    strBuilder.Append("        <li class=\"pagination-first disable\" ><a href=\"javascript:;\">Trang đầu</a></li>\r\n");
                    strBuilder.Append("        <li class=\"pagination-prev disable\" ><a href=\"javascript:;\">Trang trước</a></li>\r\n");
                }
                if (intTotalPage <= 5)
                {
                    for (int i = 0; i < intTotalPage; i++)
                    {

                        string t = string.Empty;
                        t = CreateLinkPagePagging(strPathPage, intCurrentPage, (i + 1));
                        strBuilder.Append(t);
                    }
                }
                else
                {
                    if (intCurrentPage <= 4)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            string t = string.Empty;
                            t = CreateLinkPagePagging(strPathPage, intCurrentPage, (i + 1));
                            strBuilder.Append(t);
                        }
                    }
                    else
                    {
                        int totalPage = intCurrentPage + 3 > intTotalPage ? intTotalPage : intCurrentPage + 2;
                        for (int i = intCurrentPage - 3; i < totalPage; i++)
                        {
                            string t = string.Empty;
                            t = CreateLinkPagePagging(strPathPage, intCurrentPage, (i + 1));
                            strBuilder.Append(t);
                        }
                    }
                } 
                if (intCurrentPage < intTotalPage)
                {
                    strBuilder.AppendFormat("        <li class=\"pagination-next\" ><a href=\"javascript:;\" onclick=\"pageOnclick('{0}{1}')\" >Trang tiếp</a></li>\r\n", strPathPage, intCurrentPage + 1);
                    strBuilder.AppendFormat("        <li class=\"pagination-last\" ><a href=\"javascript:;\" onclick=\"pageOnclick('{0}{1}')\" >Trang cuối</a></li>\r\n", strPathPage, intTotalPage);
                }
                else
                {
                    strBuilder.Append("        <li class=\"pagination-next-last disable\" ><a href=\"javascript:;\">Trang tiếp</a></li>\r\n");
                    strBuilder.Append("        <li class=\"pagination-last disable\" ><a href=\"javascript:;\">Trang cuối</a></li>\r\n");
                } 
                strBuilder.Append("    </ul>\r\n");
                strBuilder.Append("    </div>\r\n");

                strBuilder.Append("</div>\r\n");
            }
            else { strBuilder.Append("<div class=\"bottom-pager\"><span>Hiện tại danh sách này chưa có dữ liệu.</span></div>\r\n"); }
            return strBuilder.ToString();
        }
       
        public static string CreateLinkPagePagging(string strPathPage, int intCurrentPage, int Number)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (intCurrentPage == Number)
            {
                strBuilder.AppendFormat("<li  class=\"pagination-page active\" title=\"{0}\"><a href=\"javascript:;\">{0}</a></li>\r\n", Number);
            }
            else
            {
                strBuilder.AppendFormat("<li class=\"pagination-page\" title=\"{1}\" ><a href=\"javascript:;\" onclick=\"pageOnclick('{0}{1}')\">{1}</a></li>\r\n",  strPathPage  , Number);
            }
            return strBuilder.ToString();
        }
    }
}