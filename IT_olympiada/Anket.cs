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
               
        string tablica;        
        string glavPole;
        public Anket(string table, int count = 0, string znach = "")
        {
            InitializeComponent();

            if (table == "Команды")
            {
                label1.Text = "Код команды[ключ]";
                label2.Text = "Название";
                label3.Hide();
                label4.Hide();
                label5.Hide();
                textBox3.Hide();
                textBox4.Hide();
                textBox5.Hide();
                glavPole = "Код_команды";
            }
            if (table == "Результаты")
            {
                label1.Text = "Номер[ключ]";
                label2.Text = "Номер участника";
                label3.Text = "Код задачи";
                label4.Text = "Балл";
                label5.Hide();                
                textBox5.Hide();
                glavPole = "Номер_записи";
            }
            if (table == "Участники")
            {
                label1.Text = "Код команды";
                label2.Text = "Номер участника[ключ]";
                label3.Text = "ФИО";
                label4.Text = "Номер школы";
                label5.Text = "Возраст";
                
                glavPole = "Номер_участника";
            }
            if (table == "Задачи")
            {
                label1.Text = "Личное задание";
                label2.Text = "Максимальный балл";
                label3.Text = "Код_задачи[ключ]";
                glavPole = "Код_задачи";
                
                textBox4.Hide();
                textBox5.Hide();
                label4.Hide();
                label5.Hide();
            }
            tablica = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string znachenie = textBox1.Text;
            if (tablica == "Участники") znachenie = textBox2.Text;
            if (tablica == "Задачи") znachenie = textBox3.Text;
            if (int.TryParse(znachenie, out var parsedNumber))
            {
                poisk(tablica, znachenie, glavPole);
            }
        }

        public void poisk(string table, string znach, string pole)
        {
            if (!String.IsNullOrEmpty(znach))
            { 
                string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
                //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";

                string query = $"SELECT * FROM {table} WHERE {pole}=@znach";

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@znach", znach);

                SqlDataReader reader = command.ExecuteReader();
                int kolvo = 0;
                int i = 0;
                
                try
                {
                    while (reader.Read())
                    {

                        textBox1.Text = reader[0].ToString();
                        textBox2.Text = reader[1].ToString();
                        if (tablica != "Команды")
                        {
                            textBox3.Text = reader[2].ToString();
                            if (tablica != "Задачи")
                            {
                                textBox4.Text = reader[3].ToString();
                                if (tablica == "Участники")
                                    textBox5.Text = reader[4].ToString();
                                
                            }
                        }
                        kolvo++;
                    }
                }
                catch
                {
                    MessageBox.Show("Ошибка");
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
            string znach1 = textBox1.Text;
            string znach2 = textBox2.Text;
            string znach3 = textBox3.Text;
            string znach4 = textBox4.Text;
            string znach5 = textBox5.Text;
            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = "";
            try
            {
                if (tablica == "Участники")
                {
                    query = $"INSERT INTO Участники (Код_команды,Номер_участника,ФИО,Номер_школы,Возраст)VALUES(@znach1, @znach2,@znach3,@znach4,@znach5)";
                }
                if (tablica == "Задачи")
                {
                    query = $"INSERT INTO Задачи (Личные_задания,Максимальное_количество_баллов,Код_задачи)VALUES(@znach1, @znach2,@znach3)";
                }
                if (tablica == "Результаты")
                {
                    query = $"INSERT INTO Результаты (Номер_записи,Номер_участника,Код_задачи,Балл)VALUES(@znach1, @znach2,@znach3,@znach4)";
                }
                if (tablica == "Команды")
                {
                    query = $"INSERT INTO Команды (Код_команды,Название_команды)VALUES(@znach1, @znach2)";
                }

                SqlCommand command = new SqlCommand(query, connection);

                if (!String.IsNullOrEmpty(znach1))
                {
                    command.Parameters.Add("@znach1", znach1);
                    command.Parameters.Add("@znach2", znach2);
                    command.Parameters.Add("@znach3", znach3);
                    command.Parameters.Add("@znach4", znach4);
                    command.Parameters.Add("@znach5", znach5);
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
            
                    if (tablica == "Участники")
                    {
                        query = $"UPDATE Участники SET Код_команды=@znach1,ФИО=@znach3,Номер_школы=@znach4,Возраст=@znach5 WHERE Номер_участника=@znach2" ;
                    }
                    if (tablica == "Задачи")
                    {
                        query = $"UPDATE Задачи SET Личные_задания=@znach1, Максимальное_количество_баллов=@znach2 WHERE Код_задачи=@znach3";
                    }
                    if (tablica == "Результаты")
                    {
                        query = $"UPDATE Результаты SET Номер_участника=@znach2, Код_задачи=@znach3, Балл=@znach4  WHERE Номер_записи=@znach1";
                    }
                    if (tablica == "Команды")
                    {
                        query = $"UPDATE Команды SET Название_команды=@znach2 WHERE Код_команды=@znach1";
                    }
            
                    SqlCommand command = new SqlCommand(query, connection);
            
                    if (!String.IsNullOrEmpty(znach3))
                    {
                        command.Parameters.Add("@znach1", znach1);
                        command.Parameters.Add("@znach2", znach2);
                        command.Parameters.Add("@znach3", znach3);
                        command.Parameters.Add("@znach4", znach4);
                        command.Parameters.Add("@znach5", znach5);
                        
                    }
                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Данные изменены");
                    }
                    catch { MessageBox.Show("Ошибка ввода данных"); }
                        
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string znach = textBox1.Text;
            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string zapros = "";
            if (tablica == "Команды")
                zapros = $"DELETE FROM Команды WHERE Код_команды=@znach";
            if (tablica == "Участники")
            {
                znach = textBox2.Text;
                zapros = $"DELETE FROM Участники WHERE Номер_участника=@znach";
            }
            if (tablica == "Задачи")
            {
                znach=textBox3.Text;
                zapros = $"DELETE FROM Задачи WHERE Личные_задания=@znach";
            }
            if (tablica == "Результаты")

                zapros = $"DELETE FROM Результаты WHERE Номер_записи=@znach";


            SqlCommand command = new SqlCommand(zapros, connection);
            command.Parameters.Add("@znach", znach);
            try
            {
                command.ExecuteNonQuery();
                MessageBox.Show("Данные удалены");
            }
            catch
            {
                MessageBox.Show("Ошибка удаления."+'\n' +"Необходимо удалить данные в других таблицах, связанные с этим полем");
            }
            
        }
    }
}
