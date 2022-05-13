using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Zad5.Model
{
    public class WarehouseProcedureRepository : IWarehouseProcedureRepository
    {
        private readonly string _DbConnectionString = "Data Source=DESKTOP-6R143CT;Initial Catalog=mydatabase;Integrated Security=True";
        public bool addWarehouse(WarehouseRequest warehouseRequest)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_DbConnectionString))
                {
                    using (SqlCommand com = new SqlCommand("AddProductToWarehouse"))
                    {
                        com.Connection = con;
                        con.Open();
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.Add("@IdProduct", SqlDbType.Int).Value = warehouseRequest.IdProduct;
                        com.Parameters.Add("@IdWarehouse", SqlDbType.Int).Value = warehouseRequest.IdWarehouse;
                        com.Parameters.Add("@Amount", SqlDbType.Int).Value = warehouseRequest.Amount;
                        com.Parameters.Add("@CreatedAt", SqlDbType.DateTime).Value = warehouseRequest.CreatedAt;
                        com.ExecuteNonQuery();
                        return true;
                    }
                }
            } catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
