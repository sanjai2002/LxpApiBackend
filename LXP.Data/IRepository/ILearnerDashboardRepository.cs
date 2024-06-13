﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Data.IRepository
{
    public interface ILearnerDashboardRepository
    {
        public List<Enrollment> GetLearnerenrolledCourseCount(Guid learnerId);
        public List<Enrollment> GetLearnerCompletedCount(Guid learnerId);
        public List<Enrollment> GetLearnerDashboardInProgressCount(Guid learnerId);
    }
}
