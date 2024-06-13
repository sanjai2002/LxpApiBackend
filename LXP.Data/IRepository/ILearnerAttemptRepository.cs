﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;

namespace LXP.Data.IRepository
{
    public interface ILearnerAttemptRepository
    {
        object GetScoreByTopicIdAndLernerId(Guid LearnerId);

        object GetScoreByLearnerId(Guid LearnerId);
    }
}
