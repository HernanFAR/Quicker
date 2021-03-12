using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Common.Repository.DTO
{
    public class TestModelRelationDTO : AbstractModel<int>, IDTOOf<TestModelRelation>
    {
        [Required, StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int TestModelId { get; set; }

        public string UniqueName { get; set; }

        public string TestModel { get; set; }
    }
}
