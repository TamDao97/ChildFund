using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildFund.Controllers
{
    public class ModelContent
    {
        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        public string Content { get; set; }

        [AllowHtml]
        [UIHint("tinymce_full_compressed")]
        public string Content2 { get; set; }
    }
}