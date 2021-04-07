using Quicker.Abstracts.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Common.Repository
{
    public class Category : AbstractModel<int>
    {
        [Required, StringLength(128, MinimumLength = 2)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
