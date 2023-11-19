using Microsoft.Data.Sqlite;

namespace BlazorShortener.Services
{
    public class SqliteManager
    {
        private SqliteConnection Con { get; set; }

        private const string Table_Links_Query = "CREATE TABLE IF NOT EXISTS links (id INTEGER NOT NULL PRIMARY KEY, shorturl varchar, fullurl varchar, time varchar)";
        private const string Table_Access_Query = "CREATE TABLE IF NOT EXISTS accesslist (id INTEGER NOT NULL PRIMARY KEY, shorturl varchar, userip varchar, time varchar)";
        private const string DataSource = "Data Source=BlazorShortener.db";

        private SqliteConnection GetCon()
        {
            Con ??= new(DataSource);
            CreateTables();

            return Con;
        }

        private static void CreateTables()
        {
            using SqliteConnection con = new(DataSource);
            con.Open();

            SqliteCommand cmd = con.CreateCommand();
            cmd.CommandText = Table_Links_Query;

            cmd.ExecuteNonQuery();
            SqliteCommand cmd2 = con.CreateCommand();

            cmd2.CommandText = Table_Access_Query;
            cmd2.ExecuteNonQuery();
        }

        public async Task<string> InsertUrl(string LongInputUrl, string DomainBase)
        {
            string Shortcode = GetShortcode(LongInputUrl);
            if (string.IsNullOrEmpty(Shortcode))
            {
                string TempGuid = Guid.NewGuid().ToString().Split('-')[1];
                Shortcode = TempGuid.Split('-')[0];

                using SqliteConnection con = GetCon();
                con.Open();

                SqliteCommand cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO links (shorturl, fullurl, time) VALUES (@shorturl, @fullurl, @time)";

                cmd.Parameters.AddWithValue("@shorturl", Shortcode);
                cmd.Parameters.AddWithValue("@fullurl", LongInputUrl);

                cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                await cmd.ExecuteNonQueryAsync();
            }

            return $"{DomainBase}{Shortcode}";
        }

        public async Task ShortUrlAccessed(string shorturl, string userip)
        {
            using SqliteConnection con = GetCon();
            con.Open();

            SqliteCommand cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO accesslist (shorturl, userip, time) VALUES (@shorturl, @userip, @time)";

            cmd.Parameters.AddWithValue("@shorturl", shorturl);
            if (string.IsNullOrEmpty(userip))
            {
                cmd.Parameters.AddWithValue("@userip", "FEHLER");
            }
            else
            {
                cmd.Parameters.AddWithValue("@userip", userip);
            }

            cmd.Parameters.AddWithValue("@time", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            await cmd.ExecuteNonQueryAsync();
        }

        private string GetShortcode(string LongInputUrl)
        {
            using SqliteConnection con = GetCon();
            con.Open();

            SqliteCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT shorturl FROM links WHERE fullurl=@longurl";

            cmd.Parameters.AddWithValue("@longurl", LongInputUrl);
            using SqliteDataReader reader = cmd.ExecuteReader();

            string Shortcode = string.Empty;

            while (reader.Read())
            {
                Shortcode = reader.GetString(0);
            }

            return Shortcode;
        }

        public string GetLongurl(string shorturl)
        {
            using SqliteConnection con = GetCon();
            con.Open();

            SqliteCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT fullurl FROM links WHERE shorturl = @shorturl";

            cmd.Parameters.AddWithValue("@shorturl", shorturl);
            using SqliteDataReader reader = cmd.ExecuteReader();

            string Longurl = string.Empty;

            while (reader.Read())
            {
                Longurl = reader.GetString(0);
            }

            return Longurl;
        }

    }
}
