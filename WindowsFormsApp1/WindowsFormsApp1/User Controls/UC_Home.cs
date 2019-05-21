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
    public partial class UC_Home : UserControl
    {
        bool isOpen = false;
        UC_DropDownList DT;
        Form1 form1;

        string search = "";

        static string conn_param = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=cource";
        string table = "";
        string table_row="";

        public UC_Home(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            DT = new UC_DropDownList(this.form1);
            DT.Dock = DockStyle.Fill;

            panelDropDown.Controls.Clear();
            panelDropDown.Controls.Add(DT);
            onClickDropDown(btnTable.Text);
        }

        DataSet FillDataSetFromDB(string fields, string table, string orderby, string where = "1=1")
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            var select = string.Format("SELECT {4} FROM {0} WHERE {5} order by {3} limit {2} offset {1}",
                table, 1 * 50, (1 + 1) * 50, orderby, fields, where);
            var dataAdapter = new NpgsqlDataAdapter(select, connection);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        void FillGridFromDataSet(DataSet dataSet)
        {
            try
            {
                dataGridView1.Columns.Clear();
                for (int i = 0; i < dataSet.Tables[0].Columns.Count; i++)
                {
                    var col = dataSet.Tables[0].Columns[i];
                    col.Caption = FindInSinonyms(col.ColumnName);
                    dataGridView1.Columns.Add(col.ColumnName, col.Caption);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void onClickDropDown(string btn)
        {
            btnTable.Text = btn;
            btn = btn.Trim().ToLower();

            if (btn == "районы")
            {
                var ds = FillDataSetFromDB("*", btn, "id_district", "district like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "районы";
                table_row = "районы";
            }
            if (btn == "качества кассет")
            {
                var ds = FillDataSetFromDB("*", "качества", "id_quality", "quality like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "качества";
                table_row = "качества";
            }
            if (btn == "типы собственности")
            {
                var ds = FillDataSetFromDB("*", "типы", "id_type", "type like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "типы";
                table_row = "типы";
            }
            if (btn == "услуги проката")
            {
                var ds = FillDataSetFromDB("*", "услуги", "id_service", "service like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "услуги";
                table_row = "услуги";
            }
            if (btn == "страны")
            {
                var ds = FillDataSetFromDB("*", "страны", "id_country", "country like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "страны";
                table_row = "страны";
            }
            if (btn == "режиссёры")
            {
                var ds = FillDataSetFromDB("*", "режисёры", "id_producer", "lname like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "режисёры";
                table_row = "режисёры";
            }
            if (btn == "владельцы видеопрокатов")
            {
                var ds = FillDataSetFromDB("*", "владельцы", "id_owner", "lname like '" + search + "%'");
                FillGridFromDataSet(ds);
                table = "владельцы";
                table_row = "владельцы";
            }
            if (btn == "время работы видеопрокатов")
            {
                var ds = FillDataSetFromDB("*", "\"время работы\"", "id_time");
                FillGridFromDataSet(ds);
                table = "\"время работы\"";
                table_row = "\"время работы\"";
            }
            if (btn == "ценники видеопрокатов")
            {
                var ds = FillDataSetFromDB("*", "цены", "id_price_list");
                FillGridFromDataSet(ds);
                table = "цены";
                table_row = "цены";
            }
            if (btn == "киностудии")
            {
                var dataSet = FillDataSetFromDB("*", "киностудии", "id_studio", "studio like '" + search + "%'");
                FillGridFromDataSet(dataSet);

                dataGridView1.Columns["id_country"].Visible = false;

                dataGridView1.Columns.Add("country", "Страна");
                table = "киностудии NATURAL JOIN страны";
                table_row = "киностудии";
            }
            if (btn == "фильмы")
            {
                var dataSet = FillDataSetFromDB("*", "фильмы", "id_film", "caption like '" + search + "%'");
                FillGridFromDataSet(dataSet);
                dataGridView1.Columns["id_producer"].Visible = false;
                dataGridView1.Columns["id_studio"].Visible = false;

                dataGridView1.Columns.Add("studio", "Киностудия");
                table = "фильмы NATURAL JOIN киностудии";
                table_row = "фильмы";
            }
            if (btn == "квитанции")
            {
                var dataSet = FillDataSetFromDB("id_receipt,id_service,date,id_video,id_cassette,id_price_list ",
                    "квитанции NATURAL JOIN услуги NATURAL JOIN кассеты NATURAL JOIN цены", "id_receipt");
                FillGridFromDataSet(dataSet);

                dataGridView1.Columns["id_service"].Visible = false;
                dataGridView1.Columns["id_video"].Visible = false;
                dataGridView1.Columns["id_cassette"].Visible = false;
                dataGridView1.Columns["id_price_list"].Visible = false;

                dataGridView1.Columns.Add("service", "Услуга");
                dataGridView1.Columns.Add("vcaption", "Видеопрокат");
                table = "квитанции NATURAL JOIN услуги NATURAL JOIN видеопрокаты";
                table_row = "квитанции";
            }
            if (btn == "кассеты")
            {
                var dataSet = FillDataSetFromDB("*", "кассеты", "id_cassette");
                FillGridFromDataSet(dataSet);

                dataGridView1.Columns["id_quality"].Visible = false;
                dataGridView1.Columns["id_film"].Visible = false;
                dataGridView1.Columns["demand"].Visible = false;
                dataGridView1.Columns["photo"].Visible = false;

                dataGridView1.Columns.Add("fcaption", "Фильм");
                dataGridView1.Columns.Add("quality", "Качество");
                table = "кассеты NATURAL JOIN фильмы NATURAL JOIN качества";
                table_row = "кассеты";
            }
            if (btn == "видеопрокаты")
            {
                var dataSet = FillDataSetFromDB("*", "видеопрокаты", "id_video", "caption like '" + search + "%'");
                FillGridFromDataSet(dataSet);

                dataGridView1.Columns["id_type"].Visible = false;
                dataGridView1.Columns["id_time"].Visible = false;
                dataGridView1.Columns["id_owner"].Visible = false;
                dataGridView1.Columns["id_district"].Visible = false;

                dataGridView1.Columns.Add("time_start", "Время начала");
                dataGridView1.Columns.Add("time_end", "Время конца");
                dataGridView1.Columns.Add("type", "Тип собст.");
                dataGridView1.Columns.Add("fname", "Имя");
                dataGridView1.Columns.Add("lname", "Фамилия");
                dataGridView1.Columns.Add("district", "Район");
                table = "видеопрокаты NATURAL JOIN \"время работы\" NATURAL JOIN типы NATURAL JOIN владельцы NATURAL JOIN районы";
                table_row = "видеопрокаты";
            }
            dataGridView1.Rows.Add();
        }

        public void closeDropDownMenu()
        {
            panelDropDown.Height = 0;
            isOpen = false;
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            timerDropDown.Start();
        }

        private void timerDropDown_Tick(object sender, EventArgs e)
        {
            if (isOpen)
            {
                panelDropDown.Height -= 30;
                if (panelDropDown.Height <= 0)
                {
                    timerDropDown.Stop();
                    isOpen = false;
                }
            }
            else if (!isOpen)
            {
                panelDropDown.Height += 30;
                if (panelDropDown.Height >= 300)
                {
                    timerDropDown.Stop();
                    isOpen = true;
                }
            }
        }

        static List<KeyValuePair<string, string>> synonyms = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("id_type", "Номер"),
            new KeyValuePair<string, string>("type", "Тип"),

            new KeyValuePair<string, string>("id_time", "Номер"),
            new KeyValuePair<string, string>("time_start", "Время начала"),
            new KeyValuePair<string, string>("time_end", "Время окончания"),

            new KeyValuePair<string, string>("id_owner", "Номер"),
            new KeyValuePair<string, string>("fname", "Имя"),
            new KeyValuePair<string, string>("lname", "Фамилия"),
            new KeyValuePair<string, string>("pname", "Отчество"),

            new KeyValuePair<string, string>("id_video", "Номер"),
            new KeyValuePair<string, string>("caption", "Название"),
            new KeyValuePair<string, string>("adress", "Адрес"),
            new KeyValuePair<string, string>("phone", "Телефон"),
            new KeyValuePair<string, string>("license", "Лицензия"),
            new KeyValuePair<string, string>("amount", "Кол-во сотрудников"),

            new KeyValuePair<string, string>("id_district", "Номер"),
            new KeyValuePair<string, string>("district", "Район"),

            new KeyValuePair<string, string>("id_price_list", "Номер"),
            new KeyValuePair<string, string>("price_sell", "Цена продажи"),
            new KeyValuePair<string, string>("price_rent", "Цена аренды"),

            new KeyValuePair<string, string>("id_cassette", "Номер"),
            new KeyValuePair<string, string>("photo", "Фото"),
            new KeyValuePair<string, string>("cost", "Цена"),
            new KeyValuePair<string, string>("demand", "Популярен?"),

            new KeyValuePair<string, string>("id_country", "Номер"),
            new KeyValuePair<string, string>("country", "Страна"),

            new KeyValuePair<string, string>("id_receipt","Номер"),
            new KeyValuePair<string, string>("date", "Дата"),

            new KeyValuePair<string, string>("id_service", "Номер"),
            new KeyValuePair<string, string>("service", "Услуга"),

            new KeyValuePair<string, string>("id_quality", "Номер"),
            new KeyValuePair<string, string>("quality", "Качество"),

            new KeyValuePair<string, string>("id_studio", "Номер"),
            new KeyValuePair<string, string>("studio", "Киностудия"),

            new KeyValuePair<string, string>("id_producer", "Номер"),
            new KeyValuePair<string, string>("fname", "Имя"),
            new KeyValuePair<string, string>("lname", "Фамилия"),
            new KeyValuePair<string, string>("pname", "Отчество"),

            new KeyValuePair<string, string>("id_film", "Номер"),
            new KeyValuePair<string, string>("caption", "Название"),
            new KeyValuePair<string, string>("year", "Год выпуска"),
            new KeyValuePair<string, string>("duration", "Длительность"),
            new KeyValuePair<string, string>("information", "Информация"),
            new KeyValuePair<string, string>("сумма", "сумма")

        };

        string FindInSinonyms(string str)
        {
            foreach (var synonym in synonyms)
            {
                if (synonym.Key == str)
                {
                    return synonym.Value;
                }
            }
            return "none";
        }

        void execSql(string sql)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(conn_param);
                connection.Open();
                NpgsqlCommand com = new NpgsqlCommand(sql, connection);
                com.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        void delete_from_where(string table, string condition)
        {
            string sql = string.Format("delete from {0} where {1}", table, condition);
            execSql(sql);
        }

        int getFirstIntFromTable(string sql)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();
            var dataAdapter = new NpgsqlDataAdapter(sql, connection);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        int getCountRowsOnDelete(string table, string sentense)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            var dataAdapter = new NpgsqlDataAdapter(
                string.Format("SELECT COUNT(*) FROM {0} where {1}", table, sentense),
                connection);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        string FuckingCondition(int col)
        {
            string name = dataGridView1.Columns[col].Name;
            string condition = "";
            if (name == "country")
            {
                condition = "id_country = (SELECT id_country FROM страны WHERE country =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "studio")
            {
                condition = "id_studio = (SELECT id_studio FROM киностудии WHERE studio =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "service")
            {
                condition = "id_service = (SELECT id_service FROM услуги WHERE service =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "fcaption")
            {
                condition = "id_film = (SELECT id_film FROM фильмы WHERE caption =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "quality")
            {
                condition = "id_quality = (SELECT id_quality FROM качества WHERE quality =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "vcaption")
            {
                condition = "id_video = (SELECT id_video FROM видеопрокаты WHERE caption =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "time_start")
            {
                condition = "id_time = (SELECT id_time FROM \"время работы\" WHERE time_start =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "time_end")
            {
                condition = "id_time = (SELECT id_time FROM \"время работы\" WHERE time_end =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "type")
            {
                condition = "id_type = (SELECT id_type FROM типы WHERE type =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "fname")
            {
                condition = "id_owner = (SELECT id_owner FROM владельцы WHERE fname =" + readValue(col) + " LIMIT 1)";
            }
            else if (name == "lname")
            {
                condition = "id_owner = (SELECT id_owner FROM владельцы WHERE lname =" + readValue(col) + " LIMIT 1)";

            }
            else if (name == "district")
            {
                condition = "id_district = (SELECT id_district FROM районы WHERE district =" + readValue(col) + " LIMIT 1)";
            }
            return condition;
        }

        bool isFuckingCol(int col)
        {
            string name = dataGridView1.Columns[col].Name;
            if (
                name == "country"
                || name == "studio"
                || name == "service"
                || name == "fcaption"
                || name == "quality"
                || name == "vcaption"
                || name == "time_start"
                || name == "time_end"
                || name == "type"
                || name == "fname"
                || name == "lname"
                || name == "district"
               )
               return true;

            return false;
        }

        bool isIntCol(int col)
        {
            string name = dataGridView1.Columns[col].Name;

            if (
                (name.Substring(0,2)=="id")
                || (name == "year")
                || (name == "duration")
                || (name =="cost")
                || (name =="price_sell")
                || (name =="price_rent")
                || (name == "amount")
                || (name == "time_start")
                || (name == "time_end")
                )
            {
                return true;
            }
            return false;
        }

        string readValue(int col)
        {
            if (dataGridView1.Rows[0].Cells[col].Value == null)
                return "null";
            if (isIntCol(col))
                return dataGridView1.Rows[0].Cells[col].Value.ToString();
            return string.Format("'{0}'",dataGridView1.Rows[0].Cells[col].Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string condition="";
            for(int col=0;col<dataGridView1.Columns.Count;col++)
            {
                if (col == 0)
                {
                    if (isFuckingCol(col))
                        condition += FuckingCondition(col);
                    else
                        condition += dataGridView1.Columns[col].Name + " = " + readValue(col);
                }
                else if (isFuckingCol(col))
                    condition += " OR " + FuckingCondition(col);
                else
                    condition += " OR " + dataGridView1.Columns[col].Name + " = " + readValue(col);
            }

            if (MessageBox.Show($"Вы действительно хотите удалить {getCountRowsOnDelete(table, condition)} записей?", "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
            

            delete_from_where(table_row, condition);
            MessageBox.Show("удалено!");
            onClickDropDown(btnTable.Text);
        }
    }
}
