using System;
using System.Windows.Forms;
using WindowsFormsApp1.User_Controls;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int x = 0;
        private int y = 0;

        public UC_Home uc_home;
        public UC_Table uc_table;
        public UC_Queue uc_queue;
        public UC_Diagram uc_diagram;

        public Form1()
        {
            InitializeComponent();
            DateTimer.Start();

            PanelWidth = panelLeft.Width;
            isCollapsed = false;

            uc_home = new UC_Home(this);
            uc_table = new UC_Table(this);
            uc_queue = new UC_Queue();
            uc_diagram = new UC_Diagram();

            selectPanel.Top = btnHome.Top;
            AddControlsToPanel(uc_home);
            
            timerAnimation.Start();
        }

        private void AddControlsToPanel(Control c)
        {
            c.Dock = DockStyle.Fill;
            //c.Dock = DockStyle.Top;
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(c);
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            x = e.X; y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Location = new System.Drawing.Point(this.Location.X + (e.X - x), this.Location.Y + (e.Y - y));

            }
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DateTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            labelTime.Text = dt.ToString("hh:mm:ss");
        }

        int PanelWidth;
        bool isCollapsed;

        private void timerAnimation_Tick(object sender, EventArgs e)
        {
            if (isCollapsed)
            {
                panelLeft.Width = panelLeft.Width + 10;
                if (panelLeft.Width >= PanelWidth)
                {
                    timerAnimation.Stop();
                    isCollapsed = false;
                    this.Refresh();
                }
            }
            else
            {
                panelLeft.Width = panelLeft.Width - 10;
                if (panelLeft.Width <= 59)
                {
                    timerAnimation.Stop();
                    isCollapsed = true;
                    this.Refresh();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timerAnimation.Start();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            selectPanel.Top = btnHome.Top;
            AddControlsToPanel(uc_home);
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            selectPanel.Top = btnTable.Top;
            AddControlsToPanel(uc_table);
        }

        private void btnQu_Click(object sender, EventArgs e)
        {
            selectPanel.Top = btnQu.Top;
            AddControlsToPanel(uc_queue);
        }

        private void btnDiagram_Click(object sender, EventArgs e)
        {
            selectPanel.Top = btnDiagram.Top;
            AddControlsToPanel(uc_diagram);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
