using Dapper;
using Microsoft.Data.SqlClient;
namespace Giggle_Garments_MVC.Models.Repositories
{
    public class CheckoutRepository:GenericRepository<OrderInfo>
    {
        static string connectionstr ;
        public CheckoutRepository(string con):base(connectionstr)
        {
            connectionstr = con;
        }
        public void add(OrderInfo order)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Insert into OrderInfo values(@Id,@orderdate,@fname,@lname,@country,@city,@address,@postalcode,@phone)";
                con.Execute(query, order);
            }
        }
        public OrderInfo GetOrderInfo(int id) 
        {
            return base.GetById(id);
        }
        public List<CartItem> GetOrderedProducts(int id)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Select p.Id as ProdId,p.Price,p.ImagePath as Image,o.Size,o.Quantity  from Product p join OrderedProduct o ON p.Id=o.ProductId  where  o.OrderId=@id";
                return con.Query<CartItem>(query, new { Id = id }).ToList();
            }

        }
        public int GetOrderId()
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Select top 1 Id from  OrderInfo order by Id desc"; 
                var order = con.QueryFirst(query);
                return order.Id;
            }

        }
        public void AddOrderedProduct(List<OrderedProduct> orderedProducts)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Insert into OrderedProduct values(@OrderId,@ProductId,@Size,@Quantity)";
                con.Execute(query, orderedProducts);
            }
        }
    }
}
