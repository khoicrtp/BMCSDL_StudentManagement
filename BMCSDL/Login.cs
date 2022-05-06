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
using System.Security.Cryptography;
using System.IO;

namespace BMCSDL
{
    public static class Encryptor
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
    public partial class Login : Form
    {
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
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
            try
            {
                string hashedPassword = MD5Hash(textBox2.Text);
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=.;Initial Catalog=QLSV;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                SqlDataReader dataReader;
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_SEL_PUBLIC_NHANVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TENDN", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MK", hashedPassword));

                dataReader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dataReader);

                if (dt.Rows.Count > 0)
                {
                    DataRow[] datas1 = dt.AsEnumerable().ToArray();
                    String hellomsg = "Login successfully ! Welcome teacher " + datas1[0][0].ToString();
                    MessageBox.Show(hellomsg);
                    MainMenu mainM = new MainMenu(datas1[0][0].ToString(), datas1[0][1].ToString(), 
                        datas1[0][2].ToString(), datas1[0][3].ToString(), datas1[0][4].ToString() , datas1[0][5].ToString() , datas1[0][6].ToString());
                    this.Hide();
                    mainM.ShowDialog();
                }
                else
                {
                    cnn.Close();
                    cnn.Open();
                    cmd = new SqlCommand("SP_SEL_PUBLIC_SINHVIEN", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@TENDN", textBox1.Text));
                    cmd.Parameters.Add(new SqlParameter("@MK", hashedPassword));

                    dataReader = cmd.ExecuteReader();
                    dt = new DataTable();
                    dt.Load(dataReader);

                    cnn.Close();

                    if (dt.Rows.Count > 0)
                    {
                        DataRow[] datas1 = dt.AsEnumerable().ToArray();
                        MessageBox.Show("Login successfully ! Welcome student");
                        /*
                        TeacherManagement teacherM = new TeacherManagement("", "", "", "", "", "", "");
                        this.Hide();
                        teacherM.ShowDialog();
                        */
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }

                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }
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
            /*
            this.Hide();
            TeacherManagement teacherM = new TeacherManagement("a");
            teacherM.ShowDialog();
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
