using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;

using DapperExtensions;
using DapperExtensions.Sql;

namespace KeLi.HelloDapper.SQLite.Utils
{
    public class DapperHelper
    {
        private readonly SQLiteConnectionStringBuilder builder;

        public DapperHelper(string dbPath)
        {
            if (string.IsNullOrWhiteSpace(dbPath))
                throw new ArgumentNullException(nameof(dbPath));

            builder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbPath
            };
        }

        public dynamic InsertOrUpdate<T>(T entity, Action<T> updater, Func<T, bool> finder) where T : class
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            if (updater is null)
                throw new ArgumentNullException(nameof(updater));

            if (finder is null)
                throw new ArgumentNullException(nameof(finder));

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                var data = connection.GetList<T>();
                var target = data.FirstOrDefault(finder);

                if (target is null)
                    return connection.Insert(entity);

                updater.Invoke(target);

                return connection.Update(target);
            }
        }

        public dynamic Insert<T>(T entity, Func<T, bool> finder = null) where T : class
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                if (finder is null)
                    return connection.Insert(entity);

                var data = connection.GetList<T>();
                var target = data.FirstOrDefault(finder);

                if (target is null)
                    return connection.Insert(entity);

                return null;
            }
        }

        public bool Delete<T>(Func<T, bool> finder) where T : class
        {
            if (finder is null)
                throw new ArgumentNullException(nameof(finder));

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                var target = connection.GetList<T>().FirstOrDefault(finder);

                if (target is null)
                    return false;

                return connection.Delete(target);
            }
        }

        public bool Update<T>(Action<T> updater, Func<T, bool> finder) where T : class
        {
            if (updater is null)
                throw new ArgumentNullException(nameof(updater));

            if (finder is null)
                throw new ArgumentNullException(nameof(finder));

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                var data = connection.GetList<T>();
                var target = data.FirstOrDefault(finder);

                if (target is null)
                    return false;

                updater.Invoke(target);

                return connection.Update(target);
            }
        }

        public T Query<T>(Func<T, bool> finder = null) where T : class
        {
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                var data = connection.GetList<T>();

                if (finder is null)
                    return data.FirstOrDefault();
                
                return data.FirstOrDefault(finder);
            }
        }

        public List<T> QueryList<T>(Func<T, bool> finder = null) where T : class
        {
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();

            using (IDbConnection connection = new SQLiteConnection(builder.ConnectionString))
            {
                connection.Open();

                var data = connection.GetList<T>();

                return finder is null ? data.ToList() : data.Where(finder).ToList();
            }
        }
    }
}