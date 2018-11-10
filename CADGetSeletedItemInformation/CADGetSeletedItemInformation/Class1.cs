using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.PlottingServices;
using System.IO;
using ExcelClass;


[assembly: CommandClass(typeof(CADGetSeletedItemInformation.Class1))]
namespace CADGetSeletedItemInformation
{
    public class Class1
    {
        public static string startPath =  Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        [CommandMethod("qq")]
        public static void Selected()
        { 
            selectedTargets(); 
        }



        private static void selectedTargets()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Entity entity = null;
            DBObjectCollection EntityCollection = new DBObjectCollection();
            PromptSelectionResult ents = ed.GetSelection();
            LayerTableRecord currentLayer = GetCurrentLayer(db);
            List<Point3d> Circles = new List<Point3d>();
            List<string[]> CirclesNumber = new List<string[]>();
            string savePath = Path.Combine(startPath, "CAD_REVIT_DATA");
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);

                        if (entity != null & entity.Layer == currentLayer.Name & entity.GetType().Name == "Circle")
                        {
                            EntityCollection.Add(entity);
                            Point3d maxP = entity.GeometricExtents.MaxPoint;
                            Point3d minP = entity.GeometricExtents.MinPoint;
                            Point3d res = GetCircleCenterPointAndRadius(maxP, minP);
                            Circles.Add(res);
                        }
                        else if (entity.Layer == currentLayer.Name & entity.GetType().Name == "BlockReference")
                        {
                            CirclesNumber.Add(getBlockAttributes(transaction, id, ed));
                        }
                    }
                    transaction.Commit();
                }

                Directory.CreateDirectory(savePath);

                saveToText(Circles,Path.Combine(savePath,"Result.txt"));
                saveToText(CirclesNumber,Path.Combine(savePath, "ResultTree.txt"));
            } 
            ed.WriteMessage("\n" + currentLayer.Name.ToString() );
        }

        private static string[] getBlockAttributes(Transaction tr, ObjectId blkId, Editor ed)
        {
            string[] point = null;
            BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
            ed.WriteMessage("\nBlock: " + btr.Name);
            btr.Dispose();
            AttributeCollection attCol = blkRef.AttributeCollection;
            foreach (ObjectId attId in attCol)
            {
                AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                string str = ("\n  Attribute Tag: " + attRef.Tag + "\n    Attribute String: " + attRef.TextString);
                ed.WriteMessage(str);
                if (attRef.Tag == "POINT")
                {
                    point = new string[] { Math.Round(blkRef.Position.X, 1, MidpointRounding.AwayFromZero).ToString(),
                                           Math.Round(blkRef.Position.Y, 1, MidpointRounding.AwayFromZero).ToString(), attRef.TextString };
                }
            }

            return point;
        }


        private static void saveToText(List<Point3d> Circles, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < Circles.Count; i++)
            {
                sw.WriteLine(Math.Round(Circles[i].X, 1, MidpointRounding.AwayFromZero).ToString() + "," +
                             Math.Round(Circles[i].Y, 1, MidpointRounding.AwayFromZero).ToString() + "," + Circles[i].Z.ToString());
                sw.Flush();
            }
            sw.Close();
        }

        private static void saveToText(List<string[]> CirclesNumber, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            for (int i = 0; i < CirclesNumber.Count; i++)
            {
                string[] tmp = CirclesNumber[i];
                if (tmp != null)
                {
                    sw.WriteLine(tmp[0] + "," + tmp[1] + "," + tmp[2]);
                    sw.Flush();

                }
            }
            sw.Close();
        }
        private static Point3d GetCircleCenterPointAndRadius(Point3d maxP, Point3d MinP)
        {
            double centerX = (maxP.X + MinP.X) / 2;
            double centerY = (maxP.Y + MinP.Y) / 2;
            double radius = maxP.X - centerX;
            Point3d res = new Point3d(centerX, centerY, radius);
            return res;

        }
        private static LayerTableRecord GetCurrentLayer(Database db)
        {
            LayerTableRecord layer = new LayerTableRecord();
            using (Transaction transaction = db.TransactionManager.StartTransaction())
            {
                layer = transaction.GetObject(db.Clayer, OpenMode.ForRead) as LayerTableRecord;
            }
            return layer;

        }



        [CommandMethod("qqq")]
        public void ListAttributes()
        {

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            Transaction tr = db.TransactionManager.StartTransaction();
            // Start the transaction 
            try
            {

                // Build a filter list so that only
                // block references are selected

                TypedValue[] filList = new TypedValue[1] { new TypedValue((int)DxfCode.Start, "INSERT") };
                SelectionFilter filter = new SelectionFilter(filList);
                PromptSelectionOptions opts = new PromptSelectionOptions();
                opts.MessageForAdding = "Select block references: ";
                PromptSelectionResult res = ed.GetSelection(opts, filter);
                // Do nothing if selection is unsuccessful
                if (res.Status != PromptStatus.OK)
                    return;

                SelectionSet selSet = res.Value;
                ObjectId[] idArray = selSet.GetObjectIds();
                foreach (ObjectId blkId in idArray)
                {
                    BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);
                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
                    ed.WriteMessage("\nBlock: " + btr.Name);
                    btr.Dispose();
                    AttributeCollection attCol = blkRef.AttributeCollection;
                    foreach (ObjectId attId in attCol)
                    {
                        AttributeReference attRef = (AttributeReference)tr.GetObject(attId, OpenMode.ForRead);
                        string str = ("\n  Attribute Tag: " + attRef.Tag + "\n    Attribute String: " + attRef.TextString);
                        ed.WriteMessage(str);
                    }
                }
                tr.Commit();
            }

            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ed.WriteMessage(("Exception: " + ex.Message));
            }

            finally
            {
                tr.Dispose();
            }

        }







        [CommandMethod("bbb")]
        public void GETXDATA()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("获取扩展数据XDATA\n");
            PromptEntityOptions entOps = new PromptEntityOptions("请选择要打开的对象\n");
            PromptEntityResult entRes = ed.GetEntity(entOps);
            if (entRes.Status != PromptStatus.OK)
            {
                ed.WriteMessage("选择对象失败，退出"); return;
            }
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Entity ent = (Entity)tr.GetObject(entRes.ObjectId, OpenMode.ForRead);
                ResultBuffer resBuf = ent.XData;
                if (resBuf != null)
                {
                    var iter = resBuf.GetEnumerator();
                    while (iter.MoveNext())
                    {
                        TypedValue tmpVal = (TypedValue)iter.Current;
                        ed.WriteMessage(tmpVal.TypeCode.ToString() + ":");
                        ed.WriteMessage(tmpVal.Value.ToString() + "\n");
                    }
                }
            }
        }




        private static void FindBlockBackup()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Entity entity = null;
            DBObjectCollection EntityCollection = new DBObjectCollection();
            PromptSelectionResult ents = ed.GetSelection();
            LayerTableRecord currentLayer = GetCurrentLayer(db);
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);

                        if (entity != null)
                        {
                            Database db2 = entity.Database;
                            var dra = entity.Drawable;
                            BlockTable bt = (BlockTable)transaction.GetObject(db.BlockTableId, OpenMode.ForRead);
                            BlockTableRecord btr = (BlockTableRecord)transaction.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                            EntityCollection.Add(entity);
                            ed.WriteMessage("\n" + entity.Layer.ToString());
                        }
                    }
                    transaction.Commit();
                }
            }

            ed.WriteMessage("\n" + currentLayer.Name.ToString());
        }

    }
}
