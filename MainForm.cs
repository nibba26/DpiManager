using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace DpiManager
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance

        public enum KeyModifiers
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        const int WM_HOTKEY = 0x0312;

        Keys keyUp = Keys.Up;
        Keys keyDown = Keys.Down;
        int idx = 0;

        //bool isEnable = false;
        //int countdown = 11;


        public MainForm()
        {
            InitializeComponent();

            RegisterHotKey(this.Handle, HOTKEY_ID, KeyModifiers.Control | KeyModifiers.Shift, keyUp);
            RegisterHotKey(this.Handle, HOTKEY_ID, KeyModifiers.Control | KeyModifiers.Shift, keyDown);
        }


        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)message.LParam & 0xFFFF);
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());

                    if ((KeyModifiers.Control | KeyModifiers.Shift) == modifier && keyUp == key)
                    {
                        IncrementChangeScale();
                    }

                    if ((KeyModifiers.Control | KeyModifiers.Shift) == modifier && keyDown == key)
                    {
                        DecrementChangeScale();
                    }

                    break;
            }
            base.WndProc(ref message);
        }


        private void ChangeScale()
        {
            int disp = 2;
            int scale = 100 + idx * 25;

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = @".\resources\AdjustDisplay.exe";
            info.Arguments = $"disp={disp} scale={scale}";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(info);
            btnStart.Text = scale.ToString();
        }

        private void IncrementChangeScale()
        {
            idx++;
            if (idx > 4)
            {
                idx = 0;
            }

            ChangeScale();
        }

        private void DecrementChangeScale()
        {
            idx--;
            if (idx <= 0)
            {
                idx = 4;
            }

            ChangeScale();
        }



        private void btnWatch_Click(object sender, EventArgs e)
        {
            IncrementChangeScale();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //this.TopMost = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int x = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int y = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Location = new Point(x, y);
        }
    }


}
