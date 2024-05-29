﻿using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Data.DBContexts;
using LXP.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.Repository
{
    public class UserReportRepository : IUserReportRepository
    {
        private readonly LXPDbContext _lXPDbContext;
        
        public UserReportRepository(LXPDbContext lXPDbContext)
        {
            _lXPDbContext = lXPDbContext;
        }
        public IEnumerable<UserReportViewModel> GetUserReport()
        {
            //var learnerId = _lXPDbContext.Enrollments
            //   .GroupBy(e => e.LearnerId)
            //   .Select(g => g.Key)
            //   .ToList();



            var query = _lXPDbContext.Enrollments
             .GroupBy(e => e.LearnerId)
                 .Select(grouped => new UserReportViewModel
          {
              UserName = $"{_lXPDbContext.LearnerProfiles.Where(x => x.LearnerId.Equals(grouped.Key)).First().FirstName} {_lXPDbContext.LearnerProfiles.Where(x => x.LearnerId.Equals(grouped.Key)).First().LastName}",
              LearnerId = grouped.Key.ToString(),
              EnrolledCourse = grouped.Count(),
              CompletedCourse = grouped.Count(x => x.CompletedStatus == 1),
              LastLogin= _lXPDbContext.Learners.Where(x => x.LearnerId.Equals(grouped.Key)).First().UserLastLogin
          });

            var userReports = query.ToList();
            return userReports;


        }
    }
}
