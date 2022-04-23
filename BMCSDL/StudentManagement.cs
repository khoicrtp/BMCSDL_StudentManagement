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
        public StudentManagement(string MANV)
        {
            InitializeComponent();
            this.MANV = MANV;
        }
        private void refreshData()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("ViewSV", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MALOP", textBox3.Text));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        private void StudentManagement_Load(object sender, EventArgs e)
        {
            //refreshData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("ViewSV", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MALOP", textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
            cmd.ExecuteNonQuery();

            cnn.Close();
            refreshData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainMenu mainM = new MainMenu(MANV);
            mainM.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MainMenu mainM = new MainMenu(MANV);
            this.Hide();
            mainM.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            StudentManagementUtil addS = new StudentManagementUtil(MANV);
            addS.ShowDialog();
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
            if (textBox1.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Please enter required information");
                return;
            }
            try
            {
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("DeleteSV", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
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

