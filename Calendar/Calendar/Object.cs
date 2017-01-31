using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calendar
{
    class Object
    {
        // 日付用のラベル
        public Label DayLabel(string name, int dirNum)
        {
            Label lb = new Label();
            lb.Name = name; // 名前を付ける
            lb.BackColor = Color.White;
            lb.Font = new Font("MS Gothic", 12,FontStyle.Bold);
            if (dirNum == 0) { lb.ForeColor = Color.Red; }
            else if (dirNum == 6) { lb.ForeColor = Color.Blue; }
            return lb;
        }

        // 予定用のテキストボックス
        public TextBox PlanTB(string name)
        {
            TextBox tb = new TextBox();
            tb.Name = name; // 名前を付ける
            // 編集不可にする
            tb.ReadOnly = true;
            tb.BackColor = Color.White;
            tb.Multiline = true;
            // 大きさを大きくする
            tb.Height = tb.Height * 2;
            return tb;
        }
    }
}
