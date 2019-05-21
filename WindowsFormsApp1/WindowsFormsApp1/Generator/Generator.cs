using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bogus;
using Npgsql;
namespace WindowsFormsApp1.Generator
{
    public class Generator
    {
        static string conn_param = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=cource";

        public Generator()
        {
            //GenerateQualities(5);
            //GenerateTypes(5);
            //GenerateTimes(10);
            //GeneratePrices(10);
            //GenerateDistricts(20);
            //GenerateCountries(40);
            //GenerateServices(5);
            //GenerateOwners(200);
            //GenerateProducers(200);
            //GenerateStudios(200);
            //GenerateFilms(400);
            //GenerateVideos(160);
            //GenerateKassettes(800);
            //GenerateReceipts(19000);
        }

        public static void GenerateReceipts(int count)
        {
            Faker faker = new Faker("ru");
            Random rand = new Random();

            var services_id = getReferenceKeys("услуги");
            var videos_id = getReferenceKeys("видеопрокаты");
            var cassettes_id = getReferenceKeys("кассеты");
            var prices_id = getReferenceKeys("цены");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into квитанции" +
                        "(id_service, date, id_video, id_cassette, id_price_list)" +
                        " values({0}, '{1}', {2}, {3}, {4})",
                        services_id[rand.Next(0, services_id.Count - 1)],
                        faker.Date.Between(new DateTime(1985, 1, 1), DateTime.Now),
                        videos_id[rand.Next(0, videos_id.Count - 1)],
                        cassettes_id[rand.Next(0, cassettes_id.Count - 1)],
                        prices_id[rand.Next(0, prices_id.Count - 1)]
                        );
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateKassettes(int count)
        {
            Faker faker = new Faker("ru");
            Random rand = new Random();

            var quality_id = getReferenceKeys("качества");
            var film_id = getReferenceKeys("фильмы");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into кассеты" +
                        "(id_quality, photo, cost, demand, id_film)" +
                        " values({0}, null, {1}, '{2}', {3})",
                        quality_id[rand.Next(0, quality_id.Count - 1)],
                        rand.Next(25, 76),
                        faker.Random.Bool(),
                        film_id[rand.Next(0, film_id.Count - 1)]
                        );
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateVideos(int count)
        {
            Faker faker = new Faker("ru");
            Random rand = new Random();

            var type_id = getReferenceKeys("типы");
            var time_id = getReferenceKeys("\"время работы\"");
            var own_id = getReferenceKeys("владельцы");
            var distr_id = getReferenceKeys("районы");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into видеопрокаты" +
                        "(caption, adress, id_type, phone, license, id_time, amount, id_owner, id_district)" +
                        " values('{0}', '{1}', {2}, '{3}', '{4}', {5}, {6}, {7}, {8})",
                        faker.Lorem.Sentence(2),
                        faker.Address.StreetAddress(),
                        type_id[rand.Next(0, type_id.Count - 1)],
                        faker.Phone.PhoneNumber("+38(071)###-####"),
                        faker.Random.String(12, '0', '9'),
                        time_id[rand.Next(0, time_id.Count - 1)],
                        rand.Next(100, 600),
                        own_id[rand.Next(0, own_id.Count - 1)],
                        distr_id[rand.Next(0, distr_id.Count - 1)]);
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateFilms(int count)
        {
            Faker faker = new Faker("ru");
            Random rand = new Random();

            var stud_id = getReferenceKeys("киностудии");
            var prod_id = getReferenceKeys("режисёры");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into фильмы(caption, id_producer, id_studio," +
                        " year, duration, information)" +
                        " values('{0}', {1}, {2}, {3}, {4}, '{5}')",
                        faker.Lorem.Sentence(3),
                        prod_id[rand.Next(0, prod_id.Count - 1)],
                        stud_id[rand.Next(0, stud_id.Count - 1)],
                        rand.Next(1980, DateTime.Now.Year),
                        rand.Next(0, 480), faker.Lorem.Sentence());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }
        
        public static void GenerateStudios(int count)
        {
            Faker faker = new Faker("ru");
            Random rand = new Random();

            var countries_id = getReferenceKeys("страны");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into киностудии(studio, id_country)" +
                    " values('{0}', {1})",
                    faker.Company.CompanyName(), countries_id[rand.Next(0, countries_id.Count-1)]);
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static List<int> getReferenceKeys(string table)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            var select = string.Format("SELECT * FROM {0}", table);
            var dataAdapter = new NpgsqlDataAdapter(select, connection);
            var dataSet = new System.Data.DataTable();
            dataAdapter.Fill(dataSet);
            connection.Close();
            List<int> list = new List<int>();
            for (int i = 0; i < dataSet.Rows.Count; i++)
                list.Add((int)dataSet.Rows[i].ItemArray[0]);
            return list;
        }

        public static void GenerateServices(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into услуги(service)" +
                    " values('{0}')",
                    faker.Lorem.Word());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateQualities(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into качества(quality)" +
                    " values('{0}')",
                    faker.Commerce.ProductMaterial());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateCountries(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into страны(country)" +
                    " values('{0}')",
                    faker.Address.Country());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateDistricts(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into районы(district)" +
                    " values('{0}')",
                    faker.Address.State());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateProducers(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into режисёры(fname, lname, pname)" +
                    " values('{0}', '{1}', '{2}')",
                    faker.Name.FirstName(), faker.Name.LastName(), faker.Name.FirstName());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateOwners(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into владельцы(fname, lname, pname)" +
                    " values('{0}', '{1}', '{2}')",
                    faker.Name.FirstName(), faker.Name.LastName(), faker.Name.FirstName());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateTypes(int count)
        {
            Faker faker = new Faker("ru");

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                string sql = string.Format("insert into типы(type) values('{0}')",
                    faker.Commerce.ProductAdjective());
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GenerateTimes(int count)
        {
            Random rand = new Random();
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                int a = rand.Next(0, 24);
                int b = rand.Next(a, 24);
                string sql = string.Format("insert into \"время работы\"(time_start, time_end)" +
                    " values({0}, {1})", a, b);
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }

        public static void GeneratePrices(int count)
        {
            Random rand = new Random();
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            for (int i = 0; i < count; i++)
            {
                int a = rand.Next(0, 100);
                int b = rand.Next(a, 100);
                string sql = string.Format("insert into цены(price_sell, price_rent)" +
                    " values({0}, {1})", b, a);
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}
