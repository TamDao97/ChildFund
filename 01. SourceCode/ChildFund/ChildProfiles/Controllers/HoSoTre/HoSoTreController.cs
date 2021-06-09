using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.HoSoTre
{
    public class HoSoTreController : Controller
    {
    
        public ActionResult HSThongTinGiaDinh()
        { 
            return PartialView();
        }

        public ActionResult HSCapNhat()
        {
            ViewBag.Title = "Báo cáo cập nhật hồ sơ trẻ";

            return View();
        }

        public ActionResult HSThemCapNhat()
        {
            ViewBag.Title = "Cập nhật hồ sơ";

            return View();
        }
        public ActionResult HSVietnamDuyet()
        {
            ViewBag.Title = "Hồ sơ đã duyệt";

            return View();
        }

        public ActionResult HSXemThongTinTre()
        {
            ViewBag.Title = "Xem thông tin về trẻ";

            return View();
        }

        public ActionResult HSSoSanhHoSo()
        {
            ViewBag.Title = "So sánh hồ sơ trẻ";

            return View();
        }

         
        public ActionResult HSThemMauCauChuyen()
        {
            ViewBag.Title = "Tạo mẫu câu chuyện về trẻ";

            return View();
        }

    }
}