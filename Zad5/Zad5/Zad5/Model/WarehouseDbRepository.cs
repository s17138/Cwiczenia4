using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Zad5.Model
{
    public class WarehouseDbRepository : IWarehouseDbRepository
    {
        private readonly string _DbConnectionString = "Data Source=DESKTOP-6R143CT;Initial Catalog=mydatabase;Integrated Security=True";

        public async Task<int> addWarehouse(WarehouseRequest warehouseRequest)
        {
            try
            {
                if ((await CheckIfProductExists(warehouseRequest.IdProduct)) && (await CheckIfWarehousetExists(warehouseRequest.IdWarehouse)) && warehouseRequest.Amount > 0)
                {
                    int? orderId = await GetOrderId(warehouseRequest.IdProduct, warehouseRequest.IdWarehouse, warehouseRequest.CreatedAt);
                    if (orderId == null)
                    {
                        return -1;
                    } else if (await CheckIfOrderHasBeenRealized(orderId))
                    {
                        return -2;
                    } else
                    {
                        await UpdateOrder(orderId);
                        int? productPrice = await GetProductPrice(warehouseRequest.IdProduct);
                        int resultId = await CreateProductWarehouse(warehouseRequest.IdProduct, orderId, warehouseRequest.Amount, productPrice);
                        return resultId;
                    }
                }
                else
                {
                    return -3;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -4;
            }
        }

        private async Task<bool> CheckIfProductExists(int? productId)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "Select top 1 * from Product where IdProduct=@id";
                    com.Parameters.Add("@id", SqlDbType.Int).Value = productId;
                    SqlDataReader dr = await com.ExecuteReaderAsync();
                    return dr.HasRows;
                }
            }
        }
        private async Task<bool> CheckIfWarehousetExists(int? warehouseId)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "Select top 1 * from Warehouse where IdWarehouse=@id";
                    com.Parameters.Add("@id", SqlDbType.Int).Value = warehouseId;
                    SqlDataReader dr = await com.ExecuteReaderAsync();
                    return dr.HasRows;
                }
            }
        }

        private async Task<int?> GetOrderId(int? idProduct, int? idAmount, DateTime createdAt)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = " SELECT top 1 IdOrder FROM \"Order\" " +
                        "WHERE IdProduct = @IdProduct AND Amount = @Amount AND FulfilledAt IS NULL " +
                        "AND CreatedAt < @CreatedAt;";
                    com.Parameters.Add("@IdProduct", SqlDbType.Int).Value = idProduct;
                    com.Parameters.Add("@Amount", SqlDbType.Int).Value = idAmount;
                    com.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = createdAt;
                    SqlDataReader dr = await com.ExecuteReaderAsync();
                    var hasRows = dr.HasRows;
                    if (!hasRows)
                    {
                        return null;
                    } else
                    {
                        await dr.ReadAsync();
                        int result = int.Parse(dr["IdOrder"].ToString());
                        return result;
                    }
                }
            }
        }

        private async Task<bool> CheckIfOrderHasBeenRealized(int? orderId)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "Select top 1 * from Product_Warehouse where IdOrder=@IdOrder";
                    com.Parameters.Add("@IdOrder", SqlDbType.Int).Value = orderId;
                    SqlDataReader dr = await com.ExecuteReaderAsync();
                    return dr.HasRows;
                }
            }
        }

        private async Task<bool> UpdateOrder(int? orderId)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "Update Order set FulfilledAt = @FulfilledAt where IdOrder=@IdOrder";
                    com.Parameters.Add("@IdOrder", SqlDbType.Int).Value = orderId;
                    com.Parameters.Add("@FulfilledAt", SqlDbType.DateTime).Value = DateTime.Now;
                    int rowsAffected = await com.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
        private async Task<int?> GetProductPrice(int? productId)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
                    com.Parameters.Add("@IdProduct", SqlDbType.Int).Value = productId;
                    int rowsAffected = await com.ExecuteNonQueryAsync();
                    SqlDataReader dr = await com.ExecuteReaderAsync();
                    if (!dr.HasRows)
                    {
                        return null;
                    }
                    else
                    {
                        await dr.ReadAsync();
                        int result = int.Parse(dr["Price"].ToString());
                        return result;
                    }
                }
            }
        }

        private async Task<int> CreateProductWarehouse(int? idProduct, int? idOrder, int? amount, int? productPrice)
        {
            using (SqlConnection con = new SqlConnection(_DbConnectionString))
            {
                using (SqlCommand com = new SqlCommand())
                {
                    com.Connection = con;
                    con.Open();
                    com.CommandType = CommandType.Text;
                    com.CommandText =
                       // "SET IDENTITY_INSERT Product_Warehouse ON; " +
                        "Insert into Product_Warehouse(IdProduct, IdOrder, Amount, Price, CreatedAt) values(@IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
                    com.Parameters.Add("@IdProduct", SqlDbType.Int).Value = idProduct;
                    com.Parameters.Add("@IdOrder", SqlDbType.Int).Value = idOrder;
                    com.Parameters.Add("@Price", SqlDbType.Int).Value = amount * productPrice;
                    com.Parameters.Add("@Amount", SqlDbType.Int).Value = amount;
                    com.Parameters.Add("@CreatedAt", SqlDbType.Int).Value = DateTime.Now;
                    int resultId = (int)await com.ExecuteScalarAsync();
                    return resultId;
                }
            }
        }
    }
}
