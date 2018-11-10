using System; 
using System.Data;
using System.Windows.Forms;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Runtime; 



[assembly: CommandClass(typeof(CADPalette.Class1))]
namespace CADPalette
{
    public class Class1
    {
        [CommandMethod("addplate")]
        public void AddPlate()
        {
            System.Windows.Forms.UserControl mycontrol = new UI1();
            Autodesk.AutoCAD.Windows.PaletteSet ps = new Autodesk.AutoCAD.Windows.PaletteSet("PlateteSet");
            ps.Visible = true;
            ps.Style = PaletteSetStyles.ShowAutoHideButton;
            ps.Dock = DockSides.None;
            ps.MinimumSize = new System.Drawing.Size(200, 100);
            ps.Size = new System.Drawing.Size(200, 100); 
            ps.Add("PaletteSet", mycontrol);
            ps.Visible = true; 
        }


    }
}
