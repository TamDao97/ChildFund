package nts.childprofile.Sql;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.support.annotation.Nullable;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import nts.childprofile.common.Constants;
import nts.childprofile.common.DateUtils;
import nts.childprofile.common.Utils;
import nts.childprofile.model.ChildProfileModel;
import nts.childprofile.model.ChildProfileSearchCondition;
import nts.childprofile.model.ChildProfileSearchResult;
import nts.childprofile.model.ComboboxResult;
import nts.childprofile.model.FamilyMemberModel;
import nts.childprofile.model.ImageChildByYearModel;
import nts.childprofile.model.ReportProfilesModel;

public class DataBaseHelper extends SQLiteOpenHelper {
    private Context context;

    public DataBaseHelper(Context context, String name, SQLiteDatabase.CursorFactory factory, int version) {
        super(context, name, factory, version);
        this.context = context;
    }

    @Override
    public void onCreate(SQLiteDatabase db) {

    }

    public void queryData(String sql) {
        SQLiteDatabase database = getWritableDatabase();
        database.execSQL(sql);
    }

    public void insert(String tableName, ContentValues values) {
        SQLiteDatabase database = this.getWritableDatabase();
        if (checkExistsData(tableName, values.getAsString("Id")).getCount() == 0) {
            database.insert(tableName, null, values);
        }
        database.close();
    }

    public void insertReportChildProfile(String tableName, ContentValues values) {
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(Constants.DATABASE_TABLE_ReportProfile, "ChildProfileId=?", new String[]{values.getAsString("ChildProfileId")});
        db.insert(tableName, null, values);
    }

    public void deleteReport(String chileProfileId) {
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(Constants.DATABASE_TABLE_ReportProfile, "ChildProfileId=?", new String[]{chileProfileId});
    }

    public ReportProfilesModel getReportInfo(String childProfileId) {
        ReportProfilesModel reportProfilesModel = null;
        String query = "Select * from ReportProfile where ChildProfileId='" + childProfileId + "'";
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        if (cursor != null) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    reportProfilesModel = new ReportProfilesModel();
                    reportProfilesModel.Content = cursor.getString(cursor.getColumnIndex("Content"));
                    reportProfilesModel.Description = cursor.getString(cursor.getColumnIndex("Description"));
                    reportProfilesModel.ChildProfileId = cursor.getString(cursor.getColumnIndex("ChildProfileId"));
                    reportProfilesModel.Status = cursor.getString(cursor.getColumnIndex("ProcessStatus"));
                    reportProfilesModel.IsDelete = Boolean.valueOf(cursor.getString(cursor.getColumnIndex("IsDelete")));
                    reportProfilesModel.Url = cursor.getString(cursor.getColumnIndex("Url"));
                    reportProfilesModel.CreateBy = cursor.getString(cursor.getColumnIndex("CreateBy"));
                    reportProfilesModel.UpdateBy = cursor.getString(cursor.getColumnIndex("UpdateBy"));
                    cursor.moveToNext();
                }
            }
        }
        cursor.close();
        return reportProfilesModel;
    }

    public List<ReportProfilesModel> getReportChildProfile() {
        List<ReportProfilesModel> listReport = new ArrayList<>();
        ReportProfilesModel reportProfilesModel;
        String query = "Select * from ReportProfile";
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        if (cursor != null) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    reportProfilesModel = new ReportProfilesModel();
                    reportProfilesModel.Content = cursor.getString(cursor.getColumnIndex("Content"));
                    reportProfilesModel.Description = cursor.getString(cursor.getColumnIndex("Description"));
                    reportProfilesModel.ChildProfileId = cursor.getString(cursor.getColumnIndex("ChildProfileId"));
                    reportProfilesModel.Status = cursor.getString(cursor.getColumnIndex("ProcessStatus"));
                    reportProfilesModel.IsDelete = Boolean.valueOf(cursor.getString(cursor.getColumnIndex("IsDelete")));
                    reportProfilesModel.Url = cursor.getString(cursor.getColumnIndex("Url"));
                    reportProfilesModel.CreateBy = cursor.getString(cursor.getColumnIndex("CreateBy"));
                    reportProfilesModel.UpdateBy = cursor.getString(cursor.getColumnIndex("UpdateBy"));
                    listReport.add(reportProfilesModel);
                    cursor.moveToNext();
                }
            }
        }
        cursor.close();
        return listReport;
    }

    public Cursor checkExistsData(String tableName, String Id) {
        String query = "Select * From " + tableName + " where Id ='" + Id + "'";
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        return cursor;
    }

    public boolean delete(String tableName, String Id) {
        SQLiteDatabase db = this.getWritableDatabase();
        return db.delete(tableName, "Id=?", new String[]{Id}) > 0;
    }

    public Cursor getAllChildProfileId() {
        String query = "Select Id, ChildCode From " + Constants.DATABASE_TABLE_ChildProfile;
        SQLiteDatabase database = getReadableDatabase();
        return database.rawQuery(query, null);
    }

    public List<ChildProfileSearchResult> searchChildProfile(ChildProfileSearchCondition childProfileSearchCondition) {
        List<ChildProfileSearchResult> listResult = new ArrayList<>();
        ChildProfileSearchResult childProfileSearchResult;
        String query = "Select Id,ImageThumbnailPath ,Name, ProcessStatus,ChildCode,DateOfBirth,Gender,CreateDate,SchoolName,TypeChildProfile From " + Constants.DATABASE_TABLE_ChildProfile + " where 1=1";
        if (childProfileSearchCondition.ChildCode != null && childProfileSearchCondition.ChildCode != "" && !childProfileSearchCondition.ChildCode.isEmpty()) {
            query = query + " and ChildCode like '%" + childProfileSearchCondition.ChildCode + "%'";
        }
        if (childProfileSearchCondition.Name != null && childProfileSearchCondition.Name != "" && !childProfileSearchCondition.Name.isEmpty()) {
            query = query + " and Name like '%" + childProfileSearchCondition.Name + "%'";
        }
        if (childProfileSearchCondition.Address != null && childProfileSearchCondition.Address != "" && !childProfileSearchCondition.Address.isEmpty()) {
            query = query + " and SchoolName like '%" + childProfileSearchCondition.Address + "%'";
        }
        if (childProfileSearchCondition.OrderType) {
            query = query + " order by Name asc";
        } else {
            query = query + " order by Name desc";

        }
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        if (cursor.getCount() > 0) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    childProfileSearchResult = new ChildProfileSearchResult();
                    childProfileSearchResult.Id = cursor.getString(cursor.getColumnIndex("Id"));
                    childProfileSearchResult.Avata = cursor.getString(cursor.getColumnIndex("ImageThumbnailPath"));
                    childProfileSearchResult.Name = cursor.getString(cursor.getColumnIndex("Name"));
                    childProfileSearchResult.Status = cursor.getString(cursor.getColumnIndex("ProcessStatus"));
                    childProfileSearchResult.ChildCode = cursor.getString(cursor.getColumnIndex("ChildCode"));
                    childProfileSearchResult.DateOfBirth = cursor.getString(cursor.getColumnIndex("DateOfBirth"));
                    childProfileSearchResult.Gender = cursor.getString(cursor.getColumnIndex("Gender"));
                    childProfileSearchResult.CreateDate = cursor.getString(cursor.getColumnIndex("CreateDate"));
                    childProfileSearchResult.School = cursor.getString(cursor.getColumnIndex("SchoolName"));
                    childProfileSearchResult.TypeChildProfile = cursor.getString(cursor.getColumnIndex("TypeChildProfile"));
                    listResult.add(childProfileSearchResult);
                    cursor.moveToNext();
                }
            }
        }
        cursor.close();
        return listResult;
    }

    public List<ChildProfileModel> getListChildProfileToSync() {
        ChildProfileModel childProfileModel;
        String query = "Select * from ChildProfile where TypeChildProfile<>'" + Constants.TYPE_CHILDPROFILE_SQLITE_DOWNLOAD + "'";
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        List<ChildProfileModel> listResult = new ArrayList<>();
        if (cursor != null) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    childProfileModel = new ChildProfileModel();
                    childProfileModel.Id = cursor.getString(cursor.getColumnIndex("Id"));
                    childProfileModel.InfoDate = cursor.getString(cursor.getColumnIndex("InfoDate"));
                    childProfileModel.EmployeeName = cursor.getString(cursor.getColumnIndex("EmployeeName"));
                    childProfileModel.EmployeeTitle = cursor.getString(cursor.getColumnIndex("EmployeeTitle"));
                    childProfileModel.ProgramCode = cursor.getString(cursor.getColumnIndex("ProgramCode"));
                    childProfileModel.ProvinceId = cursor.getString(cursor.getColumnIndex("ProvinceId"));
                    childProfileModel.DistrictId = cursor.getString(cursor.getColumnIndex("DistrictId"));
                    childProfileModel.WardId = cursor.getString(cursor.getColumnIndex("WardId"));
                    childProfileModel.Address = cursor.getString(cursor.getColumnIndex("Address"));
                    childProfileModel.ChildCode = cursor.getString(cursor.getColumnIndex("ChildCode"));
                    childProfileModel.SchoolId = cursor.getString(cursor.getColumnIndex("SchoolId"));
                    childProfileModel.SchoolOtherName = cursor.getString(cursor.getColumnIndex("SchoolOtherName"));
                    childProfileModel.EthnicId = cursor.getString(cursor.getColumnIndex("EthnicId"));
                    childProfileModel.ReligionId = cursor.getString(cursor.getColumnIndex("ReligionId"));
                    childProfileModel.Name = cursor.getString(cursor.getColumnIndex("Name"));
                    childProfileModel.NickName = cursor.getString(cursor.getColumnIndex("NickName"));
                    childProfileModel.Gender = Integer.parseInt(cursor.getString(cursor.getColumnIndex("Gender")));
                    childProfileModel.DateOfBirth = cursor.getString(cursor.getColumnIndex("DateOfBirth"));
                    childProfileModel.LeaningStatus = cursor.getString(cursor.getColumnIndex("LeaningStatus"));
                    childProfileModel.ClassInfo = cursor.getString(cursor.getColumnIndex("ClassInfo"));
                    childProfileModel.FavouriteSubject = cursor.getString(cursor.getColumnIndex("FavouriteSubject"));
                    childProfileModel.LearningCapacity = cursor.getString(cursor.getColumnIndex("LearningCapacity"));
                    childProfileModel.Housework = cursor.getString(cursor.getColumnIndex("Housework"));
                    childProfileModel.Health = cursor.getString(cursor.getColumnIndex("Health"));
                    childProfileModel.Personality = cursor.getString(cursor.getColumnIndex("Personality"));
                    childProfileModel.Hobby = cursor.getString(cursor.getColumnIndex("Hobby"));
                    childProfileModel.Dream = cursor.getString(cursor.getColumnIndex("Dream"));
                    childProfileModel.FamilyMember = cursor.getString(cursor.getColumnIndex("FamilyMember"));
                    childProfileModel.ListFamilyMember = new Gson().fromJson(childProfileModel.FamilyMember, new TypeToken<List<FamilyMemberModel>>() {
                    }.getType());
                    childProfileModel.LivingWithParent = cursor.getString(cursor.getColumnIndex("LivingWithParent"));
                    childProfileModel.NotLivingWithParent = cursor.getString(cursor.getColumnIndex("NotLivingWithParent"));
                    childProfileModel.LivingWithOther = cursor.getString(cursor.getColumnIndex("LivingWithOther"));
                    childProfileModel.LetterWrite = cursor.getString(cursor.getColumnIndex("LetterWrite"));
                    childProfileModel.HouseType = cursor.getString(cursor.getColumnIndex("HouseType"));
                    childProfileModel.HouseRoof = cursor.getString(cursor.getColumnIndex("HouseRoof"));
                    childProfileModel.HouseWall = cursor.getString(cursor.getColumnIndex("HouseWall"));
                    childProfileModel.HouseFloor = cursor.getString(cursor.getColumnIndex("HouseFloor"));
                    childProfileModel.UseElectricity = cursor.getString(cursor.getColumnIndex("UseElectricity"));
                    childProfileModel.SchoolDistance = cursor.getString(cursor.getColumnIndex("SchoolDistance"));
                    childProfileModel.ClinicDistance = cursor.getString(cursor.getColumnIndex("ClinicDistance"));
                    childProfileModel.WaterSourceDistance = cursor.getString(cursor.getColumnIndex("WaterSourceDistance"));
                    childProfileModel.WaterSourceUse = cursor.getString(cursor.getColumnIndex("WaterSourceUse"));
                    childProfileModel.RoadCondition = cursor.getString(cursor.getColumnIndex("RoadCondition"));
                    childProfileModel.IncomeFamily = cursor.getString(cursor.getColumnIndex("IncomeFamily"));
                    childProfileModel.HarvestOutput = cursor.getString(cursor.getColumnIndex("HarvestOutput"));
                    childProfileModel.NumberPet = cursor.getString(cursor.getColumnIndex("NumberPet"));
                    childProfileModel.FamilyType = cursor.getString(cursor.getColumnIndex("FamilyType"));
                    childProfileModel.TotalIncome = cursor.getString(cursor.getColumnIndex("TotalIncome"));
                    childProfileModel.IncomeSources = cursor.getString(cursor.getColumnIndex("IncomeSources"));
                    childProfileModel.IncomeOther = cursor.getString(cursor.getColumnIndex("IncomeOther"));
                    childProfileModel.ImagePath = cursor.getString(cursor.getColumnIndex("ImagePath"));
                    childProfileModel.ImageThumbnailPath = cursor.getString(cursor.getColumnIndex("ImageThumbnailPath"));
                    childProfileModel.ConsentName = cursor.getString(cursor.getColumnIndex("ConsentName"));
                    childProfileModel.ConsentRelationship = cursor.getString(cursor.getColumnIndex("ConsentRelationship"));
                    childProfileModel.ConsentVillage = cursor.getString(cursor.getColumnIndex("ConsentVillage"));
                    childProfileModel.ConsentWard = cursor.getString(cursor.getColumnIndex("ConsentWard"));
                    childProfileModel.SiblingsJoiningChildFund = cursor.getString(cursor.getColumnIndex("SiblingsJoiningChildFund"));
                    childProfileModel.Malformation = cursor.getString(cursor.getColumnIndex("Malformation"));
                    childProfileModel.Orphan = cursor.getString(cursor.getColumnIndex("Orphan"));
                    childProfileModel.ImageSignaturePath = cursor.getString(cursor.getColumnIndex("ImageSignaturePath"));
                    childProfileModel.ImageSignatureThumbnailPath = cursor.getString(cursor.getColumnIndex("ImageSignatureThumbnailPath"));
                    String handicap = cursor.getString(cursor.getColumnIndex("Handicap"));
                    if(handicap.equals("1")){
                        childProfileModel.Handicap = true;
                    }else {
                        childProfileModel.Handicap = false;
                    }
                    childProfileModel.SaleforceID = cursor.getString(cursor.getColumnIndex("SaleforceId"));
                    childProfileModel.TypeChildProfile = cursor.getString(cursor.getColumnIndex("TypeChildProfile"));
                    listResult.add(childProfileModel);
                    cursor.moveToNext();
                }
            }
        }
        cursor.close();
        return listResult;
    }

    public ChildProfileModel getInfoChildProfile(String id) {
        ChildProfileModel childProfileModel = new ChildProfileModel();
        String query = "Select * from ChildProfile where Id='" + id + "'";
        SQLiteDatabase database = getReadableDatabase();
        Cursor cursor = database.rawQuery(query, null);
        if (cursor != null) {
            if (cursor.moveToFirst()) {
                childProfileModel.Id = cursor.getString(cursor.getColumnIndex("Id"));
                childProfileModel.InfoDate = cursor.getString(cursor.getColumnIndex("InfoDate"));
                childProfileModel.EmployeeName = cursor.getString(cursor.getColumnIndex("EmployeeName"));
                childProfileModel.EmployeeTitle = cursor.getString(cursor.getColumnIndex("EmployeeTitle"));
                childProfileModel.ProgramCode = cursor.getString(cursor.getColumnIndex("ProgramCode"));
                childProfileModel.ProvinceId = cursor.getString(cursor.getColumnIndex("ProvinceId"));
                childProfileModel.DistrictId = cursor.getString(cursor.getColumnIndex("DistrictId"));
                childProfileModel.WardId = cursor.getString(cursor.getColumnIndex("WardId"));
                childProfileModel.Address = cursor.getString(cursor.getColumnIndex("Address"));
                childProfileModel.ChildCode = cursor.getString(cursor.getColumnIndex("ChildCode"));
                childProfileModel.SchoolId = cursor.getString(cursor.getColumnIndex("SchoolId"));
                childProfileModel.SchoolOtherName = cursor.getString(cursor.getColumnIndex("SchoolOtherName"));
                childProfileModel.EthnicId = cursor.getString(cursor.getColumnIndex("EthnicId"));
                childProfileModel.ReligionId = cursor.getString(cursor.getColumnIndex("ReligionId"));
                childProfileModel.Name = cursor.getString(cursor.getColumnIndex("Name"));
                childProfileModel.NickName = cursor.getString(cursor.getColumnIndex("NickName"));
                childProfileModel.Gender = Integer.parseInt(cursor.getString(cursor.getColumnIndex("Gender")));
                childProfileModel.DateOfBirth = cursor.getString(cursor.getColumnIndex("DateOfBirth"));
                childProfileModel.LeaningStatus = cursor.getString(cursor.getColumnIndex("LeaningStatus"));
                childProfileModel.ClassInfo = cursor.getString(cursor.getColumnIndex("ClassInfo"));
                childProfileModel.FavouriteSubject = cursor.getString(cursor.getColumnIndex("FavouriteSubject"));
                childProfileModel.LearningCapacity = cursor.getString(cursor.getColumnIndex("LearningCapacity"));
                childProfileModel.Housework = cursor.getString(cursor.getColumnIndex("Housework"));
                childProfileModel.Health = cursor.getString(cursor.getColumnIndex("Health"));
                childProfileModel.Personality = cursor.getString(cursor.getColumnIndex("Personality"));
                childProfileModel.Hobby = cursor.getString(cursor.getColumnIndex("Hobby"));
                childProfileModel.Dream = cursor.getString(cursor.getColumnIndex("Dream"));
                childProfileModel.FamilyMember = cursor.getString(cursor.getColumnIndex("FamilyMember"));
                childProfileModel.LivingWithParent = cursor.getString(cursor.getColumnIndex("LivingWithParent"));
                childProfileModel.NotLivingWithParent = cursor.getString(cursor.getColumnIndex("NotLivingWithParent"));
                childProfileModel.LivingWithOther = cursor.getString(cursor.getColumnIndex("LivingWithOther"));
                childProfileModel.LetterWrite = cursor.getString(cursor.getColumnIndex("LetterWrite"));
                childProfileModel.HouseType = cursor.getString(cursor.getColumnIndex("HouseType"));
                childProfileModel.HouseRoof = cursor.getString(cursor.getColumnIndex("HouseRoof"));
                childProfileModel.HouseWall = cursor.getString(cursor.getColumnIndex("HouseWall"));
                childProfileModel.HouseFloor = cursor.getString(cursor.getColumnIndex("HouseFloor"));
                childProfileModel.UseElectricity = cursor.getString(cursor.getColumnIndex("UseElectricity"));
                childProfileModel.SchoolDistance = cursor.getString(cursor.getColumnIndex("SchoolDistance"));
                childProfileModel.ClinicDistance = cursor.getString(cursor.getColumnIndex("ClinicDistance"));
                childProfileModel.WaterSourceDistance = cursor.getString(cursor.getColumnIndex("WaterSourceDistance"));
                childProfileModel.WaterSourceUse = cursor.getString(cursor.getColumnIndex("WaterSourceUse"));
                childProfileModel.RoadCondition = cursor.getString(cursor.getColumnIndex("RoadCondition"));
                childProfileModel.IncomeFamily = cursor.getString(cursor.getColumnIndex("IncomeFamily"));
                childProfileModel.HarvestOutput = cursor.getString(cursor.getColumnIndex("HarvestOutput"));
                childProfileModel.NumberPet = cursor.getString(cursor.getColumnIndex("NumberPet"));
                childProfileModel.FamilyType = cursor.getString(cursor.getColumnIndex("FamilyType"));
                childProfileModel.TotalIncome = cursor.getString(cursor.getColumnIndex("TotalIncome"));
                childProfileModel.IncomeSources = cursor.getString(cursor.getColumnIndex("IncomeSources"));
                childProfileModel.IncomeOther = cursor.getString(cursor.getColumnIndex("IncomeOther"));
                childProfileModel.ImagePath = cursor.getString(cursor.getColumnIndex("ImagePath"));
                childProfileModel.ImageThumbnailPath = cursor.getString(cursor.getColumnIndex("ImageThumbnailPath"));
                childProfileModel.ConsentName = cursor.getString(cursor.getColumnIndex("ConsentName"));
                childProfileModel.ConsentRelationship = cursor.getString(cursor.getColumnIndex("ConsentRelationship"));
                childProfileModel.ConsentVillage = cursor.getString(cursor.getColumnIndex("ConsentVillage"));
                childProfileModel.ConsentWard = cursor.getString(cursor.getColumnIndex("ConsentWard"));
                childProfileModel.SiblingsJoiningChildFund = cursor.getString(cursor.getColumnIndex("SiblingsJoiningChildFund"));
                childProfileModel.Malformation = cursor.getString(cursor.getColumnIndex("Malformation"));
                childProfileModel.Orphan = cursor.getString(cursor.getColumnIndex("Orphan"));
                childProfileModel.ImageSignaturePath = cursor.getString(cursor.getColumnIndex("ImageSignaturePath"));
                childProfileModel.ImageSignatureThumbnailPath = cursor.getString(cursor.getColumnIndex("ImageSignatureThumbnailPath"));

                String handicap = cursor.getString(cursor.getColumnIndex("Handicap"));
                if(handicap.equals("1")){
                    childProfileModel.Handicap = true;
                }else {
                    childProfileModel.Handicap = false;
                }
                childProfileModel.SaleforceID = cursor.getString(cursor.getColumnIndex("SaleforceId"));
                childProfileModel.TypeChildProfile = cursor.getString(cursor.getColumnIndex("TypeChildProfile"));
            }
            cursor.close();
        }
        childProfileModel.convertObjectJsonToModel(context);
        return childProfileModel;
    }

    public void updateChildProfile(ChildProfileModel childProfileModel) {
        SQLiteDatabase database = this.getWritableDatabase();
        ContentValues values = new ContentValues();
        values.put("Id", childProfileModel.Id);
        values.put("InfoDate", childProfileModel.InfoDate);
        values.put("EmployeeName", childProfileModel.EmployeeName);
        values.put("ProgramCode", childProfileModel.ProgramCode);
        values.put("ProvinceId", childProfileModel.ProvinceId);
        values.put("DistrictId", childProfileModel.DistrictId);
        values.put("WardId", childProfileModel.WardId);
        values.put("Address", childProfileModel.Address);
        values.put("FullAddress", childProfileModel.FullAddress);
        values.put("ChildCode", childProfileModel.ChildCode);
        values.put("SchoolId", childProfileModel.SchoolId);
        values.put("SchoolOtherName", childProfileModel.SchoolOtherName);
        values.put("SchoolName", childProfileModel.School);
        values.put("EthnicId", childProfileModel.EthnicId);
        values.put("ReligionId", childProfileModel.ReligionId);
        values.put("Name", childProfileModel.Name);
        values.put("NickName", childProfileModel.NickName);
        values.put("Gender", childProfileModel.Gender);
        values.put("DateOfBirth", childProfileModel.DateOfBirth);
        values.put("LeaningStatus", childProfileModel.LeaningStatus);
        values.put("ClassInfo", childProfileModel.ClassInfo);
        values.put("FavouriteSubject", new Gson().toJson(childProfileModel.FavouriteSubjectModel));
        values.put("LearningCapacity", new Gson().toJson(childProfileModel.LearningCapacityModel));
        values.put("Housework", new Gson().toJson(childProfileModel.HouseworkModel));
        values.put("Health", new Gson().toJson(childProfileModel.HealthModel));
        values.put("Personality", new Gson().toJson(childProfileModel.PersonalityModel));
        values.put("Hobby", new Gson().toJson(childProfileModel.HobbyModel));
        values.put("Dream", new Gson().toJson(childProfileModel.DreamModel));
        values.put("FamilyMember", new Gson().toJson(childProfileModel.ListFamilyMember));
        values.put("LivingWithParent", new Gson().toJson(childProfileModel.LivingWithParentModel));
        values.put("NotLivingWithParent", new Gson().toJson(childProfileModel.NotLivingWithParentModel));
        values.put("LivingWithOther", new Gson().toJson(childProfileModel.LivingWithOtherModel));
        values.put("LetterWrite", new Gson().toJson(childProfileModel.LetterWriteModel));
        values.put("HouseType", new Gson().toJson(childProfileModel.HouseTypeModel));
        values.put("HouseRoof", new Gson().toJson(childProfileModel.HouseRoofModel));
        values.put("HouseWall", new Gson().toJson(childProfileModel.HouseWallModel));
        values.put("HouseFloor", new Gson().toJson(childProfileModel.HouseFloorModel));
        values.put("UseElectricity", new Gson().toJson(childProfileModel.UseElectricityModel));
        values.put("SchoolDistance", new Gson().toJson(childProfileModel.SchoolDistanceModel));
        values.put("ClinicDistance", new Gson().toJson(childProfileModel.ClinicDistanceModel));
        values.put("WaterSourceDistance", new Gson().toJson(childProfileModel.WaterSourceDistanceModel));
        values.put("WaterSourceUse", new Gson().toJson(childProfileModel.WaterSourceUseModel));
        values.put("RoadCondition", new Gson().toJson(childProfileModel.RoadConditionModel));
        values.put("IncomeFamily", new Gson().toJson(childProfileModel.IncomeFamilyModel));
        values.put("HarvestOutput", new Gson().toJson(childProfileModel.HarvestOutputModel));
        values.put("NumberPet", new Gson().toJson(childProfileModel.NumberPetModel));
        values.put("FamilyType", childProfileModel.FamilyType);
        values.put("TotalIncome", childProfileModel.TotalIncome);
        values.put("IncomeSources", childProfileModel.IncomeSources);
        values.put("IncomeOther", new Gson().toJson(childProfileModel.IncomeOtherModel));
        values.put("StoryContent", childProfileModel.StoryContent);
        values.put("AreaApproverId", childProfileModel.AreaApproverId);
        values.put("AreaApproverDate", childProfileModel.AreaApproverDate);
        values.put("OfficeApproveBy", childProfileModel.OfficeApproveBy);
        values.put("OfficeApproveDate", childProfileModel.OfficeApproveDate);
        values.put("ProcessStatus", "0");
        values.put("IsDelete", childProfileModel.IsDelete);
        values.put("CreateBy", childProfileModel.CreateBy);
        values.put("CreateDate", childProfileModel.CreateDate);
        values.put("UpdateBy", childProfileModel.UpdateBy);
        values.put("UpdateDate", childProfileModel.UpdateDate);
        values.put("ConsentName", childProfileModel.ConsentName);
        values.put("ConsentRelationship", childProfileModel.ConsentRelationship);
        values.put("ConsentVillage", childProfileModel.ConsentVillage);
        values.put("ConsentWard", childProfileModel.ConsentWard);
        values.put("SiblingsJoiningChildFund", new Gson().toJson(childProfileModel.SiblingsJoiningChildFundModel));
        values.put("Malformation", new Gson().toJson(childProfileModel.MalformationModel));
        values.put("Orphan", new Gson().toJson(childProfileModel.OrphanModel));
        values.put("EmployeeTitle", childProfileModel.EmployeeTitle);
        values.put("ImageSignaturePath", childProfileModel.ImageSignaturePath);
        values.put("ImageSignatureThumbnailPath", childProfileModel.ImageSignatureThumbnailPath);
        values.put("ImagePath", childProfileModel.ImagePath);
        values.put("ImageThumbnailPath", childProfileModel.ImageThumbnailPath);
//        values.put("SaleforceId", childProfileModel.SaleforceId);
        values.put("Handicap", childProfileModel.Handicap);
        values.put("ImageSize", childProfileModel.ImageSize);

        if (!Utils.isEmpty(childProfileModel.Id)) {
            if (childProfileModel.TypeChildProfile.equals(Constants.TYPE_CHILDPROFILE_SQLITE_CREATED)) {
                values.put("TypeChildProfile", Constants.TYPE_CHILDPROFILE_SQLITE_CREATED);
                values.put("Id", DateUtils.CurrentDate("dd-MM-yyyy hh:mm:ss"));
                database.update(Constants.DATABASE_TABLE_ChildProfile, values, "Id=?", new String[]{childProfileModel.Id});
            } else {
                values.put("TypeChildProfile", Constants.TYPE_CHILDPROFILE_SQLITE_UPDATED);
                database.update(Constants.DATABASE_TABLE_ChildProfile, values, "Id=?", new String[]{childProfileModel.Id});
            }
        } else {
            values.put("TypeChildProfile", Constants.TYPE_CHILDPROFILE_SQLITE_CREATED);
            values.put("Id", DateUtils.CurrentDate("dd-MM-yyyy hh:mm:ss"));
            database.insert(Constants.DATABASE_TABLE_ChildProfile, null, values);
        }
        database.close();
    }

    public void createImageCPR(ContentValues values) {
        SQLiteDatabase database = getWritableDatabase();
        deleteImageCPR(values.getAsString("ChildProfileId"));
        database.insert(Constants.DATABASE_TABLE_ImageChildByYear, null, values);
    }

    public void deleteImageCPR(String chileProfileId) {
        SQLiteDatabase db = this.getWritableDatabase();
        db.delete(Constants.DATABASE_TABLE_ImageChildByYear, "ChildProfileId=?", new String[]{chileProfileId});
    }

    public List<ImageChildByYearModel> getListImageCPR() {
        List<ImageChildByYearModel> listImageCPR = new ArrayList<>();
        ImageChildByYearModel imageChildByYearModel;
        SQLiteDatabase database = getReadableDatabase();
        String query = "Select * from ImageChildByYear";
        Cursor cursor = database.rawQuery(query, null);
        if (cursor != null) {
            if (cursor.moveToFirst()) {
                while (!cursor.isAfterLast()) {
                    imageChildByYearModel = new ImageChildByYearModel();
                    imageChildByYearModel.ChildProfileId = cursor.getString(cursor.getColumnIndex("ChildProfileId"));
                    imageChildByYearModel.ImageUrl = cursor.getString(cursor.getColumnIndex("ImageUrl"));
                    listImageCPR.add(imageChildByYearModel);
                    cursor.moveToNext();
                }
            }
        }
        cursor.close();
        return listImageCPR;
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {

    }
}
