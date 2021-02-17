using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Repository.DTO
{
    public class QuestionDTO : AbstractModel<int>, IDomainOf<Question>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Votes { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public string Category { get; set; }
    }
}
