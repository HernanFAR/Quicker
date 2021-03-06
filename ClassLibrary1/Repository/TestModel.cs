﻿using Quicker.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Common.Repository
{
    public class TestModel : AbstractModel<int>
    {
        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(0, 100)]
        public int Percent { get; set; }

        public ICollection<TestModelRelation> TestModelRelationNavigations { get; set; }
    }
}
