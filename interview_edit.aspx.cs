using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

public partial class interview_edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        btn_delete.Attributes.Add("onclick ", "return confirm( '確定要刪除嗎?');"); //確定按下「刪除」按鈕 跳出提示訊息：確定即刪除；取消即返回

        string[] str_s = Session["OK"].ToString().Split('-'); //session為顯示名稱。需分割。

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        SqlDataReader dr = null;

        if (!IsPostBack)//第一次執行本程式
        {
            //連結資料庫 讀取資料(start)------------------------------------------------------------------------------------------------------------------------------------------------

            Conn.Open();
            SqlCommand searchclasscmd = new SqlCommand(
                 "select c_create_time, c_id, c_opening, c_name, c_phone, c_resume_no, c_resume_source, " +
                 "c_state, c_remark, c_modify_time, c_modify_user, i_time, i_state, i_remark, r_time, r_state, r_remark, r_empno from dbo.HR_interview where c_id = @my_c_id", Conn);

            searchclasscmd.Parameters.Add("@my_c_id", SqlDbType.VarChar, 10);
            searchclasscmd.Parameters["@my_c_id"].Value = Request.QueryString["id"];


            dr = searchclasscmd.ExecuteReader();

            if (dr.HasRows)
            {
                //有資料。編輯
                while (dr.Read())
                {
                    lb_c_create_time.Text = dr[0].ToString();
                    lb_c_id.Text = dr[1].ToString();
                    ddl_c_opening.SelectedValue = dr[2].ToString();
                    tb_c_name.Text = dr[3].ToString();
                    tb_c_phone.Text = dr[4].ToString().Trim();
                    tb_c_resume_no.Text = dr[5].ToString().Trim();
                    rbl_c_resume_source.SelectedValue = dr[6].ToString();
                    rbl_c_state.SelectedValue = dr[7].ToString();
                    tb_c_remark.Text = dr[8].ToString().Trim();
                    lb_c_modify_time.Text = dr[9].ToString();
                    tb_c_modify_user.Text = str_s[1];
                    tb_i_time.Text = dr[11].ToString().Trim();
                    rbl_i_state.SelectedValue = dr[12].ToString();
                    tb_i_remark.Text = dr[13].ToString().Trim();
                    tb_r_time.Text = dr[14].ToString().Trim();
                    rbl_r_state.SelectedValue = dr[15].ToString();
                    tb_r_remark.Text = dr[16].ToString().Trim();
                    tb_r_empno.Text = dr[17].ToString().Trim();
                }
            }
            else
            {
                //沒資料。新增
                lb_c_create_time.Text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss"); //建立時間
                lb_c_id.Text = Request.QueryString["id"]; //序號
                lb_c_modify_time.Text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss"); //修改時間
                tb_c_modify_user.Text = str_s[1];
            }

            if (dr != null)
            {
                searchclasscmd.Cancel();
                dr.Close();
            }
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            //連結資料庫 讀取資料(end)--------------------------------------------------------------------------------------------------------------------------------------------------
        }

        //如果為編輯模式，顯示新增、刪除按鈕
        if (Request.QueryString["s"] == "e")
        {
            btn_insert.Visible = true;
            btn_delete.Visible = true;
        }

        ShowState(); //判斷現在為哪一狀態顯示那些Table
    }

    //編輯&新增：點擊「儲存」依URL的s判斷是e=編輯、i=新增模式，分別編輯或新增資料
    protected void btn_store_Click(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_cmd = null;

        if (Request.QueryString["s"] == "i") //新增
        {
            sql_cmd = "insert into HR_interview([c_create_time],[c_id],[c_opening],[c_name],[c_phone],[c_resume_no],[c_resume_source],[c_state],[c_remark],[a_state],[a_progress],[c_modify_time],[c_modify_user]) " +
            "values(GETDATE(), @my_c_id, @my_c_opening, @my_c_name, @my_c_phone, @my_c_resume_no, @my_c_resume_source, @my_c_state, @my_c_remark,@my_a_state, @my_a_progress, GETDATE(), @my_c_modify_user)";
        }
        else if (Request.QueryString["s"] == "e") //編輯
        {
            sql_cmd = "update [HR_interview] set [c_opening] = @my_c_opening, [c_name] = @my_c_name," +
          "[c_phone] = @my_c_phone, [c_resume_no] = @my_c_resume_no, [c_resume_source] = @my_c_resume_source, [c_state] = @my_c_state," +
          "[c_remark] = @my_c_remark,[a_state] = @my_a_state, [a_progress] = @my_a_progress, [c_modify_time] = GETDATE(), [c_modify_user] = @my_c_modify_user," +
          "[i_time] = @my_i_time, [i_state] = @my_i_state, [i_remark] = @my_i_remark," +
          "[r_time] = @my_r_time, [r_state] = @my_r_state, [r_empno] = @my_r_empno, [r_remark] = @my_r_remark where [c_id] = @my_c_id";
        }
        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_c_id", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_id"].Value = Request.QueryString["id"]; //序號

        cmd.Parameters.Add("@my_c_opening", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_opening"].Value = ddl_c_opening.SelectedValue; //職缺

        cmd.Parameters.Add("@my_c_name", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_c_name"].Value = tb_c_name.Text.Trim(); //姓名

        cmd.Parameters.Add("@my_c_phone", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_phone"].Value = tb_c_phone.Text.Trim(); //電話

        cmd.Parameters.Add("@my_c_resume_no", SqlDbType.VarChar, 20);//履歷代碼
        //200805：修改內容為空時填入空白。BY PEGGY
        if (tb_c_resume_no.Text != null)
        {
            //200908：資料庫設定c_resume_no為唯一索引，在TextChange判斷是否重複，出現訊息，如顯示訊息後未修改儲存，會存不進去資料庫。BY PEGGY
            cmd.Parameters["@my_c_resume_no"].Value = tb_c_resume_no.Text.Trim();

        }
        else
        {
            cmd.Parameters["@my_c_resume_no"].Value = " ";
        }

        cmd.Parameters.Add("@my_c_resume_source", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_c_resume_source"].Value = rbl_c_resume_source.SelectedValue; //履歷來源

        cmd.Parameters.Add("@my_c_state", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_c_state"].Value = rbl_c_state.SelectedValue; //邀約狀態

        cmd.Parameters.Add("@my_c_remark", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_c_remark"].Value = tb_c_remark.Text.Trim(); //備註

        cmd.Parameters.Add("@my_c_modify_user", SqlDbType.VarChar, 10);
        string[] str_s = Session["OK"].ToString().Split('-');
        cmd.Parameters["@my_c_modify_user"].Value = str_s[1]; //修改人 

        //判斷狀態
        cmd.Parameters.Add("@my_a_state", SqlDbType.VarChar, 10);

        if (rbl_r_state.SelectedValue == "報到成功" || rbl_r_state.SelectedValue == "報到失敗")
        {
            cmd.Parameters["@my_a_state"].Value = rbl_r_state.SelectedValue;
        }
        else if (rbl_i_state.SelectedValue == "錄取成功" || rbl_i_state.SelectedValue == "錄取失敗")
        {
            cmd.Parameters["@my_a_state"].Value = rbl_i_state.SelectedValue;
        }
        else if (rbl_c_state.SelectedValue == "成功" || rbl_c_state.SelectedValue == "失敗" || rbl_c_state.SelectedValue == "無效")
        {
            cmd.Parameters["@my_a_state"].Value = "邀約" + rbl_c_state.SelectedValue;
        }

        //判斷是否進行中或結案
        cmd.Parameters.Add("@my_a_progress", SqlDbType.VarChar, 10);

        if (rbl_c_state.SelectedValue == "失敗" || rbl_c_state.SelectedValue == "無效" || rbl_i_state.SelectedValue == "錄取失敗" || rbl_r_state.SelectedValue == "報到失敗" || rbl_r_state.SelectedValue == "報到成功") //200803：新增「報到成功」為結案。BY PEGGY
        {
            ;
            cmd.Parameters["@my_a_progress"].Value = "結案";
        }
        else
        {
            cmd.Parameters["@my_a_progress"].Value = "進行中";
        }

        //判斷面試或報到階段填入的值
        if (Request.QueryString["s"] == "e")
        {
            cmd.Parameters.Add("@my_i_time", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_r_time", SqlDbType.DateTime);

            if (tb_i_time.Text != string.Empty) //注意!Text判斷為空值需用此方法
            {
                tb_i_time.Text = Convert.ToDateTime(tb_i_time.Text).ToString("yyyy-MM-dd  HH:mm:ss"); //注意!Text需先轉換成DateTime //格式中hh是12小時制；HH是24小時制
                cmd.Parameters["@my_i_time"].Value = tb_i_time.Text; //面試時間
            }
            else
            {
                cmd.Parameters["@my_i_time"].Value = DBNull.Value;
            }

            if (tb_r_time.Text != string.Empty)
            {
                //DateTime dt_rtime = DateTime.ParseExact(tb_r_time.Text, "yyyy-MM-dd hh:mm:ss", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                //tb_i_time.Text = Convert.ToDateTime(tb_i_time.Text).ToString("yyyy-MM-dd  hh:mm:ss"); //注意!Text需先轉換成DateTime
                cmd.Parameters["@my_r_time"].Value = tb_r_time.Text; //報到時間
            }
            else
            {
                cmd.Parameters["@my_r_time"].Value = DBNull.Value;
            }

            //200805：修改內容為空時填入空白。BY PEGGY

            cmd.Parameters.Add("@my_i_state", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@my_r_state", SqlDbType.VarChar, 10);
            if (rbl_i_state.SelectedValue != null || rbl_r_state.SelectedValue != null)
            {
                cmd.Parameters["@my_i_state"].Value = rbl_i_state.SelectedValue; //面試狀態
                cmd.Parameters["@my_r_state"].Value = rbl_r_state.SelectedValue; //報到狀態
            }
            else
            {
                cmd.Parameters["@my_i_state"].Value = " ";
                cmd.Parameters["@my_r_state"].Value = " ";
            }


            cmd.Parameters.Add("@my_i_remark", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@my_r_remark", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@my_r_empno", SqlDbType.VarChar, 10);
            if (tb_i_remark.Text != null || tb_r_remark.Text != null || tb_r_empno.Text != null)
            {
                cmd.Parameters["@my_i_remark"].Value = tb_i_remark.Text.Trim(); //面試備註
                cmd.Parameters["@my_r_remark"].Value = tb_r_remark.Text.Trim(); //報到備註 
                cmd.Parameters["@my_r_empno"].Value = tb_r_empno.Text.Trim(); //工號
            }
            else
            {
                cmd.Parameters["@my_i_remark"].Value = " ";
                cmd.Parameters["@my_r_remark"].Value = " ";
                cmd.Parameters["@my_r_empno"].Value = " ";
            }

            //200915：判斷工號是否重複或不存在，如正確，新增工號進考核資料表(evaluation)。BY PEGGY--------------------------------------------------------
            if (rbl_r_state.SelectedValue == "報到成功" && tb_r_empno.Text != "")
            {
                //200915：查詢是否EHRS存在在職員工工號，預防新增錯誤
                SqlCommand hrs_empno_cmd = new SqlCommand("select count(pa51002) from[EHRS].[hrs_mis].[dbo].[WPA51] where  pa51002 = @my_e_empno and PA51011 = 1", Conn);
                hrs_empno_cmd.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 10);
                hrs_empno_cmd.Parameters["@my_e_empno"].Value = tb_r_empno.Text; //工號

                int hrs_empno = (int)hrs_empno_cmd.ExecuteScalar(); //查詢HRS工號筆數
                hrs_empno_cmd.Cancel();


                //200915：查詢是否interview是否有重複工號，預防後續考核新增重複資料
                SqlCommand r_empno_cmd = new SqlCommand("select count(r_empno) from HR_interview where r_empno = @my_r_empno", Conn);
                r_empno_cmd.Parameters.Add("@my_r_empno", SqlDbType.VarChar, 10);
                r_empno_cmd.Parameters["@my_r_empno"].Value = tb_r_empno.Text; //工號

                int r_empno = (int)r_empno_cmd.ExecuteScalar(); //查詢工號筆數
                r_empno_cmd.Cancel();

                if (r_empno > 0)
                {
                    Response.Write("<script language='javascript'>confirm('修改錯誤! 存在重複工號，請確認');</script>");
                    return;
                }
                else if (hrs_empno < 1)
                {
                    Response.Write("<script language='javascript'>confirm('考核資料新增錯誤! 查無此工號，請確認!');</script>");
                    return;
                }
                else
                {
                    insert_r_empno(); //新增工號至考核資料(evaluation)
                }
            }
            else if (rbl_r_state.SelectedValue != "報到成功" && tb_r_empno.Text != "")
            {
                Response.Write("<script language='javascript'>confirm('考核資料新增錯誤! 請確認狀態為「報到成功」!');</script>");
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------

        //僅邀約階段 限制選項為必要選擇
        if (ddl_c_opening.SelectedValue == "請選擇")
        {
            Response.Write("<script language='javascript'>confirm('請選擇職缺');</script>");
        }
        else if (rbl_c_resume_source.SelectedValue == "")
        {
            Response.Write("<script language='javascript'>confirm('請選擇履歷來源');</script>");
            cmd.Parameters.Add("@my_a_state", SqlDbType.VarChar, 10);
        }
        else if (rbl_c_state.SelectedValue == "")
        {
            Response.Write("<script language='javascript'>confirm('請選擇邀約狀態');</script>");
        }
        else
        {
            cmd.ExecuteNonQuery();
            cmd.Cancel(); //----關閉DataReader之前，一定要先「取消」SqlCommand 

            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }

            if (Request.QueryString["s"] == "i") //新增
            {
                Response.Write("<script language='javascript'>alert('新增完成! 序號：" + Request.QueryString["id"] + "'); location.href='../interview_edit.aspx?ID=" + Request.QueryString["id"] + "&s=e'; </script>"); //跳出訊息框並跳轉至另一頁面
                btn_insert.Visible = true;
                btn_delete.Visible = true;
            }
            else if (Request.QueryString["s"] == "e") //編輯
            {
                Response.Write("<script language='javascript'>alert('修改完成! 序號：" + Request.QueryString["id"] + "'); </script>");
                btn_insert.Visible = true;
                btn_delete.Visible = true;
            }
        }
        ShowState(); //判斷現在為哪一狀態顯示那些Table。需要重新讀資料庫的資料
    }

    //200908：修改時判斷履歷代碼是否為唯一值(資料庫已設c_resume_no為唯一索引)。BY PEGGY
    //P.S 如顯示訊息後未修改就儲存，會存不進去資料庫
    protected void tb_c_resume_no_TextChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand c_resume_no_cmd = new SqlCommand("SELECT count(c_id) from HR_interview where c_resume_no ='" + tb_c_resume_no.Text + "'", Conn);

        int count = (int)c_resume_no_cmd.ExecuteScalar();
        if (count > 0)
        {
            Response.Write("<script language='javascript'>confirm('資料儲存錯誤!!  請確認「履歷代碼」');</script>");
            return; //跳出Sub不再執行
        }

        c_resume_no_cmd.Cancel();
    }

    //判斷是否有撈資料 根據哪一階段顯示哪個Table
    //[事件呼叫：PageLoad、btn_store_Click]
    public void ShowState()
    {

        //判斷哪個階段(代表該階段通過) 顯示那些畫面
        if (rbl_r_state.SelectedValue == "報到成功")
        {
            tb_interview.Visible = true;
            tb_reportfor.Visible = true;
        }
        else if (rbl_r_state.SelectedValue == "報到失敗")
        {
            tb_interview.Visible = true;
            tb_reportfor.Visible = true;
        }
        else if (rbl_i_state.SelectedValue == "錄取成功")
        {
            tb_interview.Visible = true;
            tb_reportfor.Visible = true;
        }
        else if (rbl_i_state.SelectedValue == "錄取失敗")
        {
            tb_interview.Visible = true;
            tb_reportfor.Visible = false;
        }
        else if (rbl_c_state.SelectedValue == "成功")
        {
            tb_interview.Visible = true;
        }
        else if (rbl_c_state.SelectedValue == "失敗" || rbl_c_state.SelectedValue == "無效")
        {
            tb_interview.Visible = false;
            tb_reportfor.Visible = false;
        }

    }

    //刪除：點擊「刪除」刪掉一筆資料並返回query畫面
    //[在PageLoad事件 會先判斷是否有點擊按鈕 並 跳出訊息框 確認是否要刪除]
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();


        SqlCommand Deletecmd = new SqlCommand("delete from [HR_interview] where [c_id] = @my_c_id", Conn);
        Deletecmd.Parameters.Add("@my_c_id", SqlDbType.VarChar, 10);
        Deletecmd.Parameters["@my_c_id"].Value = Request.QueryString["id"];

        Deletecmd.ExecuteNonQuery();

        Deletecmd.Cancel();
        //----關閉DataReader之前，一定要先「取消」SqlCommand

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        Response.Redirect("~/interview_query.aspx");
    }


    //新增：點擊「新增」帶入預設值 
    //[預設值：導覽列點擊編輯頁面、登入畫面、query畫面點擊新增按鈕、edit畫面點擊新增按鈕皆帶入]
    protected void btn_insert_Click(object sender, EventArgs e)
    {
        string str_c_id = null;
        //設定 新增模式 預設值
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand("select MAX(c_id) from dbo.HR_interview", Conn); //取最大值
        int c_id = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();

        if (c_id.ToString().Substring(0, 2) == (DateTime.Now.ToString("yyyy")).Substring(2, 2))
        {
            str_c_id = (c_id + 1).ToString(); //當年度
        }
        else
        {
            c_id = int.Parse((DateTime.Now.ToString("yyyy")).Substring(2, 2)); //西元年後兩位數
            str_c_id = ((c_id * 1000000) + 1).ToString();
        }
        Conn.Close();
        Response.Redirect("/interview_edit.aspx?ID=" + str_c_id + "&s=i");
    }

    //新增工號資料進考核(evaluation)資料表
    //[事件呼叫：btn_store_click]
    public void insert_r_empno()
    {
        string str_e_id = null;
        DateTime dt_e_end_date;
        string[] str_s = Session["OK"].ToString().Split('-'); //session為顯示名稱。需分割。
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd_id = new SqlCommand("select MAX(e_id) from dbo.HR_evaluation", Conn); //取最大值
        int e_id = (int)cmd_id.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd_id.Cancel();

        if (e_id.ToString().Substring(0, 2) == (DateTime.Now.ToString("yyyy")).Substring(2, 2))
        {
            str_e_id = (e_id + 1).ToString(); //當年度
        }
        else
        {
            e_id = int.Parse((DateTime.Now.ToString("yyyy")).Substring(2, 2)); //西元年後兩位數
            str_e_id = ((e_id * 1000000) + 1).ToString();
        }

        string sql_cmd = null;
        sql_cmd = "INSERT INTO [dbo].[HR_evaluation]" +
               "([e_id],[e_name],[e_empno],[e_opening],[e_dept],[e_start_date],[e_end_date],[e_change],[e_modify_user],[ep_a_state]) " +
               "SELECT @my_e_id,@my_e_name,@my_e_empno,c.[paak003],REPLACE(REPLACE(b.[pa11003], '常日', ''), '輪班', '') as pa11003,@my_e_start_date, DATEADD (day,-1, DATEADD ( month,3, @my_e_start_date)),@my_e_change,@my_e_modify_user,'未考核' " +
               "FROM [EHRS].[hrs_mis].[dbo].[WPA51] AS a INNER JOIN " +
               "[EHRS].[hrs_mis].[dbo].[WPA11] AS b ON a.[PA51014] = b.[PA11002] INNER JOIN " +
               "[EHRS].[hrs_mis].[dbo].[WPAAK] AS c ON a.[pa51135] = c.[paak002] " +
               "where a.[PA51002] = @my_e_empno and a.[PA51011] = 1";

        SqlCommand cmd_insert_e = new SqlCommand(sql_cmd, Conn);

        cmd_insert_e.Parameters.Add("@my_e_id", SqlDbType.VarChar, 20);
        cmd_insert_e.Parameters["@my_e_id"].Value = str_e_id; //序號

        cmd_insert_e.Parameters.Add("@my_e_name", SqlDbType.VarChar, 10);
        cmd_insert_e.Parameters["@my_e_name"].Value = tb_c_name.Text; //姓名

        cmd_insert_e.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 10);
        cmd_insert_e.Parameters["@my_e_empno"].Value = tb_r_empno.Text; //工號

        cmd_insert_e.Parameters.Add("@my_e_start_date", SqlDbType.VarChar, 10);
        cmd_insert_e.Parameters["@my_e_start_date"].Value = tb_r_time.Text; //考核開始日
        
        cmd_insert_e.Parameters.Add("@my_e_change", SqlDbType.VarChar, 10);
        cmd_insert_e.Parameters["@my_e_change"].Value = "新人"; //項目

        cmd_insert_e.Parameters.Add("@my_e_modify_user", SqlDbType.VarChar, 10);
        cmd_insert_e.Parameters["@my_e_modify_user"].Value = str_s[1]; //修改人名稱

        cmd_insert_e.ExecuteNonQuery();

        cmd_insert_e.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        Response.Write("<script language='javascript'>confirm('考核資料新增完成! 工號為：" + tb_r_empno.Text + "');</script>");
        tb_r_empno.Enabled = false; //工號欄位唯讀不可修改
    }

}