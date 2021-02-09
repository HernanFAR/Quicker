using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.Test.Models
{
    public class SecondLevelEntityDTO : AbstractModel<int>, IDTOOf<SecondLevelEntity>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int FirstLevelEntityId { get; set; }

        public string FirstLevelEntity { get; set; }
    }
}
