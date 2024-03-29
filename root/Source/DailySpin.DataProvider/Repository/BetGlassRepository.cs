﻿using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using DailySpin.Website.Models;

namespace DailySpin.DataProvider.Repository
{
    public class BetGlassRepository : BaseRepository<BetsGlass>, IBetGlassRepository
    {
        public BetGlassRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
