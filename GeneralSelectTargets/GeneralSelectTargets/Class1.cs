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

[assembly: CommandClass(typeof(GeneralSelectTargets.Class1))] 
namespace GeneralSelectTargets
{
    public class Class1
    {
        public static Point3d pt;

        [CommandMethod("ASD")]
        public static void SelectItems()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Entity entity = null;
            DBObjectCollection EntityCollection = new DBObjectCollection();
            PromptSelectionResult ents = ed.GetSelection();
            //LayerTableRecord currentLayer = GetCurrentLayer(db);
            List<Point3d> Circles = new List<Point3d>();
            List<string[]> CirclesNumber = new List<string[]>();
            //string savePath = Path.Combine(startPath, "CAD_REVIT_DATA");
            CursorCoords();
            ed.WriteMessage(pt.X.ToString() + "," + pt.Y.ToString());
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);



                        getBlockAttributes(transaction, id, ed);
                        if (entity != null &  entity.GetType().Name == "Circle")
                        {
                            EntityCollection.Add(entity);
                            Point3d maxP = entity.GeometricExtents.MaxPoint;
                            Point3d minP = entity.GeometricExtents.MinPoint;
                            Point3d res = GetCircleCenterPointAndRadius(maxP, minP);
                            Circles.Add(res);
                        }
                        else if (  entity.GetType().Name == "BlockReference")
                        {
                            CirclesNumber.Add(getBlockAttributes(transaction, id, ed));
                        }
                        else if (entity.GetType().Name == "PloyLine" )
                        {
                            Polyline pl = (Polyline)transaction.GetObject(id, OpenMode.ForWrite, true);
                            ed.WriteMessage("test");
                        }

                    }
                    transaction.Commit();
                }
                Matrix3d m3 =  ed.CurrentUserCoordinateSystem;
                ed.WriteMessage(ed.CurrentUserCoordinateSystem.ToString());
            }  
        }


        private static string[] getBlockAttributes(Transaction tr, ObjectId blkId, Editor ed)
        {
            string[] point = null;
            BlockReference blkRef = (BlockReference)tr.GetObject(blkId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(blkRef.BlockTableRecord, OpenMode.ForRead);
            ed.WriteMessage("\nBlock: " + btr.Name);

            BlockTable bt = (BlockTable)tr.GetObject(HostApplicationServices.WorkingDatabase.BlockTableId, OpenMode.ForRead);
            ObjectId ModelSpaceID = bt[BlockTableRecord.ModelSpace];


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


        [CommandMethod("CC")] 
        public static void CursorCoords() 
        { 
            var doc = Application.DocumentManager.MdiActiveDocument; 
            if (doc == null) return; 
            var ed = doc.Editor; 

            ed.PointMonitor += (s, e) => 
            { 
                var ed2 = (Editor)s; 
                if (ed2 == null) return; 
                // If the call is just to set the last point, ignore 

                if (e.Context.History == PointHistoryBits.LastPoint) 
                    return; 
                // Get the inverse of the current UCS matrix, to display in UCS  
                var ucs = ed2.CurrentUserCoordinateSystem.Inverse(); 

                // Checked whether the point was snapped to 
                var snapped = (e.Context.History & PointHistoryBits.ObjectSnapped) > 0; 

                // Transform the snapped or computed point to the current UCS 
                pt = (snapped ? e.Context.ObjectSnappedPoint :  e.Context.ComputedPoint).TransformBy(ucs);
                // Display the point with each ordinate at 4 decimal places 


                try 
                { 
                    // ed2.WriteMessage("{0}: {1:F4}\n", snapped ? "Snapped" : "Found", pt); 

                } 
                catch (Autodesk.AutoCAD.Runtime.Exception ex) 
                { 
                    if (ex.ErrorStatus != ErrorStatus.NotApplicable) 
                        throw; 
                } 
            }; 
        } 
    }
}
