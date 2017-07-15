using System.IO;
using System.Data;
using System.Data.SQLite;
using UnityEngine;

public class DBHelper
{
	public static void Save(DataTable dt, string fullPath, bool backup = true)
	{
        string connStr = "Data Source=" + fullPath + ".db";
        Debug.Log(connStr+" "+dt.TableName);
        using (var connInMem = new SQLiteConnection(connStr))
        {
            using (var cmd = new SQLiteCommand())
            {
                connInMem.Open();
                cmd.Connection = connInMem;

                var sh = new SQLiteHelper(cmd);
                sh.DropTable(dt.TableName);
                sh.CreateTable(dt);

                foreach (DataRow row in dt.Rows)
                {
                    sh.Insert(dt.TableName, row);
                }

                //备份
                if (backup && File.Exists(fullPath))
                {
                    string backFileName = fullPath + ".bak";
                    if (File.Exists(backFileName))
                    {
                        File.Delete(backFileName);
                    }

                    using (var destination = new SQLiteConnection(backFileName))
                    {
                        destination.Open();
                        connInMem.BackupDatabase(destination, "main", "main", -1, null, 0);
                        destination.Close();
                    }
                }
                connInMem.Close();

                Debug.Log("保存成功！");
            }
        }
	}

    public static DataTable Load(string filePath, string tableName)
	{
        string connStr = "Data Source=" + filePath + ".db";
	    using (var connInMem = new SQLiteConnection(connStr))
	    {
            using (var cmd = new SQLiteCommand())
            {
                cmd.Connection = connInMem;
                var sh = new SQLiteHelper(cmd);
                string sqlQuery = "select * from " + tableName;
                var dt = sh.Select(sqlQuery);
                if (dt != null)
                {
                    dt.TableName = tableName;
                }

                return dt;
            }
	    }
	   
	    return null;
	}

}
