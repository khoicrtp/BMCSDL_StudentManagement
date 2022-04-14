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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please enter your Username and Password");
                return;
            }
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSV;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            SqlDataReader dataReader;
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SP_LOGIN_NV", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TENDN", textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@MATKHAU", textBox2.Text));

            dataReader = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(dataReader);
            
            if (dt.Rows.Count>0)
            {
                MessageBox.Show("Login successfully ! Welcome teacher !");
                //MainMenu mainM = new MainMenu();
                //this.Hide();
                //mainM.ShowDialog();
            }
            else
            {
                cnn.Close();
                cnn.Open();
                cmd = new SqlCommand("SP_LOGIN_SV", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TENDN", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MATKHAU", textBox2.Text));

                dataReader = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dataReader);

                cnn.Close();

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Login successfully ! Welcome student");
                    //MainMenu mainM = new MainMenu();
                    //this.Hide();
                    //mainM.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid username or password");
                }
            }

            cnn.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLBongDaDataSet.CAUTHU' table. You can move, or remove it, as needed.
            this.cAUTHUTableAdapter.Fill(this.qLBongDaDataSet.CAUTHU);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
