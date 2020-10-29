using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using jmail;
using System.Activities.Expressions;
using System.Runtime.InteropServices; //Optional
using System.Text.RegularExpressions;
using System.Text;
using System.IdentityModel.Claims;
using System.Runtime.CompilerServices;

using System.IO;

public partial class evaluation_edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        btn_delete.Attributes.Add("onclick ", "return confirm( '確定要刪除嗎?');"); //確定按下「刪除」按鈕 跳出提示訊息：確定即刪除；取消即返回
        btn_submit.Attributes.Add("onclick ", "return confirm( '提交後無法修改資料，請問確定要提交嗎?');"); //確認「提交」按鈕
        btn_hr_check.Attributes.Add("onclick ", "return confirm( '確認後無法修改資料，請問確定要提交嗎?');"); //確認「人事確認」按鈕

        string[] str_s = Session["OK"].ToString().Split('-');

        //權限(s)-----------------------------------------------------------------------------------------------------
        string[] arr_auth = DB_authority(str_s[1]);
        if (arr_auth[0] == "99") //管理者
        {
            btn_insert.Visible = true;
            btn_delete.Visible = true;
            btn_store.Visible = true;
            lb_e_create_time.Visible = true;
            tb_e_create_time.Visible = true;
            lb_e_modify_time.Visible = true;
            tb_e_modify_time.Visible = true;
            lb_e_modify_user.Visible = true;
            tb_e_modify_user.Visible = true;
            tb_ep_hr_remark.Visible = true;
            btn_submit.Visible = true;
            btn_hr_check.Visible = true;
        }
        else if (arr_auth[0] == "10" || arr_auth[0] == "11") //主要負責人：HR。10：課長、11：HR
        {
            btn_insert.Visible = true;
            btn_delete.Visible = true;
            btn_store.Visible = true;
            lb_e_create_time.Visible = true;
            tb_e_create_time.Visible = true;
            lb_e_modify_time.Visible = true;
            tb_e_modify_time.Visible = true;
            lb_e_modify_user.Visible = true;
            tb_e_modify_user.Visible = true;
            tb_ep_hr_remark.Visible = true;
            btn_hr_check.Visible = true;
            if (arr_auth[0] == "10") //201028_雅婷：身兼jackie主管考核。BY PEGGY
            {
                btn_submit.Visible = true;
            }
        }    
        else if (arr_auth[0] == "20" || arr_auth[0] == "21") //使用者：主管
        {
            btn_submit.Visible = true;
        }
        //權限(e)-----------------------------------------------------------------------------------------------------

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        SqlDataReader dr = null;

        if (!IsPostBack)//第一次執行本程式
        {
            ////連結資料庫 讀取資料(s)------------------------------------------------------------------------------------------------------------------------------------------------

            Conn.Open();
            SqlCommand search_cmd = new SqlCommand(
                 "select e_id, e_name, e_empno, e_opening, e_dept, Convert(varchar(10),e_start_date,23) , Convert(varchar(10), e_end_date,23) , e_change, e_create_time, e_modify_time, e_modify_user, ep_a_state, " +
                 "ep_ability, ep_attitude, ep_response, ep_relationship, ep_attendance, ep_comprehensive, ep_level, ep_pass, ep_res_user, ep_res_time, ep_res_check_user, ep_res_check_time, ep_hr_remark " +
                 "from dbo.HR_evaluation where e_id = @my_e_id", Conn);

            search_cmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 10);
            search_cmd.Parameters["@my_e_id"].Value = Request.QueryString["id"];

            dr = search_cmd.ExecuteReader();

            if (dr.HasRows)
            {
                //有資料。編輯
                while (dr.Read())
                {
                    tb_e_id.Text = dr[0].ToString();
                    tb_e_name.Text = dr[1].ToString();
                    tb_e_empno.Text = dr[2].ToString();
                    tb_e_opening.Text = dr[3].ToString();
                    tb_e_dept.Text = dr[4].ToString();
                    tb_e_start_date.Text = dr[5].ToString();
                    tb_e_end_date.Text = dr[6].ToString();

                    if (dr[7].ToString() == "新人")
                    {
                        ddl_e_change.Items[1].Text = dr[7].ToString();
                        ddl_e_change.SelectedIndex = 1;
                        ddl_e_change.Enabled = false;
                    }
                    else
                    {
                        ddl_e_change.SelectedValue = dr[7].ToString();
                    }
                    lb_title.Text = dr[7].ToString() + "-" + lb_title.Text;
                    
                    tb_e_create_time.Text = dr[8].ToString();
                    tb_e_modify_time.Text = dr[9].ToString();
                    tb_e_modify_user.Text = dr[10].ToString();

                    if (dr[11].ToString() == "結案" || dr[11].ToString() == "已考核")
                    {
                        set_radio_button(dr[12].ToString(), rb_ep_ability_1, rb_ep_ability_2, rb_ep_ability_3, rb_ep_ability_4, rb_ep_ability_5);
                        set_radio_button(dr[13].ToString(), rb_ep_attitude_1,rb_ep_attitude_2,rb_ep_attitude_3,rb_ep_attitude_4,rb_ep_attitude_5);
                        set_radio_button(dr[14].ToString(), rb_ep_response_1, rb_ep_response_2, rb_ep_response_3, rb_ep_response_4, rb_ep_response_5);
                        set_radio_button(dr[15].ToString(), rb_ep_relationship_1, rb_ep_relationship_2, rb_ep_relationship_3, rb_ep_relationship_4, rb_ep_relationship_5);
                        set_radio_button(dr[16].ToString(), rb_ep_attendance_1, rb_ep_attendance_2, rb_ep_attendance_3, rb_ep_attendance_4, rb_ep_attendance_5);
                        tb_ep_Comprehensive.Text = dr[17].ToString();
                        set_radio_button(dr[18].ToString(), rb_ep_level_1, rb_ep_level_2, rb_ep_level_3, rb_ep_level_4, rb_ep_level_5);
                        set_radio_button(dr[19].ToString(), rb_ep_pass_1, rb_ep_pass_2, rb_ep_pass_3);
                        tb_ep_res_user.Text = dr[20].ToString();
                        tb_ep_res_time.Text = dr[21].ToString();
                        btn_submit.Enabled = false;

                        if (dr[11].ToString() == "結案")
                        {
                            tb_ep_res_check_user.Text = dr[22].ToString();
                            tb_ep_res_check_time.Text = dr[23].ToString();
                            tb_ep_hr_remark.Text = dr[24].ToString();
                            btn_hr_check.Enabled = false;
                        }
                    }
                   
                }
            }
            else
            {
                tb_e_create_time.Text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss"); //建立時間
                tb_e_id.Text = Request.QueryString["id"]; //序號
                tb_e_modify_time.Text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss"); //修改時間
                tb_e_modify_user.Text = str_s[1];

            }


            if (dr != null)
            {
                search_cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            ////連結資料庫 讀取資料(e)--------------------------------------------------------------------------------------------------------------------------------------------------
       
        }

        //工號不為空，取得HRS單位主管(s)-------------------------------------------------------------------------------
        if (tb_e_empno.Text != "")
        {
            Conn.Open();
            SqlCommand search_cmd = new SqlCommand(
                 "select b.[pa51004] from[EHRS].[hrs_mis].[dbo].[WPA51] a left join [EHRS].[hrs_mis].[dbo].[WPA51] b " +
                 "on a.[pa51012] = b.[pa51002] " +
                 "where a.[pa51002] = @my_e_empno", Conn);

            search_cmd.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 10);
            search_cmd.Parameters["@my_e_empno"].Value = tb_e_empno.Text;

            dr = search_cmd.ExecuteReader();

            if (dr.HasRows)
            {
                //有資料。編輯
                while (dr.Read())
                {
                    tb_manager.Text = dr[0].ToString();
                }
            }
            if (dr != null)
            {
                search_cmd.Cancel();
                dr.Close();
            }
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
        }
        //工號不為空，取得HRS單位主管(e)-------------------------------------------------------------------------------

        if (Request.QueryString["s"] == "e")
        {
            tb_e_empno.ReadOnly = true;
            tb_e_start_date.ReadOnly = true;
            ddl_e_change.Enabled = false;
        }
    }

    //新增：點擊「新增」帶入預設值 
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    protected void btn_insert_Click(object sender, EventArgs e)
    {
        string str_e_id = null;
        //設定 新增模式 預設值
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand("select MAX(e_id) from dbo.HR_evaluation", Conn); //取最大值
        int e_id = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();

        if (e_id.ToString().Substring(0, 2) == (DateTime.Now.ToString("yyyy")).Substring(2, 2))
        {
            str_e_id = (e_id + 1).ToString(); //當年度
        }
        else
        {
            e_id = int.Parse((DateTime.Now.ToString("yyyy")).Substring(2, 2)); //西元年後兩位數
            str_e_id = ((e_id * 1000000) + 1).ToString();
        }

        Response.Redirect("/evaluation_edit.aspx?ID=" + str_e_id + "&s=i");
    }

    //編輯&新增：點擊「儲存」依URL的s判斷是e=編輯、i=新增模式，分別編輯或新增資料
    protected void btn_store_Click(object sender, EventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string sql_cmd = null;

        if (Request.QueryString["s"] == "i") //新增
        {
            sql_cmd = "INSERT INTO [dbo].[HR_evaluation]" +
               "([e_id],[e_name],[e_empno],[e_opening],[e_dept],[e_start_date],[e_end_date],[e_change],[e_create_time],[e_modify_time],[e_modify_user],[ep_a_state]) " +
               "Values (@my_e_id,@my_e_name,@my_e_empno,@my_e_opening,@my_e_dept,@my_e_start_date,@my_e_end_date,@my_e_change,GETDATE(),GETDATE(),@my_e_modify_user,'未考核') ";
        }
        else if (Request.QueryString["s"] == "e") //編輯
        {
            sql_cmd = "update [dbo].[HR_evaluation] set [e_end_date] = @my_e_end_date, [e_modify_time] = GETDATE(), [e_modify_user] = @my_e_modify_user where [e_id] = @my_e_id";
        }

        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_id"].Value = tb_e_id.Text; //序號

        cmd.Parameters.Add("@my_e_end_date", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_end_date"].Value = tb_e_end_date.Text; //考核結束日

        if (Request.QueryString["s"] == "i")
        {
            cmd.Parameters.Add("@my_e_name", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_e_name"].Value = tb_e_name.Text; //姓名

            cmd.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_e_empno"].Value = tb_e_empno.Text; //工號

            cmd.Parameters.Add("@my_e_opening", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_e_opening"].Value = tb_e_opening.Text; //職缺

            cmd.Parameters.Add("@my_e_dept", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_e_dept"].Value = tb_e_dept.Text; //部門

            cmd.Parameters.Add("@my_e_start_date", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_e_start_date"].Value = tb_e_start_date.Text; //考核開始日

        }
        
        if (ddl_e_change.SelectedValue == "請選擇")
        {
            Record("新增人員資料錯誤-未選擇項目");
            Response.Write("<script language='javascript'>confirm('請選擇項目');</script>");
            return;
        }
        else
        {
        cmd.Parameters.Add("@my_e_change", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_change"].Value = ddl_e_change.SelectedValue; //項目
        }
       
        cmd.Parameters.Add("@my_e_modify_user", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_e_modify_user"].Value = str_s[1]; //修改人

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }


        if (Request.QueryString["s"] == "i") //新增
        {
            Record("新增人員資料。序號：" + tb_e_id.Text);
            Response.Write("<script language='javascript'>alert('新增完成! 序號：" + Request.QueryString["id"] + "'); location.href='../evaluation_edit.aspx?ID=" + Request.QueryString["id"] + "&s=e'; </script>"); //跳出訊息框並跳轉至另一頁面  
        }
        else if (Request.QueryString["s"] == "e") //編輯
        {
            Record("修改人員資料。序號：" + tb_e_id.Text);
            Response.Write("<script language='javascript'>alert('修改完成! 序號：" + tb_e_id.Text + "'); </script>");            
        }
    }

    //刪除：點擊「刪除」刪掉一筆資料並返回query畫面
    //[在PageLoad事件 會先判斷是否有點擊按鈕 並 跳出訊息框 確認是否要刪除]
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();


        SqlCommand Deletecmd = new SqlCommand("delete from [HR_evaluation] where [e_id] = @my_e_id", Conn);
        Deletecmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 10);
        Deletecmd.Parameters["@my_e_id"].Value = Request.QueryString["id"];

        Deletecmd.ExecuteNonQuery();

        Deletecmd.Cancel();
        //----關閉DataReader之前，一定要先「取消」SqlCommand

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        Record("刪除資料。序號：" + tb_e_id.Text);
        Response.Redirect("~/evaluation_query.aspx");
    }

    //修改：點擊「提交」修改該筆考核資料
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
      
        SqlCommand cmd = new SqlCommand(
            "update [dbo].[HR_evaluation] set [ep_ability] = @my_ep_ability, [ep_attitude] = @my_ep_attitude, " +
            "[ep_response] = @my_ep_response, [ep_relationship] = @my_ep_relationship, [ep_attendance] = @my_ep_attendance, [ep_comprehensive] = @my_ep_comprehensive, " +
            "[ep_level] = @my_ep_level, [ep_pass] = @my_ep_pass, [ep_res_user] = @my_ep_res_user, [ep_res_time] = GETDATE() , [ep_a_state] = '已考核' " +
            "where [e_id] = @my_e_id"
            , Conn);
       
        cmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_e_id"].Value = Request.QueryString["id"];

        string[] str_para = new string[] { "@my_ep_ability", "@my_ep_attitude", "@my_ep_response", "@my_ep_relationship", "@my_ep_attendance", "@my_ep_level", "@my_ep_pass" };
        string[] str_ep = new string[] { "工作能力", "工作態度", "應對進退", "人際關係", "出勤狀況", "總考核", "試用期通過與否" };
        Byte tmp_com = 0; //是否包含尚可或差的選項
        int tmp_ep = str_ep.Length; //計算考核項目是否皆以填寫

        string[] rb_result = new string[7];
        rb_result[0] = get_radio_button(rb_ep_ability_1, rb_ep_ability_2, rb_ep_ability_3, rb_ep_ability_4, rb_ep_ability_5);      //工作能力
        rb_result[1] = get_radio_button(rb_ep_attitude_1, rb_ep_attitude_2, rb_ep_attitude_3, rb_ep_attitude_4, rb_ep_attitude_5); //工作態度
        rb_result[2] = get_radio_button(rb_ep_response_1, rb_ep_response_2, rb_ep_response_3, rb_ep_response_4, rb_ep_response_5); //應對進退
        rb_result[3] = get_radio_button(rb_ep_relationship_1, rb_ep_relationship_2, rb_ep_relationship_3, rb_ep_relationship_4, rb_ep_relationship_5); //人際關係
        rb_result[4] = get_radio_button(rb_ep_attendance_1, rb_ep_attendance_2, rb_ep_attendance_3, rb_ep_attendance_4, rb_ep_attendance_5); //出勤狀況
        rb_result[5] = get_radio_button(rb_ep_level_1, rb_ep_level_2, rb_ep_level_3, rb_ep_level_4, rb_ep_level_5); //總考核
        rb_result[6] = get_radio_button(rb_ep_pass_1, rb_ep_pass_2, rb_ep_pass_3); //試用期通過與否


        for (int i = 0; i < rb_result.Length; i++)
        {
            if (rb_result[i] == " ")
            {
                Record("提交錯誤-未選擇項目「" + str_ep[i] + "」。序號：" + tb_e_id.Text);
                Response.Write("<script language='javascript'>alert('請選擇「" + str_ep[i] + "」項目'); </script>");
            }
            else
            {
                tmp_ep--;

                cmd.Parameters.Add(str_para[i], SqlDbType.Decimal, 1);
                cmd.Parameters[str_para[i]].Precision = 1;
                cmd.Parameters[str_para[i]].Scale = 0;
                cmd.Parameters[str_para[i]].Value = decimal.Parse(rb_result[i]);

            }

            if (rb_result[i] == "1" || rb_result[i] == "2") //若其中1項為尚可或差
            {
                tmp_com = 1;
            }
        }

        if (tb_ep_Comprehensive.Text == "" && tmp_com == 1 && rb_ep_pass_3.Checked) //若其中1項為尚可或差但通過，綜合表現必填。
        {
            Record("提交錯誤-未輸入綜合表現。序號：" + tb_e_id.Text);
            Response.Write("<script language='javascript'>alert('請輸入「綜合表現」項目'); </script>");
            return;
        }
        else if (rb_ep_pass_1.Checked) //201023：新增因未通過，綜合表現必填。BY PEGGY
        {
            Record("提交錯誤-考核未通過需輸入綜合表現。序號：" + tb_e_id.Text);
            Response.Write("<script language='javascript'>alert('請於「綜合表現」項目輸入未通過之原因'); </script>");
            return;
        }
        else
        {
            cmd.Parameters.Add("@my_ep_comprehensive", SqlDbType.VarChar, 200); //綜合表現
            cmd.Parameters["@my_ep_comprehensive"].Value = tb_ep_Comprehensive.Text;
        }

        cmd.Parameters.Add("@my_ep_res_user", SqlDbType.VarChar, 10); //填寫人
        cmd.Parameters["@my_ep_res_user"].Value = str_s[1];

        if (tmp_ep == 0)
        {
            cmd.ExecuteNonQuery();
            send_mail();
            Record("提交完成。序號：" + tb_e_id.Text);
            Response.Write("<script language='javascript'>alert('提交完成! 序號：" + Request.QueryString["id"] + "'); location.href='../evaluation_query.aspx?ID=" + Request.QueryString["id"] + "&s=e'; </script>"); //跳出訊息框並跳轉至另一頁面
        }

            cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        
        
    }

    //修改：點擊「人事確認」修改該筆考核資料
    protected void btn_hr_check_Click(object sender, EventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand(
           "update [dbo].[HR_evaluation] set [ep_res_check_user] = @my_ep_res_check_user, [ep_res_check_time] = GETDATE(), [ep_hr_remark] = @my_ep_hr_remark, " +
           "[ep_a_state] = '結案' " +
           "where [e_id] = @my_e_id", Conn);

        cmd.Parameters.Add("@my_ep_res_check_user", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_ep_res_check_user"].Value = str_s[1];

        cmd.Parameters.Add("@my_ep_hr_remark", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_ep_hr_remark"].Value = tb_ep_hr_remark.Text;

        cmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_e_id"].Value = Request.QueryString["id"];

        cmd.ExecuteNonQuery();
    
        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        Record("人事確認完成。序號：" + tb_e_id.Text);
        Response.Redirect("~/evaluation_query.aspx");
    }

    //上一頁：：點擊按鈕
    //201019_Betty：新增上一頁按鈕。BY PEGGY
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>history.go(-2);</script>");
    }

    //輸入工號欄位帶出HRS的預設值：姓名、職稱、部門
    protected void tb_e_empno_TextChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        SqlDataReader dr = null;

        Conn.Open();

        SqlCommand empno_cmd = new SqlCommand(
             "select a.[pa51004] , REPLACE(REPLACE(b.[pa11003],'常日',''),'輪班','') as [pa11003],  c.[paak003] " +
             "from [EHRS].[hrs_mis].[dbo].[WPA51] a " +
             "inner join [EHRS].[hrs_mis].[dbo].[WPA11] b on a.[pa51014] = b.[pa11002] " +
             "inner join [EHRS].[hrs_mis].[dbo].[WPAAK] c on a.[pa51135] = c.[paak002] " +
             "where  a.[PA51011] = 1 and a.[pa51002] = @my_e_id ", Conn);

        empno_cmd.Parameters.Add("@my_e_id", SqlDbType.VarChar, 10);
        empno_cmd.Parameters["@my_e_id"].Value = tb_e_empno.Text;

        dr = empno_cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                tb_e_name.Text = dr[0].ToString();
                tb_e_dept.Text = dr[1].ToString();
                tb_e_opening.Text = dr[2].ToString();
            }
        }
        else
        {
            Response.Write("<script language='javascript'>confirm('查無此工號，請確認!');</script>");
        }

        if (dr != null)
        {
            empno_cmd.Cancel();
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }

    //輸入考核開始日，系統自動計算並帶出考核結束日
    protected void tb_e_start_date_TextChanged(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        SqlDataReader dr = null;

        Conn.Open();
        //200915：查詢是否evaluation是否存在相同考核開始日的工號，預防重複新增相同資料
        SqlCommand search_e_empno_cmd = new SqlCommand("select count(e_empno) from[dbo].[HR_evaluation] where e_empno = @my_e_empno and e_start_date = @my_e_start_date", Conn);
        search_e_empno_cmd.Parameters.Add("@my_e_empno", SqlDbType.VarChar, 10);
        search_e_empno_cmd.Parameters["@my_e_empno"].Value = tb_e_empno.Text; //工號

        search_e_empno_cmd.Parameters.Add("@my_e_start_date", SqlDbType.VarChar, 10);
        search_e_empno_cmd.Parameters["@my_e_start_date"].Value = tb_e_start_date.Text; //考核開始日

        int e_empno = (int)search_e_empno_cmd.ExecuteScalar(); //查詢HRS工號筆數
        search_e_empno_cmd.Cancel();

        if (e_empno > 0)
        {
            Record("新增錯誤-此工號已有考核資料。工號：" + tb_e_empno.Text);
            Response.Write("<script language='javascript'>confirm('此工號已有考核資料，請確認!');</script>");
            tb_e_start_date.Text = "";
            return;
        }
        Conn.Close();


        if (tb_e_start_date.Text != "")
        {
            tb_e_end_date.Text = DateTime.Parse(tb_e_start_date.Text).AddMonths(3).AddDays(-1).ToString("yyyy-MM-dd");
        }
    }

    //讀取權限資料表，抓取個別權限
    private string[] DB_authority(string name)
    {
        string[] arr_auth = new string[1]; //修改：新增欄位權限

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        SqlCommand cmd = new SqlCommand("select evaluation from dbo.hr_authority where name = @my_name", Conn);
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
                    arr_auth[i] = dr[i].ToString();//選擇個別權限欄位
                }
            }
        }
        cmd.Cancel();
        Conn.Close();

        return arr_auth;
    }

    //jmail寄信
    //事件呼叫：btn_submit_Click
    private void send_mail()
    {
        //讀取資料表：MHE_Notice_Email，收件者(s)--------------------------------------------------------------------------------
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;
        SqlCommand cmd = new SqlCommand("select [user_mail] from [dbo].[MHE_Notice_Email] where  mhe_states = 'HR'", Conn);

        dr = cmd.ExecuteReader();
        string to_mail = null;//收件者
        if (dr.HasRows)
        {
            //有資料。編輯
            while (dr.Read())
            {
                to_mail += dr[0].ToString() + ",";
            }
        }

        cmd.Cancel();
        Conn.Close();
        //讀取資料表：MHE_Notice_Email，收件者(e)--------------------------------------------------------------------------------
        string str_ep_pass = null;
        if (rb_ep_pass_1.Checked)
        {
            str_ep_pass = "未通過";
        }
        else if (rb_ep_pass_2.Checked)
        {
            str_ep_pass = "延長六個月";
        }
        else if (rb_ep_pass_3.Checked)
        {
            str_ep_pass = "通過";
        }
        string[] arr_to_mail = to_mail.Split(',');
        Message jmail = new Message();
        jmail.From = "mheit";
        for(int i=0;i < arr_to_mail.Length -1 ; i++)
        {
            jmail.AddRecipient(arr_to_mail[i]);
        }
        jmail.MailServerUserName = "mheit";
        jmail.MailServerPassWord = "tiehm159852";
        jmail.Subject = "[MHEHR] 考核完成"; //主旨
        jmail.Body = "人員資料：\r\n" +
            "序號：" + tb_e_id.Text + "\r\n" +
            "課室：" + tb_e_dept.Text + "\r\n" +
            "工號：" + tb_e_empno.Text + "\r\n" +
            "姓名：" + tb_e_name.Text + "\r\n" +
            "考核結果：" + str_ep_pass + "\r\n\r\n" +
            "此為系統自動發出，請勿回信\r\n" +
            "系統連結：http://172.17.1.100:8081";
        jmail.Charset = "BIG-5";
        jmail.Send("mail.mhe.com.tw", false);

    }

    //取得RadioButton的值
    private string get_radio_button(params RadioButton[] radio_button_group)
    {
        string str_rb = " ";
        for (int i = 0; i < radio_button_group.Length; i++)
        {
            if (radio_button_group[i].Checked)
            {
                str_rb = radio_button_group[i].ID;
                str_rb = str_rb.Substring(str_rb.Length - 1, 1); //取ID最右邊的字元
                return str_rb;
            }
        }
        return str_rb;
    }


    //選擇RadioButtion選取
    private void set_radio_button(string rb_value, params RadioButton[] radio_button_group)
    {
        int value = Convert.ToInt32(rb_value);
        for (int i = 1;i <= radio_button_group.Length; i++)
        {
            if (i == value)
            {
                radio_button_group[value - 1].Checked = true;
            }
        }
    }

    //201015：以文字檔寫入，紀錄使用者操作。BY PEGGY
    //事件呼叫：btn_store_Click*3、btn_delete_Click、btn_submit_Click*3、btn_hr_check_Click、tb_e_start_date_TextChanged
    public void Record(string str)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        string login_name = str_s[1];
        //(務必修改這個檔案的權限，需要「寫入」的權限)
        //寫入檔案
        StreamWriter sw = new StreamWriter("E:\\hr\\file\\record_log.txt", true);
        sw.Write(login_name);
        sw.Write("---");
        sw.Write(str);
        sw.Write("---");
        sw.Write(DateTime.Now.ToString());
        sw.WriteLine();

        sw.Close();
        sw.Dispose();
    }

}


