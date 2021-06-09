package reportward.nts.reportward.model;

import java.util.Date;
import java.util.List;

public class ProfileReportSearchResult {
    public String Id;
    public String Name;
    public String Gender;
    public Date Birthday;
    public int Age;
    public String Level;
    public String Address;
    public String FullAddress;
    public Date DateAction;
    //public IEnumerable<ProfileChildAbuse> FormAbuse;

    public String FormAbuseName;
    public String ProvinceId;
    public String DistrictId;
    public String WardId;

    public int ProcessingStatus;
    public String ProcessingName;
    public Date ReceptionDate;

    public String ProviderName;
    public String ProviderPhone;
    public String ProviderAddress;
    public String ProviderNote;
}
