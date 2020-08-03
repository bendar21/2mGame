using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2mGame
{
    class Shop
    {
        //Declare private global variables
        PictureBox shop;
        Bitmap shopImage;


        //code to autosize every picturebox
        public Shop(int argsLeft, int argsTop, Bitmap argsImageFile)
        {
            shop = new PictureBox();
            shop.Image = argsImageFile;
            shop.Left = argsLeft;
            shop.Top = argsTop;
            shop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

        }

        //public accessor so that we can see the pictureBox from our main form
        public PictureBox shopRT
        {
            get { return shop; }
            set { shop = value; }
        }
        //move up or down method. Direction will either be 1 for down or -1 for up
        public void moveUpDown(int direction, int distance, Bitmap shopImage)
        {
            shopRT.Top = shopRT.Top + (direction * distance);
        }
        
        //move right or left method. Direction will either be 1 for right or -1 for Left
        public void moveRightLeft(int direction, int distance, Bitmap shopImage)
        {

            shopRT.Left = shopRT.Left + (direction * distance);
        }
    }
}
