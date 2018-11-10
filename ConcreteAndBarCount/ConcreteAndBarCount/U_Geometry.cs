using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Geometry;  
using Autodesk.AutoCAD.DatabaseServices; 
namespace ConcreteAndBarCount
{
    class U_Geometry
    {
        public Point3d StartPoint { get; set; }
        public Point3d EndPoint { get; set; }
        public Point3d Center { get; set; }
        public string Type { get; set; }
        public double Radius { get; set; }
        public double Length { get; set; }
        public double Area { get; set; }
        public string Text { get; set; }
        public string OutputStr { get; }
        public string Layer { get; }
        private Line line;
        private Arc arc;
        private Circle circle;
        private Polyline polyline;
        private MText mText;
        private DBText dbText;

        public U_Geometry(Object Obentity)
        {
            if (Obentity.GetType().Name == "Line")
            {
                this.line = (Line)Obentity;

                this.Type = this.line.GetType().Name;
                this.Length = this.line.Length;
                this.StartPoint = this.line.StartPoint;
                this.EndPoint = this.line.EndPoint;
                this.Layer = this.line.Layer;

                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.Length) +
                            GeoToString(this.StartPoint) +
                            GeoToString(this.EndPoint);
            }
            else if (Obentity.GetType().Name == "Arc")
            {
                this.arc = (Arc)Obentity;

                this.Type = this.arc.GetType().Name;
                this.Length = this.arc.Length;
                this.StartPoint = this.arc.StartPoint;
                this.EndPoint = this.arc.EndPoint;
                this.Center = this.arc.Center;
                this.Radius = this.Radius;
                this.Layer = this.arc.Layer;
                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.Length) +
                            GeoToString(this.Radius) +
                            GeoToString(this.StartPoint) +
                            GeoToString(this.EndPoint) +
                            GeoToString(this.Center);

            }
            else if (Obentity.GetType().Name == "Circle")
            {
                this.circle = (Circle)Obentity;

                this.Radius = this.circle.Radius;
                this.Type = this.circle.GetType().Name;
                this.Length = Math.PI * this.circle.Radius * 2;
                this.StartPoint = this.circle.StartPoint;
                this.EndPoint = this.circle.EndPoint;
                this.Center = this.circle.Center;
                this.Layer = this.circle.Layer;
                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.Length) +
                            GeoToString(this.Radius) +
                            GeoToString(this.StartPoint) +
                            GeoToString(this.EndPoint) +
                            GeoToString(this.Center);

            }
            else if (Obentity.GetType().Name == "Polyline")
            {
                this.polyline = (Polyline)Obentity;

                this.Type = this.polyline.GetType().Name;
                this.Length = this.polyline.Length;
                this.StartPoint = this.polyline.StartPoint;
                this.EndPoint = this.polyline.EndPoint;
                this.Area = this.polyline.Area;
                this.Layer = this.polyline.Layer;
                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.Length) +
                            GeoToString(this.Area) +
                            GeoToString(this.StartPoint) +
                            GeoToString(this.EndPoint);
            }
            else if (Obentity.GetType().Name == "MText")
            {
                this.mText = (MText)Obentity;

                this.Type = this.mText.GetType().Name;
                this.Text = this.mText.Text;
                this.StartPoint = this.mText.Location;
                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.StartPoint);
            }
            else if (Obentity.GetType().Name == "DBText")
            {
                this.dbText = (DBText)Obentity;

                this.Type = this.dbText.GetType().Name;
                this.Text = this.dbText.TextString;
                this.StartPoint = this.dbText.Position;
                OutputStr = GeoToString(this.Layer) +
                            GeoToString(this.Type) +
                            GeoToString(this.StartPoint);

            }

        }

            private string GeoToString(Point3d point)
        { 
            return "(" + point.X.ToString() + "," + point.Y.ToString() + "," + point.Z.ToString() + ")";
        }

        private string GeoToString(string str)
        {
            return str + " ";
        }

        private string GeoToString(double str)
        {
            return str.ToString() + " ";
        }


       
    }
}
