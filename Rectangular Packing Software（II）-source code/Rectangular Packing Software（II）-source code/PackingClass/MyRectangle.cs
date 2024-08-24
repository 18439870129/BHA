using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PackingClass
{
    public class MyRectangle:IComparable<MyRectangle>
    {        
        private int index;
        private int width;
        private int height;
        private int area;
        private Point lbpoint;
        private Point rtpoint;
        private string status;

        public MyRectangle()
        {

        }

        public MyRectangle(int _index, int _width, int _height, int _area,string _status)
        {
            this.index = _index;
            this.width = _width;
            this.height = _height;
            this.area = _area;
            this.status = _status;
        }
        
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
                
        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        
        public int Area
        {
            get { return area; }
            set { area = value; }
        }
        
        public Point LB_Point
        {
            get { return lbpoint; }
            set { lbpoint = value; }
        }
        
        public Point RT_Point
        {
            get { return rtpoint; }
            set { rtpoint = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }


        //排序，比较的方法
        public int CompareTo(MyRectangle other)
        {
            if (other == null)
                return 1;
            int value = this.Area - other.Area;
            return value;
        }





    }
}
