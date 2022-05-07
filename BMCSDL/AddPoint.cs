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

            SqlCommand cmd = new SqlCommand("SP_SEL_DIEM_SINHVIEN", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
            cmd.Parameters.Add(new SqlParameter("@MANV", this.MANV));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow dataRow in this.dt.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }

            for (int i = 0; i < this.dt.Rows.Count; i++)
            {
                this.dt.Rows[i][2] = Decrypt256((this.dt.Rows[i][2]).ToString());
            }

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
                String EncryptedPoint = Encrypt256(textBox3.Text);
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_INS_DIEM_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@DIEMTHI", EncryptedPoint));
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

        private void button4_Click(object sender, EventArgs e)
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
                String EncryptedPoint = Encrypt256(textBox3.Text);
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_UPD_DIEM_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@DIEMTHI", EncryptedPoint));
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                String SelectedMASV = this.dt.Rows[selectedRow][0].ToString();
                string connetionString;
                SqlConnection cnn;
                String EncryptedPoint = Encrypt256(textBox3.Text);
                connetionString = @"Data Source=.;Initial Catalog=QLSVNhom;Integrated Security=True;";
                cnn = new SqlConnection(connetionString);
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_DEL_DIEM_SINHVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MASV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@MAHP", textBox2.Text));
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
    }
}
