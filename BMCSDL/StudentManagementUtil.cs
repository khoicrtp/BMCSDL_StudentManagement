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
    public partial class StudentManagementUtil : Form
    {
        string MANV;
        public StudentManagementUtil(string MANV)
        {
            InitializeComponent();
            this.MANV = MANV;
        }

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
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Please input all required data");
            }
            try
            {
                string connetionString;
                SqlConnection cnn;
                //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                String hashedPassword = MD5Hash(textBox7.Text);
                SqlCommand cmd = new SqlCommand("SP_INS_ENCRYPT_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@HOTEN", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@NGAYSINH", textBox3.Text));
                cmd.Parameters.Add(new SqlParameter("@DIACHI", textBox4.Text));
                cmd.Parameters.Add(new SqlParameter("@MALOP", textBox5.Text));
                cmd.Parameters.Add(new SqlParameter("@TENDN", textBox6.Text));
                cmd.Parameters.Add(new SqlParameter("@MK", hashedPassword));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Student inserted successfully !");

                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Please input all required data");
            }
            try
            {
                string connetionString;
                SqlConnection cnn;
                //connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();
                String hashedPassword = MD5Hash(textBox7.Text);
                SqlCommand cmd = new SqlCommand("SP_UPD_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@HOTEN", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@NGAYSINH", textBox3.Text));
                cmd.Parameters.Add(new SqlParameter("@DIACHI", textBox4.Text));
                cmd.Parameters.Add(new SqlParameter("@MALOP", textBox5.Text));
                cmd.Parameters.Add(new SqlParameter("@TENDN", textBox6.Text));
                cmd.Parameters.Add(new SqlParameter("@MATKHAU", hashedPassword));
                cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Update student info successfully !");
                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data!");
            }
        }
    }
}
