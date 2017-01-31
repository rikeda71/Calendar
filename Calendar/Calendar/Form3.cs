using System;
using System.Windows.Forms;

namespace Calendar
{
    public partial class Form3 : Form
    {
        private DataBaseController database = new DataBaseController();
        private string[][] plans;
        private Form1 form;
        private int year, month, day;

        public Form3(int Year, int Month, int Day, Form1 parent)
        {
            InitializeComponent();
            form = parent;
            year = Year; month = Month; day = Day;
            // フォームの設定
            label1.Text = Year + "年" + Month + "月" + Day + "日の予定";
            PlansListAdd();
            if (listBox1.Items.Count == 0)
            {
                button1.Enabled = false;
            }
        }

        // ListBoxに予定を追加する
        private void PlansListAdd()
        {
            string str;
            plans = database.displayPlans(year, month, day);
            for (int k = 0; k < plans.GetLength(0); k++)
            {
                str = "";
                if (!(plans[k][0] == ""))
                {
                    str += plans[k][0] + " ～ " + plans[k][1] + " ";
                }
                str += plans[k][2];
                listBox1.Items.Add(str);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("予定を選択してください");
                return;
            }
            string select = listBox1.SelectedItem.ToString();
            Form2 form2 = new Form2(year, month, day, true, select);
            form2.FormClosed += new FormClosedEventHandler(form.FormClosedEvent);
            form2.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            form.button1_Click(sender, e);
            this.Close();
        }
    }
}
