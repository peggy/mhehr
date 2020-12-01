using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

/// <summary>
/// class_login 的摘要描述
/// </summary>
public class class_login : System.Web.UI.Page
{
    public class_login()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //讀取權限資料表，判斷回傳值是否可登入
    //事件呼叫：login
    public string DB_login_authority(string account, string password, string type)
    {
        string sql_str = null;
        string auth = null;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        if (type == "AD")
        {
            sql_str = "select hr from dbo.hr_authority where account = @my_account";
        }
        if (type == "NAD")
        {
            sql_str = "SELECT department,make_us FROM [MHEDB].[dbo].[MHE_user] where work_no = @my_account and user_id = " + password;
        }

        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_account", SqlDbType.VarChar, 30);
        cmd.Parameters["@my_account"].Value = account; //帳號

        if (type == "AD")
        {
            auth = cmd.ExecuteScalar().ToString(); //回傳第一筆資料
        }

        if (type == "NAD")
        {
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    auth = dr[0].ToString(); //部門
                    auth += "-";
                    auth += dr[1].ToString(); //人名
                }
            }
        }

        cmd.Cancel();
        Conn.Close();

        return auth;
    }

    //讀取權限資料表，以各網頁類型判斷個別權限
    //繼承-事件呼叫：evaluation_query、evaluation_edit、store(gv_store_RowDataBound)
    //new-事件呼叫：masterpage_home
    public string[] DB_authority(string name, string type)
    {
        string[] arr_auth = new string[4]; //修改：新增欄位權限

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        SqlCommand cmd = new SqlCommand("select interview,evaluation,store,extension from dbo.hr_authority where name = @my_name", Conn);
        cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_name"].Value = name; //姓名

        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            //有資料。編輯
            while (dr.Read())
            {
                for (int i = 0; i < arr_auth.Length; i++)
                {
                    if (type == "evaluation")
                    {
                        arr_auth[i] = dr[1].ToString();
                    }
                    else if (type == "store")
                    {
                        arr_auth[i] = dr[2].ToString();
                    }
                    else if (type == "masterpage")
                    {
                        arr_auth[i] = dr[i].ToString();
                    }
                }
            }
        }
        cmd.Cancel();
        Conn.Close();

        return arr_auth;
    }

    //檔案讀寫：寫入counter_store
    //new-事件呼叫：masterpage_home
    public void record_store_count()
    {
        //讀取檔案 (務必修改這個檔案的權限，需要「寫入」的權限)
        StreamReader sr = new StreamReader(Server.MapPath("counter_store.txt"));

        //把檔案內, 原本的訪客人數[加一]
        string visitors = sr.ReadLine();
        visitors = Convert.ToString(Convert.ToInt32(visitors) + 1);
        sr.Close();
        sr.Dispose();


        //寫入檔案
        StreamWriter sw = new StreamWriter(Server.MapPath("counter_store.txt"));
        sw.WriteLine(visitors);
        sw.Close();
        sw.Dispose();

    }

    //檔案讀寫：寫入record_extension_log
    //繼承-事件呼叫：masterpage_home
    public void record_extension_log(string type, string str)
    {
        string path = null;
        string login_name = null;
        if (type == "extension_log") //增刪分機表
        {
            path = "E:\\HR\\file\\record_extension_log.txt";
        }
        else if (type == "extension_user_log") //匯出Excel
        {
            path = "E:\\HR\\file\\record_extension_user_log.txt";
        }
        else if (type == "extension_load_log") //載入分機表
        {
            path = "E:\\HR\\file\\record_extension_load_log.txt";
        }

        //(務必修改這個檔案的權限，需要「寫入」的權限)
        //寫入檔案
        StreamWriter sw = new StreamWriter(path, true);
        if (Session["OK"] != null)
        {
            string[] str_s = Session["OK"].ToString().Split('-');
            login_name = str_s[1];
            sw.Write(login_name);
            sw.Write("---");
        }
        else
        {
            string ip = Page.Request.UserHostAddress;
            sw.Write(ip);
            sw.Write("---");
        }
        sw.Write(str);
        sw.Write("---");
        sw.Write(DateTime.Now.ToString());
        sw.WriteLine();

        sw.Close();
        sw.Dispose();
    }
}