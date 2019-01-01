using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SplineCase
{
    public partial class SplitSpline : Form
    {
        public string PATH_ = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public string PATH;

        public SplitSpline()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            this.PATH = Path.Combine(PATH_, "CAD_SPLINE_PARAMETERS.txt");
            Loading();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Saving();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void Loading()
        {
            try
            {
                StreamReader sr = new StreamReader(this.PATH);
                string tmp = sr.ReadLine();
                this.txtNum.Text = tmp;
                sr.Close();
            }
            catch (Exception)
            {
                this.txtNum.Text = "0"; 
            }
        }

        private void Saving()
        {
            StreamWriter sw = new StreamWriter(this.PATH);
            sw.WriteLine(this.txtNum.Text);
            sw.Flush();
            sw.Close();
               
        }


        private void txtNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Saving();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SplitSpline_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Saving();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
