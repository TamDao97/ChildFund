package nts.childprofile.model;

/**
 * Created by NTS-VANVV on 09/01/2019.
 */

public class ChangePasswordUserModel {
    /// <summary>
    /// Id người dùng
    /// </summary>
    public String Id;

    /// <summary>
    /// Mật khẩu cũ
    /// </summary>
    public String PasswordOld;

    /// <summary>
    /// Mật khẩu mới
    /// </summary>
    public String PasswordNew;

    /// <summary>
    /// Xác nhận mật khẩu mới
    /// </summary>
    public String ConfirmPasswordNew;
}
