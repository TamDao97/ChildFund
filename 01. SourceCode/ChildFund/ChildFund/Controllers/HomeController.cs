using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildFund.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult ChangeLanguage(string lang)
        {
            new LanguageManagement().SetLanguage(lang);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult TrangChu()
        {
            ViewBag.Title = "Trang chủ";

            return View();
        }

        public ActionResult TiepNhan()
        {
            ViewBag.Title = "Tiếp nhận báo cáo trường hợp từ cộng đồng";

            return View();
        }

        public ActionResult TiepNhanXacNhan()
        {
            ViewBag.Title = "Trường hợp đã xác nhận";

            return View();
        }

        public ActionResult TiepNhanAo()
        {
            ViewBag.Title = "Trường hợp báo cáo ảo";

            return View();
        }

        public ActionResult TimKiemTruongHop()
        {
            ViewBag.Title = "Tra cứu báo cáo trường hợp";
            return View();
        }

        public ActionResult DanhSachCa()
        {
            ViewBag.Title = "Danh sách trường hợp";

            return View();
        }

        public ActionResult ThemMoiCa()
        {
            ViewBag.Title = "Thêm mới trường hợp";

            return View();
        }

        public ActionResult XacMinhDanhGia()
        {
            ViewBag.Title = "Cập nhật tiến độ xử lý trường hợp";

            return View();
        }

        public ActionResult ThuVien()
        {
            ViewBag.Title = "Thư viện tài liệu";

            return View();
        }

        public ActionResult CauHinh()
        {
            ViewBag.Title = "Cấu hình hệ thống";

            return View();
        }

        public ActionResult DangNhap()
        {
            ViewBag.Title = "Đăng nhập hệ thống";

            return View();
        }

        public ActionResult ThongKeTruongHop()
        {
            ViewBag.Title = "Thống kê trường hợp trên địa bàn";

            return View();
        }

        #region Cấp trên
        public ActionResult TrangChuTren()
        {
            ViewBag.Title = "Trang chủ";

            return View();
        }

        public ActionResult TiepNhanCapDuoi()
        {
            ViewBag.Title = "Tiếp nhận trường hợp từ cấp dưới";

            return View();
        }

        public ActionResult BaoCaoSuVuCapTren()
        {
            ViewBag.Title = "Báo cáo trường hợp trên địa bàn";

            return View();
        }

        public ActionResult ThongKeSuVu()
        {
            ViewBag.Title = "Thống kê trường hợp trên địa bàn";

            return View();
        }
        #endregion

        #region Quản trị hệ thống
        public ActionResult QLTrangChu()
        {
            ViewBag.Title = "Trang chủ";

            return View();
        }

        public ActionResult QLDanhSachSuVu()
        {
            ViewBag.Title = "Danh sách trường hợp";

            return View();
        }

        public ActionResult QLBaoCaoSuVu()
        {
            ViewBag.Title = "Báo cáo trường hợp";

            return View();
        }

        public ActionResult QLThongKeSuVuViTri()
        {
            ViewBag.Title = "Thống kê trường hợp theo vị trí địa lý";

            return View();
        }

        public ActionResult QLThongKeSuVuTop()
        {
            ViewBag.Title = "Thống kê vị trí địa lý nhiều trường hợp";

            return View();
        }

        public ActionResult QLNguoiDung()
        {
            ViewBag.Title = "Quản lý người dùng";

            return View();
        }

        public ActionResult QLThemNguoiDung()
        {
            ViewBag.Title = "Thêm mới người dùng";

            return View();
        }

        public ActionResult QLNhomQuyen()
        {
            ViewBag.Title = "Quản lý nhóm quyền";

            return View();
        }

        public ActionResult QLThemNhomQuyen()
        {
            ViewBag.Title = "Thêm mới nhóm quyền";

            return View();
        }
        public ActionResult QLThanhVien()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLLienHe()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLMenu()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLThemMenu()
        {
            ViewBag.Title = "Thêm mới bài viết";

            return View();
        }
        public ActionResult QLNhomTin()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }

        public ActionResult QLThemNhomTin()
        {
            ViewBag.Title = "Thêm mới bài viết";

            return View();
        }
        public ActionResult QLSlide()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLThemSlide()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLSay()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLThemSay()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLTinTuc()
        {
            ViewBag.Title = "Quản lý bài viết";

            return View();
        }
        public ActionResult QLCauHinh()
        {
            ViewBag.Title = "Thêm mới bài viết";

            return View();
        }
        public ActionResult QLThemMoiBaiViet()
        {
            ViewBag.Title = "Thêm mới bài viết";

            return View();
        }

        public ActionResult QLThuVien()
        {
            ViewBag.Title = "Quản lý thư viện";

            return View();
        }

        public ActionResult QLThemMoiThuVien()
        {
            ViewBag.Title = "Thêm mới tài liệu";

            return View();
        }
        public ActionResult QLCauchuyen()
        {
            ViewBag.Title = "Thêm mới tài liệu";

            return View();
        }
        public ActionResult QLBinhluan()
        {
            ViewBag.Title = "Thêm mới tài liệu";

            return View();
        }
        public ActionResult QLThuVienAnh()
        {
            ViewBag.Title = "Quản lý thư viện ảnh";

            return View();
        }
        public ActionResult QLThemThuVienAnh()
        {
            ViewBag.Title = "Xem danh sách ảnh";

            return View();
        }

        public ActionResult QLThuVienVideo()
        {
            ViewBag.Title = "Quản lý thư viện ảnh";

            return View();
        }
        public ActionResult QLThemThuVienVideo()
        {
            ViewBag.Title = "Xem danh sách ảnh";

            return View();
        }
        #endregion

        #region Hồ sơ trẻ
        public ActionResult HSTrangChu()
        {
            ViewBag.Title = "Trang chủ";

            return View();
        }

        public ActionResult HSChoDuyet()
        {
            ViewBag.Title = "Hồ sơ mới";

            return View();
        }

        public ActionResult HSThemChoDuyet()
        {
            ViewBag.Title = "Thêm hồ sơ mới";

            return View();
        }

        public ActionResult HSCapNhat()
        {
            ViewBag.Title = "Hồ sơ cập nhật";

            return View();
        }

        public ActionResult HSThemCapNhat()
        {
            ViewBag.Title = "Thêm cập nhật hồ sơ";

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
        public ActionResult HSNhomQuyen()
        {
            ViewBag.Title = "Quản lý nhóm quyền";

            return View();
        }

        public ActionResult HSThemNhomQuyen()
        {
            ViewBag.Title = "Thêm mới nhóm quyền";

            return View();
        }
        public ActionResult HSNguoiDung()
        {
            ViewBag.Title = "Quản lý người dùng";

            return View();
        }

        public ActionResult HSThemNguoiDung()
        {
            ViewBag.Title = "Thêm mới người dùng";

            return View();
        }

        public ActionResult HSMauCauChuyen()
        {
            ViewBag.Title = "Quản lý mẫu câu chuyện về trẻ";

            return View();
        }
        public ActionResult HSThemMauCauChuyen()
        {
            ViewBag.Title = "Tạo mẫu câu chuyện về trẻ";

            return View();
        }
        public ActionResult HSThuVienAnh()
        {
            ViewBag.Title = "Quản lý thư viện ảnh";

            return View();
        }
        public ActionResult HSXemChiTietAnh()
        {
            ViewBag.Title = "Xem danh sách ảnh";

            return View();
        }
        #endregion
    }
}
