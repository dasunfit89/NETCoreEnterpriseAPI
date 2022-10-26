using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using EnterpriseApp.API.Data;
using EnterpriseApp.API.Data.Entity;
using Newtonsoft.Json;

namespace EnterpriseApp.API.Data
{
    public class FirestoreRepository : IRepository
    {
        private FirestoreDb _fireStoreDb;

        public FirestoreRepository()
        {
            string filepath = "locatieapp-81a4f-634e8ed0f4be.json";

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);

            _fireStoreDb = FirestoreDb.Create("locatieapp-81a4f");
        }

        public async Task DeleteAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            CollectionReference colRef = _fireStoreDb.Collection(collectionName);

            DocumentReference docRef = colRef.Document(obj.Id);

            var result = await docRef.DeleteAsync();
        }

        public async Task<List<T>> GetAllAsync<T>() where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            List<T> entities = new List<T>();

            Query query = _fireStoreDb.Collection(collectionName);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            entities = ProcessQuery<T>(querySnapshot);

            return entities;
        }

        public async Task<T> FindAsync<T>(string id) where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            T entity = null;

            DocumentReference docRef = _fireStoreDb.Collection(collectionName).Document(id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                entity = snapshot.ConvertTo<T>();
                entity.Id = snapshot.Id;
            }

            return entity;
        }

        public async Task<List<T>> FilterAsync<T>(Expression<Func<T, bool>> filter) where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            List<T> entities = new List<T>();

            Query query = _fireStoreDb.Collection(collectionName);

            Dictionary<string, object> dict = new Dictionary<string, object>();

            foreach (var item in dict)
            {
                query = query.WhereEqualTo(item.Key, item.Value);
            }

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            entities = ProcessQuery<T>(querySnapshot);

            return entities;
        }

        public async Task<bool> SaveAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            CollectionReference colRef = _fireStoreDb.Collection(collectionName);

            await colRef.AddAsync(obj);

            return true;
        }

        public async Task<T> UpdateAsync<T>(T obj) where T : BaseEntity, new()
        {
            string collectionName = typeof(T).Name;

            CollectionReference colRef = _fireStoreDb.Collection(collectionName);

            DocumentReference docRef = colRef.Document(obj.Id);

            await docRef.SetAsync(obj, SetOptions.Overwrite);

            return obj;
        }

        private List<T> ProcessQuery<T>(QuerySnapshot querySnapshot) where T : BaseEntity, new()
        {
            List<T> entities = new List<T>();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> entityDictionary = documentSnapshot.ToDictionary();

                    string json = JsonConvert.SerializeObject(entityDictionary);

                    T entity = JsonConvert.DeserializeObject<T>(json);
                    entity.Id = documentSnapshot.Id;
                    entity.CreatedOn = documentSnapshot.CreateTime.Value.ToDateTime();

                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
