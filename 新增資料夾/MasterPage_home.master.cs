using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

using System.IO; //檔案讀寫


public partial class MasterPage_home : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["OK"] != null)
            {
                lb_login_name.Text = Session["OK"].ToString();
                lbtn_login.Text = "Logout";

              
                string[] str_s = Session["OK"].ToString().Split('-');
                string[] arr_auth = DB_authority(str_s[1]);

                if (arr_auth[0] == "99" || arr_auth[0] == "10") //interview
                {
                    li_ii.Visible = true; //邀約
                    lbtn_ii_edit.Visible= true; //編輯
                }

                if (arr_auth[1] == "99" || arr_auth[1] == "10") //evaluation
                {
                    li_e.Visible = true; //考核
                    lbtn_e_edit.Visible = true; //編輯
                }

                if (arr_auth[1] == "20" || arr_auth[1] == "21") //evaluation
                { 
                    li_e.Visible = true; //考核
                }

                if (arr_auth[2] == "99" || arr_auth[2] == "10") //store
                {
                    lbtn_store_edit.Visible = true;
                    li_public.Visible = true;
                }
                li_public.Visible = true;
            }
            else
            {
                lb_login_name.Text = "";
            }
        }
    }

    //邀約
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    protected void lbtn_interview_edit_Click(object sender, EventArgs e)
    {

        string str_cmd = "select MAX(c_id) from dbo.HR_interview";
        string str_id = DB_insert(str_cmd,"year");

        Response.Redirect("/interview_edit.aspx?ID=" + str_id + "&s=i");
        //帶入新增的預設值(end)-----------------------------------------------------------------------
    }

    //考核
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    protected void lbtn_e_edit_Click(object sender, EventArgs e)
    {
        string str_cmd = "select MAX(e_id) from dbo.HR_evaluation";
        string str_id = DB_insert(str_cmd,"year");
        Response.Redirect("/evaluation_edit.aspx?ID=" + str_id + "&s=i");

    }

    //特約
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    protected void lbtn_store_edit_Click(object sender, EventArgs e)
    {
        string str_cmd = "select MAX(id) from dbo.HR_store";
        string str_id = DB_insert(str_cmd, "number");
        Response.Redirect("/store_edit.aspx?ID=" + str_id + "&s=i");
    }

    //特約
    //計數訪客人數
    protected void lbtn_store_query_Click(object sender, EventArgs e)
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

        Response.Redirect("/store_query.aspx");
    }

    //登入、登出
    protected void lbtn_login_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        lb_login_name.Text = "";
        Response.Redirect("~/login.aspx");
    }

    //帶入新增的預設值(start)
    //[事件呼叫：lbtn_interview_edit_Click、lbtn_e_edit_Click]
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    public string DB_insert(string str_db,string type)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string str_c_id = null;

        SqlCommand cmd = new SqlCommand(str_db, Conn); //取最大值
        int c_id = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();
        if (type == "year")
        {
            if (c_id.ToString().Substring(0, 2) == (DateTime.Now.ToString("yyyy")).Substring(2, 2))
            {
                str_c_id = (c_id + 1).ToString(); //當年度
            }
            else
            {
                c_id = int.Parse((DateTime.Now.ToString("yyyy")).Substring(2, 2)); //西元年後兩位數
                str_c_id = ((c_id * 1000000) + 1).ToString();
            }
        }
        else if (type == "number")
        {
            str_c_id = (c_id + 1).ToString();
            str_c_id = string.Format("{0:000}", Convert.ToInt32(str_c_id));
        }
        
        Conn.Close();

        return str_c_id;
    }



    //讀取權限資料表，抓取個別權限
    private string[] DB_authority(string name)
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
                for (int i = 0;i < arr_auth.Length;i++)
                {
                    arr_auth[i] = dr[i].ToString();//選擇個別權限欄位
                }
            }
        }
        cmd.Cancel();
        Conn.Close();

        return arr_auth;
    }





    
}
