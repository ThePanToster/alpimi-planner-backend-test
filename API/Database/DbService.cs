﻿using System.Data;
using Dapper;
using Microsoft.EntityFrameworkCore;

namespace AlpimiAPI.Database
{
    public class DbService : IDbService
    {
        private readonly IDbConnection _db;

        public DbService(IDbConnection dbConnection)
        {
            _db = dbConnection;
        }

        public async Task<T?> Get<T>(string command, object parms)
        {
            return await _db.QuerySingleOrDefaultAsync<T>(command, parms);
        }

        public async Task<IEnumerable<T>?> GetAll<T>(string command, object parms)
        {
            return (await _db.QueryAsync<T>(command, parms)).ToList();
        }

        public async Task<T?> Post<T>(string command, object parms)
        {
            Task<T?> result;
            result = _db.QuerySingleOrDefaultAsync<T>(command, parms);
            return await result;
        }

        public async Task<T?> Update<T>(string command, object parms)
        {
            Task<T?> result;
            result = _db.QuerySingleOrDefaultAsync<T>(command, parms);
            return await result;
        }

        public async Task Delete(string command, object parms)
        {
            await _db.ExecuteAsync(command, parms);
        }

        public async Task Raw(string command)
        {
            await _db.ExecuteAsync(command);
        }
    }
}
