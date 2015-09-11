using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Number
{
    public class Alg
    {
        Game game;
        static Random random = new Random();
        public Mover[] num = new Mover[10];
        public Mover[] opr = new Mover[10];



        static public int Atop = 0;

        //A
        static public AnimPoint Am = new AnimPoint(new Point(0, Form1.W + Form1.MT + Form1.W * 13 / 72));
        SolidBrush brush0 = new SolidBrush(Color.FromArgb(180,Color.AliceBlue));
        public void DrawA(Graphics g)
        {
     //       g.FillRectangle(brush0, 0, Form1.MT + Form1.W, Form1.W, Am.PT.Y - Form1.MT - Form1.W);

            for (int i = 0; i < 10; ++i)
                if (num[i].loc < 300) num[i].Draw(g);

            for (int i = 0; i < 10; ++i) opr[i].Draw(g);
        }

        //L
        Rectangle LRect1 = new Rectangle(Form1.W - 360, Form1.H - 195, 120, 40);
        Rectangle LRect2 = new Rectangle(Form1.W - 240, Form1.H - 195, 240, 40);
        SolidBrush brush1 = new SolidBrush(Color.FromArgb(255, 199, 232, 245));
        SolidBrush brush2 = new SolidBrush(Color.FromArgb(100, 181, 224, 242));
        Font ffRect = new Font("Lucida Console", 24);
        public void DrawL(Graphics g)
        {
            g.FillRectangle(brush1, LRect1);            
            g.FillRectangle(brush2, LRect2);
            g.DrawString("GO!", ffRect, SystemBrushes.ControlDarkDark, Form1.W - 80, Form1.H - 190);
            g.DrawString("Redo", ffRect, SystemBrushes.ControlDarkDark, Form1.W - 350, Form1.H - 190);
        }

        int[] getA()
        {
            int[] d = new int[10];
            int l = 0;
            for (int i = 0; i < 10; ++i) d[i] = -1;
            for (int i = 200; i < 220; ++i)
            {
                for (int j = 0; j < 10; ++j)
                    if (num[j].loc == i)
                        d[l++] = j;
            }
            return d;
        }

        public void ClearAlg()
        {
            Atop = 0;
            int[] d = getA();
            for (int i = 0; i < 10; ++i) opr[i].Disappear();
            for (int i = 0; i < 10; ++i) if (d[i] != -1) num[d[i]].Disappear();
            Am.SetTo(new Point(0, Form1.W + Form1.MT + Form1.W * 13 / 72));

        }
        public void ClearOpr()
        {
            for (int i = 0; i < 10; ++i) opr[i].Disappear();
        }

        void MoveAction(Stnum stnum, int[] d)
        {
            int j = 0, i;
            for (i = 0; i < 10; ++i)
            {
                for (; j < stnum.st.Length; ++j)
                    if (num[d[i]].str[0] == stnum.st[j])
                    {
                        num[d[i]].MoveTo(num[d[i]].str, 200 + j);
                        ++j;
                        break;
                    }
            }
            i = 0;
            for (j = 0; j < stnum.st.Length; ++j)
                if (stnum.st[j] > '9' || stnum.st[j] < '0')
                {
                    opr[i].Appear(stnum.st[j].ToString(), 200 + j);
                    ++i;
                }
            Am.SetTo(new Point(0, Form1.H));

            game.map.Resize(0);
            game.map.MoveAction(stnum.n);

        }
        void AttackAction(Stnum stnum,int[] d)
        {
            int j = 0, i;
            for (i = 0; i < 10; ++i)
            {
                for (; j < stnum.st.Length; ++j)
                    if (d[i]!=-1 && num[d[i]].str[0] == stnum.st[j])
                    {
                        num[d[i]].MoveTo(num[d[i]].str, 200 + j);
                        ++j;
                        break;
                    }
            }

            i = 0;
            for (j = 0; j < stnum.st.Length; ++j)
                if (stnum.st[j] > '9' || stnum.st[j] < '0')
                {
                    opr[i].Appear(stnum.st[j].ToString(), 200 + j);
                    ++i;
                }
            Am.SetTo(new Point(0, Form1.H));

            game.map.AttackAction(stnum);

        }

        public string ALine()
        {
            string sn = "";
            int[] d = new int[10];
            for (int i = 0; i < 10; ++i) d[i] = -1;
            for (int i = 200; i < 220; ++i)
            {
                for (int j = 0; j < 10; ++j)
                    if (num[j].loc == i)
                    {
                        sn += num[j].str;
                        if (i-200<10) d[i - 200] = j;
                    }
            }
            return sn;
        }

        public bool GoMove()
        {
            string sn = ALine();
            int[] d = getA();

            Stnum stnum;

            stnum = Calc.CheckMove(sn);
            if (stnum.n != 0)
            {
                game.WaitMoving = true;
                MoveAction(stnum, d);
                return true;
            }

            return false;


        }
        public bool GoAttack()
        {
            string sn = ALine();
            int[] d = getA();

            Stnum stnum;

            stnum = Calc.CheckAttack(sn);
            if (stnum.n != -1 )
            {
                game.WaitAttacking = true;
                AttackAction(stnum, d);

                return true;
            }

            return false;
        }

        private void GO()
        {
            if (Atop == 0) return;

            int[] dd = getA();
            for (int i = 0; i < 10; ++i)
                if (dd[i] != -1) num[dd[i]].Used = true;

            if (!GoMove())
                if (!GoAttack())
                {
                    ClearAlg();
                    if (CheckEmpty()) game.NewRound();
                }


        }

        public void Redo()
        {
            if (Atop == 0)
            {
                for (int i = 0; i < 10; ++i) 
                    if (!num[i].Used) num[i].MoveTo(num[i].str, 200 + Atop++);                
                return;
            }

            for (int i = 0; i < 10; ++i) num[i].MoveTo(num[i].str, 300 + i);
            Atop = 0;
        }
        public void GetClick(Point p)
        {
            if (game.Waiting) return;

            if (LRect1.Contains(p))
            {
                Redo();
                return;
            }
            else if (LRect2.Contains(p))
            {
                GO();
                return;
            }

            Rectangle r;
            bool flag=false;
            for (int i = 0; i < 10; ++i)
            {
                r = GridR[i];
                if (num[i].loc>=300 && r.Contains(p))
                {
                    flag = true;
                    num[i].MoveTo(num[i].str, 200 + Atop);
                    ++Atop;
                }
            }
            if (flag) return;
        }

        //G
        SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 181, 224, 242));
        static Rectangle[] GridR=new Rectangle[10];
        Rectangle GetRectangle(int l)
        {
            Rectangle r;
            if (l < 5)
                r = new Rectangle(Form1.W - (5 - l) * 70, Form1.H - 139, 57, 57);
            else
                r = new Rectangle(Form1.W - (10 - l) * 70, Form1.H - 67, 57, 57);
            return r;
        }
        public void DrawG(Graphics g)
        {
            for (int i = 0; i < 10; ++i)
                g.FillRectangle(brush3, GridR[i]);

            for (int i = 0; i < 10; ++i)
                if (num[i].loc >= 300) num[i].DrawCenter(g,GridR[i]);
        }
        public void GetNum()
        {
            for (int i = 0; i < 10; ++i)
            {
                int k = random.Next(10);
                num[i].ToAlpha = 255;
                num[i].Used = false;
                num[i].MoveTo(k.ToString(),300+i);
            }

            game.help.Start();
            game.WaitAttacking = false;
            
        }

        //
        public void Refresh()
        {
            LRect1 = new Rectangle(Form1.W - 360, Form1.H - 195, 120, 40);
            LRect2 = new Rectangle(Form1.W - 240, Form1.H - 195, 240, 40);
            for (int i = 0; i < 10; ++i) num[i].MoveTo(num[i].str,num[i].loc);
            for (int i = 0; i < 10; ++i) opr[i].MoveTo(opr[i].str, opr[i].loc);
            for (int i = 0; i < 10; ++i) GridR[i] = GetRectangle(i);
            for (int i = 0; i < 10; ++i) num[i].Refresh();
            for (int i = 0; i < 10; ++i) opr[i].Refresh();
            Am.Refresh();
        }
        public bool CheckEmpty()
        {
            if (game.WaitMoving) return false;
            for (int i = 0; i < 10; ++i)
                if (!num[i].Used) return false;
            return true;
        }
        public Rectangle ShowRect()
        {
            return new Rectangle(Form1.W - 360, Form1.H - 260, 360, 260);
        }

        static public Point GetPoint(int loc)
        {
            if (loc < 300)
            {
                Point p = new Point(Form1.W-360, Form1.H-248);
                int l = loc % 200;
                if (l <= 10) p.X += l * 30;
                else
                {
                    p.X += (l - 11) * 30;
                    p.Y = Form1.H - 301;
                }
                return p;
            }
            else
            {
                Point p = new Point();
                int l = loc % 300;
                p.X = GridR[l].Left+1;
                p.Y = GridR[l].Top+4;
                return p;
            }
        }

        public Alg(Game g)
        {
            game = g;
            for (int i = 0; i < 10; ++i) num[i] = new Mover("", 300 + i);
            for (int i = 0; i < 10; ++i) opr[i] = new Mover("", 0);
            for (int i = 0; i < 10; ++i) GridR[i] = GetRectangle(i);
        }

    }
}
