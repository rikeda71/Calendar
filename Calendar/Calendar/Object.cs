using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Calendar
{
    class Object
    {
        Form1 form;

        public Object(Form1 parent)
        {
            form = parent;
        }

        // 文字列から数字を抽出するためのもの
        private Regex re = new Regex(@"[^0-9]");

        // 日付用のラベル
        public Label DayLabel(string name, int dirNum)
        {
            Label lb = new Label();
            // 大きさを大きくする
            lb.Width = lb.Width * 2;
            // 名前を付ける
            lb.Name = name; 
            // デザイン
            lb.BackColor = Color.White;
            lb.Font = new Font("MS Gothic", 12, FontStyle.Bold);
            if (dirNum == 0) { lb.ForeColor = Color.Red; }
            else if (dirNum == 6) { lb.ForeColor = Color.Blue; }
            // イベントの追加
            lb.MouseClick += new MouseEventHandler(lb_Click);
            return lb;
        }

        // 予定用のテキストボックス
        public TextBox PlanTB(string name)
        {
            TextBox tb = new TextBox();
            // 大きさを大きくする
            tb.Height = tb.Height * 3;
            tb.Width = tb.Width * 2;
            // 名前を付ける
            tb.Name = name; 
            tb.Font = new Font("MS 明朝", 8);
            // 編集不可にする
            tb.ReadOnly = true;
            tb.BackColor = Color.White;
            tb.Multiline = true;
            // イベントの追加
            tb.MouseHover += new EventHandler(tb_MouseHover);
            tb.MouseClick += new MouseEventHandler(tb_Click);

            return tb;
        }

        /**
         * イベント類
         */

        // マウスを近づけたら
        private void tb_MouseHover(object sender, EventArgs e)
        {
            ToolTip tip = new ToolTip();
            string FocusName = ((TextBox)sender).Name; // クリックした
            //Help.ShowPopup(tableLayoutPanel1, Plans[ForcuseNum - 1], Control.MousePosition);
            tip.SetToolTip(Form1.Find(form.tableLayoutPanel1, FocusName), form.Plans[int.Parse(re.Replace(FocusName, "")) - 1]);
        }

        // 予定の全表示
        private void tb_Click(object sender, EventArgs e)
        {
            int textBoxNum = int.Parse(re.Replace(((TextBox)sender).Name, ""));
            if (textBoxNum > 31) return;
            Form2 form2 = new Form2(form.Year, form.Month, textBoxNum, false,"");
            form2.FormClosed += new FormClosedEventHandler(form.FormClosed);
            form2.Show();
            form.SetPlans();
        }

        // 予定の登録フォーム表示
        private void lb_Click(object sender, EventArgs e)
        {
            int labelNum = int.Parse(re.Replace(((Label)sender).Name, ""));
            if (labelNum > 31) return;
            Form3 form3 = new Form3(form.Year, form.Month, labelNum, form);
            form3.Show();
        }
    }
}
