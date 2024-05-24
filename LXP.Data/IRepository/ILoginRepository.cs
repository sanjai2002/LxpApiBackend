﻿
using LXP.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Data.IRepository
{
    public interface ILoginRepository
    {

        public Task LoginLearner(Learner loginmodel);

        public Task<bool> AnyUserByEmail(string loginmodel);


        public Task<bool> AnyLearnerByEmailAndPassword(string Email, string Password);

        public Task<Learner> GetLearnerByEmail(string Email);


        public Task UpdateLearnerPassword(string Email, string Password);


        public Task UpdateLearnerLastLogin(string Email);



    }
}