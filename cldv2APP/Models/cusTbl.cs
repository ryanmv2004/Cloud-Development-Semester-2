using Microsoft.Data.SqlClient;

namespace cldv2APP.Models
{
    public class cusTbl
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string city { get; set; }

        public static string con_string = "Server=tcp:st1036cldvpoedb.database.windows.net,1433;Initial Catalog=st1036cldvpoe;Persist Security Info=False;User ID=ryanv2004;Password=AceVents12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static SqlConnection con = new SqlConnection(con_string);
        public static string con_string_redundant = "Server=tcp:cldvpoeredundantst1036.database.windows.net,1433;Initial Catalog=cldvpoeredundant;Persist Security Info=False;User ID=ryanv2004;Password=AceVents12;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public static SqlConnection con_redundant = new SqlConnection(con_string_redundant);

        public int insert_Customer(cusTbl c)
        {
            try
            {
                string sql = "INSERT INTO cusTbl (name, email, password, city) VALUES (@name, @email, @password, @city)";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlCommand cmd_redundant = new SqlCommand(sql, con_redundant);

                cmd.Parameters.AddWithValue("@name", c.name);
                cmd.Parameters.AddWithValue("@email", c.email);
                cmd.Parameters.AddWithValue("@password", c.password);
                cmd.Parameters.AddWithValue("@city", c.city);

                cmd_redundant.Parameters.AddWithValue("@name", c.name);
                cmd_redundant.Parameters.AddWithValue("@email", c.email);
                cmd_redundant.Parameters.AddWithValue("@password", c.password);
                cmd_redundant.Parameters.AddWithValue("@city", c.city);

                con.Open();
                con_redundant.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                int rowsAffected1 = cmd_redundant.ExecuteNonQuery();
                con.Close();
                con_redundant.Close();
                return rowsAffected;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return -1;
            }
        }

    }
}
