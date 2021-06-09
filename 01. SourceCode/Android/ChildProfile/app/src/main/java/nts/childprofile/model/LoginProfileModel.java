package nts.childprofile.model;

import java.util.List;

/**
 * Created by NTS-VANVV on 29/12/2018.
 */

public class LoginProfileModel {
    /// <summary>
    /// Id user
    /// </summary>
    public String Id;

    /// <summary>
    /// Tên đăng nhập
    /// </summary>
    public String Name;

    /// <summary>
    /// Tài khoản đăng nhập
    /// </summary>
    public String UserName;

    /// <summary>
    /// Cấp sử dụng tài khoản
    /// </summary>
    public String UserLever;

    /// <summary>redi
    /// Id vùng hoạt động
    /// </summary>
    public String AreaUserId;

    /// <summary>
    /// Id tỉnh hoạt động
    /// </summary>
    public String ProvinceId;

    /// <summary>
    /// Id huyện hoạt động
    /// </summary>
    public String DistrictId;

    public String WardId;

    /// <summary>
    /// Trạng thái hoạt động tài khoản
    /// </summary>
    public Boolean IsDisable;

    /// <summary>
    /// Danh sách quyền
    /// </summary>
    public List<String> ListRoles;

    /// <summary>
    /// Image path
    /// </summary>
    public String ImagePath;
}
