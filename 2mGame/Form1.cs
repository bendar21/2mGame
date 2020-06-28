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
        int airborne = 3;
      
        //code to find and use image and give properties from class Shop
        Shop shelf;
        Shop Player;
        Shop covid;
        Shop corona;
        const int NUMBER_OF_FOOD = 10;
        Shop[] yum = new Shop[NUMBER_OF_FOOD];

        Bitmap sh = _2mGame.Properties.Resources.shelf;
        Bitmap shb = _2mGame.Properties.Resources.redCircle;
        Bitmap gs = _2mGame.Properties.Resources.Player;
        Bitmap food = _2mGame.Properties.Resources.blueCircle;
        

        public void Form1_Load(object sender, EventArgs e)
        {
            //code to add images/shelves to form 
            shelf = new Shop(150, 150, sh);
            Controls.Add(shelf.shopRT);
            shelf = new Shop(450, 150, sh);
            Controls.Add(shelf.shopRT);
            shelf = new Shop(750, 150, sh);
            Controls.Add(shelf.shopRT);
            shelf = new Shop(1050, 150, sh);
            Controls.Add(shelf.shopRT);
            shelf = new Shop(1350, 150, sh);
            Controls.Add(shelf.shopRT);
            //player and covid spawner
            Player = new Shop(800, 750, gs);
            Controls.Add(Player.shopRT);
            covid = new Shop(900, 300, shb);
            Controls.Add(covid.shopRT);
            corona = new Shop(900, 300, shb);
            Controls.Add(corona.shopRT);


            tickerTimer.Enabled = true;
            tickerTimer.Interval = 5; //every 5ms
            //create timer event
            tickerTimer.Tick += TickerTimer_Tick;

            for (int i = 0; i < yum.Length; i++)
            {
                int xCoordinate = rand.Next(this.Width - 50);
                int yCoordinate = rand.Next(this.Height - 50);
                yum[i] = new Shop(xCoordinate, yCoordinate, food);
                Controls.Add(yum[i].shopRT);
            }
            if (count == 10)
            {
                Form1 NewForm = new Form1();
                NewForm.Show();
                this.Dispose(false);
                MessageBox.Show("You got Food for ISO");
            }


        }
        private void TickerTimer_Tick(object sender, EventArgs e)
        {
            //code more making groceries arent behind shelves
            for (int i = 0; i < yum.Length; i++)
            {
                if (shelf.shopRT.Bounds.IntersectsWith(yum[i].shopRT.Bounds))
                {
                    yum[i].shopRT.Left -= 2;
                }
            }
            //covid movement y axis
            if (Player.shopRT.Top < 400)
            {
                covid.moveUpDown(up, airborne, shb);
            }
            else if(Player.shopRT.Top > 400)
            {
                covid.moveUpDown(down, airborne, shb);
            }
            //corona movement x axis
            if (Player.shopRT.Left < 750)
            {
                corona.moveRightLeft(left, airborne, shb);
            }
            else if (Player.shopRT.Left > 750)
            {
                corona.moveRightLeft(right, airborne, shb);
            }

            //code for when player hits shopper and gets corona/loses game
            if (Player.shopRT.Bounds.IntersectsWith(covid.shopRT.Bounds))
            {
                tickerTimer.Enabled = false;
                string message = "Oh darn you got covid19 and died";
                MessageBox.Show(message);
                Form1 NewForm = new Form1();
                NewForm.Show();
                this.Dispose(false);       
            }

            if (Player.shopRT.Bounds.IntersectsWith(corona.shopRT.Bounds))
            {
                tickerTimer.Enabled = false;
                string message = "Oh darn you got covid19 and died";
                MessageBox.Show(message);
                Form1 NewForm = new Form1();
                NewForm.Show();
                this.Dispose(false);
            }

            //code to make player stay on screen
            if (Player.shopRT.Top > 800 )
            {
                Player.shopRT.Top -= 2;
            }
            if (Player.shopRT.Top < -1)
            {
                Player.shopRT.Top += 2;
            }
            if (Player.shopRT.Left < 0)
            {
                Player.shopRT.Left += 2;
            }
            if (Player.shopRT.Left > 1535)
            {
                Player.shopRT.Left -= 2;
            }
            //code to make covid stay on screen
            if (covid.shopRT.Top > 720)
            {
                covid.shopRT.Top -= 10;
                covid.moveRightLeft(left, airborne, gs);
            }
            if (covid.shopRT.Top < 20)
            {
                covid.shopRT.Top += 10;
                covid.moveRightLeft(right, airborne, gs);
            }
            if (covid.shopRT.Left < 0)
            {
                covid.shopRT.Left += 10;
                covid.moveUpDown(up, airborne, gs);
            }
            if (covid.shopRT.Left > 1535)
            {
                covid.shopRT.Left -= 10;
                covid.moveUpDown(down, airborne, gs);
            }

            if (corona.shopRT.Top > 650)
            {
                corona.shopRT.Top -= 10;
                corona.moveRightLeft(right, airborne, gs);
            }
            if (corona.shopRT.Top < 20)
            {
                corona.shopRT.Top += 10;
                corona.moveRightLeft(left, airborne, gs);
            }
            if (corona.shopRT.Left < 0)
            {
                corona.shopRT.Left += 10;
                corona.moveUpDown(up, airborne, gs);
            }
            if (corona.shopRT.Left > 1535)
            {
                corona.shopRT.Left -= 10;
                corona.moveUpDown(down, airborne, gs);
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
                Player.moveUpDown(up, playerDist, gs);
                
            }
            if (e.KeyCode == Keys.S)
            {
                Player.moveUpDown(down, playerDist, gs);
               
                

            }
            if (e.KeyCode == Keys.A)
            {
                Player.moveRightLeft(left, playerDist, gs);
               
                

            }
            if (e.KeyCode == Keys.D)
            {
                Player.moveRightLeft(right, playerDist, gs);
                
                
            }
        }
    }
}
