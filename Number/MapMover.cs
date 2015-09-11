using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Number
{
    public class MapMover:Mover
    {
        public new int X, Y;

        static public Color colorP = Color.FromArgb(250, 0, 0, 0);
        static public Color colorO = Color.FromArgb(0, 0, 0, 0);

        public override void Draw(Graphics g)
        {
            if (X < 0 || X >= game.map.MapX || Y < 0 || Y >= game.map.MapY) MessageBox.Show("MapOutofRange  "+this.ToString());
            if (game != null) DrawCenter(g, game.map.MapR[X, Y]);
        }
        public override void Refresh()
        {
            ToPT = game.map.GetPoint(X, Y);
            base.Refresh();
            int ww;
            if (game.map.MapX < game.map.MapY) ww = Map.W / 2 / game.map.MapY; else ww = ww = Map.W / 2 / game.map.MapX;
            if (ww > 0) ff = new Font("Georgia", ww, FontStyle.Bold);
        }

        public void Appear(string st, int x, int y, int a1 = 0, int a2 = 75)
        {
            str = st;
            X = x; Y = y;
            if (game != null) ToPT = PT = game.map.GetPoint(X, Y);
            color = Color.FromArgb(a1, colorO);
            ToAlpha = a2;
        }
        public void Appear(string st, int x, int y, int a1, int a2, Color cl, int dltAlpha=30)
        {
            str = st;
            X = x; Y = y; deltaAlpha = dltAlpha;
            if (game != null) ToPT = PT = game.map.GetPoint(X, Y);
            color = Color.FromArgb(a1, cl);
            ToAlpha = a2;
        }



        public void MoveTo(int x, int y)
        {
            X = x; Y = y;
            ToPT = game.map.GetPoint(x,y);
        }

        public MapMover(string st,int x,int y)
        {
            str = st;
            X = x; Y = y;
            if (game!=null) ToPT = PT = game.map.GetPoint(X, Y);
            color = colorO;
            deltaAlpha = 10;
            ff = new Font("Georgia", Map.W / 10, FontStyle.Bold);
        }
        public MapMover() { }
    }
}
