using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PackingClass
{
    public class BLL
    {
        public BLL()
        {

        }

        //寻找最下方的一条水平线边界
        public static Line FindBottomLine(List<Line> LineList)
        {
            Line b_line = LineList[0];
            foreach (Line line in LineList)
            {
                if (line.Start_Point.Y < b_line.Start_Point.Y)
                {
                    b_line = line;
                }
            }

            return b_line;
        }

        //寻找给定点的边界线段
        public static Line FindFrameLine(List<Line> LineList,Point point)
        {
            Line myline = null;
            foreach (Line line in LineList)
            {
                if (line.Start_Point == point)
                {
                    myline = line;
                    break;
                }
            }

            return myline;
        }

        











    }
}
