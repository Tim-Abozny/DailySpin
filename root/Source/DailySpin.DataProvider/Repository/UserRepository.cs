﻿using DailySpin.DataProvider.Data;
using DailySpin.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailySpin.DataProvider.Repository
{
    public class UserRepository : IBaseRepository<UserAccount>
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public IQueryable<UserAccount> GetAll()
        {
            return _db.Users;
        }

        public async Task Delete(UserAccount entity)
        {
            _db.Users.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(UserAccount entity)
        {
            await _db.Users.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<UserAccount> Update(UserAccount entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}