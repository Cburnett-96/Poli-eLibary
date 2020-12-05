﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Poli_eLibary
{
    public partial class BorrowBook : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
(
int nLeftRect,     // x-coordinate of upper-left corner
int nTopRect,      // y-coordinate of upper-left corner
int nRightRect,    // x-coordinate of lower-right corner
int nBottomRect,   // y-coordinate of lower-right corner
int nWidthEllipse, // height of ellipse
int nHeightEllipse // width of ellipse
);
        public BorrowBook()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
        }
        SqlConnection sqlcon = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\Cburnett\source\repos\Poli-eLibary\poli.mdf; Integrated Security = True; Connect Timeout = 30");
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }
            base.WndProc(ref m);
        }
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BorrowBook_Load(object sender, EventArgs e)
        {
            studentid_textbox.Text = Main.SetValueForText1;
        }
        private void listborrow_Click(object sender, EventArgs e)
        {
            if (studentid_textbox.Text == "admin")
            {
                TableBorrow tbl = new TableBorrow();
                tbl.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Only Authorized Personnel Can Access!", "ATTENTION!");
            }
        }
        private void borrow_Click(object sender, EventArgs e)
        {
            if (bookid.Text != "" && currentdate.Text != "")
            {
                string query = "Insert into BorrowReturn (StudentID, BookID, BorrowDate) VALUES('" + studentid_textbox.Text.Trim() + "','" + bookid.Text.Trim() + "','" + currentdate.Text.Trim() + "')";
                SqlDataAdapter sda = new SqlDataAdapter(query, sqlcon);
                DataTable dtbl = new DataTable();
                sda.Fill(dtbl);
                MessageBox.Show("Successfully Borrowed!");
                Main m = new Main();
                m.Show();
                this.Close();
            }
            else {
                MessageBox.Show("Please Fill-up!", "ATTENTION!");
            }
           }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
