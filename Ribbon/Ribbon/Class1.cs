using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput; 

[assembly:CommandClass(typeof(Ribbon.Class1))]
namespace Ribbon
{
    public class Class1
    {
        [CommandMethod("AddRibon")]
        public void AddRibon()
        { 

            RibbonControl rc = ribbonControl();
            RibbonTab rt = ribbontab("ExcelToLine","1");
            RibbonPanel rp = ribbonPanel("Excel To Line");
            RibbonButton rb = ribbonButton("讀取Excel", "E2P\n");



            bool flag = true; 
            foreach (var ff in rc.Tabs)
            {
                if (ff.Title.ToString() == "ExcelToLine") flag = false; 
            }

            if (flag)
            {
                rp.Source.Items.Add(rb);
                rt.Panels.Add(rp);
                rc.Tabs.Add(rt); 
            }

            
        }
          



        public static RibbonControl ribbonControl()
        {
            if (Autodesk.Windows.ComponentManager.Ribbon == null)
            {
                Autodesk.Windows.ComponentManager.ItemInitialized += new EventHandler<RibbonItemEventArgs>(ComponentManager_ItemInitialized); 
            }
            return ComponentManager.Ribbon;

        }

        public static RibbonTab ribbontab(string Name , string id)
        {
            RibbonTab ribTab = new Autodesk.Windows.RibbonTab();
            ribTab.Title = Name;
            ribTab.Id = id;
            ribTab.IsActive = true;
            return ribTab;
        }

        public static RibbonPanel ribbonPanel(string Title)
        {
            RibbonPanelSource ribSourcePanel = new RibbonPanelSource();
            ribSourcePanel.Title = Title;
            RibbonPanel ribPanel = new RibbonPanel();
            ribPanel.Source = ribSourcePanel;
            return ribPanel;
        }

        public static RibbonButton ribbonButton(string btName,string cmdName)
        {
            RibbonButton ribButton = new RibbonButton();
            ribButton.Text = btName;
            ribButton.CommandParameter = cmdName;
            ribButton.ShowText = true; 
            ribButton.CommandHandler = new AdskCommandHandler();
            return ribButton; 
        }



        static void ComponentManager_ItemInitialized(object sender, RibbonItemEventArgs e)
        {
            if(Autodesk.Windows.ComponentManager.Ribbon == null)
            {
                Autodesk.Windows.ComponentManager.ItemInitialized -= new EventHandler<RibbonItemEventArgs>(ComponentManager_ItemInitialized);
            } 
        }

        public class AdskCommandHandler : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
            public void Execute(object parameter)
            {
                RibbonButton ribBtn = parameter as RibbonButton;
                if (ribBtn != null)
                {
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute((String)ribBtn.CommandParameter, true, false, true);    
                }
                RibbonTextBox ribTxt = parameter as RibbonTextBox;
                if (ribTxt != null)
                {
                    System.Windows.Forms.MessageBox.Show(ribTxt.TextValue); 
                } 
            } 

        }









        public static void messgae(string word)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            ed.WriteMessage(word);

        }








    }
}
