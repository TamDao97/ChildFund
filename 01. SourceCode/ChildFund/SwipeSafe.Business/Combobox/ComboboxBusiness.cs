
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using SwipeSafe.Model;
using SwipeSafe.Model.Repositories;
using SwipeSafe.Model.SearchResults;
namespace SwipeSafe.Business
{
    public class ComboboxBusiness
    {
        private ReportAppEntities _dbCbb = new ReportAppEntities();

        public List<ComboboxResult> GetFormsAbuses()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.FormsAbuses.AsNoTracking()
                                orderby a.OrderNumber
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception)
            {
            }
            return searchResult;
        }

        public List<ComboboxResult> GetRelationshipCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Relationships.AsNoTracking()
                                orderby a.OrderNumber
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        public List<ComboboxResult> GetProvinceCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Provinces.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetDistrictCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Districts.AsNoTracking()
                                where a.ProvinceId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }
        public List<ComboboxResult> GetWardCBB(string id)
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Wards.AsNoTracking()
                                where a.DistrictId == id
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        /// GetDistrictCBB Mobile
        /// </summary>
        /// <returns></returns>
        public List<ComboboxResult> GetDistrictCBB()
        {
            List<ComboboxResult> searchResult = new List<ComboboxResult>();
            try
            {
                searchResult = (from a in _dbCbb.Districts.AsNoTracking()
                                orderby a.Name
                                select new ComboboxResult()
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    ParentId = a.ProvinceId
                                }
                               ).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

        /// <summary>
        /// GetWardCBB Mobile
        /// </summary>
        /// <returns></returns>
        public List<WardGroupModel> GetWardCBB()
        {
            List<WardGroupModel> searchResult = new List<WardGroupModel>();
            try
            {
                var result = (from a in _dbCbb.Wards.AsNoTracking()
                              orderby a.Name
                              select new ComboboxResult()
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  ParentId = a.DistrictId
                              }
                                 ).ToList();

                searchResult = result.GroupBy(r => r.ParentId).Select(r => new WardGroupModel() { DistrictId = r.Key, ListWard = r.ToList() }).ToList();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return searchResult;
        }

    }
}
