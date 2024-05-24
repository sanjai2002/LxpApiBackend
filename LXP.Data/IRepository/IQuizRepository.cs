﻿using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface IQuizRepository
    {
        QuizDto GetQuizById(Guid quizId);
        IEnumerable<QuizDto> GetAllQuizzes();
        void CreateQuiz(QuizDto quiz, Guid TopicId);
        void UpdateQuiz(QuizDto quiz);
        void DeleteQuiz(Guid quizId);
        Task<Quiz> GetQuizByNameAsync(string name);
        Guid? GetQuizIdByTopicId(Guid topicId);
    }
}

//using LXP.Common.DTO;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace LXP.Data.IRepository
//{
//    public interface IQuizRepository
//    {
//        Task<QuizDto> GetQuizByIdAsync(Guid quizId);
//        Task<IEnumerable<QuizDto>> GetAllQuizzesAsync();
//        Task UpdateQuizAsync(QuizDto quiz);
//        Task DeleteQuizAsync(Guid quizId);
//        Task CreateQuizAsync(QuizDto quiz, Guid TopicId);
//    }
//}

////using LXP.Common.DTO;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace LXP.Data.IRepository
////{
////    public interface IQuizRepository
////    {

////        QuizDto GetQuizById(Guid quizId);
////        IEnumerable<QuizDto> GetAllQuizzes();


////        void UpdateQuiz(QuizDto quiz);
////        void DeleteQuiz(Guid quizId);
////        Task<Quiz> GetQuizByNameAsync(string name);
////        void CreateQuiz(QuizDto quiz, Guid TopicId);




////    }
////}

////  void CreateQuiz(QuizDto quiz);