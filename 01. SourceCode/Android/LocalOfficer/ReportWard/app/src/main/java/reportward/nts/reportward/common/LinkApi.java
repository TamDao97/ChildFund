package reportward.nts.reportward.common;

public class LinkApi {
    /***
     * Đăng nhập
     * Type: Post
     */
    public static String login = "api/mb/tdqg/login";

    /***
     * Đăng xuất
     * Type: Get
     */
    public static String logout = "api/mb/tdqg/logout?state=";

    /***
     * Reset Pass
     * Type: Get
     */
    public static String resetpass = "api/mb/tdqg/resetpass?email=";

    /***
     * Danh sách tỉnh
     * Type: Get
     */
    public static String dstinhtp = "api/mb/tdqg/dstinhtp";

    /***
     * Dánh sách huyện theo id tỉnh
     * Type: Get
     */
    public static String DSQuanHuyen = "api/mb/tdqg/DSQuanHuyen";

    /***
     * Danh sách xã theo id huyện
     * Type: Get
     */
    public static String DSPhuongXa = "api/mb/tdqg/DSPhuongXa";

    /***
     * Mối quan hệ với nạn nhận
     * Type: Get
     */
    public static String dsquanhevoinannhan = "api/mb/tdqg/dsquanhevoinannhan";

    /***
     * Loại đối tượng xâm hại
     * Type: Get
     */
    public static String dsloaidoituongxamhai = "api/mb/tdqg/dsloaidoituongxamhai";


    /***
     * Danh sách đối tượng trẻ
     * Type: Get
     */
    public static String dsdoituongtre = "api/mb/tdqg/dsdoituongtre";

    /***
     * Danh sách trạng thái hồ sơ
     * Type: Get
     */
    public static String dstrangthaihoso = "api/mb/tdqg/dstrangthaihoso";

    /***
     * Danh sách dân tộc
     * Type: Get
     */
    public static String dsDanToc = "api/mb/tdqg/dsDanToc";

    /***
     * Tra cứu ca tư vấn
     * Type: Post
     */
    public static String dsCaTuVan = "api/mb/tdqg/catuvan/danhsach";

    /***
     * Danh sách giới tính
     * Type: Get
     */
    public static String dsGioiTinh = "api/mb/tdqg/dsgioitinh";

    /***
     * Danh sách đối tượng gọi
     * Type: Get
     */
    public static String dsDoiTuongGoi = "api/mb/tdqg/DSDoiTuongGoi";

    /***
     * Danh sách loại hồ sơ
     * Type: Get
     */
    public static String dsloaihoso = "api/mb/tdqg/dsloaihoso";

    /***
     * Danh sách nguồn thông tin
     * Type: Get
     */
    public static String dsnguonthongtin = "/api/mb/tdqg/dsnguonthongtin";

    /***
     * Danh sách phân loại hồ sơ
     * Type: Get
     */
    public static String dsphanloaihoso = "api/mb/tdqg/dsphanloaihoso";

    /***
     * Danh sách môi trường xâm hại
     * Type: Get
     */
    public static String dsmoitruongxamhai = "api/mb/tdqg/dsmoitruongxamhai";

    /***
     * Danh sách kết quả hỗ trợ can thiệp
     * Type: Get
     */
    public static String dsketquahotrocanthiep = "api/mb/tdqg/dsketquahotrocanthiep";

    /***
     * Danh sách hình thức liên hệ
     * Type: Get
     */
    public static String dshinhthuclienhe = "api/mb/tdqg/dshinhthuclienhe";

    /***
     * Danh sách các vấn đề
     * Type: Get
     */
    public static String dsVanDe = "api/mb/tdqg/dsVanDe";

    /***
     * Danh sách thông tin trẻ
     * Type: Get
     */
    public static String dsTre = "api/mb/tdqg/catuvan/dstre";

    /***
     * Danh sách đối tượng
     * Type: Get
     */
    public static String dsDoiTuong = "api/mb/tdqg/catuvan/dsdoituong";

    /***
     * tạo hồ sơ ca tư vấn
     */
    public static String hoso = "api/mb/tdqg/catuvan/hoso";

    /***
     * Thêm mới trẻ
     * Type:Post
     */
    public static String tre = "api/mb/tdqg/catuvan/tre";

    /***
     * Thêm mới đối tượng
     * Type:Post
     */
    public static String doituong = "api/mb/tdqg/catuvan/dsdoituong";

    /***
     * Cập nhật đối tượng
     * Type:Put
     */
    public static String suadoituong ="api/mb/tdqg/catuvan/dsdoituong/";

    /***
     * Kết quả hỗ trợ - can thiệp
     * Type:Post
     */
    public static String ketQuaHoTro = "/api/mb/tdqg/baocao/canthiep/ketquahotro";

    /***
     * Báo cáo địa bàn trẻ được can thiệp
     * Type:Post
     */
    public static String diabantre = "api/mb/tdqg/baocao/canthiep/diabantre";

    /***
     * Loại ca và môi trường
     * Type:Post
     */
    public static String loaiCaMoiTruong = "/api/mb/tdqg/baocao/canthiep/loaicamoitruong";

    /***
     * Báo cáo theo độ tuổi giới tính dan tộc
     * Type:Post
     */
    public static String tuoigioitinhdantoc = "api/mb/tdqg/baocao/canthiep/tuoigioitinhdantoc";

    /***
     * Báo cáo theo thủ phạm bạo lực
     * Type:Post
     */
    public static String thuphambaoluctreem = "api/mb/tdqg/baocao/thuphambaoluctreem";

    /***
     * Báo cáo theo thu phạm mua bán
     * Type:Post
     */
    public static String thuphammuabantreem = "api/mb/tdqg/baocao/thuphammuabantreem";

    /***
     * Báo cáo theo thu phạm xâm hại tình dục
     * Type:Post
     */
    public static String thuphamxamhaitinhductreem = "api/mb/tdqg/baocao/thuphamxamhaitinhductreem";

    /***
     * Báo cáo theo thủ phạm xâm hại
     * Type:Post
     */
    public static String thuphamxamhaitreem = "api/mb/tdqg/baocao/thuphamxamhaitreem";

    /***
     * Đổi mật khẩu
     * Type:Post
     */
    public static String changePass = "api/mb/tdqg/changepass";

    /***
     * Upload file
     * Type:Post
     */
    public static String upload = "/api/mb/tdqg/file/upload";

    /***
     * Cập nhật hồ sơ
     * Type:Put
     */
    public static String hosoUpdate = "/api/mb/tdqg/catuvan/hoso/";

    /***
     * Chi tết hồ sơ
     * Type:get
     */
    public static String hosochitiet = "/api/mb/tdqg/catuvan/hosochitiet";

    /***
     * Lần liên hệ
     * Type:get
     */
    public static String dslanlienhetheohoso = "/api/mb/tdqg/catuvan/dslanlienhetheohoso";

    /***
     * Lần liên hệ
     * Type:get
     */
    public static String chitietcatuvanapp = "/api/mb/tdqg/catuvan/chitietcatuvan-app";
}
