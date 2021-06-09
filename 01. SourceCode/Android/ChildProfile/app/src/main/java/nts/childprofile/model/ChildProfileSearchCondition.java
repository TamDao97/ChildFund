package nts.childprofile.model;

import java.util.Date;

/**
 * Created by NTS-VANVV on 27/12/2018.
 */

public class ChildProfileSearchCondition extends SearchConditionBase {
    public String Name;
    public String ChildCode;
    public String ProvinceId;
    public String DistrictId;
    public String WardId;
    public Date DateFrom;
    public Date DateTo;
    public String CreateBy;
    public String UserId;
    public int Export;
    public String Address;
}
