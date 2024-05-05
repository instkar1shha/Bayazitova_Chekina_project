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

namespace OPBDSHKA
{
    public partial class adminUS : Form
    {
        DATAB dataBase = new DATAB();

        public adminUS()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            string query = $"SELECT * FROM [Агентство недвижимости].dbo.[Пользователи]";
            SqlCommand command = new SqlCommand(query, dataBase.getConnection());
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);

            dataGridView1.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            ADMIN aDMIN = new ADMIN();
            aDMIN.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void adminUS_Load(object sender, EventArgs e)
        {

        }
    
    }
}