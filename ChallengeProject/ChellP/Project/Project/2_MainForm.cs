using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Collections;

namespace Project
{
    public partial class _2_MainForm : Form
    {
        Form1 frm1;
        string strConn = "Server=localhost;Database=salon;Uid=soyeon;Pwd=1352;";
        SerialPort sp;
        ArrayList idsf = new ArrayList();
        // MySqlConnection conn = new MySqlConnection(strConn);
        public _2_MainForm(Form1 frm1, String name)
        {
            InitializeComponent();
            this.frm1 = frm1;
            label92.Text = name + "님 반갑습니다.";
            if (name == "administrator") // 관리자일 경우 시스템 관리 버튼을 활성화 시킴
            {
                label92.Text =  "관리자님 반갑습니다.";
                button4.Enabled = true;
                button4.ForeColor = Color.Black;
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            // 로그인 폼 같이 끄기
            frm1.Close();
        }

        private void _2_MainForm_Load(object sender, EventArgs e)
        {
            //회원리스트
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("이름 ", 70, HorizontalAlignment.Center);
            listView1.Columns.Add("성별 ", 40, HorizontalAlignment.Center);
            listView1.Columns.Add("생일 ", 130, HorizontalAlignment.Center);
            listView1.Columns.Add("휴대전화", 120, HorizontalAlignment.Center);
            listView1.Columns.Add("수신거부", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("등급 ", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("VIP포인트", 90, HorizontalAlignment.Center);
            listView1.Columns.Add("회원번호", 0, HorizontalAlignment.Center);
            listView1.Columns.Add("선호서비스", 105, HorizontalAlignment.Center);

            memberLvSet("SELECT * FROM 회원리스트");

            //개인 매출 리스트
            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;

            listView2.Columns.Add("날짜", 70, HorizontalAlignment.Center);
            listView2.Columns.Add("이름", 71, HorizontalAlignment.Center);
            listView2.Columns.Add("서비스 ", 91, HorizontalAlignment.Center);
            listView2.Columns.Add("가격", 73, HorizontalAlignment.Center);

            //예약 리스트
            listView6.View = View.Details;
            listView6.GridLines = true;
            listView6.FullRowSelect = true;

            listView6.Columns.Add("회원번호", 0, HorizontalAlignment.Center);
            listView6.Columns.Add("휴대전화번호", 200, HorizontalAlignment.Center);
            listView6.Columns.Add("날짜", 200, HorizontalAlignment.Center);
            listView6.Columns.Add("서비스 ", 165, HorizontalAlignment.Center);
            listView6.Columns.Add("비고", 150, HorizontalAlignment.Center);

            //매출 리스트
            listView3.View = View.Details;
            listView3.GridLines = true;
            listView3.FullRowSelect = true;

            listView3.Columns.Add("매출번호", 0, HorizontalAlignment.Center);
            listView3.Columns.Add("날짜", 178, HorizontalAlignment.Center);
            listView3.Columns.Add("서비스", 178, HorizontalAlignment.Center);
            listView3.Columns.Add("가격", 178, HorizontalAlignment.Center);
            listView3.Columns.Add("휴대전화번호", 181, HorizontalAlignment.Center);

            //소모품 리스트
            listView5.View = View.Details;
            listView5.GridLines = true;
            listView5.FullRowSelect = true;

            listView5.Columns.Add("제품 이름", 235, HorizontalAlignment.Center);
            listView5.Columns.Add("제고", 235, HorizontalAlignment.Center);
            listView5.Columns.Add("비고", 245, HorizontalAlignment.Center);


            //사용자 리스트
            listView4.View = View.Details;
            listView4.GridLines = true;
            listView4.FullRowSelect = true;

            listView4.Columns.Add("이름", 178, HorizontalAlignment.Center);
            listView4.Columns.Add("아이디", 178, HorizontalAlignment.Center);
            listView4.Columns.Add("비밀번호", 178, HorizontalAlignment.Center);
            listView4.Columns.Add("직급", 181, HorizontalAlignment.Center);
        }
        //
        private void button1_Click(object sender, EventArgs e)
        {
            //고객관리
            id.BringToFront();
            joinpanel.BringToFront();

            memberLvSet("SELECT * FROM 회원리스트");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 회원 검색 
            listView1.MultiSelect = false;
            panel3.BringToFront();
        }
        private void Button12_Click(object sender, EventArgs e)
        {
            //회원 검색 - 검색
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                label75.Text = "";

                if (textBox1.Text.Trim() == ""
                    && (!radioButton1.Checked && !radioButton2.Checked)
                    && textBox2.Text.Trim() == ""
                    && textBox3.Text.Trim() == ""
                    && textBox4.Text.Trim() == ""
                    && textBox34.Text.Trim() == "")
                {
                    listView1.Items.Clear();
                    memberLvSet("SELECT * FROM 회원리스트");
                }
                else
                {
                    listView1.Items.Clear();
                    string sql = "SELECT * FROM 회원리스트 WHERE";

                    // WHERE문 이후 세팅
                    string temp = sql;

                    if (textBox1.Text.Trim() != "")
                    {
                        sql += " 이름 LIKE '" + textBox1.Text + "'";
                    }
                    if (radioButton2.Checked)
                    {
                        if (sql != temp)
                        {
                            sql += " AND";
                            temp = sql;
                        }
                        sql += " 성별 = '남'";
                    }
                    else if (radioButton1.Checked)
                    {
                        if (sql != temp)
                        {
                            sql += " AND";
                            temp = sql;
                        }
                        sql += " 성별 = '여'";
                    }
                    if (textBox2.Text.Trim() != "" || textBox3.Text.Trim() != "" || textBox4.Text.Trim() != "")
                    {
                        if (sql != temp)
                        {
                            sql += " AND";
                            temp = sql;
                        }
                        sql += " 휴대전화번호 LIKE '";
                        if (textBox2.Text.Trim() != "")
                            sql += textBox2.Text;
                        else sql += "%";
                        if (textBox3.Text.Trim() != "")
                            sql += textBox3.Text;
                        else sql += "%";
                        if (textBox4.Text.Trim() != "")
                            sql += textBox4.Text;
                        else sql += "%";
                        sql += "'";
                    }
                    if (textBox34.Text.Trim() != "")
                    {
                        if (sql != temp)
                        {
                            sql += " AND";
                            temp = sql;
                        }
                        sql += " 등급 LIKE '" + textBox34.Text + "'";
                    }
                    sql += ";";
                    //

                    textBox1.Text = "";
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox34.Text = "";

                    memberLvSet(sql);

                }

            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // 회원 등록 버튼
            listView1.MultiSelect = false;
            panel4.BringToFront();

        }
        private void Button9_Click(object sender, EventArgs e)
        {
            // 회원등록 - 등록
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "INSERT INTO 회원 VALUES(null, @이름, @성별, @생일, @휴대전화번호, @수신거부여부, 0);";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                //데이터 세팅
                cmd.Parameters.AddWithValue("@이름", textBox8.Text);
                if (radioButton3.Checked)
                    cmd.Parameters.AddWithValue("@성별", 0);
                else if (radioButton4.Checked)
                    cmd.Parameters.AddWithValue("@성별", 1);
                cmd.Parameters.AddWithValue("@생일", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@휴대전화번호", textBox7.Text + textBox6.Text + textBox5.Text);
                if (checkBox1.Checked)
                    cmd.Parameters.AddWithValue("@수신거부여부", 0);
                else
                    cmd.Parameters.AddWithValue("@수신거부여부", 1);
                //

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                memberLvSet("SELECT * FROM 회원리스트");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //회원 수정 버튼
            panel1.BringToFront();
            label33.Text = "고객을 선택하세요";
            listView1.MultiSelect = false;
        }

        private void Button39_Click(object sender, EventArgs e)
        {
            // 회원 수정 - 선택
            if (listView1.SelectedIndices.Count > 0)
            {
                string name = listView1.SelectedItems[0].SubItems[0].Text;
                string cell = listView1.SelectedItems[0].SubItems[3].Text;

                label33.Text = name + "님을 선택하셨습니다."; // 이름
                textBox19.Text = listView1.SelectedItems[0].SubItems[0].Text;
                if (listView1.SelectedItems[0].SubItems[1].Text == "남") //성별
                    radioButton7.Checked = true;
                else
                    radioButton8.Checked = true;
                //

                //생일
                dateTimePicker4.Value = DateTime.ParseExact(listView1.SelectedItems[0].SubItems[2].Text, "yyyy-MM-dd", null);
                //

                if (cell.Length == 10) // 휴대전화번호
                {
                    textBox18.Text = cell.Substring(0, 3);
                    textBox17.Text = cell.Substring(3, 3);
                    textBox16.Text = cell.Substring(6, 4);
                }
                else
                {
                    textBox18.Text = cell.Substring(0, 3);
                    textBox17.Text = cell.Substring(3, 4);
                    textBox16.Text = cell.Substring(7, 4);
                }

                if (listView1.SelectedItems[0].SubItems[4].Text == "수락") // 수신거부 여부
                    checkBox4.Checked = true;
                else
                    checkBox4.Checked = false;

                textBox9.Text = listView1.SelectedItems[0].SubItems[6].Text; // VIP point
                // 숨겨진 라벨에 회원번호 저장
                ids.Text = listView1.SelectedItems[0].SubItems[7].Text;
            }
            else
            {
                label33.Text = "고객을 선택하세요";
                ids.Text = "";
            }
        }
        private void Button11_Click_1(object sender, EventArgs e)
        {
            // 회원 수정 - 수정
            if (ids.Text == "")
            {
                msg.Text = "고객을 선택해주세요";
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "UPDATE 회원 SET 이름=@이름, 성별=@성별, 생일=@생일, 휴대전화번호=@휴대전화번호, 수신거부여부=@수신거부여부, vip포인트=@vip포인트 WHERE _회원번호 = @_회원번호";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //데이터 세팅
                    cmd.Parameters.AddWithValue("@이름", textBox19.Text);
                    if (radioButton7.Checked)
                        cmd.Parameters.AddWithValue("@성별", 0);
                    else if (radioButton8.Checked)
                        cmd.Parameters.AddWithValue("@성별", 1);
                    cmd.Parameters.AddWithValue("@생일", dateTimePicker4.Value);
                    cmd.Parameters.AddWithValue("@휴대전화번호", textBox18.Text + textBox17.Text + textBox16.Text);
                    if (checkBox4.Checked) // 수신거부 여부
                        cmd.Parameters.AddWithValue("@수신거부여부", 0);
                    else
                        cmd.Parameters.AddWithValue("@수신거부여부", 1);

                    cmd.Parameters.AddWithValue("@vip포인트", Int32.Parse(textBox9.Text));
                    cmd.Parameters.AddWithValue("@_회원번호", Int32.Parse(ids.Text));
                    //

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee)
                    {
                        Debug.Write(ee.ToString());
                    }
                    conn.Close();
                    memberLvSet("SELECT * FROM 회원리스트");
                }
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            // 회원 제거 버튼
            panel8.BringToFront();
            listView1.MultiSelect = true;
        }

        private void Button41_Click_1(object sender, EventArgs e)
        {
            // 회원 제거 - 선택
            string ids = "";
            int count = 0;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    ids += ", ";
                }
                ids += listView1.SelectedItems[i].SubItems[7].Text;
                count += 1;
            }

            label18.Text = count + "명";
            ids2.Text = ids;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            //회원 제거 - 제거
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "DELETE FROM 회원 WHERE _회원번호 IN (" + ids2.Text + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                memberLvSet("SELECT * FROM 회원리스트");
            }

        }
        private void button35_Click(object sender, EventArgs e)
        {
            // 예약 버튼
            panel23.BringToFront();
            panel22.BringToFront();

            bookingLvSet("SELECT * FROM 예약 ORDER BY 날짜 DESC");
        }
        private void button38_Click(object sender, EventArgs e)
        {
            // 예약 추가
            panel24.BringToFront();
        }
        private void Button37_Click(object sender, EventArgs e)
        {
            // 예약 추가 - 추가
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "INSERT INTO 예약 VALUES(null, @회원_휴대전화번호, @날짜, @서비스_이름, @비고);";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                //데이터 세팅
                cmd.Parameters.AddWithValue("@회원_휴대전화번호", textBox38.Text + textBox37.Text + textBox36.Text);
                cmd.Parameters.AddWithValue("@날짜", dateTimePicker6.Value);
                cmd.Parameters.AddWithValue("@서비스_이름", textBox33.Text);
                cmd.Parameters.AddWithValue("@비고", textBox35.Text);
                //

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                bookingLvSet("SELECT * FROM 예약 ORDER BY 날짜 DESC");
            }

        }
        private void button36_Click(object sender, EventArgs e)
        {
            // 예약 제거 버튼
            panel25.BringToFront();
            listView6.MultiSelect = true;
        }
        private void Button46_Click(object sender, EventArgs e)
        {
            // 예약 제거 - 선택
            string ids = "";
            int count = 0;

            for (int i = 0; i < listView6.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    ids += ", ";
                }
                ids += listView6.SelectedItems[i].SubItems[0].Text;
                count += 1;
            }

            label69.Text = count + "개";
            ids3.Text = ids;
        }

        private void Button40_Click_1(object sender, EventArgs e)
        {
            // 예약 제거 - 제거
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "DELETE FROM 예약 WHERE 예약번호 IN (" + ids3.Text + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                bookingLvSet("SELECT * FROM 예약 ORDER BY 날짜 DESC");
            }
        }
        private void Button43_Click(object sender, EventArgs e)
        {
            // 매출 전환
            panel2.BringToFront();
            listView6.MultiSelect = false;
            label80.Text = "예약을 선택해주세요.";
        }
        private void Button45_Click(object sender, EventArgs e)
        {
            // 매출 전환 - 선택
            if (listView6.SelectedIndices.Count > 0)
            {
                label83.Text = listView6.SelectedItems[0].SubItems[0].Text;
                label80.Text = "예약이 선택되었습니다.";
            }
            else
            {
                label80.Text = "예약을 선택해주세요.";
            }
        }
        private void Button44_Click(object sender, EventArgs e)
        {
            // 매출 전환 - 전환
            sellpanel.BringToFront();
            panel19.BringToFront();
            panel5.BringToFront();
            sellLvSet();

            if (label83.Text != "" && label80.Text != "예약을 선택해주세요.")
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "SELECT 날짜, 서비스_이름, 회원_휴대전화번호 FROM 예약 WHERE 예약번호 = " + label83.Text;

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        dateTimePicker3.Value = DateTime.ParseExact(rdr[0].ToString(), "yyyy-MM-dd tt h:mm:ss", null);
                        textBox28.Text = rdr[1].ToString();
                        string cell = rdr[2].ToString();
                        if (cell.Length == 10) // 휴대전화번호
                        {
                            textBox40.Text = cell.Substring(0, 3);
                            textBox39.Text = cell.Substring(3, 3);
                            textBox29.Text = cell.Substring(6, 4);
                        }
                        else
                        {
                            textBox40.Text = cell.Substring(0, 3);
                            textBox39.Text = cell.Substring(3, 4);
                            textBox29.Text = cell.Substring(7, 4);
                        }
                    }
                    rdr.Close();
                    conn.Close();
                }
            } else
            {
                return;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            // 개인 매출 버튼
            listView1.MultiSelect = false;
            panel9.BringToFront();
        }

        private void Button42_Click(object sender, EventArgs e)
        {
            // 개인 매출 - 선택

            if (listView1.SelectedIndices.Count > 0)
            {
                string sql = "";
                string name = listView1.SelectedItems[0].SubItems[0].Text;
                string cell = listView1.SelectedItems[0].SubItems[3].Text;
                label77.Text = name + "님을 선택하셨습니다.";
                listView2.Items.Clear();

                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    if (name != "비회원")
                    {
                        sql = "SELECT 매출.날짜, 회원리스트.이름, 매출.서비스_이름, 매출.매출 FROM 매출 " +
                            "JOIN 회원리스트 ON 매출.회원_휴대전화번호 = 회원리스트.휴대전화번호 " +
                            "WHERE 매출.회원_휴대전화번호 = '" + cell + "' ORDER BY 날짜 DESC;";

                        //개인 매출 리스트
                        listView2.Clear();
                        listView2.View = View.Details;
                        listView2.GridLines = true;
                        listView2.FullRowSelect = true;


                        listView2.Columns.Add("날짜", 70, HorizontalAlignment.Center);
                        listView2.Columns.Add("이름", 71, HorizontalAlignment.Center);
                        listView2.Columns.Add("서비스 ", 91, HorizontalAlignment.Center);
                        listView2.Columns.Add("가격", 73, HorizontalAlignment.Center);
                    }
                      else
                    {
                        sql = "SELECT 매출.날짜, 매출.회원_휴대전화번호, 매출.서비스_이름, 매출.매출 FROM 매출 LEFT OUTER JOIN 회원리스트 ON 매출.회원_휴대전화번호 = 회원리스트.휴대전화번호 WHERE 이름 IS NULL ORDER BY 날짜 DESC;";

                        //개인 매출 리스트
                        listView2.Clear();
                        listView2.View = View.Details;
                        listView2.GridLines = true;
                        listView2.FullRowSelect = true;

                        listView2.Columns.Add("날짜", 70, HorizontalAlignment.Center);
                        listView2.Columns.Add("휴대전화번호", 81, HorizontalAlignment.Center);
                        listView2.Columns.Add("서비스 ", 71, HorizontalAlignment.Center);
                        listView2.Columns.Add("가격", 68, HorizontalAlignment.Center);
                    }
                        
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        String[] arr = new String[8];
                        arr[0] = string.Format("{0:yyyy/MM/dd}", rdr[0]);
                        arr[1] = rdr[1].ToString();
                        arr[2] = rdr[2].ToString();
                        arr[3] = rdr[3].ToString();

                        ListViewItem lvt = new ListViewItem(arr);
                        listView2.Items.Add(lvt);

                    }
                    rdr.Close();
                    conn.Close();
                }
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 매출 버튼
            sellpanel.BringToFront();
            panel5.BringToFront();

            sellLvSet();
        }
        private void button30_Click(object sender, EventArgs e)
        {
            // 매출 추가
            listView3.MultiSelect = false;
            panel19.BringToFront();
        }
        private void Button33_Click(object sender, EventArgs e)
        {
            // 매출 추가 - 추가
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "INSERT INTO 매출 VALUES(@날짜, @서비스_이름, @매출, @회원_휴대전화번호, null);";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                //데이터 세팅
                cmd.Parameters.AddWithValue("@날짜", dateTimePicker3.Value);
                cmd.Parameters.AddWithValue("@서비스_이름", textBox28.Text);
                cmd.Parameters.AddWithValue("@매출", textBox27.Text);
                cmd.Parameters.AddWithValue("@회원_휴대전화번호", textBox40.Text + textBox39.Text + textBox29.Text);
                //

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                sellLvSet();
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            // 매출 수정
            listView3.MultiSelect = false;
            panel20.BringToFront();
         }
        private void Button47_Click(object sender, EventArgs e)
        {
            // 매출 수정 - 선택
            if (listView3.SelectedIndices.Count > 0)
            {
                label86.Text = "매출이 선택되었습니다.";
                label87.Text = listView3.SelectedItems[0].SubItems[0].Text;

                string cell = listView3.SelectedItems[0].SubItems[4].Text;

                // 날짜
                dateTimePicker5.Value = DateTime.ParseExact(listView3.SelectedItems[0].SubItems[1].Text, "yyyy-MM-dd tt h:mm:ss", null);
                textBox31.Text = listView3.SelectedItems[0].SubItems[2].Text; //서비스
                textBox30.Text = listView3.SelectedItems[0].SubItems[3].Text; //가격
                // 휴대전화번호
                if (cell.Length == 10) 
                {
                    textBox42.Text = cell.Substring(0, 3);
                    textBox41.Text = cell.Substring(3, 3);
                    textBox32.Text = cell.Substring(6, 4);
                }
                else
                {
                    textBox42.Text = cell.Substring(0, 3);
                    textBox41.Text = cell.Substring(3, 4);
                    textBox32.Text = cell.Substring(7, 4);
                }
            }
            else
            {
                label86.Text = "매출을 선택해주세요";
            }

        }
        private void Button15_Click(object sender, EventArgs e)
        {
            // 매출 수정 - 수정
            if (label87.Text == "")
            {
               label86.Text = "매출을 선택해주세요";
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "UPDATE 매출 SET 날짜=@날짜, 서비스_이름=@서비스_이름, 매출=@매출, 회원_휴대전화번호=@회원_휴대전화번호 WHERE 매출번호 = @매출번호";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //데이터 세팅
                    cmd.Parameters.AddWithValue("@날짜", dateTimePicker5.Value);
                    cmd.Parameters.AddWithValue("@서비스_이름", textBox31.Text);
                    cmd.Parameters.AddWithValue("@매출", Int32.Parse(textBox30.Text));
                    cmd.Parameters.AddWithValue("@회원_휴대전화번호", textBox42.Text + textBox41.Text + textBox32.Text);
                    cmd.Parameters.AddWithValue("@매출번호", Int32.Parse(label87.Text));
                    //

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee)
                    {
                        Debug.Write(ee.ToString());
                    }
                    conn.Close();
                    sellLvSet();
                }
            }

        }
        private void button32_Click(object sender, EventArgs e)
        {
            // 매출 삭제
            panel21.BringToFront();
            listView3.MultiSelect = true;
        }
        private void Button48_Click(object sender, EventArgs e)
        {
            // 매출 삭제 - 선택
            string ids = "";
            int count = 0;

            for (int i = 0; i < listView3.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    ids += ", ";
                }
                ids += listView3.SelectedItems[i].SubItems[0].Text;
                count += 1;
            }

            label65.Text = count + "개";
            label90.Text = ids;
        }
        private void Button34_Click(object sender, EventArgs e)
        {
            // 매출 삭제 - 삭제
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "DELETE FROM 매출 WHERE 매출번호 IN (" + label90.Text + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                sellLvSet();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //소모품 관리
            panel6.BringToFront();
            panel7.BringToFront();

            listView5.Items.Clear();
            itemLvSet();
        }
        private void button19_Click(object sender, EventArgs e)
        {
            // 사용 제품 수정
            panel13.BringToFront();
            listView5.MultiSelect = false;

        }
        private void Button49_Click(object sender, EventArgs e)
        {
            // 사용 제품 수정 - 선택

            if (listView5.SelectedIndices.Count > 0)
            {
                label37.Text = listView5.SelectedItems[0].SubItems[0].Text + "(이)가 선택되었습니다.";
                label91.Text = listView5.SelectedItems[0].SubItems[0].Text;
                textBox21.Text = listView5.SelectedItems[0].SubItems[1].Text;
                textBox22.Text = listView5.SelectedItems[0].SubItems[2].Text;
            }
            else
            {
                return;
            }
        }

        private void Button23_Click(object sender, EventArgs e)
        {
            // 사용 제품 수정 - 저장
            if (label91.Text == "")
            {
                label37.Text = "제품을 선택해 주세요";
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "UPDATE 소모품 SET 제고=@제고, 비고=@비고 WHERE 제품_이름=@제품_이름";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //데이터 세팅
                    cmd.Parameters.AddWithValue("@제고", textBox21.Text);
                    cmd.Parameters.AddWithValue("@비고", textBox22.Text);
                    cmd.Parameters.AddWithValue("@제품_이름", label91.Text);
                    //

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee)
                    {
                        Debug.Write(ee.ToString());
                    }
                    conn.Close();
                    itemLvSet();
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            // 사용 제품 등록
            panel11.BringToFront();
            listView5.MultiSelect = false;
        }
        private void Button20_Click(object sender, EventArgs e)
        {
            // 사용 제품 등록 - 등록
            if (textBox23.Text.Trim() !="")
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "INSERT INTO 소모품 VALUES(@제품_이름, 0, @비고);";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //데이터 세팅
                    cmd.Parameters.AddWithValue("@제품_이름", textBox23.Text);
                    cmd.Parameters.AddWithValue("@비고", textBox20.Text);
                    //

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee)
                    {
                        Debug.Write(ee.ToString());
                    }
                    conn.Close();
                    itemLvSet();
                }
            }
            
        }

        private void button22_Click(object sender, EventArgs e)
        {
            // 사용 제품 제거
            listView5.MultiSelect = true;
            panel12.BringToFront();
        }
        private void Button50_Click(object sender, EventArgs e)
        {
            // 사용 제품 제거 - 선택
            string ids = "";
            int count = 0;

            for (int i = 0; i < listView5.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    ids += ", ";
                }
                ids += "'"+listView5.SelectedItems[i].SubItems[0].Text+"'";
                count += 1;
            }

            label16.Text = count + "개";
            label93.Text = ids;
            
        }
        private void Button21_Click(object sender, EventArgs e)
        {
            // 사용 제품 제거 - 제거
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "DELETE FROM 소모품 WHERE 제품_이름 IN (" + label93.Text + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                itemLvSet();
            }
        }
        //

        //시스템 관리
        private void button4_Click(object sender, EventArgs e)
        {
            panel14.BringToFront();
            panel16.BringToFront();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "SELECT * FROM 시스템_사용자";

                listView4.Items.Clear();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[4];
                    arr[0] = rdr[1].ToString();
                    arr[1] = rdr[0].ToString();
                    arr[2] = rdr[2].ToString();
                    arr[3] = rdr[3].ToString();
                    ListViewItem lvt = new ListViewItem(arr);
                    listView4.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }
        }
        //
        private void button27_Click(object sender, EventArgs e)
        {
            // 사용자 등록
            panel15.BringToFront();
            listView4.MultiSelect = false;
        }
        private void Button24_Click(object sender, EventArgs e)
        {
            // 사용자 등록 - 등록
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "INSERT INTO 시스템_사용자 VALUES(@아이디, @이름, @비밀번호, @직급);";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                //데이터 세팅
                cmd.Parameters.AddWithValue("@아이디", textBox10.Text);
                cmd.Parameters.AddWithValue("@이름", textBox13.Text);
                cmd.Parameters.AddWithValue("@비밀번호", textBox11.Text);
                cmd.Parameters.AddWithValue("@직급", textBox12.Text);
                //

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                    label95.Text = "다른 아이디를 사용해 주세요.";
                }
                conn.Close();
                userLvSet();
            }
        }
        private void button25_Click(object sender, EventArgs e)
        {
            // 사용자 삭제
            panel18.BringToFront();
            listView4.MultiSelect = true;
        }
        private void Button51_Click(object sender, EventArgs e)
        {
            // 사용자 삭제 - 선택
            string ids = "";
            int count = 0;

            for (int i = 0; i < listView4.SelectedItems.Count; i++)
            {
                if (i != 0)
                {
                    ids += ", ";
                }
                ids += "'"+listView4.SelectedItems[i].SubItems[1].Text+"'";
                count += 1;
            }

            label52.Text = count + "명";
            label94.Text = ids;
        }
        private void Button29_Click(object sender, EventArgs e)
        {
            // 사용자 삭제 - 삭제
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "DELETE FROM 시스템_사용자 WHERE 아이디 IN (" + label94.Text + ");";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ee)
                {
                    Debug.Write(ee.ToString());
                }
                conn.Close();
                userLvSet();
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            // 사용자 수정
            panel17.BringToFront();
            listView4.MultiSelect = false;
        }
        private void Button52_Click(object sender, EventArgs e)
        {
            // 사용자 수정 - 선택
            if (listView4.SelectedIndices.Count > 0)
            {
                label50.Text = listView4.SelectedItems[0].SubItems[1].Text; // 아이디
                textBox26.Text = listView4.SelectedItems[0].SubItems[0].Text; // 이름
                textBox25.Text = listView4.SelectedItems[0].SubItems[2].Text; // 비밀번호
                textBox24.Text = listView4.SelectedItems[0].SubItems[3].Text; // 직급
            }
            else
            {
                label50.Text = "선택해주세요";
            }
        }
        private void Button28_Click(object sender, EventArgs e)
        {
            // 사용자 수정 - 수정
            if (label50.Text == "선택해주세요")
            {
                return;
            }
            else
            {
                using (MySqlConnection conn = new MySqlConnection(strConn))
                {
                    conn.Open();
                    string sql = "UPDATE 시스템_사용자 SET 이름=@이름, 비밀번호=@비밀번호, 직급=@직급 WHERE 아이디 = @아이디";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //데이터 세팅
                    cmd.Parameters.AddWithValue("@이름", textBox26.Text);
                    cmd.Parameters.AddWithValue("@비밀번호", textBox25.Text);
                    cmd.Parameters.AddWithValue("@직급", textBox24.Text);
                    cmd.Parameters.AddWithValue("@아이디", label50.Text);
                    //

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ee)
                    {
                        Debug.Write(ee.ToString());
                    }
                    conn.Close();
                    userLvSet();
                }
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            // 문자
            listView1.MultiSelect = true;
            panel10.BringToFront();

        }
        private void Button53_Click(object sender, EventArgs e)
        {
            // 문자 - 선택
            idsf.Clear();
            int count = 0;

            for (int i = 0; i < listView1.SelectedItems.Count; i++)
            {
                idsf.Add(listView1.SelectedItems[i].SubItems[3].Text);
                count += 1;
            }

            label27.Text = count + "명";
        }
        private void Button16_Click(object sender, EventArgs e)
        {
            // 문자 - 해당 고객 선호 서비스 삽입
            textBox14.AppendText("&선호서비스&");
            textBox14.Focus();
        }
        private void Button17_Click(object sender, EventArgs e)
        {
            // 문자 - 보내기
            // sms를 보낼 포트 설정
            sp = new SerialPort();
            sp.PortName = "COM4";
            sp.BaudRate = 230400;
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.Encoding = Encoding.UTF8;

            string content = textBox14.Text;
            string personal = "";
            
            sp.Open();

            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();

                if (!sp.IsOpen)
                {
                    return;
                }

                foreach (string phone in idsf)
                {
                    MySqlCommand cmd = new MySqlCommand("SELECT 선호서비스, 수신거부여부 FROM 회원리스트 WHERE 휴대전화번호 LIKE '" + phone + "';", conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    if (check.Checked)
                    {
                        rdr.Read();
                        if (rdr[1].ToString() == "수락")
                        {
                            personal = content.Replace("&선호서비스&", rdr[0].ToString());
                            personal = personal.Replace("\r\n", " ");
                            personal = personal.Replace(" ", "");
                            rdr.Close();

                            sp.Write("AT+CMGS=" + phone + Environment.NewLine);
                            System.Threading.Thread.Sleep(100);
                            sp.Write(personal + Environment.NewLine);
                            sp.Write("AT" + phone + Environment.NewLine);
                        }
                    }
                    else
                    {
                        rdr.Read();
                        personal = content.Replace("&선호서비스&", rdr[0].ToString());
                        personal = personal.Replace("\r\n", " ");
                        personal = personal.Replace(" ", "");
                        rdr.Close();

                        sp.Write("AT+CMGS=" + phone + Environment.NewLine);
                        System.Threading.Thread.Sleep(100);
                        sp.Write(personal + Environment.NewLine);
                        sp.Write("AT" + phone + Environment.NewLine);
                    }
                    
                }

                //버퍼 비우기
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();
                sp.Close();
                sp = null;

            }
        }

        public void memberLvSet(String tsql)
        {
            // 고객 리스트 뷰 세팅
            listView1.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = tsql;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[9];
                    arr[0] = rdr[0].ToString();
                    arr[1] = rdr[1].ToString();
                    arr[2] = string.Format("{0:yyyy/MM/dd}", rdr[2]);
                    arr[3] = rdr[3].ToString();
                    arr[4] = rdr[4].ToString();
                    arr[5] = rdr[5].ToString();
                    arr[6] = rdr[6].ToString();
                    arr[7] = rdr[7].ToString();
                    arr[8] = rdr[8].ToString();

                    ListViewItem lvt = new ListViewItem(arr);
                    listView1.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }

        }
        public void bookingLvSet(String tsql)
        {
            // 예약 리스트 뷰 세팅
            listView6.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = tsql;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[5];
                    arr[0] = rdr[0].ToString();
                    arr[1] = rdr[1].ToString();
                    arr[2] = string.Format("{0:yyyy/MM/dd hh:mm}", rdr[2]);
                    arr[3] = rdr[3].ToString();
                    arr[4] = rdr[4].ToString();

                    ListViewItem lvt = new ListViewItem(arr);
                    listView6.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }
        }
        public void sellLvSet()
        {
            // 매출 리스트 뷰 세팅
            //String tsql = "SELECT 매출.날짜, 매출.서비스_이름, 매출.매출, 매출.회원_휴대전화번호, 회원.이름 FROM 매출 LEFT OUTER JOIN 회원 ON 매출.회원_휴대전화번호 = 회원.휴대전화번호";
            String tsql = "SELECT * FROM 매출 ORDER BY 날짜 DESC";
            listView3.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = tsql;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[5];
                    arr[0] = string.Format("{0:yyyy/MM/dd hh:mm}", rdr[4].ToString());
                    arr[1] = rdr[0].ToString();
                    arr[2] = rdr[1].ToString();
                    arr[3] = rdr[2].ToString();
                    arr[4] = rdr[3].ToString();
                    ListViewItem lvt = new ListViewItem(arr);
                    listView3.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }
        }
        public void itemLvSet()
        {
            // 소모품 리스트 뷰 세팅
            String tsql = "SELECT * FROM 소모품";
            listView5.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = tsql;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[3];
                    arr[0] = rdr[0].ToString();
                    arr[1] = rdr[1].ToString();
                    arr[2] = rdr[2].ToString();
                    ListViewItem lvt = new ListViewItem(arr);
                    listView5.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }
        }
        public void userLvSet()
        {
            // 사용자 리스트 뷰 세팅
            String tsql = "SELECT * FROM 시스템_사용자";
            listView4.Items.Clear();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = tsql;

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    String[] arr = new String[4];
                    arr[0] = rdr[1].ToString();
                    arr[1] = rdr[0].ToString();
                    arr[2] = rdr[2].ToString();
                    arr[3] = rdr[3].ToString();
                    ListViewItem lvt = new ListViewItem(arr);
                    listView4.Items.Add(lvt);

                }
                rdr.Close();
                conn.Close();
            }
        }

    }
}

/*
sp.WriteLine("AT+CMGF=1" + Environment.NewLine);
sp.WriteLine("AT+CSMP=17,167,0,8" + Environment.NewLine);
sp.WriteLine("AT+CSCS=\"UCS2\"" + Environment.NewLine);
sp.WriteLine("AT+CMGW=휴대폰번호" + Environment.NewLine);
sp.WriteLine(SmsEngine.UnicodeStr2HexStr("Hellowwwww안녕하세요") + Environment.NewLine);
SendKeys.Send("^(z)" );
 */
