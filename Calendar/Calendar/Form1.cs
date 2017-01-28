using System;
using System.Windows.Forms;


/**
 * メモ
 * 作成するメソッド
 * 年、月を与え、予定を挿入するメソッド
 * テキストボックスをクリックすると予定を新しく挿入するメソッド
 * カーソルを合わせると予定を自然にぽっと出してくれるメソッド
 */

namespace Calendar
{
    public partial class Form1 : Form
    {
        // 今の年月日を格納する
        private int Year = 0;
        public int Month = 0;
        public int Day = 0;

        // クラスの宣言
        private Object obj = new Object();
        private DayController dayController = new DayController();

        public Form1()
        {
            InitializeComponent();
            CalendarStart();
        }

        // 他のメソッドを呼び出す
        private void CalendarStart()
        {
            dayController.GetTodayProperty(ref Year, ref Month, ref Day);
            // カレンダーに年月を格納する
            SetMonth();
            // 今月1日の曜日を取得する
            int weekNum = dayController.GetWeekNum(Year, Month, 1);
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
            int count = 0; // 作った(日付+予定)数

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    // 予定テキストボックスの作成
                    if (i % 2 != 0)
                    {
                        TextBox tb = obj.PlanTB("plan"+count);
                        tableLayoutPanel1.Controls.Add(tb, j, i);
                    }
                    // 日付テキストボックスの作成
                    else {
                        count++;// 作った数のカウント
                        Label lb = obj.DayLabel("day"+count, j);
                        tableLayoutPanel1.Controls.Add(lb, j, i);
                    }
                }
            }
            SetDay(); // 日にちを挿入
        }

        // カレンダーに日にちを挿入する
        private void SetDay()
        {
            int count = 0; // コントロールの数
            int dayNum = 1;
            int dayMaxNum = dayController.GetDayNum(Year, Month);
            bool dayCountFlag = false;
            int weekNum = dayController.GetWeekNum(Year, Month, 1);

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
        }
    }
}
