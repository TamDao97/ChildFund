using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InformationHub.Model.Model.ReportProfile
{
  public  class ViewContentModel
    {
        public int Count { get; set; }
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        public string Note { get; set; }
    }
}
