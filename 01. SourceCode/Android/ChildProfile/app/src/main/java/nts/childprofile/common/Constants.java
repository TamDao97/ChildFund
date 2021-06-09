package nts.childprofile.common;

/**
 * Created by NTS-VANVV on 26/12/2018.
 */

public class Constants {
    public static String ApiUrl = "http://192.168.100.3:49152/";
//    public static String ApiUrl = "http://14.248.84.128:9530/";
//    public static String ApiUrl = "http://childprofile.childfund.org.vn/";

    /**
     * Mã chọn ảnh
     */
    public static final int REQUEST_CHOOSE_IMAGE = 200;

    /**
     * Yêu cầu chụp ảnh
     */
    public static final int REQUEST_IMAGE_CAPTURE = 100;

    /**
     * Yêu cầu chụp ảnh thay đổi của trẻ
     */
    public static final int REQUEST_IMAGE_CAPTURE_CHANGE = 101;

    /**
     * Mã chọn ảnh
     */
    public static final int REQUEST_CHOOSE_IMAGE_Signature = 300;

    /**
     * Yêu cầu chụp ảnh
     */
    public static final int REQUEST_IMAGE_CAPTURE_Signature = 400;

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

    public static final String Childprofile_Data_Fix = "childprofile_data_fix";

    //Tôn giáo
    public static final String Key_Data_Fix_Geligion = "Fix_Geligion";

    public static final String Key_Data_Fix_Nation = "Fix_Nation";

    public static final String Key_Data_Fix_Province_Area = "Fix_Province_Area";

    public static final String Key_Data_Fix_District_Area = "Fix_District_Area";
    public static final String Key_Data_Fix_Ward_Area = "Fix_Ward_Area";

    public static final String Key_Data_Fix_Relationship = "Fix_Relationship";
    public static final String Key_Data_Fix_Job = "Fix_Job";
    public static final String Key_Data_Fix_Repoet_Content = "Fix_Repoet_Content";
    public static final String Key_Data_Fix_School = "Fix_School";
    public static final String Key_Data_Profile_Draft = "Profile_Draft";
    public static final String Key_Data_Profile_Draft_Update = "Profile_Draft_Update";
    public static final String DraftUpdate = "DraftUpdate";
    public static final String Key_Data_Fix_Village = "Fix_Village";

    //cấu hình trạng thái hs trẻ
    public static final String ProfilesNew = "0";
    public static final String ProfilesConfimedLevel1 = "1";
    public static final String ProfilesConfimedLevel2 = "2";

    //Id fix anh chị em ruột;
    public static final String RelationshipYoungerSister = "R0006";    //Em gái
    public static final String RelationshipOlderSister = "R0008";    //Chị
    public static final String RelationshipYoungerBrother = "R0009";//Em trai
    public static final String RelationshipOlderBrother = "R0010";    //Anh

    //cấu hình trạng thái báo cáo
    public static final String ReportProfilesNew = "0";
    public static final String ReportProfilesConfimedLevel1 = "1";
    public static final String ReportProfilesConfimedLevel2 = "2";

    /// <summary>
    /// Cấp văn phòng hà nội
    /// </summary>
    public static final String LevelOffice = "1";

    /// <summary>
    /// Cấp vùng
    /// </summary>
    public static final String LevelArea = "2";

    /// <summary>
    /// Cấp giáo viên
    /// </summary>
    public static final String LevelTeacher = "3";

    public static final int IMAGE_COMPRESS_QUALITY = 85;

    public static final String DATABASE_NAME = "ChildFund.db";
    public static final String DATABASE_TABLE_Ethnic = "Ethnic";
    public static final String DATABASE_TABLE_Job = "Job";
    public static final String DATABASE_TABLE_Relationship = "Relationship";
    public static final String DATABASE_TABLE_Religion = "Religion";
    public static final String DATABASE_TABLE_School = "School";
    public static final String DATABASE_TABLE_Village = "Village";
    public static final String DATABASE_TABLE_ChildProfile = "ChildProfile";
    public static final String DATABASE_TABLE_ReportProfile = "ReportProfile";
    public static final String DATABASE_TABLE_ImageChildByYear = "ImageChildByYear";
    public static final String TYPE_CHILDPROFILE_SQLITE_DOWNLOAD = "1";
    public static final String TYPE_CHILDPROFILE_SQLITE_CREATED = "2";
    public static final String TYPE_CHILDPROFILE_SQLITE_UPDATED = "3";

    public static final int TYPE_DOWNLOAD_IMAGE_ORIGINAL = 1;// tải ảnh gốc trẻ
    public static final int TYPE_DOWNLOAD_IMAGE_THUMB = 2;// tải ảnh thumbnail
    public static final int TYPE_DOWNLOAD_IMAGE_SIGNATURE = 3;// tải ảnh Signature
    public static final int TYPE_DOWNLOAD_IMAGE_SIGNATURE_THUMB = 4;// tải ảnh Signature thumb

    public static final String FAMILY_RELATIONSHIP_FATHER = "R0001";
    public static final String FAMILY_RELATIONSHIP_STEP_FATHER = "R0015";
    public static final String FAMILY_RELATIONSHIP_MOTHER = "R0007";
    public static final String FAMILY_RELATIONSHIP_STEP_MOTHER = "R0016";
}
