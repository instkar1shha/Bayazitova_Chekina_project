using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPBDSHKA
{
    public partial class dobav : Form
    {
        DATAB dataBase = new DATAB();
        public dobav()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dataBase.openConnection();
                string District = textBox1.Text;
                if (District.Any(char.IsDigit))
                {
                    MessageBox.Show("Район не должен содержать цифры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string Street = textBox2.Text;
                if (Street.Any(char.IsDigit))
                {
                    MessageBox.Show("Улица не должна содержать цифры.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int Nhouse;
                if (!int.TryParse(textBox3.Text, out Nhouse) || Nhouse <= 0 || Nhouse > 300)
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер дома (от 1 до 300).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int Npod;
                if (!int.TryParse(textBox4.Text, out Npod) || Npod <= 0 || Npod > 20)
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер подъезда (от 1 до 20).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int Nflat;
                if (!int.TryParse(textBox8.Text, out Nflat) || Nflat <= 0 || Nflat > 500)
                {
                    MessageBox.Show("Пожалуйста, введите корректный номер квартиры (от 1 до 500).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int KolFlats;
                if (!int.TryParse(textBox7.Text, out KolFlats) || KolFlats <= 0)
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество квартир.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int etazh;
                if (!int.TryParse(textBox6.Text, out etazh) || etazh <= 0 || etazh > 40)
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество этажей (от 1 до 40).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                int KolRooms;
                if (!int.TryParse(textBox5.Text, out KolRooms) || KolRooms <= 0 || KolRooms > 5)
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество комнат (от 1 до 5).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int Square;
                if (!int.TryParse(textBox9.Text, out Square) || Square < 35 || Square > 200)
                {
                    MessageBox.Show("Пожалуйста, введите корректную площадь (от 35 до 200).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double Cost;
                if (!double.TryParse(textBox10.Text, out Cost) || Cost < 3000000 || Cost > 100000000)
                {
                    MessageBox.Show("Пожалуйста, введите корректную стоимость (от 3 миллионов до 100 миллионов).", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string queryID = $"SELECT [Код пользователя] FROM Пользователи WHERE Логин = '{Form1.loginUSER}' AND Пароль = '{Form1.passUSER}'";
                SqlCommand commandID = new SqlCommand(queryID, dataBase.getConnection());
                int usID = Convert.ToInt32(commandID.ExecuteScalar());
                dataBase.closeConnection();

                var addQuery = $"INSERT INTO Квартиры VALUES(N'{District}',N'{Street}',{Nhouse},{Npod},{Nflat},{KolFlats},{etazh},{KolRooms},{Square},{Cost}, {usID})";
                var command = new SqlCommand(addQuery, dataBase.getConnection());
                dataBase.openConnection();
                command.ExecuteNonQuery();
                MessageBox.Show("Запись успешно создана!!", "Успех!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dataBase.closeConnection();

                this.Hide();
                Form4 form4 = new Form4();
                form4.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





    private void dobav_Load(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
