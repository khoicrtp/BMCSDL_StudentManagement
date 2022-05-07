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
    public partial class AddTeachers : Form
    {
        //string MANV;
        DataTable dt;
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
        public AddTeachers()
        {
            InitializeComponent();
            //this.MANV = MANV;
            dt = new DataTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
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

                String EncSalary = Encrypt256(textBox5.Text);
                String HashPassword = MD5Hash(textBox6.Text);
                SqlCommand cmd = new SqlCommand("SP_INS_ENCRYPT_NHANVIEN", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MANV", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@EMAIL", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@TENDN", textBox3.Text));
                cmd.Parameters.Add(new SqlParameter("@HOTEN", textBox4.Text));
                cmd.Parameters.Add(new SqlParameter("@LUONG", EncSalary));
                cmd.Parameters.Add(new SqlParameter("@MK", HashPassword));
                cmd.Parameters.Add(new SqlParameter("@PUBKEY", "PUBPUB"));
                cmd.ExecuteNonQuery();

                MessageBox.Show("Insert staff successfully!");
                cnn.Close();
            }
            catch
            {
                MessageBox.Show("Something went wrong! Please check inputted data");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void AddTeachers_Load(object sender, EventArgs e)
        {

        }
    }
}
