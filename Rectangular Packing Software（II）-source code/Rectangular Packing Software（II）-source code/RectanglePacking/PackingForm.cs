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

namespace RectanglePacking
{
    public partial class PackingForm : Form
    {
        
        private List<MyRectangle> RectangleList_1 = new List<MyRectangle>();  //矩形件泛型集合 
        private List<MyRectangle> RectangleList_ALL = new List<MyRectangle>();
        private int SpaceWidth;  //板材宽度
        private int SpaceHeight; //板材高度
        private int RectangleCount; //矩形件的个数
        //private List<Line> BLinesList = new List<Line>();  //Bottom下方的水平线段边界泛型集合
        //private List<Line> LLinesList = new List<Line>();  //Left左方的竖直线段边界泛型集合
        //private List<Line> RLinesList = new List<Line>();  //Right右方的竖直线段边界泛型集合
        private int PackingCount; //已排矩形的个数

        private List<MyRectangle> TempList_1 = new List<MyRectangle>();
        private List<MyRectangle> TempList_2 = new List<MyRectangle>();
        private List<MyRectangle> TempList_3 = new List<MyRectangle>();
        private List<MyRectangle> TempList_4 = new List<MyRectangle>();
        private List<MyRectangle> TempList_5 = new List<MyRectangle>();
        private List<MyRectangle> TempList_6 = new List<MyRectangle>();
        private List<MyRectangle> TempList_7 = new List<MyRectangle>();

        public PackingForm()
        {
            InitializeComponent();
        }

        private void PackingForm_Load(object sender, EventArgs e)
        {
            SpaceHeight = 1000000;  //设置一个很大的值，代替无限高度
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您要退出软件吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                this.Close();
            }
        }
        
        
        //查找是否包含相同尺寸矩形
        private bool Find_Rectangle(List<MyRectangle> list, MyRectangle Rec)
        {
            bool flag = false;
            foreach (MyRectangle rec in list)
            {
                if ((rec.Width == Rec.Width & rec.Height == Rec.Height)||(rec.Height==Rec.Width & rec.Width==Rec.Height))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //排样运算
            
            //string[] Files = { "zdf1.txt", "zdf2.txt", "zdf3.txt", "zdf4.txt", "zdf5.txt", "zdf6.txt", "zdf7.txt", "zdf8.txt", "zdf9.txt", "zdf10.txt", "zdf11.txt", "zdf12.txt", "zdf13.txt", "zdf14.txt", "zdf15.txt", "zdf16.txt" };

            string[] Files = { "zdf12.txt", "zdf13.txt", "zdf14.txt", "zdf15.txt" };

            //string[] Files = { "cgcut1.txt", "cgcut2.txt", "cgcut3.txt", "gcut1.txt", "gcut2.txt", "gcut3.txt", "gcut4.txt", "gcut5.txt", "gcut6.txt", "gcut7.txt", "gcut8.txt", "gcut9.txt", "gcut10.txt", "gcut11.txt", "gcut12.txt", "gcut13.txt", "ngcut1.txt", "ngcut2.txt", "ngcut3.txt", "ngcut4.txt", "ngcut5.txt", "ngcut6.txt", "ngcut7.txt", "ngcut8.txt", "ngcut9.txt", "ngcut10.txt", "ngcut11.txt", "ngcut12.txt" };

            //string[] Files = { "n1a.txt", "n1b.txt", "n1c.txt", "n1d.txt", "n1e.txt", "n2a.txt", "n2b.txt", "n2c.txt", "n2d.txt", "n2e.txt", "n3a.txt", "n3b.txt", "n3c.txt", "n3d.txt", "n3e.txt", "n4a.txt", "n4b.txt", "n4c.txt", "n4d.txt", "n4e.txt", "n5a.txt", "n5b.txt", "n5c.txt", "n5d.txt", "n5e.txt", "n6a.txt", "n6b.txt", "n6c.txt", "n6d.txt", "n6e.txt", "n7a.txt", "n7b.txt", "n7c.txt", "n7d.txt", "n7e.txt", "t1a.txt", "t1b.txt", "t1c.txt", "t1d.txt", "t1e.txt", "t2a.txt", "t2b.txt", "t2c.txt", "t2d.txt", "t2e.txt", "t3a.txt", "t3b.txt", "t3c.txt", "t3d.txt", "t3e.txt", "t4a.txt", "t4b.txt", "t4c.txt", "t4d.txt", "t4e.txt", "t5a.txt", "t5b.txt", "t5c.txt", "t5d.txt", "t5e.txt", "t6a.txt", "t6b.txt", "t6c.txt", "t6d.txt", "t6e.txt", "t7a.txt", "t7b.txt", "t7c.txt", "t7d.txt", "t7e.txt" };

            //string[] Files = { "n1a.txt" };

            List<MyRectangle> Rectangle_Distinct = new List<MyRectangle>();

            for (int fileindex = 0; fileindex < Files.Length; fileindex++)
            {
                //数据初始化
                RectangleList_1.Clear();
                RectangleList_ALL.Clear();
                Rectangle_Distinct.Clear();
                

                DateTime begintime = DateTime.Now;

                string filepath = Application.StartupPath + "\\zdf\\" + Files[fileindex]; //txt文件的路径

                StreamReader sr = new StreamReader(filepath);
                string str;
                //自动生成编号
                int index = 0;
                int area = 0;
                int length_temp = 0;
                while ((str = sr.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        SpaceWidth = Convert.ToInt32(str);
                        
                    }
                    else
                    {
                        string[] Str = str.Split(' ');

                        MyRectangle Rec = new MyRectangle(index, Convert.ToInt32(Str[1]), Convert.ToInt32(Str[2]), Convert.ToInt32(Str[2]) * Convert.ToInt32(Str[1]), "未排");

                        RectangleList_1.Add(Rec);

                        area = area + Rec.Area;

                        if (Rec.Width > SpaceWidth & Rec.Width > length_temp)
                            length_temp = Rec.Width;
                        else if (Rec.Height > SpaceWidth & Rec.Height > length_temp)
                            length_temp = Rec.Height;
                        else if (Rec.Width <= Rec.Height & Rec.Height <= SpaceWidth & Rec.Width > length_temp)
                            length_temp = Rec.Width;
                        else if (Rec.Height <= Rec.Width & Rec.Width <= SpaceWidth & Rec.Height > length_temp)
                            length_temp = Rec.Height;

                        if (Find_Rectangle(Rectangle_Distinct,Rec)==false)
                        {
                            Rectangle_Distinct.Add(Rec);
                        }

                    }

                    index = index + 1;

                }
                sr.Close();

                float H_Max = Convert.ToSingle(Convert.ToSingle(area) / Convert.ToSingle(SpaceWidth));
                if (H_Max < length_temp)
                {
                    H_Max = length_temp;
                }

                RectangleCount = RectangleList_1.Count;
                int DistinctRecCount = Rectangle_Distinct.Count;

                List<MyRectangle> RectangleList = new List<MyRectangle>();
                List<Line> BLinesList = new List<Line>();
                List<Line> LLinesList = new List<Line>();
                List<Line> RLinesList = new List<Line>();

                int H_ALL = 0;
                

                //一级循环体
                bool IterationFlag = true;
                while (IterationFlag)
                {
                    int i_all = 1;
                    while (i_all <= 2 * DistinctRecCount)
                    {       
                            RectangleList.Clear();
                            foreach (MyRectangle rec in RectangleList_1)
                            {
                                MyRectangle myrec = new MyRectangle();
                                myrec.Index = rec.Index;
                                myrec.Width = rec.Width;
                                myrec.Height = rec.Height;
                                myrec.Area = rec.Area;
                                myrec.LB_Point = rec.LB_Point;
                                myrec.RT_Point = rec.RT_Point;
                                myrec.Status = rec.Status;

                                RectangleList.Add(myrec);

                            }

                            //按顺序，轮流来，每一个矩形都做为第一个来排一次
                            MyRectangle maxRec2 = Rectangle_Distinct[(i_all - 1) / 2];

                            MyRectangle maxRec = new MyRectangle();
                            foreach (MyRectangle rec in RectangleList)
                            {
                                if (rec.Width == maxRec2.Width & rec.Height == maxRec2.Height)
                                {
                                    maxRec = rec;
                                    break;
                                }
                            }

                            if (i_all % 2 == 1)
                            {
                                if (maxRec.Width <= SpaceWidth & maxRec.Height <= H_Max)
                                {
                                    maxRec.LB_Point = new Point(0, 0);
                                    maxRec.RT_Point = new Point(maxRec.Width, maxRec.Height);
                                    maxRec.Status = "已排";
                                }
                                else
                                {
                                    i_all = i_all + 1;                                    
                                    continue;
                                }
                            }
                            else
                            {
                                if (maxRec.Height <= SpaceWidth & maxRec.Width <= H_Max)
                                {
                                    int _w66 = maxRec.Width;
                                    int _h66 = maxRec.Height;
                                    maxRec.Width = _h66;
                                    maxRec.Height = _w66;
                                    maxRec.LB_Point = new Point(0, 0);
                                    maxRec.RT_Point = new Point(maxRec.Width, maxRec.Height);
                                    maxRec.Status = "已排";
                                }
                                else
                                {
                                    i_all = i_all + 1;                                    
                                    continue;
                                }
                            }

                            //对其余的矩形，循环排样
                            PackingCount = 1; //已完成排样的矩形个数

                            //更新线段边界集合
                            BLinesList.Add(new Line(new Point(0, maxRec.Height), new Point(maxRec.Width, maxRec.Height)));
                            BLinesList.Add(new Line(new Point(maxRec.Width, 0), new Point(SpaceWidth, 0)));
                            LLinesList.Add(new Line(new Point(maxRec.Width, 0), new Point(maxRec.Width, maxRec.Height)));
                            LLinesList.Add(new Line(new Point(0, maxRec.Height), new Point(0, SpaceHeight)));
                            RLinesList.Add(new Line(new Point(SpaceWidth, 0), new Point(SpaceWidth, SpaceHeight)));

                            bool flag_ok = true;
                            while (PackingCount < RectangleCount)  //循环终止条件：直到所有的矩形都排完
                            {
                                //寻找可排点（寻找最下方的一条水平边界）
                                Line b_line = BLL.FindBottomLine(BLinesList);
                                Line l_line = BLL.FindFrameLine(LLinesList, b_line.Start_Point);
                                Line r_line = BLL.FindFrameLine(RLinesList, b_line.End_Point);


                                if (l_line.End_Point.Y == SpaceHeight || r_line.End_Point.Y == SpaceHeight)
                                {
                                    //含板材边界的情况
                                    PackingCompute(b_line, l_line, r_line, RectangleList, BLinesList, LLinesList, RLinesList, H_Max);
                                }
                                else
                                {
                                    //不含板材边界的情况
                                    PackingCompute_2(b_line, l_line, r_line, RectangleList, BLinesList, LLinesList, RLinesList, H_Max);
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
                                foreach (MyRectangle rec in RectangleList)
                                {
                                   MyRectangle myrec = new MyRectangle();
                                   myrec.Index = rec.Index;
                                   myrec.Width = rec.Width;
                                   myrec.Height = rec.Height;
                                   myrec.Area = rec.Area;
                                   myrec.LB_Point = rec.LB_Point;
                                   myrec.RT_Point = rec.RT_Point;
                                   myrec.Status = rec.Status;
                                   RectangleList_ALL.Add(myrec);
                                }

                                IterationFlag = false;
                                
                                BLinesList.Clear();
                                LLinesList.Clear();
                                RLinesList.Clear();

                                break;
                            }
                            else
                            {
                                i_all = i_all + 1;
                                
                                BLinesList.Clear();
                                LLinesList.Clear();
                                RLinesList.Clear();
                            }

                    }

                    H_Max = H_Max + 1;

                }

                //dataGridView1.DataSource = RectangleList_ALL;
                //dataGridView1.Refresh();

                //dataGridView2.DataSource = RectangleList_ALL;

                //输出高度
                //textBox2.Text = H_ALL.ToString();

                //计算面积
                //int area = 0;
                //foreach (MyRectangle rec in RectangleList_ALL)
                //{
                //    area = area + rec.Area;
                //}
                //textBox4.Text = area.ToString();

                //输出初始点
                string initialpt = "";
                foreach (MyRectangle rec in RectangleList_ALL)
                {
                    if (rec.LB_Point.X == 0 & rec.LB_Point.Y == 0)
                    {
                        initialpt = rec.Width.ToString() + ";" + rec.Height.ToString();
                        break;
                    }
                }

                DateTime endtime = DateTime.Now;

                //保存结果
                string path = Application.StartupPath + "\\result.txt";
                StreamWriter sw = File.AppendText(path);
                sw.WriteLine(Files[fileindex] + " " + H_ALL.ToString()+" "+initialpt+" "+begintime.ToString()+" "+endtime.ToString());
                sw.Flush();
                sw.Close();

            }

            MessageBox.Show("排样运算完毕！", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Bitmap btm = DrawPackingPicture(H_ALL);
            //pictureBox1.Image = btm;
            
            //btm.Save(Application.StartupPath + "\\123.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            
            //MessageBox.Show("排样运算完毕！", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        //排样算法-针对给定的可排点，寻找合适的矩形（含板材边界的情况）
        private void PackingCompute(Line b_line, Line l_line, Line r_line, List<MyRectangle> RectangleList, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList, float H_Max)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;

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

            //先判断情况
            //bool flag1 = false;  //该宽度是否可用的标志
            //string flag2 = ">";  //是正好填满该宽度，还是小于该宽度的标志
            //foreach (MyRectangle Rec in RectangleList)
            //{
            //    if (Rec.Status == "未排")
            //    {
            //        if (Rec.Width == W || Rec.Height == W)
            //        {
            //            flag1 = true;
            //            flag2 = "=";
            //            break;
            //        }
            //        else if (Rec.Width < W || Rec.Height < W)
            //        {
            //            flag1 = true;
            //            flag2 = "<";
            //        }
            //    }
            //}

            //根据三种情况，分别处理
            //if (flag1)  //该宽度可以排下
            //{
            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            MyRectangle packingRectangle = null;

            //if (flag2 == "=")  //（一）有矩形可以正好填满该宽度
            //{                   

            #region 矩形宽=W，高=minH
            foreach (MyRectangle Rec in RectangleList)
            {
                if (Rec.Status == "未排")
                {
                    if (Rec.Width == W & Rec.Height == minH)
                    {
                        packingRectangle = Rec;
                        break;
                    }
                    else if (Rec.Height == W & Rec.Width == minH)
                    {
                        int w1 = Rec.Width;
                        int h1 = Rec.Height;
                        packingRectangle = Rec;
                        packingRectangle.Width = h1; //横竖旋转
                        packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                        break;
                    }
                }
            }
            #endregion

            #region 矩形宽=W，高=maxH
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width == W & Rec.Height == maxH)
                        {
                            packingRectangle = Rec;
                            break;
                        }
                        else if (Rec.Height == W & Rec.Width == maxH)
                        {
                            int w1 = Rec.Width;
                            int h1 = Rec.Height;
                            packingRectangle = Rec;
                            packingRectangle.Width = h1; //横竖旋转
                            packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 矩形宽=W,高不限定
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width == W & b_line.Start_Point.Y + Rec.Height <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                            }

                        }
                        else if (Rec.Height == W & b_line.Start_Point.Y + Rec.Width <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }

                        }
                    }
                }

            }
            #endregion

            //对寻找到的矩形进行排样
            //    packingRectangle.LB_Point = b_line.Start_Point;
            //    packingRectangle.RT_Point = new Point(packingRectangle.LB_Point.X + packingRectangle.Width, packingRectangle.LB_Point.Y + packingRectangle.Height);
            //    packingRectangle.Status = "已排";

            //    //更新矩形集合RectangleList
            //    RectangleList_Update(packingRectangle,RectangleList);

            //    //更新已排矩形的个数
            //    PackingCount = PackingCount + 1;

            //    //更新边界线段集合
            //    UpdateLinesList(packingRectangle, BLinesList, LLinesList, RLinesList);

            //}
            //else if (flag2 == "<")  //（二）没有矩形正好填满该宽度
            //{

            #region 矩形宽<W，高=minH或maxH
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width < W & (Rec.Height == minH || Rec.Height == maxH))
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                            }
                        }
                        else if (Rec.Height < W & (Rec.Width == minH || Rec.Width == maxH))
                        {
                            if (packingRectangle == null)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                        }
                    }
                }
            }
            #endregion

            #region 矩形宽<W,高不限定
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width < W & b_line.Start_Point.Y + Rec.Height <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                            }
                        }
                        else if (Rec.Height < W & b_line.Start_Point.Y + Rec.Width <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                        }
                    }
                }
            }
            #endregion

            //对寻找到的矩形进行排样
            if (packingRectangle != null)
            {
                if (flag == "靠左")
                {
                    packingRectangle.LB_Point = b_line.Start_Point;
                }
                else if (flag == "靠右")
                {
                    packingRectangle.LB_Point = new Point(b_line.End_Point.X - packingRectangle.Width, b_line.End_Point.Y);
                }
                packingRectangle.RT_Point = new Point(packingRectangle.LB_Point.X + packingRectangle.Width, packingRectangle.LB_Point.Y + packingRectangle.Height);
                packingRectangle.Status = "已排";

                //更新矩形集合RectangleList
                RectangleList_Update(packingRectangle, RectangleList);

                //更新已排矩形的个数
                PackingCount = PackingCount + 1;

                //更新边界线段集合
                UpdateLinesList(packingRectangle, BLinesList, LLinesList, RLinesList);
                //}

            }
            else  //（三）该宽度不可用
            {
                //更新边界线段集合
                UpdateLinesList_2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);

            }

        }


        //更新矩形集合RectangleList
        private void RectangleList_Update(MyRectangle myrec, List<MyRectangle> RectangleList)
        {
            int count = RectangleList.Count;
            for (int i = 0; i < count; i++)
            {
                if (RectangleList[i].Index == myrec.Index)
                {
                    RectangleList[i] = myrec;
                    break;
                }

            }
        }


        //更新边界线段集合
        private void UpdateLinesList(MyRectangle myrec, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList)
        {
            #region 对于矩形的下边，将影响BLinesList集合
            Line L1 = new Line(myrec.LB_Point, new Point(myrec.RT_Point.X, myrec.LB_Point.Y));
            foreach (Line line in BLinesList)
            {
                if (line.Start_Point.Y == L1.Start_Point.Y & line.Start_Point.X <= L1.Start_Point.X & line.End_Point.X >= L1.End_Point.X)
                {
                    if (line.Length == L1.Length)
                    {
                        BLinesList.Remove(line);
                        break;
                    }
                    else if (line.Start_Point.X == L1.Start_Point.X)
                    {
                        line.Start_Point = L1.End_Point;
                        break;
                    }
                    else if (line.End_Point.X == L1.End_Point.X)
                    {
                        line.End_Point = L1.Start_Point;
                        break;
                    }
                }
            }
            #endregion

            #region 对于矩形的上边，将影响BLinesList集合
            Line L2 = new Line(new Point(myrec.LB_Point.X, myrec.RT_Point.Y),myrec.RT_Point);
            bool flag = true;
            bool flag2 = false;
            foreach (Line line in BLinesList)
            {
                if (line.End_Point == L2.Start_Point)
                {
                    line.End_Point = L2.End_Point;
                    flag2 = true;
                }
                else if (line.Start_Point == L2.End_Point)
                {
                    line.Start_Point = L2.Start_Point;
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
                BLinesList.Add(L2);
            }

            #endregion

            #region 对于矩形的左边，将影响LLinesList和RLinesList集合
            Line L3 = new Line(myrec.LB_Point,new Point(myrec.LB_Point.X,myrec.RT_Point.Y));
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
            Line L4 = new Line(new Point(myrec.RT_Point.X, myrec.LB_Point.Y),myrec.RT_Point);
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
        private void PackingCompute_2(Line b_line, Line l_line, Line r_line, List<MyRectangle> RectangleList, List<Line> BLinesList, List<Line> LLinesList, List<Line> RLinesList, float H_Max)
        {
            int W = b_line.Length;
            int L = l_line.Length;
            int R = r_line.Length;
            int H = GetH(BLinesList);

            //先判断情况
            //bool flag1 = false;  //该宽度是否可用的标志
            //string flag2 = ">";  //是正好填满该宽度，还是小于该宽度的标志
            //foreach (MyRectangle Rec in RectangleList)
            //{
            //    if (Rec.Status == "未排")
            //    {
            //        if (Rec.Width == W || Rec.Height == W)
            //        {
            //            flag1 = true;
            //            flag2 = "=";
            //            break;
            //        }
            //        else if (Rec.Width < W || Rec.Height < W)
            //        {
            //            flag1 = true;
            //            flag2 = "<";
            //        }
            //    }
            //}

            //根据三种情况，分别处理
            //if (flag1)  //该宽度可以排下
            //{
            int minH = L;
            int maxH = R;
            if (minH > maxH)
            {
                int h = maxH;
                maxH = minH;
                minH = h;
            }

            MyRectangle packingRectangle = null;

            //if (flag2 == "=")  //（一）有矩形可以正好填满该宽度
            //{

            #region 矩形宽=W，高=minH
            foreach (MyRectangle Rec in RectangleList)
            {
                if (Rec.Status == "未排")
                {
                    if (Rec.Width == W & Rec.Height == minH)
                    {
                        packingRectangle = Rec;
                        break;
                    }
                    else if (Rec.Height == W & Rec.Width == minH)
                    {
                        int w1 = Rec.Width;
                        int h1 = Rec.Height;
                        packingRectangle = Rec;
                        packingRectangle.Width = h1; //横竖旋转
                        packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                        break;
                    }
                }
            }
            #endregion

            #region 矩形宽=W，高=maxH
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width == W & Rec.Height == maxH)
                        {
                            packingRectangle = Rec;
                            break;
                        }
                        else if (Rec.Height == W & Rec.Width == maxH)
                        {
                            int w1 = Rec.Width;
                            int h1 = Rec.Height;
                            packingRectangle = Rec;
                            packingRectangle.Width = h1; //横竖旋转
                            packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 矩形宽=W,高H不变
            if (packingRectangle == null)
            {
                int h = 0;
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width == W & (b_line.Start_Point.Y + Rec.Height) < H)
                        {
                            if (packingRectangle == null)
                            {
                                h = Geth(Rec.Height, L, R);
                                packingRectangle = Rec;
                            }
                            else if (Geth(Rec.Height, L, R) < h)
                            {
                                h = Geth(Rec.Height, L, R);
                                packingRectangle = Rec;
                            }

                        }
                        else if (Rec.Height == W & (b_line.Start_Point.Y + Rec.Width) < H)
                        {
                            if (packingRectangle == null)
                            {
                                h = Geth(Rec.Width, L, R);
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if (Geth(Rec.Width, L, R) < h)
                            {
                                h = Geth(Rec.Width, L, R);
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }

                        }
                    }
                }

            }
            #endregion

            #region 矩形宽=W,高H变大
            if (packingRectangle == null)
            {
                int h = 0;
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width == W & b_line.Start_Point.Y + Rec.Height <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                h = b_line.Start_Point.Y + Rec.Height - H;
                                packingRectangle = Rec;
                            }
                            else if ((b_line.Start_Point.Y + Rec.Height - H) < h)
                            {
                                h = b_line.Start_Point.Y + Rec.Height - H;
                                packingRectangle = Rec;
                            }

                        }
                        else if (Rec.Height == W & b_line.Start_Point.Y + Rec.Width <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                h = b_line.Start_Point.Y + Rec.Width - H;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if ((b_line.Start_Point.Y + Rec.Width - H) < h)
                            {
                                h = b_line.Start_Point.Y + Rec.Width - H;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }

                        }
                    }
                }

            }
            #endregion

            //对寻找到的矩形进行排样
            //packingRectangle.LB_Point = b_line.Start_Point;
            //packingRectangle.RT_Point = new Point(packingRectangle.LB_Point.X + packingRectangle.Width, packingRectangle.LB_Point.Y + packingRectangle.Height);
            //packingRectangle.Status = "已排";

            ////更新矩形集合RectangleList
            //RectangleList_Update(packingRectangle,RectangleList);

            ////更新已排矩形的个数
            //PackingCount = PackingCount + 1;

            ////更新边界线段集合
            //UpdateLinesList(packingRectangle,BLinesList,LLinesList,RLinesList);

            //}

            //else if (flag2 == "<")  //（二）没有矩形正好填满该宽度
            //{

            #region 矩形宽<W，高=minH或maxH
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width < W & (Rec.Height == minH || Rec.Height == maxH))
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                            }
                        }
                        else if (Rec.Height < W & (Rec.Width == minH || Rec.Width == maxH))
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;  //（这里不知道是否有问题？？）
                            }
                        }
                    }
                }
            }
            #endregion

            #region 矩形宽<W,高H不变
            if (packingRectangle == null)
            {
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width < W & (b_line.Start_Point.Y + Rec.Height) < H)
                        {
                            if (packingRectangle == null)
                            {
                                packingRectangle = Rec;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                packingRectangle = Rec;
                            }
                        }
                        else if (Rec.Height < W & (b_line.Start_Point.Y + Rec.Width) < H)
                        {
                            if (packingRectangle == null)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;
                            }
                            else if (packingRectangle.Area < Rec.Area)
                            {
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;
                            }
                        }
                    }
                }


            }
            #endregion

            #region 矩形宽<W,高H变大
            if (packingRectangle == null)
            {
                int h = 0;
                foreach (MyRectangle Rec in RectangleList)
                {
                    if (Rec.Status == "未排")
                    {
                        if (Rec.Width < W & b_line.Start_Point.Y + Rec.Height <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                h = b_line.Start_Point.Y + Rec.Height - H;
                                packingRectangle = Rec;
                            }
                            else if ((b_line.Start_Point.Y + Rec.Height - H) < h)
                            {
                                h = b_line.Start_Point.Y + Rec.Height - H;
                                packingRectangle = Rec;
                            }

                        }
                        else if (Rec.Height < W & b_line.Start_Point.Y + Rec.Width <= H_Max)
                        {
                            if (packingRectangle == null)
                            {
                                h = b_line.Start_Point.Y + Rec.Width - H;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;
                            }
                            else if ((b_line.Start_Point.Y + Rec.Width - H) < h)
                            {
                                h = b_line.Start_Point.Y + Rec.Width - H;
                                int w1 = Rec.Width;
                                int h1 = Rec.Height;
                                packingRectangle = Rec;
                                packingRectangle.Width = h1; //横竖旋转
                                packingRectangle.Height = w1;
                            }

                        }
                    }
                }

            }
            #endregion

            //对寻找到的矩形进行排样
            if (packingRectangle != null)
            {
                if (Math.Abs(packingRectangle.Height - L) < Math.Abs(packingRectangle.Height - R))
                {
                    //靠左排放
                    packingRectangle.LB_Point = b_line.Start_Point;
                }
                else
                {
                    //靠右排放
                    packingRectangle.LB_Point = new Point(b_line.End_Point.X - packingRectangle.Width, b_line.End_Point.Y);
                }

                packingRectangle.RT_Point = new Point(packingRectangle.LB_Point.X + packingRectangle.Width, packingRectangle.LB_Point.Y + packingRectangle.Height);
                packingRectangle.Status = "已排";

                //更新矩形集合RectangleList
                RectangleList_Update(packingRectangle, RectangleList);

                //更新已排矩形的个数
                PackingCount = PackingCount + 1;

                //更新边界线段集合
                UpdateLinesList(packingRectangle, BLinesList, LLinesList, RLinesList);
                //}

            }
            else  //（三）该宽度不可用
            {
                //更新边界线段集合
                UpdateLinesList_2(b_line, l_line, r_line, BLinesList, LLinesList, RLinesList);

            }

        }


        //绘图，输出排样图
        private Bitmap DrawPackingPicture(int H)
        {            
            Bitmap btm = new Bitmap(SpaceWidth * 10, (H + 50) * 10);
            Graphics g = Graphics.FromImage(btm);

            //实现坐标系以左下角为原点
            g.TranslateTransform(0f, (H + 50)*10);  //将GDI+中原始的坐标原点平移（H + 50 为绘图区域的高度）  
            g.ScaleTransform(1f, -1f);  //变换x，y轴的正方向

            g.Clear(Color.Black);

            //注意：经过坐标原点变换后，绘制矩形的函数为左下角点！！！
            //提示为左上角点（因为原本默认的是原点在左上角）
            //实质：从指定的坐标点出发，沿X轴正向画宽度，沿Y轴正向画高度！

            SolidBrush sbrush = new SolidBrush(Color.LightBlue);
            Pen mypen = new Pen(Color.Red, 2);

            foreach (MyRectangle rec in RectangleList_ALL)
            {
                Rectangle Rec = new Rectangle(rec.LB_Point.X * 10, rec.LB_Point.Y * 10, rec.Width * 10, rec.Height * 10);
                g.DrawRectangle(mypen, Rec);
                g.FillRectangle(sbrush, Rec);
            }

            return btm;

        }
        

        

        

        
        

    }
}
