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






