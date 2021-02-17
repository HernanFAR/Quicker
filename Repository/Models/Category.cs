using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class Category : AbstractModel<int>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        public ICollection<Question> QuestionNavigations { get; set; }
    }
}
