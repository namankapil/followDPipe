using flappy_bird.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace flappy_bird
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<int> pipe1 = new List<int>();  //first pipe
        List<int> pipe2 = new List<int>();  //2nd pipe
        int pipeWidth = 45;         // pipe width
        int pipeDifferentY = 130;   // differnce b/w upper nd lower pipe
        int pipeDifferentX = 180;   // horizontal difference b/w 2 pipes
        bool start = true;
        bool running;                // false on die
        int step = 15;               //jump
        int OriginalX, OriginalY;    // position of bird  
        bool ResetPipes = false;     // false = pipes are not empty, true = pipes are empty 
        int points;                  // points when passes bw pipes
        bool inPipe = false;         //true when bird correctly passes b/w pipes, else false
        int score;                   //score
        int ScoreDifferent;          //score differnce b/w current and max

        

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void Die()
        {
            running = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            button1.Visible = true;
            button1.Enabled = true;
            ReadAndShowScore();
            points = 0;
            pictureBox1.Location = new Point(OriginalX, OriginalY);
        }

        private void ReadAndShowScore()
        {
            using (StreamReader reader = new StreamReader("score.ini"))
            {
                int score = int.Parse(reader.ReadToEnd());
                reader.Close();

                if (int.Parse(label1.Text) == 0 || int.Parse(label1.Text) > 0)
                {
  /**/                  ScoreDifferent = score - int.Parse(label1.Text) + 1;
                }
                if (score < int.Parse(label1.Text))
                {
                    MessageBox.Show("score max. Score is"+score);
                    using (StreamWriter writer = new StreamWriter("score.ini"))
                    {
                        writer.Write(label1.Text); 
                        writer.Close();
                    }
                }
                if (score >= int.Parse(label1.Text))
                {
                    MessageBox.Show(string.Format("Score not max",MessageBoxButtons.OK,MessageBoxIcon.Information));
                }
                
            }
        }

        private void StartGame()
        {
            ResetPipes = false;
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            Random random = new Random();
            int num = random.Next(40, this.Height - this.pipeDifferentY);
            int num1 = num + this.pipeDifferentY;
            pipe1.Clear();
            pipe1.Add(this.Width);
            pipe1.Add(num);
            pipe1.Add(this.Width);
            pipe1.Add(num1);

            num = random.Next(40, this.Height - pipeDifferentY);
            num1 = num + pipeDifferentY;
            pipe2.Clear();
            pipe2.Add(this.Width + pipeDifferentX);
            pipe2.Add(num);
            pipe2.Add(this.Width + pipeDifferentX);
            pipe2.Add(num1);

            button1.Visible = false;
            button1.Enabled = false;
            running = true;
            Focus();

        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!ResetPipes && pipe1.Any() && pipe2.Any()) 
            {
                //upper pipe1
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe1[0],0,pipeWidth,pipe1[1]));
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe1[0]-10 ,pipe1[3]-pipeDifferentY,65,15));
            
                //lower pipe
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe1[2],pipe1[3],pipeWidth,this.Height-pipe1[3]));
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe1[2] - 10, pipe1[3], 65, 15));

                //upper pipe2
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe2[0], 0, pipeWidth, pipe2[1]));
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe2[0] - 10, pipe2[3] - pipeDifferentY, 65, 15));

                //lower pipe2
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe2[2], pipe2[3], pipeWidth, this.Height - pipe2[3]));
                e.Graphics.FillRectangle(Brushes.BlueViolet, new Rectangle(pipe2[2] - 10, pipe2[3], 65, 15));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OriginalX = pictureBox1.Location.X;
            OriginalY = pictureBox1.Location.Y;
            if (!File.Exists("score.ini"))
            {
                File.Create("score.ini").Dispose();
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pipe1[0] + pipeWidth <= 0  || start == true) // will occur only once (first time only)
            {
                Random rnd = new Random();
                int px = this.Width;
                int py = rnd.Next(40, (this.Height - pipeDifferentY));
                var p2x = px;
                var p2y = py + pipeDifferentY;
                pipe1.Clear();
                pipe1.Add(px);
                pipe1.Add(py);
                pipe1.Add(p2x);
                pipe1.Add(p2y);
            }
            else   // controls speed of pipes
            {
                pipe1[0] = pipe1[0] - 2;
                pipe1[2] = pipe1[2] - 2;
            }

            if (pipe2[0] + pipeWidth <= 0) 
            {
                Random rnd = new Random();
                int px = this.Width ;
                int py = rnd.Next(40, (this.Height - pipeDifferentY));
                var p2x = px;
                var p2y = py + pipeDifferentY;
                pipe2.Clear();
                pipe2.Add(px);
                pipe2.Add(py);
                pipe2.Add(p2x);
                pipe2.Add(p2y);
            }
            else  //controls speed of pipes
            {
                pipe2[0] = pipe2[0] - 2; 
                pipe2[2] = pipe2[2] - 2;
            }
            if (start == true)
            {
                start = false;
            }

        }

        private void CheckForPoint()
        {
            Rectangle rec = pictureBox1.Bounds; //pictureBox rectangle
            Rectangle rec1 = new Rectangle(pipe1[2] + 20, pipe1[3] - pipeDifferentY, 15, pipeDifferentY); //Rectangle of width 15 nd height pipeDifferentY b/w lower nd upper pipe of pipe1
            Rectangle rec2 = new Rectangle(pipe2[2] + 20, pipe2[3] - pipeDifferentY, 15, pipeDifferentY); //Rectangle of width 15 nd height pipeDifferentY b/w lower nd upper pipe of pipe2
            Rectangle intersect1 = Rectangle.Intersect(rec,rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec,rec2);
            if(!ResetPipes | start)
            {
                if (intersect1 != Rectangle.Empty || intersect2 != Rectangle.Empty)
                {
                    points++;
                    inPipe = true;
                }
                else 
                {
                    inPipe = false;
                }
            }
        }

        private void CheckForCollision()
        {
            Rectangle rec = pictureBox1.Bounds;
            Rectangle rec1 = new Rectangle(pipe1[0], 0, pipeWidth, pipe1[1]);
            Rectangle rec2 = new Rectangle(pipe1[2], pipe1[3], pipeWidth, this.Height-pipe1[3]);
            Rectangle rec3 = new Rectangle(pipe2[0], 0, pipeWidth, pipe2[1]);
            Rectangle rec4 = new Rectangle(pipe2[2], pipe2[3], pipeWidth, this.Height - pipe2[3]);
            Rectangle intersect1 = Rectangle.Intersect(rec, rec1);
            Rectangle intersect2 = Rectangle.Intersect(rec, rec2);
            Rectangle intersect3 = Rectangle.Intersect(rec, rec3);
            Rectangle intersect4 = Rectangle.Intersect(rec, rec4);
            if (!ResetPipes || start)
            {
                if (intersect1 != Rectangle.Empty || intersect2 != Rectangle.Empty || intersect3 != Rectangle.Empty || intersect4 != Rectangle.Empty)
                {
                    Die();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    step = -15;
                    pictureBox1.Image = Resources.bird_straight;
                    break;

            }

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + step);

            if (pictureBox1.Location.Y < 0)
            {
                pictureBox1.Location = new Point(pictureBox1.Location.X, 0);
            }
            if (pictureBox1.Location.Y + pictureBox1.Height >= this.ClientSize.Height)
            {
                Die();
            }
            
            CheckForCollision();
            if (running)
            {
                CheckForPoint();
            }
            label1.Text = Convert.ToString(points);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    step = 15;
                    pictureBox1.Image = Resources.bird_down;
                    break;
            }
        }
    }
}
