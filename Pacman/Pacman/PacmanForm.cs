using Pacman.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman
{
    public partial class PacmanForm : Form
    {
        Pacman pacman;
        Image foodImage;
        List<Prepreka> prepreki;
        static readonly int TIMER_INTERVAL = 200;
        static readonly int BR_PREPREKI = 8;
        public static int WORLD_WIDTH = 15;
        public static int WORLD_HEIGHT = 10;
        bool[][] foodWorld;

        public PacmanForm()
        {
            InitializeComponent();
            foodImage = Resources.food;
            DoubleBuffered = true;
            newGame();
        }

        public void newGame()
        {
            pacman = new Pacman();
            timer.Interval = TIMER_INTERVAL;
            this.Width = Pacman.Radius * 2 * (WORLD_WIDTH + 1);
            this.Height = Pacman.Radius * 2 * (WORLD_HEIGHT + 2);
            foodWorld = new bool[WORLD_HEIGHT][];
            for(int i = 0; i < WORLD_HEIGHT; i++)
            {
                foodWorld[i] = new bool[WORLD_WIDTH];
                for(int j = 0; j < WORLD_WIDTH; j++)
                {
                    foodWorld[i][j] = true;
                }
            }
            GenerateWalls();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = WORLD_HEIGHT * WORLD_WIDTH - 3 * BR_PREPREKI;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            timer.Start();
        }

        public void GenerateWalls()
        {
            Random rand = new Random();
            prepreki = new List<Prepreka>();
            for(int i = 0; i < BR_PREPREKI; i++)
            {
                int x = rand.Next(WORLD_WIDTH);
                int y = rand.Next(WORLD_HEIGHT - 2);
                Prepreka p = new Prepreka();
                p.P = new Point(x, y);
                bool f = true;
                foreach(Prepreka prep in prepreki)
                {
                    if (p.isColided(prep.P))
                    {
                        i--;
                        f = false;
                        break;
                    }
                }
                if (f)
                {
                    prepreki.Add(p);
                }
            }
            foreach(Prepreka p in prepreki)
            {
                foodWorld[p.P.Y][p.P.X] = false;
                foodWorld[p.P.Y + 1][p.P.X] = false;
                foodWorld[p.P.Y + 2][p.P.X] = false;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (!validMove())
            {
                timer.Stop();
                return;
            }
            pacman.Move(WORLD_WIDTH, WORLD_HEIGHT);
            foodWorld[pacman.Y][pacman.X] = false;
            Invalidate();
            checkWin();
        }

        private bool validMove()
        {
            foreach(Prepreka prep in prepreki)
            {
                switch (pacman.Direction)
                {
                    case Direction.DOWN:
                        if (Pacman.Mod(pacman.Y + 1, WORLD_HEIGHT) == prep.P.Y && pacman.X == prep.P.X) return false;
                        break;
                    case Direction.UP:
                        if (Pacman.Mod(pacman.Y - 1, WORLD_HEIGHT) == prep.P.Y + 2 && pacman.X == prep.P.X) return false;
                        break;
                    case Direction.RIGHT:
                        if (Pacman.Mod(pacman.X + 1, WORLD_WIDTH) == prep.P.X && (pacman.Y == prep.P.Y || pacman.Y == prep.P.Y + 1 || pacman.Y == prep.P.Y + 2))
                            return false;
                        break;
                    case Direction.LEFT:
                        if (Pacman.Mod(pacman.X - 1, WORLD_WIDTH) == prep.P.X && (pacman.Y == prep.P.Y || pacman.Y == prep.P.Y + 1 || pacman.Y == prep.P.Y + 2))
                            return false;
                        break;
                }
            }
            return true;
        }

        private void checkWin()
        {
            for (int i = 0; i < WORLD_HEIGHT; i++)
                for (int j = 0; j < WORLD_WIDTH; j++)
                    if (foodWorld[i][j] == true) return;
            timer.Stop();
            DialogResult result = MessageBox.Show("You win! Play again?", "Congratulations!", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                newGame();
            else Application.Exit();
        }

        private void PacmanForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!timer.Enabled)
            {
                timer.Enabled = true;
            }
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
                pacman.ChangeDirection(Direction.UP);
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
                pacman.ChangeDirection(Direction.DOWN);
            if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
                pacman.ChangeDirection(Direction.RIGHT);
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
                pacman.ChangeDirection(Direction.LEFT);
            Invalidate();
        }

        private void PacmanForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Random rand = new Random();
            int count = 24;
            for (int i = 0; i < foodWorld.Length; i++)
            {
                for (int j = 0; j < foodWorld[i].Length; j++)
                {
                    if (foodWorld[i][j])
                    {
                        count++;
                        g.DrawImageUnscaled(foodImage, j * Pacman.Radius * 2 + (Pacman.Radius * 2 - foodImage.Height) / 2, i * Pacman.Radius * 2 + (Pacman.Radius * 2 - foodImage.Width) / 2);
                    }
                }
            }
            Brush b = new SolidBrush(Color.Red);
            foreach(Prepreka p in prepreki)
            {
                g.FillRectangle(b, p.P.X * Pacman.Radius * 2, p.P.Y * Pacman.Radius * 2, 2 * Pacman.Radius, 6 * Pacman.Radius);
            }
            textBox1.Text = String.Format("Points: {0}", WORLD_HEIGHT * WORLD_WIDTH - count);
            progressBar1.Value = WORLD_HEIGHT * WORLD_WIDTH - count;
            pacman.Draw(g);
        }
    }
}
