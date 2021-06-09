package nts.swipesafe.model;

import java.util.List;

import nts.swipesafe.common.Utils;

/**
 * Created by NTS-VANVV on 29/01/2019.
 */

public class ChildModel {
    public String Id;
    public String ReportId;
    public String Name;
    public String Gender;
    public Integer Age;
    public String Birthday;
    public String Level;
    public String Address;
    public String ProvinceId;
    public String DistrictId;
    public String WardId;
    public String FullAddress;
    public String DateAction;

    /// <summary>
    /// Hành vi xam hại
    /// </summary>
    public List<ChildAbuseModel> ListAbuse;

    /***
     * Các biến hiển thị
     */
    public String GenderName;
    public String LevelName;
    public String ProvinceName;
    public String DistrictName;
    public String WardName;

    public boolean IsChangeValue() {
        if (!Utils.isEmpty(Id) || !Utils.isEmpty(Name) || !Utils.isEmpty(Gender) || Age != null
                || !Utils.isEmpty(Birthday) || !Utils.isEmpty(Level) || !Utils.isEmpty(Address)
                || !Utils.isEmpty(ProvinceId) || !Utils.isEmpty(DistrictId) || !Utils.isEmpty(WardId) || !Utils.isEmpty(DateAction)) {
            return true;
        }
        return false;
    }
}
