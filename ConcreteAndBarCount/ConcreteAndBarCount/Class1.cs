using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.PlottingServices;
using System.IO;
using ExcelClass;

[assembly: CommandClass(typeof(ConcreteAndBarCount.Class1))]
namespace ConcreteAndBarCount
{
    public class Class1
    {

        public static Document doc  ;
        public static Database db ;
        public static Editor ed  ;
        public static Form1 form;
        public static EXCEL Excel;
        public static string folder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        public static string SavePath;
        public static List<string> CheckedStr;

        [CommandMethod("zz")]
        public static void Count()
        { 
            doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            db = doc.Database;
            ed = doc.Editor; 

            form = new Form1(LayersToList(db, ed), doc, db, ed);
            Application.ShowModalDialog(form);
            if (form.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SavePath = Path.Combine(folder, "CADResults");
                Directory.CreateDirectory(SavePath);
                CheckedStr = form.CheckStr;


                DBObjectCollection EntityCollection = new DBObjectCollection();
                PromptSelectionResult ents = ed.GetSelection();
                Main_Process(ents);

                if (form.chText.Checked)
                {
                    Method_3_GetTextWordsIndicatedLayers(ents);
                }
            }


        }

        public static void Main_Process(PromptSelectionResult ents)
        {
            Entity entity = null;
            //LayerTableRecord currentLayer = GetCurrentLayer(db);  
            List<string> ResData = new List<string>();
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    Dictionary<string, List<string>> Res = new Dictionary<string, List<string>>();
                    List<U_Geometry> OBJs = new List<U_Geometry>();
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true); 
                        string TargetslayName = entity.Layer;
                        if (entity != null & TargetslayName.IndexOf(form.ResLayer) != -1)
                        {
                            foreach (string ss in CheckedStr)
                            {
                                if (entity.GetType().Name.IndexOf(ss) != -1)
                                {
                                    Object obj = transaction.GetObject(id, OpenMode.ForWrite, true);
                                    OBJs.Add(new U_Geometry(obj)); 
                                }
                            }
                        }
                    }
                    transaction.Commit();

                    List<List<U_Geometry>> RES = new List<List<U_Geometry>>();
                    int[] Is_Pick = new int[OBJs.Count];
                    for (int i = 0; i < OBJs.Count; i++)
                    {
                        if (Is_Pick[i] == 1)
                            continue;

                        int j = 0;
                        Is_Pick[i] = 1;
                        List<U_Geometry> tmp = new List<U_Geometry>();
                        tmp.Add(OBJs[i]);
                        while (j < OBJs.Count)
                        {
                            if (Is_Pick[j] == 0 && Is_Connect(tmp[0], OBJs[j]))
                            {
                                tmp.Add(OBJs[j]);
                                Is_Pick[j] = 1;
                                j = 0;
                            }
                            else if (tmp.Count > 1 && Is_Pick[j] == 0 && Is_Connect(tmp[tmp.Count - 1], OBJs[j]))
                            {
                                tmp.Add(OBJs[j]);
                                Is_Pick[j] = 1;
                                j = 0;
                            }
                            j++;
                        }

                        RES.Add(tmp);
                    }

                    if (RES.Count != 0)
                    {
                        StreamWriter sw = new StreamWriter(Path.Combine(SavePath, "CADResult.txt"));
                        List<string> To_Excel = new List<string>();
                        foreach (List<U_Geometry> res in RES)
                        {
                            foreach (U_Geometry ss in res)
                            {
                                sw.WriteLine(ss.OutputStr);
                                sw.Flush();
                                To_Excel.Add(ss.OutputStr);
                            }
                        }
                        sw.Close();

                        string[,] TO_EXCEL = new string[To_Excel.Count, 5];
                        foreach (string ss in To_Excel)
                        {

                        }


                    }
                }
            }
        }

         

        public static void Method_3_GetTextWordsIndicatedLayers(PromptSelectionResult ents)
        {
            Entity entity = null; 
            //LayerTableRecord currentLayer = GetCurrentLayer(db);  
            List<string> ResData = new List<string>();
            List<MText> mtext = new List<MText>();
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);

                        if (entity != null & entity.GetType().Name.IndexOf("Text") != -1 & entity.Layer.IndexOf(form.ResLayer) != -1)
                        {
                            MText text = (MText)transaction.GetObject(id, OpenMode.ForWrite, true);
                            mtext.Add(text);
                            
                            //ed.WriteMessage(res + "\n");
                        }

                    }
                    transaction.Commit();
                }
            }


            if (mtext.Count != 0)
            {

                var find = mtext.OrderBy(t => t.Location.X); 
                Dictionary<double, List<MText>> res = new Dictionary<double, List<MText>>();
                foreach (MText mm in find)
                {
                    List<MText> tmp = new List<MText>();
                    tmp = res.ContainsKey(mm.Location.Y) ? res[mm.Location.Y] : new List<MText>();
                    tmp.Add(mm);
                    res[mm.Location.Y] = tmp;
                    ed.WriteMessage(mm.Text + "\n");
                }
                 

                StreamWriter sw = new StreamWriter(Path.Combine(SavePath, "Text.txt"));
                foreach (KeyValuePair<double, List<MText>> RR in res.OrderByDescending(e => e.Key))
                {
                    string ss = "";
                    foreach (MText mm in RR.Value)
                    {
                        ss += mm.Text + " ";
                    }
                    sw.WriteLine(ss);
                    sw.Flush();
                }
                sw.Close();
            }




        }

        public static List<string> LayersToList(Database db, Editor ed)
        {
            List<string> lstlay = new List<string>();

            LayerTableRecord layer;
            using (Transaction transaction = db.TransactionManager.StartOpenCloseTransaction())
            {
                LayerTable lt = transaction.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                foreach (ObjectId layerId in lt)
                {
                    layer = transaction.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                    lstlay.Add(layer.Name); 
                }

            }
            return lstlay;
        }
         

        private static bool Is_Connect(U_Geometry OBJ_1, U_Geometry OBJ_2)
        {
            if (Is_SamePoint(OBJ_1.StartPoint, OBJ_2.StartPoint) ||
                Is_SamePoint(OBJ_1.EndPoint, OBJ_2.EndPoint) ||
                Is_SamePoint(OBJ_1.StartPoint, OBJ_2.EndPoint) ||
                Is_SamePoint(OBJ_1.EndPoint, OBJ_2.StartPoint))
            {
                return true;
            }
            return false;
        }

        private static bool Is_SamePoint(Point3d p1, Point3d p2)
        {
            if (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z)
            {
                return true;
            }
            return false;
        }




        //public static void GetObjects()
        //{

        //    Entity entity = null;
        //    DBObjectCollection EntityCollection = new DBObjectCollection();
        //    PromptSelectionResult ents = ed.GetSelection();
        //    List<string> ResData = new List<string>();
        //    if (ents.Status == PromptStatus.OK)
        //    {
        //        List<Point3d[]> point = new List<Point3d[]>();
        //        using (Transaction transaction = db.TransactionManager.StartTransaction())
        //        {
        //            SelectionSet SS = ents.Value;
        //            foreach (ObjectId id in SS.GetObjectIds())
        //            {
        //                entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);
        //                string TargetslayName = entity.Layer;
        //                if (entity != null & TargetslayName.IndexOf(form.ResLayer) != -1 & (entity.GetType().Name == "Line" | entity.GetType().Name == "Arc") | entity.GetType().Name == "Circle")
        //                {
        //                    if (entity.GetType().Name == "Line")
        //                    {
        //                        Line Line = (Line)transaction.GetObject(id, OpenMode.ForWrite, true);
        //                        Point3d[] pp = new Point3d[] { Line.StartPoint, Line.EndPoint };
        //                        point.Add(pp);



        //                    }
        //                }
        //            }
        //            transaction.Commit();
        //        }
        //        CreatePolyLines(point);
        //    }
        //}

        //private static void CreatePolyLines(List<Point3d[]> points)
        //{
        //    using (Transaction transaction = db.TransactionManager.StartTransaction())
        //    {
        //        ///////
        //        BlockTable acBlkTbl;
        //        acBlkTbl = transaction.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;

        //        // Open the Block table record Model space for write
        //        BlockTableRecord acBlkTblRec;
        //        acBlkTblRec = transaction.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
        //                                        OpenMode.ForWrite) as BlockTableRecord;

        //        // Create a polyline with two segments (3 points)
        //        using (Polyline acPoly = new Polyline())
        //        {
        //            foreach (Point3d[] pp in points)
        //            {
        //                acPoly.AddVertexAt(0, new Point2d(pp[0][0], pp[0][1]), 0, 0, 0);
        //            }
        //            acPoly.AddVertexAt(0, new Point2d(points[points.Count - 1][1][0], points[points.Count - 1][1][1]), 0, 0, 0);

        //            // Add the new object to the block table record and the transaction
        //            acBlkTblRec.AppendEntity(acPoly);
        //            transaction.AddNewlyCreatedDBObject(acPoly, true);
        //            transaction.Commit();
        //        }
        //    }
        //}

    }
}
