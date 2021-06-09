using ChildProfiles.Model.ChildProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildProfiles.Model;
using ChildProfiles.Model.Model.ChildStory;
using Newtonsoft.Json;
using NTS.Common.Utils;
using System.Text.RegularExpressions;
using ChildProfiles.Model.Entity;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

namespace ChildProfiles.Business.Business
{
    public class StoryBusiness
    {
        private ChildProfileModel _child;
        string str_child_sex1 = string.Empty;
        string str_child_sex2 = string.Empty;
        string str_child_sex3 = string.Empty;
        string str_child_sex4 = string.Empty;
        string str_child_sex5 = string.Empty;
        private string _firstName = string.Empty;
        private string[] numberStr = { "zero", "1", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };
        public StoryBusiness(ChildProfileModel model)
        {
            _child = model;

            if (_child.Gender == 1)
            {
                str_child_sex1 = "he";
                str_child_sex2 = "his";
                str_child_sex3 = "him";
                str_child_sex4 = "boy";
                str_child_sex5 = "He";

            }
            else
            {
                str_child_sex1 = "she";
                str_child_sex2 = "her";
                str_child_sex3 = "her";
                str_child_sex4 = "girl";
                str_child_sex5 = "She";
            }

            _firstName = StringUtils.FormatProperCase(_child.Name.Trim().Split(' ').Last());
        }

        public string GenerateStory(string content)
        {
            string storyContent = content;
            ChildStoryModel story = new ChildStoryModel();

            Random rd = new Random();
            var indexStory = rd.Next(1, 3);
            try
            {
                _firstName = ConvertString.convertToUnSign(_firstName);

                string fullName = string.Empty;
                foreach (var item in _child.Name.Trim().ToLower().Split(' '))
                {
                    if (item != null && item != "") { fullName = fullName + " " + item.UppercaseFirst(); }
                }

                storyContent = storyContent.Replace("[fullName]", fullName);

                story.HealthProblem = this.HealthProblem();
                storyContent = storyContent.Replace("[strName]", _firstName);
                storyContent = storyContent.Replace("[strHealth]", story.HealthProblem);

                story.SpecialSituation = SpecialSituation();
                storyContent = storyContent.Replace("[strSpecialSituation]", story.SpecialSituation);
                story.FamilyMember = FamilyMember();

                storyContent = storyContent.Replace("[strFamilyMember]", story.FamilyMember);

                story.HouseCondition = HouseCondition();
                storyContent = storyContent.Replace("[strHouseCondition]", story.HouseCondition);

                story.WaterSource = WaterSource(indexStory);
                storyContent = storyContent.Replace("[strWaterSource]", story.WaterSource);

                switch (indexStory)
                {
                    case 1:
                        story.ParentJob = ParentJob_Story_1();
                        storyContent = storyContent.Replace("[strParentsJob]", story.ParentJob);

                        story.Characteristics = Characteristics_Story1();
                        storyContent = storyContent.Replace("[strCharacteristic]", story.Characteristics);

                        story.Hobby = Hobby_Story1();
                        storyContent = storyContent.Replace("[strHobby]", story.Hobby);

                        story.Dream = Dream_Story_1_2();
                        storyContent = storyContent.Replace("[strDream]", story.Dream);

                        story.HouseWork = HouseWork_Story_1();
                        storyContent = storyContent.Replace("[strHouseWork]", story.HouseWork);
                        break;
                    case 2:
                        story.ParentJob = ParentJob_Story_2();
                        storyContent = storyContent.Replace("[strParentsJob]", story.ParentJob);

                        story.Characteristics = Characteristics_Story2_3();
                        storyContent = storyContent.Replace("[strCharacteristic]", story.Characteristics);

                        story.Hobby = Hobby_Story_2_3();
                        storyContent = storyContent.Replace("[strHobby]", story.Hobby);

                        story.Dream = Dream_Story_1_2();
                        storyContent = storyContent.Replace("[strDream]", story.Dream);

                        story.HouseWork = HouseWork_Story_2();
                        storyContent = storyContent.Replace("[strHouseWork]", story.HouseWork);
                        break;
                    case 3:
                        story.Characteristics = Characteristics_Story2_3();
                        storyContent = storyContent.Replace("[strCharacteristic]", story.Characteristics);

                        story.Hobby = Hobby_Story_2_3();
                        storyContent = storyContent.Replace("[strHobby]", story.Hobby);

                        story.Dream = Dream_Story_3();
                        storyContent = storyContent.Replace("[strDream]", story.Dream);

                        story.HouseWork = HouseWork_Story_3();
                        storyContent = storyContent.Replace("[strHouseWork]", story.HouseWork);
                        break;
                    default:
                        story.Characteristics = Characteristics_Story1();
                        storyContent = storyContent.Replace("[strCharacteristic]", story.Characteristics);

                        story.Hobby = Hobby_Story1();
                        storyContent = storyContent.Replace("[strHobby]", story.Hobby);

                        story.Dream = Dream_Story_1_2();
                        storyContent = storyContent.Replace("[strDream]", story.Dream);

                        story.HouseWork = HouseWork_Story_1();
                        storyContent = storyContent.Replace("[strHouseWork]", story.HouseWork);
                        break;
                }

                story.Subject = Subject();
                storyContent = storyContent.Replace("[strSubject]", story.Subject);

                storyContent = storyContent.Replace("[str_child_sex1]", str_child_sex1);
                storyContent = storyContent.Replace("[str_child_sex2]", str_child_sex2);
                storyContent = storyContent.Replace("[str_child_sex3]", str_child_sex3);
                storyContent = storyContent.Replace("[str_child_sex4]", str_child_sex4);
                storyContent = storyContent.Replace("[str_child_sex5]", str_child_sex5);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("StoryBusiness.GenerateStory", ex.Message, content);
                throw;
            }
            storyContent = storyContent.Trim();

            // Lặp khoảng 5 lần để loại bỏ các ký tự trống, thừa dấu
            for (int i = 0; i < 5; i++)
            {
                storyContent = storyContent.Replace("\"", "");
                storyContent = storyContent.Replace("  ", " ");
                storyContent = storyContent.Replace(" . .  ", ". ");
                storyContent = storyContent.Replace("..", ".");
                storyContent = storyContent.Replace(". . ", ". ");
                storyContent = storyContent.Replace(" .", ".");
                storyContent = storyContent.Replace(",,", ",");
                storyContent = storyContent.Replace(" ,", ",");
            }

            return storyContent;
        }

        private string HealthProblem()
        {
            string returnValue = string.Empty;
            bool? healthProblem = _child.Handicap;

            if ((healthProblem != null && healthProblem == true) || _child.HealthModel.ListObject.Any(r => r.Id == "04" & r.Check))
            {
                returnValue = _firstName.UppercaseFirst() + " is a child with disability.";
            }

            return returnValue;
        }

        public string SpecialSituation()
        {
            string str_special_situation = string.Empty;
            var situation = _child.NotLivingWithParentModel.ListObject.FirstOrDefault(t => t.Check);
            if (situation == null)
            {
                return String.Empty;
            }
            switch (situation.Id)
            {
                case "01":
                    str_special_situation = string.Format("{0} parents have passed away, which is a great loss to {1}.", str_child_sex2.UppercaseFirst(), str_child_sex3);
                    break;
                case "02":
                    str_special_situation = string.Format("{0} father has passed away, which is a great loss to {1}.", str_child_sex2.UppercaseFirst(), str_child_sex3);
                    break;
                case "03":
                    str_special_situation = string.Format("{0} mother has passed away, which is a great loss to {1}.", str_child_sex2.UppercaseFirst(), str_child_sex3);
                    break;
                case "04":
                    str_special_situation = string.Format("{0} father has left the family, which is a great loss to {1}.", str_child_sex2.UppercaseFirst(), str_child_sex3);
                    break;
                case "05":
                    str_special_situation = string.Format("{0} mother has left the family, which is a great loss to {1}.", str_child_sex2.UppercaseFirst(), str_child_sex3);
                    break;
                case "06":
                    str_special_situation = string.Format("{0} parents have got separated.", str_child_sex2.UppercaseFirst());
                    break;
                case "07":
                    str_special_situation = string.Format("{0} parents have got divorced.", str_child_sex2.UppercaseFirst());
                    break;
                case "08":
                    str_special_situation = string.Format("{0} parents are working for hire far from home. The meager incomes can hardly meet the basic needs of the family.", str_child_sex2.UppercaseFirst());
                    break;
                case "09":
                    str_special_situation = string.Format("{0} father is working for hire far from home. The meager income can hardly meet the basic needs of the family.", str_child_sex2.UppercaseFirst());
                    break;
                case "10":
                    str_special_situation = string.Format("{0} mother is working for hire far from home. The meager income can hardly meet the basic needs of the family.", str_child_sex2.UppercaseFirst());
                    break;
                case "11":
                    // Trường hợp single mom thì bỏ qua
                    // str_special_situation = string.Format("{0} is an illegitimate child.", _child.Name);
                    break;
                default:
                    break;
            }
            return str_special_situation;
        }

        public string FamilyMember()
        {
            bool hasGrandFather, hasGrandMother, hasFather, hasMother, hasStepFather, hasStepMother = false;

            // Sống cùng ông
            hasGrandFather = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0004") : true) && r.LiveWithChild == 1) != null ? true : false;

            // Sống cùng bà
            hasGrandMother = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0005") : true) && r.LiveWithChild == 1) != null ? true : false;

            // Có sống cùng bố không
            hasFather = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0001") : true) && r.LiveWithChild == 1) != null ? true : false;
            hasStepFather = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0015") : true) && r.LiveWithChild == 1) != null ? true : false;

            // Có sống cùng mẹ không
            hasMother = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0007") : true) && r.LiveWithChild == 1) != null ? true : false;
            hasStepMother = _child.ListFamilyMember.FirstOrDefault(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0016") : true) && r.LiveWithChild == 1) != null ? true : false;

            //// Có sống anh em trai
            int brothers = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0010") || r.RelationshipId.Equals("R0009")) : true) && (r.LiveWithChild == 1)).Count();
            int youngerBrother = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0009") : true) && (r.LiveWithChild == 1)).Count();
            int olderBrother = _child.ListFamilyMember.Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0010") : true) && (r.LiveWithChild == 1)).Count();

            //// Có sống cùng chị em gái
            int sisters = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0008") || r.RelationshipId.Equals("R0006")) : true) && (r.LiveWithChild == 1)).Count();
            int youngerSister = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0006") : true) && (r.LiveWithChild == 1)).Count();
            int olderSister = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? r.RelationshipId.Equals("R0008") : true) && (r.LiveWithChild == 1)).Count();

            int others = _child.ListFamilyMember
                .Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0013") || r.RelationshipId.Equals("R0014")) : true) && r.LiveWithChild == 1).Count();

            //// Tạm thời bỏ qua các mối quan hệ này
            //string other_relatives = "other_relatives";

            string family_member = _firstName + ", ";

            // Thông tin về ông bà
            if (hasGrandFather && hasGrandMother)
            {
                family_member = family_member + str_child_sex2 + " grandparents";
            }
            else if (hasGrandFather && !hasGrandMother)
            {
                family_member = family_member + str_child_sex2 + " grandfather";
            }
            else if (!hasGrandFather && hasGrandMother)
            {
                family_member = family_member + str_child_sex2 + " grandmother";
            }
            else
            {
                family_member = family_member + ","; ;
            }

            family_member = family_member + ", ";
            // Thông tin về bố mẹ
            if (hasFather && hasMother)
            {
                family_member = family_member + str_child_sex2 + " parents";
            }
            else if ((hasFather || hasStepFather) && !hasMother)
            {
                if (hasFather)
                {
                    family_member = family_member + str_child_sex2 + " father";
                }
                else
                {
                    family_member = family_member + str_child_sex2 + " step father";
                }

            }
            else if (!hasFather && (hasMother || hasStepMother))
            {
                if (hasMother)
                {
                    family_member = family_member + str_child_sex2 + " mother";
                }
                else
                {
                    family_member = family_member + str_child_sex2 + " step mother";
                }
            }

            //// Có sống cùng chị em gái
            if (brothers > 0 & sisters == 0)
            {
                // Thông tin về số anh, em trai
                if (brothers == 1)
                {
                    string brother = youngerBrother > 0 ? "younger brother" : "older brother";
                    family_member = family_member + ", " + brother;
                }
                else if (brothers > 1)
                {
                    family_member = family_member + ", " + numberStr[brothers] + " brothers";
                }
            }
            else if (brothers == 0 && sisters > 0)
            {
                // Thông tin về số chị, em gái
                if (sisters == 1)
                {
                    string sister = youngerSister > 0 ? "younger sister" : "older sister";
                    family_member = family_member + ", " + sister;
                }
                else if (sisters > 1)
                {
                    family_member = family_member + ", " + numberStr[sisters] + " sisters";
                }
            }
            else if (brothers > 0 & sisters > 0)
            {
                family_member = family_member + ", " + numberStr[sisters + brothers] + " siblings";
            }

            //  Thông tin mối quan hệ khác
            if (others > 0)
            {
                family_member = family_member + ", other relatives";
            }

            family_member = family_member.Replace(",,", "");

            family_member = family_member.Replace(_firstName + ", ", _firstName + " lives with ");
            family_member = ConvertString.RepalaceByAnd(family_member);

            return family_member;
        }

        public string HouseCondition()
        {
            string house_type = _child.HouseTypeModel.ListObject.Where(r => r.Check).Any() ? _child.HouseTypeModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;
            string house_roof = _child.HouseRoofModel.ListObject.Where(r => r.Check).Any() ? _child.HouseRoofModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;
            string house_wall = _child.HouseWallModel.ListObject.Where(r => r.Check).Any() ? _child.HouseWallModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;
            string house_floor = _child.HouseFloorModel.ListObject.Where(r => r.Check).Any() ? _child.HouseFloorModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;

            string house_condition = string.Empty;

            if (!string.IsNullOrEmpty(house_type))
            {
                house_condition = "small " + house_type;
            }
            else
            {
                house_condition = "small house";
            }

            if (!string.IsNullOrEmpty(house_roof) || !string.IsNullOrEmpty(house_wall) ||
                !string.IsNullOrEmpty(house_floor))
            {
                house_condition = house_condition + " with";
            }

            if (!string.IsNullOrEmpty(house_roof))
            {
                house_condition = house_condition + " a " + house_roof + " roof,";
            }
            if (!string.IsNullOrEmpty(house_wall))
            {
                house_condition = house_condition = house_condition + " " + house_wall + " walls";
            }
            if (!string.IsNullOrEmpty(house_floor))
            {
                house_condition = house_condition = house_condition + ", " + house_floor + " floor";
            }

            house_condition = house_condition.Replace("with,", "with");

            house_condition = ConvertString.RepalaceByAnd(house_condition);

            house_condition = house_condition.Replace(", and", " and");

            if (house_condition.EndsWith("and"))
            {
                house_condition = house_condition.Substring(0, house_condition.LastIndexOf("and"));
            }

            return house_condition;
        }

        public string ParentJob_Story_1()
        {
            // Có sống cùng bố không
            bool hasFather = _child.ListFamilyMember.Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0001") || r.RelationshipId.Equals("R0015")) : true) && r.LiveWithChild == 1).Any();

            // Có sống cùng mẹ không
            bool hasMother = _child.ListFamilyMember.Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0007") || r.RelationshipId.Equals("R0016")) : true) && r.LiveWithChild == 1).Any();

            // Ở đây cần phải kiểm tra xem phần thông tin gia đình thì bố mẹ còn sống không
            string parents_job = string.Empty;

            string father_job = string.Empty;
            string mother_job = string.Empty;
            ChildProfileEntities db = new ChildProfileEntities();

            var jobs = db.Jobs.ToList();

            if (hasFather)
            {
                var jc = _child.ListFamilyMember.FirstOrDefault(d => !string.IsNullOrEmpty(d.RelationshipId) ? (d.RelationshipId.Equals("R0001") || d.RelationshipId.Equals("R0015")) : true);
                if (jc != null)
                {
                    father_job = (from r in jobs
                                  where r.Id.Equals(jc.Job)
                                  select r.NameEn).FirstOrDefault();
                }
            }
            if (hasMother)
            {
                var job = _child.ListFamilyMember.FirstOrDefault(d => !string.IsNullOrEmpty(d.RelationshipId) ? (d.RelationshipId.Equals("R0007") || d.RelationshipId.Equals("R0016")) : true);
                if (job != null)
                {
                    mother_job = (from r in jobs
                                  where r.Id.Equals(job.Job)
                                  select r.NameEn).FirstOrDefault();
                }
            }

            string pet = GetPet();
            string plant = GetPlant();
            string mix_plant_get = string.Empty;
            if (!string.IsNullOrEmpty(pet) && !string.IsNullOrEmpty(plant))
            {
                mix_plant_get = pet + " and " + plant;
            }
            else
            {
                mix_plant_get = plant + pet;
            }

            if (!string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
            {
                // Cả bố và mẹ đều làm ruộng
                if (father_job.Equals("farmer") && mother_job.Equals("farmer"))
                {
                    parents_job = str_child_sex2.UppercaseFirst() + " parents are both farmers who earn their living by " + mix_plant_get;
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (father_job.Equals("farmer") && !mother_job.Equals("farmer"))
                {
                    parents_job = str_child_sex2.UppercaseFirst() +
                                  " father is a farmer who earns their living by " + mix_plant_get;
                    if (!string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = parents_job + ". Meanwhile, " + str_child_sex2 + " mother works as " +
                                      ConvertString.maotu(mother_job);
                    }
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && mother_job.Equals("farmer"))
                {
                    parents_job = str_child_sex2.UppercaseFirst() +
                                  " mother is a farmer who earns their living by " + mix_plant_get;
                    if (!string.IsNullOrEmpty(father_job))
                    {
                        parents_job = parents_job + ". Meanwhile, " + str_child_sex2 + " father works as " +
                                      ConvertString.maotu(father_job);
                    }
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && father_job.Equals(mother_job) && !string.IsNullOrEmpty(father_job))
                {
                    parents_job = str_child_sex2.UppercaseFirst() + " parents both work as " + father_job + "s";
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && !mother_job.Equals("farmer") && !father_job.Equals(mother_job))
                {
                    if (!string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " mother works as " + ConvertString.maotu(mother_job) +
                                      " while " + str_child_sex2 + " father is " + ConvertString.maotu(father_job);
                    }
                    else if (!string.IsNullOrEmpty(father_job) && string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " father is " + ConvertString.maotu(father_job);
                    }
                    else if (string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " mother is " + ConvertString.maotu(mother_job);
                    }

                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
            }

            if (string.IsNullOrEmpty(parents_job) && !string.IsNullOrEmpty(mix_plant_get))
            {
                parents_job = "The family rely on " + mix_plant_get;
            }

            return parents_job;
        }

        public string ParentJob_Story_2()
        {
            // Có sống cùng bố không
            bool hasFather = _child.ListFamilyMember.Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0001") || r.RelationshipId.Equals("R0015")) : true) && r.LiveWithChild == 1).Any();

            // Có sống cùng mẹ không
            bool hasMother = _child.ListFamilyMember.Where(r => (!string.IsNullOrEmpty(r.RelationshipId) ? (r.RelationshipId.Equals("R0007") || r.RelationshipId.Equals("R0016")) : true) && r.LiveWithChild == 1).Any();

            // Ở đây cần phải kiểm tra xem phần thông tin gia đình thì bố mẹ còn sống không
            string parents_job = string.Empty;

            string father_job = string.Empty;
            string mother_job = string.Empty;
            ChildProfileEntities db = new ChildProfileEntities();

            var jobs = db.Jobs.ToList();

            if (hasFather)
            {
                var jc = _child.ListFamilyMember.FirstOrDefault(d => !string.IsNullOrEmpty(d.RelationshipId) ? (d.RelationshipId.Equals("R0001") || d.RelationshipId.Equals("R0015")) : true);
                if (jc != null)
                {
                    father_job = (from r in jobs
                                  where r.Id.Equals(jc.Job)
                                  select r.NameEn).FirstOrDefault();
                }
            }
            if (hasMother)
            {
                var job = _child.ListFamilyMember.FirstOrDefault(d => !string.IsNullOrEmpty(d.RelationshipId) ? (d.RelationshipId.Equals("R0007") || d.RelationshipId.Equals("R0016")) : true);
                if (job != null)
                {
                    mother_job = (from r in jobs
                                  where r.Id.Equals(job.Job)
                                  select r.NameEn).FirstOrDefault();
                }
            }

            string pet = GetPet();
            string plant = GetPlant();
            string mix_plant_get = string.Empty;
            if (!string.IsNullOrEmpty(pet) && !string.IsNullOrEmpty(plant))
            {
                mix_plant_get = pet + " and " + plant;
            }
            else
            {
                mix_plant_get = plant + pet;
            }

            // Cả bố và mẹ đều làm ruộng
            if (!string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
            {
                if (father_job.Equals("farmer") && mother_job.Equals("farmer"))
                {
                    parents_job = "Like most families in the community, " + _firstName.UppercaseFirst() + "'s" + " parents are farmers. They rely on " + mix_plant_get;
                    parents_job = parents_job + ", which they work hard to produce in the surrounding fields and hilly areas.  However, an often low yield makes it difficult to provide an income sufficient to meet other basic needs";
                }
                else if (father_job.Equals("farmer") && !mother_job.Equals("farmer"))
                {
                    parents_job = str_child_sex2.UppercaseFirst() +
                                  " father is a farmer who earns their living by " + mix_plant_get;
                    if (!string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = parents_job + ". Meanwhile, " + str_child_sex2 + " mother works as " +
                                      ConvertString.maotu(mother_job);
                    }
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && mother_job.Equals("farmer"))
                {
                    parents_job = str_child_sex2.UppercaseFirst() +
                                  " mother is a farmer who earns their living by " + mix_plant_get;
                    if (!string.IsNullOrEmpty(father_job))
                    {
                        parents_job = parents_job + ". Meanwhile, " + str_child_sex2 + " father works as " +
                                      ConvertString.maotu(father_job);
                    }
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && father_job.Equals(mother_job) && !string.IsNullOrEmpty(father_job))
                {
                    parents_job = str_child_sex2.UppercaseFirst() + " parents both work as " + father_job + "s";
                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
                else if (!father_job.Equals("farmer") && !mother_job.Equals("farmer") && !father_job.Equals(mother_job))
                {
                    if (!string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " mother works as " + ConvertString.maotu(mother_job) +
                                      " while " + str_child_sex2 + " father is " + ConvertString.maotu(father_job);
                    }
                    else if (!string.IsNullOrEmpty(father_job) && string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " father is " + ConvertString.maotu(father_job);
                    }
                    else if (string.IsNullOrEmpty(father_job) && !string.IsNullOrEmpty(mother_job))
                    {
                        parents_job = str_child_sex2.UppercaseFirst() + " mother is " + ConvertString.maotu(mother_job);
                    }

                    parents_job = parents_job + ". Despite their non-stop effort, the family still experience various deprivations in their daily lives such as poor health care, lack of quality education, inadequate living standards, to name a few";
                }
            }

            if (string.IsNullOrEmpty(parents_job) && !string.IsNullOrEmpty(mix_plant_get))
            {
                parents_job = "The family rely on " + mix_plant_get;
            }

            return parents_job;
        }

        private string GetPet()
        {
            bool chicken = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool duck = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool pig = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool buffalo = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool cow = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool goat = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "06").Any();

            List<string> pets = new List<string>();
            string strPet = string.Empty;

            if (chicken)
            {
                pets.Add("chickens");
            }
            if (duck)
            {
                pets.Add("ducks");
            }
            if (pig)
            {
                pets.Add("pigs");
            }
            if (buffalo)
            {
                pets.Add("buffaloes");
            }
            if (cow)
            {
                pets.Add("cows");
            }
            if (goat)
            {
                pets.Add("goats");
            }
            if (pets.Count > 0)
            {
                for (int i = 0; i < pets.Count - 1; i++)
                {
                    strPet = strPet + pets[i] + ", ";
                }
                strPet = strPet + pets.Last();
            }

            strPet = ConvertString.RepalaceByAnd(strPet);

            if (!string.IsNullOrEmpty(strPet))
            {
                strPet = "raising " + strPet;
            }

            return strPet;
        }

        private string GetPlant()
        {
            bool rice = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool corn = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool potatoes = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool cassava = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool sugarcane = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool fruit_trees = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool grass_jelly_plants = _child.NumberPetModel.ListObject.Where(r => r.Check && r.Id == "07").Any();

            List<string> plants = new List<string>();
            string strPlants = string.Empty;

            if (rice)
            {
                plants.Add("rice");
            }
            if (corn)
            {
                plants.Add("corn");
            }
            if (potatoes)
            {
                plants.Add("potatoes");
            }
            if (cassava)
            {
                plants.Add("cassava");
            }
            if (sugarcane)
            {
                plants.Add("sugarcane");
            }
            if (fruit_trees)
            {
                plants.Add("fruits");
            }
            if (grass_jelly_plants)
            {
                plants.Add("grass jelly plants");
            }
            if (plants.Count > 0)
            {
                for (int i = 0; i < plants.Count - 1; i++)
                {
                    strPlants = strPlants + plants[i] + ", ";
                }
                strPlants = strPlants + plants.Last();
            }

            strPlants = ConvertString.RepalaceByAnd(strPlants);

            if (!string.IsNullOrEmpty(strPlants))
            {
                strPlants = "growing " + strPlants;
            }
            else
            {
                strPlants = "growing rice and potatoes ";
            }

            return strPlants;
        }

        public string WaterSource(int story)
        {
            string distance_to_water_source = _child.WaterSourceDistanceModel.ListObject.Where(r => r.Check).Any() ? _child.WaterSourceDistanceModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;
            string watersource = _child.WaterSourceUseModel.ListObject.Where(r => r.Check).Any() ? _child.WaterSourceUseModel.ListObject.FirstOrDefault(r => r.Check).Id : string.Empty;
            string watersourceName = _child.WaterSourceUseModel.ListObject.Where(r => r.Check).Any() ? _child.WaterSourceUseModel.ListObject.FirstOrDefault(r => r.Check).NameEN : string.Empty;
            string str_water = string.Empty;

            if (!string.IsNullOrEmpty(watersource))
            {
                str_water = "Drinking, washing and cooking water is collected from the " + distance_to_water_source +
                            " " + watersourceName;
                if (watersource.Equals("02") || watersource.Equals("03"))
                {
                    if (story == 1)
                    {
                        str_water = str_water + " which is unhealthy from pollution while appropriate healthcare is hardly accessible in the community";
                    }
                    else
                    {
                        str_water = str_water + " which is often polluted with garbage and dust. ";
                    }

                }
            }

            str_water = str_water.TrimStart();
            if (!str_water.EndsWith("."))
            {
                str_water = str_water + ".";
            }


            return str_water;
        }

        public string Characteristics_Story1()
        {
            bool friendly = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool active = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool jolly = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool outgoing = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool shy = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool humorous = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool calm = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool sincere = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool caring = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool quiet = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> characteristices = new List<string>();
            string strcharacter = String.Empty;

            if (!friendly && !active && !jolly && !outgoing && !shy && !humorous && !calm && !sincere && !caring && !quiet)
            {
                strcharacter = "Though life is hard for " + ConvertString.UppercaseFirst(_firstName) + ", " + str_child_sex1 + " is an amiable and obedient child.";
            }
            else
            {
                if (friendly)
                {
                    characteristices.Add("friendly");
                }
                if (active)
                {
                    characteristices.Add("active");
                }
                if (jolly)
                {
                    characteristices.Add("jolly");
                }
                if (outgoing)
                {
                    characteristices.Add("outgoing");
                }
                if (shy)
                {
                    characteristices.Add("shy");
                }
                if (humorous)
                {
                    characteristices.Add("humorous");
                }
                if (calm)
                {
                    characteristices.Add("calm");
                }
                if (sincere)
                {
                    characteristices.Add("sincere");
                }
                if (caring)
                {
                    characteristices.Add("caring");
                }
                if (quiet)
                {
                    characteristices.Add("quiet");
                }
                if (characteristices.Count > 0)
                {
                    for (int i = 0; i < characteristices.Count - 1; i++)
                    {
                        strcharacter = strcharacter + characteristices[i] + ", ";
                    }
                    strcharacter = strcharacter + characteristices.Last();
                }

                strcharacter = ConvertString.RepalaceByAnd(strcharacter);

                if (strcharacter.StartsWith("a") || strcharacter.StartsWith("e") || strcharacter.StartsWith("i") || strcharacter.StartsWith("o") || strcharacter.StartsWith("u"))
                {
                    strcharacter = "an " + strcharacter;
                }
                else
                {
                    strcharacter = "a " + strcharacter;
                }

                if (shy || quiet)
                {
                    strcharacter = ConvertString.UppercaseFirst(_firstName) + " appears to be " + strcharacter + " child.";
                }
                else
                {
                    strcharacter = "Though life is hard for " + ConvertString.UppercaseFirst(_firstName) + ", " + str_child_sex1 + " appears to be " + strcharacter + " child.";
                }
            }

            return strcharacter;
        }

        public string Characteristics_Story2_3()
        {
            bool friendly = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool active = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool jolly = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool outgoing = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool shy = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool humorous = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool calm = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool sincere = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool caring = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool quiet = _child.PersonalityModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> characteristices = new List<string>();
            string strcharacter = String.Empty;

            if (!friendly && !active && !jolly && !outgoing && !shy && !humorous && !calm && !sincere && !caring && !quiet)
            {
                strcharacter = "Though life is hard for " + ConvertString.UppercaseFirst(_firstName) + ", " + str_child_sex1 + " is an amiable and obedient child.";
            }
            else
            {
                if (friendly)
                {
                    characteristices.Add("friendly");
                }
                if (active)
                {
                    characteristices.Add("active");
                }
                if (jolly)
                {
                    characteristices.Add("jolly");
                }
                if (outgoing)
                {
                    characteristices.Add("outgoing");
                }
                if (shy)
                {
                    characteristices.Add("shy");
                }
                if (humorous)
                {
                    characteristices.Add("humorous");
                }
                if (calm)
                {
                    characteristices.Add("calm");
                }
                if (sincere)
                {
                    characteristices.Add("sincere");
                }
                if (caring)
                {
                    characteristices.Add("caring");
                }
                if (quiet)
                {
                    characteristices.Add("quiet");
                }

                if (characteristices.Count > 0)
                {
                    for (int i = 0; i < characteristices.Count - 1; i++)
                    {
                        strcharacter = strcharacter + characteristices[i] + ", ";
                    }
                    strcharacter = strcharacter + characteristices.Last();
                }

                strcharacter = ConvertString.RepalaceByAnd(strcharacter);

                if (strcharacter.StartsWith("a") || strcharacter.StartsWith("e") || strcharacter.StartsWith("i") || strcharacter.StartsWith("o") || strcharacter.StartsWith("u"))
                {
                    strcharacter = "an " + strcharacter;
                }
                else
                {
                    strcharacter = "a " + strcharacter;
                }

                if (shy || quiet)
                {
                    strcharacter = ConvertString.UppercaseFirst(_firstName) + " appears to be " + strcharacter + " child.";
                }
                else
                {
                    strcharacter = "Though life is hard for " + ConvertString.UppercaseFirst(_firstName) + ", " + str_child_sex1 + " appears to be " + strcharacter + " child.";
                }
            }

            return strcharacter;
        }

        public string Hobby_Story1()
        {
            bool playing_soccer = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool playing_badminton = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool skipping_rope = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool swimming = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool singing = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool dancing = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool reading_books = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool talk_with_beloved_people = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool playing_with_toys = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool play_kneading_dough = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> hobbies = new List<string>();
            string strhobby = string.Empty;

            if (playing_soccer)
            {
                hobbies.Add("playing soccer");
            }
            if (playing_badminton)
            {
                if (playing_soccer)
                {
                    hobbies.Add("badminton");
                }
                else
                {
                    hobbies.Add("playing badminton");
                }
            }
            if (skipping_rope)
            {
                hobbies.Add("skipping rope");
            }
            if (swimming)
            {
                hobbies.Add("swimming");
            }
            if (singing)
            {
                hobbies.Add("singing");
            }
            if (dancing)
            {
                hobbies.Add("dancing");
            }
            if (reading_books)
            {
                hobbies.Add("reading books");
            }
            if (talk_with_beloved_people)
            {
                hobbies.Add("talking with beloved people");
            }
            if (playing_with_toys)
            {
                hobbies.Add("playing with toys");

            }
            if (play_kneading_dough)
            {
                hobbies.Add("playing with clay");
            }

            if (hobbies.Count > 0)
            {
                for (int i = 0; i < hobbies.Count - 1; i++)
                {
                    strhobby = strhobby + hobbies[i] + ", ";
                }
                strhobby = strhobby + hobbies.Last();
            }

            strhobby = ConvertString.RepalaceByAnd(strhobby);
            if (!string.IsNullOrEmpty(strhobby))
            {
                strhobby = _firstName + " likes " + strhobby;
            }

            return strhobby;
        }

        public string Hobby_Story_2_3()
        {
            bool playing_soccer = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool playing_badminton = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool skipping_rope = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool swimming = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool singing = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool dancing = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool reading_books = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool talk_with_beloved_people = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool playing_with_toys = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool play_kneading_dough = _child.HobbyModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> hobbies = new List<string>();
            string strhobby = string.Empty;

            if (playing_soccer)
            {
                hobbies.Add("playing soccer");
            }
            if (playing_badminton)
            {
                if (playing_soccer)
                {
                    hobbies.Add("badminton");
                }
                else
                {
                    hobbies.Add("playing badminton");
                }
            }
            if (skipping_rope)
            {
                hobbies.Add("skipping rope");
            }
            if (swimming)
            {
                hobbies.Add("swimming");
            }
            if (singing)
            {
                hobbies.Add("singing");
            }
            if (dancing)
            {
                hobbies.Add("dancing");
            }
            if (reading_books)
            {
                hobbies.Add("reading books");
            }
            if (talk_with_beloved_people)
            {
                hobbies.Add("talking with beloved people");
            }
            if (playing_with_toys)
            {
                hobbies.Add("playing with toys");

            }
            if (play_kneading_dough)
            {
                hobbies.Add("playing with clay");
            }

            if (hobbies.Count > 0)
            {
                for (int i = 0; i < hobbies.Count - 1; i++)
                {
                    strhobby = strhobby + hobbies[i] + ", ";
                }
                strhobby = strhobby + hobbies.Last();
            }

            strhobby = ConvertString.RepalaceByAnd(strhobby);

            if (!string.IsNullOrEmpty(strhobby))
            {
                strhobby = _firstName + " likes " + strhobby;
            }

            return strhobby;
        }

        public string Dream_Story_1_2()
        {
            bool doctor = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool army_officer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool police_officer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool singer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool artist = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool good_farmer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool teacher = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool driver = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool scientist = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool engineer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> dreames = new List<string>();

            string strdream = string.Empty;

            if (doctor)
            {
                dreames.Add(" or a doctor");
            }
            if (army_officer)
            {
                dreames.Add(" or an army officer");
            }
            if (police_officer)
            {
                dreames.Add(" or a police officer");
            }
            if (singer)
            {
                dreames.Add(" or a singer");
            }
            if (artist)
            {
                dreames.Add(" or an artist");
            }
            if (good_farmer)
            {
                dreames.Add(" or a good farmer");
            }
            if (teacher)
            {
                dreames.Add(" or a teacher");
            }
            if (driver)
            {
                dreames.Add(" or a driver");
            }
            if (scientist)
            {
                dreames.Add(" or a scientist");
            }
            if (engineer)
            {
                dreames.Add(" or an engineer");
            }

            for (int i = 0; i < dreames.Count; i++)
            {
                strdream = strdream + dreames[i];
            }

            if (!string.IsNullOrEmpty(strdream))
            {
                strdream = strdream.Substring(3, strdream.Length - 3);
                strdream = "This lovely " + str_child_sex4 + " wishes to be" + strdream + " one day.";
            }

            return strdream;
        }

        public string Dream_Story_3()
        {
            bool doctor = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool army_officer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool police_officer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool singer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool artist = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool good_farmer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool teacher = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool driver = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool scientist = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool engineer = _child.DreamModel.ListObject.Where(r => r.Check && r.Id == "10").Any();

            List<string> dreames = new List<string>();

            string strdream = string.Empty;

            if (doctor)
            {
                dreames.Add(" or a doctor");
            }
            if (army_officer)
            {
                dreames.Add(" or an army officer");
            }
            if (police_officer)
            {
                dreames.Add(" or a police officer");
            }
            if (singer)
            {
                dreames.Add(" or a singer");
            }
            if (artist)
            {
                dreames.Add(" or an artist");
            }
            if (good_farmer)
            {
                dreames.Add(" or a good farmer");
            }
            if (teacher)
            {
                dreames.Add(" or a teacher");
            }
            if (driver)
            {
                dreames.Add(" or a driver");
            }
            if (scientist)
            {
                dreames.Add(" or a scientist");
            }
            if (engineer)
            {
                dreames.Add(" or an engineer");
            }

            for (int i = 0; i < dreames.Count; i++)
            {
                strdream = strdream + dreames[i];
            }

            if (!string.IsNullOrEmpty(strdream))
            {
                strdream = strdream.Substring(3, strdream.Length - 3);
                strdream = _firstName + " dreams of becoming " + strdream + " one day.";
            }

            return strdream;
        }

        public string Subject()
        {
            bool maths = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool natural_and_social_sciences = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool vietnamese = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool dictation = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool handicraft = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool music = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool moral_education = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool art = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool biology = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool physics = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "10").Any();
            bool geography = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "11").Any();
            bool history = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "12").Any();
            bool english = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "13").Any();
            bool informatics = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "14").Any();
            bool chemistry = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "15").Any();
            bool literature = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "16").Any();
            bool physical_education = _child.FavouriteSubjectModel.ListObject.Where(r => r.Check && r.Id == "17").Any();

            List<string> subjects = new List<string>();

            string strsubject = string.Empty;

            if (maths)
            {
                subjects.Add("Maths");
            }
            if (natural_and_social_sciences)
            {
                subjects.Add("Natural and Social Sciences");
            }
            if (vietnamese)
            {
                subjects.Add("Vietnamese");
            }
            if (dictation)
            {
                subjects.Add("Dictation");
            }
            if (handicraft)
            {
                subjects.Add("Handicraft");
            }
            if (music)
            {
                subjects.Add("Music");
            }
            if (moral_education)
            {
                subjects.Add("Moral Education");
            }
            if (art)
            {
                subjects.Add("Art");
            }
            if (biology)
            {
                subjects.Add("Biology");
            }
            if (physics)
            {
                subjects.Add("Physics");
            }
            if (geography)
            {
                subjects.Add("Geography");
            }
            if (history)
            {
                subjects.Add("History");
            }
            if (english)
            {
                subjects.Add("English");
            }
            if (informatics)
            {
                subjects.Add("Informatics");
            }
            if (chemistry)
            {
                subjects.Add("Chemistry");
            }
            if (literature)
            {
                subjects.Add("Literature");
            }
            if (physical_education)
            {
                subjects.Add("Physical Education");
            }

            if (subjects.Count > 0)
            {
                for (int i = 0; i < subjects.Count - 1; i++)
                {
                    strsubject = strsubject + subjects[i] + ", ";
                }
                strsubject = strsubject + subjects.Last();
            }

            strsubject = ConvertString.RepalaceByAnd(strsubject);

            if (subjects.Count == 1)
            {
                strsubject = str_child_sex2.UppercaseFirst() + " favorite subject is " + strsubject;
            }
            else if (subjects.Count > 1)
            {
                strsubject = str_child_sex2.UppercaseFirst() + " favorite subjects include " + strsubject;
            }

            return strsubject;
        }

        public string HouseWork_Story_1()
        {
            bool do_housework = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool look_after = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool feed_chicken = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool tend_buffalo = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool do_some_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool collect_fire_wood = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool fetch_water = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool do_farming_work = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool do_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool clear_weeds = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "10").Any();
            bool too_young_to_do = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "11").Any();
            bool cannot_do_due_to_disability = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "12").Any();

            List<string> dohomeworks = new List<string>();

            string strhousework = string.Empty;

            if (do_housework)
            {
                dohomeworks.Add("do housework");
            }
            if (look_after)
            {
                dohomeworks.Add("look after the younger sibling");
            }
            if (feed_chicken)
            {
                dohomeworks.Add("feed the poultries");
            }
            if (tend_buffalo)
            {
                dohomeworks.Add("tend buffaloes");
            }
            if (do_some_chores)
            {
                dohomeworks.Add("harvest the crops");
            }
            if (collect_fire_wood)
            {
                dohomeworks.Add("collect fire wood");
            }
            if (fetch_water)
            {
                dohomeworks.Add("fetch water");
            }
            if (do_farming_work)
            {
                dohomeworks.Add("do farming work");
            }
            if (do_chores)
            {
                dohomeworks.Add("do chores");
            }
            if (clear_weeds)
            {
                dohomeworks.Add("clear weeds");
            }

            if (dohomeworks.Count > 0)
            {
                for (int i = 0; i < dohomeworks.Count - 1; i++)
                {
                    strhousework = strhousework + dohomeworks[i] + ", ";
                }
                strhousework = strhousework + dohomeworks.Last();
            }

            strhousework = ConvertString.RepalaceByAnd(strhousework);

            if (!string.IsNullOrEmpty(strhousework))
            {
                strhousework = str_child_sex1.UppercaseFirst() + " often helps " + str_child_sex2 +
                               " family " + strhousework + " besides school time.";
            }

            return strhousework;
        }

        public string HouseWork_Story_2()
        {
            bool do_housework = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool look_after = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool feed_chicken = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool tend_buffalo = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool do_some_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool collect_fire_wood = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool fetch_water = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool do_farming_work = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool do_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool clear_weeds = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "10").Any();
            bool too_young_to_do = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "11").Any();
            bool cannot_do_due_to_disability = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "12").Any();

            List<string> dohomeworks = new List<string>();

            string strhousework = string.Empty;

            if (do_housework)
            {
                dohomeworks.Add("do housework");
            }
            if (look_after)
            {
                dohomeworks.Add("look after the younger sibling");
            }
            if (feed_chicken)
            {
                dohomeworks.Add("feed the poultries");
            }
            if (tend_buffalo)
            {
                dohomeworks.Add("tend buffaloes");
            }
            if (do_some_chores)
            {
                dohomeworks.Add("harvest the crops");
            }
            if (collect_fire_wood)
            {
                dohomeworks.Add("collect fire wood");
            }
            if (fetch_water)
            {
                dohomeworks.Add("fetch water");
            }
            if (do_farming_work)
            {
                dohomeworks.Add("do farming work");
            }
            if (do_chores)
            {
                dohomeworks.Add("do chores");
            }
            if (clear_weeds)
            {
                dohomeworks.Add("clear weeds");
            }

            if (dohomeworks.Count > 0)
            {
                for (int i = 0; i < dohomeworks.Count - 1; i++)
                {
                    strhousework = strhousework + dohomeworks[i] + ", ";
                }
                strhousework = strhousework + dohomeworks.Last();
            }

            strhousework = ConvertString.RepalaceByAnd(strhousework);

            if (!string.IsNullOrEmpty(strhousework))
            {
                strhousework = "After school, " + str_child_sex1 + " often helps " + str_child_sex2 + " family " + strhousework + ".";
            }

            return strhousework;
        }

        public string HouseWork_Story_3()
        {
            bool do_housework = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "01").Any();
            bool look_after = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "02").Any();
            bool feed_chicken = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "03").Any();
            bool tend_buffalo = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "04").Any();
            bool do_some_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "05").Any();
            bool collect_fire_wood = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "06").Any();
            bool fetch_water = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "07").Any();
            bool do_farming_work = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "08").Any();
            bool do_chores = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "09").Any();
            bool clear_weeds = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "10").Any();
            bool too_young_to_do = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "11").Any();
            bool cannot_do_due_to_disability = _child.HouseworkModel.ListObject.Where(r => r.Check && r.Id == "12").Any();

            List<string> dohomeworks = new List<string>();

            string strhousework = string.Empty;

            if (do_housework)
            {
                dohomeworks.Add("do housework");
            }
            if (look_after)
            {
                dohomeworks.Add("look after");
            }
            if (feed_chicken)
            {
                dohomeworks.Add("feed chicken");
            }
            if (tend_buffalo)
            {
                dohomeworks.Add("tend buffalo");
            }
            if (do_some_chores)
            {
                dohomeworks.Add("do some chores");
            }
            if (collect_fire_wood)
            {
                dohomeworks.Add("collect fire wood");
            }
            if (fetch_water)
            {
                dohomeworks.Add("fetch water");
            }
            if (do_farming_work)
            {
                dohomeworks.Add("do farming work");
            }
            if (do_chores)
            {
                dohomeworks.Add("do chores");
            }
            if (clear_weeds)
            {
                dohomeworks.Add("clear weeds");
            }

            if (dohomeworks.Count > 0)
            {
                for (int i = 0; i < dohomeworks.Count - 1; i++)
                {
                    strhousework = strhousework + dohomeworks[i] + ", ";
                }
                strhousework = strhousework + dohomeworks.Last();
            }

            strhousework = ConvertString.RepalaceByAnd(strhousework);

            if (!string.IsNullOrEmpty(strhousework))
            {
                strhousework = "Besides school time, " + str_child_sex1 + " often helps " + str_child_sex2 + " family " + strhousework + ".";
            }

            return strhousework;
        }
    }


    public static class ConvertString
    {
        public static string RepalaceByAnd(string str)
        {
            if (!str.Contains(","))
            {
                return str;
            }
            string left = str.Substring(0, str.LastIndexOf(","));
            string right = str.Substring(str.LastIndexOf(",") + 1, str.Length - str.LastIndexOf(",") - 1);
            if (string.IsNullOrWhiteSpace(right))
            {
                return left;
            }
            else
            {
                return left + " and" + right;
            }
        }

        public static string UppercaseFirst(this string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("null");
            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        public static string maotu(string word)
        {
            string first = word.First().ToString().ToUpper();
            if (first == "A" || first == "E" || first == "O" || first == "I" || first == "U")
            {
                return "an " + word;
            }
            else return "a " + word;
        }

        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
    }
}
