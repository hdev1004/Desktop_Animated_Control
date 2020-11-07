using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using System.IO;

namespace AnimatedGifExamApp2
{
    class SQLConnection
    {
        public string defaultPath = @"C:\Hdev_Animated_Control";
        private static SQLiteConnection conn = null;
        private string sqlFile = @"C:\Hdev_Animated_Control\data.sqlite";
        private string txtFile = @"C:\Hdev_Animated_Control\success.txt";

        public SQLiteConnection GetConn()
        {
            return conn;
        }
        public string GetSqlFile()
        {
            return sqlFile;
        }

        public void Crate_DB_File_Click()
        {
            FileInfo fileInfo = new FileInfo(sqlFile);
            //파일 있는지 확인 있을때(true), 없으면(false)
            if (!fileInfo.Exists)
            {
                SQLiteConnection.CreateFile(sqlFile);
            }
        }

        public void Open_DB()
        {
            conn = new SQLiteConnection(@"Data Source=" + sqlFile + ";Version=3;");
            conn.Open();
        }

        public void Crate_Table()
        {
            FileInfo fileInfo = new FileInfo(txtFile);
            //파일 있는지 확인 있을때(true), 없으면(false)
            if (!fileInfo.Exists)
            {
                string sql = "create table members (id varchar(20), width double, height double, top double, left double, loop int, flip int, isTop int, isNew int)";
                SQLiteCommand command = new SQLiteCommand(sql, conn);

                int result = command.ExecuteNonQuery();
                sql = "create index idx01 on members(id)";
                command = new SQLiteCommand(sql, conn);
                result = command.ExecuteNonQuery();

                using (StreamWriter outputFile = new StreamWriter(txtFile))
                {
                    outputFile.WriteLine();
                }
            } 
        }

        public void InsertRow(String id, double width, double height, double top, double left, int loop, int flip, int isTop, int isNew = 1)
        {
            String sql = "insert into members (id, width, height, top, left, loop, flip, isTop, isNew) values ('" + id + "', " + width + ", " + height + ", " + top + ", " + left + ", " + loop + ", " + flip + ", " + isTop + ", " + isNew + ")";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
        }
        public void UpdatetRow(String id, double width, double height, double top, double left, int loop, int flip, int isTop, int isNew)
        {
            String sql = "update members set width=" + width + ", height=" + height + ", top=" + top + ", left=" + left + ", loop=" + loop + ", flip=" + flip + ", isTop=" + isTop + ", isNew= " + isNew + " where id='" + id + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
        }
        public void DeleteRow(String id)
        {
            String sql = "delete from members where id='" + id + "'";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            int result = command.ExecuteNonQuery();
        }

        public void QueryData()
        {
            String sql = "select * from members";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                //MessageBox.Show(rdr["id"] + " " + rdr["width"] + " " + rdr["height"] + " " + rdr["left"] + " " + rdr["top"]);
            }
            rdr.Close();
        }

        public void CloseConnection()
        {
            conn.Close();
        }
    }
}
