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

        private void button1_Click(object sender, EventArgs e)
        {
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("AddSV", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@HOTEN", textBox2.Text));
            cmd.Parameters.Add(new SqlParameter("@NGAYSINH", textBox3.Text));
            cmd.Parameters.Add(new SqlParameter("@DIACHI", textBox4.Text));
            cmd.Parameters.Add(new SqlParameter("@MALOP", textBox5.Text));
            cmd.Parameters.Add(new SqlParameter("@TENDN", textBox6.Text));
            cmd.Parameters.Add(new SqlParameter("@MATKHAU", textBox7.Text));
            cmd.ExecuteNonQuery();

            cnn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
