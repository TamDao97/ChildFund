﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
   public class FamilyQuestionModel
    {
        public string IndexQ1 { get; set; }
        public string IndexQ2C1 { get; set; }
        public string IndexQ2C2 { get; set; }
        public string IndexQ3 { get; set; }
        public string IndexQ4 { get; set; }

        public string NameQ1 { get; set; }
        public string NameQ2C1 { get; set; }
        public string NameQ2C2 { get; set; }
        public string NameQ3 { get; set; }
        public string NameQ4 { get; set; }

        public bool CheckC1 { get; set; }
        public bool CheckC2 { get; set; }
        public bool CheckC3 { get; set; }
        public bool CheckC4 { get; set; }
        public bool CheckC5 { get; set; }
    }
}
