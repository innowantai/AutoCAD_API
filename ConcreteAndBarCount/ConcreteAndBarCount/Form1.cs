using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms; 
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;
 
namespace ConcreteAndBarCount
{
    public partial class Form1 : Form
    {
        public string ResLayer; 
        public static string CADpastDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public string txtBoxIndex; 
        public List<string> CheckStr; 
        private List<CheckBox> checkBoxes;  
        List<string> layers;
        Document doc  ;
        Database db  ;
        Editor ed  ;

        public Form1(List<string> layers_, Document doc_, Database db_, Editor ed_)
        {
            InitializeComponent();
            layers = layers_;
            doc = doc_;
            db = db_;
            ed = ed_;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            rdb1.Checked = true;
            checkBoxes = new List<CheckBox> {chLine, chPolyline, chArc, chCircle, chText }; 
            LoadPastData(); 

            for (int i = 0; i < layers.Count; i++) 
                cmb.Items.Add("第" + (i + 1).ToString() + "個圖層 : " + layers[i]);  
            cmb.SelectedIndex = 0; 
             
            String Size = SystemInformation.PrimaryMonitorSize.ToString();
            String Width = SystemInformation.PrimaryMonitorSize.Width.ToString();
            String Height = SystemInformation.PrimaryMonitorSize.Height.ToString(); 
            this.Location = new Point(Int32.Parse(Width)/2, Int32.Parse(Height) / 2);
             

            try
            {
                txtBox.Text = txtBoxIndex;
            }
            catch (System.Exception)
            {

                txtBox.Text = this.layers[0];
            }
            
        }


        private void LoadPastData()
        {
            StreamReader sr = new StreamReader(Path.Combine(CADpastDataPath, "CADParameters.txt"));
            try
            {
                txtBoxIndex = sr.ReadLine();
                chLine.Checked = Convert.ToBoolean(sr.ReadLine());
                chPolyline.Checked = Convert.ToBoolean(sr.ReadLine());
                chArc.Checked = Convert.ToBoolean(sr.ReadLine());
                chCircle.Checked = Convert.ToBoolean(sr.ReadLine());
                chText.Checked = Convert.ToBoolean(sr.ReadLine());

            }
            catch (System.Exception)
            {
                txtBoxIndex = this.layers[0]; 
            }

            sr.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.ResLayer = rdb1.Checked ? txtBox.Text : this.layers[cmb.SelectedIndex];
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close(); 
             
            List<string> CheckStr_ = new List<string>() { "Line", "Polyline", "Arc", "Circle", "NNNNGGGGG"};
            CheckStr = new List<string>();
            for (int i = 0; i < this.checkBoxes.Count; i++)
            {
                if (this.checkBoxes[i].Checked)
                {
                    this.CheckStr.Add(CheckStr_[i]);
                }
            }

            SaveText();
        }      

        private void SaveText()
        {

            StreamWriter sw = new StreamWriter(Path.Combine(CADpastDataPath, "CADParameters.txt"));
            sw.WriteLine(txtBox.Text);
            sw.Flush();
            foreach (CheckBox item in this.checkBoxes)
            {
                sw.WriteLine(item.Checked.ToString());
                sw.Flush();
            }
            sw.Close();
        }


        private void txtBox_TextChanged(object sender, EventArgs e)
        {
            int kk = 0;
            string text = txtBox.Text.Trim();
            foreach (string ss in this.layers)
            {
                if (ss.IndexOf(text) != -1)
                {
                    cmb.SelectedIndex = kk;
                    break;
                }
                kk = kk + 1;
            }

        }
        private void label2_Click(object sender, EventArgs e)
        {
            rdb2.Checked = true;
        }
        private void lblLayers_Click(object sender, EventArgs e)
        {
            rdb1.Checked = true; 
        }
         
    }
}
