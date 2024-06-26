using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Giggle_Garments_MVC;
using System.Configuration;
using Dapper;


namespace Giggle_Garments_MVC.Models.Repositories
{
    public class CartRepository
    {

        string connectionstr;
        public List<CartItem> cartItems = new List<CartItem>();

        public CartRepository(string con) 
        {
            connectionstr = con;
        }
        public void add(CartItem c)
        {

            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "insert into CartItem (Id,CartId,ProdId,Name,Size,Quantity,Price,Image) values(@id,@cartid,@prodid,@name,@size,@quantity,@price,@image)";
                con.Query<CartItem>(query, c);
            }


        }
        public List<CartItem> getCart(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "select * from CartItem where CartId='" + id + "'";
                return con.Query<CartItem>(query, new { Id = id }).ToList();
            }

        }
        public void RemoveCart(string id)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Delete from CartItem where CartId='" + id + "'";
                con.Execute(query, new { CartId = id });
            }
        }
        public void RemovefromCart(string Id)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Delete from CartItem where Id='" + Id + "'";
                con.Execute(query, new { Id = Id });
            }
           
        }
        public int Getquantity(String Id)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Select Quantity from CartItem where Id = @Id"; // Use parameter
                var cartItem = con.QueryFirst(query, new { Id = Id });
                return cartItem.Quantity;

            }

            
        }
        public int GetCartItemQuantity(string cartid)
        {
            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "Select SUM(Quantity)from CartItem where CartId='" + cartid + "'";
                var q = con.ExecuteScalar(query, new { CartId = cartid });
                return q is DBNull ? 0 : Convert.ToInt32(q);
            }


        }
        public bool Isupdate(string cartId, int prodId, int qty, string size)
        {

            using (SqlConnection con = new SqlConnection(connectionstr))
            {
                string query = "update CartItem set Quantity=Quantity+@qty where CartId='" + cartId + "' AND ProdId='" + prodId + "'AND Size='" + size + "'";
                int c = con.Execute(query, new { CartId = cartId, ProdId = prodId, qty = qty, Size = size });
                if (c == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
           
        }

    }
}
