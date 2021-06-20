using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary.DAO
{
    public class FrecuencyDAO
    {
        private SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionString);
        private SqlCommand cmd;
        public FrecuencyDAO() { }
        public DataTable GetFrecuency()
        {
            DataTable res = new DataTable();
            cmd = new SqlCommand("Select * from frecuencia", connection)
            {
                CommandType = CommandType.Text
            };
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            connection.Open();
            try
            {
                da.Fill(res);
            }
            catch { }
            connection.Close();

            return res;
        }

        public void UpdateFrecuency(int id)
        {
            cmd = new SqlCommand("update_frecuencia", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            
            connection.Open();
            try
            {
                cmd.BeginExecuteNonQuery();
            }
            catch { }
            connection.Close();
        }
    }
}
