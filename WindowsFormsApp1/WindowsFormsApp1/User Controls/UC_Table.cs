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
using System.IO;
using System.Drawing.Imaging;
using WindowsFormsApp1.Forms;

namespace WindowsFormsApp1.User_Controls { 

    //таблица квитанции
    //удалить все где date = xxx
    //все где caption видеопроката = yyy

    //обрезать цифры до 2 знаков после запятой
    //сообщение об удалении с подтверждением V
    
    //!//!//!
    /*
     * если удаляется район. Выбрать действией пользователя: удалить изи заменить на какой-либо другой район.
    */
    //видепрокаты кол-во сотрудников проверка

    public partial class UC_Table : UserControl
    {
        UC_DropDownList DT;
        bool isOpen = false;
        int maxHeight = 300;
        Form1 form1;
        int page = 0;
        string search = "";
        DateTimePicker dtp = new DateTimePicker();
        Rectangle rectangle;
        static string conn_param = "Server=127.0.0.1; Port=5432; User Id=postgres; Password=admin; Database=cource";

        string ToBase64Bitmap(Bitmap b)
        {
            System.IO.MemoryStream ms = new MemoryStream();
            b.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }

        public UC_Table(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
            DT = new UC_DropDownList(this.form1);
            DT.Dock = DockStyle.Fill;

            panelDropDown.Controls.Clear();
            panelDropDown.Controls.Add(DT);

            dataGridView1.Controls.Add(dtp);
            dtp.Visible = false;
            dtp.Format = DateTimePickerFormat.Custom;
            dtp.TextChanged += new EventHandler(dtp_TextChange);
            dataGridView1.RowTemplate.Height = 28;
            onClickDropDown(btnTable.Text);
            var Generator = new Generator.Generator();
        }

        int getCountRowsOnDelete(string table, string table_id, int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            var dataAdapter = new NpgsqlDataAdapter(
                string.Format("SELECT COUNT(*) FROM {0} where {1} = ",
                table, table_id) + id.ToString(),
                connection);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        int getCountRowsOnParentTable(string table, string table_parent, string table_id, int id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            var dataAdapter = new NpgsqlDataAdapter(
                string.Format("SELECT COUNT(*) FROM {0} where {1} = ",
                table+" NATURAL JOIN "+ table_parent, table_id) + id.ToString(),
                connection);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        public bool delete(string table, string table_id, int id)
        {
            //return true for cancel
           
            int count = getCountRowsOnDelete(table, table_id, id);
            string text = "Номер записи: "+id
                +"\nбудет удалено записей из таблицы " + table + ": "+count+"\n";
            if (table == "страны")
            {
                string pt = "киностудии";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0);
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "киностудии")
            {
                string pt = "фильмы";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "фильмы")
            {
                string pt = "кассеты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "кассетты")
            {
                string pt = "квитанции";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "режисёры")
            {
                string pt = "фильмы";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "качества")
            {
                string pt = "кассеты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "услуги")
            {
                string pt = "квитанции";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "цены")
            {
                string pt = "квитанции";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "видеопрокаты")
            {
                string pt = "квитанции";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "районы")
            {
                string pt = "видеопрокаты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "типы")
            {
                string pt = "видеопрокаты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "владельцы")
            {
                string pt = "видеопрокаты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            if (table == "\"время работы\"")
            {
                string pt = "видеопрокаты";
                count = getCountRowsOnParentTable(table, pt, table_id, id);
                if (count != 0) ;
                text += "будет удалено записей из таблицы " + pt + ": " + count + "\n";
            }
            text += "вы уверены?\n";
            if(MessageBox.Show(text, "Внимание!", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return true;
            }

            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();
            NpgsqlCommand com = new NpgsqlCommand(string.Format("delete from {0} where {1} = ",
                table, table_id) + id.ToString(), connection);
            com.ExecuteNonQuery();
            connection.Close();
            return false;//exit?
        }

        public DataTable fillComboBox(string fields, string table, string display_member)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            var select = "SELECT " + fields + " from " + table + " order by " + display_member;

            var dataAdapter = new NpgsqlDataAdapter(select, connection);
            var dataTable = new DataTable();

            dataAdapter.Fill(dataTable);
            connection.Close();
            return dataTable;
        }
        /// <summary>
        /// добавить ComboBox в таблицу
        /// </summary>
        /// <param name="name_cmbCol">имя колонки в таблице</param>
        /// <param name="headerText">отображение колонки</param>
        /// <param name="table">из какой таблицы брать данные</param>
        /// <param name="ValueMember">какой член table брать за индекс</param>
        /// <param name="DisplayMember">какой член table показывать</param>
        /// <param name="Xindex">каким по порядку разместить</param>
        void addComboBoxColumn(string name_cmbCol, string headerText, string table,
            string ValueMember, string DisplayMember, int Xindex)
        {
            DataGridViewComboBoxColumn cmbCol = new DataGridViewComboBoxColumn();
            cmbCol.HeaderText = headerText;
            cmbCol.Name = name_cmbCol;
            string fields = "*";
            if (table == "цены")
            {
                fields = DisplayMember + ", 'продажа: '||price_sell||' | прокат: '||price_rent as цена";
            }
            else if (table == "\"время работы\"")
            {
                fields = DisplayMember + ", 'начало: '||time_start ||':00 | конец: '||time_end||':00' as время";
            }else if(table == "кассеты")
            {
                fields = DisplayMember + ", 'цена: '||cost||' | фильм: '||caption as кассета";
                table = "кассеты NATURAL JOIN фильмы";
            }
            cmbCol.DataSource = fillComboBox(fields, table, DisplayMember);
            cmbCol.ValueMember = ValueMember;
            if (table == "цены")
            {
                DisplayMember = "цена";
            } else if (table == "\"время работы\"")
            {
                DisplayMember = "время";
            }else if (table == "кассеты NATURAL JOIN фильмы")
            {
                DisplayMember = "кассета";
            }
            cmbCol.DisplayMember = DisplayMember;
            cmbCol.FlatStyle = FlatStyle.Flat;


            dataGridView1.Columns.Add(cmbCol);
            dataGridView1.Columns[name_cmbCol].DisplayIndex = Xindex;
        }
        void setComboBoxColumn(string column, string column_with_id)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells[column].Value = row.Cells[column_with_id].Value;
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
            foreach(var synonym in synonyms)
            {
                if (synonym.Key == str)
                {
                    return synonym.Value;
                }
            }
            return "none";
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
                    dataGridView1.Columns.Add( col.ColumnName, col.Caption);
                }
                for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    dataGridView1.Rows.Add(dataSet.Tables[0].Rows[i].ItemArray);
            }
            catch (Exception e)
            {

            }
        }

        public string FixBase64ForImage(string Image)
        {
            System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
            sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
            return sbText.ToString();
        }

        DataSet FillDataSetFromDB(string fields, string table, string orderby, string where="1=1")
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            var select = string.Format("SELECT {4} FROM {0} WHERE {5} order by {3} limit {2} offset {1}",
                table, page * 50, (page + 1) * 50, orderby, fields, where);
            var dataAdapter = new NpgsqlDataAdapter(select, connection);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        public void onClickDropDown(string btn)
        {
            btnTable.Text = btn;

            btn = btn.Trim().ToLower();

            update.Text = "(" + ((page * 50) + 1) + " - " + ((page + 1) * 50) + ")";

            
            if (btn == "районы")
            {
                //easy_select(btn, page, "id_district");
                var ds = FillDataSetFromDB("*", btn, "id_district", "district like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text+="/" + getCountRowsTable(btn);
            }
            if (btn == "качества кассет")
            {
                //easy_select("качества", page, "id_quality");
                var ds = FillDataSetFromDB("*", "качества", "id_quality", "quality like '"+search+"%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("качества");

            }
            if (btn == "типы собственности")
            {
                //easy_select("типы", page, "id_type");
                var ds = FillDataSetFromDB("*", "типы", "id_type", "type like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("типы");
            }
            if (btn == "услуги проката")
            {
                //easy_select("услуги", page, "id_service");
                var ds = FillDataSetFromDB("*", "услуги", "id_service", "service like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("услуги");

            }
            if (btn == "страны")
            {
                //easy_select("страны", page, "id_country");
                var ds = FillDataSetFromDB("*", "страны", "id_country", "country like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("страны");

            }
            if (btn == "режиссёры")
            {
                //easy_select("режисёры", page, "id_producer");
                var ds = FillDataSetFromDB("*", "режисёры", "id_producer", "lname like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("режисёры");

            }
            if (btn == "владельцы видеопрокатов")
            {
                //easy_select("владельцы", page, "id_owner");
                var ds = FillDataSetFromDB("*", "владельцы", "id_owner", "lname like '" + search + "%'");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("владельцы");

            }
            if (btn == "время работы видеопрокатов")
            {
                //easy_select("\"время работы\"", page, "id_time");
                var ds = FillDataSetFromDB("*", "\"время работы\"", "id_time");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("\"время работы\"");


            }
            if (btn == "ценники видеопрокатов")
            {
                //easy_select("цены", page, "id_price_list");
                var ds = FillDataSetFromDB("*", "цены", "id_price_list");
                FillGridFromDataSet(ds);
                update.Text += "/" + getCountRowsTable("цены");

            }
            if (btn == "киностудии")
            {
                var dataSet = FillDataSetFromDB("*", "киностудии", "id_studio", "studio like '" + search + "%'");
                FillGridFromDataSet(dataSet);
                update.Text += "/" + getCountRowsTable("киностудии");

                //hide id_country        
                dataGridView1.Columns["id_country"].Visible = false;
                addComboBoxColumn("cmb_country", "Страна", "страны", "id_country", "country", 2);
                setComboBoxColumn("cmb_country", "id_country");
            }
            if (btn == "фильмы")
            {
                var dataSet = FillDataSetFromDB("*", "фильмы", "id_film", "caption like '" + search + "%'");
                FillGridFromDataSet(dataSet);
                update.Text += "/" + getCountRowsTable("фильмы");


                dataGridView1.Columns["id_producer"].Visible = false;
                addComboBoxColumn("cmb_producer", "Режисёр", "режисёры", "id_producer", "lname", 2);
                setComboBoxColumn("cmb_producer", "id_producer");

                dataGridView1.Columns["id_studio"].Visible = false;
                addComboBoxColumn("cmb_studio", "Киностудия", "киностудии", "id_studio", "studio", 3);
                setComboBoxColumn("cmb_studio", "id_studio");
            }
            if (btn == "квитанции")
            {
                var dataSet = FillDataSetFromDB("id_receipt,id_service,date,id_video,id_cassette,id_price_list, " +
                    "CASE" +
                        " WHEN service = 'продажа' THEN cost + price_sell "+
                        "WHEN service = 'прокат' THEN cost + price_rent END as сумма ",
                    "квитанции NATURAL JOIN услуги NATURAL JOIN кассеты NATURAL JOIN цены", "id_receipt");
                FillGridFromDataSet(dataSet);
                update.Text += "/" + getCountRowsTable("квитанции");


                dataGridView1.Columns["id_service"].Visible = false;
                dataGridView1.Columns["id_video"].Visible = false;
                dataGridView1.Columns["id_cassette"].Visible = false;
                dataGridView1.Columns["id_price_list"].Visible = false;

                addComboBoxColumn("cmb_service", "Услуга", "услуги", "id_service", "service", 2);
                addComboBoxColumn("cmb_video", "Видеопрокат", "видеопрокаты", "id_video", "caption", 4);
                addComboBoxColumn("cmb_cassette", "Кассета", "кассеты", "id_cassette", "id_cassette", 5);
                addComboBoxColumn("cmb_price_list", "Ценник", "цены", "id_price_list", "id_price_list", 6);

                setComboBoxColumn("cmb_service", "id_service");
                setComboBoxColumn("cmb_video", "id_video");
                setComboBoxColumn("cmb_cassette", "id_cassette");
                setComboBoxColumn("cmb_price_list", "id_price_list");

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if(row.Cells["date"].Value!=null && row.Cells["date"].Value.ToString()!="")
                        row.Cells["date"].Value = row.Cells["date"].Value.ToString().Substring(0, 10);
                }
            }
            if (btn == "кассеты")
            {
                var dataSet = FillDataSetFromDB("*", "кассеты", "id_cassette");
                FillGridFromDataSet(dataSet);
                update.Text += "/" + getCountRowsTable("кассеты");


                dataGridView1.Columns["id_quality"].Visible = false;
                dataGridView1.Columns["id_film"].Visible = false;
                addComboBoxColumn("cmb_quality", "Качество", "качества", "id_quality", "quality", 2);
                addComboBoxColumn("cmb_film", "Фильм", "фильмы", "id_film", "caption", 4);
                setComboBoxColumn("cmb_quality", "id_quality");
                setComboBoxColumn("cmb_film", "id_film");

                dataGridView1.Columns["demand"].Visible = false;
                DataGridViewCheckBoxColumn chk_demand = new DataGridViewCheckBoxColumn();
                {
                    chk_demand.HeaderText = "Спрос";
                    chk_demand.Name = "chk_demand";
                }


                dataGridView1.Columns["photo"].Visible = false;
                var picCol = new DataGridViewImageColumn();
                
                picCol.HeaderText = "обложка";
                picCol.Name = "pic_photo";
                picCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

                dataGridView1.Columns.Add(picCol);
                dataGridView1.Columns["pic_photo"].DisplayIndex = 3;
                dataGridView1.Columns.Add(chk_demand);
                dataGridView1.Columns["chk_demand"].DisplayIndex = 8;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["photo"].Value != null)
                    {
                        try
                        {
                            Byte[] bitmapData = Convert.FromBase64String(FixBase64ForImage(row.Cells["photo"].Value.ToString()));
                            System.IO.MemoryStream streamBitmap = new System.IO.MemoryStream(bitmapData);
                            Bitmap bmp = new Bitmap((Bitmap)Image.FromStream(streamBitmap));
                            ((DataGridViewImageCell)row.Cells["pic_photo"]).Value = bmp;
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    if (row.Cells["demand"].Value != null)
                    {
                        var b = row.Cells["demand"].Value;
                        if (b.ToString() == "True")
                            row.Cells["chk_demand"].Value = true;
                        else row.Cells["chk_demand"].Value = false;

                    }
                }
            }         
            if (btn == "видеопрокаты")
            {
                var dataSet = FillDataSetFromDB("*", "видеопрокаты", "id_video", "caption like '" + search + "%'");
                FillGridFromDataSet(dataSet);
                update.Text += "/" + getCountRowsTable("видеопрокаты");


                dataGridView1.Columns["id_type"].Visible = false;
                dataGridView1.Columns["id_time"].Visible = false;
                dataGridView1.Columns["id_owner"].Visible = false;
                dataGridView1.Columns["id_district"].Visible = false;

                addComboBoxColumn("cmb_type",     "Тип Собственности",     "типы",             "id_type",     "type", 3);
                addComboBoxColumn("cmb_time",     "Время работы",     "\"время работы\"", "id_time",     "id_time", 6);
                addComboBoxColumn("cmb_owner",    "Владелец",    "владельцы",        "id_owner",    "lname", 9);
                addComboBoxColumn("cmb_district", "Район", "районы",           "id_district", "district", 10);

                setComboBoxColumn("cmb_type", "id_type");
                setComboBoxColumn("cmb_time", "id_time");
                setComboBoxColumn("cmb_owner", "id_owner");
                setComboBoxColumn("cmb_district", "id_district");
            }

            if (dataGridView1.Columns.Count != 0)
            {
                dataGridView1.Columns[0].ReadOnly = true;
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    var sub = dataGridView1.Columns[i].Name.Substring(0, 3);

                    if (dataGridView1.Columns[i].Name == "cmb_price_list")
                    {
                        dataGridView1.Columns[i].Width = 200;
                    }
                    else if(dataGridView1.Columns[i].Name == "cmb_time")
                    {
                        dataGridView1.Columns[i].Width = 210;
                    }
                    else if (dataGridView1.Columns[i].Name == "cmb_cassette")
                    {
                        dataGridView1.Columns[i].Width = 360;
                    }
                    else if (sub == "cmb")
                    {
                        dataGridView1.Columns[i].Width = 140;
                    }
                    else if (sub == "chk")
                    {

                    }
                    else if (dataGridView1.Columns[i].Name == "date")
                    {
                        dataGridView1.Columns[i].Width = 100;
                    }
                   
                    else
                    {
                        dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                }
            }
            //dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            btnRight.Left = update.Left + update.Width - 3;
            btnLeft.Left = -btnLeft.Width + update.Left + 6;
            btnSearch.Left = btnRight.Left + btnRight.Width;

            var temp = SearchBox.Left;
            SearchBox.Left = btnSearch.Left + btnSearch.Width;
            SearchBox.Width += temp - SearchBox.Left;


        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var btn = btnTable.Text.Trim().ToLower();

            int id = int.Parse(e.Row.Cells[0].Value.ToString());
            string table = "";
            string where_id = "";

            if (btn == "районы")
            {
                //delete(btn, "id_district", id);
                table = btn;
                where_id = "id_district";
            }
            else if (btn == "качества кассет")
            {
                table = "качества";
                where_id = "id_quality";
                //delete("качества", "id_quality", id);
            }
            else if (btn == "типы собственности")
            {
                table = "типы";
                where_id = "id_type";
                //delete("типы", "id_type", id);
            }
            else if (btn == "услуги проката")
            {
                table = "услуги";
                where_id = "id_service";
                //delete("услуги", "id_service", id);
            }
            else if (btn == "страны")
            {
                table = "страны";
                where_id = "id_country";
                //delete("страны", "id_country", id);
            }
            else if (btn == "режиссёры")
            {
                table = "режисёры";
                where_id = "id_producer";
                //delete("режисёры", "id_producer", id);
            }
            else if (btn == "владельцы видеопрокатов")
            {
                table = "владельцы";
                where_id = "id_owner";
                //delete("владельцы", "id_owner", id);
            }
            else if (btn == "время работы видеопрокатов")
            {
                table = "\"время работы\"";
                where_id = "id_time";
                //delete("\"время работы\"", "id_time", id);
            }
            else if (btn == "ценники видеопрокатов")
            {
                table = "цены";
                where_id = "id_price_list";
               // delete("цены", "id_price_list", id);
            }
            else if (btn == "киностудии")
            {
                table = "киностудии";
                where_id = "id_studio";
                //delete("киностудии", "id_studio", id);
            }
            else if (btn == "фильмы")
            {
                table = "фильмы";
                where_id = "id_film";
                //delete("фильмы", "id_film", id);
            }
            else if (btn == "квитанции")
            {
                table = "квитанции";
                where_id = "id_receipt";
                //delete("квитанции", "id_receipt", id);
            }
            else if (btn == "кассеты")
            {
                table = "кассеты";
                where_id = "id_cassette";
                //delete("кассеты", "id_cassette", id);
            }
            else if (btn == "видеопрокаты")
            {
                table = "видеопрокаты";
                where_id = "id_video";
                //delete("видеопрокаты", "id_video", id);
            }
            else
            {
                return;
            }
            
            e.Cancel = delete(table, where_id, id);
            //onClickDropDown(btnTable.Text);
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
            }catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        bool readStrFromCell(DataGridViewCell cell, out string r)
        {
            bool result = true;

            if (cell.Value != null)
            {
                r = cell.Value.ToString();
            }
            else
            {
                r = "";
                result = false;
            }

            return result;
        }
        bool readIntFromCell(DataGridViewCell cell, out string i)
        {
            int temp = 0;
            i = "null";
            if (cell.Value != null)
            {
                if (int.TryParse(cell.Value.ToString(), out temp))
                {
                    i = cell.Value.ToString();
                }
                else if (cell.Value.ToString() != "")
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            
            return true;
        }
        bool readBIntFromCell(DataGridViewCell cell, out string r)
        {
           var result = readIntFromCell(cell, out r);
            if (r == "null")
                r = " is " + r;
            else r = " = " + r;
            return result;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var btn = btnTable.Text.Trim().ToLower();
            var row = dataGridView1.Rows[e.RowIndex];

            if (btn == "районы")
            {
                string sql = "";
                if (row.Cells["id_district"].Value == null)//insert
                {
                    string district = row.Cells["district"].Value.ToString();
                    sql = string.Format("insert into районы(district) values('{0}')", district);
                }
                else//update
                {
                    string id = row.Cells["id_district"].Value.ToString();
                    string district = row.Cells["district"].Value.ToString();
                    sql = string.Format("update районы set " +
                        "district = '{1}' " +
                        "where id_district = {0}",
                        id,
                        district
                        );
                }
                execSql(sql);
                
            }
            if (btn == "качества кассет")
            {
                string sql = "";
                if (row.Cells["id_quality"].Value == null)//insert
                {
                    string quality = row.Cells["quality"].Value.ToString();
                    sql = string.Format("insert into качества(quality) values('{0}')", quality);
                }
                else//update
                {
                    string id = row.Cells["id_quality"].Value.ToString();
                    string quality = row.Cells["quality"].Value.ToString();
                    sql = string.Format("update качества set " +
                        "quality = '{1}' " +
                        "where id_quality = {0}",
                        id,
                        quality
                        );
                }
                execSql(sql);
            }
            if (btn == "типы собственности")
            {
                string sql = "";
                if (row.Cells["id_type"].Value == null)//insert
                {
                    string type = row.Cells["type"].Value.ToString();
                    sql = string.Format("insert into типы(type) values('{0}')", type);
                }
                else//update
                {
                    string id = row.Cells["id_type"].Value.ToString();
                    string type = row.Cells["type"].Value.ToString();
                    sql = string.Format("update типы set " +
                        "type = '{1}' " +
                        "where id_type = {0}",
                        id,
                        type
                        );
                }
                execSql(sql);
            }
            if (btn == "услуги проката")
            {
                string sql = "";
                if (row.Cells["id_service"].Value == null)//insert
                {
                    string service = row.Cells["service"].Value.ToString();
                    sql = string.Format("insert into услуги(service) values('{0}')", service);
                }
                else//update
                {
                    string id = row.Cells["id_service"].Value.ToString();
                    string service = row.Cells["service"].Value.ToString();
                    sql = string.Format("update услуги set " +
                        "service = '{1}' " +
                        "where id_service = {0}",
                        id,
                        service
                        );
                }
                execSql(sql);
            }
            if (btn == "страны")
            {
                string sql = "";
                if (row.Cells["id_country"].Value == null)//insert
                {
                    string country = row.Cells["country"].Value.ToString();
                    sql = string.Format("insert into страны(country) values('{0}')", country);
                }
                else//update
                {
                    string id = row.Cells["id_country"].Value.ToString();
                    string country = row.Cells["country"].Value.ToString();
                    sql = string.Format("update страны set " +
                        "country = '{1}' " +
                        "where id_country = {0}",
                        id,
                        country
                        );
                }
                execSql(sql);
            }
            if (btn == "режиссёры")
            {
                string sql = "";
                if (row.Cells["id_producer"].Value == null)//insert
                {
                    string fname = "";
                    string lname = "";
                    string pname = "";

                    if (row.Cells["lname"].Value != null)
                        lname = row.Cells["lname"].Value.ToString();

                    if (row.Cells["fname"].Value != null)
                        fname = row.Cells["fname"].Value.ToString();

                    if (row.Cells["pname"].Value != null)
                        pname = row.Cells["pname"].Value.ToString();

                    sql = string.Format("insert into режисёры(fname, lname, pname)" +
                        " values('{0}', '{1}', '{2}')", fname, lname, pname);
                }
                else//update
                {
                    string id = row.Cells["id_producer"].Value.ToString();
                    string fname = row.Cells["fname"].Value.ToString();
                    string lname = row.Cells["lname"].Value.ToString();
                    string pname = row.Cells["pname"].Value.ToString();

                    sql = string.Format("update режисёры set " +
                        "fname = '{1}', " +
                        "lname = '{2}', " +
                        "pname = '{3}' " +
                        "where id_producer = {0}",
                        id,
                        fname,
                        lname,
                        pname
                        );
                }
                execSql(sql);
            }
            if (btn == "владельцы видеопрокатов")
            {
                string sql = "";
                if (row.Cells["id_owner"].Value == null)//insert
                {
                    string fname = "";
                    if(row.Cells["fname"].Value != null)
                        fname = row.Cells["fname"].Value.ToString();

                    string lname = "";
                    if (row.Cells["lname"].Value != null)
                        lname = row.Cells["lname"].Value.ToString();

                    string pname = "";
                    if (row.Cells["pname"].Value != null)
                        pname = row.Cells["pname"].Value.ToString();

                    sql = string.Format("insert into владельцы(fname, lname, pname)" +
                        " values('{0}', '{1}', '{2}')", fname, lname, pname);
                }
                else//update
                {
                    string id = row.Cells["id_owner"].Value.ToString();
                    string fname = row.Cells["fname"].Value.ToString();
                    string lname = row.Cells["lname"].Value.ToString();
                    string pname = row.Cells["pname"].Value.ToString();

                    sql = string.Format("update владельцы set " +
                        "fname = '{1}', " +
                        "lname = '{2}', " +
                        "pname = '{3}' " +
                        "where id_owner = {0}",
                        id,
                        fname,
                        lname,
                        pname
                        );
                }
                execSql(sql);
            }
            if (btn == "время работы видеопрокатов")
            {
                string sql = "";
                if (row.Cells["id_time"].Value == null)//insert
                {
                    string time_start = "null";
                    if (row.Cells["time_start"].Value != null)
                        time_start = row.Cells["time_start"].Value.ToString();

                    string time_end = "null";
                    if (row.Cells["time_end"].Value != null)
                        time_end = row.Cells["time_end"].Value.ToString();

                    sql = string.Format("insert into \"время работы\"(time_start, time_end)" +
                        " values({0}, {1})", time_start, time_end);
                }
                else//update
                {
                    string id = row.Cells["id_time"].Value.ToString();
                    int temp;

                    string time_start = " = null";
                    if (row.Cells["time_start"].Value != null)
                    {
                        if (int.TryParse(row.Cells["time_start"].Value.ToString(), out temp))
                        {
                            time_start = " = " + row.Cells["time_start"].Value.ToString();
                        }
                        else if (row.Cells["time_start"].Value.ToString() != "")
                        {
                            onClickDropDown(btnTable.Text);
                            return;
                        }
                    }

                    string time_end = " = null";
                    if (row.Cells["time_end"].Value != null)
                    {
                        if (int.TryParse(row.Cells["time_end"].Value.ToString(), out temp))
                        {
                            time_end = " = " + row.Cells["time_end"].Value.ToString();
                        }
                        else if (row.Cells["time_end"].Value.ToString() != "")
                        {
                            onClickDropDown(btnTable.Text);
                            return;
                        }
                    }

                    sql = string.Format("update \"время работы\" set " +
                        "time_start {1}, " +
                        "time_end   {2} " +
                        "where id_time = {0}",
                        id,
                        time_start,
                        time_end
                        );
                }
                execSql(sql);
            }
            if (btn == "ценники видеопрокатов")
            {
                string sql = "";
                if (row.Cells["id_price_list"].Value == null)//insert
                {
                    string price_sell = "null";
                    if (row.Cells["price_sell"].Value != null)
                        price_sell = row.Cells["price_sell"].Value.ToString();

                    string price_rent = "null";
                    if (row.Cells["price_rent"].Value != null)
                        price_rent = row.Cells["price_rent"].Value.ToString();

                    sql = string.Format("insert into цены(price_sell, price_rent)" +
                        " values({0}, {1})", price_sell, price_rent);
                }
                else//update
                {
                    string id = row.Cells["id_price_list"].Value.ToString();
                    int temp;

                    string price_sell = " = null";
                    if (row.Cells["price_sell"].Value != null)
                    {
                        if (int.TryParse(row.Cells["price_sell"].Value.ToString(), out temp))
                        {
                            price_sell = " = " + row.Cells["price_sell"].Value.ToString();
                        }
                        else if(row.Cells["price_sell"].Value.ToString()!="")
                        {
                            onClickDropDown(btnTable.Text);
                            return;
                        }
                    }

                    string price_rent = " = null";
                    if (row.Cells["price_rent"].Value != null)
                    {
                        if (int.TryParse(row.Cells["price_rent"].Value.ToString(), out temp))
                        {
                            price_rent = " = " + row.Cells["price_rent"].Value.ToString();
                        }
                        else if (row.Cells["price_rent"].Value.ToString() != "")
                        {
                            onClickDropDown(btnTable.Text);
                            return;
                        }
                    }

                    sql = string.Format("update цены set " +
                        "price_sell   {1}, " +
                        "price_rent   {2} " +
                        "where id_price_list = {0}",
                        id,
                        price_sell,
                        price_rent
                        );
                }
                execSql(sql);
            }
            if (btn == "киностудии")
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_country"]) { 
                    row.Cells["id_country"].Value = row.Cells["cmb_country"].Value;
                    return;
                }

                string sql = "";
                if (row.Cells["id_studio"].Value == null)//insert
                {
                    string studio = "";
                    if(row.Cells["studio"].Value!=null)
                        studio = row.Cells["studio"].Value.ToString();
                    string id_country = "null";
                    if(row.Cells["id_country"].Value!=null)
                        id_country = row.Cells["id_country"].Value.ToString();

                    sql = string.Format("insert into киностудии(studio, id_country) values('{0}', {1})", studio, id_country);
                }
                else//update
                {
                    string id = row.Cells["id_studio"].Value.ToString();
                    string studio = "";
                    if (row.Cells["studio"].Value != null)
                        studio = row.Cells["studio"].Value.ToString();
                    string id_country = " is null";
                    if (row.Cells["id_country"].Value != null)
                        id_country = " = " + row.Cells["id_country"].Value.ToString();
                    sql = string.Format("update киностудии set " +
                        "studio = '{1}', " +
                        "id_country {2} " +
                        "where id_studio = {0}",
                        id,
                        studio,
                        id_country
                        );
                }
                execSql(sql);
            }
            if (btn == "кассеты")
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_quality"])
                {
                    row.Cells["id_quality"].Value = row.Cells["cmb_quality"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_film"])
                {
                    row.Cells["id_film"].Value = row.Cells["cmb_film"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["pic_photo"])
                {
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["chk_demand"])
                {
                    if((bool)row.Cells["chk_demand"].Value==true)
                        row.Cells["demand"].Value = "1";
                    else
                        row.Cells["demand"].Value = "0";
                    return;
                }

                string sql = "";
                if (row.Cells["id_cassette"].Value == null)//insert
                {
                    string id_quality = "null";
                    string photo = "";
                    string demand = "null";
                    string cost = "null";
                    string id_film = "null";

                    if (row.Cells["id_quality"].Value != null) id_quality = row.Cells["id_quality"].Value.ToString();
                    if (row.Cells["photo"].Value != null) photo = row.Cells["photo"].Value.ToString();
                    if (row.Cells["demand"].Value != null) demand = row.Cells["demand"].Value.ToString();
                    if (row.Cells["cost"].Value != null) cost = row.Cells["cost"].Value.ToString();
                    if (row.Cells["id_film"].Value != null) id_film = row.Cells["id_film"].Value.ToString();

                    sql = string.Format("insert into кассеты(id_quality, photo, cost, demand, id_film)" +
                        " values({0}, '{1}', {2}, {3}, {4})",
                        id_quality, photo, cost, demand, id_film);
                }
                else//update
                {
                    string id = row.Cells["id_cassette"].Value.ToString();
                    string id_quality = "";
                    string photo = "";
                    string demand = "";
                    string cost = "";
                    string id_film = "";

                    readIntFromCell(row.Cells["id_quality"], out id_quality);
                    readStrFromCell(row.Cells["photo"], out photo);
                    readIntFromCell(row.Cells["demand"], out demand);
                    if (demand != "null")
                        demand = "'" + demand + "'";
                    if (!readIntFromCell(row.Cells["cost"], out cost))
                    {
                        onClickDropDown(btnTable.Text);
                        return;
                    }
                    readIntFromCell(row.Cells["id_film"], out id_film);

                    sql = string.Format("update кассеты set " +
                        "id_quality = {1}, " +
                        "photo = '{2}'," +
                        "demand = {3}," +
                        "cost = {4}," +
                        "id_film = {5} "+
                        "where id_cassette = {0}",
                        id,
                        id_quality,
                        photo,
                        demand,
                        cost,
                        id_film
                        );
                }
               execSql(sql);
                return;
            }
            if (btn == "фильмы")
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_producer"])
                {
                    row.Cells["id_producer"].Value = row.Cells["cmb_producer"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_studio"])
                {
                    row.Cells["id_studio"].Value = row.Cells["cmb_studio"].Value;
                    return;
                }

                string sql = "";
                if (row.Cells["id_film"].Value == null)//insert
                {
                    string caption = "";
                    string id_producer = "null";
                    string id_studio = "null";
                    string year = "null";
                    string duration = "null";
                    string information = "";

                    readStrFromCell(row.Cells["caption"], out caption);
                    readIntFromCell(row.Cells["id_producer"], out id_producer);
                    readIntFromCell(row.Cells["id_studio"], out id_studio);
                    readIntFromCell(row.Cells["year"], out year);
                    readIntFromCell(row.Cells["duration"], out duration);
                    readStrFromCell(row.Cells["information"], out information);

                    sql = string.Format("insert into фильмы(caption, id_producer, id_studio," +
                        " year, duration, information)" +
                        " values('{0}', {1}, {2}, {3}, {4}, '{5}')",
                        caption, id_producer, id_studio, year, duration, information);
                }
                else//update
                {
                    string id = row.Cells["id_film"].Value.ToString();
                    string caption = "";
                    string id_producer = "null";
                    string id_studio = "null";
                    string year = "null";
                    string duration = "null";
                    string information = "";

                    readStrFromCell(row.Cells["caption"], out caption);
                    readIntFromCell(row.Cells["id_producer"], out id_producer);
                    readIntFromCell(row.Cells["id_studio"], out id_studio);
                    readIntFromCell(row.Cells["year"], out year);
                    readIntFromCell(row.Cells["duration"], out duration);
                    readStrFromCell(row.Cells["information"], out information);

                    sql = string.Format("update фильмы set " +
                        "caption = '{1}'," +
                        "id_producer = {2}," +
                        "id_studio = {3}," +
                        "year = {4}," +
                        "duration = {5}," +
                        "information = '{6}'"+
                        "where id_film = {0}",
                        id, caption, id_producer,
                        id_studio, year,
                        duration, information
                        );
                }
                execSql(sql);

            }
            if (btn == "квитанции")
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_service"])
                {
                    row.Cells["id_service"].Value = row.Cells["cmb_service"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_video"])
                {
                    row.Cells["id_video"].Value = row.Cells["cmb_video"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_cassette"])
                {
                    row.Cells["id_cassette"].Value = row.Cells["cmb_cassette"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_price_list"])
                {
                    row.Cells["id_price_list"].Value = row.Cells["cmb_price_list"].Value;
                    return;
                }

                string sql = "";
                if (row.Cells["id_receipt"].Value == null)//insert
                {
                    string id_service = "null";
                    string date = "";
                    string id_video = "null";
                    string id_cassette = "null";
                    string id_price_list = "null";

                    readIntFromCell(row.Cells["id_service"], out id_service);
                    readStrFromCell(row.Cells["date"], out date);
                    if (date != "")
                        date = "'" + date + "'";
                    else date = "null";
                    readIntFromCell(row.Cells["id_video"], out id_video);
                    readIntFromCell(row.Cells["id_cassette"], out id_cassette);
                    readIntFromCell(row.Cells["id_price_list"], out id_price_list);

                    sql = string.Format("insert into квитанции(id_service, date, id_video," +
                        " id_cassette, id_price_list)" +
                        " values({0}, {1}, {2}, {3}, {4})",
                        id_service, date, id_video, id_cassette, id_price_list);
                }
                else//update
                {
                    string id = row.Cells["id_receipt"].Value.ToString();
                    string id_service = "null";
                    string date = "";
                    string id_video = "null";
                    string id_cassette = "null";
                    string id_price_list = "null";

                    readIntFromCell(row.Cells["id_service"], out id_service);
                    readStrFromCell(row.Cells["date"], out date);
                    if (date != "")
                        date = "'" + date + "'";
                    else date = "null";
                    readIntFromCell(row.Cells["id_video"], out id_video);
                    readIntFromCell(row.Cells["id_cassette"], out id_cassette);
                    readIntFromCell(row.Cells["id_price_list"], out id_price_list);

                    sql = string.Format("update квитанции set " +
                        "id_service = {1}," +
                        "date = {2}," +
                        "id_video = {3}," +
                        "id_cassette = {4}," +
                        "id_price_list = {5} " +
                        "where id_receipt = {0}",
                        id,
                        id_service,
                        date,
                        id_video,
                        id_cassette,
                        id_price_list
                        );
                }
                execSql(sql);
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["date"])
                    return;
            }
            if (btn == "видеопрокаты")
            {
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_type"])
                {
                    row.Cells["id_type"].Value = row.Cells["cmb_type"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_time"])
                {
                    row.Cells["id_time"].Value = row.Cells["cmb_time"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_owner"])
                {
                    row.Cells["id_owner"].Value = row.Cells["cmb_owner"].Value;
                    return;
                }
                if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["cmb_district"])
                {
                    row.Cells["id_district"].Value = row.Cells["cmb_district"].Value;
                    return;
                }

                string sql = "";
                if (row.Cells["id_video"].Value == null)//insert
                {
                    string caption = "";
                    string adress = "";
                    string id_type = "null";
                    string phone = "+";
                    string license = "";
                    string id_time = "null";
                    string amount = "null";
                    string id_owner = "null";
                    string id_district = "null";

                    readStrFromCell(row.Cells["caption"], out caption);
                    readStrFromCell(row.Cells["adress"], out adress);
                    readIntFromCell(row.Cells["id_type"], out id_type);
                    readStrFromCell(row.Cells["phone"], out phone);
                    if (phone == "")
                        phone = "+";
                    readStrFromCell(row.Cells["license"], out license);
                    readIntFromCell(row.Cells["id_time"], out id_time);
                    readIntFromCell(row.Cells["amount"], out amount);
                    readIntFromCell(row.Cells["id_owner"], out id_owner);
                    readIntFromCell(row.Cells["id_district"], out id_district);

                    sql = string.Format("insert into видеопрокаты" +
                        "(caption, adress, id_type, phone, license, id_time, amount, id_owner, id_district)" +
                        " values('{0}', '{1}', {2}, '{3}', '{4}', {5}, {6}, {7}, {8})",
                        caption, adress, id_type, phone, license, id_time, amount, id_owner, id_district);
                }
                else//update
                {
                    string id = row.Cells["id_video"].Value.ToString();
                    string caption = "";
                    string adress = "";
                    string id_type = "null";
                    string phone = "+";
                    string license = "";
                    string id_time = "null";
                    string amount = "null";
                    string id_owner = "null";
                    string id_district = "null";

                    readStrFromCell(row.Cells["caption"], out caption);
                    readStrFromCell(row.Cells["adress"], out adress);
                    readIntFromCell(row.Cells["id_type"], out id_type);
                    readStrFromCell(row.Cells["phone"], out phone);
                    if (phone == "")
                        phone = "+";
                    readStrFromCell(row.Cells["license"], out license);
                    readIntFromCell(row.Cells["id_time"], out id_time);
                    readIntFromCell(row.Cells["amount"], out amount);
                    readIntFromCell(row.Cells["id_owner"], out id_owner);
                    readIntFromCell(row.Cells["id_district"], out id_district);

                    sql = string.Format("update видеопрокаты set " +
                        "caption = '{1}'," +
                        "adress = '{2}'," +
                        "id_type = {3}," +
                        "phone = '{4}'," +
                        "license = '{5}'," +
                        "id_time = {6}," +
                        "amount = {7}," +
                        "id_owner = {8}," +
                        "id_district = {9} " +

                        "where id_video = {0}",
                        id,
                        caption,
                        adress,
                        id_type, 
                        phone, 
                        license, 
                        id_time, 
                        amount, 
                        id_owner, 
                        id_district
                        );
                }
                execSql(sql);

                return;
            }
            onClickDropDown(btnTable.Text);
        }


        //контролы формочки гыы
        public void closeDropDownMenu()
        {
            if (panelDropDown.Height == maxHeight)
            {
                panelDropDown.Height = 0;
                isOpen = false;
            }
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

        private void label1_Click(object sender, EventArgs e)
        {
            onClickDropDown(btnTable.Text);
        }

        private int getCountRowsTable(string table)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn_param);
            connection.Open();

            var dataAdapter = new NpgsqlDataAdapter(
                string.Format("SELECT COUNT(*) FROM {0}", table),
                connection);
            var dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            connection.Close();
            return int.Parse(dataTable.Rows[0][0].ToString());
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            page++;
            btnRight.Left = update.Left + update.Width - 3;
            btnLeft.Left = -btnLeft.Width + update.Left + 6;
            btnSearch.Left = btnRight.Left + btnRight.Width;

            var temp = SearchBox.Left;
            SearchBox.Left = btnSearch.Left + btnSearch.Width;
            SearchBox.Width += temp - SearchBox.Left;
            onClickDropDown(btnTable.Text);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            page = Math.Max(0, --page);
            update.Text = "(" + ((page * 50) + 1) + " - " + ((page + 1) * 50) + ")";
            btnRight.Left = update.Left + update.Width - 3;
            btnLeft.Left = -btnLeft.Width + update.Left + 6;
            btnSearch.Left = btnRight.Left + btnRight.Width;

            var temp = SearchBox.Left;
            SearchBox.Left = btnSearch.Left + btnSearch.Width;
            SearchBox.Width += temp - SearchBox.Left;
            onClickDropDown(btnTable.Text);
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                search = SearchBox.Text;
                onClickDropDown(btnTable.Text);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            search = SearchBox.Text;
            onClickDropDown(btnTable.Text);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex!=-1)
            {
                var btn = btnTable.Text.Trim().ToLower();
                var row = dataGridView1.Rows[e.RowIndex];
                if (btn == "квитанции")
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "date")
                    {
                        rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        dtp.Size = new Size(rectangle.Width, rectangle.Height);
                        dtp.Location = new Point(rectangle.X, rectangle.Y);
                        dtp.Visible = true;
                    }
                }
                if (btn == "кассеты")
                {
                    if (dataGridView1[e.ColumnIndex, e.RowIndex] == row.Cells["pic_photo"])
                    {
                        using (OpenFileDialog dlg = new OpenFileDialog())
                        {
                            dlg.Title = "Open Image";
                            dlg.Filter = "jpg files (*.jpg)|*.jpg";

                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                var bitmap = new Bitmap(dlg.FileName);
                                ((DataGridViewImageCell)row.Cells["pic_photo"]).Value = bitmap;
                                string a = ToBase64Bitmap(bitmap);
                                row.Cells["photo"].Value = a;
                            }
                        }
                    }
                }
                if (btn == "фильмы")
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "information")
                    {
                        MessageBox.Show(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex!=-1)
            {
                var btn = btnTable.Text.Trim().ToLower();
                if (btn == "квитанции")
                {
                    if (dataGridView1.Columns[e.ColumnIndex].Name == "date")
                    {
                        rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                        dtp.Size = new Size(rectangle.Width, rectangle.Height);
                        dtp.Location = new Point(rectangle.X, rectangle.Y);
                        dtp.Visible = true;
                    }
                }
            }
        }
        private void dtp_TextChange(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell.Value = dtp.Text.ToString();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            dtp.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var btn = btnTable.Text.Trim().ToLower();
            if (btn == "кассеты")
            {
                if (dataGridView1.RowTemplate.Height != 96)
                    dataGridView1.RowTemplate.Height = 96;
                else dataGridView1.RowTemplate.Height = 28;
                onClickDropDown(btnTable.Text);
            }
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;

            //onClickDropDown(btnTable.Text);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            var btn = btnTable.Text.Trim().ToLower();

            if (e.KeyCode == Keys.Delete && e.Shift)
            {
                if (btn == "квитанции")
                {
                    ShowMyDialogBox();
                }
                e.Handled = true;
            }
        }



        public void ShowMyDialogBox()
        {
            Form2 testDialog = new Form2();

            if (testDialog.ShowDialog(this) == DialogResult.OK)
            {
                var date="";
                var caption = "";
                if(testDialog.chCaption.Checked==true)
                    caption = testDialog.caption;
                if (testDialog.chDate.Checked == true)
                   date = testDialog.date;
                if(date!="" && caption != "")
                    MessageBox.Show("удалить из квитанции, где дата = " + date+"\n"
                        + "и где название видеопроката = " + caption);

                else if (date != "")
                    MessageBox.Show("удалить из квитанции, где дата = " + date);
                else if (caption != "")
                    MessageBox.Show("удалить из квитанции, где название видеопроката = " + caption);
            }
            else
            {
            }
            testDialog.Dispose();
        }
    }
}
