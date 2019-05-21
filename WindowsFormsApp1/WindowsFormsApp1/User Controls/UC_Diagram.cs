using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using Npgsql;

namespace WindowsFormsApp1.User_Controls
{
    public partial class UC_Diagram : UserControl
    {
        Func<ChartPoint, string> labelPoint = chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        

        public UC_Diagram()
        {
            InitializeComponent();
        }

        static string conn_param = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=cource";


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

        int GetValueByMonthYear(int month, int year)
        {
            var dataSet = FillDataSetFromDB(string.Format(
                "SELECT\n" +
                "service,\n" +
                "		CASE \n" +
                "		WHEN service='продажа' THEN \n" +
                "			(\n" +
                "			SELECT SUM(cost+price_sell)\n" +
                "			FROM \"квитанции\"\n" +
                "			NATURAL JOIN \"кассеты\"\n" +
                "			NATURAL JOIN \"цены\"\n" +
                "			WHERE EXTRACT(YEAR from date) = {1} \n" +
                "			AND EXTRACT(MONTH from date) = {0}  \n" +
                "			)\n" +
                "		WHEN service='прокат' THEN\n" +
                "		  (\n" +
                "			SELECT SUM(cost+price_rent)\n" +
                "			FROM \"квитанции\"\n" +
                "			NATURAL JOIN \"кассеты\"\n" +
                "			NATURAL JOIN \"цены\"\n" +
                "			WHERE EXTRACT(YEAR from date) = {1} \n" +
                "			AND EXTRACT(MONTH from date) = {0}  \n" +
                "			)\n" +
                "		END as сумма\n" +
                "FROM \"квитанции\"\n" +
                "NATURAL JOIN \"услуги\"\n" +
                "GROUP BY service", month, year)
               );
            return int.Parse(dataSet.Tables[0].Rows[0][1].ToString())
                + int.Parse(dataSet.Tables[0].Rows[1][1].ToString());
        }
        void LoadChart(int year)
        {
            var values = new ChartValues<int>();
            for (int i = 1; i <= 12; i++)
            {
                values.Add(GetValueByMonthYear(i, year));
            }
            cartesianChart1.Series.Clear();
            cartesianChart1.AxisX.Clear();
            cartesianChart1.Series.Add(new LineSeries()
            {
                Values = values,
                Title = "сумма: ",
            });
            cartesianChart1.AxisX.Add(new Axis
            {
                //Title = "месяц",
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = false
                },
                Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }
            });
        }
        private void UC_Diagram_Load(object sender, EventArgs e)
        {
            LoadPie();
            LoadChart(2000);
        }
        void LoadPie()
        {
            var dataSet = FillDataSetFromDB("SELECT caption, sum(cost + price_sell) as summ, COUNT(*)\n" +
               "FROM \n" +
               "\"квитанции\"\n" +
               "NATURAL JOIN \"услуги\"\n" +
               "NATURAL JOIN \"кассеты\"\n" +
               "NATURAL JOIN \"фильмы\"\n" +
               "NATURAL JOIN \"цены\"\n" +
               "WHERE service = 'продажа'\n" +
               "GROUP BY caption\n" +
               "ORDER BY summ DESC\n" +
               "LIMIT 5"
               );
            pieChart1.Series = new SeriesCollection();

            int sum = 0;
            string caption = "";
            for (int i = 0; i < 5; i++)
            {
                sum = int.Parse(dataSet.Tables[0].Rows[i][1].ToString());
                caption = dataSet.Tables[0].Rows[i][0].ToString();
                pieChart1.Series.Add(new PieSeries
                {
                    Title = caption,
                    Values = new ChartValues<int> { sum },
                });
            }
            pieChart1.LegendLocation = LegendLocation.Bottom;
        }

        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int year = int.Parse(textBox5.Text);
                    LoadChart(year);
                }
                catch (Exception)
                {
                    MessageBox.Show("данной даты нет в записях!");
                    return; 
                }
            }
        }
    }
}
