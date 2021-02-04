using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

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

    //讀取資料表[IT_login_log]，判斷SessionID是否同一人，是則否則false，true
    //事件呼叫：blank(pageload)
    public Boolean DB_login_log(string account, string type)
    {
        Boolean result = false;
        string state = "insert";
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string sql_str = null;
        string DB_account = null;

        sql_str = "SELECT account FROM [dbo].[IT_login_log] where session_id =  @my_session_id";
        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_session_id", SqlDbType.VarChar, 100);
        cmd.Parameters["@my_session_id"].Value = Session.SessionID;
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                DB_account = dr[0].ToString();
            }
        }

        if (DB_account != null) //已存在使用者
        {
            if (DB_account != account) //不同人
            {
                result = true;
                DB_logout_log(DB_account, "delete"); //刪除原使用者DB紀錄
                state = "insert";
            }
            else //同一人
            {
                state = "update";
            }
        }
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }

        //新增或修改資料(s)----------------------------------------------------------------------------------------------------------------------
        if (state == "insert")
        {
            sql_str = "INSERT INTO [dbo].[IT_login_log] ([account], [computer_name], [computer_ip], [session_id], [login_time],[load_time]) " +
            "VALUES ( @my_account, @my_computer_name, @my_computer_ip, @my_session_id, getdate(), getdate())";
        }
        else if (state == "update")
        {
            sql_str = "UPDATE [dbo].[IT_login_log] SET account = @my_account, computer_name = @my_computer_name, computer_ip = @my_computer_ip, login_time = getdate(), load_time = getdate()" +
                "where session_id = @my_session_id";
        }

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_account", SqlDbType.VarChar, 30); //帳號
        cmd.Parameters["@my_account"].Value = account;

        cmd.Parameters.Add("@my_computer_name", SqlDbType.VarChar, 30); //電腦名稱
        cmd.Parameters["@my_computer_name"].Value = System.Net.Dns.GetHostName();

        cmd.Parameters.Add("@my_computer_ip", SqlDbType.VarChar, 20); //ip
        cmd.Parameters["@my_computer_ip"].Value = Page.Request.UserHostAddress;

        cmd.Parameters.Add("@my_session_id", SqlDbType.VarChar, 100); //id
        cmd.Parameters["@my_session_id"].Value = Session.SessionID;

        cmd.ExecuteNonQuery();
        cmd.Cancel();
        //新增或修改資料(e)----------------------------------------------------------------------------------------------------------------------
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        return result;
    }

    //刪除資料表[IT_login_log]：刪除SessionID
    //事件呼叫：class_login(DB_login_log)
    public void DB_logout_log(string account, string type)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        string sql_str = null;

        sql_str = "DELETE FROM [dbo].[IT_login_log] where account = @my_account and session_id = @my_session_id";
        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_account", SqlDbType.VarChar, 30);
        cmd.Parameters["@my_account"].Value = account;
        cmd.Parameters.Add("@my_session_id", SqlDbType.VarChar, 100);
        cmd.Parameters["@my_session_id"].Value = Session.SessionID;
        cmd.ExecuteNonQuery();
        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
    }

    //讀取權限資料表，判斷回傳值是否可登入
    //事件呼叫：login
    public string DB_login_authority(string account,string password,string type)
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
            sql_str = "SELECT dept,name FROM dbo.hr_authority where account = @my_account and password = " + password;
            //sql_str = "SELECT department,make_us FROM [MHEDB].[dbo].[MHE_user] where work_no = @my_account and user_id = " + password;
        }

        SqlCommand cmd =  new SqlCommand(sql_str, Conn);
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
                    auth = dr[0].ToString();
                    auth += "-";
                    auth += dr[1].ToString();
                }
            }
        }  

        cmd.Cancel();
        Conn.Close();

        return auth;
    }

    //讀取權限資料表，以各網頁類型判斷個別權限
    //繼承-事件呼叫：evaluation_query、evaluation_edit、store(gv_store_RowDataBound)、personal(tb_search_TextChanged、btn_update_Click)
    //new-事件呼叫：masterpage_home
    public string[] DB_authority(string account,string type)
    {
        string[] arr_auth = new string[6]; //修改：新增欄位權限
        string sql_str = null;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

       
        sql_str = "select interview,evaluation,store,extension,evaluation_make,personal from dbo.hr_authority where account = @my_account";
        
        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_account", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_account"].Value = account; //姓名

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
                    else if (type == "personal")
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
    //繼承-事件呼叫：extension(pageload、btn_export_Click)、extension_edit(btn_insert_Click、gv_extension_edit_RowDeleting、gv_extension_edit_RowUpdating)
    public void record_extension_log(string type,string str)
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

    //檔案讀寫：寫入
    //繼承-事件呼叫(personal)：personal(pageload、tb_search_TextChanged、btn_hr_update_Click、btn_submit_Click、btn_check_Click、tb_password_mail_TextChanged)
    //繼承-事件呼叫(class_schedule)：class_schedule(Page_Load、export_excel)
    //繼承-事件呼叫(meal)：
    public void record_personal_log(string str, [Optional] string type)
    {
        string path = null;
        if (type == "personal")
        {
            path = "E:\\HR\\file\\record_personal_log.txt";
        }
        else if (type == "class_schedule")
        {
            path = "E:\\HR\\file\\record_class_schedule_log.txt";
        }
        else if (type == "meal")
        {
            path = "E:\\HR\\file\\record_meal_log.txt";
        }
        
        
        //(務必修改這個檔案的權限，需要「寫入」的權限)
        //寫入檔案
        StreamWriter sw = new StreamWriter(path, true);
        
        string[] str_s = Session["OK"].ToString().Split('-');
        sw.Write(str_s[1]);
        sw.Write("---");
        
        sw.Write(str);
        sw.Write("---");
        sw.Write(DateTime.Now.ToString());
        sw.WriteLine();

        sw.Close();
        sw.Dispose();
    }

    
}