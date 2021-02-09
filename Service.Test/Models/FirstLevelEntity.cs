using Quicker.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.Test.Models
{
    public class FirstLevelEntity : AbstractModel<int>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        public ICollection<SecondLevelEntity> SecondLevelEntityNavigations { get; set; }
    }
}
