using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary.Model;

namespace ClassLibrary.DAO
{
    public class ArticuloDAO
    {

        private SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionString);
        private SqlCommand cmd;
        private Articulo art;
        public ArticuloDAO(){}
        public ArrayList GetArticulos()
        {
            DataTable res = new DataTable();
            float diff = 0;
            cmd = new SqlCommand("get_articulos", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            connection.Open();
            try
            {
                da.Fill(res);
            } catch (Exception ex){ Console.WriteLine("GET ERROR: " + ex.Message); }
            connection.Close();


            ArrayList articulos = new ArrayList();
            foreach (DataRow item in res.Rows)
            {
                art = new Articulo();
                art.Code = item["cod"].ToString();
                art.Name = item["name"].ToString();
                art.Price = float.Parse(item["price"].ToString());
                art.Url = item["url"].ToString();
                art.Image = item["img"].ToString();
                art.Status = item["status"].ToString();
                art.Diff = 0;
                cmd = new SqlCommand("SELECT dbo.diferencia(@cod)", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@cod", SqlDbType.Int).Value = int.Parse(art.Code);
                connection.Open();
                try
                {
                    diff = float.Parse(cmd.ExecuteScalar().ToString());
                    //Console.WriteLine("here" + cmd.ExecuteScalar().ToString());
                }
                catch (Exception ex){
                    Console.WriteLine("DIFF ERROR: " + ex.Message);
                }
                connection.Close();
                if (diff != 0) art.Diff = diff;
                articulos.Add(art);
            }
            return articulos;
        }

        public Articulo GetArticulo(string code)
        {
            DataTable res = new DataTable();

            cmd = new SqlCommand("get_articulo", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = int.Parse(code);
            connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(res);
            } catch (Exception ex)
            {
                Console.WriteLine("SOLOGET ERROR: " + ex.Message);
            }
            connection.Close();
            art = new Articulo();
            art.Code = res.Rows[0]["cod"].ToString();
            art.Name = res.Rows[0]["name"].ToString();
            art.Price = float.Parse(res.Rows[0]["price"].ToString());
            art.Url = res.Rows[0]["url"].ToString();
            art.Image = res.Rows[0]["img"].ToString();
            art.Status = res.Rows[0]["status"].ToString();

            return art;
        }

        public void insertArticulo(Articulo articulo)
        {
            SqlCommand cmd = new SqlCommand("insert_articulo", connection);
            
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = articulo.Name;
            cmd.Parameters.Add("@url", SqlDbType.VarChar).Value = articulo.Url;
            cmd.Parameters.Add("@price", SqlDbType.Real).Value = articulo.Price;
            cmd.Parameters.Add("@img", SqlDbType.VarChar).Value = articulo.Image;
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = articulo.Status;
            connection.Open();

            try
            {
                cmd.ExecuteNonQuery();
            } catch (Exception e)
            {
                Console.WriteLine("INSERT ERROR: " + e.Message);
            }
            connection.Close();
        }

        public void updateArticulo(Articulo articulo)
        {
            SqlCommand cmd = new SqlCommand("update_articulo", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = articulo.Name;
            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = int.Parse(articulo.Code);
            cmd.Parameters.Add("@price", SqlDbType.Real).Value = articulo.Price;
            cmd.Parameters.Add("@img", SqlDbType.VarChar).Value = articulo.Image;
            cmd.Parameters.Add("@status", SqlDbType.VarChar).Value = articulo.Status;
            connection.Open();

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("INSERT ERROR: " + e.Message);
            }
            connection.Close();

        }

        public void deleteArticulo(int cod)
        {
            cmd = new SqlCommand("delete_articulo", connection);
                
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = cod;
            connection.Open();

            try
            {
                cmd.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                Console.WriteLine("DELETE ERROR: " + ex.Message);
            }
            connection.Close();
        }
    }

}
