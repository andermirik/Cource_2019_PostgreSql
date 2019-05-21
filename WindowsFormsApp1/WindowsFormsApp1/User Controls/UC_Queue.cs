using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace WindowsFormsApp1.User_Controls
{
    public partial class UC_Queue : UserControl
    {
        static string conn_param = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=cource";

        public UC_Queue()
        {
            InitializeComponent();
        }

        void FillGridFromDataSet(DataSet dataSet)
        {
            try
            {
                dataGridView1.Columns.Clear();
                for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    var col = dataSet.Tables[0].Columns[i];
                    dataGridView1.Columns.Add(col.ColumnName, col.Caption);
                    dataGridView1.Columns[col.ColumnName].ReadOnly = true;

                }
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    dataGridView1.Rows.Add(dataSet.Tables[0].Rows[i].ItemArray);

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }
                AutoScrollPosition = new Point(0, 0);

            }
            catch (Exception e)
            {

            }
        }

        DataSet FillDataSetFromDB(string sql)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            var select = sql;
            var dataAdapter = new NpgsqlDataAdapter(select, connection);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "SELECT " +
                "id_video as id, " +
                "caption as название," +
                " adress as адресс," +
                " phone as телефон," +
                " license as лицензия," +
                " amount as \"кол-во рабочих\"," +
                " type as \"тип собственности\"," +
                " time_start as \"время начала работы\"," +
                " time_end as \"время окончания работы\"," +
                " fname as имя," +
                " lname as фамилия," +
                " district as район"  +
                " FROM \"видеопрокаты\"" +
                "NATURAL JOIN \"типы\"" +
                "NATURAL JOIN \"время работы\"" +
                "NATURAL JOIN \"владельцы\"" +
                "NATURAL JOIN \"районы\"";

            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //RIGHT JOIN. ВСЕ ФИЛЬМЫ
            string sql = "SELECT \n" +
                "id_cassette,\n" +
                "caption,\n" +
                "quality,\n" +
                "demand\n" +
                "FROM \"кассеты\"\n" +
                "RIGHT JOIN \"качества\" ON \"кассеты\".id_quality = \"качества\".id_quality\n" +
                "RIGHT JOIN \"фильмы\" ON \"кассеты\".id_film = \"фильмы\".id_film";


            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "id_film as id, "+
                "caption as название,\n" +
                "fname as имя,\n" +
                "lname as фамилия,\n" +
                "studio as киностудия,\n" +
                "country as страна,\n" +
                "year as \"год выпуска\",\n" +
                "duration as длительность,\n" +
                "information as информация\n" +
                "FROM фильмы\n" +
                "NATURAL JOIN \"режисёры\"\n" +
                "NATURAL JOIN \"киностудии\"\n" +
                "NATURAL JOIN \"страны\"";

            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "country as страна,\n" +
                "studio as студия\n" +
                "FROM \"страны\"\n" +
                "LEFT JOIN \"киностудии\" ON \"страны\".id_country = \"киностудии\".id_country\n" +
                "ORDER BY studio ASC NULLS FIRST";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "studio as студия, \n" +
                "(\n" +
                "	SELECT\n" +
                "	caption\n" +
                "	FROM \"фильмы\"\n" +
                "	WHERE \"фильмы\".id_studio = \"киностудии\".id_studio\n" +
                "	LIMIT 1\n" +
                ") AS фильм\n" +
                "FROM \"киностудии\"\n" +
                "WHERE (\n" +
                "	SELECT\n" +
                "	caption\n" +
                "	FROM \"фильмы\"\n" +
                "	WHERE \"фильмы\".id_studio = \"киностудии\".id_studio\n" +
                "	LIMIT 1\n" +
                ") is null";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "id_receipt as id, " +
                "service as услуга,\n" +
                "caption as видеопрокат,\n" +
                "CASE WHEN service='прокат' THEN cost + price_rent\n" +
                "     WHEN service='продажа' THEN cost + price_sell\n" +
                "		 ELSE 0\n" +
                "END as ценник,\n" +
                "date as дата\n" +
                "FROM \"квитанции\"  \n" +
                "INNER JOIN \"услуги\"  ON \"квитанции\".id_service = \"услуги\".id_service\n" +
                "INNER JOIN \"видеопрокаты\" ON \"квитанции\".id_video = \"видеопрокаты\".id_video\n" +
                "INNER JOIN \"кассеты\" ON \"квитанции\".id_cassette = \"кассеты\".id_cassette\n" +
                "INNER JOIN \"цены\" ON \"квитанции\".id_price_list = \"цены\".id_price_list";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "id_receipt as id, " +
                "service as услуга,\n" +
                "caption as название,\n" +
                "cost+price_rent as сумма,\n" +
                "date as дата \n" +
                "FROM \"квитанции\"  \n" +
                "INNER JOIN \"услуги\"  ON \"квитанции\".id_service = \"услуги\".id_service\n" +
                "INNER JOIN \"видеопрокаты\" ON \"квитанции\".id_video = \"видеопрокаты\".id_video\n" +
                "INNER JOIN \"кассеты\" ON \"квитанции\".id_cassette = \"кассеты\".id_cassette\n" +
                "INNER JOIN \"цены\" ON \"квитанции\".id_price_list = \"цены\".id_price_list\n" +
                "WHERE date >= now() - interval '1 month'\n" +
                "	AND service = '"+textBox9.Text+"'";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "service as услуга,\n" +
                "caption as видеопрокат,\n" +
                "CASE WHEN service = 'продажа' THEN cost+price_sell\n" +
                "     WHEN service = 'прокат' THEN cost+price_rent\n" +
                "		 ELSE 0\n" +
                "END as сумма,\n" +
                "date as дата \n" +
                "FROM \"квитанции\"  \n" +
                "INNER JOIN \"услуги\"  ON \"квитанции\".id_service = \"услуги\".id_service\n" +
                "INNER JOIN \"видеопрокаты\" ON \"квитанции\".id_video = \"видеопрокаты\".id_video\n" +
                "INNER JOIN \"кассеты\" ON \"квитанции\".id_cassette = \"кассеты\".id_cassette\n" +
                "INNER JOIN \"цены\" ON \"квитанции\".id_price_list = \"цены\".id_price_list\n" +
                "WHERE date >= now() - interval '1 year'\n" +
                "	AND service = '"+textBox8.Text+"'";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string sql = "SELECT " +
                "id_video as id, \n" +
                "caption as название,\n" +
                "adress as адресс,\n" +
                "phone as телефон,\n" +
                "license as лицензия,\n" +
                "time_start as старт,\n" +
                "time_end as конец,\n" +
                "fname as имя,\n" +
                "lname as фамилия,\n" +
                "district as район,\n" +
                "amount as количество\n" +
                "FROM \"видеопрокаты\"\n" +
                "NATURAL JOIN \"время работы\"\n" +
                "NATURAL JOIN \"владельцы\"\n" +
                "NATURAL JOIN \"районы\"\n" +
                "WHERE district = '"+ textBox10.Text+ "'";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string sql = "SELECT \n" +
                "id_film as id, " +
                "caption as название,\n" +
                "fname as имя,\n" +
                "lname as фамилия,\n" +
                "studio as киностудия,\n" +
                "year as год,\n" +
                "duration as длительность,\n" +
                "information as информация\n" +
                "FROM \"фильмы\"\n" +
                "NATURAL JOIN \"режисёры\"\n" +
                "NATURAL JOIN \"киностудии\"\n" +
                "WHERE studio = '"+ textBox11.Text + "'";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "id_receipt as id, " +
                "caption as видеопрокат,\n" +
                "service as услуга,\n" +
                "CASE WHEN service = 'продажа' THEN ROUND(AVG(cost + price_sell),2)\n" +
                "		 WHEN service = 'прокат' THEN ROUND(AVG(cost + price_rent),2)\n" +
                "END as среднее\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"видеопрокаты\"\n" +
                "NATURAL JOIN \"услуги\"\n" +
                "NATURAL JOIN \"кассеты\"\n" +
                "NATURAL JOIN \"цены\"\n" +
                "GROUP BY id_receipt, caption, service";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button18_Click(object sender, EventArgs e)
        {
            string sql = string.Format("SELECT\n" +
                "service as услуга,\n" +
                "CASE WHEN service = 'продажа' THEN ROUND(AVG(cost + price_sell),2)\n" +
                "		 WHEN service = 'прокат' THEN ROUND(AVG(cost + price_rent),2)\n" +
                "END as среднее\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"видеопрокаты\"\n" +
                "NATURAL JOIN \"услуги\"\n" +
                "NATURAL JOIN \"кассеты\"\n" +
                "NATURAL JOIN \"цены\"\n" +
                "WHERE service = '{0}'\n" +
                "GROUP BY service", textBox1.Text);
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox3.Text);
                a = int.Parse(textBox4.Text);
            }
            catch (Exception )
            {
                MessageBox.Show("в обоих ячейках должны быть числа!");
                return;
            }
            string sql = string.Format("SELECT\n" +
                "id_receipt as id, " +
                "caption as видеопрокат,\n" +
                "service as услуга,\n" +
                "CASE WHEN service = 'продажа' THEN ROUND(AVG(cost + price_sell),2)\n" +
                "		 WHEN service = 'прокат' THEN ROUND(AVG(cost + price_rent),2)\n" +
                "END as среднее\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"видеопрокаты\"\n" +
                "NATURAL JOIN \"услуги\"\n" +
                "NATURAL JOIN \"кассеты\"\n" +
                "NATURAL JOIN \"цены\"\n" +
                "GROUP BY id_receipt, caption, service\n" +
                "HAVING\n" +
                "	CASE WHEN service = 'продажа' THEN AVG(cost + price_sell)\n" +
                "			WHEN service = 'прокат' THEN AVG(cost + price_rent)\n" +
                "	END BETWEEN {0} AND {1}", textBox3.Text, textBox4.Text);
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button17_Click(object sender, EventArgs e)
        {

            try
            {
                int a = int.Parse(textBox5.Text);
                a = int.Parse(textBox6.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("в обоих ячейках должны быть числа!");
                return;
            }
            string sql = string.Format(
                "SELECT\n" +
                    "id_receipt as id, " +
                    "caption as видеопрокат,\n" +
                    "service as услуга,\n" +
                    "CASE WHEN service = 'продажа' THEN ROUND(AVG(cost + price_sell),2)\n" +
                    "		 WHEN service = 'прокат' THEN ROUND(AVG(cost + price_rent),2)\n" +
                    "END as среднее\n" +
                    "FROM \"квитанции\"\n" +
                    "NATURAL JOIN \"видеопрокаты\"\n" +
                    "NATURAL JOIN \"услуги\"\n" +
                    "NATURAL JOIN \"кассеты\"\n" +
                    "NATURAL JOIN \"цены\"\n" +
                    "WHERE service = '{0}'\n" +
                    "GROUP BY id_receipt, caption, service\n" +
                    "HAVING\n" +
                    "	CASE WHEN service = 'продажа' THEN AVG(cost + price_sell)\n" +
                    "			WHEN service = 'прокат' THEN AVG(cost + price_rent)\n" +
                    "	END BETWEEN {1} AND {2}", textBox2.Text, textBox5.Text, textBox6.Text
                    );
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string sql = "SELECT DISTINCT " +
                "service as услуга,\n" +
                "		CASE \n" +
                "		WHEN service='продажа' THEN \n" +
                "			(\n" +
                "			SELECT SUM(cost+price_sell)\n" +
                "			FROM \"квитанции\"\n" +
                "			NATURAL JOIN \"кассеты\"\n" +
                "			NATURAL JOIN \"цены\"\n" +
                "			)\n" +
                "		WHEN service='прокат' THEN\n" +
                "		  (\n" +
                "			SELECT SUM(cost+price_rent)\n" +
                "			FROM \"квитанции\"\n" +
                "			NATURAL JOIN \"кассеты\"\n" +
                "			NATURAL JOIN \"цены\"\n" +
                "			)\n" +
                "		END as сумма\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"услуги\"";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "id_owner as id, " +
                "fname  as имя,\n" +
                "lname as фамилия,\n" +
                "(\n" +
                "	SELECT\n" +
                "	COUNT(*)\n" +
                "	FROM \"видеопрокаты\"\n" +
                "	WHERE \"видеопрокаты\".id_owner = \"владельцы\".id_owner\n" +
                ") as количество\n" +
                "FROM \"владельцы\"";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string sql = "SELECT\n" +
                "district as \"район\",\n" +
                "ROUND(AVG(a.c),2) as \"в среднем клиентов на видеопрокат\"\n" +
                "FROM\n" +
                "(SELECT\n" +
                "district,\n" +
                "COUNT(*) as c\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"видеопрокаты\"\n" +
                "NATURAL JOIN \"районы\"\n" +
                "GROUP BY caption, district\n" +
                "ORDER BY district\n" +
                ")as a\n" +
                "GROUP BY a.district";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string sql = "SELECT district,\n" +
                "	ROUND((\n" +
                "		SELECT\n" +
                "		COUNT(*)*100.0\n" +
                "		FROM \"видеопрокаты\"\n" +
                "		NATURAL JOIN \"время работы\"\n" +
                "		NATURAL JOIN \"районы\"\n" +
                "		WHERE \n" +
                "		(\n" +
                "		(time_start >= 22 AND time_end <= 24) OR \n" +
                "		(time_start >= 0 AND time_end <= 6)\n" +
                "		) AND a.district = district\n" +
                "	)\n" +
                "	/\n" +
                "	(\n" +
                "		SELECT\n" +
                "		COUNT(*)*1.0\n" +
                "		FROM \"видеопрокаты\"\n" +
                "		NATURAL JOIN \"время работы\"\n" +
                "		NATURAL JOIN \"районы\"\n" +
                "		WHERE \n" +
                "		a.district = district\n" +
                "	),2) as процент\n" +
                "FROM (\"видеопрокаты\"\n" +
                "NATURAL JOIN \"районы\")as a \n" +
                "GROUP BY district ORDER BY процент DESC";
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox7.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("в ячейке должно быть число!");
                return;
            }
            string sql = string.Format(
                "SELECT\n" +
                "service as услуга,\n" +
                "COUNT(*) as количество,\n" +
                "CASE WHEN service = 'продажа' THEN\n" +
                "			SUM(cost+price_sell)\n" +
                "		 WHEN service = 'прокат' THEN\n" +
                "		  SUM(cost+price_rent)\n" +
                "END as \"общая цена\"\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"услуги\"\n" +
                "NATURAL JOIN \"кассеты\"\n" +
                "NATURAL JOIN \"цены\"\n" +
                "WHERE EXTRACT(year from date)={1}\n" +
                "AND service = '{0}' \n"+
                "GROUP BY service", ServiceBox.Text, textBox7.Text);
            FillGridFromDataSet(FillDataSetFromDB(sql));
        }
    }
}
