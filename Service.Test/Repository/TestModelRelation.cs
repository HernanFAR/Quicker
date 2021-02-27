using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using Quicker.Test.Repository.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quicker.Test.Repository
{
    public class TestModelRelation : AbstractModel<int>, IDomainOf<TestModelRelationDTO>
    {
        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int TestModelId { get; set; }

        public string UniqueName { get; set; }

        public TestModel TestModelNavigation { get; set; }
    }
}
