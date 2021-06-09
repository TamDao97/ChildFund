using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using System.Collections.Generic;
using System.Linq;

namespace ChildProfiles.Business.Business
{
    public class ProfileCatalogDA
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public List<SubjectCapacityModel> GetDataSubjectCapacity(QuestionModel dataSubject, QuestionModel dataCapacity)
        {
            List<SubjectCapacityModel> data = new List<SubjectCapacityModel>();

            int ix = (dataSubject.Answers.Count / 2) + (dataSubject.Answers.Count % 2);

            List<CheckBoxModel> dataSubjectc1 = dataSubject.Answers.Take(ix).ToList();
            List<CheckBoxModel> dataSubjectc2 = dataSubject.Answers.Skip(ix).ToList();

            data = (from a in dataSubjectc1
                    join b in dataCapacity.Answers on a.Index equals b.Index into b
                    from bb in b.DefaultIfEmpty()
                    select new SubjectCapacityModel
                    {
                        IndexC1 = a.Index,
                        CheckC1 = a.Check,
                        Index = bb != null ? bb.Index : string.Empty,
                        NameCapacity = bb != null ? bb.Name : string.Empty,
                        CheckC3 = bb != null ? bb.Check : false,
                        NameSubjectC1 = a.Name
                    }).ToList();
            for (int i = 0; i < dataSubject.Answers.Count - ix; i++)
            {
                data[i].IndexC2 = dataSubjectc2[i].Index;
                data[i].NameSubjectC2 = dataSubjectc2[i].Name;
                data[i].CheckC2 = dataSubjectc2[i].Check;

            }

            return data;
        }

        public List<FamilyWorkHealthModel> GetDataFamilyWorkHealth(QuestionModel dataHouseWork, QuestionModel dataHealth)
        {
            List<FamilyWorkHealthModel> data = new List<FamilyWorkHealthModel>();


            int ix = (dataHouseWork.Answers.Count / 2) + (dataHouseWork.Answers.Count % 2);

            List<CheckBoxModel> dataSubjectc1 = dataHouseWork.Answers.Take(ix).ToList();
            List<CheckBoxModel> dataSubjectc2 = dataHouseWork.Answers.Skip(ix).ToList();

            data = (from a in dataSubjectc1
                    join b in dataHealth.Answers on a.Index equals b.Index into b
                    from bb in b.DefaultIfEmpty()
                    select new FamilyWorkHealthModel
                    {
                        IndexC1 = a.Index,
                        CheckC1 = a.Check,
                        Index = bb != null ? bb.Index : string.Empty,
                        NameHealth = bb != null ? bb.Name : string.Empty,
                        CheckC3 = bb != null ? bb.Check : false,
                        NameFamilyWorkC1 = a.Name
                    }).ToList();
            for (int i = 0; i < ix; i++)
            {
                data[i].IndexC2 = dataSubjectc2[i].Index;
                data[i].NameFamilyWorkC2 = dataSubjectc2[i].Name;
                data[i].CheckC2 = dataSubjectc2[i].Check;
            }

            return data;
        }

        public List<PersonalsModel> GetDataPersonals(QuestionModel dataCharacteristics, QuestionModel dataHobbies, QuestionModel dataDream)
        {
            List<PersonalsModel> data = new List<PersonalsModel>();


            data = (from a in dataDream.Answers
                    join b in dataHobbies.Answers on a.Index equals b.Index into b
                    from bb in b.DefaultIfEmpty()
                    join c in dataCharacteristics.Answers on a.Index equals c.Index into c
                    from cc in c.DefaultIfEmpty()
                    select new PersonalsModel
                    {
                        IndexDream = a.Index,
                        IndexHobbies = bb != null ? bb.Index : string.Empty,
                        IndexCharacter = cc != null ? cc.Index : string.Empty,
                        NameHobbies = bb != null ? bb.Name : string.Empty,
                        NameCharacter = cc != null ? cc.Name : string.Empty,
                        NameDream = a.Name,
                        CheckC1 = cc != null ? cc.Check : false,
                        CheckC2 = bb != null ? bb.Check : false,
                        CheckC3 = a.Check
                    }).ToList();

            return data;
        }


        public List<FamilyQuestionModel> GetDataFamilyInfo(QuestionModel dataFamilyQ1, QuestionModel dataFamilyQ2, QuestionModel dataFamilyQ3, QuestionModel dataFamilyQ4)
        {
            List<FamilyQuestionModel> data = new List<FamilyQuestionModel>();


            int ix = (dataFamilyQ2.Answers.Count / 2) + (dataFamilyQ2.Answers.Count % 2);

            List<CheckBoxModel> dataQ2c1 = dataFamilyQ2.Answers.Take(ix).ToList();
            List<CheckBoxModel> dataQ2c2 = dataFamilyQ2.Answers.Skip(ix).ToList();

            data = (from a in dataQ2c1
                    join b in dataFamilyQ1.Answers on a.Index equals b.Index into b
                    from bb in b.DefaultIfEmpty()
                    join c in dataFamilyQ3.Answers on a.Index equals c.Index into c
                    from cc in c.DefaultIfEmpty()
                    join d in dataFamilyQ4.Answers on a.Index equals d.Index into d
                    from dd in d.DefaultIfEmpty()
                    select new FamilyQuestionModel
                    {
                        IndexQ1 = bb != null ? bb.Index : string.Empty,
                        IndexQ2C1 = a.Index,
                        IndexQ3 = cc != null ? cc.Index : string.Empty,
                        IndexQ4 = dd != null ? dd.Index : string.Empty,
                        NameQ1 = bb != null ? bb.Name : string.Empty,
                        NameQ2C1 = a.Name,
                        NameQ3 = cc != null ? cc.Name : string.Empty,
                        NameQ4 = dd != null ? dd.Name : string.Empty,

                        CheckC1 = bb != null ? bb.Check : false,
                        CheckC2 = a.Check,
                        CheckC4 = cc != null ? cc.Check : false,
                        CheckC5 = dd != null ? dd.Check : false,

                    }).ToList();
            for (int i = 0; i < ix - (dataFamilyQ2.Answers.Count % 2); i++)
            {
                data[i].IndexQ2C2 = dataQ2c2[i].Index;
                data[i].NameQ2C2 = dataQ2c2[i].Name;
                data[i].CheckC3 = dataQ2c2[i].Check;
            }
            return data;
        }

        public List<HouseConditionModel> GetDataConditionHome(QuestionModel dataQ1, QuestionModel dataQ2, QuestionModel dataQ3, QuestionModel dataQ4)
        {
            List<HouseConditionModel> data = new List<HouseConditionModel>();

            data = (from a in dataQ2.Answers
                    join b in dataQ1.Answers on a.Index equals b.Index into b
                    from bb in b.DefaultIfEmpty()
                    join c in dataQ3.Answers on a.Index equals c.Index into c
                    from cc in c.DefaultIfEmpty()
                    join d in dataQ4.Answers on a.Index equals d.Index into d
                    from dd in d.DefaultIfEmpty()
                    select new HouseConditionModel
                    {
                        IndexQ1 = bb != null ? bb.Index : string.Empty,
                        IndexQ2 = a.Index,
                        IndexQ3 = cc != null ? cc.Index : string.Empty,
                        IndexQ4 = dd != null ? dd.Index : string.Empty,
                        NameQ1 = bb != null ? bb.Name : string.Empty,
                        NameQ2 = a.Name,
                        NameQ3 = cc != null ? cc.Name : string.Empty,
                        NameQ4 = dd != null ? dd.Name : string.Empty,

                        CheckC1 = bb != null ? bb.Check : false,
                        CheckC2 = a.Check,
                        CheckC3 = cc != null ? cc.Check : false,
                        CheckC4 = dd != null ? dd.Check : false,
                    }).ToList();

            return data;
        }

        //public List<OtherConditionModel> GetDataOtherCondition()
        //{  
        //    return ConvertFileJson<List<OtherConditionModel>>.ReadFile("~/JsonData/OtherCondition.json"); ;
        //}

        public List<Relationship> GetRelationships()
        {
            var listRelationShip = db.Relationships.AsNoTracking().ToList();
            return listRelationShip;
        }
    }
}
