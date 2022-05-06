using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BMCSDL
{
    public partial class MainMenu : Form
    {
        string MANV;
        string HOTEN;
        string EMAIL;
        string LUONG;
        string TENDN;
        string MATKHAU;
        string PUBKEY;
        public MainMenu(string MANV, string HOTEN, string EMAIL, string LUONG, string TENDN, string MATKHAU, string PUBKEY)
        {
            InitializeComponent();
            this.MANV = MANV;
            this.HOTEN = HOTEN;
            this.EMAIL = EMAIL;
            this.LUONG = LUONG;
            this.TENDN = TENDN;
            this.MATKHAU = MATKHAU;
            this.PUBKEY = PUBKEY;
            label1.Text = "Welcome back, " + this.HOTEN;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClassManagement classM = new ClassManagement(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            classM.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentManagement studentM = new StudentManagement(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            studentM.ShowDialog();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddPoint addP = new AddPoint(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            this.Hide();
            addP.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login loginF = new Login();
            this.Hide();
            loginF.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TeacherManagement addP = new TeacherManagement(MANV, HOTEN, EMAIL, LUONG, TENDN, MATKHAU, PUBKEY);
            this.Hide();
            addP.ShowDialog();
        }
    }
}
