using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;
using ExcelClass; 
[assembly: CommandClass(typeof(SplineCase.Class1))]
namespace SplineCase
{
    public class Class1
    {
        public static Document doc;
        public static Database db;
        public static Editor ed;
        public static string folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string SavePath;
        public static List<string> CheckedStr;
        public static List<string> P_Layers; 
        

        [CommandMethod("sp")]
        public void Main()
        { 
            SplitSpline SplitForm = new SplitSpline();
            SplitForm.ShowDialog();
            if (SplitForm.DialogResult == System.Windows.Forms.DialogResult.OK)
            { 

                doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                db = doc.Database;
                ed = doc.Editor;
                Database acCurDb = doc.Database;
                DBObjectCollection EntityCollection = new DBObjectCollection();
                PromptSelectionResult ents = ed.GetSelection();
                double num = Convert.ToDouble(SplitForm.txtNum.Text.Trim());
                if (num > 0)
                { 
                    List<Spline> splies = Main_Process(ents);
                    List<Point3d> data = SplintToExcel(splies, num);
                    PlotLine(data, acCurDb);
                }
            }


        }


        public List<Spline> Main_Process(PromptSelectionResult ents)
        {
            Entity entity = null;
            List<string> ResData = new List<string>();
            List<Spline> splies = new List<Spline>();
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;

                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);
                        string TargetslayName = entity.Layer;

                        Object obj = transaction.GetObject(id, OpenMode.ForWrite, true);
                        if (obj.GetType().Name == "Spline")
                        {
                            Spline sp = obj as Spline;
                            splies.Add(sp);
                        }

                    }
                    transaction.Commit();
                }
            }

            return splies;
        }

        private List<Point3d> SplintToExcel(List<Spline> splies, double span)
        {

            List<Point3d> POINTs = new List<Point3d>();
            foreach (Spline sp in splies)
            {
                int cntNum = sp.NumControlPoints;
                double spLen = sp.GetDistAtPoint(sp.GetControlPointAt(cntNum - 1));
                double dist = span;
                while (dist < spLen)
                {
                    POINTs.Add(sp.GetPointAtDist(dist));
                    dist += span;
                }
            }

            int kk = 0;
            string[,] points = new string[POINTs.Count, 2];
            foreach (Point3d item in POINTs)
            {
                points[kk, 0] = item.X.ToString();
                points[kk, 1] = item.Y.ToString();
                kk++;
            }

            string PATH = Path.Combine(folder, "SplineTest", "SplineData.xlsx");
            EXCEL Excel = new EXCEL(PATH);
            Excel.Save_To(PATH, "SplineData", 1, 1, points);

            return POINTs;

        }
         

        private void PlotLine(List<Point3d> data, Database acCurDb)
        {

            using (Transaction acTrans = doc.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                for (int i = 0; i < data.Count; i++)
                {
                    if (i < data.Count - 1)
                    {
                        using (Line acLine = GetVerticalPoints(data[i], data[i + 1]))
                        {

                            acBlkTblRec.AppendEntity(acLine);
                            acTrans.AddNewlyCreatedDBObject(acLine, true);
                        }
                    }
                    else
                    {
                        using (Line acLine = GetVerticalPoints(data[i - 1], data[i]))
                        {

                            acBlkTblRec.AppendEntity(acLine);
                            acTrans.AddNewlyCreatedDBObject(acLine, true);
                        }
                    }
                }
                acTrans.Commit();
            }
        }

        private Line GetVerticalPoints(Point3d p1, Point3d p2)
        {
            Double EXT = 20;
            Line line = new Line(p1, p2);
            double Len = line.Length;
            Point3d Dir = new Point3d(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z) / Len;
            Point3d newP1 = new Point3d(p1.X + EXT * Dir.Y, p1.Y + EXT * -Dir.X, p1.Z+ EXT * Dir.Z);
            Point3d newP2 = new Point3d(p1.X + -EXT * Dir.Y, p1.Y + -EXT * -Dir.X, p1.Z + -EXT * Dir.Z);

            return new Line(newP1, newP2);
        }


         




    }
}
