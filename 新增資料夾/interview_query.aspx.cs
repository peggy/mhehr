using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Activities.Expressions;
using System.Runtime.InteropServices; //Optional
using System.Text.RegularExpressions;
using System.Text;


public partial class interview_query : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!Page.IsPostBack)//第一次執行本程式
        {
            //連線資料庫 讀取最新時間的前10筆資料(start)-----------------------------------------------------
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
            Conn.Open();
            SqlDataReader dr = null;
            string sqlstr = "select top(10) c_id as 序號, c_name as 姓名, c_opening as 職缺, i_time as 面試時間, r_time as 報到時間, a_state as 狀態, a_progress as 進度,c_modify_user as 修改人, c_modify_time as 修改時間 from HR_interview order by c_modify_time desc"; //200804：新增「修改人」查詢欄位。BY PEGGY
            SqlCommand cmd = new SqlCommand(sqlstr, Conn);
            dr = cmd.ExecuteReader();

            gv_Candidates.DataSource = dr;
            gv_Candidates.DataBind();

            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            //連線資料庫 讀取最新時間的前10筆資料(end)---------------------------------------------------------
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
        sqlsb.Append("select c_id as 序號, c_name as 姓名, c_opening as 職缺, i_time as 面試時間, r_time as 報到時間, a_state as 狀態, a_progress as 進度,c_modify_user as 修改人, c_modify_time as 修改時間 from HR_interview " + //200804：新增「修改人」查詢欄位。BY PEGGY
            "where a_state like '%' + @my_a_state + '%' " +
            "and c_name like '%' + @my_c_name + '%' " +
            "and c_resume_no like '%' + @my_c_resume_no + '%' "+ //200804_家瑋：新增「履歷代碼」條件。BY PEGGY
            "and c_opening like '%' + @my_c_opening + '%' ");



        if (rbl_a_progress.SelectedValue == "進行中" || rbl_a_progress.SelectedValue == "結案")
        {
            sqlsb.Append(" and a_progress like '%' + @my_a_progress + '%'");
        }

        if (tb_c_modify_time_1.Text != string.Empty && tb_c_modify_time_1.Text != string.Empty) //注意!Text判斷為空值需用此方法
        {
            sqlsb.Append(" and c_modify_time between  @my_c_modify_time_1  and  @my_c_modify_time_2");//注意! 查詢日期 不須加單引號 只需參數即可
        }

        if (tb_i_time_1.Text != string.Empty && tb_i_time_2.Text != string.Empty)
        {
            sqlsb.Append(" and i_time between @my_i_time_1 and @my_i_time_2");
        }

        if (tb_r_time_1.Text != string.Empty && tb_r_time_2.Text != string.Empty)
        {
            sqlsb.Append(" and r_time between @my_r_time_1 and @my_r_time_2 ");
        }

        sqlsb.Append(" order by c_modify_time desc");

        SqlCommand cmd = new SqlCommand(sqlsb.ToString(), Conn);

        cmd.Parameters.Add("@my_a_state", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_a_state"].Value = ddl_a_state.SelectedValue; //階段(結束狀態)

        cmd.Parameters.Add("@my_c_opening", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_opening"].Value = ddl_c_opening.SelectedValue; //職缺

        cmd.Parameters.Add("@my_c_name", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_name"].Value = tb_c_name.Text; //應徵者姓名

        cmd.Parameters.Add("@my_c_resume_no", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_resume_no"].Value = tb_c_resume_no.Text; //履歷代碼

        if (rbl_a_progress.SelectedValue == "進行中" || rbl_a_progress.SelectedValue == "結案")
        {
            cmd.Parameters.Add("@my_a_progress", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_a_progress"].Value = rbl_a_progress.SelectedValue;
        }

        if (tb_c_modify_time_1.Text != string.Empty && tb_c_modify_time_2.Text != string.Empty) //注意!Text判斷為空值需用此方法
        {
            cmd.Parameters.Add("@my_c_modify_time_1", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_c_modify_time_2", SqlDbType.DateTime);

            DateTime dt_time_2 = DateTime.ParseExact(tb_c_modify_time_2.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces); //注意!Text需先轉換成DateTime。但 因jQuery有先定義日期格式 所以沒轉也沒關係
            dt_time_2 = dt_time_2.AddDays(1); //因查詢日期為當天的00:00:00 想在查詢期間含當天 就用加一天的方式
                                              //tb_c_modify_time_1.Text = Convert.ToDateTime(tb_c_modify_time_1.Text).ToString("yyyy-MM-dd"); //P.S 另一種轉換成日期的方式

            cmd.Parameters["@my_c_modify_time_1"].Value = tb_c_modify_time_1.Text; //修改時間(起) 
            cmd.Parameters["@my_c_modify_time_2"].Value = dt_time_2; //修改時間(迄)
        }

        if (tb_i_time_1.Text != string.Empty && tb_i_time_2.Text != string.Empty)
        {
            cmd.Parameters.Add("@my_i_time_1", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_i_time_2", SqlDbType.DateTime);

            DateTime dt_itime_2 = DateTime.ParseExact(tb_i_time_2.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            dt_itime_2 = dt_itime_2.AddDays(1);

            cmd.Parameters["@my_i_time_1"].Value = tb_i_time_1.Text; //面試時間(起)
            cmd.Parameters["@my_i_time_2"].Value = dt_itime_2; //面試時間(迄)
        }

        if (tb_r_time_1.Text != string.Empty && tb_r_time_2.Text != string.Empty)
        {
            cmd.Parameters.Add("@my_r_time_1", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_r_time_2", SqlDbType.DateTime);

            DateTime dt_rtime_2 = DateTime.ParseExact(tb_r_time_2.Text, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            dt_rtime_2 = dt_rtime_2.AddDays(1);

            cmd.Parameters["@my_r_time_1"].Value = tb_r_time_1.Text; //報到時間(起)
            cmd.Parameters["@my_r_time_2"].Value = dt_rtime_2; //報到時間(迄)
        }

        dr = cmd.ExecuteReader();

        gv_Candidates.DataSource = dr;
        gv_Candidates.DataBind();

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
            hyper.NavigateUrl = "/interview_edit.aspx?ID=" + e.Row.Cells[0].Text + "&s=e";
            e.Row.Cells[0].Controls.Add(hyper);
        }
    }

}