using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;

[assembly:CommandClass(typeof(EstablishGUI.Class1))]
namespace EstablishGUI
{
    class Class1
    {

        [CommandMethod("ww")]
        public void ShowDialog()
        {


            MyForm form = new MyForm();
            form.ShowInTaskbar = false;
            Application.ShowModalDialog(form);


            //using (MyForm form = new MyForm())
            //{
            //    form.ShowInTaskbar = false;
            //    Application.ShowModalDialog(form);
            //    if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            //    {
            //        Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage("\n" + form.InputText.text);
            //    } 
            //}
        }
    }
}
