using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Number
{
    public class Mover:AnimPoint
    {
        static public Game game;

        public string str="";
        public Font ff;
        public int loc;
        public int ToAlpha=255,deltaAlpha=30;        
        public Color color;
        static public Color colorM =Color.FromArgb(200, 0, 0, 0);
        static public Color colorA = Color.FromArgb(255, 100, 149, 237);
        static public Color colorB = Color.FromArgb(100, 0, 135, 142);
        public bool Used = false;

        public Point GetPoint(int loc)
        {
            Point p=new Point(0,0);

            switch (loc)
            {
                case 0:
                    return new Point(Form1.W, Form1.H);

            }

                p = Alg.GetPoint(loc);
                return p;
            
        }

        //Draw
        public override void Refresh()
        {
            if (color.A != ToAlpha)
            {
                if (color.A > ToAlpha)
                {
                    if (color.A > ToAlpha + deltaAlpha)
                        color = Color.FromArgb(color.A - deltaAlpha, color);
                    else color = Color.FromArgb(ToAlpha,color);
                }
                else if (color.A < ToAlpha)
                {
                    if (color.A < ToAlpha - deltaAlpha)
                        color = Color.FromArgb(color.A + deltaAlpha, color);
                    else color = Color.FromArgb(ToAlpha, color);
                }

            }

            base.Refresh();


        }
        public virtual void Draw(Graphics g)
        {
            g.DrawString(str,ff,new SolidBrush(color),PT);
        }
        public virtual void DrawCenter(Graphics g,Rectangle r)
        {
            Size sz = TextRenderer.MeasureText(str, ff);
            int dx = (r.Width - sz.Width) / 2;
            int dy = (r.Height - sz.Height) / 2;
            g.DrawString(str, ff, new SolidBrush(color), new Point(PT.X + dx, PT.Y + dy));
        }

        //Action
        public void MoveTo(string s, int loc)
        {
            str = s;

            this.loc = loc;

            ToPT = GetPoint(loc);

        }
        public virtual void  Appear(string s, int loc)
        {
            str = s;
            this.loc = loc;
            ToAlpha = 255;
            PT = GetPoint(loc);
            ToPT = GetPoint(loc);
        }
        public void Disappear()
        {
            ToAlpha = 0;
            loc = 0;
        }

        //
        public Mover() { }
        public Mover(string s,int loc)
        {
            str = s;

            this.loc = loc;
            PT = GetPoint(loc);
            ToPT = GetPoint(loc);            

            switch (loc)
            {
                case 0:
                    color = Color.FromArgb(0,colorA);
                    ToAlpha = 0;
                    ff = new Font("Lucida Console", Form1.W / 10, FontStyle.Bold);
                    return;
                case 10:
                    return;
                case 21:
                    color = Color.FromArgb(180, SystemColors.ControlDarkDark);
                    ff = new Font("Lucida Console", Form1.W / 10, FontStyle.Bold);
                    return;
                case 22:
                    color = Color.FromArgb(180, SystemColors.ControlDarkDark);
                    ff = new Font("Lucida Console", Form1.W / 10, FontStyle.Bold);
                    return;
            }

            if (loc < 200)
            {
                color = colorM; 
                ff = new Font("Georgia", Map.W / 10, FontStyle.Bold);
            }
            else
            {
                color = colorA;
                ff = new Font("Lucida Console", Form1.W / 10, FontStyle.Bold);
            }

        }

    }
}
