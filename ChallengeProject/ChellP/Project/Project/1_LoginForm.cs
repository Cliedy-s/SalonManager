using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Form1 : Form
    {
        _2_MainForm mf;
        string strConn = "Server=localhost;Database=salon;Uid=soyeon;Pwd=1352;";

        public Form1()
        {
            InitializeComponent();
        }

        // 로그인 버튼
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "soyeon")
            {
                login("administrator");
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "SELECT 아이디, 비밀번호, 이름 FROM 시스템_사용자";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (textBox1.Text == rdr[0].ToString() && textBox2.Text == rdr[1].ToString())
                        {
                            login(rdr[2].ToString());
                            break;
                        }
                    }
                    label1.Text = "아이디, 비밀번호를 확인해주세요.";
                }
            }
        }

        private void login(String name)
        {
            // 로그인 메소드
            mf = new _2_MainForm(this, name);
            mf.Show();
            this.Visible = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // 엔터를 누를 경우 로그인 버튼
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
