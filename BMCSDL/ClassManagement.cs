using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMCSDL
{
    public partial class ClassManagement : Form
    {
        string MANV;
        string HOTEN;
        string EMAIL;
        string LUONG;
        string TENDN;
        string MATKHAU;
        string PUBKEY;
        DataTable dt;
        public ClassManagement(string MANV, string HOTEN, string EMAIL, string LUONG, string TENDN, string MATKHAU, string PUBKEY)
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

            SqlCommand cmd = new SqlCommand("SP_SEL_LOP", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Please enter required information");
                    return;
                }
                string connetionString;
                SqlConnection cnn;
                //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_INS_LOP", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MALOP", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@TENLOP", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.ExecuteNonQuery();

                cnn.Close();
                refreshData();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }

        }

        private void ClassManagement_Load(object sender, EventArgs e)
        {
            refreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
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

                SqlCommand cmd = new SqlCommand("SP_UPD_LOP", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MALOP", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@TENLOP", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.ExecuteNonQuery();

                cnn.Close();
                refreshData();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string connetionString;
                SqlConnection cnn;
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                String SelectedMALOP = this.dt.Rows[selectedRow][0].ToString();
                //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_DEL_LOP", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MALOP", SelectedMALOP));
                //cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.ExecuteNonQuery();

                cnn.Close();
                refreshData();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            refreshData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mainM = new MainMenu(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            mainM.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
