using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model.StatisticModels;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticGenderWardController : BaseController
    {
        StatisticGenderWardBussiness _buss = new StatisticGenderWardBussiness();
        // GET: StatisticGenderWard
        public ActionResult StatisticGenderWard(StatisticSearchCondition model)
        {
            try
            {

                // chart
                List<StatisticByGenderModelChart> lstChart = new List<StatisticByGenderModelChart>();
                StatisticByGenderModelChart itemChart;
                // table
                List<StatisticGenderWardModel> lstTable = new List<StatisticGenderWardModel>();
                StatisticGenderWardModel itemTable;

                var data = _buss.SearchStatisticByAge(model);
                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                foreach (var item in abuSeAll)
                {
                    //table
                    itemTable = new StatisticGenderWardModel();
                    itemTable.Type = item.Name;
                    itemTable.CountNam = data.Where(u => u.Gender == Constants.Male && item.Id.Equals(u.Type)).Count();
                    itemTable.CountNu = data.Where(u => u.Gender == Constants.FeMale && item.Id.Equals(u.Type)).Count();
                    itemTable.CountKhong = data.Where(u => u.Gender == Constants.UnMale && item.Id.Equals(u.Type)).Count();
                    lstTable.Add(itemTable);
                    //chart
                    itemChart = new StatisticByGenderModelChart();
                    itemChart.Label = item.Name;
                    itemChart.countTotal = new List<int>();
                    itemChart.countTotal.Add(itemTable.CountNam);
                    itemChart.countTotal.Add(itemTable.CountNu);
                    itemChart.countTotal.Add(itemTable.CountKhong);
                    lstChart.Add(itemChart);
                }

                return Json(new { ok = true, lstChart = lstChart, lstTable = lstTable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {

                return Json(new { ok = false, mess = exx.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReportByGenderWard()
        {
            return View();

        }
    }
}