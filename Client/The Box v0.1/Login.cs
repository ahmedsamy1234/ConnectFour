﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace The_Box_v0._1
{
    public partial class Login : Form
    {
        float _xStartBox;
        float _yStartBox;
        float _xEndBox;
        float _yEndBox;
        float _widthBox;
        float _heightBox;
        //User user1;
        public static List<User> allUsers;
        ClientSocket UserSocket;
        public User Creator;
        public Login()
        {
            InitializeComponent();
      
            allUsers = new List<User>();
           Room.rooms  = new List<Room>();
            
            UserSocket = ClientSocket.SingletonClient();
            
        }

        // Make Form Movable
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void HeadLoginPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        ///
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            Creator = new User(User_Name.Text);
            Creator.socket1 = UserSocket;

            //allUsers.Add(user1);

            ((ClientSocketInterface)Creator.socket1).SendRequest("log");
            ((ClientSocketInterface)Creator.socket1).ResponseLog(Creator);
            MainForm mainForm = new MainForm(this, Creator);
            mainForm.Show();
            this.Hide();
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool b = true;

       
            if(User_Name.Text.Length == 0)
            {
                label4.Visible = true;
                Log_In_Button.Enabled = false;
            }

            else if(User_Name.Text.Length > 0)
            {
                label4.Visible = false;
                Log_In_Button.Enabled = true;
                ((ClientSocketInterface)UserSocket).SendRequest("IfisInList");

                if (((ClientSocketInterface)UserSocket).ResponsecheckIfisInList(User_Name.Text))
                {

                    label3.Visible = true;
                    Log_In_Button.Enabled = false;
                }else

                {

                    label3.Visible = false;
                    Log_In_Button.Enabled = true;
                }

             
            }           
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
