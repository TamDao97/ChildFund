package reportward.nts.reportward.common;

/**
 * Created by NTS-VANVV on 26/12/2018.
 */

public class Constants {
    //    public static String ApiUrl = "http://27.72.31.133:6868/";
    //API test
    public static String ApiUrlBac = "http://27.72.31.133:6868/";
    public static String ApiUrlTrung = "http://27.72.31.133:6868/";
    public static String ApiUrlNam = "http://27.72.31.133:6868/";

//    //API tổng đài
//    public static String ApiUrlBac = "http://hanoi.tongdai111.vn:6868/";
//    public static String ApiUrlTrung = "http://danang.tongdai111.vn:6868/";
//    public static String ApiUrlNam = "http://angiang.tongdai111.vn:6868/";

    /**
     * Mã chọn ảnh
     */
    public static final int REQUEST_CHOOSE_IMAGE = 200;

    /**
     * Yêu cầu chụp ảnh
     */
    public static final int REQUEST_IMAGE_CAPTURE = 100;

    //xóa
    public static boolean ProfilesIsDelete = true;
    public static boolean ProfilesIsUse = false;

    //gioi tinh
    public static int Male = 1;
    public static int FeMale = 0;

    /**
     * Chuỗi rỗng
     */
    public static final String STRING_EMPTY = "";

    /**
     * Giá trị NULL từ Server
     */
    public static final String RESULT_STRING_NULL = "null";

    /**
     * Tiêu đề dialog nút ACCEPT
     */
    public static final String DIALOG_CONTROL_VALUE_ACCEPT = "OK";

    /**
     * Tiêu đề dialog nút CANCEL
     */
    public static final String DIALOG_CONTROL_VALUE_CANCEL = "THOÁT";

    /**
     * Tiêu đề dialog nút DELETE
     */
    public static final String DIALOG_CONTROL_VALUE_DELETE = "XÓA";

    public static final String ReportWrap_Data_Fix = "report_wrap_data_fix";

    public static final String Key_Data_Fix_Abuse = "Fix_Abuse";
    public static final String Key_Data_Fix_Province = "Fix_Province";
    public static final String Key_Data_Fix_District = "Fix_District";
    public static final String Key_Data_Fix_Ward = "Fix_Ward";
    public static final String Key_Data_Fix_Relationship = "Fix_Relationship";
    public static final String Key_Data_Fix_Job = "Fix_Job";


    //cấu hình trạng thái hs trẻ
    public static final String ProfilesNew = "0";
    public static final String ProfilesConfimedLevel1 = "1";
    public static final String ProfilesConfimedLevel2 = "2";

    /// <summary>
    /// Cấp Xã
    /// </summary>
    public static final int LevelWard = 1;

    /// <summary>
    /// Cấp Huyện
    /// </summary>
    public static final int LevelDistrict = 2;

    /// <summary>
    /// Cấp Tỉnh
    /// </summary>
    public static final int LevelProvince = 3;

    public static final String StatusSuccess = "1";
    public static final String StatusError = "0";

    public static final String Key_Info_Login = "InfoLogin";
    public static final String LoginProfile = "InfoLogin";

    //Key list notify cộng thêm key đang nhập
    public static final String Key_Notify = "Notify_";
    public static final String ListNotify = "ListNotify";
    public static final String NumOfNotify = "NumOfNotify";
}
