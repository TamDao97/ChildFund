package nts.childprofile.model;

import android.content.Context;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;

import java.io.IOException;
import java.io.InputStream;
import java.util.List;

/**
 * Created by NTS-VANVV on 07/01/2019.
 */

public class ChildProfileModel {
    public String Id;
    public String InfoDate;
    public String EmployeeName;
    public String EmployeeTitle;
    public String ProgramCode;
    public String ProvinceId;
    public String DistrictId;
    public String WardId;
    public String Address;
    public String FullAddress;
    public String ChildCode;
    public String SchoolId;
    public String SchoolOtherName;
    public String EthnicId;
    public String ReligionId;
    public String Name;
    public String NickName;
    public int Gender;
    public String DateOfBirth;
    public String LeaningStatus;
    public String ClassInfo;
    public ObjectBaseModel FavouriteSubjectModel;
    public ObjectBaseModel LearningCapacityModel;
    public ObjectBaseModel HouseworkModel;
    public ObjectBaseModel HealthModel;
    public ObjectBaseModel PersonalityModel;
    public ObjectBaseModel HobbyModel;
    public ObjectBaseModel DreamModel;
    public List<FamilyMemberModel> ListFamilyMember;
    public ObjectBaseModel LivingWithParentModel;
    public ObjectBaseModel NotLivingWithParentModel;
    public ObjectBaseModel LivingWithOtherModel;
    public ObjectBaseModel LetterWriteModel;
    public ObjectBaseModel HouseTypeModel;
    public ObjectBaseModel HouseRoofModel;
    public ObjectBaseModel HouseWallModel;
    public ObjectBaseModel HouseFloorModel;
    public ObjectBaseModel UseElectricityModel;
    public ObjectBaseModel SchoolDistanceModel;
    public ObjectBaseModel ClinicDistanceModel;
    public ObjectBaseModel WaterSourceDistanceModel;
    public ObjectBaseModel WaterSourceUseModel;
    public ObjectBaseModel RoadConditionModel;
    public ObjectBaseModel IncomeFamilyModel;
    public ObjectBaseModel HarvestOutputModel;
    public ObjectBaseModel NumberPetModel;
    public String FamilyType;
    public String TotalIncome;
    public String IncomeSources;
    public ObjectBaseModel IncomeOtherModel;
    public String StoryContent;
    public String ImagePath;
    public String ImageThumbnailPath;
    public String AreaApproverId;
    public String AreaApproverDate;
    public String OfficeApproveBy;
    public String OfficeApproveDate;
    public String ProcessStatus;
    public boolean IsDelete;
    public String CreateBy;
    public String CreateDate;
    public String UpdateBy;
    public String UpdateDate;
    public String ImageSignaturePath;
    public String ImageSignatureThumbnailPath;
    public String UserLever;
    public boolean IsRemoveImage;
    public String ConsentName;
    public String ConsentRelationship;
    public String ConsentVillage;
    public String ConsentWard;
    public ObjectBaseModel SiblingsJoiningChildFundModel;
    public ObjectBaseModel MalformationModel;
    public ObjectBaseModel OrphanModel;
    public boolean Handicap;
    public String SaleforceID;
    public String ImageSize;

    public String FavouriteSubject;
    public String LearningCapacity;
    public String Housework;
    public String Health;
    public String Personality;
    public String Hobby;
    public String Dream;
    public String LivingWithParent;
    public String NotLivingWithParent;
    public String LivingWithOther;
    public String LetterWrite;
    public String HouseType;
    public String HouseRoof;
    public String HouseWall;
    public String HouseFloor;
    public String UseElectricity;
    public String SchoolDistance;
    public String ClinicDistance;
    public String WaterSourceDistance;
    public String WaterSourceUse;
    public String RoadCondition;
    public String IncomeFamily;
    public String HarvestOutput;
    public String NumberPet;
    public String SiblingsJoiningChildFund;
    public String Malformation;
    public String Orphan;
    public String IncomeOther;
    public String FamilyMember;
    public String School;
    public String TypeChildProfile;

    public void convertObjectJsonToModel(Context context) {
        if (FavouriteSubject != null && FavouriteSubject != "" && !FavouriteSubject.isEmpty()) {
            FavouriteSubjectModel = new Gson().fromJson(FavouriteSubject, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("FavouriteSubject.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                FavouriteSubjectModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (ClinicDistance != null && ClinicDistance != "" && !ClinicDistance.isEmpty()) {
            ClinicDistanceModel = new Gson().fromJson(ClinicDistance, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("ClinicDistance.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                ClinicDistanceModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Dream != null && Dream != "" && !Dream.isEmpty()) {
            DreamModel = new Gson().fromJson(Dream, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Dream.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                DreamModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (HarvestOutput != null && HarvestOutput != "" && !HarvestOutput.isEmpty()) {
            HarvestOutputModel = new Gson().fromJson(HarvestOutput, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("HarvestOutput.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HarvestOutputModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Health != null && Health != "" && !Health.isEmpty()) {
            HealthModel = new Gson().fromJson(Health, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Health.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HealthModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Hobby != null && Hobby != "" && !Hobby.isEmpty()) {
            HobbyModel = new Gson().fromJson(Hobby, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Hobby.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HobbyModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (HouseFloor != null && HouseFloor != "" && !HouseFloor.isEmpty()) {
            HouseFloorModel = new Gson().fromJson(HouseFloor, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("HouseFloor.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HouseFloorModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (HouseRoof != null && HouseRoof != "" && !HouseRoof.isEmpty()) {
            HouseRoofModel = new Gson().fromJson(HouseRoof, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("HouseRoof.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HouseRoofModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (HouseType != null && HouseType != "" && !HouseType.isEmpty()) {
            HouseTypeModel = new Gson().fromJson(HouseType, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("HouseType.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HouseTypeModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (HouseWall != null && HouseWall != "" && !HouseWall.isEmpty()) {
            HouseWallModel = new Gson().fromJson(HouseWall, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("HouseWall.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HouseWallModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Housework != null && Housework != "" && !Housework.isEmpty()) {
            HouseworkModel = new Gson().fromJson(Housework, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Housework.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                HouseworkModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (IncomeFamily != null && IncomeFamily != "" && !IncomeFamily.isEmpty()) {
            IncomeFamilyModel = new Gson().fromJson(IncomeFamily, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("IncomeFamily.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                IncomeFamilyModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (IncomeOther != null && IncomeOther != "" && !IncomeOther.isEmpty()) {
            IncomeOtherModel = new Gson().fromJson(IncomeOther, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("IncomeOther.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                IncomeOtherModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (IncomeOther != null && IncomeOther != "" && !IncomeOther.isEmpty()) {
            IncomeOtherModel = new Gson().fromJson(IncomeOther, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("IncomeOther.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                IncomeOtherModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (LearningCapacity != null && LearningCapacity != "" && !LearningCapacity.isEmpty()) {
            LearningCapacityModel = new Gson().fromJson(LearningCapacity, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("LearningCapacity.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                LearningCapacityModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (LetterWrite != null && LetterWrite != "" && !LetterWrite.isEmpty()) {
            LetterWriteModel = new Gson().fromJson(LetterWrite, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("LetterWrite.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                LetterWriteModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (LivingWithOther != null && LivingWithOther != "" && !LivingWithOther.isEmpty()) {
            LivingWithOtherModel = new Gson().fromJson(LivingWithOther, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("LivingWithOther.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                LivingWithOtherModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (LivingWithParent != null && LivingWithParent != "" && !LivingWithParent.isEmpty()) {
            LivingWithParentModel = new Gson().fromJson(LivingWithParent, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("LivingWithParent.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                LivingWithParentModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Malformation != null && Malformation != "" && !Malformation.isEmpty()) {
            MalformationModel = new Gson().fromJson(Malformation, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Malformation.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                MalformationModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (NotLivingWithParent != null && NotLivingWithParent != "" && !NotLivingWithParent.isEmpty()) {
            NotLivingWithParentModel = new Gson().fromJson(NotLivingWithParent, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("NotLivingWithParent.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                NotLivingWithParentModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (NumberPet != null && NumberPet != "" && !NumberPet.isEmpty()) {
            NumberPetModel = new Gson().fromJson(NumberPet, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("NumberPet.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                NumberPetModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Orphan != null && Orphan != "" && !Orphan.isEmpty()) {
            OrphanModel = new Gson().fromJson(Orphan, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Orphan.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                OrphanModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (Personality != null && Personality != "" && !Personality.isEmpty()) {
            PersonalityModel = new Gson().fromJson(Personality, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("Personality.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                PersonalityModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (RoadCondition != null && RoadCondition != "" && !RoadCondition.isEmpty()) {
            RoadConditionModel = new Gson().fromJson(RoadCondition, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("RoadCondition.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                RoadConditionModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (SchoolDistance != null && SchoolDistance != "" && !SchoolDistance.isEmpty()) {
            SchoolDistanceModel = new Gson().fromJson(SchoolDistance, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("SchoolDistance.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                SchoolDistanceModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (SiblingsJoiningChildFund != null && SiblingsJoiningChildFund != "" && !SiblingsJoiningChildFund.isEmpty()) {
            SiblingsJoiningChildFundModel = new Gson().fromJson(SiblingsJoiningChildFund, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("SiblingsJoiningChildFund.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                SiblingsJoiningChildFundModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (UseElectricity != null && UseElectricity != "" && !UseElectricity.isEmpty()) {
            UseElectricityModel = new Gson().fromJson(UseElectricity, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("UseElectricity.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                UseElectricityModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (WaterSourceDistance != null && WaterSourceDistance != "" && !WaterSourceDistance.isEmpty()) {
            WaterSourceDistanceModel = new Gson().fromJson(WaterSourceDistance, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("WaterSourceDistance.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                WaterSourceDistanceModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (WaterSourceUse != null && WaterSourceUse != "" && !WaterSourceUse.isEmpty()) {
            WaterSourceUseModel = new Gson().fromJson(WaterSourceUse, new TypeToken<ObjectBaseModel>() {
            }.getType());
        } else {
            try {
                InputStream is = context.getAssets().open("WaterSourceUse.json");
                int size = is.available();
                byte[] buffer = new byte[size];
                is.read(buffer);
                is.close();
                String jsonString = new String(buffer, "UTF-8");
                WaterSourceUseModel = new Gson().fromJson(jsonString, new TypeToken<ObjectBaseModel>() {
                }.getType());
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        if (FamilyMember != null && FamilyMember != "" && !FamilyMember.isEmpty() && FamilyMember != "[]") {
            ListFamilyMember = new Gson().fromJson(FamilyMember, new TypeToken<List<FamilyMemberModel>>() {
            }.getType());
        }
    }
}
