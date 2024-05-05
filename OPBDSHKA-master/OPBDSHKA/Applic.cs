using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace OPBDSHKA
{
    public partial class Applic : Form
    {
        DATAB dataBase = new DATAB();
        private int usId;

        public Applic()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Applic_Load(object sender, EventArgs e)
        {
            string queryDIS = "SELECT DISTINCT Район FROM Квартиры";
            SqlCommand cmd = new SqlCommand(queryDIS, dataBase.getConnection());
            dataBase.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader.GetString(0));
            }
            reader.Close();
            dataBase.closeConnection();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            ClientForm clientForm = new ClientForm();
            clientForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка ввода числа комнат
            int Wrooms;
            if (!int.TryParse(textBox4.Text, out Wrooms) || Wrooms < 1 || Wrooms > 5)
            {
                MessageBox.Show("Пожалуйста, введите корректное число комнат (от 1 до 5).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка ввода стоимости
            double Cost;
            if (!double.TryParse(textBox1.Text, out Cost) || Cost < 3000000 || Cost > 100000000)
            {
                MessageBox.Show("Пожалуйста, введите корректную стоимость (не менее 3 миллионов и не более 100 млн).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка выбора района
            string Distinct = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(Distinct))
            {
                MessageBox.Show("Пожалуйста, выберите район.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Остальной код для добавления заявки в базу данных
            string status = "Активный";
            DateTime currentDate = DateTime.Now.Date;
            string queryID = $"SELECT [Код пользователя] FROM Пользователи WHERE Логин = '{Form1.loginUSER}' AND Пароль = '{Form1.passUSER}'";
            SqlCommand commandID = new SqlCommand(queryID, dataBase.getConnection());
            dataBase.openConnection();
            int usID = Convert.ToInt32(commandID.ExecuteScalar());
            dataBase.closeConnection();

            string queryDOB = $"INSERT INTO Заявки VALUES({usID}, GETDATE(), {Cost}, N'{Distinct}', {Wrooms}, N'{status}')";
            var commandDOB = new SqlCommand(queryDOB, dataBase.getConnection());
            dataBase.openConnection();

            if (commandDOB.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заявка успешно создана!!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            dataBase.closeConnection();
            this.Hide();
            ClientForm clientForm = new ClientForm();
            clientForm.ShowDialog();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Дополнительный код, если необходимо
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Дополнительный код, если необходимо
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Дополнительный код, если необходимо
        }
    }
}

