using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using System.Windows.Forms; 

using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

namespace EstablishGUI
{
    public partial class MyForm : Form
    {
        public MyForm()
        {
            InitializeComponent();
        }

        private void MyForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            using (EditorUserInteraction edUsrInt = ed.StartUserInteraction(this))
            {
                Point3d pt = GetPoint("\nPlease select point : ");
                this.InputText.Text = "(" + pt.X.ToString() + "," + pt.Y.ToString() + "," + pt.Z.ToString() + ")" ;
                edUsrInt.End();
                this.Focus();

            }
        }

        public Point3d GetPoint(string word)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptPointResult pt = ed.GetPoint(word);
            if (pt.Status == PromptStatus.OK)
            {
                return (Point3d)pt.Value;
            }
            else
            {
                return new Point3d();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

        }

        private void InputText_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
