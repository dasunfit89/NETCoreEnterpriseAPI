using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;

namespace EnterpriseApp.API.Data
{
    public interface IRepository
    {
        // generic interface method to save or update object asynchronously into local table
        Task<bool> SaveAsync<T>(T obj) where T : BaseEntity, new();

        // generic interface method to save or update object asynchronously into local table
        Task<T> UpdateAsync<T>(T obj) where T : BaseEntity, new();

        // Generic interface method to fetch all the objects asynchronously from local table
        Task<List<T>> GetAllAsync<T>() where T : BaseEntity, new();

        // Generic interface method to find a specific object asynchronously from the local table
        Task<T> FindAsync<T>(string id) where T : BaseEntity, new();

        Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new();

        Task<T> FilterOneAsync<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new();

        Task DeleteAsync<T>(T obj) where T : BaseEntity, new();
    }
}
