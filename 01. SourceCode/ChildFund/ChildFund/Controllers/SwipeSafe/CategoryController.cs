
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ChildFund.Controllers.SwipeSafe
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult ListCategory(CategorySearchCondition modelSearch)
        //{
        //    SearchResultObject<CategorySearchResult> list = new SearchResultObject<CategorySearchResult>();
        //    using (var client = new HttpClient())
        //    {
        //        using (var formData = new MultipartFormDataContent())
        //        {
        //            HttpContent streamContent = new StringContent(JsonConvert.SerializeObject(modelSearch));
        //            formData.Add(streamContent, "Model");
        //            var responseImage = client.PostAsync(string.Format("{0}api/Category/SearchCategory", Constants.ApiUrl), formData).Result;
        //            if (responseImage.IsSuccessStatusCode)
        //            {
        //                list = JsonConvert.DeserializeObject<SearchResultObject<CategorySearchResult>>(responseImage.Content.ReadAsStringAsync().Result);
        //            }
        //        }
        //    }
        //    ViewBag.pages = Common.PhanTrang(modelSearch.PageSize, modelSearch.PageNumber, list.TotalItem, "");

        //    return PartialView(list);
        //}
    }
}