using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace _2mGame
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        int count = 0;
        Random rand = new Random();
        Timer tickerTimer = new Timer();
        Timer movementTimer = new Timer();
        string direction = "up";
        //Variables to determine direction
        int right = 1;
        int left = -1;
        int up = -1;
        int down = 1;
        //Distance variable
        int playerDist = 10;
        //code to find and use image and give properties from class Shop
        const int NUMBER_OF_FOOD = 30;
        const int NUMBER_OF_ENEMIES = 8;
        const int NUMBER_OF_SHELVES = 5;
        Shop[] yum = new Shop[NUMBER_OF_FOOD];
        Shop[] covid = new Shop[NUMBER_OF_ENEMIES];
        Shop[] shelf = new Shop[NUMBER_OF_SHELVES];
        Shop Player;
        Shop Worker;

        Bitmap sh = _2mGame.Properties.Resources.shelf;
        Bitmap corona = _2mGame.Properties.Resources.coronaVirus;
        Bitmap gs = _2mGame.Properties.Resources.Player;
        Bitmap food = _2mGame.Properties.Resources.HS;
        Bitmap exit = _2mGame.Properties.Resources.worker;

        private void Form2_Load(object sender, EventArgs e)
        {
            //instruction messagebox
            MessageBox.Show("OhNo we ran outta handsanitiser halfway through quarantine" + "\r\n" + "we'll need twice as much now that covid19 cases have quadrupled");
            //player spawner
            Player = new Shop(800, 750, gs);
            Controls.Add(Player.shopRT);
            //worker spawn
            Worker = new Shop(1420, 740, exit);
            Controls.Add(Worker.shopRT);
            //timer enabled and ticks 
            tickerTimer.Enabled = false;
            tickerTimer.Interval = 5;//every 5ms
            movementTimer.Enabled = false;
            movementTimer.Interval = 50;
            //create timer events
            tickerTimer.Tick += TickerTimer_Tick;
            movementTimer.Tick += movementTimer_Tick;

            //grocery spawner
            for (int i = 0; i < yum.Length; i++)
            {
                int xCoordinate = rand.Next(this.Width - 50);
                int yCoordinate = rand.Next(this.Height - 50);
                yum[i] = new Shop(xCoordinate, yCoordinate, food);
                Controls.Add(yum[i].shopRT);
            }
            //covid spawner
            for (int i = 0; i < covid.Length; i++)
            {
                int xCoordinate = rand.Next(this.Width - 50);
                int yCoordinate = 300;
                covid[i] = new Shop(xCoordinate, yCoordinate, corona);
                Controls.Add(covid[i].shopRT);

            }
            //shelves placer
            for (int i = 0; i < shelf.Length; i++)
            {
                int xCoordinate = 300 * i + 150;
                int yCoordinate = 150;
                shelf[i] = new Shop(xCoordinate, yCoordinate, sh);
                Controls.Add(shelf[i].shopRT);
                
            }
            
        }
        private void movementTimer_Tick(object sender, EventArgs e)
        {
            //covid movement speed and direction
            for (int i = 0; i < covid.Length; i++)
            {
                int airborne = i + 1;
                if (Player.shopRT.Top < covid[i].shopRT.Top)
                {
                    covid[i].moveUpDown(up, airborne, corona);
                }
                else if (Player.shopRT.Top > covid[i].shopRT.Top)
                {
                    covid[i].moveUpDown(down, airborne, corona);
                }
                if (Player.shopRT.Left < covid[i].shopRT.Left)
                {
                    covid[i].moveRightLeft(left, airborne, corona);
                }
                else if (Player.shopRT.Left > covid[i].shopRT.Left)
                {
                    covid[i].moveRightLeft(right, airborne, corona);
                }

                //code for when player hits shopper and gets corona/loses game
                if (Player.shopRT.Bounds.IntersectsWith(covid[i].shopRT.Bounds))
                {
                    movementTimer.Enabled = false;
                    tickerTimer.Enabled = false;
                    string message = "Oh darn you got covid19 and died";
                    MessageBox.Show(message);
                    Form2 NewForm = new Form2();
                    NewForm.Show();
                    this.Dispose(false);
                }
            }
        }
        private void TickerTimer_Tick(object sender, EventArgs e)
        {
            
            //code to make player stay on screen
            if (Player.shopRT.Top > 800)
            {
                Player.shopRT.Top -= 10;
            }
            if (Player.shopRT.Top < -1)
            {
                Player.shopRT.Top += 10;
            }
            if (Player.shopRT.Left < 0)
            {
                Player.shopRT.Left += 10;
            }
            if (Player.shopRT.Left > 1535)
            {
                Player.shopRT.Left -= 10;
            }

            //code for collecting groceries
            for (int i = 0; i < yum.Length; i++)
            {
                if (yum[i].shopRT.Top > 800)
                {
                    yum[i].shopRT.Top -= 10;
                }
                if (yum[i].shopRT.Top < -1)
                {
                    yum[i].shopRT.Top += 10;
                }
                if (yum[i].shopRT.Left < 0)
                {
                    yum[i].shopRT.Left += 10;
                }
                if (yum[i].shopRT.Left > 1535)
                {
                    yum[i].shopRT.Left -= 10;
                }
                if (Player.shopRT.Bounds.IntersectsWith(yum[i].shopRT.Bounds))
                {
                    count++;
                    yum[i].shopRT.Top = 2000;
                    yum[i].shopRT.Left = 2000;
                    yum[i].shopRT.Dispose();
                    lblScore.Text = "Grocery Items Collected: " + count;
                }
                if (Worker.shopRT.Bounds.IntersectsWith(yum[i].shopRT.Bounds))
                {           
                    yum[i].shopRT.Top -= 10;
                }
            }
            //win by collecting all the items and take them to counter
            if (Player.shopRT.Bounds.IntersectsWith(Worker.shopRT.Bounds))
            {
                if (direction == "down")
                {
                    Player.shopRT.Top -= 10;
                }
                if (direction == "right")
                {
                    Player.shopRT.Left -= 10;
                }
                if (count == 15)
                {
                    movementTimer.Enabled = false;
                    tickerTimer.Enabled = false;
                    string Winmessage = "yah we got hand sanitiser";
                    MessageBox.Show(Winmessage);
                    Form2 NewForm = new Form2();
                    NewForm.Show();
                    this.Dispose(false);
                }
            }

            //code to make player stay out of shelves
            for (int i = 0; i < shelf.Length; i++)
            {
                if (Player.shopRT.Bounds.IntersectsWith(shelf[i].shopRT.Bounds))
                {
                    if (direction == "up")
                    {
                        Player.shopRT.Top += 10;
                    }
                    if (direction == "down")
                    {
                        Player.shopRT.Top -= 10;
                    }
                    if (direction == "left")
                    {
                        Player.shopRT.Left += 10;
                    }
                    if (direction == "right")
                    {
                        Player.shopRT.Left -= 10;
                    }
                }
                //code to stop hand sanitiser hiding in shelves
                for (int x = 0; x < yum.Length; x++)
                {
                    if (yum[x].shopRT.Bounds.IntersectsWith(shelf[i].shopRT.Bounds))
                    {
                        yum[x].shopRT.Left += 10;

                    }
                }
            }
        }
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            //Shopper control keys. W:Up; S:Down; A:Left; D:Right
            //changes player orientation by changing the image
            //enable timer to start game when character moves
            
            if (e.KeyCode == Keys.W)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player;
                Player.moveUpDown(up, playerDist, gs);
                tickerTimer.Enabled = true;
                movementTimer.Enabled = true;
                direction = "up";

            }
            if (e.KeyCode == Keys.S)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player180;
                Player.moveUpDown(down, playerDist, gs);
                tickerTimer.Enabled = true;
                movementTimer.Enabled = true;
                direction = "down";
            }
            if (e.KeyCode == Keys.A)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player270;
                Player.moveRightLeft(left, playerDist, gs);
                tickerTimer.Enabled = true;
                movementTimer.Enabled = true;
                direction = "left";
            }
            if (e.KeyCode == Keys.D)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player90;
                Player.moveRightLeft(right, playerDist, gs);
                tickerTimer.Enabled = true;
                movementTimer.Enabled = true;
                direction = "right";
            }
            //pause button
            if (e.KeyCode == Keys.P)
            {
                if (tickerTimer.Enabled == false)
                {
                    tickerTimer.Enabled = true;
                    movementTimer.Enabled = true;
                }
                else if (tickerTimer.Enabled == true)
                {
                    tickerTimer.Enabled = false;
                    movementTimer.Enabled = false;
                }
            }
        }
    }
}
