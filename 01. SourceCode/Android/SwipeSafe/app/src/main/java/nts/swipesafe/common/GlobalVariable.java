package nts.swipesafe.common;

import android.app.Application;
//import android.support.v7.app.AppCompatActivity;

import java.util.ArrayList;
import java.util.List;

import nts.swipesafe.model.ChildAbuseModel;
import nts.swipesafe.model.ChildInfoModel;
import nts.swipesafe.model.ChildModel;
import nts.swipesafe.model.ComboboxItem;
import nts.swipesafe.model.GalleryModel;
import nts.swipesafe.model.InternetModel;
import nts.swipesafe.model.PrisonerInfoModel;
import nts.swipesafe.model.PrisonerModel;
import nts.swipesafe.model.ReportInfoModel;
import nts.swipesafe.model.ReportModel;
import nts.swipesafe.model.ReporterModel;
import nts.swipesafe.model.VideoModel;

/**
 * Created by NTS-VANVV on 29/12/2018.
 */

public class GlobalVariable extends Application {
    public ReportInfoModel reportInfoModel = new ReportInfoModel();
    public List<ChildInfoModel> listChild = new ArrayList<>();
    public List<PrisonerInfoModel> listPrisoner = new ArrayList<>();

    public ReportModel reportModel = new ReportModel();
    public ArrayList<GalleryModel> fileReport = new ArrayList<>();

    public ReportModel getReport() {
        return reportModel;
    }

    public void updateReport(ReportModel report) {
        reportModel = report;
    }

    public void clearReport() {
        reportModel = new ReportModel();
        fileReport = new ArrayList<>();
    }

    /***
     * Set mô tả hành vi xâm hại
     * @param description
     */
    public void setDescription(String description) {
        reportModel.Description = description;
    }

    /***
     * Get mô tả hành vi xâm hại
     */
    public String getDescription() {
        return reportModel.Description;
    }

    /***
     * Set thông tin người báo cáo
     * @param reporterModel
     */
    public void setReporter(ReporterModel reporterModel) {
        reportModel.Id = reporterModel.Id;
        reportModel.Name = reporterModel.Name;
        reportModel.ProvinceId = reporterModel.ProvinceId;
        reportModel.ProvinceName = reporterModel.ProvinceName;
        reportModel.DistrictId = reporterModel.DistrictId;
        reportModel.DistrictName = reporterModel.DistrictName;
        reportModel.WardId = reporterModel.WardId;
        reportModel.WardName = reporterModel.WardName;
        reportModel.Address = reporterModel.Address;
        reportModel.FullAddress = reporterModel.FullAddress;
        reportModel.Phone = reporterModel.Phone;
        reportModel.Email = reporterModel.Email;
        reportModel.Relationship = reporterModel.Relationship;
        reportModel.RelationshipName = reporterModel.RelationshipName;
        reportModel.Birthday = reporterModel.Birthday;
        reportModel.Gender = reporterModel.Gender;
        reportModel.GenderName = reporterModel.GenderName;
        reportModel.Type = reporterModel.Type;
        reportModel.Status = "0";
    }

    /***
     * Get thông tin người báo cáo
     */
    public ReporterModel getReporter() {
        ReporterModel reporterModel = new ReportModel();
        reporterModel.Id = reportModel.Id;
        reporterModel.Name = reportModel.Name;
        reporterModel.ProvinceId = reportModel.ProvinceId;
        reporterModel.ProvinceName = reportModel.ProvinceName;
        reporterModel.DistrictId = reportModel.DistrictId;
        reporterModel.DistrictName = reportModel.DistrictName;
        reporterModel.WardId = reportModel.WardId;
        reporterModel.WardName = reportModel.WardName;
        reporterModel.Address = reportModel.Address;
        reporterModel.FullAddress = reportModel.FullAddress;
        reporterModel.Phone = reportModel.Phone;
        reporterModel.Email = reportModel.Email;
        reporterModel.Relationship = reportModel.Relationship;
        reporterModel.RelationshipName = reportModel.RelationshipName;
        reporterModel.Birthday = reportModel.Birthday;
        reporterModel.Gender = reportModel.Gender;
        reporterModel.GenderName = reportModel.GenderName;
        reporterModel.Type = reportModel.Type;
        reporterModel.Status = "0";
        return reporterModel;
    }

    public List<ChildModel> getListChild() {
        if (reportModel.ListChild == null) {
            return reportModel.ListChild = new ArrayList<>();
        }
        return reportModel.ListChild;
    }

    public int addChild(ChildModel childModel, int index) {
        if (reportModel.ListChild == null) {
            reportModel.ListChild = new ArrayList<>();
        }
        if (index == -1) {
            reportModel.ListChild.add(childModel);
            index = getChildSize() - 1;
        } else {
            reportModel.ListChild.get(index).Id = childModel.Id;
            reportModel.ListChild.get(index).ReportId = childModel.ReportId;
            reportModel.ListChild.get(index).Name = childModel.Name;
            reportModel.ListChild.get(index).Gender = childModel.Gender;
            reportModel.ListChild.get(index).GenderName = childModel.GenderName;
            reportModel.ListChild.get(index).Age = childModel.Age;
            reportModel.ListChild.get(index).Birthday = childModel.Birthday;
            reportModel.ListChild.get(index).Level = childModel.Level;
            reportModel.ListChild.get(index).LevelName = childModel.LevelName;
            reportModel.ListChild.get(index).Address = childModel.Address;
            reportModel.ListChild.get(index).ProvinceId = childModel.ProvinceId;
            reportModel.ListChild.get(index).ProvinceName = childModel.ProvinceName;
            reportModel.ListChild.get(index).DistrictId = childModel.DistrictId;
            reportModel.ListChild.get(index).DistrictName = childModel.DistrictName;
            reportModel.ListChild.get(index).WardId = childModel.WardId;
            reportModel.ListChild.get(index).WardName = childModel.WardName;
            reportModel.ListChild.get(index).FullAddress = childModel.FullAddress;
            reportModel.ListChild.get(index).DateAction = childModel.DateAction;
            reportModel.ListChild.get(index).ListAbuse = childModel.ListAbuse;
        }
        return index;
    }

    public ChildModel getChild(int index) {
        if (reportModel.ListChild != null && reportModel.ListChild.size() > 0) {
            return reportModel.ListChild.get(index);
        }
        return null;
    }

    public void removeChild(int index) {
        if (reportModel.ListChild != null && reportModel.ListChild.size() > 0) {
            reportModel.ListChild.remove(index);
        }
    }

    public int getChildSize() {
        if (reportModel.ListChild != null) {
            return reportModel.ListChild.size();
        }
        return 0;
    }

    public void addPrisoner(PrisonerModel prisonerModel, int index) {
        if (reportModel.ListPrisoner == null) {
            reportModel.ListPrisoner = new ArrayList<>();
        }
        if (index == -1) {
            reportModel.ListPrisoner.add(prisonerModel);
        } else {
            reportModel.ListPrisoner.get(index).Id = prisonerModel.Id;
            reportModel.ListPrisoner.get(index).ReportId = prisonerModel.ReportId;
            reportModel.ListPrisoner.get(index).Name = prisonerModel.Name;
            reportModel.ListPrisoner.get(index).Gender = prisonerModel.Gender;
            reportModel.ListPrisoner.get(index).GenderName = prisonerModel.GenderName;
            reportModel.ListPrisoner.get(index).Age = prisonerModel.Age;
            reportModel.ListPrisoner.get(index).Birthday = prisonerModel.Birthday;
            reportModel.ListPrisoner.get(index).Phone = prisonerModel.Phone;
            reportModel.ListPrisoner.get(index).Relationship = prisonerModel.Relationship;
            reportModel.ListPrisoner.get(index).RelationshipName = prisonerModel.RelationshipName;
            reportModel.ListPrisoner.get(index).Address = prisonerModel.Address;
            reportModel.ListPrisoner.get(index).ProvinceId = prisonerModel.ProvinceId;
            reportModel.ListPrisoner.get(index).ProvinceName = prisonerModel.ProvinceName;
            reportModel.ListPrisoner.get(index).DistrictId = prisonerModel.DistrictId;
            reportModel.ListPrisoner.get(index).DistrictName = prisonerModel.DistrictName;
            reportModel.ListPrisoner.get(index).WardId = prisonerModel.WardId;
            reportModel.ListPrisoner.get(index).WardName = prisonerModel.WardName;
            reportModel.ListPrisoner.get(index).FullAddress = prisonerModel.FullAddress;
        }
    }

    public PrisonerModel getPrisoner(int index) {
        if (reportModel.ListPrisoner != null && reportModel.ListPrisoner.size() > 0) {
            return reportModel.ListPrisoner.get(index);
        }
        return null;
    }

    public void removePrisoner(int index) {
        if (reportModel.ListPrisoner != null && reportModel.ListPrisoner.size() > 0) {
            reportModel.ListPrisoner.remove(index);
        }
    }

    //Dữ liệu local
    public List<ComboboxItem> ListProvince = new ArrayList<>();
    public List<ComboboxItem> ListRelationship = new ArrayList<>();
    public List<InternetModel> ListInternetImage = new ArrayList<>();
    public List<InternetModel> ListInternetVideo = new ArrayList<>();
    public List<VideoModel> ListGenderVideo = new ArrayList<>();
}
