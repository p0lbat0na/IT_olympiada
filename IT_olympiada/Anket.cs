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

namespace IT_olympiada
{
    public partial class Anket : Form
    {
        
        int row = 0;
        string tablename;
        int colCount;
        string znacheniye;
        string id;
        public Anket(string table, int count = 0, string znach = "")
        {
            InitializeComponent();

            if (table == "Команды")
            {
                label1.Text = "Код команды";
                label2.Text = "Название";
                
                
                id = "Код_команды";
            }
            if (table == "Результаты")
            {
                label1.Text = "Номер";
                label2.Text = "Номер участника";
                label3.Text = "Код задачи";
                label4.Text = "Балл";
                
                
                id = "Номер_записи";
            }
            if (table == "Участники")
            {
                label1.Text = "Код команды";
                label2.Text = "Номер участника";
                label3.Text = "ФИО";
                label4.Text = "Номер школы";
                label5.Text = "Возраст";
                
                id = "Номер_участника";
            }
            if (table == "Задачи")
            {
                label1.Text = "Личное задание";
                label2.Text = "Максимальный балл";
                

                id = "Личные_задания";
            }
            tablename = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string atribut = textBox1.Text;

            if (tablename == "Участники") atribut = textBox2.Text;
            poisk(tablename, atribut, id);
        }

        public void poisk(string table, string atribut, string pole)
        {
            if (!String.IsNullOrEmpty(atribut))
            {

                string query = "SELECT *";
                string p = "num";
                string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
                //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";

                query = $"SELECT * FROM {table} WHERE {pole}=@atribut";

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@atribut", atribut);

                SqlDataReader reader = command.ExecuteReader();
                int kolvo = 0;
                int i = 0;
                string[,] result = new string[10, 9];
                try
                {
                    while (reader.Read())
                    {

                        textBox1.Text = reader[0].ToString();
                        textBox2.Text = reader[1].ToString();
                        if (tablename != "Команды")
                        {
                            textBox3.Text = reader[2].ToString();
                            if (tablename != "Задачи")
                            {
                                textBox4.Text = reader[3].ToString();
                                if (tablename == "Участники")
                                    textBox5.Text = reader[4].ToString();
                                
                            }
                        }
                        kolvo++;
                    }
                }
                catch
                {
                    MessageBox.Show("Неверный формат");
                }
                if (kolvo == 0)
                {
                    MessageBox.Show("Данные не найдены");
                    this.Hide();
                }
               

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string atribut1 = textBox1.Text;
            string atribut2 = textBox2.Text;
            string atribut3 = textBox3.Text;
            string atribut4 = textBox4.Text;
            string atribut5 = textBox5.Text;
            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "";
            try
            {
                if (tablename == "Участники")
                {
                    query = $"INSERT INTO Участники (Код_команды,Номер_участника,ФИО,Номер_школы,Возраст)VALUES(@atribut1, @atribut2,@atribut3,@atribut4,@atribut5)";
                }
                if (tablename == "Задачи")
                {
                    query = $"INSERT INTO Задачи (Личные_задания,Максимальное_количество_баллов)VALUES(@atribut1, @atribut2,@atribut3)";
                }
                if (tablename == "Результаты")
                {
                    query = $"INSERT INTO Результаты (Номер_записи,Номер_участника,Код_задачи,Балл)VALUES(@atribut1, @atribut2,@atribut3,@atribut4)";
                }
                if (tablename == "Команды")
                {
                    query = $"INSERT INTO Команды (Код_команды,Название_команды)VALUES(@atribut1, @atribut2)";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (!String.IsNullOrEmpty(atribut1))
                {
                    command.Parameters.Add("@atribut1", atribut1);
                    command.Parameters.Add("@atribut2", atribut2);
                    command.Parameters.Add("@atribut3", atribut3);
                    command.Parameters.Add("@atribut4", atribut4);
                    command.Parameters.Add("@atribut5", atribut5);
                }

                command.ExecuteNonQuery();
                MessageBox.Show("Данные сохранены");
            }
            catch
            {

                DialogResult result = MessageBox.Show(
                "Такая запись уже существует. Перезаписать данные?",
                "Сообщение",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {

                    if (tablename == "Участники")
                    {
                        query = $"UPDATE Участники SET Код_команды=@atribut1,ФИО=@atribut3,Номер_школы=@atribut4,Возраст=@atribut5 WHERE Номер_участника=@atribut2" ;
                    }
                    if (tablename == "Задачи")
                    {
                        query = $"UPDATE Задачи SET Максимальное_количество_баллов=@atribut2 WHERE Личные_задания=@atribut1";
                    }
                    if (tablename == "Результаты")
                    {
                        query = $"UPDATE Результаты SET Код_задачи=@atribut2,Балл=@atribut3 WHERE Номер_записи=@atribut1";
                    }
                    if (tablename == "Команды")
                    {
                        query = $"UPDATE Команды SET Название_команды=@atribut2WHERE Код_команды=@atribut1";
                    }

                    SqlCommand command = new SqlCommand(query, connection);

                    if (!String.IsNullOrEmpty(atribut1))
                    {
                        command.Parameters.Add("@atribut1", atribut1);
                        command.Parameters.Add("@atribut2", atribut2);
                        command.Parameters.Add("@atribut3", atribut3);
                        command.Parameters.Add("@atribut4", atribut4);
                        command.Parameters.Add("@atribut5", atribut5);
                        
                    }

                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные изменены");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string atribut = textBox1.Text;
            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "";
            if (tablename == "Команды")
                query = $"DELETE FROM Команды WHERE Код_команды=@atribut";
            if (tablename == "Участники")
            {
                atribut = textBox2.Text;
                query = $"DELETE FROM Участники WHERE Номер_участника=@atribut";
            }
            if (tablename == "Задачи")
            
                query = $"DELETE FROM Задачи WHERE Личные_задания=@atribut";

            if (tablename == "Результаты")

                query = $"DELETE FROM Результаты WHERE Номер_записи=@atribut";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add("@atribut", atribut);
            command.ExecuteNonQuery();
            MessageBox.Show("Данные удалены");
        }
    }
}
