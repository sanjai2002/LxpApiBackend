﻿using LXP.Common.Entities;
using LXP.Data.IRepository;

namespace LXP.Data.Repository
{
    public class MaterialTypeRepository : IMaterialTypeRepository
    {
        private readonly LXPDbContext _lXPDbContext; // through this connection with table in db

        public MaterialTypeRepository(LXPDbContext lXPDbContext) // creating a constructor and using the gl variable in all over the class
        {
            _lXPDbContext = lXPDbContext;
        }

        public MaterialType GetMaterialTypeByMaterialTypeId(Guid materialTypeId)
        {
            return _lXPDbContext.MaterialTypes.Find(materialTypeId);
        }

        public List<MaterialType> GetAllMaterialTypes()
        {
            return _lXPDbContext.MaterialTypes.ToList();
        }
    }
}
