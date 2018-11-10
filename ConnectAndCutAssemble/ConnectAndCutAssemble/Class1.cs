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


[assembly: CommandClass(typeof(ConnectAndCutAssemble.Class1))]
namespace ConnectAndCutAssemble
{ 
    public class Class1
    {

        public static Point3d pt;

        [CommandMethod("trr")]
        public static void Trr()
        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3d[] pt1 = new Point3d[2];
            Point3d[] pt2 = new Point3d[2];
            CursorCoords();
            bool flag1 = GetPoints(db, ed, ref pt1);
            Point3d cuP1 = pt;
            if (!flag1) return; 
            bool flag2 = GetPoints(db, ed, ref pt2);
            Point3d cuP2 = pt;
            if (!flag2) return;
            Point3d croPt = GetCrossPoint(pt1, pt2);

            

        }



        private static Point3d CmpDist(Point3d[] pt,Point3d target)
        {
            double L1 = GetDist(pt[0], target);
            double L2 = GetDist(pt[1], target);

            return L1 < L2 ? pt[0] : pt[1];
        }

        private static double GetDist(Point3d pt1, Point3d pt2)
        {
            double dx = pt1.X - pt2.X;
            double dy = pt1.Y - pt2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }



        private static bool GetPoints(Database db, Editor ed, ref Point3d[] pt)
        {

            DBObjectCollection EntityCollection = new DBObjectCollection();
            PromptSelectionResult ents = ed.GetSelection();
            Entity entity = null;
            Point3d maxP = new Point3d();
            Point3d minP = new Point3d();
            if (ents.Status == PromptStatus.OK)
            {
                using (Transaction transaction = db.TransactionManager.StartTransaction())
                {
                    SelectionSet SS = ents.Value;
                    foreach (ObjectId id in SS.GetObjectIds())
                    {
                        entity = (Entity)transaction.GetObject(id, OpenMode.ForWrite, true);
                        Entity entity2 = (Entity)transaction.GetObject(id, OpenMode.ForRead, true);
                        if (entity.GetType().Name == "Line")
                        {
                            Line line = (Line)transaction.GetObject(id, OpenMode.ForRead, true);
                            EntityCollection.Add(entity);
                            maxP = line.StartPoint;
                            minP = line.EndPoint;
                            pt[0] = maxP;
                            pt[1] = minP;

                            return true;
                        }
                    }
                    transaction.Commit();
                }
            }
            return false;

        }
        private static Point3d GetCrossPoint(Point3d[] PT1, Point3d[] PT2)
        {
            Point3d pt1 = PT1[0];
            Point3d pt2 = PT1[1];
            Point3d pt3 = PT2[0];
            Point3d pt4 = PT2[1];
            double m1, m2, b1, b2 , newX, newY;
            m1 = (pt1.Y - pt2.Y) / (pt1.X - pt2.X);
            b1 = pt1.Y - m1 * pt1.X;
            m2 = (pt3.Y - pt4.Y) / (pt3.X - pt4.X);
            b2 = pt3.Y - m2 * pt3.X;

            if (m1.ToString() == "Infinity" || m1.ToString() == "-Infinity")
            {
                newX = (pt1.X + pt2.X)/2;
                newY = m2 * newX + b2; 
            }
            else if (m2.ToString() == "Infinity" || m2.ToString() ==  "-Infinity")
            {

                newX = (pt3.X + pt4.X) / 2;
                newY = m1 * newX + b1;
            }
            else
            {
                newX = (b2 - b1) / (m1 - m2);
                newY = newX * m1 + b1; 
            } 
            return new Point3d(newX, newY, 0); 
        } 


        /// <summary>
        /// Real-time mouse coordinate 
        /// </summary>
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
                pt = (snapped ? e.Context.ObjectSnappedPoint : e.Context.ComputedPoint).TransformBy(ucs);
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
