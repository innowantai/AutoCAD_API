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
using System.Windows.Forms;
using Autodesk.AutoCAD.PlottingServices;
using System.IO;
using ExcelClass;


[assembly: CommandClass(typeof(CADPllotTest.Class1))]
namespace CADPllotTest
{
    public class Class1
    {
        [CommandMethod("e2p")]
        public static void ChangePlotSetting()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = ".\\";
            dialog.Filter = "xls files (*.*)|*.xlsx;*.xls;";
            dialog.InitialDirectory = @"~\desktop";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string excelPath = dialog.FileName;
                CADProcess(excelPath);
            } 
        }
         


        private static bool CADProcess(string excelPath)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            Database acCurDb = acDoc.Database;

            PromptStringOptions pStrOpts = new PromptStringOptions("\n 輸入預處理之excel分頁\n單頁 or \n多頁 (1,3,4 or 1-5) :  ");
            pStrOpts.AllowSpaces = true;
            PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts); 
            string num = pStrRes.StringResult;

            pStrOpts = new PromptStringOptions("\n 是否繪製里程(1 : 是 , 0 : 否):  ");
            pStrOpts.AllowSpaces = true;
            pStrRes = acDoc.Editor.GetString(pStrOpts);
            string num2 = pStrRes.StringResult;

            if (num.Trim() == "") return false;

            int kk = 1;
            List<string[,]> allData = new List<string[,]>();
            List<string> label = new List<string>();
            List<double> angle = new List<double>();
            List<Point[]> milePoints = new List<Point[]>();
            List<Point[]> labelPoints = new List<Point[]>();
            List<Point[]> BoundaryCoordinate = new List<Point[]>();
            bool mileOpen = num2 == "1" ? true : false; 
            try
            { 
                allData = ExcelClass.ExcelSaveAndRead.Read(excelPath, 3, 1, num);
                foreach (var data in allData)
                {
                    List<Point> Data = DataProces(data);
                    CAD_PLOT(Data, acDoc, ed, acCurDb);

                    GetMileInformation(Data, ref milePoints, ref labelPoints, ref label, ref angle, mileOpen);
                    GetMileageBoundaryInformation(ref BoundaryCoordinate, ref allData, Data, kk, mileOpen);
                    kk = kk + 1;
                }

                MileageProces(acDoc,ed,acCurDb,milePoints,BoundaryCoordinate,labelPoints,label,angle, mileOpen);
            }
            catch (System.Exception)
            {
                MessageBox.Show("輸入之sheet有誤");
                return false;
            }
            return true;
        }

        private static bool MileageProces(Document acDoc, Editor ed, Database acCurDb, 
                                        List<Point[]> milePoints, List<Point[]> BoundaryCoordinate, 
                                        List<Point[]> labelPoints, List<string> label, List<double> angle,bool mileOpen )
        {
            if (!mileOpen) return false; 

            foreach (Point[] mm in milePoints)
                CAD_PLOT(GetVerticalPoinyByDistancd(2000, mm), acDoc, ed, acCurDb);

            foreach (Point[] bb in BoundaryCoordinate)
                CAD_PLOT(GetVerticalPoinyByDistancd(160000, bb), acDoc, ed, acCurDb);
             
            for (int kk = 0; kk < labelPoints.Count; kk++) 
                CAD_Text(acDoc, acCurDb, GetVerticalPoinyByDistancd(5000, labelPoints[kk]), label[kk], angle[kk]); 

            return true;

        }

      
        private static bool GetMileInformation(List<Point> tmpData, ref List<Point[]> milePoints, ref List<Point[]> labelPoints, ref List<string> label, ref List<double> angle, bool mileOpen)
        {
            if (!mileOpen) return false; 

            for (int i = 0; i < tmpData.Count; i++)
            {
                if (tmpData[i].D % 20 == 0)
                {
                    Point[] points = new Point[2];
                    if (i != tmpData.Count - 1)
                    {
                        points[0] = new Point(tmpData[i].D, tmpData[i].X, tmpData[i].Y);
                        points[1] = new Point(tmpData[i + 1].D, tmpData[i + 1].X, tmpData[i + 1].Y);
                    }
                    else
                    {
                        points[0] = new Point(tmpData[i].D, tmpData[i].X, tmpData[i].Y);
                        points[1] = new Point(tmpData[i - 1].D, tmpData[i - 1].X, tmpData[i - 1].Y);
                    }

                    milePoints.Add(points);

                    if (tmpData[i].D % 100 == 0)
                    {
                        string tmp1 = (Math.Floor(tmpData[i].D / 1000)).ToString();
                        string tmp2 = (tmpData[i].D - Math.Floor(tmpData[i].D / 1000) * 1000).ToString("000");
                        label.Add(tmp1 + "K+" + tmp2);
                        //labelPoints.Add(GetTextCoor(num2, points));
                        labelPoints.Add(points);
                        angle.Add(GetAngle(points));
                    }
                }
            }
            return true;
        }

        private static bool GetMileageBoundaryInformation(ref List<Point[]> BoundaryCoordinate, ref List<string[,]> allData, List<Point> Data, int kk, bool mileOpen)
        {
            if (!mileOpen) return false;

            int ll = Data.Count;
            if (kk == 1 && allData.Count == 1)
            {
                BoundaryCoordinate.Add(new Point[2] { new Point(Data[0].D, Data[0].X, Data[0].Y),
                                                              new Point(Data[1].D, Data[1].X, Data[1].Y) });
                BoundaryCoordinate.Add(new Point[2] { new Point(Data[ll - 1].D, Data[ll - 1].X, Data[ll - 1].Y),
                                                              new Point(Data[ll - 2].D, Data[ll - 2].X, Data[ll - 2].Y) });
            }
            else if (kk == allData.Count)
            {
                BoundaryCoordinate.Add(new Point[2] { new Point(Data[ll - 1].D, Data[ll - 1].X, Data[ll - 1].Y),
                                                              new Point(Data[ll - 2].D, Data[ll - 2].X, Data[ll - 2].Y) });
            }
            else if (kk == 1)
            {
                BoundaryCoordinate.Add(new Point[2] { new Point(Data[0].D, Data[0].X, Data[0].Y),
                                                              new Point(Data[1].D, Data[1].X, Data[1].Y) });
            }
            return true;
        }

        private static List<Point> DataProces(string[,] exData)
        {
            double[,] data = new double[exData.GetLength(0), 3];
            List<int> ngPo = new List<int>();
            List<Point> Points = new List<Point>();
            for (int j = 0; j < exData.GetLength(0); j++)
            {
                if (exData[j, 0].Trim() != "" && exData[j, 1].Trim() != "" && exData[j, 2].Trim() != "")
                {
                    Points.Add(new Point(double.Parse(exData[j, 0].Trim()), double.Parse(exData[j, 1].Trim()), double.Parse(exData[j, 2].Trim())));
                }
            }
            return Points;
        }

        private static List<Point> GetVerticalPoinyByDistancd(double D, Point[] Data)
        {
            double resX0, resY0, resX1, resY1;
            double m = (Data[1].Y - Data[0].Y) / (Data[1].X - Data[0].X);
            double tmp = Math.Sqrt(D * D / (m * m + 1));
            resY0 = Data[0].Y + tmp;
            resY1 = Data[0].Y - tmp;
            resX0 = Data[0].X - m * tmp;
            resX1 = Data[0].X + m * tmp;
            List<Point> res = new List<Point>();
            res.Add(new Point(0, resX0, resY0));
            res.Add(new Point(0, resX1, resY1));
            return res;
        }

        private static double GetAngle(Point[] point)
        {
            return Math.Atan((point[1].Y - point[0].Y) / (point[1].X - point[0].X));
        }

        private static Point[] GetTextCoor(Double D, Point[] Data)
        { 
            double m = (Data[1].Y - Data[0].Y) / (Data[1].X - Data[0].X);
            double tmp = Math.Sqrt(D * D / (m * m + 1));
            double resX0, resY0, resX1, resY1;
            resY0 = Data[0].Y + tmp;
            resY1 = Data[0].Y - tmp;
            resX0 = Data[0].X + m * tmp;
            resX1 = Data[0].X - m * tmp;
            return new Point[] { new Point(0, resX1, resY1), new Point(0, Data[1].X, Data[1].Y) };

        }

        private static void CAD_PLOT(List<Point> data, Document acDoc, Editor ed, Database acCurDb)
        {
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                for (int i = 0; i < data.Count - 1; i++)
                {
                    using (Line acLine = new Line(
                        new Point3d(data[i].X, data[i].Y, 0),
                        new Point3d(data[i + 1].X, data[i + 1].Y, 0)))
                    {
                        acBlkTblRec.AppendEntity(acLine);
                        acTrans.AddNewlyCreatedDBObject(acLine, true);
                    }
                }
                acTrans.Commit();
            }
        }

        private static void CAD_Text(Document acDoc, Database acCurDb, List<Point> point, string text, double angle)
        {
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                // Open the Block table record Model space for write
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                // Create a single-line text object
                DBText acText = new DBText();
                acText.SetDatabaseDefaults();
                acText.Position = new Point3d(point[1].X - 1500, point[1].Y - 12000, 0);
                acText.Height = 5000;
                acText.TextString = text;
                acText.Rotation = angle;
                // Change the oblique angle of the text object to 45 degrees(0.707 in   radians)
                // acText.Oblique = 0.707;
                acBlkTblRec.AppendEntity(acText);
                acTrans.AddNewlyCreatedDBObject(acText, true);
                // Save the changes and dispose of the transaction
                acTrans.Commit();
            }
        }  
    }




}
