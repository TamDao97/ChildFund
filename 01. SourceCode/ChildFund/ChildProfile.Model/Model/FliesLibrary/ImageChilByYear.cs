using ChildProfiles.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.FliesLibrary
{
    public class ImageChildView
    {
        public int? Year { get; set; }
        public List<ImageChildByYear> List { get; set; }
    }
    public class ImageChildByYearView : ImageChildByYear
    {
        public string ChildCode { get; set; }
        public string ChildName { get; set; }
    }
    public class ImageChildByYearLevelView
    {
        public string AreaName { get; set; }
        public string  AreaId { get; set; }
        public List<ImageChildByYearView> List { get; set; }
    }
}
