using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CADPllotTest
{
    class Point
    {
        private double x;
        private double y;
        private double z;
        private double d;
        public Point(double dd, double xx, double yy)
        {
            x = xx;
            y = yy;
            d = dd;
        }
        public Point(double dd, double xx, double yy,double zz)
        {
            x = xx;
            y = yy;
            z = zz;
        }

        public double X
        {
            get { return x; }
        }
        public double Y
        {
            get { return y; }
        }
        public double Z
        {
            get { return z; }
        }
        public double D
        {
            get { return d; }
        }
    }
}
