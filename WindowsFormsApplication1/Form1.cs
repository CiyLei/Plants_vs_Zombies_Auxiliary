using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Process jc;
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }
        public const int MEM_COMMIT = 0x1000;       //已物理分配
        public const int MEM_PRIVATE = 0x20000;
        public const int PAGE_READWRITE = 0x04;     //可读写内存
        [DllImport("kernel32.dll")]     //查询内存块信息
        public static extern int VirtualQueryEx(
            IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int size, out int numBytesRead);
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int size, out int numBytesWrite);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll ")]
        static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, out int lpBuffer, int nSize, out int lpNumberOfBytesRead);

        string game_name = "植物大战僵尸中文版";
        string jizhi = "";
        int pianyi = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            IntPtr hwnd = FindWindow(null, game_name);
            //RegisterHotKey(hwnd,

            //int nv_zhi = Int32.Parse("00020C98", System.Globalization.NumberStyles.HexNumber);
            /*读取内存基址
            string jizhi = ReadID(game_name, 0x006A9EC0);
            int pianyi = int.Parse(jizhi) + 0x768;
            jizhi = ReadID(game_name, pianyi);
            pianyi = int.Parse(jizhi) + 0x5560;
            jizhi = ReadID(game_name, pianyi);
            */

            //WriteID(game_name, "00020C98", "2");  修改内存基址

            Process[] ps = Process.GetProcesses();
            for (int i = 0; i < Process.GetProcesses().Length; i++)
            {
                if (ps[i].MainWindowTitle == game_name)
                    jc = Process.GetProcesses()[i];
            }
        }
        private void WriteID(string ID_name, string ID_jizhi,string ID_zhi)
        {
            int bytesSize = 1 << 4;
            byte[] byWrite = new byte[bytesSize];
            long numWrite = Convert.ToInt64(ID_zhi);
            long numAddr = Convert.ToInt64(ID_jizhi, 10);
            int numWriteSize = 0;
            //将数据写入byte数组中
            for (int i = 0; i < bytesSize; i++)
            {
                byWrite[i] = (byte)((numWrite & (0x00000000000000FF << i * 8)) >> i * 8);
            }
            if (WriteProcessMemory(jc.Handle, (IntPtr)numAddr, byWrite, bytesSize, out numWriteSize))
            {
                //if (numWriteSize == bytesSize)      //如果和实际写入字节数一样提示成功
                    //MessageBox.Show("写入成功!");
            }
            //else
                //MessageBox.Show("写入失败");
        }
        private string ReadID(string formID, int ID)
        {
            IntPtr hwnd = FindWindow(null, formID);
            //const int PROCESS_ALL_ACCESS = 0x1F0FFF;
            const int PROCESS_VM_READ = 0x0010;
            const int PROCESS_VM_WRITE = 0x0020;
            if (hwnd != IntPtr.Zero)
            {
                int calcID;
                int calcProcess;
                int dataAddress;
                int readByte;
                GetWindowThreadProcessId(hwnd, out calcID);
                calcProcess = OpenProcess(PROCESS_VM_READ | PROCESS_VM_WRITE, false, calcID);
                ReadProcessMemory(calcProcess, ID, out dataAddress, 4, out readByte);
                return dataAddress.ToString();
            }
            else
            {
                //MessageBox.Show("没有找到窗口");
                return "0";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.None, Keys.F1);
            //注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.None, Keys.F2);
            //注册热键Alt+D，Id号为102。HotKey.KeyModifiers.Alt也可以直接使用数字1来表示。
            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.None, Keys.F3);
            HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.None, Keys.F4);
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            string jizhi;
            int pianyi;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Shift+S
                            jizhi = ReadID(game_name, 0x006A9EC0);
                            pianyi = int.Parse(jizhi) + 0x768;
                            jizhi = ReadID(game_name, pianyi);
                            pianyi = int.Parse(jizhi) + 0x5560;
                            WriteID(game_name, pianyi.ToString(), "9990");
                            break;
                        case 101:    //按下的是Ctrl+B
                            if (label2.Text == "F2:无CD(关)")
                            {
                                label2.Text = "F2:无CD(开)";
                                timer2.Enabled = true;
                            }
                            else
                            {
                                label2.Text = "F2:无CD(关)";
                                timer2.Enabled = false;
                            }
                            break;
                        case 102:    //按下的是Alt+D
                            WriteID(game_name, "6943176", "9999");
                            WriteID(game_name, "6943188", "9999");
                            WriteID(game_name, "6943200", "9999");
                            WriteID(game_name, "6943212", "9999");
                            WriteID(game_name, "6943224", "9999");
                            WriteID(game_name, "6943236", "9999");
                            WriteID(game_name, "6943248", "9999");
                            WriteID(game_name, "6943260", "9999");
                            WriteID(game_name, "6943272", "9999");
                            WriteID(game_name, "6943284", "9999");
                            WriteID(game_name, "6943296", "9999");
                            WriteID(game_name, "6943320", "9999");
                            WriteID(game_name, "6943332", "9999");
                            break;
                        case 103:    
                            jizhi = ReadID(game_name, 0x006A9EC0);
                            pianyi = int.Parse(jizhi) + 0x768;
                            jizhi = ReadID(game_name, pianyi);
                            pianyi = int.Parse(jizhi) + 0x138;
                            jizhi = ReadID(game_name, pianyi);
                            pianyi = int.Parse(jizhi) + 0x30;
                            WriteID(game_name, pianyi.ToString(), "6");
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }
        private void Form1_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
            //注销Id号为101的热键设定
            HotKey.UnregisterHotKey(Handle, 101);
            //注销Id号为102的热键设定
            HotKey.UnregisterHotKey(Handle, 102);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            jizhi = ReadID(game_name, 0x006A9EC0);
            pianyi = int.Parse(jizhi) + 0x768;
            jizhi = ReadID(game_name, pianyi);
            pianyi = int.Parse(jizhi) + 0xe4;
            jizhi = ReadID(game_name, pianyi);
            pianyi = int.Parse(jizhi) + 0x50;
            WriteID(game_name, pianyi.ToString(), "1");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            jizhi = ReadID(game_name, 0x006A9EC0);
            pianyi = int.Parse(jizhi) + 0x768;
            jizhi = ReadID(game_name, pianyi);
            pianyi = int.Parse(jizhi) + 0x144;
            jizhi = ReadID(game_name, pianyi);
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 1)
                    pianyi = int.Parse(jizhi) + 0xa0;
                if (i == 2)
                    pianyi = int.Parse(jizhi) + 0xf0;
                /*if (i == 3)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 4)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 5)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 6)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 7)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 8)
                    pianyi = int.Parse(jizhi) + 0x50;
                if (i == 9)
                    pianyi = int.Parse(jizhi) + 0x50;*/
                WriteID(game_name, pianyi.ToString(), "0");
                jizhi = ReadID(game_name, pianyi);
            }
        }
    }
}
