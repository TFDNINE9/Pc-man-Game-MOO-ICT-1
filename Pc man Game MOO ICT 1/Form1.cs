using System;
using System.Drawing;
using System.Windows.Forms;

namespace Pc_man_Game_MOO_ICT_1
{
    public partial class Form1 : Form
    {
        bool goup, godown, goleft, goright, isGameOver;
        int score, playerSpeed, redGhostSpeed, yellowGhostSpeed, pinkGhostX, pinkGhostY;

        public Form1()
        {
            InitializeComponent();
            resetGame();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goleft = true;
                pacman.Image = Properties.Resources.left;
            }
            if (e.KeyCode == Keys.Right)
            {
                goright = true;
                pacman.Image = Properties.Resources.right;
            }
            if (e.KeyCode == Keys.Up)
            {
                goup = true;
                pacman.Image = Properties.Resources.Up;
            }
            if (e.KeyCode == Keys.Down)
            {
                godown = true;
                pacman.Image = Properties.Resources.down;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) goleft = false;
            if (e.KeyCode == Keys.Right) goright = false;
            if (e.KeyCode == Keys.Up) goup = false;
            if (e.KeyCode == Keys.Down) godown = false;
            if (e.KeyCode == Keys.Enter && isGameOver) resetGame();
        }

        private void mainGameTimer(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;

            int newLeft = pacman.Left;
            int newTop = pacman.Top;

            if (goleft)
            {
                newLeft -= playerSpeed;
                pacman.Image = Properties.Resources.left;
            }
            if (goright)
            {
                newLeft += playerSpeed;
                pacman.Image = Properties.Resources.right;
            }
            if (godown)
            {
                newTop += playerSpeed;
                pacman.Image = Properties.Resources.down;
            }
            if (goup)
            {
                newTop -= playerSpeed;
                pacman.Image = Properties.Resources.Up;
            }

            // Check for wall collision
            bool canMove = true;
            Rectangle futureBounds = new Rectangle(newLeft, newTop, pacman.Width, pacman.Height);

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "wall" && futureBounds.IntersectsWith(x.Bounds))
                {
                    canMove = false;
                    break;
                }
            }

            if (canMove)
            {
                pacman.Left = newLeft;
                pacman.Top = newTop;
            }

            // Wrap-around screen logic
            if (pacman.Left < -10) pacman.Left = 680;
            if (pacman.Left > 680) pacman.Left = -10;
            if (pacman.Top < -10) pacman.Top = 550;
            if (pacman.Top > 550) pacman.Top = 0;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {
                    if ((string)x.Tag == "coin" && x.Visible && pacman.Bounds.IntersectsWith(x.Bounds))
                    {
                        score++;
                        x.Visible = false;
                    }

                    if ((string)x.Tag == "ghost" && pacman.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameOver("You Lose!");
                    }
                }
            }

            // Moving ghosts
            redGhost.Left += redGhostSpeed;
            if (redGhost.Bounds.IntersectsWith(pictureBox1.Bounds) || redGhost.Bounds.IntersectsWith(pictureBox2.Bounds))
            {
                redGhostSpeed = -redGhostSpeed;
            }

            yellowGhost.Left -= yellowGhostSpeed;
            if (yellowGhost.Bounds.IntersectsWith(pictureBox4.Bounds) || yellowGhost.Bounds.IntersectsWith(pictureBox3.Bounds))
            {
                yellowGhostSpeed = -yellowGhostSpeed;
            }

            pinkGhost.Left -= pinkGhostX;
            pinkGhost.Top -= pinkGhostY;

            if (pinkGhost.Top < 0 || pinkGhost.Top > 320) pinkGhostY = -pinkGhostY;
            if (pinkGhost.Left < 0 || pinkGhost.Left > 420) pinkGhostX = -pinkGhostX;

            if (score == 46) gameOver("You Win!");
        }

        private void resetGame()
        {
            txtScore.Text = "Score: 0";
            score = 0;
            redGhostSpeed = 5;
            yellowGhostSpeed = 5;
            pinkGhostX = 5;
            pinkGhostY = 5;
            playerSpeed = 8;
            isGameOver = false;

            pacman.Left = 31;
            pacman.Top = 46;
            redGhost.Left = 150;
            redGhost.Top = 40;
            yellowGhost.Left = 290;
            yellowGhost.Top = 280;
            pinkGhost.Left = 300;
            pinkGhost.Top = 150;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox) x.Visible = true;
            }

            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + Environment.NewLine + message;
        }
    }
}
