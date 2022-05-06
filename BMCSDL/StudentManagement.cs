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
    public partial class StudentManagement : Form
    {
        string MANV;
        string HOTEN;
        string EMAIL;
        string LUONG;
        string TENDN;
        string MATKHAU;
        string PUBKEY;
        DataTable dt;
        public StudentManagement(string MANV, string HOTEN, string EMAIL, string LUONG, string TENDN, string MATKHAU, string PUBKEY)
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

            SqlCommand cmd = new SqlCommand("SP_SEL_SINHVIEN", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MALOP", textBox3.Text));
            cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        private void StudentManagement_Load(object sender, EventArgs e)
        {
            //refreshData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mainM = new MainMenu(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            mainM.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MainMenu mainM = new MainMenu(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            this.Hide();
            mainM.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StudentManagementUtil addS = new StudentManagementUtil(MANV);
            addS.ShowDialog();
            refreshData();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please input class ID");
            }
            else refreshData();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                String SelectedMASV= this.dt.Rows[selectedRow][0].ToString();
                string connetionString;
                SqlConnection cnn;
                //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_DEL_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.Parameters.Add(new SqlParameter("@MASV", SelectedMASV));

                cmd.ExecuteNonQuery();

                cnn.Close();
                refreshData();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }
        }
    }
}

