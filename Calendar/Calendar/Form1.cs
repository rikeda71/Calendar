using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;


/**
 * メインフォーム
 */

namespace Calendar
{
    public partial class Form1 : Form
    {
        // 今の年月日を格納する
        public int Year = 0;
        public int Month = 0;
        public int Day = 0;

        // 予定を格納
        public string[] Plans = new string[42];
        private string[][] googleCalPlans;

        // クラスの宣言
        private Object obj;
        private DayController dayC;
        private DataBaseController database;
        private GoogleCalendarInfo googleCalInfo;

        public Form1()
        {
            InitializeComponent();

            obj = new Object(this);
            dayC = new DayController();
            database = new DataBaseController();
            googleCalInfo = new GoogleCalendarInfo();

            this.SuspendLayout(); // レイアウトロジックの中断
            googleCalPlans = googleCalInfo.GetGoogleCalPlan();
            CalendarStart();
            this.ResumeLayout(false); // レイアウトロジックの最下位
        }

        // 他のメソッドを呼び出す
        private void CalendarStart()
        {
            dayC.GetTodayProperty(ref Year, ref Month, ref Day);
            // カレンダーに年月を格納する
            SetMonth();
            // 今月1日の曜日を取得する
            int weekNum = dayC.GetWeekNum(Year, Month, 1);
            MakeTable(weekNum);
        }

        // カレンダーに年月を挿入
        private void SetMonth()
        {
            label8.Text = Year + " /";
            if (Month < 10) { label9.Text = "0" + Month.ToString(); }
            else { label9.Text = Month.ToString(); }
        }

        // テキストボックスを表に挿入する
        private void MakeTable(int weekNum)
        {
            int dayCount = 0; // 作った日付数
            int planCount = 0; // 作った予定数

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    // 予定テキストボックスの作成
                    if (i % 2 != 0)
                    {
                        planCount++;
                        TextBox tb = obj.PlanTB("plan" + planCount);
                        tableLayoutPanel1.Controls.Add(tb, j, i);
                        
                    }
                    // 日付テキストボックスの作成
                    else {
                        dayCount++;// 作った数のカウント
                        Label lb = obj.DayLabel("day" + dayCount, j);
                        tableLayoutPanel1.Controls.Add(lb, j, i);
                    }
                }
            }
            SetDay(); // 日にちを挿入
            SetPlans(); // 予定を挿入
        }

        // カレンダーに日にちを挿入する
        private void SetDay()
        {
            int count = 0; // コントロールの数
            int dayNum = 1;
            int dayMaxNum = dayC.GetDayNum(Year, Month);
            bool dayCountFlag = false;
            int weekNum = dayC.GetWeekNum(Year, Month, 1);

            for (int i = 0; i < tableLayoutPanel1.RowCount; i+=2)
            {
                for(int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    count++; // 数のカウント
                    Label lb = (Label)Find(tableLayoutPanel1, "day" + count);
                    lb.Text = "";
                    if (weekNum == j) { dayCountFlag = true; }
                    if(dayCountFlag && dayNum <= dayMaxNum)
                    {
                        lb.Text = dayNum.ToString();
                        if (dayC.IfToday(Year, Month, dayNum)) { lb.BackColor = Color.Gray; }
                        else { lb.BackColor = Color.White; }
                        dayNum++;
                    }
                }
            }
        }

        // カレンダーに予定を挿入する
        public void SetPlans()
        {
            int count = 0;
            int dayNum = 1;
            int dayMaxNum = dayC.GetDayNum(Year, Month);
            bool dayCountFlag = false;
            int weekNum = dayC.GetWeekNum(Year, Month, 1);
            for(int i = 1; i < tableLayoutPanel1.RowCount; i += 2)
            {
                for(int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    count++; // 数のカウント
                    TextBox tb = (TextBox)Find(tableLayoutPanel1, "plan" + count);
                    tb.Clear();

                    if (weekNum == j) { dayCountFlag = true; }
                    if (dayCountFlag && dayNum <= dayMaxNum)
                    {
                        string[][] plan = database.displayPlans(Year, Month, dayNum);
                        dayNum++; // 参照したから日付の加算
                        if (plan == null) continue;

                        string str = "";

                        for (int k = 0; k < plan.GetLength(0); k++)
                        {
                            if (! (plan[k][0] == "")) {
                                str += plan[k][0] + " ～ " + plan[k][1] + " ";
                            }
                            str += plan[k][2] + Environment.NewLine;
                        }
                        tb.Text = str;
                        Plans[count - 1] = str;
                    }
                }
            }
            SynchroGoogleCal();
            SetPlansColor();
        }

        // 予定のあるところの色付け
        private void SetPlansColor()
        {
            for(int i = 0; i < 42; i++)
            {
                TextBox tb = (TextBox)Find(tableLayoutPanel1, "plan" + (i + 1).ToString());
                if(tb.Text == "") { tb.BackColor = Color.White; }
                else { tb.BackColor = Color.LightBlue; }
            }
        }

        // 指定した名前のコントロールを返す
        public static Control Find(Control parent, String name)
        {
            // 指定した名前のコントロールがあるか検索
            var r = parent.Controls[name];

            // 発見した場合、該当コントロールを返す
            if (r != null)
            {
                return r;
            }

            // 全部で発見できなかった場合、子コントロールで探す
            foreach (Control c in parent.Controls)
            {
                // TODO:必要ならcに対する条件を追加します。

                // 子コントロールcを起点にしてnameを検索します。
                r = Find(c, name);
                if (r != null)
                {
                    // cの子孫にnameが見つかった場合、該当コントロールを返します。
                    return r;
                }
            }
            // nameが見つからなかった場合、nullを返します。
            return null;
        }

        // カレンダーを進める
        private void Next_Click(object sender, EventArgs e)
        {
            if(++Month > 12)
            {
                Year++;
                Month = 1;
            }
            SetMonth();
            SetDay();
            SetPlans();
        }

        // カレンダーを戻す
        private void Return_Click(object sender, EventArgs e)
        {
            if(--Month < 1)
            {
                Year--;
                Month = 12;
            }
            SetMonth();
            SetDay();
            SetPlans();
        }

        // 登録フォームを呼び出す
        public void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(Year, Month, Day, false, "");
            form2.FormClosed += new FormClosedEventHandler(FormClosedEvent);
            AddOwnedForm(form2);
            form2.Show();
            SetPlans();
        }

        // Form2が閉じたら予定を更新する
        public void FormClosedEvent(object sendar, FormClosedEventArgs e)
        {
            SetPlans();
        }

        // GoogleCalendarとの同期
        private void SynchroGoogleCal()
        {
            for(int i = 0; i < googleCalPlans.GetLength(0); i++)
            {
                int GYear = int.Parse(googleCalPlans[i][1].Substring(0, 4));
                int GMonth = int.Parse(googleCalPlans[i][1].Substring(5, 2));
                int GDay = int.Parse(googleCalPlans[i][1].Substring(8, 2));

                // 同じ年月か判定
                if (Year != GYear || Month != GMonth) continue;
                for(int j = 0; j < 42; j++)
                {
                    Label lb = (Label)Find(tableLayoutPanel1, "day" + (j+1).ToString());
                    if (lb.Text == "") continue; 
                    if(int.Parse(lb.Text) == GDay) {
                        TextBox tb = (TextBox)Find(tableLayoutPanel1, "plan" + (j + 1).ToString());
                        tb.Text += googleCalPlans[i][0] + Environment.NewLine;
                    }
                }

            }

        }
    }
}
