using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calendar
{
    public partial class Form2 : Form
    {
        private string[] hour = new string[24];
        private string[] minute = new string[2];

        private DayController dayC = new DayController();
        private DataBaseController database = new DataBaseController();

        private int year, month, day;

        public Form2(int Year, int Month, int Day)
        {
            InitializeComponent();
            monthCalendar1.SetDate(DateTime.Parse(Year + "/" + Month + "/" + Day));
            SetTimes();
            AddComboTimes();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == null)
            {
                MessageBox.Show("予定を入力してください!");
                return;
            }
            dayC.GetDateProperty(ref year, ref month, ref day, monthCalendar1.SelectionEnd);
            database.InsertPlan(year, month, day, comboBox1.Text, comboBox2.Text, textBox1.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 時間を表すパラメータを配列に格納
        private void SetTimes()
        {
            for(int i = 0; i < 24; i++)
            {
                if (i < 10) { hour[i] = "0" + i; }
                else { hour[i] = i.ToString(); }
                
            }

            minute[0] = "00";
            minute[1] = "30";
        }

        // 選択したら"予定を入力"が消える
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(textBox1.Text =="予定を入力") { textBox1.Clear(); }
        }

        // 時間をコンボボックスに格納
        private void AddComboTimes()
        {
            for(int i=0; i < 24; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    comboBox1.Items.Add(hour[i] + ":" + minute[j]);
                    if (i == 0 && j == 0) continue;
                    comboBox2.Items.Add(hour[i] + ":" + minute[j]);
                }
            }
            comboBox2.Items.Add("23:59");

            // ドロップダウンリストの項目数を7にする。
            comboBox1.MaxDropDownItems = 7;
            comboBox1.IntegralHeight = false;
            comboBox2.MaxDropDownItems = 7;
            comboBox2.IntegralHeight = false;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
        }
    }
}
