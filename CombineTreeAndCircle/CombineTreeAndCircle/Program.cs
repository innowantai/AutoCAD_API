using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CombineTreeAndCircle
{
    class Program
    {
        public static string startPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

        static void Main(string[] args)
        {
            string getPath = Path.Combine(startPath, "CAD_REVIT_DATA"); 
            string path1 = Path.Combine(getPath, "Result.txt");
            string path2 = Path.Combine(getPath, "ResultTree.txt");
            string path3 = Path.Combine(getPath, "treeList.txt");
            List<string[]> circle = TakeOffSamePoint(readTxt(path1));
            List<string[]> treeNumber = TakeOffSamePoint(readTxt(path2));
            Dictionary<string, string> TreeList = getTreeList(path3);
            double[] target = new double[circle.Count];
            string[] NUMBER = new string[circle.Count];

            int kk = 0;
            foreach (string[] number in treeNumber)
            {
                kk = 0;
                int lastkk = -1;
                double lastDist = 1000000000000000; 
                foreach (string[] cc in circle)
                {  
                    double dist = Dist(number[0], number[1], cc[0], cc[1]);

                    if (dist < lastDist & target[kk] == 0)
                    {
                        lastkk = kk;
                        lastDist = dist;
                    }

                    if (dist == 0)
                    {
                        lastkk = kk;
                        lastDist = dist;
                        break;
                    }
                    kk++;
                }

                NUMBER[lastkk] = number[2]; 
                target[lastkk] = 1; 
            }


            kk = 0;
            StreamWriter sw = new StreamWriter(Path.Combine(getPath, "final.txt"));
            foreach (var nn in NUMBER)
            {
                if (nn != null)
                {
                    string[] tmp = circle[kk];
                    string treeH = TreeList.Keys.Contains(nn) ? TreeList[nn] : "0";
                    sw.WriteLine(tmp[0] + "," + tmp[1] + "," + (double.Parse(tmp[2]) * 1).ToString() + "," + treeH + "," + nn);
                    sw.Flush();

                }
                kk++;
            }
            sw.Close();


            //saveOriFiles("oriResult.txt", circle);
            //saveOriFiles("oriResultTree.txt", treeNumber);
        }


        private static Dictionary<string, string> getTreeList(string path)
        {
            Dictionary<string, string> DataDict = new Dictionary<string, string>();
            StreamReader sr = new StreamReader(path);
            while (sr.Peek() != -1)
            {
                string[] tmp = sr.ReadLine().Split(',');
                DataDict[tmp[0]] = tmp[1];
            }
            sr.Close();
            return DataDict;

        }


        private static List<string[]> TakeOffSamePoint(List<string[]> Data)
        { 
            double[] tag = new double[Data.Count];
            for (int kk = 0; kk < Data.Count - 1; kk++)
            {
                string[] dd = Data[kk];
                for (int i = kk + 1; i < Data.Count; i++)
                {
                    string[] tmp = Data[i]; 
                    if (String.Equals(dd[0], tmp[0], StringComparison.OrdinalIgnoreCase) & String.Equals(dd[1], tmp[1], StringComparison.OrdinalIgnoreCase))
                    {
                        tag[i] = -1;
                    }
                } 
            }

            List<string[]> resData = new List<string[]>();
            for (int i = 0; i < Data.Count; i++)
            {
                if (tag[i] != -1)
                {
                    resData.Add(Data[i]);
                }
            }

            return resData;
        }

        private static double Dist(string X1, string Y1, string X2, string Y2)
        {
            double dx = Double.Parse(X1) - Double.Parse(X2);
            double dy = Double.Parse(Y1) - Double.Parse(Y2);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private static List<string[]> readTxt(string path)
        {
            StreamReader sr = new StreamReader(path);
            List<string[]> Data = new List<string[]>();
            while (sr.Peek() != -1)
            {
                Data.Add(sr.ReadLine().Split(','));
            }
            sr.Close();
            return Data;
        } 

        private static void saveOriFiles(string path, List<string[]> Data)
        {
            StreamWriter sw = new StreamWriter(path);
            foreach (string[] dd in Data)
            {
                sw.WriteLine(dd[0] + "," + dd[1] + "," + dd[2]);
                sw.Flush();
            }
            sw.Close();
        }







    }
}
