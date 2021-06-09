using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model
{
    public class WardGroupModel
    {
        public string DistrictId { get; set; }
        public List<SearchResults.ComboboxResult> ListWard { get; set; }
    }
}
