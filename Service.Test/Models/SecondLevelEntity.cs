using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Service.Test.Models
{
    public class SecondLevelEntity : AbstractModel<int>, IDomainOf<SecondLevelEntityDTO>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        public int FirstLevelEntityId { get; set; }

        public FirstLevelEntity FirstLevelEntityNavigation { get; set; }
    }
}
