using Quicker.Abstracts.Model;
using Quicker.Interfaces.Model;
using Repository.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class Question : AbstractModel<int>, IDTOOf<QuestionDTO>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Votes { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public Category CategoryNavigation { get; set; }

        public ICollection<Answer> AnswerNavigations { get; set; }
    }
}
