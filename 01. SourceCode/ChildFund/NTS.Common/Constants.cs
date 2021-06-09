using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Common
{
    public static class Constants
    {
        //bảng danh mục SwipeSafe
        //loai menu
        public const string MenuContent = "3";
        public const string MenuReference = "2";
        public const string MenuLink = "1";

        //vi tri menu
        public const string MenuBottom = "2";
        public const string MenuTop = "1";
        //loai menu lien ket
        public const string MenuTypeHome = "0";
        public const string MenuTypeNew = "1";
        public const string MenuTypeLibrary = "2";
        public const string MenuTypeImg = "3";
        public const string MenuTypeVideo = "4";
        public const string MenuTypeStory = "5";
        public const string MenuTypeContact = "6";
        public const string MenuTypeReport = "7";
        // laoi cate
        public const string CateNews = "1";
        public const string ApiUrl = "http://localhost:18123/";

        /// <summary>
        /// Đang hoạt động
        /// </summary>
        public const bool IsActive = true;

        /// <summary>
        /// Ngừng hoạt động
        /// </summary>
        public const bool NotActive = false;

        public const string FolderFileUser = "FileUser";
        public const string FolderReportProfile = "ReportProfile";
        public const string FolderImageChildProfile = "ImageChild";

        public const string TypeOther = "AT05";

        /// <summary>
        /// Giới tính Nam
        /// </summary>
        public const int Male = 1;

        /// <summary>
        ///  Giới tính Nữ
        /// </summary>
        public const int FeMale = 2;
        public const int UnMale = 0;

        /// <summary>
        /// Hồ sơ mới
        /// </summary>
        public const string CreateNew = "0";
        /// <summary>
        /// Cấp vùng duyệt
        /// </summary>
        public const string ApproverArea = "1";
        /// <summary>
        /// Cấp văn phòng HN duyệt
        /// </summary>
        public const string ApproveOffice = "2";

        /// <summary>
        /// Đã xóa
        /// </summary>
        public const bool IsDelete = true;

        /// <summary>
        /// Đang sử dụng
        /// </summary>
        public const bool IsUse = false;

        /// <summary>
        /// Cấp admin
        /// </summary>
        public const string LevelAdmin ="0";

        /// <summary>
        /// Cấp văn phòng hà nội
        /// </summary>
        public const string LevelOffice = "1";

        /// <summary>
        /// Cấp vùng
        /// </summary>
        public const string LevelArea = "2";

        /// <summary>
        /// Cấp giáo viên
        /// </summary>
        public const string LevelTeacher = "3";

        /// <summary>
        /// Còn nhỏ
        /// </summary>
        public const string LeaningChildhood = "11";

        /// <summary>
        /// Bỏ học
        /// </summary>
        public const string LeaningDropout = "12";

        /// <summary>
        /// Trẻ tàn tật
        /// </summary>
        public const string LeaningHandicapped = "13";

        /// <summary>
        /// Học mẫu giáo
        /// </summary>
        public const string LeaningKindergarten = "14";

        /// <summary>
        /// Học tiểu học
        /// </summary>
        public const string LeaningPrimarySchool = "15";

        /// <summary>
        /// Học trung học
        /// </summary>
        public const string LeaningHighSchool = "16";

        // File type
        public const int ImageType = 1;
        public const int DocumentType = 2;
        public const int OthorFileType = 3;

        /// <summary>
        /// Đã xem Notification
        /// </summary>
        public const string ViewNotification = "1";
        /// <summary>
        /// Chưa xem Notification
        /// </summary>
        public const string NotViewNotification = "0";

        //mã trẻ
        public const string ChildCode199 = "199";
        public const string ChildCode213 = "213";
        // trang thái xử lý báo cáo
        public const int ProcessStatus0 = 0;
        public const int ProcessStatus1 = 1;
        public const int ProcessStatus2 = 2;
        public const int ProcessStatus3 = 3;
        public const int ProcessStatus4 = 4;

        // trang thái xử lý báo cáo
        /// <summary>
        /// Loại hình xâm hại thể chất
        /// </summary>
        public const string Abuse1 = "AT01";
        /// <summary>
        /// Loại hình xâm hại tình dục
        /// </summary>
        public const string Abuse2 = "AT02";
        /// <summary>
        /// Loại hình xâm hại tinh thần
        /// </summary>
        public const string Abuse3 = "AT03";
        /// <summary>
        /// Loại hình xâm hại xao nhãng
        /// </summary>
        public const string Abuse4 = "AT04";
        public const string Abuse5 = "AT05";

        // mức độ báo cáo
        public const int LevelLow = 0;
        public const int LevelNomal = 1;
        public const int LevelHight = 2;
        // mức độ báo cáo
        public const int SourcePhone = 0;
        public const int SourceLive = 1;
        public const int SourceOther = 2;

        public const string Scholl_Connho = "con nho";
        public const string Scholl_Bohoc = "bo hoc";
        public const string Scholl_Khuyettat = "khuyet tat";
        public const string Scholl_Maugiao = "mau giao";
        public const string Ethnic_Kinh = "E0001";
        public const string Religion_None = "R0001";
        
    }
}
