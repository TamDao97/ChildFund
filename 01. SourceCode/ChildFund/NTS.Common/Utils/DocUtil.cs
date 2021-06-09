using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace NTSFramework.Common.Utils
{
    public static class DocUtil
    {
        public static WTable GetTableByFindText(this WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTable))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTable)
            {
                return entity as WTable;
            }
            else
            {
                return null;
            }
        }

        public static void NTSReplaceFirst(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = true;
            document.Replace(given, replace, false, true);
            document.ReplaceFirst = false;
        }

        public static void NTSReplaceHtml(this WordDocument document, string given, string html)
        {
            WordDocument replaceDoc = new WordDocument();
            IWSection htmlContent = replaceDoc.AddSection();
            if (htmlContent.Body.IsValidXHTML(html, XHTMLValidationType.Transitional))
            {
                htmlContent.Body.InsertXHTML(html);
            }
            document.Replace(given, replaceDoc, false, false);
        }

        public static void NTSReplaceImage(this WordDocument document, string given, string path)
        {
            WordDocument replate = new WordDocument();
            string filepath = HostingEnvironment.MapPath("~/" + path);
            if (File.Exists(filepath))
            {
                IWParagraph paragraph = replate.AddSection().AddParagraph();
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                WPicture mImage = (WPicture)paragraph.AppendPicture(Image.FromFile(filepath));
                mImage.HeightScale = 70f;
                mImage.WidthScale = 70f;
            }
            document.ReplaceFirst = true;
            document.Replace(given, replate, false, true);
            document.ReplaceFirst = false;
        }


        public static void NTSAddText(this IWSection section, string text, float fontSize, bool bold, HorizontalAlignment align)
        {
            IWParagraph mPara = section.AddParagraph();
            mPara.ParagraphFormat.HorizontalAlignment = align;
            IWTextRange mtext = mPara.AppendText(text);
            mtext.CharacterFormat.FontSize = fontSize;
            mtext.CharacterFormat.Bold = bold;
            mtext.CharacterFormat.FontName = "Times New Roman";
        }

        public static void NTSReplaceAll(this WordDocument document, string given, string replace)
        {
            document.ReplaceFirst = false;
            document.Replace(given, replace, false, true);
        }
    }
}
