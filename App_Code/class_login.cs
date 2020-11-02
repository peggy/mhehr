using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

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
    public int DB_login_authority(string name)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand("select hr from dbo.hr_authority where name = @my_name", Conn);
        cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_name"].Value = name; //姓名

        int hr = Convert.ToInt32(cmd.ExecuteScalar()); //回傳第一筆資料

        cmd.Cancel();
        Conn.Close();

        return hr;
    }

    //讀取權限資料表，以各網頁類型判斷個別權限
    //繼承-事件呼叫：evaluation_query、evaluation_edit、store(gv_store_RowDataBound)
    //new-事件呼叫：masterpage_home
    public string[] DB_authority(string name, string type)
    {
        string[] arr_auth = new string[3]; //修改：新增欄位權限

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        SqlCommand cmd = new SqlCommand("select interview,evaluation,store from dbo.hr_authority where name = @my_name", Conn);
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
}