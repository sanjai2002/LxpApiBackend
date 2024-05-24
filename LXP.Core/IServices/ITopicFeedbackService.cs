﻿using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.IServices
{
    public interface ITopicFeedbackService
    {
        IEnumerable<TopicFeedbackQuestionNoDTO> GetAllFeedbackQuestions();
        TopicFeedbackQuestionNoDTO GetFeedbackQuestionById(Guid id);
        void SubmitFeedbackResponse(TopicFeedbackResponseDTO feedbackResponse);
        bool AddFeedbackQuestion(TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options);
        bool UpdateFeedbackQuestion(Guid id, TopicFeedbackQuestionDTO question, List<FeedbackOptionDTO> options);
        bool DeleteFeedbackQuestion(Guid id);
        IEnumerable<TopicFeedbackQuestionNoDTO> GetFeedbackQuestionsByTopicId(Guid topicId);

    }
}
