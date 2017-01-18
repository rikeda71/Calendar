using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Calendar
{
    public partial class Form1 : Form
    {
        private int Year;
        private int Month;
        private int Day;
        DateTime today = new DateTime();

        public Form1()
        {
            InitializeComponent();
            CalendarStart();
        }

        private void CalendarStart()
        {
            today = DateTime.Today;
            string TodayStr = today.ToString("月");
            int cumma = TodayStr.IndexOf(",");
            Month = int.Parse(TodayStr.Substring(0, cumma-1));
            Year = int.Parse(TodayStr.Substring(cumma+1));
            
        }
    }
}
