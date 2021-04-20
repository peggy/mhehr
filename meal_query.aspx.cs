using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class meal_query : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btn_cancel_update.OnClientClick = "javascript:return confirm('確定刪除取消?')"; //確認刪除取消
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)//第一次執行本程式
        {
            tb_make_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            DBInit("init");
            record_meal_log("LOAD。訂餐查詢載入。", "meal_query");
        }
    }

    //已訂餐、未訂餐顯示畫面	
    protected void rbl_meal_state_SelectedIndexChanged(object sender, EventArgs e)	
    {	
        string[] arr_dept_yes = new string[] { "客人", "總務", "守衛室", "事務所", "生產部", "製程課", "設備課", "廠務課", "品管課", "倉管課", "物料課", "製造課" };	
        List<string> arr_dept_no = DB_dept();	
        gv_meal.DataSource = null;	
        gv_meal.DataBind();	
        gv_no_meal.DataSource = null;	
        gv_no_meal.DataBind();	
        ddl_dept.Items.Clear();	
        ddl_dept.Items.Add(new ListItem("請選擇", "%"));
        if (rbl_meal_state.SelectedIndex == 0)//已訂餐	
        {	
            yes_meal_state.Visible = true;	
            for (int i = 0; i < arr_dept_yes.Length;i++)	
            {	
                ddl_dept.Items.Add(arr_dept_yes[i]);	
            }	
        }	
        else if (rbl_meal_state.SelectedIndex == 1) //未訂餐	
        {	
            yes_meal_state.Visible = false;	
            for (int i = 0; i < arr_dept_no.Count;i++)	
            {	
                ddl_dept.Items.Add(arr_dept_no[i]);	
            }	
        }	
        else	
        {	
            yes_meal_state.Visible = false;	
        }	
    }

    //查詢
    protected void btn_query_Click(object sender, EventArgs e)
    {
        string str = null;	
        if (rbl_meal_state.SelectedIndex == 0)	
        {	
            DBInit("query");	
            str = "SELECT。查詢已訂餐。日期：" + tb_make_date.Text + "，工號：" + tb_work_no.Text + "，狀態：" + ddl_op_state.SelectedValue + "，部門：" + ddl_dept.SelectedItem.Text + "，班別：" + ddl_class.SelectedValue + "，排班：" + ddl_scheduling.Text;	
        }	
        else if (rbl_meal_state.SelectedIndex == 1)	
        {	
            DB_query_no_meal_state();	
            str = "SELECT。查詢未訂餐。日期：" + tb_make_date.Text + "，部門：" + ddl_dept.SelectedItem.Text + "，班別：" + ddl_class.SelectedValue;	
        }	
        	
        record_meal_log(str, "meal_query");
    }

    //刪除取消	
    protected void btn_cancel_update_Click(object sender, EventArgs e)	
    {	
        if (tb_work_no.Text == "99" || tb_work_no.Text == "101" || tb_work_no.Text == "102" || tb_work_no.Text == "103")	
        {	
            Response.Write("<script language='javascript'>confirm('錯誤!\\n此工號無法刪除取消');</script>");	
            record_meal_log("UPDATE。刪除取消：失敗。此工號無法刪除取消。日期：" + tb_make_date.Text + "工號：" + tb_work_no.Text, "meal_query");	
            return;	
        }	
        else if (tb_work_no.Text.Length == 6)	
        {	
            string str = null;	
            if (DB_cancel_update() == true)	
            {	
                str = "失敗";	
                Response.Write("<script language='javascript'>confirm('錯誤! 查詢取消狀態，查無此人');</script>");	
            }	
            else	
            {	
                str = "成功";	
                DBInit("query");	
                Response.Write("<script language='javascript'>confirm('刪除取消成功!');</script>");	
            }	
            record_meal_log("UPDATE。刪除取消：" + str + "。日期：" + tb_make_date.Text + "工號：" + tb_work_no.Text, "meal_query");	
        }	
        else	
        {	
            Response.Write("<script language='javascript'>confirm('錯誤!\\n工號格式錯誤');</script>");	
            record_meal_log("UPDATE。刪除取消：失敗。工號格式錯誤。日期：" + tb_make_date.Text + "工號：" + tb_work_no.Text, "meal_query");	
            return;	
        }	
    }

    protected void gv_meal_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))
        {
            //早餐
            DropDownList ddl_2 = new DropDownList();
            ddl_2.ID = "ddl_2";
            ddl_2.EnableViewState = false;
            ddl_2.Items.Add(new ListItem("Y", "Y"));
            ddl_2.Items.Add(new ListItem("N", ""));
            e.Row.Cells[9].Controls.Add(ddl_2);
            //午餐
            DropDownList ddl_3 = new DropDownList();
            ddl_3.ID = "ddl_3";
            ddl_3.EnableViewState = false;
            ddl_3.Items.Add(new ListItem("Y", "Y"));
            ddl_3.Items.Add(new ListItem("N", ""));
            e.Row.Cells[10].Controls.Add(ddl_3);
            //晚餐
            DropDownList ddl_4 = new DropDownList();
            ddl_4.ID = "ddl_4";
            ddl_4.EnableViewState = false;
            ddl_4.Items.Add(new ListItem("Y", "Y"));
            ddl_4.Items.Add(new ListItem("N", ""));
            e.Row.Cells[11].Controls.Add(ddl_4);
        }
    }

     //編輯欄位：取消更新，綁定早午晚餐及備註欄位
     //屬性：showfooter = true
    int total_breakfast = 0, total_lunch = 0, total_dinner = 0, total_b_vegetarian = 0, total_l_vegetarian = 0, total_d_vegetarian = 0; //因RowData每一列會Bound，所以需做全域變數，才不會歸零
    protected void gv_meal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Visible = false; //sys_id
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Button btn_edit = (Button)e.Row.Cells[0].FindControl("btn_edit");
            Button d_btn = (Button)e.Row.Cells[1].FindControl("btn_update_state");

            d_btn.OnClientClick = "javascript:return confirm('確定取消?')"; //確認取消

            //狀態顯示(s)-----------------------------------------------
            e.Row.Cells[2].Visible = false; //sys_id
            if (e.Row.Cells[13].Text == "取消")
            {
                e.Row.Attributes.Add("style", "color:#8E9EAB");
                btn_edit.Enabled = false; //編輯
                d_btn.Enabled = false;    //取消
            }
            else
            {
                //異常：無修改人Length=6
                //計算加總(s)-----------------------
                if (e.Row.Cells[9].Text == "Y")
                {
                    total_breakfast++;
                }
                if (e.Row.Cells[10].Text == "Y")
                {
                    total_lunch++;
                }
                if (e.Row.Cells[11].Text == "Y")
                {
                    total_dinner++;
                }
                if (e.Row.Cells[12].Text == "Y")
                {
                    if (e.Row.Cells[9].Text == "Y")
                    {
                        total_b_vegetarian++;
                    }
                    if (e.Row.Cells[10].Text == "Y")
                    {
                        total_l_vegetarian++;
                    }
                    if (e.Row.Cells[11].Text == "Y")
                    {
                        total_d_vegetarian++;
                    }
                }
                //計算加總(e)-----------------------
            }
            //狀態顯示(e)-----------------------------------------------
            //編輯狀態(s)-----------------------------------------------
            if (gv_meal.EditIndex == e.Row.RowIndex)
            {
                //確認更新
                Button u_btn = (Button)e.Row.Cells[0].FindControl("btn_update");
                u_btn.OnClientClick = "javascript:return confirm('確定儲存?')";

                string str_tmp = null;
                int tmp = 0;
                DropDownList ddl_22 = (DropDownList)e.Row.FindControl("ddl_2");
                DropDownList ddl_33 = (DropDownList)e.Row.FindControl("ddl_3");
                DropDownList ddl_44 = (DropDownList)e.Row.FindControl("ddl_4");
                for (int i = 2; i < e.Row.Cells.Count; i++) //第0格為編輯按鈕，第1格為取消按鈕
                {
                    TextBox tb = null;
                    tb = e.Row.Cells[i].Controls[0] as TextBox;
                    tmp = 0;
                    if (tb.Text == "" && tb != null)
                    {
                        tmp = 1;
                    }
                    if (i == 9) //早餐
                    {
                        ddl_22.SelectedIndex = tmp;
                        e.Row.Cells[i].Controls.Clear();
                        e.Row.Cells[i].Controls.Add(ddl_22);
                        str_tmp += "早餐：" + tmp + "，";
                    }
                    else if (i == 10) //午餐
                    {
                        ddl_33.SelectedIndex = tmp;
                        e.Row.Cells[i].Controls.Clear();
                        e.Row.Cells[i].Controls.Add(ddl_33);
                        str_tmp += "午餐：" + tmp + "，";
                    }
                    else if (i == 11) //晚餐
                    {
                        ddl_44.SelectedIndex = tmp;
                        e.Row.Cells[i].Controls.Clear();
                        e.Row.Cells[i].Controls.Add(ddl_44);
                        str_tmp += "晚餐：" + tmp + "，";
                    }
                    else if (i == 15) //備註
                    {
                        tb.Width = Unit.Pixel(150); //寬度設定
                    }
                    else
                    {
                        tb.Enabled = false;
                        tb.Width = Unit.Pixel(90); //寬度設定
                        if (i == 5)
                        {
                            str_tmp += "姓名：" + tb.Text + "，";
                        }
                        else if (i == 6)
                        {
                            str_tmp += "日期：" + tb.Text + "，";
                        }
                    }
                }
                record_meal_log("UPDATE。載入更新訂餐。" + str_tmp, "meal_query");
            }
            //編輯狀態(e)-----------------------------------------------
        }

        //最後一列呈現加總
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Visible = false; //sys_id
            e.Row.Cells[8].Text = "小計<hr/>素食<hr/>合計<hr/>";
            e.Row.Cells[9].Text = total_breakfast.ToString() + "<hr/>" + total_b_vegetarian.ToString() + "<hr/>" + (total_breakfast- total_b_vegetarian).ToString();
            e.Row.Cells[10].Text = total_lunch.ToString() + "<hr/>" + total_l_vegetarian.ToString() + "<hr/>" + (total_lunch - total_l_vegetarian).ToString();
            e.Row.Cells[11].Text = total_dinner.ToString() + "<hr/>" + total_d_vegetarian.ToString() + "<hr/>" + (total_dinner - total_d_vegetarian).ToString();
            
        }
    }

    //編輯模式
    protected void gv_meal_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gv_meal.EditIndex = e.NewEditIndex;
        DBInit("query");
    }
    //離開編輯模式
    protected void gv_meal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gv_meal.EditIndex = -1;
        DBInit("query");
    }

    //更新資料表[PM_Opuser_Work]：抓早午晚餐及備住的欄位更新
    protected void gv_meal_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
       DropDownList my_ddl_breakfast, my_ddl_lunch, my_ddl_dinner;
        TextBox my_t_remarks, my_t_make_date, my_t_sys_id, my_t_make_us;
        my_ddl_breakfast = (DropDownList)gv_meal.Rows[e.RowIndex].Cells[9].FindControl("ddl_2"); //早餐
        my_ddl_lunch = (DropDownList)gv_meal.Rows[e.RowIndex].Cells[10].FindControl("ddl_3"); //午餐
        my_ddl_dinner = (DropDownList)gv_meal.Rows[e.RowIndex].Cells[11].FindControl("ddl_4"); //晚餐
        my_t_sys_id = (TextBox)gv_meal.Rows[e.RowIndex].Cells[2].Controls[0]; //id
        my_t_make_us = (TextBox)gv_meal.Rows[e.RowIndex].Cells[5].Controls[0]; //姓名
        my_t_make_date = (TextBox)gv_meal.Rows[e.RowIndex].Cells[6].Controls[0]; //日期
        my_t_remarks = (TextBox)gv_meal.Rows[e.RowIndex].Cells[15].Controls[0]; //備註
        string[] str_s = Session["OK"].ToString().Split('-');
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        string sql_cmd = "update [dbo].[PM_Opuser_Work] set sys_date = getdate(), breakfast = @my_breakfast, lunch = @my_lunch, dinner = @my_dinner, " +
            " op_state = 2, modify_user = @my_modify_user, remarks = @my_remarks " +
            " where sys_id = @my_sys_id";
        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);
        cmd.Parameters.Add("@my_breakfast", SqlDbType.VarChar, 1);
        cmd.Parameters["@my_breakfast"].Value = my_ddl_breakfast.SelectedValue; //早餐
        cmd.Parameters.Add("@my_lunch", SqlDbType.VarChar, 1);
        cmd.Parameters["@my_lunch"].Value = my_ddl_lunch.SelectedValue; //午餐
        cmd.Parameters.Add("@my_dinner", SqlDbType.VarChar, 1);
        cmd.Parameters["@my_dinner"].Value = my_ddl_dinner.SelectedValue; //晚餐
        cmd.Parameters.Add("@my_remarks", SqlDbType.VarChar, 50);
        cmd.Parameters["@my_remarks"].Value = my_t_remarks.Text; //備註
        cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_modify_user"].Value = str_s[1]; //修改人
        cmd.Parameters.Add("@my_sys_id", SqlDbType.Int);
        cmd.Parameters["@my_sys_id"].Value = my_t_sys_id.Text; //id
        cmd.ExecuteNonQuery();
        cmd.Cancel();
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        record_meal_log("UPDATE。更新訂餐。姓名：" + my_t_make_us.Text + "，日期：" + my_t_make_date.Text + "，" +
            "早餐：" + my_ddl_breakfast.SelectedIndex + "，午餐：" + my_ddl_lunch.SelectedIndex + "，晚餐：" + my_ddl_dinner.SelectedIndex,
            "meal_query");
	Response.Write("<script language='javascript'>confirm('更新成功!');</script>");
        gv_meal.EditIndex = -1;
        if (tb_make_date.Text != "" || tb_work_no.Text != "" || ddl_class.SelectedValue != "%" || ddl_op_state.SelectedValue != "%" || ddl_scheduling.SelectedValue != "%")
        {
            DBInit("query");
        }
        else
        {
            DBInit("init");
        }
    }

    //更新資料表[PM_Opuser_Work]：狀態改為取消
    protected void gv_meal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "update_state")
        {
            Button btn = (Button)e.CommandSource;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string[] str_s = Session["OK"].ToString().Split('-');
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
            Conn.Open();
            string sql_cmd = "update [dbo].[PM_Opuser_Work] set sys_date = getdate(), op_state = 0, modify_user = @my_modify_user " +
                " where sys_id = @my_sys_id";
            SqlCommand cmd = new SqlCommand(sql_cmd, Conn);
            cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_modify_user"].Value = str_s[1]; //修改人
            cmd.Parameters.Add("@my_sys_id", SqlDbType.Int);
            cmd.Parameters["@my_sys_id"].Value = gv_meal.Rows[row.DataItemIndex].Cells[2].Text;
            cmd.ExecuteNonQuery();
            cmd.Cancel();
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }
            record_meal_log("UPDATE。取消訂餐。姓名：" + gv_meal.Rows[row.DataItemIndex].Cells[5].Text + "，日期：" + gv_meal.Rows[row.DataItemIndex].Cells[6].Text + "。" , "meal_query");
            Response.Write("<script language='javascript'>confirm('取消成功!');</script>");
	    gv_meal.EditIndex = -1;
            if (tb_make_date.Text != "" || tb_work_no.Text != "" || ddl_class.SelectedValue != "%" || ddl_op_state.SelectedValue != "%" || ddl_scheduling.SelectedValue != "%")
            {
                DBInit("query");
            }
            else
            {
                DBInit("init");
            }
        }
    }

    //讀取資料表[v_mhe_dining_list]：綁定GridView
    //事件呼叫：Page_Load、btn_query_Click、gv_meal_RowEditing、gv_meal_RowCancelingEdit、gv_meal_RowUpdating
    public void DBInit(string type)
    {
        if (tb_make_date.Text == "") //限制每次查詢僅能查一天
        {
            Response.Write("<script language='javascript'>confirm('請選擇日期!');</script>");
            gv_meal.DataSource = null;
            gv_meal.DataBind();
            return;
        }

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlDataReader dr = null;
        SqlCommand cmd = null;
        string sql_str = null;
        sql_str = " SELECT sys_id,convert(varchar(20),sys_date,20) as 修改時間, work_no as 工號, make_us as 姓名, convert(varchar(20),make_date,23) as 工作日期, class as 班別, Scheduling as 排班, " +
                  " breakfast as 早餐, lunch as 午餐, dinner as 晚餐, Vegetarian as 素食, case when op_state = '1' and modify_user<> '' then '代訂' " +
                  " when op_state = '1' and modify_user = '' then '訂餐' when op_state = '0' then '取消' when op_state = '2' then '修改' end as '狀態', replace([modify_user],' ','') as 修改人, remarks as 備註 " +
                  " from v_mhe_dining_list " +
                  " where location like '%' + @my_dept + '%' " +
                  " and class like '%' + @my_class + '%'  and scheduling like '%' + @my_scheduling + '%' ";

        if (type == "init")
        {
            sql_str += " and make_date = '" + DateTime.Now.ToString("yyyy-MM-dd") + "' ";
        }
        else if (type == "query")
        {
            if (tb_make_date.Text != "")
            {
                sql_str += "and make_date = @my_make_date ";
            }
            if (tb_work_no.Text != "")
            {
                sql_str += " and work_no = @my_work_no ";
            }
            if (ddl_op_state.SelectedValue == "代訂")
            {
                sql_str += " and modify_user != ''";
            }
            else if (ddl_op_state.SelectedValue == "1")
            {
                sql_str += " and OP_State like '%' + @my_op_state + '%'  and modify_user = '' ";
            }
            else
            {
                sql_str += " and OP_State like '%' + @my_op_state + '%'";
            }
        }

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_class"].Value = ddl_class.SelectedValue; //班別

        cmd.Parameters.Add("@my_scheduling", SqlDbType.VarChar, 5);
        cmd.Parameters["@my_scheduling"].Value = ddl_scheduling.SelectedValue; //組別

        cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 5);
        cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門

        if (ddl_op_state.SelectedValue != "代訂")
        {
            cmd.Parameters.Add("@my_op_state", SqlDbType.VarChar, 5);
            cmd.Parameters["@my_op_state"].Value = ddl_op_state.SelectedValue; //狀態
        }

        if (tb_make_date.Text != "")
        {
            cmd.Parameters.Add("@my_make_date", SqlDbType.DateTime);
            cmd.Parameters["@my_make_date"].Value = tb_make_date.Text; //日期
        }


        if (tb_work_no.Text != "")
        {
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_work_no"].Value = tb_work_no.Text; //工號
        }

        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            gv_meal.DataSource = dr;
            gv_meal.DataBind();
        }
        else
        {
            Response.Write("<script language='javascript'>confirm('查無資料!');</script>");
            gv_meal.DataSource = null;
            gv_meal.DataBind();
        }

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
    }
    //讀取資料表[WPA51、WPA11、WPB29]：依班表查詢未訂餐人員，綁定GridView	
    public void DB_query_no_meal_state()	
    {	
        if (tb_make_date.Text == "") //限制每次查詢僅能查一天	
        {	
            Response.Write("<script language='javascript'>confirm('請選擇日期!');</script>");	
            gv_meal.DataSource = null;	
            gv_meal.DataBind();	
            return;	
        }	
        string str_class = null;	
        if (ddl_class.SelectedIndex == 1) //D	
        {	
            str_class = " '001','001-1','001-2','001-3','001-4','002','006','009','012','014','015','020','022','025','026','027','028','029','030' ";	
        }	
        else if (ddl_class.SelectedIndex == 2) //N	
        {	
            str_class = " '004','010','011','013','016','021','023','024' ";	
        }	
        else	
        {	
            str_class = " '001','001-1','001-2','001-3','001-4','002','006','009','012','014','015','020','022','025','026','027','028','029','030','004','010','011','013','016','021','023','024' ";	
        }	
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);	
        Conn.Open();	
        SqlDataReader dr = null;	
        SqlCommand cmd = null;	
        string sql_str = null;	
         sql_str = " SELECT a.PA51002 as 工號, a.PA51004 as 姓名, b.PA11003  as 部門 FROM EHRS.hrs_mis.dbo.WPA51 AS a inner join EHRS.hrs_mis.dbo.WPA11 AS b " +
            " on a.PA51014 = b.PA11002 INNER JOIN ( " +
            " select work_no_1 from ( " +
            " select pb29003 COLLATE Chinese_PRC_Stroke_CI_AS as work_no_1 FROM  EHRS.hrs_mis.dbo.WPB29 " +
            " where pb29002 = @my_make_date and pb29004 in (1, 2) and pb29005 in (" + str_class + ")) as pb29 " +
            " left outer join (select work_no from mhedb.dbo.v_mhe_dining_list where make_date = @my_make_date ) as dining " +
            " on pb29.work_no_1 = dining.work_no " +
            " where work_no is null) as c " +
            " on a.pa51002 = c.work_no_1 " +
            " where pa11003 like @my_dept + '%' order by b.pa11003";
	
        cmd = new SqlCommand(sql_str, Conn);	
        cmd.Parameters.Add("@my_make_date", SqlDbType.Date);	
        cmd.Parameters["@my_make_date"].Value = tb_make_date.Text; //日期	
        	
        cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);	
        cmd.Parameters["@my_dept"].Value = ddl_dept.SelectedValue; //部門	
        dr = cmd.ExecuteReader();	
        if (dr.HasRows)	
        {	
            gv_no_meal.DataSource = dr;	
            gv_no_meal.DataBind();	
        }	
        else	
        {	
            Response.Write("<script language='javascript'>confirm('查無資料!');</script>");	
            gv_no_meal.DataSource = null;	
            gv_no_meal.DataBind();	
        }	
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
    }	
    //讀取資料表[WPA11]：部門	
    public List<string> DB_dept()	
    {	
        List<string> result = new List<string>();	
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);	
        Conn.Open();	
        SqlDataReader dr = null;	
        SqlCommand cmd = null;	
        string sql_str = null;	
        sql_str = "select distinct REPLACE(REPLACE([pa11003],'常日',''),'輪班','') FROM  EHRS.hrs_mis.dbo.WPA11 where pa11001 = 'MING' and pa11003 not in ('總務課','生管課','財務課','業務課')";	
        cmd = new SqlCommand(sql_str, Conn);	
        dr = cmd.ExecuteReader();	
        if (dr.HasRows)	
        {	
            while (dr.Read())	
            {	
                result.Add(dr[0].ToString());	
            }	
        }	
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

    //讀取資料表[PM_Opuser_Work]：讀取取消人員的工號，是則更新	
    //事件呼叫：btn_cancel_update_Click	
    public Boolean DB_cancel_update()	
    {	
        Boolean result = false;	
        string[] str_s = Session["OK"].ToString().Split('-');	
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);	
        Conn.Open();	
        SqlCommand cmd = null;	
        string sql_str = null;	
        sql_str = "SELECT count(work_no) FROM [dbo].[PM_Opuser_Work] where make_date = @my_make_date and work_no = @my_work_no and OP_State = 0";	
        cmd = new SqlCommand(sql_str, Conn);	
        cmd.Parameters.Add("@my_make_date", SqlDbType.DateTime);	
        cmd.Parameters["@my_make_date"].Value = tb_make_date.Text;	
        cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar,6);	
        cmd.Parameters["@my_work_no"].Value = tb_work_no.Text;	
        int count = (int)cmd.ExecuteScalar();	
        if (count > 0)	
        {	
            result = false;	
            cmd.Cancel();	
            sql_str = " update [dbo].[PM_Opuser_Work] set sys_date = getdate(), op_state = 1, modify_user = @my_modify_user " +	
                      " where make_date = @my_make_date and work_no = @my_work_no ";	
            cmd = new SqlCommand(sql_str, Conn);	
            cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);	
            cmd.Parameters["@my_modify_user"].Value = str_s[1];	
            cmd.Parameters.Add("@my_make_date", SqlDbType.DateTime);	
            cmd.Parameters["@my_make_date"].Value = tb_make_date.Text;	
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 6);	
            cmd.Parameters["@my_work_no"].Value = tb_work_no.Text;	
            cmd.ExecuteNonQuery();	
            cmd.Cancel();	
        }	
        else	
        {	
            result = true;	
        }	
        if (Conn.State == ConnectionState.Open)	
        {	
            Conn.Close();	
            Conn.Dispose();	
        }	
        return result;	
    }
}