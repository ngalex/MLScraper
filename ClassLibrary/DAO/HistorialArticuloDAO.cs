using ClassLibrary.Model;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary.DAO
{
    public class HistorialArticuloDAO
    {
        private SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionString);
        private SqlCommand cmd;
        private HistorialArticulo ha;
        public HistorialArticuloDAO() { }
        public ArrayList GetHistorialArticulos(int cod)
        {
            DataTable res = new DataTable();
            ArrayList hArticulos = new ArrayList();

            cmd = new SqlCommand("get_historial_articulo", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = cod;
            
            connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            foreach (DataRow item in res.Rows)
            {
                ha = new HistorialArticulo();
                ha.Id = Int32.Parse(item["Id"].ToString());
                ha.Code = item["cod"].ToString();
                ha.Dt = DateTime.Parse(item["fecha"].ToString());
                ha.Price = float.Parse(item["price"].ToString());
                ha.Status = item["status"].ToString();
                hArticulos.Add(ha);
            }

            connection.Close();
            return hArticulos;
        }

        public void InsertHistorialArticulo(HistorialArticulo ha)
        {
            SqlCommand cmd = new SqlCommand("insert_historial_articulo", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = int.Parse(ha.Code);
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = ha.Dt;
            cmd.Parameters.Add("@price", SqlDbType.Real).Value = ha.Price;
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = ha.Status;
            connection.Open();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("INSERT ERROR: " + ha.Code + " "+ e.Message);
            }
            connection.Close();
        }
    }
}
