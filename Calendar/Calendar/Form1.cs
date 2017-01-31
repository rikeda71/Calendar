using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;


/**
 * メモ
 * 予定の更新
 * 予定の削除
 * 予定の通知
 */

namespace Calendar
{
    public partial class Form1 : Form
    {
        // 今の年月日を格納する
        private int Year = 0;
        public int Month = 0;
        public int Day = 0;

        // 予定を格納
        public string[] Plans = new string[42];

        // クラスの宣言
        private Object obj = new Object();
        private DayController day = new DayController();
        private DataBaseController database = new DataBaseController();

        // 文字列から数字を抽出するためのもの
        private Regex re = new Regex(@"[^0-9]");

        public Form1()
        {
            InitializeComponent();
            this.SuspendLayout(); // レイアウトロジックの中断
            CalendarStart();
            this.ResumeLayout(false); // レイアウトロジックの最下位
        }

        // 他のメソッドを呼び出す
        private void CalendarStart()
        {
            day.GetTodayProperty(ref Year, ref Month, ref Day);
            // カレンダーに年月を格納する
            SetMonth();
            // 今月1日の曜日を取得する
            int weekNum = day.GetWeekNum(Year, Month, 1);
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
                        tb.MouseHover += new EventHandler(tb_Click);
                        tb.DoubleClick += new EventHandler(tb_DoubleClick);
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
            int dayMaxNum = day.GetDayNum(Year, Month);
            bool dayCountFlag = false;
            int weekNum = day.GetWeekNum(Year, Month, 1);

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
            int dayMaxNum = day.GetDayNum(Year, Month);
            bool dayCountFlag = false;
            int weekNum = day.GetWeekNum(Year, Month, 1);
            for(int i = 1; i < tableLayoutPanel1.RowCount; i += 2)
            {
                for(int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    count++; // 数のカウント
                    TextBox tb = (TextBox)Find(tableLayoutPanel1, "plan" + count);

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

        // 予定の全表示
        private void tb_Click(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            string FocusName = ((TextBox)sender).Name; // クリックした
            //Help.ShowPopup(tableLayoutPanel1, Plans[ForcuseNum - 1], Control.MousePosition);
            tip.SetToolTip(Find(tableLayoutPanel1, FocusName), Plans[int.Parse(re.Replace(FocusName, "")) - 1]);
        }

        // 予定の登録フォーム表示
        private void tb_DoubleClick(object sender, EventArgs e)
        {
            int textBoxNum = int.Parse(re.Replace(((TextBox)sender).Name,""));
            if (textBoxNum > 31) return;
            Form2 form2 = new Form2(Year, Month, textBoxNum, false);
            form2.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
            form2.Show();
            SetPlans();
            //Control c = Find(sender)
        }

        // 登録フォームを呼び出す
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(Year, Month, Day, false);
            form2.FormClosed += new FormClosedEventHandler(Form2_FormClosed);
            AddOwnedForm(form2);
            form2.Show();
            SetPlans();
        }

        // Form2が閉じたら予定を更新する
        private void Form2_FormClosed(object sendar, FormClosedEventArgs e)
        {
            SetPlans();
        }

    }
}
