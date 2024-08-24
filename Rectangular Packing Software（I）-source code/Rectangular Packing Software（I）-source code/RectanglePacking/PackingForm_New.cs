using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PackingClass;
using System.Threading;
using System.Xml;

namespace RectanglePacking
{
    public partial class PackingForm_New : Form
    {
        public int[][] RectangList;  //交错数组，存放矩形数(width,height,area,lbx,lby,rtx,rty,rotateflag)
        //public int[][] RectangList2;  //交错数组，存放矩形数
        public int SpaceWidth;  //板材宽度
        public int SpaceHeight = 1000000;  //板材高度: 设置一个很大的值，代替无限高度
        public int RectangleCount; //矩形件的个数
        public List<int> Distinct_Index = new List<int>();  //不同尺寸矩形的索引
        public List<int> UnPack_Index = new List<int>();  //未排矩形的索引
        //public List<int> Distinct_Index2 = new List<int>();  //不同尺寸矩形的索引
        public List<int> UnPack_Index2 = new List<int>();  //未排矩形的索引
        public List<int> UnPack_Index3 = new List<int>();  //未排矩形的索引
        public float H_Max;
        public bool thread_end;
        public bool thread_end2;
        public bool thread_success;
        public int Result_H;
        public int Result_width;
        public int Result_height;
        //public int Result_rotate;

        public int LB;

        public List<List<List<int>>> UnPack_Index_Filters = new List<List<List<int>>>();
        public List<List<List<Line>>> BLines_Filters = new List<List<List<Line>>>();
        public List<List<List<Line>>> LLines_Filters = new List<List<List<Line>>>();
        public List<List<List<Line>>> RLines_Filters = new List<List<List<Line>>>();
        public List<int> UnPack_Area = new List<int>();

        public List<List<List<int>>> UnPack_Index_Filters2 = new List<List<List<int>>>();
        public List<List<List<Line>>> BLines_Filters2 = new List<List<List<Line>>>();
        public List<List<List<Line>>> LLines_Filters2 = new List<List<List<Line>>>();
        public List<List<List<Line>>> RLines_Filters2 = new List<List<List<Line>>>();
        public List<int> UnPack_Area2 = new List<int>();

        public List<List<List<int>>> UnPack_Index_Filters3 = new List<List<List<int>>>();
        public List<List<List<Line>>> BLines_Filters3 = new List<List<List<Line>>>();
        public List<List<List<Line>>> LLines_Filters3 = new List<List<List<Line>>>();
        public List<List<List<Line>>> RLines_Filters3 = new List<List<List<Line>>>();
        public List<int> UnPack_Area3 = new List<int>();

        public int Index_Compute_ALL;

        public int i_all;
        public int i_all2;
        public int i_all3;
        public List<int> AreaSizeList = new List<int>();

        public int RecoverCount;  //设定追溯深度（即追溯数组的存储容量）

        public int FirstRec_index;  //最优解时的First矩形index
        public int FirstRec_rotate;  //最优解时的First矩形rotate

        public PackingForm_New()
        {
            InitializeComponent();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您要退出软件吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }

      

        #region 将3个线程的碰撞边界结果合并
        public void Combine()
        {            
            foreach (List<List<int>> list in UnPack_Index_Filters2)
            {
                List<List<int>> temp1 = new List<List<int>>();
                foreach (List<int> i_1 in list)
                {
                    List<int> i_2 = new List<int>();
                    foreach (int i in i_1)
                    {
                        i_2.Add(i);
                    }
                    temp1.Add(i_2);
                }
                UnPack_Index_Filters.Add(temp1);
            }
            
            UnPack_Index_Filters2.Clear();  //清空；释放内存

            foreach (List<List<int>> list in UnPack_Index_Filters3)
            {
                List<List<int>> temp1 = new List<List<int>>();
                foreach (List<int> i_1 in list)
                {
                    List<int> i_2 = new List<int>();
                    foreach (int i in i_1)
                    {
                        i_2.Add(i);
                    }
                    temp1.Add(i_2);
                }
                UnPack_Index_Filters.Add(temp1);
            }

            UnPack_Index_Filters3.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in BLines_Filters2)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                BLines_Filters.Add(temp1);
            }

            BLines_Filters2.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in BLines_Filters3)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                BLines_Filters.Add(temp1);
            }

            BLines_Filters3.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in LLines_Filters2)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                LLines_Filters.Add(temp1);
            }

            LLines_Filters2.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in LLines_Filters3)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                LLines_Filters.Add(temp1);
            }

            LLines_Filters3.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in RLines_Filters2)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                RLines_Filters.Add(temp1);
            }

            RLines_Filters2.Clear();  //清空；释放内存

            foreach (List<List<Line>> list in RLines_Filters3)
            {
                List<List<Line>> temp1 = new List<List<Line>>();
                foreach (List<Line> i_1 in list)
                {
                    List<Line> i_2 = new List<Line>();
                    foreach (Line line in i_1)
                    {
                        i_2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    temp1.Add(i_2);
                }
                RLines_Filters.Add(temp1);
            }

            RLines_Filters3.Clear();  //清空；释放内存

        }

        #endregion

        public int Get_Max(int a, int b)
        {
            int c = a;
            if (c < b)
            {
                c = b;
            }
            return c;
        }

        public void Thread_Do()
        {
            List<Line> BLinesList = new List<Line>();
            List<Line> LLinesList = new List<Line>();
            List<Line> RLinesList = new List<Line>();

            #region 循环迭代
            int first_index = 0;

            int firstRec_index = 0;
            int firstRec_rotate = 0;

            #region 每一个矩形都作为第一个，排一次

            i_all2 = (2 * Distinct_Index.Count - 1) / 3 + 1;
            while (i_all2 <= 2*(2 * Distinct_Index.Count - 1) / 3)
            {
                firstRec_index = Distinct_Index[i_all2 / 2];
                if (i_all2 % 2 == 0)  //余数为0
                {
                    firstRec_rotate = 0;
                }
                else
                {
                    firstRec_rotate = 1;
                }

                if (Index_Compute_ALL == 1)
                {
                    first_index = Distinct_Index[i_all2/2];
                    int lbx = 0;
                    int lby = 0;
                    int rtx = 0;
                    int rty = 0;
                    if (i_all2 % 2 == 0)  //余数为0
                    {
                        if (RectangList[first_index][0] <= SpaceWidth & RectangList[first_index][1] <= H_Max)
                        {
                            lbx = 0;  //lbx
                            lby = 0;  //lby
                            rtx = RectangList[first_index][0];  //rtx
                            rty = RectangList[first_index][1];  //rty 

                            UnPack_Index2.Remove(first_index);  //从未排矩形集合中移除                                 
                        }
                        else
                        {
                            #region 特殊处理
                            BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                            LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                            RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                            List<List<int>> ll_temp1 = new List<List<int>>();
                            List<int> temp1 = new List<int>();
                            foreach (int i in UnPack_Index2)
                            {
                                temp1.Add(i);
                            }
                            ll_temp1.Add(temp1);
                            UnPack_Index_Filters2.Add(ll_temp1);

                            List<List<Line>> ll_temp2 = new List<List<Line>>();
                            List<Line> temp2 = new List<Line>();
                            foreach (Line line in BLinesList)
                            {
                                temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp2.Add(temp2);
                            BLines_Filters2.Add(ll_temp2);

                            List<List<Line>> ll_temp3 = new List<List<Line>>();
                            List<Line> temp3 = new List<Line>();
                            foreach (Line line in LLinesList)
                            {
                                temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp3.Add(temp3);
                            LLines_Filters2.Add(ll_temp3);

                            List<List<Line>> ll_temp4 = new List<List<Line>>();
                            List<Line> temp4 = new List<Line>();
                            foreach (Line line in RLinesList)
                            {
                                temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp4.Add(temp4);
                            RLines_Filters2.Add(ll_temp4);

                            BLinesList.Clear();
                            LLinesList.Clear();
                            RLinesList.Clear();

                            UnPack_Index2.Clear();
                            for (int i = 0; i < RectangList.Length; i++)
                            {
                                UnPack_Index2.Add(i);
                            }
                            #endregion
                            i_all2 = i_all2 + 1;
                            continue;
                        }
                    }
                    else
                    {
                        if (RectangList[first_index][1] <= SpaceWidth & RectangList[first_index][0] <= H_Max)
                        {
                            lbx = 0;  //lbx
                            lby = 0;  //lby
                            rtx = RectangList[first_index][1];  //rtx
                            rty = RectangList[first_index][0];  //rty 

                            UnPack_Index2.Remove(first_index);  //从未排矩形集合中移除                                 
                        }
                        else
                        {
                            #region 特殊处理
                            BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                            LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                            RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                            List<List<int>> ll_temp1 = new List<List<int>>();
                            List<int> temp1 = new List<int>();
                            foreach (int i in UnPack_Index2)
                            {
                                temp1.Add(i);
                            }
                            ll_temp1.Add(temp1);
                            UnPack_Index_Filters2.Add(ll_temp1);

                            List<List<Line>> ll_temp2 = new List<List<Line>>();
                            List<Line> temp2 = new List<Line>();
                            foreach (Line line in BLinesList)
                            {
                                temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp2.Add(temp2);
                            BLines_Filters2.Add(ll_temp2);

                            List<List<Line>> ll_temp3 = new List<List<Line>>();
                            List<Line> temp3 = new List<Line>();
                            foreach (Line line in LLinesList)
                            {
                                temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp3.Add(temp3);
                            LLines_Filters2.Add(ll_temp3);

                            List<List<Line>> ll_temp4 = new List<List<Line>>();
                            List<Line> temp4 = new List<Line>();
                            foreach (Line line in RLinesList)
                            {
                                temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp4.Add(temp4);
                            RLines_Filters2.Add(ll_temp4);

                            BLinesList.Clear();
                            LLinesList.Clear();
                            RLinesList.Clear();

                            UnPack_Index2.Clear();
                            for (int i = 0; i < RectangList.Length; i++)
                            {
                                UnPack_Index2.Add(i);
                            }
                            #endregion
                            i_all2 = i_all2 + 1;
                            continue;
                        }
                    }

                    //更新线段边界集合
                    if (rtx < SpaceWidth)
                    {
                        BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                        BLinesList.Add(new Line(new Point(rtx, 0), new Point(SpaceWidth, 0)));
                        LLinesList.Add(new Line(new Point(rtx, 0), new Point(rtx, rty)));
                        LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                        RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));
                    }
                    else if (rtx == SpaceWidth)
                    {
                        BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                        LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                        RLinesList.Add(new Line(new Point(SpaceWidth, rty), new Point(SpaceWidth, SpaceHeight)));
                    }

                    #region 存储排样图形的边界线段状态
                    List<List<int>> ll_temp1_1 = new List<List<int>>();
                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index2)
                    {
                        temp1_1.Add(i);
                    }
                    ll_temp1_1.Add(temp1_1);
                    UnPack_Index_Filters2.Add(ll_temp1_1);

                    List<List<Line>> ll_temp2_1 = new List<List<Line>>();
                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp2_1.Add(temp2_1);
                    BLines_Filters2.Add(ll_temp2_1);

                    List<List<Line>> ll_temp3_1 = new List<List<Line>>();
                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp3_1.Add(temp3_1);
                    LLines_Filters2.Add(ll_temp3_1);

                    List<List<Line>> ll_temp4_1 = new List<List<Line>>();
                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp4_1.Add(temp4_1);
                    RLines_Filters2.Add(ll_temp4_1);

                    #endregion

                }
                else
                {
                    //int control_num = 500;  //如果矩形个数多于该值，则筛选。（还可以根据矩形个数的不同，选取不同的比例或固定个数）
                    //if (Distinct_Index.Count > control_num)
                    //{
                    //    bool flag_88 = false;
                    //    for (int i = 0; i < control_num; i++)
                    //    {
                    //        if (AreaSizeList[i] == i_all2)
                    //        {
                    //            flag_88 = true;
                    //            break;
                    //        }
                    //    }

                    //    if (flag_88 == false)
                    //    {
                    //        i_all2 = i_all2 + 1;
                    //        continue;
                    //    }
                    //}

                    BLinesList.Clear();
                    LLinesList.Clear();
                    RLinesList.Clear();
                    UnPack_Index2.Clear();

                    //确定追溯恢复的索引位置
                    int RecoverIndex = 0;

                    //if (RectangList.Length < 100)
                    //{
                    //    RecoverIndex = 0;
                    //}
                    //else if (RectangList.Length >= 100 & RectangList.Length < 500)
                    //{
                    //    RecoverIndex = UnPack_Index_Filters[i_all2].Count - 50;
                    //}
                    //else if (RectangList.Length >= 500)
                    //{
                    //    RecoverIndex = 0;
                    //}
                    

                    foreach (int i in UnPack_Index_Filters[i_all2][RecoverIndex])
                    {
                        UnPack_Index2.Add(i);
                    }

                    foreach (Line line in BLines_Filters[i_all2][RecoverIndex])
                    {
                        BLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }

                    foreach (Line line in LLines_Filters[i_all2][RecoverIndex])
                    {
                        LLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }

                    foreach (Line line in RLines_Filters[i_all2][RecoverIndex])
                    {
                        RLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }


                }

                bool flag_ok = true;
                while (UnPack_Index2.Count > 0)  //循环终止条件：直到所有的矩形都排完
                {
                    //寻找可排点（寻找最下方的一条水平边界）
                    Line b_line = BLL.FindBottomLine(BLinesList);
                    Line l_line = BLL.FindFrameLine(LLinesList, b_line.Start_Point);
                    Line r_line = BLL.FindFrameLine(RLinesList, b_line.End_Point);

                    if (l_line.End_Point.Y == SpaceHeight || r_line.End_Point.Y == SpaceHeight)
                    {
                        //含板材边界的情况
                        PackingCompute2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                    }
                    else
                    {
                        //不含板材边界的情况
                        PackingCompute_22(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                    }

                    if (BLinesList.Count == 0)
                    {
                        flag_ok = false;
                        break;
                    }
                }

                if (flag_ok)
                {
                    thread_success = true;
                    break;
                }
                else
                {
                    i_all2 = i_all2 + 1;

                    BLinesList.Clear();
                    LLinesList.Clear();
                    RLinesList.Clear();

                    UnPack_Index2.Clear();
                    for (int i = 0; i < RectangList.Length; i++)
                    {
                        UnPack_Index2.Add(i);
                    }
                }
            }
            if (thread_success)
            {
                //记录结果
                Result_H = GetH2(BLinesList);
                Result_width = RectangList[first_index][0];
                Result_height = RectangList[first_index][1];

                FirstRec_index = firstRec_index;
                FirstRec_rotate = firstRec_rotate;

            }

            thread_end = true;

            #endregion
            #endregion

        }

        public void Thread_Do2()
        {
            List<Line> BLinesList = new List<Line>();
            List<Line> LLinesList = new List<Line>();
            List<Line> RLinesList = new List<Line>();

            #region 循环迭代
            int first_index = 0;

            int firstRec_index = 0;
            int firstRec_rotate = 0;

            #region 每一个矩形都作为第一个，排一次

            i_all3 = 2 * (2 * Distinct_Index.Count - 1) / 3 + 1;
            while (i_all3 <= 2 * Distinct_Index.Count - 1)
            {
                firstRec_index = Distinct_Index[i_all3 / 2];
                if (i_all3 % 2 == 0)  //余数为0
                {
                    firstRec_rotate = 0;
                }
                else
                {
                    firstRec_rotate = 1;
                }

                if (Index_Compute_ALL == 1)
                {
                    first_index = Distinct_Index[i_all3/2];
                    int lbx = 0;
                    int lby = 0;
                    int rtx = 0;
                    int rty = 0;
                    if (i_all3 % 2 == 0)  //余数为0
                    {
                        if (RectangList[first_index][0] <= SpaceWidth & RectangList[first_index][1] <= H_Max)
                        {
                            lbx = 0;  //lbx
                            lby = 0;  //lby
                            rtx = RectangList[first_index][0];  //rtx
                            rty = RectangList[first_index][1];  //rty 

                            UnPack_Index3.Remove(first_index);  //从未排矩形集合中移除                                 
                        }
                        else
                        {
                            #region 特殊处理
                            BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                            LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                            RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                            List<List<int>> ll_temp1 = new List<List<int>>();
                            List<int> temp1 = new List<int>();
                            foreach (int i in UnPack_Index3)
                            {
                                temp1.Add(i);
                            }
                            ll_temp1.Add(temp1);
                            UnPack_Index_Filters3.Add(ll_temp1);

                            List<List<Line>> ll_temp2 = new List<List<Line>>();
                            List<Line> temp2 = new List<Line>();
                            foreach (Line line in BLinesList)
                            {
                                temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp2.Add(temp2);
                            BLines_Filters3.Add(ll_temp2);

                            List<List<Line>> ll_temp3 = new List<List<Line>>();
                            List<Line> temp3 = new List<Line>();
                            foreach (Line line in LLinesList)
                            {
                                temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp3.Add(temp3);
                            LLines_Filters3.Add(ll_temp3);

                            List<List<Line>> ll_temp4 = new List<List<Line>>();
                            List<Line> temp4 = new List<Line>();
                            foreach (Line line in RLinesList)
                            {
                                temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp4.Add(temp4);
                            RLines_Filters3.Add(ll_temp4);

                            BLinesList.Clear();
                            LLinesList.Clear();
                            RLinesList.Clear();

                            UnPack_Index3.Clear();
                            for (int i = 0; i < RectangList.Length; i++)
                            {
                                UnPack_Index3.Add(i);
                            }
                            #endregion
                            i_all3 = i_all3 + 1;
                            continue;
                        }
                    }
                    else
                    {
                        if (RectangList[first_index][1] <= SpaceWidth & RectangList[first_index][0] <= H_Max)
                        {
                            lbx = 0;  //lbx
                            lby = 0;  //lby
                            rtx = RectangList[first_index][1];  //rtx
                            rty = RectangList[first_index][0];  //rty 

                            UnPack_Index3.Remove(first_index);  //从未排矩形集合中移除                                 
                        }
                        else
                        {
                            #region 特殊处理
                            BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                            LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                            RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                            List<List<int>> ll_temp1 = new List<List<int>>();
                            List<int> temp1 = new List<int>();
                            foreach (int i in UnPack_Index3)
                            {
                                temp1.Add(i);
                            }
                            ll_temp1.Add(temp1);
                            UnPack_Index_Filters3.Add(ll_temp1);

                            List<List<Line>> ll_temp2 = new List<List<Line>>();
                            List<Line> temp2 = new List<Line>();
                            foreach (Line line in BLinesList)
                            {
                                temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp2.Add(temp2);
                            BLines_Filters3.Add(ll_temp2);

                            List<List<Line>> ll_temp3 = new List<List<Line>>();
                            List<Line> temp3 = new List<Line>();
                            foreach (Line line in LLinesList)
                            {
                                temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp3.Add(temp3);
                            LLines_Filters3.Add(ll_temp3);

                            List<List<Line>> ll_temp4 = new List<List<Line>>();
                            List<Line> temp4 = new List<Line>();
                            foreach (Line line in RLinesList)
                            {
                                temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp4.Add(temp4);
                            RLines_Filters3.Add(ll_temp4);

                            BLinesList.Clear();
                            LLinesList.Clear();
                            RLinesList.Clear();

                            UnPack_Index3.Clear();
                            for (int i = 0; i < RectangList.Length; i++)
                            {
                                UnPack_Index3.Add(i);
                            }
                            #endregion
                            i_all3 = i_all3 + 1;
                            continue;
                        }
                    }

                    //更新线段边界集合
                    if (rtx < SpaceWidth)
                    {
                        BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                        BLinesList.Add(new Line(new Point(rtx, 0), new Point(SpaceWidth, 0)));
                        LLinesList.Add(new Line(new Point(rtx, 0), new Point(rtx, rty)));
                        LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                        RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));
                    }
                    else if (rtx == SpaceWidth)
                    {
                        BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                        LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                        RLinesList.Add(new Line(new Point(SpaceWidth, rty), new Point(SpaceWidth, SpaceHeight)));
                    }

                    #region 存储排样图形的边界线段状态
                    List<List<int>> ll_temp1_1 = new List<List<int>>();
                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index3)
                    {
                        temp1_1.Add(i);
                    }
                    ll_temp1_1.Add(temp1_1);
                    UnPack_Index_Filters3.Add(ll_temp1_1);

                    List<List<Line>> ll_temp2_1 = new List<List<Line>>();
                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp2_1.Add(temp2_1);
                    BLines_Filters3.Add(ll_temp2_1);

                    List<List<Line>> ll_temp3_1 = new List<List<Line>>();
                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp3_1.Add(temp3_1);
                    LLines_Filters3.Add(ll_temp3_1);

                    List<List<Line>> ll_temp4_1 = new List<List<Line>>();
                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    ll_temp4_1.Add(temp4_1);
                    RLines_Filters3.Add(ll_temp4_1);

                    #endregion


                }
                else
                {
                    //int control_num = 500;  //如果矩形个数多于该值，则筛选。（还可以根据矩形个数的不同，选取不同的比例或固定个数）
                    //if (Distinct_Index.Count > control_num)
                    //{
                    //    bool flag_88 = false;
                    //    for (int i = 0; i < control_num; i++)
                    //    {
                    //        if (AreaSizeList[i] == i_all3)
                    //        {
                    //            flag_88 = true;
                    //            break;
                    //        }
                    //    }

                    //    if (flag_88 == false)
                    //    {
                    //        i_all3 = i_all3 + 1;
                    //        continue;
                    //    }
                    //}

                    BLinesList.Clear();
                    LLinesList.Clear();
                    RLinesList.Clear();
                    UnPack_Index3.Clear();

                    //确定追溯恢复的索引位置
                    int RecoverIndex = 0;

                    //if (RectangList.Length < 100)
                    //{
                    //    RecoverIndex = 0;
                    //}
                    //else if (RectangList.Length >= 100 & RectangList.Length < 500)
                    //{
                    //    RecoverIndex = UnPack_Index_Filters[i_all3].Count - 50;
                    //}
                    //else if (RectangList.Length >= 500)
                    //{
                    //    RecoverIndex = 0;
                    //}
                    

                    foreach (int i in UnPack_Index_Filters[i_all3][RecoverIndex])
                    {
                        UnPack_Index3.Add(i);
                    }

                    foreach (Line line in BLines_Filters[i_all3][RecoverIndex])
                    {
                        BLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }

                    foreach (Line line in LLines_Filters[i_all3][RecoverIndex])
                    {
                        LLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }

                    foreach (Line line in RLines_Filters[i_all3][RecoverIndex])
                    {
                        RLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    
                }

                bool flag_ok = true;
                while (UnPack_Index3.Count > 0)  //循环终止条件：直到所有的矩形都排完
                {
                    //寻找可排点（寻找最下方的一条水平边界）
                    Line b_line = BLL.FindBottomLine(BLinesList);
                    Line l_line = BLL.FindFrameLine(LLinesList, b_line.Start_Point);
                    Line r_line = BLL.FindFrameLine(RLinesList, b_line.End_Point);

                    if (l_line.End_Point.Y == SpaceHeight || r_line.End_Point.Y == SpaceHeight)
                    {
                        //含板材边界的情况
                        PackingCompute3(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                    }
                    else
                    {
                        //不含板材边界的情况
                        PackingCompute_23(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                    }

                    if (BLinesList.Count == 0)
                    {
                        flag_ok = false;
                        break;
                    }
                }

                if (flag_ok)
                {
                    thread_success = true;
                    break;
                }
                else
                {
                    i_all3 = i_all3 + 1;

                    BLinesList.Clear();
                    LLinesList.Clear();
                    RLinesList.Clear();

                    UnPack_Index3.Clear();
                    for (int i = 0; i < RectangList.Length; i++)
                    {
                        UnPack_Index3.Add(i);
                    }
                }
            }
            if (thread_success)
            {
                //记录结果
                Result_H = GetH3(BLinesList);
                Result_width = RectangList[first_index][0];
                Result_height = RectangList[first_index][1];

                FirstRec_index = firstRec_index;
                FirstRec_rotate = firstRec_rotate;

            }

            thread_end2 = true;

            #endregion
            #endregion

        }

        #region 主线程
        //计算当前已排矩形的最高高度H
        private int GetH(List<Line> BLinesList)
        {
            int H = 0;
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y > H)
                {
                    H = line.Start_Point.Y;
                }
            }
            return H;
        }

        //检测凹坑填完后，剩余宽度是否可用
        private bool Check_w(int w)
        {
            bool flag = false;
            foreach (int index in UnPack_Index)
            {
                if (RectangList[index][0] <= w || RectangList[index][1] <= w)
                {
                    flag = true;
                    break;
                }
            }

            return flag;

        }

        //排样算法-针对给定的可排点，寻找合适的矩形（含板材边界的情况）
        private void PackingCompute(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            string flag = "";
            if (l_line.End_Point.Y == SpaceHeight & r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (l_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠右";
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            foreach (int index in UnPack_Index)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高不限定
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 1;
                        }
                    }
                }                
                #endregion
                
                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH) )
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高不限定
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 1;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (flag == "靠左")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }
                else if (flag == "靠右")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index.Remove(index2);


                //更新边界线段集合
                UpdateLinesList(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    if (UnPack_Index_Filters[i_all].Count>=RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters[i_all].RemoveAt(0);
                        BLines_Filters[i_all].RemoveAt(0);
                        LLines_Filters[i_all].RemoveAt(0);
                        RLines_Filters[i_all].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters[i_all].Add(temp1_1);
                    
                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters[i_all].Add(temp2_1);
                    
                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters[i_all].Add(temp3_1);
                    
                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters[i_all].Add(temp4_1);

                }
                #endregion

            }
            else  //没找到合适的矩形（该宽度不可用）
            {
                
                //更新边界线段集合
                UpdateLinesList_2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        //更新边界线段集合
        private void UpdateLinesList(List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList, int lbx, int lby, int rtx, int rty)
        {
            #region 对于矩形的下边，将影响BLinesList集合
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y == lby & line.Start_Point.X <= lbx & line.End_Point.X >= rtx)
                {
                    if (line.Length == (rtx - lbx))
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                    else if (line.Start_Point.X == lbx)
                    {
                        line.Start_Point = new Point(rtx, lby);
                        break;
                    }
                    else if (line.End_Point.X == rtx)
                    {
                        line.End_Point = new Point(lbx, lby);
                        break;
                    }
                }
            }
            #endregion

            #region 对于矩形的上边，将影响BLinesList集合
            bool flag = true;
            bool flag2 = false;
            foreach (Line line in BLinesList)
            {
                if (line.End_Point == new Point(lbx, rty))
                {
                    line.End_Point = new Point(rtx, rty);
                    flag2 = true;
                }
                else if (line.Start_Point == new Point(rtx, rty))
                {
                    line.Start_Point = new Point(lbx, rty);
                    flag2 = true;
                }

                if (flag2)
                {
                    foreach (Line line2 in BLinesList)
                    {
                        if (line2.Start_Point == line.End_Point)
                        {
                            line.End_Point = line2.End_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                        else if (line2.End_Point == line.Start_Point)
                        {
                            line.Start_Point = line2.Start_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                    }
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                BLinesList.Add(new Line(new Point(lbx, rty), new Point(rtx, rty)));
            }

            #endregion

            #region 对于矩形的左边，将影响LLinesList和RLinesList集合
            Line L3 = new Line(new Point(lbx, lby), new Point(lbx, rty));
            bool flag3 = true;
            foreach (Line line in LLinesList)
            {
                if (line.Start_Point == L3.Start_Point & line.Length == L3.Length)
                {
                    LLinesList.Remove(line);
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length > L3.Length)
                {
                    line.Start_Point = L3.End_Point;
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length < L3.Length)
                {
                    L3.Start_Point = line.End_Point;
                    LLinesList.Remove(line);
                    RLinesList.Add(L3);
                    flag3 = false;
                    break;
                }
            }
            if (flag3)
            {
                RLinesList.Add(L3);
            }
            #endregion

            #region 对于矩形的右边，将影响RLinesList和LLinesList集合
            Line L4 = new Line(new Point(rtx, lby), new Point(rtx, rty));
            bool flag4 = true;
            foreach (Line line in RLinesList)
            {
                if (line.Start_Point == L4.Start_Point & line.Length == L4.Length)
                {
                    RLinesList.Remove(line);
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length > L4.Length)
                {
                    line.Start_Point = L4.End_Point;
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length < L4.Length)
                {
                    L4.Start_Point = line.End_Point;
                    RLinesList.Remove(line);
                    LLinesList.Add(L4);
                    flag4 = false;
                    break;
                }
            }
            if (flag4)
            {
                LLinesList.Add(L4);
            }
            #endregion
        }

        //对于废弃的排样点，更新边界线段集合
        private void UpdateLinesList_2(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            //分三种情况
            #region 1-左边界高于右边界
            if (L > R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

            #region 2-左边界低于右边界
            else if (L < R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        line.End_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }
            }
            #endregion

            #region 3-左边界高度=右边界高度
            else if (L == R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                bool flag_1 = false;
                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        foreach (Line line2 in BLinesList)
                        {
                            if (line2.Start_Point == r_line.End_Point)
                            {
                                line.End_Point = line2.End_Point;
                                BLinesList.Remove(line2);
                                flag_1 = true;
                                break;
                            }
                        }
                        if (flag_1)
                        {
                            break;
                        }
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

        }

        //计算abs(矩形高-l)和abs(矩形高-r)中的最小值
        private int Geth(int recHeight, int l, int r)
        {
            int h = Math.Abs(recHeight - l);
            if (Math.Abs(recHeight - r) < h)
            {
                h = Math.Abs(recHeight - r);
            }
            return h;
        }

        //排样算法-针对给定的可排点，寻找合适的矩形（两侧都不是板材边界的情况）
        private void PackingCompute_2(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;
            int H = GetH(BLinesList);

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_6 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_7 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            int h_1 = 0;
            int h_2 = 0;
            int h_3 = 0;
            foreach (int index in UnPack_Index)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高H不变
                else if (RectangList[index][0] == W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    int temp_h = Geth(RectangList[index][1], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 0));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 0;
                        h_1 = temp_h;
                    }

                }
                else if (RectangList[index][1] == W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    int temp_h = Geth(RectangList[index][0], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 1));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 1;
                        h_1 = temp_h;
                    }
                }
                #endregion

                #region 矩形宽=W，高H变大
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 0));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 0;
                            h_2 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 1));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 1;
                            h_2 = temp_h;
                        }
                    }
                }                
                #endregion

                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH) )
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W,高H不变
                else if (RectangList[index][0] < W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高H变大
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 0));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 0;
                            h_3 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 1));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 1;
                            h_3 = temp_h;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }
            else if (TempList_6.Count > 0)
            {
                index2 = TempList_6[0].Index;
                rotateflag2 = TempList_6[0].Rotate;
            }
            else if (TempList_7.Count > 0)
            {
                index2 = TempList_7[0].Index;
                rotateflag2 = TempList_7[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (rotateflag2 == 0)  //横放
                {
                    if (Math.Abs(RectangList[index2][1] - L) < Math.Abs(RectangList[index2][1] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                }
                else  //竖放
                {
                    if (Math.Abs(RectangList[index2][0] - L) < Math.Abs(RectangList[index2][0] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index.Remove(index2);

                //更新边界线段集合
                UpdateLinesList(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    if (UnPack_Index_Filters[i_all].Count >= RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters[i_all].RemoveAt(0);
                        BLines_Filters[i_all].RemoveAt(0);
                        LLines_Filters[i_all].RemoveAt(0);
                        RLines_Filters[i_all].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters[i_all].Add(temp1_1);

                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters[i_all].Add(temp2_1);

                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters[i_all].Add(temp3_1);

                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters[i_all].Add(temp4_1);

                }
                #endregion
            }
            else  //没找到合适的矩形（该宽度不可用）
            {                

                //更新边界线段集合
                UpdateLinesList_2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        #endregion

        #region 子线程1

        //计算当前已排矩形的最高高度H
        public int GetH2(List<Line> BLinesList)
        {
            int H = 0;
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y > H)
                {
                    H = line.Start_Point.Y;
                }
            }
            return H;
        }

        //检测凹坑填完后，剩余宽度是否可用
        private bool Check_w2(int w)
        {
            bool flag = false;
            foreach (int index in UnPack_Index2)
            {
                if (RectangList[index][0] <= w || RectangList[index][1] <= w)
                {
                    flag = true;
                    break;
                }
            }

            return flag;

        }

        //排样算法-针对给定的可排点，寻找合适的矩形（含板材边界的情况）
        public void PackingCompute2(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            string flag = "";
            if (l_line.End_Point.Y == SpaceHeight & r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (l_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠右";
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            foreach (int index in UnPack_Index2)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高不限定
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 1;
                        }
                    }
                }                
                #endregion

                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH))
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高不限定
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 1;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (flag == "靠左")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }
                else if (flag == "靠右")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index2.Remove(index2);


                //更新边界线段集合
                UpdateLinesList2(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    int t_index = i_all2 - (2 * Distinct_Index.Count - 1) / 3 - 1;
                    if (UnPack_Index_Filters2[t_index].Count >= RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters2[t_index].RemoveAt(0);
                        BLines_Filters2[t_index].RemoveAt(0);
                        LLines_Filters2[t_index].RemoveAt(0);
                        RLines_Filters2[t_index].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index2)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters2[t_index].Add(temp1_1);

                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters2[t_index].Add(temp2_1);

                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters2[t_index].Add(temp3_1);

                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters2[t_index].Add(temp4_1);

                }
                #endregion

            }
            else  //没找到合适的矩形（该宽度不可用）
            {
                
                //更新边界线段集合
                UpdateLinesList_22(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        //更新边界线段集合
        public void UpdateLinesList2(List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList, int lbx, int lby, int rtx, int rty)
        {
            #region 对于矩形的下边，将影响BLinesList集合
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y == lby & line.Start_Point.X <= lbx & line.End_Point.X >= rtx)
                {
                    if (line.Length == (rtx - lbx))
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                    else if (line.Start_Point.X == lbx)
                    {
                        line.Start_Point = new Point(rtx, lby);
                        break;
                    }
                    else if (line.End_Point.X == rtx)
                    {
                        line.End_Point = new Point(lbx, lby);
                        break;
                    }
                }
            }
            #endregion

            #region 对于矩形的上边，将影响BLinesList集合
            bool flag = true;
            bool flag2 = false;
            foreach (Line line in BLinesList)
            {
                if (line.End_Point == new Point(lbx, rty))
                {
                    line.End_Point = new Point(rtx, rty);
                    flag2 = true;
                }
                else if (line.Start_Point == new Point(rtx, rty))
                {
                    line.Start_Point = new Point(lbx, rty);
                    flag2 = true;
                }

                if (flag2)
                {
                    foreach (Line line2 in BLinesList)
                    {
                        if (line2.Start_Point == line.End_Point)
                        {
                            line.End_Point = line2.End_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                        else if (line2.End_Point == line.Start_Point)
                        {
                            line.Start_Point = line2.Start_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                    }
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                BLinesList.Add(new Line(new Point(lbx, rty), new Point(rtx, rty)));
            }

            #endregion

            #region 对于矩形的左边，将影响LLinesList和RLinesList集合
            Line L3 = new Line(new Point(lbx, lby), new Point(lbx, rty));
            bool flag3 = true;
            foreach (Line line in LLinesList)
            {
                if (line.Start_Point == L3.Start_Point & line.Length == L3.Length)
                {
                    LLinesList.Remove(line);
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length > L3.Length)
                {
                    line.Start_Point = L3.End_Point;
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length < L3.Length)
                {
                    L3.Start_Point = line.End_Point;
                    LLinesList.Remove(line);
                    RLinesList.Add(L3);
                    flag3 = false;
                    break;
                }
            }
            if (flag3)
            {
                RLinesList.Add(L3);
            }
            #endregion

            #region 对于矩形的右边，将影响RLinesList和LLinesList集合
            Line L4 = new Line(new Point(rtx, lby), new Point(rtx, rty));
            bool flag4 = true;
            foreach (Line line in RLinesList)
            {
                if (line.Start_Point == L4.Start_Point & line.Length == L4.Length)
                {
                    RLinesList.Remove(line);
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length > L4.Length)
                {
                    line.Start_Point = L4.End_Point;
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length < L4.Length)
                {
                    L4.Start_Point = line.End_Point;
                    RLinesList.Remove(line);
                    LLinesList.Add(L4);
                    flag4 = false;
                    break;
                }
            }
            if (flag4)
            {
                LLinesList.Add(L4);
            }
            #endregion
        }

        //对于废弃的排样点，更新边界线段集合
        public void UpdateLinesList_22(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            //分三种情况
            #region 1-左边界高于右边界
            if (L > R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

            #region 2-左边界低于右边界
            else if (L < R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        line.End_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }
            }
            #endregion

            #region 3-左边界高度=右边界高度
            else if (L == R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                bool flag_1 = false;
                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        foreach (Line line2 in BLinesList)
                        {
                            if (line2.Start_Point == r_line.End_Point)
                            {
                                line.End_Point = line2.End_Point;
                                BLinesList.Remove(line2);
                                flag_1 = true;
                                break;
                            }
                        }
                        if (flag_1)
                        {
                            break;
                        }
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

        }

        //计算abs(矩形高-l)和abs(矩形高-r)中的最小值
        public int Geth2(int recHeight, int l, int r)
        {
            int h = Math.Abs(recHeight - l);
            if (Math.Abs(recHeight - r) < h)
            {
                h = Math.Abs(recHeight - r);
            }
            return h;
        }

        //排样算法-针对给定的可排点，寻找合适的矩形（两侧都不是板材边界的情况）
        public void PackingCompute_22(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;
            int H = GetH2(BLinesList);

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_6 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_7 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            int h_1 = 0;
            int h_2 = 0;
            int h_3 = 0;
            foreach (int index in UnPack_Index2)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高H不变
                else if (RectangList[index][0] == W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    int temp_h = Geth2(RectangList[index][1], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 0));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 0;
                        h_1 = temp_h;
                    }

                }
                else if (RectangList[index][1] == W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    int temp_h = Geth(RectangList[index][0], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 1));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 1;
                        h_1 = temp_h;
                    }
                }
                #endregion

                #region 矩形宽=W，高H变大
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 0));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 0;
                            h_2 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 1));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 1;
                            h_2 = temp_h;
                        }
                    }
                }                
                #endregion

                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH) )
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W,高H不变
                else if (RectangList[index][0] < W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高H变大
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 0));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 0;
                            h_3 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 1));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 1;
                            h_3 = temp_h;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }
            else if (TempList_6.Count > 0)
            {
                index2 = TempList_6[0].Index;
                rotateflag2 = TempList_6[0].Rotate;
            }
            else if (TempList_7.Count > 0)
            {
                index2 = TempList_7[0].Index;
                rotateflag2 = TempList_7[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (rotateflag2 == 0)  //横放
                {
                    if (Math.Abs(RectangList[index2][1] - L) < Math.Abs(RectangList[index2][1] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                }
                else  //竖放
                {
                    if (Math.Abs(RectangList[index2][0] - L) < Math.Abs(RectangList[index2][0] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index2.Remove(index2);

                //更新边界线段集合
                UpdateLinesList2(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    int t_index = i_all2 - (2 * Distinct_Index.Count - 1) / 3 - 1;
                    if (UnPack_Index_Filters2[t_index].Count >= RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters2[t_index].RemoveAt(0);
                        BLines_Filters2[t_index].RemoveAt(0);
                        LLines_Filters2[t_index].RemoveAt(0);
                        RLines_Filters2[t_index].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index2)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters2[t_index].Add(temp1_1);

                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters2[t_index].Add(temp2_1);

                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters2[t_index].Add(temp3_1);

                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters2[t_index].Add(temp4_1);

                }
                #endregion
            }
            else  //没找到合适的矩形（该宽度不可用）
            {
                
                //更新边界线段集合
                UpdateLinesList_22(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        #endregion

        #region 子线程2

        //计算当前已排矩形的最高高度H
        public int GetH3(List<Line> BLinesList)
        {
            int H = 0;
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y > H)
                {
                    H = line.Start_Point.Y;
                }
            }
            return H;
        }

        //检测凹坑填完后，剩余宽度是否可用
        private bool Check_w3(int w)
        {
            bool flag = false;
            foreach (int index in UnPack_Index3)
            {
                if (RectangList[index][0] <= w || RectangList[index][1] <= w)
                {
                    flag = true;
                    break;
                }
            }

            return flag;

        }

        //排样算法-针对给定的可排点，寻找合适的矩形（含板材边界的情况）
        public void PackingCompute3(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            string flag = "";
            if (l_line.End_Point.Y == SpaceHeight & r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (l_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠左";
            }
            else if (r_line.End_Point.Y == SpaceHeight)
            {
                flag = "靠右";
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            foreach (int index in UnPack_Index3)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高不限定
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_3.Count == 0)
                        {
                            TempList_3.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_3[0].Index][2] < RectangList[index][2])
                        {
                            TempList_3[0].Index = index;
                            TempList_3[0].Rotate = 1;
                        }
                    }
                }                
                #endregion

                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH) )
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_4.Count == 0)
                    {
                        TempList_4.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_4[0].Index][2] < RectangList[index][2])
                    {
                        TempList_4[0].Index = index;
                        TempList_4[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高不限定
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 0));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 0;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        if (TempList_5.Count == 0)
                        {
                            TempList_5.Add(new Index_Rotate(index, 1));
                        }
                        else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                        {
                            TempList_5[0].Index = index;
                            TempList_5[0].Rotate = 1;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (flag == "靠左")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }
                else if (flag == "靠右")
                {
                    if (rotateflag2 == 0)  //横放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else  //竖放
                    {
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index3.Remove(index2);


                //更新边界线段集合
                UpdateLinesList3(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    int t_index = i_all3 - 2 * (2 * Distinct_Index.Count - 1) / 3 - 1;
                    if (UnPack_Index_Filters3[t_index].Count >= RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters3[t_index].RemoveAt(0);
                        BLines_Filters3[t_index].RemoveAt(0);
                        LLines_Filters3[t_index].RemoveAt(0);
                        RLines_Filters3[t_index].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index3)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters3[t_index].Add(temp1_1);

                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters3[t_index].Add(temp2_1);

                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters3[t_index].Add(temp3_1);

                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters3[t_index].Add(temp4_1);

                }
                #endregion

            }
            else  //没找到合适的矩形（该宽度不可用）
            {
                
                //更新边界线段集合
                UpdateLinesList_23(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        //更新边界线段集合
        public void UpdateLinesList3(List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList, int lbx, int lby, int rtx, int rty)
        {
            #region 对于矩形的下边，将影响BLinesList集合
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y == lby & line.Start_Point.X <= lbx & line.End_Point.X >= rtx)
                {
                    if (line.Length == (rtx - lbx))
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                    else if (line.Start_Point.X == lbx)
                    {
                        line.Start_Point = new Point(rtx, lby);
                        break;
                    }
                    else if (line.End_Point.X == rtx)
                    {
                        line.End_Point = new Point(lbx, lby);
                        break;
                    }
                }
            }
            #endregion

            #region 对于矩形的上边，将影响BLinesList集合
            bool flag = true;
            bool flag2 = false;
            foreach (Line line in BLinesList)
            {
                if (line.End_Point == new Point(lbx, rty))
                {
                    line.End_Point = new Point(rtx, rty);
                    flag2 = true;
                }
                else if (line.Start_Point == new Point(rtx, rty))
                {
                    line.Start_Point = new Point(lbx, rty);
                    flag2 = true;
                }

                if (flag2)
                {
                    foreach (Line line2 in BLinesList)
                    {
                        if (line2.Start_Point == line.End_Point)
                        {
                            line.End_Point = line2.End_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                        else if (line2.End_Point == line.Start_Point)
                        {
                            line.Start_Point = line2.Start_Point;
                            BLinesList.Remove(line2);
                            break;
                        }
                    }
                    flag = false;
                    break;
                }
            }

            if (flag)
            {
                BLinesList.Add(new Line(new Point(lbx, rty), new Point(rtx, rty)));
            }

            #endregion

            #region 对于矩形的左边，将影响LLinesList和RLinesList集合
            Line L3 = new Line(new Point(lbx, lby), new Point(lbx, rty));
            bool flag3 = true;
            foreach (Line line in LLinesList)
            {
                if (line.Start_Point == L3.Start_Point & line.Length == L3.Length)
                {
                    LLinesList.Remove(line);
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length > L3.Length)
                {
                    line.Start_Point = L3.End_Point;
                    flag3 = false;
                    break;
                }
                else if (line.Start_Point == L3.Start_Point & line.Length < L3.Length)
                {
                    L3.Start_Point = line.End_Point;
                    LLinesList.Remove(line);
                    RLinesList.Add(L3);
                    flag3 = false;
                    break;
                }
            }
            if (flag3)
            {
                RLinesList.Add(L3);
            }
            #endregion

            #region 对于矩形的右边，将影响RLinesList和LLinesList集合
            Line L4 = new Line(new Point(rtx, lby), new Point(rtx, rty));
            bool flag4 = true;
            foreach (Line line in RLinesList)
            {
                if (line.Start_Point == L4.Start_Point & line.Length == L4.Length)
                {
                    RLinesList.Remove(line);
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length > L4.Length)
                {
                    line.Start_Point = L4.End_Point;
                    flag4 = false;
                    break;
                }
                else if (line.Start_Point == L4.Start_Point & line.Length < L4.Length)
                {
                    L4.Start_Point = line.End_Point;
                    RLinesList.Remove(line);
                    LLinesList.Add(L4);
                    flag4 = false;
                    break;
                }
            }
            if (flag4)
            {
                LLinesList.Add(L4);
            }
            #endregion
        }

        //对于废弃的排样点，更新边界线段集合
        public void UpdateLinesList_23(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

            //分三种情况
            #region 1-左边界高于右边界
            if (L > R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.Start_Point.X, r_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

            #region 2-左边界低于右边界
            else if (L < R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        line.End_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        line.Start_Point = new Point(b_line.End_Point.X, l_line.End_Point.Y);
                        break;
                    }
                }
            }
            #endregion

            #region 3-左边界高度=右边界高度
            else if (L == R)
            {
                foreach (Line line in BLinesList)
                {
                    if (line.Start_Point == b_line.Start_Point & line.End_Point == b_line.End_Point)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                }

                bool flag_1 = false;
                foreach (Line line in BLinesList)
                {
                    if (line.End_Point == l_line.End_Point)
                    {
                        foreach (Line line2 in BLinesList)
                        {
                            if (line2.Start_Point == r_line.End_Point)
                            {
                                line.End_Point = line2.End_Point;
                                BLinesList.Remove(line2);
                                flag_1 = true;
                                break;
                            }
                        }
                        if (flag_1)
                        {
                            break;
                        }
                    }
                }

                foreach (Line line in LLinesList)
                {
                    if (line.Start_Point == l_line.Start_Point & line.End_Point == l_line.End_Point)
                    {
                        LLinesList.Remove(line);
                        break;
                    }
                }

                foreach (Line line in RLinesList)
                {
                    if (line.Start_Point == r_line.Start_Point & line.End_Point == r_line.End_Point)
                    {
                        RLinesList.Remove(line);
                        break;
                    }
                }
            }
            #endregion

        }

        //计算abs(矩形高-l)和abs(矩形高-r)中的最小值
        public int Geth3(int recHeight, int l, int r)
        {
            int h = Math.Abs(recHeight - l);
            if (Math.Abs(recHeight - r) < h)
            {
                h = Math.Abs(recHeight - r);
            }
            return h;
        }

        //排样算法-针对给定的可排点，寻找合适的矩形（两侧都不是板材边界的情况）
        public void PackingCompute_23(Line b_line, Line l_line, Line r_line, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;
            int H = GetH3(BLinesList);

            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            List<Index_Rotate> TempList_1 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_2 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_3 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_4 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_5 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_6 = new List<Index_Rotate>();
            List<Index_Rotate> TempList_7 = new List<Index_Rotate>();

            bool flag_66 = false;

            //先判断情况
            int h_1 = 0;
            int h_2 = 0;
            int h_3 = 0;
            foreach (int index in UnPack_Index3)
            {
                #region 矩形宽=W，高=minH
                if (RectangList[index][0] == W & RectangList[index][1] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 0));
                    break;
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == minH)
                {
                    TempList_1.Add(new Index_Rotate(index, 1));
                    break;
                }
                #endregion

                #region 矩形宽=W，高=maxH
                else if (RectangList[index][0] == W & RectangList[index][1] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 0));
                    }
                }
                else if (RectangList[index][1] == W & RectangList[index][0] == maxH)
                {
                    if (TempList_2.Count == 0)
                    {
                        TempList_2.Add(new Index_Rotate(index, 1));
                    }
                }
                #endregion

                #region 矩形宽=W，高H不变
                else if (RectangList[index][0] == W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    int temp_h = Geth3(RectangList[index][1], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 0));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 0;
                        h_1 = temp_h;
                    }

                }
                else if (RectangList[index][1] == W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    int temp_h = Geth(RectangList[index][0], L, R);
                    if (TempList_3.Count == 0)
                    {
                        TempList_3.Add(new Index_Rotate(index, 1));
                        h_1 = temp_h;
                    }
                    else if (temp_h < h_1)
                    {
                        TempList_3[0].Index = index;
                        TempList_3[0].Rotate = 1;
                        h_1 = temp_h;
                    }
                }
                #endregion

                #region 矩形宽=W，高H变大
                else if (RectangList[index][0] == W || RectangList[index][1] == W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] == W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 0));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 0;
                            h_2 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] == W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_4.Count == 0)
                        {
                            TempList_4.Add(new Index_Rotate(index, 1));
                            h_2 = temp_h;
                        }
                        else if (temp_h < h_2)
                        {
                            TempList_4[0].Index = index;
                            TempList_4[0].Rotate = 1;
                            h_2 = temp_h;
                        }
                    }
                }               
                #endregion

                #region 矩形宽<W，高=minH或maxH
                else if (RectangList[index][0] < W & (RectangList[index][1] == minH || RectangList[index][1] == maxH) )
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (RectangList[index][0] == minH || RectangList[index][0] == maxH))
                {
                    if (TempList_5.Count == 0)
                    {
                        TempList_5.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_5[0].Index][2] < RectangList[index][2])
                    {
                        TempList_5[0].Index = index;
                        TempList_5[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W,高H不变
                else if (RectangList[index][0] < W & (b_line.Start_Point.Y + RectangList[index][1]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 0));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 0;
                    }
                }
                else if (RectangList[index][1] < W & (b_line.Start_Point.Y + RectangList[index][0]) < H)
                {
                    if (TempList_6.Count == 0)
                    {
                        TempList_6.Add(new Index_Rotate(index, 1));
                    }
                    else if (RectangList[TempList_6[0].Index][2] < RectangList[index][2]) //找面积最大
                    {
                        TempList_6[0].Index = index;
                        TempList_6[0].Rotate = 1;
                    }
                }
                #endregion

                #region 矩形宽<W，高H变大
                else if (RectangList[index][0] < W || RectangList[index][1] < W)
                {
                    flag_66 = true;
                    if (RectangList[index][0] < W & b_line.Start_Point.Y + RectangList[index][1] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][1] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 0));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 0;
                            h_3 = temp_h;
                        }
                    }
                    else if (RectangList[index][1] < W & b_line.Start_Point.Y + RectangList[index][0] <= H_Max)
                    {
                        int temp_h = b_line.Start_Point.Y + RectangList[index][0] - H;
                        if (TempList_7.Count == 0)
                        {
                            TempList_7.Add(new Index_Rotate(index, 1));
                            h_3 = temp_h;
                        }
                        else if (temp_h < h_3)
                        {
                            TempList_7[0].Index = index;
                            TempList_7[0].Rotate = 1;
                            h_3 = temp_h;
                        }
                    }
                }                
                #endregion
            }

            int index2 = -1;
            int rotateflag2 = 0;

            if (TempList_1.Count > 0)
            {
                index2 = TempList_1[0].Index;
                rotateflag2 = TempList_1[0].Rotate;
            }
            else if (TempList_2.Count > 0)
            {
                index2 = TempList_2[0].Index;
                rotateflag2 = TempList_2[0].Rotate;
            }
            else if (TempList_3.Count > 0)
            {
                index2 = TempList_3[0].Index;
                rotateflag2 = TempList_3[0].Rotate;
            }
            else if (TempList_4.Count > 0)
            {
                index2 = TempList_4[0].Index;
                rotateflag2 = TempList_4[0].Rotate;
            }
            else if (TempList_5.Count > 0)
            {
                index2 = TempList_5[0].Index;
                rotateflag2 = TempList_5[0].Rotate;
            }
            else if (TempList_6.Count > 0)
            {
                index2 = TempList_6[0].Index;
                rotateflag2 = TempList_6[0].Rotate;
            }
            else if (TempList_7.Count > 0)
            {
                index2 = TempList_7[0].Index;
                rotateflag2 = TempList_7[0].Rotate;
            }

            if (index2 != -1)  //找到合适的矩形
            {
                //对寻找到的矩形进行排样
                int lbx = 0;
                int lby = 0;
                int rtx = 0;
                int rty = 0;
                if (rotateflag2 == 0)  //横放
                {
                    if (Math.Abs(RectangList[index2][1] - L) < Math.Abs(RectangList[index2][1] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][0]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][0];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][1]; //rty

                    }
                }
                else  //竖放
                {
                    if (Math.Abs(RectangList[index2][0] - L) < Math.Abs(RectangList[index2][0] - R))
                    {
                        //靠左放
                        lbx = b_line.Start_Point.X;  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.Start_Point.X + RectangList[index2][1]; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                    else
                    {
                        //靠右放
                        lbx = b_line.End_Point.X - RectangList[index2][1];  //lbx
                        lby = b_line.Start_Point.Y;  //lby
                        rtx = b_line.End_Point.X; //rtx
                        rty = b_line.Start_Point.Y + RectangList[index2][0]; //rty

                    }
                }

                //更新未排矩形集合（剩余矩形集合）
                UnPack_Index3.Remove(index2);

                //更新边界线段集合
                UpdateLinesList3(BLinesList, LLinesList, RLinesList, lbx, lby, rtx, rty);

                #region 存储排样图形的边界线段状态
                if (Index_Compute_ALL == 1)  //仅限于初始的限定高度（第一次运算）
                {
                    int t_index = i_all3 - 2 * (2 * Distinct_Index.Count - 1) / 3 - 1;
                    if (UnPack_Index_Filters3[t_index].Count >= RecoverCount)  //让边界线段状态列表的元素规模不超过指定值
                    {
                        UnPack_Index_Filters3[t_index].RemoveAt(0);
                        BLines_Filters3[t_index].RemoveAt(0);
                        LLines_Filters3[t_index].RemoveAt(0);
                        RLines_Filters3[t_index].RemoveAt(0);
                    }

                    List<int> temp1_1 = new List<int>();
                    foreach (int i in UnPack_Index3)
                    {
                        temp1_1.Add(i);
                    }
                    UnPack_Index_Filters3[t_index].Add(temp1_1);

                    List<Line> temp2_1 = new List<Line>();
                    foreach (Line line in BLinesList)
                    {
                        temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    BLines_Filters3[t_index].Add(temp2_1);

                    List<Line> temp3_1 = new List<Line>();
                    foreach (Line line in LLinesList)
                    {
                        temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    LLines_Filters3[t_index].Add(temp3_1);

                    List<Line> temp4_1 = new List<Line>();
                    foreach (Line line in RLinesList)
                    {
                        temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                    }
                    RLines_Filters3[t_index].Add(temp4_1);

                }
                #endregion

            }
            else  //没找到合适的矩形（该宽度不可用）
            {
                
                //更新边界线段集合
                UpdateLinesList_23(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
            }

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //排样运算

            //读取XML文件内容
            XmlDocument myxml = new XmlDocument();
            //xml文件路径            
            string xmlpath = Application.StartupPath + "\\DB.xml";
            myxml.Load(xmlpath);
            XmlNode mynode1 = myxml.DocumentElement.ChildNodes[0];
            RecoverCount = Convert.ToInt32(mynode1.InnerText);

            string[] Files = Directory.GetFiles(Application.StartupPath + "\\data3\\");

            for (int fileindex = 0; fileindex < Files.Length; fileindex++)
            {
                int result_0000 = 900000000;
                //for (int index_00 = 1; index_00 <= 30; index_00++)
                //{
                //数据初始化
                Distinct_Index.Clear();
                AreaSizeList.Clear();
                UnPack_Index.Clear();
                RectangList = null;
                //Distinct_Index2.Clear();
                UnPack_Index2.Clear();
                UnPack_Index3.Clear();
                //RectangList2 = null;


                #region 读取数据文件txt
                string filepath = Files[fileindex]; //txt文件路径

                StreamReader sr = new StreamReader(filepath);
                string str;
                int index = -2;  //自动生成编号 
                Int64 area = 0;
                //int length_temp = 0;
                while ((str = sr.ReadLine()) != null)
                {
                    if (index == -2)
                    {
                        RectangList = new int[Convert.ToInt32(str)][];  //定义交错数组大小

                    }
                    else if (index == -1)
                    {
                        SpaceWidth = Convert.ToInt32(str);
                    }
                    else
                    {
                        string[] Str = str.Split(' ');
                        //加入数组
                        RectangList[index] = new int[3];
                        //对不同的txt数据文件格式（有无序号），自适应
                        if (Str.Length == 2)
                        {
                            RectangList[index][0] = Convert.ToInt32(Str[0]);  //width
                            RectangList[index][1] = Convert.ToInt32(Str[1]);  //height
                            RectangList[index][2] = Convert.ToInt32(Str[0]) * Convert.ToInt32(Str[1]); //area
                        }
                        else if (Str.Length == 3)
                        {
                            RectangList[index][0] = Convert.ToInt32(Str[1]);  //width
                            RectangList[index][1] = Convert.ToInt32(Str[2]);  //height
                            RectangList[index][2] = Convert.ToInt32(Str[1]) * Convert.ToInt32(Str[2]); //area
                        }
                        area = area + RectangList[index][2];

                        UnPack_Index.Add(index);
                        UnPack_Index2.Add(index);
                        UnPack_Index3.Add(index);
                        //在Distinct_Index中，查找是否包含相同尺寸矩形
                        //bool flag = true;
                        //foreach (int i in Distinct_Index)
                        //{
                        //    if ((RectangList[i][0] == RectangList[index][0] & RectangList[i][1] == RectangList[index][1]) || (RectangList[i][0] == RectangList[index][1] & RectangList[i][1] == RectangList[index][0]))
                        //    {
                        //        flag = false;
                        //        break;
                        //    }
                        //}

                        //if (flag)
                        //{
                        //    Distinct_Index.Add(index);                            
                        //    //Distinct_Index2.Add(index);
                        //}
                    }

                    index = index + 1;

                }
                sr.Close();
                #endregion

                //将矩形的初始顺序随机打乱！
                //Random rd = new Random();
                //int index_1 = 0;
                //int index_2 = 0;
                //for (int i = 0; i < RectangList.Length - 1; i++)
                //{
                //    index_1 = rd.Next(RectangList.Length);
                //    index_2 = rd.Next(RectangList.Length);

                //    int a1 = RectangList[index_1][0];
                //    int a2 = RectangList[index_1][1];
                //    int a3 = RectangList[index_1][2];
                //    RectangList[index_1][0] = RectangList[index_2][0];
                //    RectangList[index_1][1] = RectangList[index_2][1];
                //    RectangList[index_1][2] = RectangList[index_2][2];
                //    RectangList[index_2][0] = a1;
                //    RectangList[index_2][1] = a2;
                //    RectangList[index_2][2] = a3;
                //}




                //在Distinct_Index中，查找是否包含相同尺寸矩形
                for (int temp_i = 0; temp_i < RectangList.Length; temp_i++)
                {
                    bool flag = true;
                    foreach (int i in Distinct_Index)
                    {
                        if ((RectangList[i][0] == RectangList[temp_i][0] & RectangList[i][1] == RectangList[temp_i][1]) || (RectangList[i][0] == RectangList[temp_i][1] & RectangList[i][1] == RectangList[temp_i][0]))
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        Distinct_Index.Add(temp_i);
                    }
                }


                //存储序号，以此作为唯一标识！！！
                //for (int i = 0; i < 2 * Distinct_Index.Count; i++)
                //{
                //    AreaSizeList.Add(i);
                //}

                H_Max = Convert.ToSingle(Convert.ToSingle(area) / Convert.ToSingle(SpaceWidth));

                float temp_h = H_Max - (int)H_Max;
                if (temp_h > 0)
                {
                    H_Max = (int)H_Max + 1;
                }

                LB = Convert.ToInt32(H_Max);


                List<Line> BLinesList = new List<Line>();
                List<Line> LLinesList = new List<Line>();
                List<Line> RLinesList = new List<Line>();
                int H_ALL = 0;

                Index_Compute_ALL = 1;  //用于标记运算次数（存储碰撞状态）
                UnPack_Index_Filters.Clear();
                BLines_Filters.Clear();
                LLines_Filters.Clear();
                RLines_Filters.Clear();
                UnPack_Area.Clear();
                UnPack_Index_Filters2.Clear();
                BLines_Filters2.Clear();
                LLines_Filters2.Clear();
                RLines_Filters2.Clear();
                UnPack_Area2.Clear();
                UnPack_Index_Filters3.Clear();
                BLines_Filters3.Clear();
                LLines_Filters3.Clear();
                RLines_Filters3.Clear();
                UnPack_Area3.Clear();

                //DateTime begintime = DateTime.Now;

                #region 循环迭代
                int first_index = 0;
                bool IterationFlag = true;
                while (IterationFlag)
                {
                    //开启线程
                    thread_end = false;
                    thread_end2 = false;
                    thread_success = false;

                    Thread Mythread = new Thread(new ThreadStart(Thread_Do));
                    Mythread.Start();

                    Thread Mythread2 = new Thread(new ThreadStart(Thread_Do2));
                    Mythread2.Start();


                    #region 每一个（不同的）矩形都作为第一个，排一次
                    i_all = 0;
                    int firstRec_index = 0;
                    int firstRec_rotate = 0;
                    while (i_all <= (2 * Distinct_Index.Count - 1) / 3)
                    {
                        firstRec_index = Distinct_Index[i_all / 2];
                        if (i_all % 2 == 0)  //余数为0
                        {
                            firstRec_rotate = 0;
                        }
                        else
                        {
                            firstRec_rotate = 1;
                        }

                        if (Index_Compute_ALL == 1)
                        {
                            first_index = Distinct_Index[i_all / 2];
                            int lbx = 0;
                            int lby = 0;
                            int rtx = 0;
                            int rty = 0;
                            if (i_all % 2 == 0)  //余数为0
                            {
                                if (RectangList[first_index][0] <= SpaceWidth & RectangList[first_index][1] <= H_Max)
                                {
                                    lbx = 0;  //lbx
                                    lby = 0;  //lby
                                    rtx = RectangList[first_index][0];  //rtx
                                    rty = RectangList[first_index][1];  //rty 

                                    UnPack_Index.Remove(first_index);  //从未排矩形集合中移除

                                }
                                else
                                {
                                    #region 特殊处理
                                    BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                                    LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                                    RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                                    List<List<int>> ll_temp1 = new List<List<int>>();
                                    List<int> temp1 = new List<int>();
                                    foreach (int i in UnPack_Index)
                                    {
                                        temp1.Add(i);
                                    }
                                    ll_temp1.Add(temp1);
                                    UnPack_Index_Filters.Add(ll_temp1);

                                    List<List<Line>> ll_temp2 = new List<List<Line>>();
                                    List<Line> temp2 = new List<Line>();
                                    foreach (Line line in BLinesList)
                                    {
                                        temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp2.Add(temp2);
                                    BLines_Filters.Add(ll_temp2);

                                    List<List<Line>> ll_temp3 = new List<List<Line>>();
                                    List<Line> temp3 = new List<Line>();
                                    foreach (Line line in LLinesList)
                                    {
                                        temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp3.Add(temp3);
                                    LLines_Filters.Add(ll_temp3);

                                    List<List<Line>> ll_temp4 = new List<List<Line>>();
                                    List<Line> temp4 = new List<Line>();
                                    foreach (Line line in RLinesList)
                                    {
                                        temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp4.Add(temp4);
                                    RLines_Filters.Add(ll_temp4);

                                    BLinesList.Clear();
                                    LLinesList.Clear();
                                    RLinesList.Clear();

                                    UnPack_Index.Clear();
                                    for (int i = 0; i < RectangList.Length; i++)
                                    {
                                        UnPack_Index.Add(i);
                                    }
                                    #endregion
                                    i_all = i_all + 1;
                                    continue;
                                }
                            }
                            else
                            {
                                if (RectangList[first_index][1] <= SpaceWidth & RectangList[first_index][0] <= H_Max)
                                {
                                    lbx = 0;  //lbx
                                    lby = 0;  //lby
                                    rtx = RectangList[first_index][1];  //rtx
                                    rty = RectangList[first_index][0];  //rty

                                    UnPack_Index.Remove(first_index);  //从未排矩形集合中移除
                                }
                                else
                                {
                                    #region 特殊处理
                                    BLinesList.Add(new Line(new Point(0, 0), new Point(SpaceWidth, 0)));
                                    LLinesList.Add(new Line(new Point(0, 0), new Point(0, SpaceHeight)));
                                    RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                                    List<List<int>> ll_temp1 = new List<List<int>>();
                                    List<int> temp1 = new List<int>();
                                    foreach (int i in UnPack_Index)
                                    {
                                        temp1.Add(i);
                                    }
                                    ll_temp1.Add(temp1);
                                    UnPack_Index_Filters.Add(ll_temp1);

                                    List<List<Line>> ll_temp2 = new List<List<Line>>();
                                    List<Line> temp2 = new List<Line>();
                                    foreach (Line line in BLinesList)
                                    {
                                        temp2.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp2.Add(temp2);
                                    BLines_Filters.Add(ll_temp2);

                                    List<List<Line>> ll_temp3 = new List<List<Line>>();
                                    List<Line> temp3 = new List<Line>();
                                    foreach (Line line in LLinesList)
                                    {
                                        temp3.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp3.Add(temp3);
                                    LLines_Filters.Add(ll_temp3);

                                    List<List<Line>> ll_temp4 = new List<List<Line>>();
                                    List<Line> temp4 = new List<Line>();
                                    foreach (Line line in RLinesList)
                                    {
                                        temp4.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                                    }
                                    ll_temp4.Add(temp4);
                                    RLines_Filters.Add(ll_temp4);

                                    BLinesList.Clear();
                                    LLinesList.Clear();
                                    RLinesList.Clear();

                                    UnPack_Index.Clear();
                                    for (int i = 0; i < RectangList.Length; i++)
                                    {
                                        UnPack_Index.Add(i);
                                    }
                                    #endregion
                                    i_all = i_all + 1;
                                    continue;
                                }
                            }

                            //更新线段边界集合
                            if (rtx < SpaceWidth)
                            {
                                BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                                BLinesList.Add(new Line(new Point(rtx, 0), new Point(SpaceWidth, 0)));
                                LLinesList.Add(new Line(new Point(rtx, 0), new Point(rtx, rty)));
                                LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                                RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));
                            }
                            else if (rtx == SpaceWidth)
                            {
                                BLinesList.Add(new Line(new Point(0, rty), new Point(rtx, rty)));
                                LLinesList.Add(new Line(new Point(0, rty), new Point(0, SpaceHeight)));
                                RLinesList.Add(new Line(new Point(SpaceWidth, rty), new Point(SpaceWidth, SpaceHeight)));
                            }

                            #region 存储排样图形的边界线段状态
                            List<List<int>> ll_temp1_1 = new List<List<int>>();
                            List<int> temp1_1 = new List<int>();
                            foreach (int i in UnPack_Index)
                            {
                                temp1_1.Add(i);
                            }
                            ll_temp1_1.Add(temp1_1);
                            UnPack_Index_Filters.Add(ll_temp1_1);

                            List<List<Line>> ll_temp2_1 = new List<List<Line>>();
                            List<Line> temp2_1 = new List<Line>();
                            foreach (Line line in BLinesList)
                            {
                                temp2_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp2_1.Add(temp2_1);
                            BLines_Filters.Add(ll_temp2_1);

                            List<List<Line>> ll_temp3_1 = new List<List<Line>>();
                            List<Line> temp3_1 = new List<Line>();
                            foreach (Line line in LLinesList)
                            {
                                temp3_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp3_1.Add(temp3_1);
                            LLines_Filters.Add(ll_temp3_1);

                            List<List<Line>> ll_temp4_1 = new List<List<Line>>();
                            List<Line> temp4_1 = new List<Line>();
                            foreach (Line line in RLinesList)
                            {
                                temp4_1.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }
                            ll_temp4_1.Add(temp4_1);
                            RLines_Filters.Add(ll_temp4_1);

                            #endregion

                        }
                        else
                        {
                            //int control_num = 500;  //如果矩形个数多于该值，则筛选。（还可以根据矩形个数的不同，选取不同的比例或固定个数）
                            //if (2 * Distinct_Index.Count > control_num)
                            //{
                            //    bool flag_88 = false;
                            //    for (int i = 0; i < control_num; i++)
                            //    {
                            //        if (AreaSizeList[i] == i_all)
                            //        {
                            //            flag_88 = true;
                            //            break;
                            //        }
                            //    }

                            //    if (flag_88 == false)
                            //    {
                            //        i_all = i_all + 1;
                            //        continue;
                            //    }
                            //}


                            BLinesList.Clear();
                            LLinesList.Clear();
                            RLinesList.Clear();
                            UnPack_Index.Clear();

                            //确定追溯恢复的索引位置
                            int RecoverIndex = 0;

                            //if (RectangList.Length < 100)
                            //{
                            //    RecoverIndex = 0;
                            //}
                            //else if (RectangList.Length >= 100 & RectangList.Length < 500)
                            //{
                            //    RecoverIndex = UnPack_Index_Filters[i_all].Count - 50;
                            //}
                            //else if (RectangList.Length >= 500)
                            //{
                            //    RecoverIndex = 0;
                            //}


                            foreach (int i in UnPack_Index_Filters[i_all][RecoverIndex])
                            {
                                UnPack_Index.Add(i);
                            }

                            foreach (Line line in BLines_Filters[i_all][RecoverIndex])
                            {
                                BLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }

                            foreach (Line line in LLines_Filters[i_all][RecoverIndex])
                            {
                                LLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }

                            foreach (Line line in RLines_Filters[i_all][RecoverIndex])
                            {
                                RLinesList.Add(new Line(new Point(line.Start_Point.X, line.Start_Point.Y), new Point(line.End_Point.X, line.End_Point.Y)));
                            }

                            //这样直接写，不行！！！引用型的变量！！！！——经常在这里犯错误！！！
                            //UnPack_Index = UnPack_Index_Filters[i_all];
                            //BLinesList = BLines_Filters[i_all];
                            //LLinesList = LLines_Filters[i_all];
                            //RLinesList = RLines_Filters[i_all];
                        }

                        bool flag_ok = true;
                        while (UnPack_Index.Count > 0)  //循环终止条件：直到所有的矩形都排完
                        {
                            //寻找可排点（寻找最下方的一条水平边界）
                            Line b_line = BLL.FindBottomLine(BLinesList);
                            Line l_line = BLL.FindFrameLine(LLinesList, b_line.Start_Point);
                            Line r_line = BLL.FindFrameLine(RLinesList, b_line.End_Point);

                            if (l_line.End_Point.Y == SpaceHeight || r_line.End_Point.Y == SpaceHeight)
                            {
                                //含板材边界的情况
                                PackingCompute(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                            }
                            else
                            {
                                //不含板材边界的情况
                                PackingCompute_2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);
                            }

                            if (BLinesList.Count == 0)
                            {
                                flag_ok = false;
                                break;
                            }
                        }

                        if (flag_ok)
                        {
                            H_ALL = GetH(BLinesList);

                            IterationFlag = false;

                            Result_H = H_ALL;
                            Result_width = RectangList[first_index][0];
                            Result_height = RectangList[first_index][1];
                            //Result_rotate = RectangList[first_index][7];

                            FirstRec_index = firstRec_index;
                            FirstRec_rotate = firstRec_rotate;

                            thread_end = true;
                            thread_end2 = true;
                            //Mythread.Abort(); //终止线程

                            break;
                        }
                        else
                        {
                            if (thread_success) //如果新线程已经找到最优解
                            {
                                //Mythread.Abort(); //终止线程
                                IterationFlag = false;
                                break;
                            }
                            else
                            {
                                i_all = i_all + 1;

                                BLinesList.Clear();
                                LLinesList.Clear();
                                RLinesList.Clear();

                                UnPack_Index.Clear();
                                for (int i = 0; i < RectangList.Length; i++)
                                {
                                    UnPack_Index.Add(i);
                                }
                            }
                        }
                    }
                    #endregion

                    while (true)
                    {
                        if (thread_end & thread_end2 & thread_success == false)
                        {
                            Mythread.Abort();
                            Mythread2.Abort();
                            break;
                        }
                        else if (thread_end & thread_success == true)
                        {
                            IterationFlag = false;
                            Mythread.Abort();
                            Mythread2.Abort();
                            break;
                        }
                        else if (thread_end2 & thread_success == true)
                        {
                            IterationFlag = false;
                            Mythread.Abort();
                            Mythread2.Abort();
                            break;
                        }
                    }

                    H_Max = H_Max + 1;

                    Index_Compute_ALL = Index_Compute_ALL + 1;

                    if (Index_Compute_ALL == 2 & IterationFlag == true)
                    {
                        //将3个线程的碰撞边界结果合并
                        Combine();


                        //冒泡排序法，对数组按面积降序排列
                        //int temp;
                        //for (int i = 1; i <= AreaSizeList.Count - 1; i++)
                        //{
                        //    for (int j = 0; j <= AreaSizeList.Count - 1 - i; j++)
                        //    {
                        //        if (UnPack_Area[j] < UnPack_Area[j + 1])
                        //        {
                        //            temp = AreaSizeList[j];
                        //            AreaSizeList[j] = AreaSizeList[j + 1];
                        //            AreaSizeList[j + 1] = temp;
                        //        }
                        //    }
                        //}

                    }

                }
                #endregion

                //DateTime endtime = DateTime.Now;

                //if (Result_H < result_0000)
                //{
                //    result_0000 = Result_H;
                //} 


                //}
                //保存结果
                string path = Application.StartupPath + "\\result.txt";
                StreamWriter sw = File.AppendText(path);
                //sw.WriteLine(Path.GetFileName(Files[fileindex]) +" "+ LB.ToString()+ " " + Result_H.ToString() + " " + Result_width.ToString() + ";" + Result_height.ToString() + " " + begintime.ToString() + " " + endtime.ToString());
                string rr = Path.GetFileName(Files[fileindex]) + " " + LB.ToString() + " " + Result_H.ToString() + " " + FirstRec_index.ToString() + " " + FirstRec_rotate.ToString();
                sw.WriteLine(rr);
                sw.Flush();
                sw.Close();

            }


            MessageBox.Show("Packing Completing！", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
