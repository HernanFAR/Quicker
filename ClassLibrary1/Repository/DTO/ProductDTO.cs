using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Test.Common.Repository.DTO
{
    public class ProductDTO : AbstractModel<int>, IDTOOf<Product>
    {
        [Required, StringLength(128, MinimumLength = 2)]
        public string Name { get; set; }

        public int Count { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Code { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }
    }
}
