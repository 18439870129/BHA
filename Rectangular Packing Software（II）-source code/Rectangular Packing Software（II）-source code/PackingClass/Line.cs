using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PackingClass
{
    public class Line
    {
        private Point startpoint;
        private Point endpoint;
        private int length;

        public Line()
        {

        }

        public Line(Point _spt, Point _ept)
        {
            this.startpoint = _spt;
            this.endpoint = _ept;
        }
        
        public Point Start_Point
        {
            get { return startpoint; }
            set { startpoint = value; }
        }
        
        public Point End_Point
        {
            get { return endpoint; }
            set { endpoint = value; }
        }

        public int Length
        {
            get 
            {
                if (this.startpoint.Y == this.endpoint.Y)
                {
                    this.length = this.endpoint.X - this.startpoint.X;
                }
                else if (this.startpoint.X == this.endpoint.X)
                {
                    this.length = this.endpoint.Y - this.startpoint.Y;
                }

                return this.length;            
            }
        }


    }
}
