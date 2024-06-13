﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LXP.Common.Entities;
using LXP.Common.ViewModels;

namespace LXP.Core.IServices
{
    public interface IMaterialTypeServices
    {
        List<MaterialTypeViewModel> GetAllMaterialType();
    }
}
