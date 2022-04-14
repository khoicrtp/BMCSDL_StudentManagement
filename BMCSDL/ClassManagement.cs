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
        public ClassManagement()
        {
            InitializeComponent();
        }

        private void refreshData()
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLBongDa;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand command = new SqlCommand("Select * from CAULACBO;", cnn); //debug
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            command.ExecuteNonQuery();
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
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLBongDa;User ID=admin;Password=a";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand command = new SqlCommand("Insert into CAULACBO values (@ClassID, @Classname, @StaffID, 'LA');", cnn); //debug
                command.Parameters.AddWithValue("@ClassID", textBox1.Text);
                command.Parameters.AddWithValue("@Classname", textBox2.Text);
                command.Parameters.AddWithValue("@StaffID", textBox3.Text);
                command.ExecuteNonQuery();
                cnn.Close();
                refreshData();
        }

        private void ClassManagement_Load(object sender, EventArgs e)
        {
            refreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLBongDa;User ID=admin;Password=a";
                cnn = new SqlConnection(connetionString);
                cnn.Open();
                SqlCommand command = new SqlCommand("Update CAULACBO set TENCLB = @Classname, MASAN = @StaffID where MACLB = @ClassID;", cnn); //debug
                command.Parameters.AddWithValue("@ClassID", textBox1.Text);
                command.Parameters.AddWithValue("@Classname", textBox2.Text);
                command.Parameters.AddWithValue("@StaffID", textBox3.Text);
                command.ExecuteNonQuery();
                cnn.Close();
                refreshData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLBongDa;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();
            SqlCommand command = new SqlCommand("Delete CAULACBO where MACLB = @ClassID;", cnn); //debug
            command.Parameters.AddWithValue("@ClassID", textBox1.Text);
            command.ExecuteNonQuery();
            cnn.Close();
            refreshData();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            MainMenu mainM = new MainMenu();
            this.Hide();
            mainM.ShowDialog();
        }
    }
}
