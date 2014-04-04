using ianhd.core.Data.Extensions;
using System;
using System.Collections.Generic;

namespace ianhd.core.Data
{
    public class Repository<T> where T : class
    {
        private Database _database;

        private string ConnectionString = string.Empty;

        [IgnoreField]
        private Database Database
        {
            get
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    throw new NullReferenceException("You must set a Database Connection String");
                }

                return _database ?? (_database = Database.CreateDatabase(ConnectionString));
            }
        }

        [IgnoreField]
        public Connection Connection
        {
            get
            {
                return this.Database.CreateConnection();
            }
        }

        public Repository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        
        public virtual void Add(T item)
        {
            using (var db = this.Database.CreateConnection())
            {
                db.Insert(item);
            }
        }

        public virtual void Update(T item)
        {
            using (var db = this.Database.CreateConnection())
            {
                db.Update(item);
            }
        }

        public virtual void Remove(T item)
        {
            using (var db = this.Database.CreateConnection())
            {
                db.Delete(item);
            }
        }
        
        public virtual T Find(int id)
        {
            using (var db = this.Database.CreateConnection())
            {
                return db.Get<T>(id);
            }
        }

        public virtual T Find(string id)
        {
            using (var db = this.Database.CreateConnection())
            {
                return db.Get<T>(id);
            }
        }

        public virtual IEnumerable<T> FindAll(IPredicate predicate = null)
        {
            using (var db = this.Database.CreateConnection())
            {
                return predicate == null ? 
                    db.GetList<T>() : 
                    db.GetList<T>(predicate);
            }
        }

        public virtual IEnumerable<T> FindMulti(GetMultiplePredicate multiCastPredicates)
        {
            using (var db = this.Database.CreateConnection())
            {
                return db.GetMultiple(multiCastPredicates).Read<T>();
            }
        }
    }
}
