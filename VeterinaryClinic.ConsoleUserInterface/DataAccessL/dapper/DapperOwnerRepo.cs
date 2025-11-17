using Dapper;
using DataAccessL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dto.Essence;

namespace DataAccessL.dapper
{
    public class DapperOwnerRepo : IRepository<OwnerDto>
    {
        static string connectionString = "data source=(localhost)\\SQLLocalDB;Initial Catalog=DbConnection;Integrated Security=True";
        IDbConnection db = new SqlConnection(connectionString);

        public void Create(OwnerDto ownerdto)
        {
            var sqlQuery = "INSERT INTO Owners (Id, FullName, PhoneNumber) " +
                           "VALUES(@Id, @FullName, @PhoneNumber)";
            db.Execute(sqlQuery, ownerdto);
        }

        public void Add(OwnerDto item)
        {
            Create(item);
        }

        public void Delete(Guid id)
        {
            var sqlQuery = "DELETE FROM Owners WHERE Id = @id";
            db.Execute(sqlQuery, new { id });
        }

        public void Dispose()
        {
            db?.Dispose();
        }

        public IEnumerable<OwnerDto> GetAll()
        {
            return db.Query<OwnerDto>("SELECT * FROM Owners").ToList();
        }

        public OwnerDto GetById(Guid id)
        {
            return db.Query<OwnerDto>("SELECT * FROM Owners WHERE Id = @id", new { id }).FirstOrDefault();
        }

        public void Save()
        {
        }

        public void Update(OwnerDto item)
        {
            var sqlQuery = "UPDATE Owners SET FullName = @FullName, PhoneNumber = @PhoneNumber WHERE Id = @Id";
            db.Execute(sqlQuery, item);
        }
    }
}