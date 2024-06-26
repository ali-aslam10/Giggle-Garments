using System;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using Microsoft.Data.SqlClient;
using NuGet.Common;
using static Dapper.SqlMapper;
namespace Giggle_Garments_MVC.Models.Repositories
{

    public class productRepository:GenericRepository<Product>
    {
        
        static string connectionstr;
        public List<Product> products = new List<Product>();

        public productRepository(string con):base(connectionstr)
        {
            connectionstr = con;
        }
        public void Add(Product p)
        {
            
            base.Add(p);
        }

        public IEnumerable<Product> GetAll()
        {
            
            return base.GetAll();
        }
        public Product GetById(int id)
        {
          
            return base.GetById(id);
        }

        public List<Product> getbycategoryId(int id)
        {

            using (var connection = new SqlConnection(connectionstr))
            {
                connection.Open();
                var query = "select * from Product where CategoryId=@Id";
                return connection.Query<Product>(query,new {Id=id}).ToList();
            }

        }

        public List<Product> getNewIn()
        {

            using (var connection = new SqlConnection(connectionstr))
            {
                connection.Open();
                var query = "select top 12 * from Product  order by Id desc";
                return connection.Query<Product>(query).ToList();
            }
        }
        public void Update(Product p)
        {

            base.Update(p);

        }
        public void Delete(int id)
        {
            //First delete Stock
            SqlConnection con = new SqlConnection(connectionstr);
            con.Open(); 
            string query = "Delete from Stock where Id='" + id + "'";
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.ExecuteNonQuery();

            base.Delete(id);


        }
        public List<Product> Search(string searchitem)
        {

            using (var connection = new SqlConnection(connectionstr))
            {
                // Open the connection
                connection.Open();

                // Prepare the query
                var query = "SELECT * FROM Product WHERE Name LIKE @searchPattern OR Id = @searchId";

                // Prepare parameters
                var parameters = new
                {
                    searchPattern = $"%{searchitem}%",
                    searchId = int.TryParse(searchitem, out int id) ? (int?)id : null
                };

                // Execute the query and return the results
                return connection.Query<Product>(query, parameters).ToList();
            }
        }

    }
   



}
