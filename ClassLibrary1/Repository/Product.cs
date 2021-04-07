using Quicker.Abstracts.Model;
using System.ComponentModel.DataAnnotations;

namespace Test.Common.Repository
{
    public class Product : AbstractModel<int>
    {
        [Required, StringLength(128, MinimumLength = 2)]
        public string Name { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Code { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
