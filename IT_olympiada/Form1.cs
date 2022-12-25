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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData(string st, int k, List<string> colName = null, string atr1 = "", string atr2 = "", string atr3 = "")
        {
            //string connectionString = "Data Source=311-UCH\\MSSQLSERVER1;Initial Catalog=turagenstvo;Integrated Security=True";
            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = st;
            SqlCommand command = new SqlCommand(query, connection);
            if (!String.IsNullOrEmpty(atr1))
            {
                command.Parameters.Add("@atr1", atr1);
                command.Parameters.Add("@atr2", atr2);
                command.Parameters.Add("@atr3", atr3);
            }

            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[k]);
                for (int i = 0; i < k; i++)
                {
                    dataGridView1.ColumnCount = k;
                    data[data.Count - 1][i] = reader[i].ToString();
                    //dataGridView1.Columns[i].HeaderText = stolb[i];
                }
            }
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = k;
            if (colName == null)
            {



                if (reader.HasRows)
                {
                    string[] columnName = new string[5];
                    dataGridView1.RowCount = 1;
                    dataGridView1.ColumnCount = k;
                    for (int i = 0; i < k; i++)
                    {
                        columnName[i] = reader.GetName(i);
                        dataGridView1.Rows[0].Cells[i].Value = columnName[i];
                    }
                }
            }
            else
            {
                for (int i = 0; i < k; i++)
                    dataGridView1.Columns[i].HeaderText = colName[i];
            }
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;

            reader.Close();
            connection.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void результатыToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string[] сolumnName = new string[] { "Номер команды",  "Баллы", "Место",};

            int k = 2;

            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT Участники.Код_команды, SUM (Результаты.Балл) FROM Участники JOIN Результаты ON Результаты.Номер_участника = Участники.Номер_участника JOIN Команды ON Команды.Код_команды=Участники.Код_команды GROUP BY Участники.Код_команды ORDER BY Участники.Код_команды";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            int j2 = 0;
            string[] komNames = new string[20];
            int[,] komandi = new int[20, 3];
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 3;
            dataGridView1.RowCount = 3;
            for (int i = 0; i < 3; i++)
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
            int goldIndex = 1;
            int silverIndex = 1;
            int bronzeIndex = 1;

            Prizeri(mesto, komandi2, j2, goldIndex, silverIndex, bronzeIndex);
            
            komandi=Rotate(komandi2);
            for (int i = 1; i < 4; i++)  
            {
                for (int p = 0; p < k+1; p++)
                {
                    dataGridView1[ p,i-1].Value = komandi[p, i-1];                    
                }
                  
            }
            
            connection.Close();
        }

        private void участникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "Select * from Участники";
            int k = 5;
            LoadData(st, k);
        }
        public int[,] Rotate(int[,] array)
        {
            int n = array.GetLength(0), m = array.GetLength(1);
            int[,] res = new int[m, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    res[j, n - i - 1] = array[i, j];
            return res;
        }
        private void результатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "Select * from Результаты";
            int k = 4;
            LoadData(st, k);
        }

        private void командыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "Select * from Команды";
            int k = 2;
            LoadData(st, k);
        }

        private void задачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "Select * from Задачи";
            int k = 3;
            LoadData(st, k);
        }

        private void информацияОКомандахИУчастникахToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string st = "Select Код_команды, Номер_участника from Участники";
            int k = 2;
            LoadData(st, k);
        }

        private void сколькоБалловНабралПобедительИМестоЕгоКомандыToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            dataGridView1[1,0].Value=(maxball).ToString();
            dataGridView1[2,0].Value=(komandi2[(Convert.ToInt32(komandaPobeditelya) - 1), 0]).ToString();
            dataGridView1[3, 0].Value = mesto[komandaPobeditelya-1];
                        
            connection.Close();
            
        }



        public void Prizeri(string[] mesta, int[,] elementi, int kolvo,int goldIndex = 1,int silverIndex = 1,int bronzeIndex=1)
        {
            elementi[0, 2] = 1;
            int gold = 0;           
            
            for (int i = 0; i < kolvo; i++)
            {
                if (elementi[i, 1] > gold)
                {
                    gold = elementi[i, 1];
                    goldIndex = elementi[i,0];
                    elementi[i, 2] = 1;
                }
            }
            elementi[1, 2] = 2;
            int silver = 0;
            for (int i = 0; i < kolvo; i++)
            {
                if (elementi[i, 1] > silver && elementi[i, 1] != gold)
                {
                    silverIndex = elementi[i, 0];
                    silver = elementi[i, 1];
                    elementi[i, 2] = 2;
                }
            }
            if (kolvo > 2)
            {
                elementi[2, 2] = 3;
                int bronze = 0;
                for (int i = 0; i < kolvo; i++)
                {
                    if (elementi[i, 1] > bronze && elementi[i, 1] != silver && elementi[i, 1] != gold)
                    {
                        bronzeIndex = elementi[i, 0];
                        bronze = elementi[i, 1];
                        elementi[i, 2] = 3;
                    }

                }
            }
            
            for (int i = 0; i < kolvo; i++)
            {
                switch (elementi[i, 2]) {
                    case 1:
                    mesta[i] = "Золото";
                        break;
                    case 2:
                    mesta[i] = "Серебро";
                        break;
                    case 3:
                    mesta[i] = "Бронзу";
                        break;
                        default: mesta[i] = "Непризовое";
                        break;
                } }
        }
        public void Prizeri2(string[] mesta, int[,] elementi, int kolvo, int goldIndex = 1, int silverIndex = 1, int bronzeIndex = 1)
        {
            elementi[2,0] = 1;
            int gold = 0;

            for (int i = 0; i < kolvo; i++)
            {
                if (elementi[1,i] > gold)
                {
                    gold = elementi[1, i];
                    goldIndex = elementi[0, i];
                    elementi[2, i] = 1;
                }
            }
            elementi[2,1] = 2;
            int silver = 0;
            for (int i = 0; i < kolvo; i++)
            {
                if (elementi[1, i] > silver && elementi[1, i] != gold)
                {
                    silverIndex = elementi[0, i];
                    silver = elementi[1, i];
                    elementi[2, i] = 2;
                }
            }
            if (kolvo > 2)
            {
                elementi[2, 2] = 3;
                int bronze = 0;
                for (int i = 0; i < kolvo; i++)
                {
                    if (elementi[1, i] > bronze && elementi[1, i] != silver && elementi[1, i] != gold)
                    {
                        bronzeIndex = elementi[0, i];
                        bronze = elementi[1, i];
                        elementi[2, i] = 3;
                    }

                }
            }

            for (int i = 0; i < kolvo; i++)
            {
                switch (elementi[2, i])
                {
                    case 1:
                        mesta[i] = "Золото";
                        break;
                    case 2:
                        mesta[i] = "Серебро";
                        break;
                    case 3:
                        mesta[i] = "Бронзу";
                        break;
                    default:
                        mesta[i] = "Непризовое";
                        break;
                }
            }
        }
        private void какиеУчатстникиЗанялиПризовыеМестаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] сolumnName = new string[] { "Номер участника", "Баллы", "Место", };

            int k = 2;

            string connectionString = "Data Source=DESKTOP-359A439\\SQLEXPRESS;Initial Catalog=IT olympiad;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"SELECT Номер_участника, SUM (Балл) FROM Участники GROUP BY Номер_участника ORDER BY Номер_участника";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            int j2 = 0;
            
            int[,] uchastniki = new int[30, 3];
            dataGridView1.Rows.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.RowCount = 5;
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Columns[i].HeaderText = сolumnName[i];
            }

            while (reader.Read())
            {
                for (int i = 0; i < k; i++)
                {
                    uchastniki[j2, i] = Convert.ToInt32(reader[i]);
                }

                j2++;
            }
            reader.Close();

            int[,] uchastniki2 = new int[j2, 3];

            for (int i = 0; i < j2; i++)
            {

                for (int p = 0; p < k; p++)
                {
                    uchastniki2[i, p] = uchastniki[i, p];
                }
            }
            string[] mesto = new string[j2];
            int goldIndex = 1;
            int silverIndex = 1;
            int bronzeIndex = 1;

            Prizeri2(mesto, uchastniki2, j2, goldIndex, silverIndex, bronzeIndex);
            //
            //uchastniki = Rotate(uchastniki2);
            for (int i = 1; i < 5; i++)
            {
                for (int p = 0; p < k + 1; p++)
                {
                    dataGridView1[i - 1,p ].Value = uchastniki2[ i - 1,p];
                }

            }

            connection.Close();
        }

        private void участникиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Anket f3 = new Anket("Участники");
            f3.Show();
        }

        private void командыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Anket f3 = new Anket("Команды");
            f3.Show();
        }

        private void задачиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Anket f3 = new Anket("Задачи");
            f3.Show();
        }

        private void результатыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Anket f3 = new Anket("Результаты");
            f3.Show();
        }

        private void поУчастникамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Query f2 = new Query("Участники");
            f2.Show();
        }

        private void поКомандамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Query f2 = new Query("Команды");
            f2.Show();
        }

        private void поЗадачамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Query f2 = new Query("Задачи");
            f2.Show();
        }

        private void поРезультатамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Query f2 = new Query("Результаты");
            f2.Show();
        }
    }
}
