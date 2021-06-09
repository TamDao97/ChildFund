package nts.childprofile.model;

/**
 * Created by NTS-VANVV on 07/01/2019.
 */

public class ObjectInputModel {
    /// <summary>
    /// Id
    /// </summary>
    public String Id ;

    /// <summary>
    /// Trạng thái check cho trường hợp chọn
    /// </summary>
    public boolean Check ;

    /// <summary>
    /// Tên hiển thị tiếng việt
    /// </summary>
    public String Name ;

    /// <summary>
    /// Tên hiển thị tiếng anh
    /// </summary>
    public String NameEN ;

    /// <summary>
    /// Giá trị cho trường hợp là nhập text
    /// </summary>
    public String Value ;

    /// <summary>
    /// Lable giá trị khác
    /// </summary>
    public String OtherName ;

    /// <summary>
    /// Nội dung giá trị khác
    /// </summary>
    public String OtherValue ;

    /// <summary>
    /// Câu hỏi yes/no
    /// </summary>
    public String Question;
    public Boolean YesValue;
    public Boolean NoValue;

    public Boolean Enabled;
}
