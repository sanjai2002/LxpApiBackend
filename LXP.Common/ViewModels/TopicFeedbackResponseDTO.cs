﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{
    public class TopicFeedbackResponseDTO
    {
        public Guid TopicFeedbackQuestionId { get; set; }
        public Guid LearnerId { get; set; }
        public string Response { get; set; }
        public Guid? OptionId { get; set; }
        // Add any other properties you need
    }
}
