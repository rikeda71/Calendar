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


/**
 * メモ
 * 作成するメソッド
 * 年、月を与え、日付を挿入するメソッド
 * 年、月を与え、予定を挿入するメソッド
 * テキストボックスをクリックすると予定を新しく挿入するメソッド
 * カーソルを合わせると予定を自然にぽっと出してくれるメソッド
 */

namespace Calendar
{
    public partial class Form1 : Form
    {
        // 今の年月日を格納する
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
            string TodayStr = today.ToString();
            
            // 年月日を取得
            Year = int.Parse(TodayStr.Substring(0, 4));
            Month = int.Parse(TodayStr.Substring(5, 2));
            Day = int.Parse(TodayStr.Substring(8, 2));

            // カレンダーに年月を格納する
            label8.Text = Year.ToString() + " /";
            if (Month < 10) { label9.Text = "0" + Month.ToString(); }
            else { label9.Text = Month.ToString(); }

            // 今月1日の曜日を取得する
            DateTime MonthStart = new DateTime(Year, Month, 1, 0, 0, 0);
            int weekNum = (int)MonthStart.DayOfWeek;

            MakeTable(weekNum);


        }

        // テキストボックスを表に挿入する
        private void MakeTable(int weekNum)
        {
            int count = 0; // 作った(日付+予定)数
            int dayNum = 1; // 日にち
            bool dayCountlag = false;

            for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel1.ColumnCount; j++)
                {
                    TextBox tb = new TextBox();
                    
                    // 編集不可にする
                    tb.ReadOnly = true;
                    tb.BackColor = Color.White;

                    // 予定テキストボックスの作成
                    if (i % 2 != 0)
                    {
                        tb.Multiline = true;
                        tb.Height = tb.Height * 2;
                        tb.Name = "plan" + count;
                    }
                    // 日付テキストボックスの作成
                    else {
                        count++;// 作った数のカウント
                        tb.Name = "day" + count;
                    }

                    tableLayoutPanel1.Controls.Add(tb, j, i);
                    Control a = Find(tableLayoutPanel1, "day1");
                    a.Text = "1";
                }
            }
        }

        // 指定した名前のコントロールを返す
        private static Control Find(Control parent, String name)
        {
            // 指定した名前のコントロールがあるか検索
            var r = parent.Controls[name];

            // 発見した場合、該当コントロールを返す
            if(r != null)
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
    }
}
