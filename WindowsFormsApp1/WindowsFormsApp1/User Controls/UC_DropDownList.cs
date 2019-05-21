using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.User_Controls
{
    public partial class UC_DropDownList : UserControl
    {
        Form1 form1;
        public UC_DropDownList(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
        }

        private void buttonClick(object sender, EventArgs e)
        {
            form1.uc_table.onClickDropDown(((Button)sender).Text);
            form1.uc_table.closeDropDownMenu();

            form1.uc_home.onClickDropDown(((Button)sender).Text);
            form1.uc_home.closeDropDownMenu();
        }
    }
}
