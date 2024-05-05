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

namespace OPBDSHKA
{
    public partial class Form2 : Form
    {
        DATAB dataBase = new DATAB();
        
        
        
        public Form2()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            textBox7.UseSystemPasswordChar = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(maskedTextBox2.Text) ||
                string.IsNullOrWhiteSpace(maskedTextBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox8.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на максимальную длину
            if (textBox1.Text.Length > 50 || textBox2.Text.Length > 50 || textBox3.Text.Length > 50 ||
                textBox6.Text.Length > 50 || textBox7.Text.Length > 50 || textBox8.Text.Length > 50)
            {
                MessageBox.Show("Максимальная длина поля составляет 50 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на минимальную длину логина и пароля
            if (textBox6.Text.Length < 8 || textBox7.Text.Length < 8)
            {
                MessageBox.Show("Минимальная длина логина и пароля должна быть не менее 8 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на ввод цифр в поля фамилия, имя и отчество
            if (textBox1.Text.Any(char.IsDigit) || textBox2.Text.Any(char.IsDigit) || textBox3.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Фамилия, имя и отчество не должны содержать цифры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка длины паспорта
            if (maskedTextBox2.Text.Length < 10)
            {
                MessageBox.Show("Минимальная длина номера паспорта должна быть не менее 10 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка длины номера телефона
            if (maskedTextBox1.Text.Length < 11)
            {
                MessageBox.Show("Минимальная длина номера телефона должна быть не менее 11 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка корректности формата почты
            if (!textBox8.Text.Contains("@") || textBox8.Text.Length < 8)
            {
                MessageBox.Show("Некорректный формат почты. Почта должна содержать символ '@' и быть не короче 8 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на уникальность пользователя
            if (checkuser())
            {
                return;
            }

            string name = textBox1.Text;
            string surname = textBox2.Text;
            string otchestvo = textBox3.Text;
            string pasport = maskedTextBox2.Text;
            string telephone = maskedTextBox1.Text;
            string loginUSER = textBox6.Text;
            string passUSER = textBox7.Text;
            string email = textBox8.Text;
            string role = checkBox1.Checked ? "seller" : "client";

            string querystring = $"INSERT INTO Пользователи VALUES('{loginUSER}','{passUSER}','{role}','{email}');";
            string queryID = $"SELECT [Код пользователя] FROM Пользователи WHERE Логин = '{loginUSER}' AND Пароль = '{passUSER}'";
            SqlCommand commandID = new SqlCommand(queryID, dataBase.getConnection());
            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());

            dataBase.openConnection();
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успех!");
                int usID = Convert.ToInt32(commandID.ExecuteScalar());
                string querystring2 = $"INSERT INTO Клиенты VALUES({usID},N'{surname}',N'{name}',N'{otchestvo}',N'{pasport}',N'{telephone}');";
                if (checkBox1.Checked)
                {
                    querystring2 = $"INSERT INTO Продавцы VALUES({usID},N'{surname}',N'{name}',N'{otchestvo}',N'{pasport}',N'{telephone}');";
                }

                SqlCommand command1 = new SqlCommand(querystring2, dataBase.getConnection());
                command1.ExecuteNonQuery();
                Form1 form1 = new Form1();
                this.Hide();
                form1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }
            dataBase.closeConnection();
        
    }


        private Boolean checkuser()
        {
            var loginUSER = textBox6.Text;
            var passUSER = textBox7.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"SELECT Логин, Пароль, Роль, Почта FROM Пользователи WHERE Логин = '{loginUSER}' AND Пароль = '{passUSER}'";

            SqlCommand command = new SqlCommand(querystring, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Такой пользователь уже существует!");
                return true;
            }
            else
            {
                return false;
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox7.UseSystemPasswordChar = false;
            button4.Visible = true;
            button2.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.UseSystemPasswordChar = true;
            button4.Visible = false;
            button2.Visible = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void StretchImage(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
