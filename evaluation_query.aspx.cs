using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;

public partial class evaluation_query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        //權限(s)------------------------------------------------------------------------------
        string[] str_s = Session["OK"].ToString().Split('-');
        string[] arr_auth = DB_authority(str_s[1]);
        string condition;
        if (arr_auth[0] == "99" || arr_auth[0] == "10")
        {
            condition = "%";
            lb_e_dept.Visible = true;
            ddl_e_dept.Visible = true;
        }
        else if (arr_auth[0] == "21")
        {
            condition = "";
            lb_e_dept.Visible = true;
            ddl_e_dept.Visible = true;
        }
        else
        {
            if (str_s[0].Contains("MHE")) //for jackie、董事長
            {
                condition = "%";
                lb_e_dept.Visible = true;
                ddl_e_dept.Visible = true;
            }
            else
            {
                condition = str_s[0].Substring(0, 3);
                ddl_e_dept.SelectedValue = condition;
            }
        }

        //權限(e)------------------------------------------------------------------------------

        string sql_str;
        if (!IsPostBack)//第一次執行本程式
        {
            //連結資料庫 讀取HRS部門、HRS職稱、讀取資料(s)------------------------------------------------------------------------------------------------------------------------
            sql_str = "select paak003 FROM  EHRS.hrs_mis.dbo.WPAak where paak001 = 'MING'";
            DB_search(sql_str, "dr", "ddl_e_opening");

            if (arr_auth[0] == "21")//查詢生產部底下部門
            {
                sql_str = "select distinct REPLACE(REPLACE([pa11003],'常日',''),'輪班','') FROM  EHRS.hrs_mis.dbo.WPA11 where pa11001 = 'MING' and [pa11003] in ('生產部','工程課','生管課','安環課','物料課','品管課','倉管課','製造課','製程課')";
            }
            else
            {
                sql_str = "select distinct REPLACE(REPLACE([pa11003],'常日',''),'輪班','') FROM  EHRS.hrs_mis.dbo.WPA11 where pa11001 = 'MING'";
            }
            DB_search(sql_str, "dr", "ddl_e_dept");

            if (arr_auth[0] == "21")//查詢生產部底下部門
            {
                sql_str = "SELECT top(10)e_id as 序號,e_empno as 工號, e_name as 姓名, e_opening as 職稱, e_dept as 部門, e_change as 項目, e_start_date as 考核開始日, e_end_date as 考核結束日 " +
                      "FROM[dbo].[HR_evaluation] " +
                      "where e_dept in  ('生產部', '工程課', '生管課', '安環課', '物料課', '品管課', '倉管課', '製造課', '製程課') " +
                      "order by ep_res_time desc, ep_res_check_time desc,e_modify_time desc ";
            }
            else
            {
                sql_str = "SELECT top(10)e_id as 序號,e_empno as 工號, e_name as 姓名, e_opening as 職稱, e_dept as 部門, e_change as 項目, e_start_date as 考核開始日, e_end_date as 考核結束日 " +
                      "FROM[dbo].[HR_evaluation] " +
                      "where e_dept like @my_e_dept " +
                      "order by ep_res_time desc, ep_res_check_time desc,e_modify_time desc ";
            }
                
            DB_search(sql_str, "gv", "gv_evaluation",condition);
            //連結資料庫 讀取HRS部門、HRS職稱、讀取資料(e)------------------------------------------------------------------------------------------------------------------------
        }
    }

    //點擊「查詢」按鈕，依條件篩出資料 綁定GridView
    protected void btn_query_Click(object sender, EventArgs e)
    {
        DBInit();
    }

    //資料庫-binding candidate gridview
    protected void DBInit()
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        StringBuilder sqlsb = new StringBuilder();

        //以判斷是否有值才增加SQL篩選條件
        sqlsb.Append(
            "select e_id as 序號,e_empno as 工號, e_name as 姓名, e_opening as 職稱, e_dept as 部門, e_change as 項目, e_start_date as 考核開始日, e_end_date as 考核結束日, ep_a_state as 狀態 " +
            "from dbo.HR_evaluation " +
            "where e_empno like '%' + @my_e_empno + '%' " +
            "and e_dept like '%' + @my_e_dept + '%' " +
            "and e_opening like '%' + @my_e_opening + '%' ");

        if (rbl_ep_a_state.SelectedIndex >= 0)
        {
            sqlsb.Append(" and ep_a_state like '%' + @my_ep_a_state + '%'");
        }

        if (tb_e_start_date_1.Text != string.Empty && tb_e_start_date_2.Text != string.Empty) //注意!Text判斷為空值需用此方法
        {
            sqlsb.Append(" and e_start_date between  @my_e_start_date_1  and  @my_e_start_date_2");//注意! 查詢日期 不須加單引號 只需參數即可
        }

        if (tb_e_end_date_1.Text != string.Empty && tb_e_end_date_2.Text != string.Empty) 
        {
            sqlsb.Append(" and e_end_date between  @my_e_end_date_1  and  @my_e_end_date_2");
        }
        sqlsb.Append(" order by ep_res_time, e_modify_time desc");

        SqlCommand cmd = new SqlCommand(sqlsb.ToString(), Conn);

        cmd.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_empno"].Value = tb_e_empno.Text; //工號

        cmd.Parameters.Add("@my_e_dept", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_dept"].Value = ddl_e_dept.SelectedValue; //部門

        cmd.Parameters.Add("@my_e_opening", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_opening"].Value = ddl_e_opening.SelectedValue; //職稱

        if (rbl_ep_a_state.SelectedIndex >= 0)
        {
            cmd.Parameters.Add("@my_ep_a_state", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_ep_a_state"].Value = rbl_ep_a_state.SelectedValue;
        }

        if (tb_e_start_date_1.Text != string.Empty && tb_e_start_date_2.Text != string.Empty) //注意!Text判斷為空值需用此方法
        {
            cmd.Parameters.Add("@my_e_start_date_1", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_e_start_date_2", SqlDbType.DateTime);

            DateTime dt_time_2 = DateTime.ParseExact(tb_e_start_date_2.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces); //注意!Text需先轉換成DateTime。但 因jQuery有先定義日期格式 所以沒轉也沒關係
            dt_time_2 = dt_time_2.AddDays(1); //因查詢日期為當天的00:00:00 想在查詢期間含當天 就用加一天的方式
                                       
            cmd.Parameters["@my_e_start_date_1"].Value = tb_e_start_date_1.Text; //考核開始日(起)
            cmd.Parameters["@my_e_start_date_2"].Value = dt_time_2; //考核開始日(迄)
        }
        else if (tb_e_start_date_1.Text == string.Empty && tb_e_start_date_2.Text != string.Empty)
        {
            Response.Write("<script language='javascript'>confirm('請選擇考核開始日的開始日期!');</script>");
            return;
        }
        else if (tb_e_start_date_1.Text != string.Empty && tb_e_start_date_2.Text == string.Empty)
        {
            Response.Write("<script language='javascript'>confirm('請選擇考核開始日的結束日期!');</script>");
            return;
        }
            

        if (tb_e_end_date_1.Text != string.Empty && tb_e_end_date_2.Text != string.Empty) //注意!Text判斷為空值需用此方法
        {
            cmd.Parameters.Add("@my_e_end_date_1", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_e_end_date_2", SqlDbType.DateTime);

            DateTime dt_time_2 = DateTime.ParseExact(tb_e_end_date_2.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces); //注意!Text需先轉換成DateTime。但 因jQuery有先定義日期格式 所以沒轉也沒關係
            dt_time_2 = dt_time_2.AddDays(1); 

            cmd.Parameters["@my_e_end_date_1"].Value = tb_e_end_date_1.Text; //考核結束日(起)
            cmd.Parameters["@my_e_end_date_2"].Value = dt_time_2; //考核結束日(迄)
        }
        else if (tb_e_end_date_1.Text == string.Empty && tb_e_end_date_2.Text != string.Empty)
        {
            Response.Write("<script language='javascript'>confirm('請選擇考核結束日的開始日期!');</script>");
            return;
        }
        else if (tb_e_end_date_1.Text != string.Empty && tb_e_end_date_2.Text == string.Empty)
        {
            Response.Write("<script language='javascript'>confirm('請選擇考核結束日的結束日期!');</script>");
            return;
        }

        dr = cmd.ExecuteReader();

        gv_evaluation.DataSource = dr;
        gv_evaluation.DataBind();

        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }

    //資料綁定時，於特定欄位新增超連結
    protected void gv_Candidates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //超連結
            HyperLink hyper = new HyperLink();
            hyper.Text = e.Row.Cells[0].Text; //序號
            hyper.ForeColor = System.Drawing.Color.FromArgb(58, 143, 183);
            hyper.NavigateUrl = "/evaluation_edit.aspx?ID=" + e.Row.Cells[0].Text + "&s=e";
            e.Row.Cells[0].Controls.Add(hyper);
        }
    }

    //讀取權限資料表，抓取個別權限
    //事件呼叫：PageLoad
    private string[] DB_authority(string name)
    {
        string[] arr_auth = new string[1]; //修改：新增欄位權限

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        SqlCommand cmd = new SqlCommand("select evaluation from dbo.hr_authority where name = @my_name", Conn);
        cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_name"].Value = name; //姓名

        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            //有資料。編輯
            while (dr.Read())
            {
                for (int i = 0; i < arr_auth.Length; i++)
                {
                    arr_auth[i] = dr[i].ToString();//選擇個別權限欄位
                }
            }
        }
        cmd.Cancel();
        Conn.Close();

        return arr_auth;
    }


    //連線資料庫，讀取資料：type=dr - 讀DataReader、type=gv - 綁定Gridview
    //事件呼叫：PageLoad
    private void DB_search(string sql_str,string type,string control,[Optional]string condition)
    {
        DropDownList ddl = null;
        GridView gv = null;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = new SqlCommand(sql_str, Conn); //職稱
        if (control == "ddl_e_opening")
        {
            ddl = ddl_e_opening;
        }
        else if (control == "ddl_e_dept")
        {
            ddl = ddl_e_dept;
        }
        else if (control == "gv_evaluation")
        {
            gv = gv_evaluation;
            cmd.Parameters.Add("@my_e_dept", SqlDbType.VarChar, 100);
            cmd.Parameters["@my_e_dept"].Value = condition; //部門
        }

        SqlDataReader dr = cmd.ExecuteReader();

        if (type == "dr")
        {
            if (dr.HasRows)
            {
            //有資料。編輯
                while (dr.Read())
                {
                ddl.Items.Add(dr[0].ToString());
                }
            }
        }
        else if (type == "gv")
        {
            gv.DataSource = dr;
            gv.DataBind();
        }        

        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }

    }
}