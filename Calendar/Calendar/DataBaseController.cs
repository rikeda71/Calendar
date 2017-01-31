using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Calendar
{

    class DataBaseController
    {
        private static SQLiteConnection _conn = null;

        public DataBaseController()
        {
            Connection();
        }

        // DBを開く
        public bool Open()
        {
            try
            {
                _conn.Open();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        
        // DBを閉じる
        public void Close()
        {
            _conn.Close();
        }

        // DBと接続
        private static void Connection()
        {
            _conn = new SQLiteConnection();
            _conn.ConnectionString = "Data Source=plans.db;Version=3;";
        }

        // 予定を挿入
        public void InsertPlan(int Year, int Month, int Day, string time1, string time2, string plan)
        {
            if (!Open()) return;
            SQLiteCommand command = _conn.CreateCommand();
            //string insertcommand = "(" + Year + ", " + Month + ", " + Day + ", '" + time1 + "', '" + time2 + "', '" + plan + "' )";
            //Console.WriteLine(insertcommand);
            command.CommandText = $"insert into plan values({Year}, {Month}, {Day}, '{time1}', '{time2}', '{plan}')";
            command.ExecuteNonQuery();
            Close();
        }

        // 予定の削除
        public void DeletePlan(int Year, int Month, int Day, string time1, string time2, string plan)
        {
            if (!Open()) return;
            SQLiteCommand command = _conn.CreateCommand();
            //string deletecommand = 
        }

        // 与えられた日付の予定をすべて出力
        // 配列で返す
        public string[][] displayPlans(int Year, int Month, int Day)
        {
            if (!Open()) return null;

            try
            {
                SQLiteCommand command = _conn.CreateCommand();
                command.CommandText = $"select Start, End, Plan from plan where Year = {Year} AND Month = {Month} AND Day = {Day}";
                using(SQLiteDataReader sdr = command.ExecuteReader())
                {
                    List<string[]> tuples = new List<string[]>();
                    for(int i = 0; sdr.Read(); i++)
                    {
                        string[] column = new string[sdr.FieldCount];
                        for(int j=0;j<sdr.FieldCount; j++)
                        {
                            column[j] = sdr[j].ToString();
                        }
                        tuples.Add(column);
                    }
                    Close();
                    return tuples.ToArray();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Close();
                return null;
            }
        }

        // 与えられたクエリを実行する
        public void ActionQuerie(string str)
        {
            Open();
            SQLiteCommand command = _conn.CreateCommand();
            command.CommandText = str;
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Close();
        }
    }
}
