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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BMCSDL
{
    public partial class TeacherManagement : Form
    {
        string MANV;
        string HOTEN;
        string EMAIL;
        string LUONG;
        string TENDN;
        string MATKHAU;
        string PUBKEY;
        DataTable dt;

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
 
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static string Encrypt256(string clearText)
        {
            string EncryptionKey = "19127181";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt256(string cipherText)
        {
            string EncryptionKey = "19127181";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        private void refreshData()
        {
            dt = new DataTable();
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=.;Initial Catalog=QLSV;Integrated Security=True;";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SP_SEL_ENCRYPT_NHANVIEN", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(this.dt);

            foreach (DataRow dataRow in this.dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i][3] = Decrypt256((this.dt.Rows[i][3]).ToString());
            }

            dataGridView1.DataSource = this.dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public TeacherManagement(string MANV, string HOTEN, string EMAIL, string LUONG, string TENDN, string MATKHAU, string PUBKEY)
        {
            InitializeComponent();
            this.MANV = MANV;
            this.HOTEN = HOTEN;
            this.EMAIL = EMAIL;
            this.LUONG = LUONG;
            this.TENDN = TENDN;
            this.MATKHAU = MATKHAU;
            this.PUBKEY = PUBKEY;

            textBox1.Text = MANV;
            textBox4.Text = HOTEN;
            textBox2.Text = EMAIL;
            textBox5.Text = Decrypt256(LUONG);
            textBox3.Text = TENDN;
            textBox6.Text = MATKHAU;
        }
     
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TeacherManagement_Load(object sender, EventArgs e)
        {
            refreshData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTeachers addT = new AddTeachers();
            addT.ShowDialog();
            refreshData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //selectedCellsButton_Click();
            int selectedRow = dataGridView1.CurrentCell.RowIndex;
            //MessageBox.Show(selectedRow.ToString());
            String SelectedMANV = this.dt.Rows[selectedRow][0].ToString();
            
            try
            {
                string connetionString;
                SqlConnection cnn;
                connetionString = @"Data Source=.;Initial Catalog=QLSV;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                //EXEC SP_INS_ENCRYPT_NHANVIEN ‘NV01’, ‘NGUYEN VAN A’, ‘NVA@’, ‘aaaaaaaa’, ‘NVA’, ‘bbbbbbbb’
                SqlCommand cmd = new SqlCommand("SP_DEL_NHANVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MANV", SelectedMANV));

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

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            refreshData();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login mainM = new Login();
            mainM.ShowDialog();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
