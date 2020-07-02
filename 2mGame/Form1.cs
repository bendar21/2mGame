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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int count = 0;
        Random rand = new Random();
        Timer tickerTimer = new Timer();

        //Variables to determine direction
        int right = 1;
        int left = -1;
        int up = -1;
        int down = 1;
        //Distance variable
        int playerDist = 10;
        int airborne = 2;

        //code to find and use image and give properties from class Shop
        const int NUMBER_OF_FOOD = 10;
        const int NUMBER_OF_ENEMIES = 2;
        const int NUMBER_OF_SHELVES = 5;
        Shop[] yum = new Shop[NUMBER_OF_FOOD];
        Shop[] covid = new Shop[NUMBER_OF_ENEMIES];
        Shop[] shelf = new Shop[NUMBER_OF_SHELVES];
        Shop Player;

        Bitmap sh = _2mGame.Properties.Resources.shelf;
        Bitmap corona = _2mGame.Properties.Resources.redCircle;
        Bitmap gs = _2mGame.Properties.Resources.Player;
        Bitmap food = _2mGame.Properties.Resources.blueCircle;
        

        public void Form1_Load(object sender, EventArgs e)
        {
            
            //player and covid spawner
            Player = new Shop(800, 750, gs);
            Controls.Add(Player.shopRT);

            tickerTimer.Enabled = true;
            tickerTimer.Interval = 5; //every 5ms
            //create timer event
            tickerTimer.Tick += TickerTimer_Tick;
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
                shelf[i] = new Shop(xCoordinate, 150 , sh);
                Controls.Add(shelf[i].shopRT);
            }
        }
        private void TickerTimer_Tick(object sender, EventArgs e)
        {
            //code more making groceries arent behind shelves
            for (int i = 0; i < shelf.Length; i++)
            {
                if (yum[i].shopRT.Bounds.IntersectsWith(shelf[i].shopRT.Bounds))
                {
                    yum[i].shopRT.Left += 10;
                    
                }
            }
                //covid movement
                for (int i = 0; i < covid.Length; i++)
            {
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
                    tickerTimer.Enabled = false;
                    string message = "Oh darn you got covid19 and died";
                    MessageBox.Show(message);
                    Form1 NewForm = new Form1();
                    NewForm.Show();
                    this.Dispose(false);
                }
            }
            //code to make player stay on screen
            if (Player.shopRT.Top > 800 )
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
                if (Player.shopRT.Bounds.IntersectsWith(yum[i].shopRT.Bounds))
                {
                    
                    count++;
                    yum[i].shopRT.Top = 2000; 
                    yum[i].shopRT.Left = 2000;
                    yum[i].shopRT.Dispose();
                    lblScore.Text = "Grocery Items Collected: " + count;
                }          
            }
            if (count == 10)
            {
                tickerTimer.Enabled = false;
                Player.shopRT.Top = 750;
                Player.shopRT.Left = 750;
                string Winmessage = "yah u got food for ISO";
                MessageBox.Show(Winmessage);
                Form1 NewForm = new Form1();
                NewForm.Show();
                this.Dispose(false);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Shopper control keys. W:Up; S:Down; A:Left; D:Right
            if (e.KeyCode == Keys.W)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player;
                
                Player.moveUpDown(up, playerDist, gs);
            }
            if (e.KeyCode == Keys.S)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player180;
                Player.moveUpDown(down, playerDist, gs);
            }
            if (e.KeyCode == Keys.A)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player270;
                Player.moveRightLeft(left, playerDist, gs);
            }
            if (e.KeyCode == Keys.D)
            {
                Player.shopRT.Image = _2mGame.Properties.Resources.Player90;
                Player.moveRightLeft(right, playerDist, gs);
            }
        }
    }
}
