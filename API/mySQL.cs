using MySql.Data.MySqlClient;

namespace StarGame
{
    public static class mySQL
    {
        public static string cs { get; } = @"server=localhost;userid=root;password=;database=stargame;charset=utf8;";

        public static MySqlConnection mySqlOpen()
        {
            MySqlConnection conn = new MySqlConnection(cs);
            conn.Open();
            return conn;
        }
    }
}
