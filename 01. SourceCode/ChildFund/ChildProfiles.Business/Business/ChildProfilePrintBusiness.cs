using ChildProfiles.Model.ChildProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Syncfusion.DocIO.DLS;
using ChildProfiles.Model.Entity;
using NTSFramework.Common.Utils;
using Newtonsoft.Json;
using ChildProfiles.Model;
using NTS.Common.Utils;

namespace ChildProfiles.Business.Business
{
    public class ChildProfilePrintBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public string ExportChildProfile(ChildProfileExport searchModel)
        {
            string pathreturn = "";

            ChildProfileModel model = new ChildProfileModel();
            try
            {
                foreach (var Id in searchModel.ListCheck)
                {
                    string templatePath = string.Empty;
                    templatePath = HttpContext.Current.Server.MapPath("~/Template/ChildProfileTemplate.doc");

                    WordDocument wordDocument = new WordDocument(templatePath);
                    wordDocument.Open(templatePath);
                    TextSelection textSelections;

                    //today date
                    textSelections = wordDocument.Find("<TodayDate>", false, true);
                    DateTime date = DateTime.Now;
                    string todayDate = date.Day.ToString() + "-" + date.Month.ToString() + "-" + date.Year.ToString();
                    if (textSelections != null)
                    {
                        WTextRange textRange = textSelections.GetAsOneRange();
                        textRange.Text = todayDate;
                    }

                    var data = (from a in db.Users.AsNoTracking()
                                join b in db.ChildProfiles.AsNoTracking() on a.Id equals b.AreaApproverId
                                join c in db.Religions.AsNoTracking() on b.ReligionId equals c.Id
                                join d in db.Provinces.AsNoTracking() on b.ProvinceId equals d.Id
                                join e in db.Districts.AsNoTracking() on b.DistrictId equals e.Id
                                join f in db.Wards.AsNoTracking() on b.WardId equals f.Id
                                join g in db.Ethnics.AsNoTracking() on b.EthnicId equals g.Id
                                where b.Id.Equals(Id)
                                select new
                                {
                                    CreateName = a.Name,
                                    ProgramCode = b.ProgramCode,
                                    ChildCode = b.ChildCode,
                                    Religion = c.Name,
                                    Province = d.Name,
                                    District = e.Name,
                                    Ward = f.Name,
                                    SchoolId = b.SchoolId,
                                    Nation = g.Name,
                                    Name = b.Name,
                                    NickName = b.NickName,
                                    Gender = b.Gender,
                                    Birthday = b.DateOfBirth,
                                    FamilyDetail = b.FamilyMember,
                                    Subject = b.FavouriteSubject,
                                    Capacity = b.LearningCapacity,
                                    HouseWork = b.Housework,
                                    Health = b.Health,
                                    Personality = b.Personality,
                                    HouseCondition = b.Housework,
                                    ConditionOther = b.IncomeOther

                                }).FirstOrDefault();

                    //createBy
                    textSelections = wordDocument.Find("<CreateBy>", false, true);
                    string Supervisor = data.CreateName;
                    if (textSelections != null)
                    {
                        WTextRange textRange = textSelections.GetAsOneRange();
                        textRange.Text = Supervisor;
                    }

                    //programCode
                    wordDocument.NTSReplaceFirst("<ProgramCode>", data.ProgramCode);

                    //childCode
                    wordDocument.NTSReplaceFirst("<ChildCode>", data.ChildCode);

                    //religion
                    wordDocument.NTSReplaceFirst("<Religion>", data.Religion);

                    //province
                    wordDocument.NTSReplaceFirst("<Province>", data.Province);

                    //district
                    wordDocument.NTSReplaceFirst("<District>", data.District);

                    //ward
                    wordDocument.NTSReplaceFirst("<Ward>", data.Ward);

                    //index
                    wordDocument.NTSReplaceFirst("<Index>", data.SchoolId);

                    //nation
                    wordDocument.NTSReplaceFirst("<Nation>", data.Nation);

                    //name
                    wordDocument.NTSReplaceFirst("<Name>", data.Name);

                    //nickname
                    wordDocument.NTSReplaceFirst("<NickName>", data.NickName);

                    //gender

                    string Gender = data.Gender.ToString();
                    if (textSelections != null)
                    {
                        WTextRange textRange = textSelections.GetAsOneRange();
                        if (Gender.Equals("1"))
                        {
                            wordDocument.NTSReplaceFirst("<Male>", "Nam x");
                            wordDocument.NTSReplaceFirst("<Female>", "Nữ");
                        }
                        else
                        {
                            wordDocument.NTSReplaceFirst("<Male>", "Nam");
                            wordDocument.NTSReplaceFirst("<Female>", "Nữ x");
                        }
                    }

                    //birthday
                    wordDocument.NTSReplaceFirst("<Birthday>", "Ngày " + data.Birthday.Day.ToString() + " tháng " + data.Birthday.Month.ToString() + " năm " + data.Birthday.Year.ToString());

                    //subjects
                    WTable SBtable = wordDocument.GetTableByFindText("<Subject>");

                    if (SBtable == null)
                    {
                        // TODO message error;
                    }

                    wordDocument.NTSReplaceFirst("<Subject>", string.Empty);
                    wordDocument.NTSReplaceFirst("<SubjectIndex1>", string.Empty);
                    wordDocument.NTSReplaceFirst("<SubjectIndex2>", string.Empty);

                    WTableRow SBtemplateRow = SBtable.Rows[0].Clone();
                    WTableRow SBrow;

                    //model.SubjectsModel = JsonConvert.DeserializeObject<QuestionModel>(data.Subject);

                    //wordDocument.NTSReplaceFirst("<OtherAnswerSubject>", model.SubjectsModel.OtherAnswer);
                    //wordDocument.NTSReplaceFirst("<MoreAnswerSubject>", model.SubjectsModel.MoreAnswer);

                    //SBrow = SBtable.Rows[0];
                    ////answers
                    //SBrow.Cells[0].Paragraphs[0].Text = model.SubjectsModel.Id + " " + model.SubjectsModel.Content;

                    //for (int j = 0; j < model.SubjectsModel.Answers.Count(); j++)
                    //{
                    //    if (j <= 7)
                    //    {
                    //        if (model.SubjectsModel.Answers[j].Check == true)
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.SubjectsModel.Answers[j].Index + " x";
                    //            SBtable.Rows[1].Cells[0].Paragraphs.Add(newParagraph);

                    //        } else
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.SubjectsModel.Answers[j].Index;
                    //            SBtable.Rows[1].Cells[0].Paragraphs.Add(newParagraph);
                    //        }
                    //    } else
                    //    {
                    //        if (model.SubjectsModel.Answers[j].Check == true)
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.SubjectsModel.Answers[j].Index + " x";
                    //            SBtable.Rows[1].Cells[2].Paragraphs.Add(newParagraph);
                    //        }
                    //        else
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.SubjectsModel.Answers[j].Index;
                    //            SBtable.Rows[1].Cells[2].Paragraphs.Add(newParagraph);
                    //        }
                    //    }

                    //}

                    ////capacity
                    //model.LeaningCapacityModel = JsonConvert.DeserializeObject<QuestionModel>(data.Capacity);

                    //wordDocument.NTSReplaceFirst("<OtherAnswerCapacity>", model.LeaningCapacityModel.OtherAnswer);
                    //wordDocument.NTSReplaceFirst("<CapacityIndex>", string.Empty);

                    ////answers
                    //SBrow.Cells[1].Paragraphs[0].Text = model.LeaningCapacityModel.Id + " " + model.LeaningCapacityModel.Content;

                    //for (int j = 0; j < model.LeaningCapacityModel.Answers.Count(); j++)
                    //{
                    //    if (model.SubjectsModel.Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.LeaningCapacityModel.Answers[j].Index + " x";
                    //        SBtable.Rows[1].Cells[4].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.LeaningCapacityModel.Answers[j].Index;
                    //        SBtable.Rows[1].Cells[4].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////housework
                    //WTable HWtable = wordDocument.GetTableByFindText("<Housework>");

                    //if (HWtable == null)
                    //{
                    //    // TODO message error;
                    //}

                    //wordDocument.NTSReplaceFirst("<Housework>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HouseworkIndex1>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HouseworkIndex2>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HealthIndex>", string.Empty);

                    //WTableRow HWtemplateRow = SBtable.Rows[0].Clone();
                    //WTableRow HWrow;

                    //model.HouseWorkModel = JsonConvert.DeserializeObject<QuestionModel>(data.HouseWork);

                    //HWrow = HWtable.Rows[0];
                    ////answers
                    //HWrow.Cells[0].Paragraphs[0].Text = model.HouseWorkModel.Id + " " + model.HouseWorkModel.Content;

                    //for (int j = 0; j < model.HouseWorkModel.Answers.Count(); j++)
                    //{
                    //    if (j <= 4)
                    //    {
                    //        if (model.HouseWorkModel.Answers[j].Check == true)
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.HouseWorkModel.Answers[j].Index + " x";
                    //            SBtable.Rows[1].Cells[0].Paragraphs.Add(newParagraph);
                    //        }
                    //        else
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.HouseWorkModel.Answers[j].Index;
                    //            SBtable.Rows[1].Cells[0].Paragraphs.Add(newParagraph);
                    //        }
                    //    } else
                    //    {
                    //        if (model.HouseWorkModel.Answers[j].Check == true)
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.HouseWorkModel.Answers[j].Index + " x";
                    //            SBtable.Rows[1].Cells[2].Paragraphs.Add(newParagraph);
                    //        }
                    //        else
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = model.HouseWorkModel.Answers[j].Index;
                    //            SBtable.Rows[1].Cells[2].Paragraphs.Add(newParagraph);
                    //        }
                    //    }
                    //}
                    //wordDocument.NTSReplaceFirst("<OtherAnswerHousework>", model.HouseWorkModel.OtherAnswer);

                    ////health
                    //model.HealthModel = JsonConvert.DeserializeObject<QuestionModel>(data.Health);

                    //HWrow.Cells[1].Paragraphs[0].Text = model.HealthModel.Id + " " + model.HealthModel.Content;

                    //for (int j = 0; j < model.HealthModel.Answers.Count(); j++)
                    //{
                    //    if (model.HealthModel.Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.HealthModel.Answers[j].Index + " x";
                    //        SBtable.Rows[1].Cells[4].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.HealthModel.Answers[j].Index;
                    //        SBtable.Rows[1].Cells[4].Paragraphs.Add(newParagraph);
                    //    }
                    //}
                    //wordDocument.NTSReplaceFirst("<OtherAnswerHealth>", model.HealthModel.OtherAnswer);


                    ////personality
                    //WTable Ptable = wordDocument.GetTableByFindText("<PersonalityIndex1>");

                    //if (Ptable == null)
                    //{
                    //    // TODO message error;
                    //}

                    //wordDocument.NTSReplaceFirst("<PersonalityIndex1>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<PersonalityIndex2>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<PersonalityIndex3>", string.Empty);

                    //WTableRow PtemplateRow = SBtable.Rows[0].Clone();
                    //WTableRow Prow;
                    //int i = 0;
                    //int n = 0;

                    //model.PersonalityModel = JsonConvert.DeserializeObject<List<QuestionModel>>(data.Personality);

                    //Prow = Ptable.Rows[0];
                    ////answers
                    //foreach (var item in model.PersonalityModel)
                    //{
                    //    //Ptable.Rows[0].Cells[i].Paragraphs[0].Text = item.Id + " " + item.Content;

                    //    for (int j = 0; j < item.Answers.Count(); j++)
                    //    {
                    //        if (item.Answers[j].Check == true)
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = item.Answers[j].Index + " x";
                    //            Ptable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //        }
                    //        else
                    //        {
                    //            WParagraph newParagraph = new WParagraph(wordDocument);
                    //            newParagraph.Text = item.Answers[j].Index;
                    //            Ptable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //        }
                    //    }
                    //    WParagraph otherAnswer = new WParagraph(wordDocument);
                    //    otherAnswer.Text = item.OtherAnswer;
                    //    Ptable.Rows[2].Cells[i].Paragraphs.Add(otherAnswer);

                    //    i++;
                    //    n = n + 2;
                    //}

                    ////familyData
                    //WTable table = wordDocument.GetTableByFindText("<FamilyData>");

                    //if (table == null)
                    //{
                    //    // TODO message error;

                    //}

                    //wordDocument.NTSReplaceFirst("<FamilyData>", string.Empty);
                    //WTableRow templateRow = table.Rows[3].Clone();
                    //WTableRow row;
                    //int index = 3;

                    ////var listFamilyInfo = (from a in db.InfoFamilies.AsNoTracking()
                    ////                      join b in db.Relationships.AsNoTracking() on a.RelationshipId equals b.Id
                    ////                      where a.Id.Equals(model.Id)
                    ////                      select new
                    ////                      {
                    ////                          Name = a.Name,
                    ////                          Birthday = a.DateOfBirth,
                    ////                          RelationshipName = b.Name,
                    ////                          Gender = a.Gender,
                    ////                          Job = a.Job,
                    ////                          LiveWithChild = a.LiveWithChild,
                    ////                      }).ToList();

                    ////foreach (var item in listFamilyInfo)
                    ////{
                    ////    if (index > 9)
                    ////    {
                    ////        table.Rows.Insert(index, templateRow.Clone());
                    ////    }
                    ////    row = table.Rows[index];
                    ////    row.Cells[0].Paragraphs[0].Text = (index - 1).ToString();
                    ////    row.Cells[1].Paragraphs[0].Text = item.Name != null ? item.Name : "";
                    ////    row.Cells[2].Paragraphs[0].Text = item.Birthday.ToString() != null ? item.Birthday.ToString() : "";
                    ////    row.Cells[3].Paragraphs[0].Text = item.RelationshipName != null ? item.RelationshipName : "";
                    ////    row.Cells[4].Paragraphs[0].Text = item.Gender.ToString() == "1" ? "Nam" : "Nữ";
                    ////    row.Cells[5].Paragraphs[0].Text = item.Job != null ? item.Job : "";
                    ////    row.Cells[6].Paragraphs[0].Text = item.LiveWithChild.ToString() == "0" ? "Có" : "Không";
                    ////    index++;
                    ////}

                    ////familyDetail
                    //WTable FMtable = wordDocument.GetTableByFindText("<FamilyDetailIndex1>");

                    //if (FMtable == null)
                    //{
                    //    // TODO message error;
                    //}

                    //wordDocument.NTSReplaceFirst("<FamilyDetailIndex1>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<FamilyDetailIndex2>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<FamilyDetailIndex3>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<FamilyDetailIndex4>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<EmailRequestIndex>", string.Empty);

                    //WTableRow FMtemplateRow = table.Rows[0].Clone();
                    //WTableRow FMrow = FMtable.Rows[0];
                    //int k = 0;
                    //n = 0;

                    //model.FamilyDetail = JsonConvert.DeserializeObject<List<QuestionModel>>(data.FamilyDetail);

                    //foreach (var item in model.FamilyDetail)
                    //{
                    //    if (k < 3)
                    //    {
                    //        if(k == 1)
                    //        {
                    //            for (int j = 0; j < item.Answers.Count(); j++)
                    //            {
                    //                if (j <= 4)
                    //                {
                    //                    if (item.Answers[j].Check == true)
                    //                    {
                    //                        WParagraph newParagraph = new WParagraph(wordDocument);
                    //                        newParagraph.Text = item.Answers[j].Index + " x";
                    //                        FMtable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //                    }
                    //                    else
                    //                    {
                    //                        WParagraph newParagraph = new WParagraph(wordDocument);
                    //                        newParagraph.Text = item.Answers[j].Index;
                    //                        FMtable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //                    }

                    //                } else
                    //                {
                    //                    if (item.Answers[j].Check == true)
                    //                    {
                    //                        WParagraph newParagraph = new WParagraph(wordDocument);
                    //                        newParagraph.Text = item.Answers[j].Index + " x";
                    //                        FMtable.Rows[1].Cells[n + 2].Paragraphs.Add(newParagraph);
                    //                    }
                    //                    else
                    //                    {
                    //                        WParagraph newParagraph = new WParagraph(wordDocument);
                    //                        newParagraph.Text = item.Answers[j].Index;
                    //                        FMtable.Rows[1].Cells[n + 2].Paragraphs.Add(newParagraph);
                    //                    }
                    //                }        
                    //            }
                    //            WParagraph otherAnswer = new WParagraph(wordDocument);
                    //            otherAnswer.Text = item.OtherAnswer;
                    //            FMtable.Rows[2].Cells[k].Paragraphs.Add(otherAnswer);

                    //            n = n + 2;

                    //        } else
                    //        {

                    //            for (int j = 0; j < item.Answers.Count(); j++)
                    //            {
                    //                if (item.Answers[j].Check == true)
                    //                {
                    //                    WParagraph newParagraph = new WParagraph(wordDocument);
                    //                    newParagraph.Text = item.Answers[j].Index + " x";
                    //                    FMtable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //                }
                    //                else
                    //                {
                    //                    WParagraph newParagraph = new WParagraph(wordDocument);
                    //                    newParagraph.Text = item.Answers[j].Index;
                    //                    FMtable.Rows[1].Cells[n].Paragraphs.Add(newParagraph);
                    //                }
                    //            }
                    //            if (k == 2)
                    //            {
                    //                WParagraph otherAnswer = new WParagraph(wordDocument);
                    //                otherAnswer.Text = item.OtherAnswer;
                    //                FMtable.Rows[2].Cells[k].Paragraphs.Add(otherAnswer);
                    //            }

                    //        }
                    //        k++;
                    //        n = n + 2;
                    //    }
                    //    else
                    //    {
                    //        //answers
                    //        for (int j = 0; j < model.FamilyDetail[3].Answers.Count(); j++)
                    //        {

                    //            if (model.FamilyDetail[3].Answers[j].Check == true)
                    //            {
                    //                WParagraph newIndex = new WParagraph(wordDocument);
                    //                newIndex.Text = model.FamilyDetail[3].Answers[j].Index + " x";
                    //                FMtable.Rows[7].Cells[0].Paragraphs.Add(newIndex);
                    //            } else
                    //            {
                    //                WParagraph newIndex = new WParagraph(wordDocument);
                    //                newIndex.Text = model.FamilyDetail[3].Answers[j].Index;
                    //                FMtable.Rows[7].Cells[0].Paragraphs.Add(newIndex);
                    //            }

                    //        }
                    //        wordDocument.NTSReplaceFirst("<FamilyDetailOther>", model.FamilyDetail[3].OtherAnswer);
                    //    }

                    //}

                    ////houseCondition
                    //WTable HCtable = wordDocument.GetTableByFindText("<HCIndex1>");

                    //if (HCtable == null)
                    //{
                    //    // TODO message error;
                    //}

                    //wordDocument.NTSReplaceFirst("<HCIndex1>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HCIndex2>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HCIndex3>", string.Empty);
                    //wordDocument.NTSReplaceFirst("<HCIndex4>", string.Empty);

                    //WTableRow HCtemplateRow = table.Rows[0].Clone();
                    //WTableRow HCrow;

                    //model.HouseConditionModel = JsonConvert.DeserializeObject<List<QuestionModel>>(data.HouseCondition);
                    //i = 0;
                    //int m = 0;
                    //foreach (var item in model.HouseConditionModel)
                    //{
                    //    HCrow = HCtable.Rows[1];
                    //    //answers

                    //    for (int j = 0; j < item.Answers.Count(); j++)
                    //    {
                    //        if (item.Answers[j].Check == true)
                    //        {
                    //            WParagraph newIndex = new WParagraph(wordDocument);
                    //            newIndex.Text = item.Answers[j].Index + " x";
                    //            HCtable.Rows[2].Cells[m].Paragraphs.Add(newIndex);
                    //        }
                    //        else
                    //        {
                    //            WParagraph newIndex = new WParagraph(wordDocument);
                    //            newIndex.Text = item.Answers[j].Index;
                    //            HCtable.Rows[2].Cells[m].Paragraphs.Add(newIndex);
                    //        }
                    //    }
                    //    i++;
                    //    m = m + 2;
                    //}

                    ////conditionOthers
                    //model.ConditionOtherModel = JsonConvert.DeserializeObject<List<OtherConditionModel>>(data.ConditionOther);

                    ////Q1
                    //for (int j = 0; j < model.ConditionOtherModel[0].Question[0].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[0].Question[0].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[0].Answers[j].Name + " x";
                    //        HCtable.Rows[4].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[0].Answers[j].Name;
                    //        HCtable.Rows[4].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q2
                    //for (int j = 0; j < model.ConditionOtherModel[0].Question[1].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[0].Question[1].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[1].Answers[j].Index + " x";
                    //        HCtable.Rows[6].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[1].Answers[j].Index;
                    //        HCtable.Rows[6].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q3
                    //for (int j = 0; j < model.ConditionOtherModel[0].Question[2].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[0].Question[2].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[2].Answers[j].Index + " x";
                    //        HCtable.Rows[8].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[2].Answers[j].Index;
                    //        HCtable.Rows[8].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q4
                    //for (int j = 0; j < model.ConditionOtherModel[0].Question[3].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[0].Question[3].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[3].Answers[j].Index + " x";
                    //        HCtable.Rows[10].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[3].Answers[j].Index;
                    //        HCtable.Rows[10].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q5
                    //for (int j = 0; j < model.ConditionOtherModel[0].Question[4].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[0].Question[4].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[4].Answers[j].Index + " x";
                    //        HCtable.Rows[12].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[0].Question[4].Answers[j].Index;
                    //        HCtable.Rows[12].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    //wordDocument.NTSReplaceFirst("<Q5AnotherAnswer>", model.ConditionOtherModel[0].Question[4].OtherAnswer);

                    ////Q6
                    //for (int j = 0; j < model.ConditionOtherModel[1].Question[0].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[1].Question[0].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[1].Question[0].Answers[j].Name + " x";
                    //        HCtable.Rows[13].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[1].Question[0].Answers[j].Name;
                    //        HCtable.Rows[13].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q7
                    //for (int j = 0; j < model.ConditionOtherModel[1].Question[1].Answers.Count(); j++)
                    //{
                    //    if (model.ConditionOtherModel[1].Question[1].Answers[j].Check == true)
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[1].Question[1].Answers[j].Index + " x";
                    //        HCtable.Rows[16].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //    else
                    //    {
                    //        WParagraph newParagraph = new WParagraph(wordDocument);
                    //        newParagraph.Text = model.ConditionOtherModel[1].Question[1].Answers[j].Index;
                    //        HCtable.Rows[16].Cells[j + 1].Paragraphs.Add(newParagraph);
                    //    }
                    //}

                    ////Q8


                    string specifyName = DateTime.Now.ToString("ddMMyyyyhhmmss");
                    string pathFileSave = string.Empty;
                    pathFileSave = HttpContext.Current.Server.MapPath("/Template/Export/HoSoTre_" + specifyName + ".doc");
                    wordDocument.Save(pathFileSave);
                    wordDocument.Close();
                    pathreturn = "/Template/Export/HoSoTre_" + specifyName + ".doc";
                    return pathreturn;
                }

            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfilePrintBusiness.ExportChildProfile", ex.Message, model);
                throw new Exception("Có lỗi trong quá trình xử lý");
            }

            return pathreturn;
        }
    }
}
