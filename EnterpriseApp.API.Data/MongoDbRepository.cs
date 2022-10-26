using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Data
{
    public class MongoDBRepository : IRepository
    {
        private IMongoDatabase database;

        public MongoDBRepository()
        {
            var settings = MongoClientSettings.FromConnectionString("xxxxxx");

            var client = new MongoClient(settings);

            database = client.GetDatabase("xx");
        }

        public async Task DeleteAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            var result = await mongoCollection.DeleteOneAsync(entity => entity.Id == obj.Id);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            List<T> entities = new List<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            entities = await mongoCollection.Find(entity => true).ToListAsync();

            return entities;
        }

        public async Task<T> FindAsync<T>(string id) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            T entity = null;

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            entity = await mongoCollection.Find(e => e.Id == id).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            List<T> entities = await mongoCollection.Find(filter).ToListAsync();

            return entities;
        }

        public async Task<T> FilterOneAsync<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            T entity = await mongoCollection.Find(filter).FirstOrDefaultAsync();

            return entity;
        }

        public async Task<bool> SaveAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            await mongoCollection.InsertOneAsync(obj);

            return true;
        }

        public async Task<T> UpdateAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = GetCollectionName<T>();

            IMongoCollection<T> mongoCollection = database.GetCollection<T>(collectionName);

            var result = await mongoCollection.ReplaceOneAsync(entity => entity.Id == obj.Id, obj);

            return obj;
        }

        private string GetCollectionName<T>() where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name.ToLower();

            return collectionName;
        }
    }
}
