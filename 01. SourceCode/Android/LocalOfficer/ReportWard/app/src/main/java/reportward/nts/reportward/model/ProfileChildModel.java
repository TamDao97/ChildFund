package reportward.nts.reportward.model;

import java.util.List;

/**
 * Created by NTS-VANVV on 14/02/2019.
 */

public class ProfileChildModel {
    public String Id;
    public int InformationSources;
    public String ReceptionDate;
    public String ChildName;
    public String ChildBirthdate;
    public Integer Gender;
    public Integer Age;
    public String CaseLocation;
    public String WardId;
    public String DistrictId;
    public String ProvinceId;
    public String FullAddress;
    public String CurrentHealth;
    public String SequelGuess;
    public String FatherName;
    public Integer FatherAge;
    public String FatherJob;
    public String MotherName;
    public Integer MotherAge;
    public String MotherJob;
    public String FamilySituation;
    public String PeopleCare;
    public String Support;
    public String ProviderName;
    public String ProviderPhone;
    public String ProviderAddress;
    public String ProviderNote;
    public int ProcessingStatus;
    public Integer SeverityLevel;
    public String CreateBy;
    public String UpdateBy;

    /// <summary>
    /// Hành vi xam hại
    /// </summary>
    public List<ChildAbuseModel> ListAbuse;
}
