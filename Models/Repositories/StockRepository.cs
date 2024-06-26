using Microsoft.Data.SqlClient;
using System.Data;

namespace Giggle_Garments_MVC.Models.Repositories
{
    public class StockRepository:GenericRepository<Stock>
    {
       static string connectionstr;
       List<Stock> stocks = new List<Stock>();
        public StockRepository(string con):base(connectionstr)
        {
            connectionstr = con;
        }
        public void Add(Stock s)
        {

            base.Add(s);
        }
        public void Update(Stock s)
        {

            base.Update(s);

        }
        public IEnumerable<Stock> GetAll()
        {

            return base.GetAll();
        }
        public Stock GetbyId(int id)
        {

            return base.GetById(id); 
        }


    }
}
