using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class ComboboxResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UnsignName { get; set; }
        public string PId { get; set; }
    }
    public class ImageHistory
    {
        public string Image { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
