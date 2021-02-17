using Quicker.Abstracts.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Repository.Models
{
    public class Answer : AbstractModel<int>
    {
        [Required, StringLength(25)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Votes { get; set; }

        public int QuestionId { get; set; }

        public Question QuestionNavigation { get; set; }

        public int? AnswerId { get; set; }

        public Answer AnswerNavigation { get; set; }
    }
}
