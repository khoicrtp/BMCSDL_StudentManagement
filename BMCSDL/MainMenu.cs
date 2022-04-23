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
        string MANV = "";
        public MainMenu(string MANV)
        {
            InitializeComponent();
            this.MANV = MANV;
            label1.Text = "Welcome back, " + this.MANV;
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
            ClassManagement classM = new ClassManagement(MANV);
            classM.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            StudentManagement studentM = new StudentManagement(MANV);
            studentM.ShowDialog();
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddPoint addP = new AddPoint(this.MANV);
            this.Hide();
            addP.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Login loginF = new Login();
            this.Hide();
            loginF.ShowDialog();
        }
    }
}
