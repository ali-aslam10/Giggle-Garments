using Giggle_Garments_MVC;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Giggle_Garments_MVC.Models.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly string _connectionString;

        public GenericRepository(string connectionString)
        {
            _connectionString = "Server=(localdb)\\mssqllocaldb;Database=GiggleGarmentsDB;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public void Add(TEntity entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(TEntity).Name;
                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != "Id");

                var columnNames = string.Join(",", properties.Select(p => p.Name));
                var parameterNames = string.Join(",", properties.Select(p => "@" + p.Name));

                var query = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});";
                connection.Execute(query, entity);

                
            }
        }

        public TEntity GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(TEntity).Name;
                var primaryKey = "Id";

                var query = $"SELECT * FROM {tableName} WHERE {primaryKey} = @Id;";

                var result = connection.Query<TEntity>(query, new { Id = id }).FirstOrDefault();

                // Check if the result is null and throw an exception if necessary
                if (result == null)
                {
                    throw new KeyNotFoundException($"Entity of type {typeof(TEntity).Name} with ID {id} was not found.");
                }

                return result;

                
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(TEntity).Name;

                var query = $"SELECT * FROM {tableName};";
                return connection.Query<TEntity>(query).ToList();

                
            }
        }

        public void Update(TEntity entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(TEntity).Name;
                var primaryKey = "Id";

                var properties = typeof(TEntity).GetProperties().Where(p => p.Name != primaryKey);

                var setClause = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));
                var query = $"UPDATE {tableName} SET {setClause} WHERE {primaryKey} = @{primaryKey};";

                connection.Execute(query, entity);

               
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var tableName = typeof(TEntity).Name;
                var primaryKey = "Id";

                var query = $"DELETE FROM {tableName} WHERE {primaryKey} = @Id;";
                connection.Execute(query, new { Id = id });
                
            }
        }

        

    }
}
