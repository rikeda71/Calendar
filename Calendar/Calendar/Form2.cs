using System;
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
        private string time1, time2, plan; // 更新or削除の時のみ使用
        bool updateFlag = false;

        public Form2(int Year, int Month, int Day, bool Exist, string nowPlan)
        {
            InitializeComponent();

            // フォームの設定
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            monthCalendar1.SetDate(DateTime.Parse(Year + "/" + Month + "/" + Day));
            SetTimes();
            AddComboTimes();
            // 既にある予定かどうか
            if (!Exist) { button3.Hide(); }
            // 既にあるなら
            else
            {
                if (nowPlan.Length < 10)
                {
                    plan = nowPlan;
                    textBox1.Text = nowPlan;
                }
                // 日付を入れているなら
                else if (nowPlan.Substring(2, 1) == ":" && nowPlan.Substring(10, 1) == ":")
                {
                    time1 = nowPlan.Substring(0, 5); time2 = nowPlan.Substring(8, 5); plan = nowPlan.Substring(14);
                    comboBox1.SelectedIndex = comboBox1.Items.IndexOf(time1);
                    comboBox2.SelectedIndex = comboBox2.Items.IndexOf(time2);
                    textBox1.Text = plan;
                }
                updateFlag = true;
                year = Year; month = Month; day = Day;
                
            }
        }

        // 予定の追加、更新
        private void button1_Click(object sender, EventArgs e)
        {
            string start = comboBox1.Text, end = comboBox2.Text;
            // 予定を挿入していいかチェックする
            // 予定の確認
            if (textBox1.Text == null)
            {
                MessageBox.Show("予定を入力してください!");
                return;
            }
            // 時間の前後関係の確認
            if (comboBox1.Text != "" && comboBox2.Text != "")
            {
                DateTime t1 = DateTime.Parse(start + " :00");
                DateTime t2 = DateTime.Parse(end + " :00");
                if (dayC.TimeCompare(t1, t2) < 1)
                {
                    MessageBox.Show("時間の前後関係が間違っています");
                    comboBox2.SelectedIndex = comboBox1.SelectedIndex;
                    return;
                }
            }
            else { start = ""; end = ""; }

            if (updateFlag) { UpdatePlan(start, end); }
            else { AddPlan(start, end); }

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

        private void button3_Click(object sender, EventArgs e)
        {

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

        //　開始時間を選んだら終了時間を自動で30分後にする
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = comboBox1.SelectedIndex;
        }

        // 予定の追加
        private void AddPlan(string start, string end)
        {
            dayC.GetDateProperty(ref year, ref month, ref day, monthCalendar1.SelectionEnd);
            database.InsertPlan(year, month, day, start, end, textBox1.Text);
        }

        // 予定の更新
        private void UpdatePlan(string start, string end)
        {
            string queries = "", condition;
            string updateStr = UpdateType(start, end);
            condition = UpdateCondition();
            queries = "update plan" + " set " + updateStr + " where " + condition;
            database.ActionQuerie(queries);
        }

        // 変更位置を返す
        private string UpdateType(string start, string end)
        {
             string updateStr = " ";

            if (monthCalendar1.SelectionEnd.Year != year)
            {
                updateStr += "Year = " + monthCalendar1.SelectionEnd.Year + ",";
            }
            if (monthCalendar1.SelectionEnd.Month != month)
            {
                updateStr += "Month = " + monthCalendar1.SelectionEnd.Month + ",";
            }
            if (monthCalendar1.SelectionEnd.Day != day)
            {
                updateStr += "Day = " + monthCalendar1.SelectionEnd.Day + ",";
            }
            if (start != time1)
            {
                updateStr += "Start = '" + start + "',";
            }
            if (end != time2)
            {
                updateStr += "End = '" + end + "',";
            }
            if (textBox1.Text != plan)
            {
                updateStr += "plan = '" + textBox1.Text + "'";
            }
            // ","が残っていたら削除
            if (updateStr.Substring(updateStr.Length - 1) == ",")
            {
                updateStr = updateStr.Remove(updateStr.Length - 1);
            }
            return updateStr;
        }

        // 変更条件を返す
        private string UpdateCondition()
        {
            return "Year = " + year + " AND Month = " + month + " AND Day = " + day + " AND Start = '" + time1 + "' AND End ='" + time2 + "' AND plan ='" + plan + "'";
        }
    }
}
