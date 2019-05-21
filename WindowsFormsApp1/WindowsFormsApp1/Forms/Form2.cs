﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.Forms
{
    public partial class Form2 : Form
    {
        public string caption;
        public string date;

        public Form2()
        {
            caption = "";
            date = "";
            InitializeComponent();
            date = dateTimePicker1.Value.Date.ToString();
        }

        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            caption = TextBox1.Text;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            date = dateTimePicker1.Value.Date.ToString();
        }
    }
}
