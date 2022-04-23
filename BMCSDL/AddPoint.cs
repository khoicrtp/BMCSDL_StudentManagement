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
    public partial class AddPoint : Form
    {
        String MANV;
        public AddPoint(String MANV)
        {
            InitializeComponent();
            this.MANV = MANV;
        }
        private void refreshData()
        {
            /*
            string connetionString;
            SqlConnection cnn;
            connetionString = @"Data Source=DESKTOP-RDCK09P;Initial Catalog=QLSVNhom;User ID=admin;Password=a";
            cnn = new SqlConnection(connetionString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand("ViewPoints", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@MALOP", textBox3.Text));

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cmd.ExecuteNonQuery();
            cnn.Close();
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void AddPoint_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainMenu mainM = new MainMenu(MANV);
            this.Hide();
            mainM.ShowDialog();
        }
    }
}
