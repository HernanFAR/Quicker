using Quicker.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quicker.Test.Repository
{
    public class TestModelRelation : AbstractModel<int>
    {
        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int TestModelId { get; set; }

        public TestModel TestModelNavigation { get; set; }
    }
}
