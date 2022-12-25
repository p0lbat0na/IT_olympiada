using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IT_olympiada
{
    public partial class Query : Form
    {
        string tablicaa;
        public Query(string tablica)
        {
            InitializeComponent();
            tablicaa = tablica;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Anket f3 = new Anket(tablicaa);
            f3.Show();
            this.Hide();
            
            f3.poisk(tablicaa, textBox2.Text, textBox1.Text);
        }
    }
}






string[] сolumnName = new string[] { "Участник", "Баллы", "Команда", "Взяла" };
string st = "Select Номер_участника, SUM (Балл) from Результаты  GROUP BY Номер_участника ORDER BY Номер_участника";
int k = 2;

string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
SqlConnection connection = new SqlConnection(connectionString);
connection.Open();
string query = st;
SqlCommand command = new SqlCommand(query, connection);
SqlDataReader reader = command.ExecuteReader();
string[,] uchastnik = new string[10, 2];
int j = 0;
int pobeditel = Convert.ToInt32(uchastnik[0, 0]);
int maxball = Convert.ToInt32(uchastnik[0, 1]);

while (reader.Read())
{

    for (int i = 0; i < k; i++)
    {
        uchastnik[j, i] = reader[i].ToString();
    }

    if (Convert.ToInt32(uchastnik[j, 1]) > maxball)
    {
        maxball = Convert.ToInt32(uchastnik[j, 1]);
        pobeditel = Convert.ToInt32(uchastnik[j, 0]);
    }

}
reader.Close();

query = $"SELECT Код_команды FROM Участники WHERE Номер_участника={pobeditel}";
command = new SqlCommand(query, connection);
reader = command.ExecuteReader();
int komandaPobeditelya = 0;
while (reader.Read())
{
    komandaPobeditelya = Convert.ToInt32(reader[0]);
}
reader.Close();

query = $"SELECT Участники.Код_команды, SUM (Результаты.Балл) FROM Участники JOIN Результаты ON Результаты.Номер_участника = Участники.Номер_участника GROUP BY Участники.Код_команды ORDER BY Участники.Код_команды";
command = new SqlCommand(query, connection);
reader = command.ExecuteReader();
int j2 = 0;
int[,] komandi = new int[5, 3];
dataGridView1.Rows.Clear();
dataGridView1.ColumnCount = 4;
dataGridView1.RowCount = 1;
for (int i = 0; i < 4; i++)
{
    dataGridView1.Columns[i].HeaderText = сolumnName[i];
}

while (reader.Read())
{
    for (int i = 0; i < k; i++)
    {
        komandi[j2, i] = Convert.ToInt32(reader[i]);
    }
    j2++;
}
reader.Close();

int[,] komandi2 = new int[j2, 3];

for (int i = 0; i < j2; i++)
{

    for (int p = 0; p < k; p++)
    {
        komandi2[i, p] = komandi[i, p];
    }
}

string[] mesto = new string[j2];
Prizeri(mesto, komandi2, j2);

dataGridView1[0, 0].Value = pobeditel.ToString();
dataGridView1[1, 0].Value = (maxball).ToString();
dataGridView1[2, 0].Value = (komandi2[(Convert.ToInt32(komandaPobeditelya) - 1), 0]).ToString();
dataGridView1[3, 0].Value = mesto[komandaPobeditelya - 1];
