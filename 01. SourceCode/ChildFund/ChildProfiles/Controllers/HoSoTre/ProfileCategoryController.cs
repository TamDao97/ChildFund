using ChildProfiles.Business.Business;
using ChildProfiles.Controllers.Base;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ChildProfiles.Controllers.HoSoTre
{
    public class ProfileCategoryController : BaseController
    {
        // GET: DanhMucHoSo
        ProfileCatalogDA _data = new ProfileCatalogDA();
        public ActionResult FavouriteSubjectModelView(ObjectBaseModel favouriteSubjectModel, ObjectBaseModel learningCapacityModel)
        {
            ViewBag.learningCapacityModel = learningCapacityModel;
            return PartialView(favouriteSubjectModel);
        }

        public ActionResult HouseworkModelView(ObjectBaseModel houseworkModel, ObjectBaseModel healthModel)
        {
            ViewBag.healthModel = healthModel;
            return PartialView(houseworkModel);
        }

        public ActionResult PersonalityModelView(ObjectBaseModel personalityModel, ObjectBaseModel hobbyModel, ObjectBaseModel dreamModel)
        {
            ViewBag.hobbyModel = hobbyModel;
            ViewBag.dreamModel = dreamModel;
            return PartialView(personalityModel);
        }

        public ActionResult FamilyInfo(string model, string typeAction)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            List<FamilyMemberModel> familyMember;
            if (!string.IsNullOrEmpty(model))
            {
                familyMember = JsonConvert.DeserializeObject<List<FamilyMemberModel>>(model, dateTimeConverter);
            }
            else familyMember = new List<FamilyMemberModel>();
            if (typeAction == "add")
            {
                familyMember.Add(new FamilyMemberModel { Gender = 1, LiveWithChild = 1 });
            }

            ViewBag.strDataTableFamilyInfo = new JavaScriptSerializer().Serialize(familyMember);
            return PartialView(familyMember);
        }

        /// <summary>
        ///Thêm row mới
        /// </summary>
        /// <param name="model"></param>
        /// <param name="typeAction"></param>
        /// <returns></returns>
        public ActionResult FamilyInfoRow(List<FamilyMemberModel> ListRelationShip)
        {
            FamilyMemberModel familyMember = null;
            Relationship relationship = null;
            List<Relationship> listRelationShip = _data.GetRelationships();
            List<FamilyMemberModel> listFamilyMember = new List<FamilyMemberModel>();

            if (ListRelationShip == null)
                ListRelationShip = new List<FamilyMemberModel>();

            foreach (var item in ListRelationShip)
            {
                relationship = listRelationShip.FirstOrDefault(r => r.Id.Equals(item.RelationshipId));
                familyMember = new FamilyMemberModel
                {
                    Name = item.Name,
                    Dateb = item.Dateb,
                    RelationshipId = item.RelationshipId,
                    Gender = (int)relationship?.Gender,
                    Job = !string.IsNullOrEmpty(item.Job) ? item.Job : "1",
                    LiveWithChild = item.LiveWithChild
                };

                listFamilyMember.Add(familyMember);
            }

            return PartialView(listFamilyMember);
        }

        public ActionResult LivingWithParentModelView(ObjectBaseModel livingWithParentModel, ObjectBaseModel notLivingWithParentModel, ObjectBaseModel livingWithOtherModel, ObjectBaseModel letterWriteModel)
        {
            ViewBag.notLivingWithParentModel = notLivingWithParentModel;
            ViewBag.livingWithOtherModel = livingWithOtherModel;
            ViewBag.letterWriteModel = letterWriteModel;
            return PartialView(livingWithParentModel);
        }

        public ActionResult ConditionHouseView(ObjectBaseModel houseTypeModel, ObjectBaseModel houseRoofModel, ObjectBaseModel houseWallModel, ObjectBaseModel houseFloorModel)
        {
            ViewBag.houseRoofModel = houseRoofModel;
            ViewBag.houseWallModel = houseWallModel;
            ViewBag.houseFloorModel = houseFloorModel;
            return PartialView(houseTypeModel);
        }
        public ActionResult UseElectricityModelView(ObjectBaseModel useElectricityModel, ObjectBaseModel schoolDistanceModel, ObjectBaseModel clinicDistanceModel, ObjectBaseModel waterSourceDistanceModel, ObjectBaseModel waterSourceUseModel, ObjectBaseModel roadConditionModel)
        {
            ViewBag.useElectricityModel = useElectricityModel;
            ViewBag.schoolDistanceModel = schoolDistanceModel;
            ViewBag.clinicDistanceModel = clinicDistanceModel;
            ViewBag.waterSourceDistanceModel = waterSourceDistanceModel;
            ViewBag.waterSourceUseModel = waterSourceUseModel;
            ViewBag.roadConditionModel = roadConditionModel;
            return PartialView();
        }

        public ActionResult RoadConditionModelView(ObjectBaseModel incomeOtherModel, string familyType, ObjectBaseModel incomeFamilyModel, ObjectBaseModel harvestOutputModel, ObjectBaseModel numberPetModel, string totalIncome)
        {
            ViewBag.incomeOtherModel = incomeOtherModel;
            ViewBag.familyType = familyType;
            ViewBag.incomeFamilyModel = incomeFamilyModel;
            ViewBag.harvestOutputModel = harvestOutputModel;
            ViewBag.numberPetModel = numberPetModel;
            ViewBag.totalIncome = totalIncome;
            return PartialView();
        }

        public ActionResult SpecialInformationView(ObjectBaseModel siblingsJoiningChildFundModel, ObjectBaseModel malformationModel, ObjectBaseModel orphanModel)
        {
            ViewBag.siblingsJoiningChildFundModel = siblingsJoiningChildFundModel;
            ViewBag.malformationModel = malformationModel;
            ViewBag.orphanModel = orphanModel;
            return PartialView();
        }
    }
}