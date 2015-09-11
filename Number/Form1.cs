using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Number
{
    public partial class Form1 : Form
    {
        public static int W = 360, H = 640;
        public static int MT = W / 18, MG = W / 30;
        Game game;

        public Form1()
        {
            InitializeComponent();
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
 //           g.SmoothingMode = SmoothingMode.AntiAlias;
            game.Draw(g);

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            game.GetClick(p);

            if (!game.Waiting && game.alg.CheckEmpty()) game.NewRound();

 //           Delayer.Enabled = false;  Delayer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = new Game();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            WH();
            game.map.Resize();
            if (game!=null) game.Refresh();
        }

        public void WH()
        {
            W = this.ClientSize.Width;
            H = this.ClientSize.Height;
            pictureBox1.Width = W;
            pictureBox1.Height = H;
        }


    }
}
