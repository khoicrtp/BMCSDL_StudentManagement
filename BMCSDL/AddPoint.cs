using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace BMCSDL
{
    public partial class AddPoint : Form
    {
        string MANV;
        string HOTEN;
        string EMAIL;
        string LUONG;
        string TENDN;
        string MATKHAU;
        string PUBKEY;
        public AddPoint(string MANV, string HOTEN, string EMAIL, string LUONG, string TENDN, string MATKHAU, string PUBKEY)
        {
            InitializeComponent();
            this.MANV = MANV;
            this.HOTEN = HOTEN;
            this.EMAIL = EMAIL;
            this.LUONG = LUONG;
            this.TENDN = TENDN;
            this.MATKHAU = MATKHAU;
            this.PUBKEY = PUBKEY;
        }
        private void refreshData()
        {
            string connetionString;
            SqlConnection cnn;
            //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
            connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("ViewPoints", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
            cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please enter required information");
                return;
            }
            try
            {
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("AddPoints", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@DIEMTHI", textBox3.Text));
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.ExecuteNonQuery();
                cnn.Close();
                refreshData();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            refreshData();
        }

        private void AddPoint_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu mainM = new MainMenu(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            this.Hide();
            mainM.ShowDialog();
        }
    }
}
