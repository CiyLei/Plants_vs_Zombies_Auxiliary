using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private int baseAddress = 0x006A9EC0; //游戏内存基址
        private string processName = "PlantsVsZombies"; //游戏进程名字
        //读取制定内存中的值
        public int ReadMemoryValue(int baseAdd)
        {
            return Helper.ReadMemoryValue(baseAdd, processName);
        }
        //将值写入指定内存中
        public void WriteMemory(int baseAdd, int value)
        {
            Helper.WriteMemoryValue(baseAdd, processName, value);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Helper.GetPidByProcessName(processName) == 0)
            {
                MessageBox.Show("启用之前游戏总该运行吧!!!");
            }
            else
            {
                int address = ReadMemoryValue(baseAddress); //读取基址(该地址不会改变)
                address = address + 0x82C; //获取2级地址
                address = ReadMemoryValue(address);
                address = address + 0x24;
                int value;
                if (int.TryParse(textBox1.Text, out value))
                    WriteMemory(address, value);
                MessageBox.Show("修改完要回到主界面一次哦!!!");
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //注册热键F1，Id号为100。HotKey.KeyModifiers.None表示没添加任何辅助键
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.None, Keys.F1);
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.None, Keys.F2);
            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.None, Keys.F3);
            HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.None, Keys.F4);
            HotKey.RegisterHotKey(Handle, 104, HotKey.KeyModifiers.None, Keys.F5);
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
            //注销Id号为101的热键设定
            HotKey.UnregisterHotKey(Handle, 101);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 102);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 103);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 104);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是F1
                            if (Helper.GetPidByProcessName(processName) == 0)
                                MessageBox.Show("启用之前游戏总该运行吧!!!");
                            else
                            {
                                int address = ReadMemoryValue(baseAddress); //读取基址(该地址不会改变)
                                address = address + 0x768; //获取2级地址
                                address = ReadMemoryValue(address);
                                address = address + 0x5560; //获取存放阳光数值的地址
                                WriteMemory(address, 0x1869F); //写入数据到地址（0x1869F表示99999）
                            }
                            break;
                        case 101:    //按下的是F2
                            if (Helper.GetPidByProcessName(processName) == 0)
                                MessageBox.Show("启用之前游戏总该运行吧!!!");
                            else
                            {
                                if(label2.Text=="F2:无CD(关)")
                                {
                                    label2.Text = "F2:无CD(开)";
                                    timer1.Enabled = true;
                                }
                                else
                                {
                                    label2.Text = "F2:无CD(关)";
                                    timer1.Enabled = false;
                                }
                            }
                            break;
                        case 102:    //按下的是F3
                            if (Helper.GetPidByProcessName(processName) == 0)
                                MessageBox.Show("启用之前游戏总该运行吧!!!");
                            else
                            {
                                WriteMemory(0x69F1C8, 9999);
                                WriteMemory(0x69F1D4, 9999);
                                WriteMemory(0x69F1E0, 9999);
                                WriteMemory(0x69F1EC, 9999);
                                WriteMemory(0x69F1F8, 9999);
                                WriteMemory(0x69F204, 9999);
                                WriteMemory(0x69F210, 9999);
                                WriteMemory(0x69F21C, 9999);
                                WriteMemory(0x69F228, 9999);
                                WriteMemory(0x69F234, 9999);
                                WriteMemory(0x69F240, 9999);
                                WriteMemory(0x69F258, 9999);
                                WriteMemory(0x69F264, 9999);
                            }
                            break;
                        case 103:
                            if (Helper.GetPidByProcessName(processName) == 0)
                                MessageBox.Show("启用之前游戏总该运行吧!!!");
                            else
                            {
                                int address = ReadMemoryValue(baseAddress);
                                address = address + 0x768;
                                address = ReadMemoryValue(address);
                                address = address + 0x138;
                                address = ReadMemoryValue(address);
                                address = address + 0x30;
                                WriteMemory(address, 6);
                            }
                            break;
                        case 104:
                            if (Helper.GetPidByProcessName(processName) == 0)
                                MessageBox.Show("启用之前游戏总该运行吧!!!");
                            else
                            {
                                int address = ReadMemoryValue(baseAddress);
                                address = address + 0x82c;
                                address = ReadMemoryValue(address);
                                address = address + 0x28;
                                WriteMemory(address, 9999);
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int address = ReadMemoryValue(baseAddress); 
            address = address + 0x768; 
            address = ReadMemoryValue(address);
            address = address + 0x144; 
            address = ReadMemoryValue(address);
            for (int i = 0; i < 10; i++)
            {
                int address1 = 0;
                if (i == 0)
                    address1 = address + 0x50;
                if (i == 1)
                    address1 = address + 0xa0;
                if (i == 2)
                    address1 = address + 0xf0;
                if (i == 3)
                    address1 = address + 0x140;
                if (i == 4)
                    address1 = address + 0x190;
                if (i == 5)
                    address1 = address + 0x1e0;
                if (i == 6)
                    address1 = address + 0x230;
                if (i == 7)
                    address1 = address + 0x280;
                if (i == 8)
                    address1 = address + 0x2d0;
                if (i == 9)
                    address1 = address + 0x320;
                WriteMemory(address1, 0);
            }
        }
    }
}
