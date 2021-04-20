﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class meal_edit : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lb_date.Text = DateTime.Now.ToString(); //現在時間

        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

	//權限
        string[] str_s = Session["OK"].ToString().Split('-');
        string[] arr_auth = DB_authority(Session["ac"].ToString(), "personal");

        if (!IsPostBack)//第一次執行本程式
        {
            DBInit();
            tb_meal_date.Text = DateTime.Now.ToString("yyyy-MM-dd");//訂餐日期
            //權限(s)-----------------------------------------------
            if (arr_auth[0] == "99" || arr_auth[0] == "10" || arr_auth[0] == "12" || arr_auth[0] == "21" || arr_auth[0] == "22" || arr_auth[0] == "13") //10-人事；12-警衛；13-會計；21-製造助理；22-助理
            {
                tb_empno.Visible = true;
                lb_empno_remark.Visible = true;
                ddl_quantity.Visible = true;
                lb_quantity.Visible = true;
            }
            //權限(e)-----------------------------------------------
            //班別編輯限制(s)------------------------------------------
            //警衛FF排除限制
            if (ddl_class.SelectedValue.StartsWith("D"))
            {
                rbl_breakfast.Enabled = false;
            }
            else if (ddl_class.SelectedValue.StartsWith("N"))
            {
                rbl_lunch.Enabled = false;
                rbl_dinner.Enabled = false;
            }
            //班別編輯限制(e)------------------------------------------
            DB_meal_personal(); //個人明細
            dt_limit(arr_auth); //訂餐時間限制
            record_meal_log("LOAD。訂餐載入。", "meal");
        }

        //查詢、班別編輯限制(s)------------------------------------------
        if (tb_empno.Text == "")
        {
            tb_class.Visible = true;
            ddl_class.Visible = false;
            lb_remark.Visible = false;
            tb_remark.Visible = false;
        }
        //else DBInit有查詢結果才顯示
        //查詢、班別編輯限制(e)------------------------------------------
    }

    //工號
    protected void tb_empno_TextChanged(object sender, EventArgs e)
    {
        cbl_vegetarian.SelectedIndex = -1;
        rbl_breakfast.Enabled = true;
        rbl_breakfast.SelectedIndex = 1;
        rbl_lunch.Enabled = true;
        rbl_lunch.SelectedIndex = 1;
        rbl_dinner.Enabled = true;
        rbl_dinner.SelectedIndex = 1;
        tb_remark.Text = "";
        if (tb_empno.Text == "99" || tb_empno.Text == "101" || tb_empno.Text == "102" || tb_empno.Text == "103")
        {
            tb_class.Visible = true;
            ddl_class.Visible = false;
            tb_name.Text = "";
            tb_class.Text = "DD";
            ddl_class.SelectedValue = "DD";
            lb_remark.Visible = true;
            tb_remark.Visible = true;
        }
        else
        {
            if (DBInit())
            {
                tb_empno.Text = "";
                tb_name.Text = "";
                tb_class.Text = "";
                ddl_class.SelectedValue = "請選擇";
            }
        }
        string[] str_s = Session["OK"].ToString().Split('-');
        string[] arr_auth = DB_authority(Session["ac"].ToString(), "personal");
        DB_meal_personal(); //個人明細
    }

    //提交
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        string[] arr_auth = DB_authority(Session["ac"].ToString(), "personal");
        //條件(s)---------------------------------------------------------------------------------
        #region
        //訂餐時間限制(s)----------------------------------------
        DateTime dt_meal = Convert.ToDateTime(tb_meal_date.Text);
        DateTime dt_now = DateTime.Now.Date;
        //任何時間點可以訂明天之後的餐
        if (DateTime.Compare(dt_now,dt_meal) == 0)  //今天
        {
            if (dt_limit(arr_auth)[0] == false)//訂餐時間限制
            {
                record_meal_log("ALERT。提交已超過訂餐時間。工號：" + tb_empno.Text + "，姓名：" + tb_name.Text, "meal");
                Response.Write("<script language='javascript'>confirm('錯誤! 已超過訂餐時間');</script>");
                return;
            }
        }
        else if (DateTime.Compare(dt_now, dt_meal) > 0) //昨天
        {
            record_meal_log("ALERT。提交已超過訂餐時間。工號：" + tb_empno.Text + "，姓名：" + tb_name.Text + "，日期：" + tb_meal_date.Text, "meal");
            Response.Write("<script language='javascript'>confirm('錯誤! 已超過訂餐時間');</script>");
            return;
        }
        //訂餐時間限制(e)----------------------------------------
        //判斷工號有無重複
        if (DB_empno() == true) //判斷工號有無重複
        {
            record_meal_log("ALERT。重複資料。工號：" + tb_empno.Text + "，姓名：" + tb_name.Text, "meal");
            Response.Write("<script language='javascript'>confirm('提交錯誤! 重複資料已存在');</script>");
            return;
        }
        if (rbl_breakfast.SelectedIndex < 0) //早餐
        {
            if (ddl_class.SelectedValue.StartsWith("N") || ddl_class.SelectedValue.StartsWith("X") || ddl_class.SelectedValue.StartsWith("F") || tb_empno.Text != "")
            {
                record_meal_log("ALERT。未選擇早餐。姓名："+ tb_name.Text +"，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇早餐');</script>");
                return;
            }            
        }
        if (rbl_lunch.SelectedIndex < 0) //午餐
        {
            if (ddl_class.SelectedValue.StartsWith("D") || ddl_class.SelectedValue.StartsWith("X") || ddl_class.SelectedValue.StartsWith("F") || tb_empno.Text != "")
            {
                record_meal_log("ALERT。未選擇午餐。姓名：" + tb_name.Text + "，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇午餐');</script>");
                return;
            }
        }
        if (rbl_dinner.SelectedIndex < 0) //晚餐
        {
            if (ddl_class.SelectedValue.StartsWith("D") || ddl_class.SelectedValue.StartsWith("X") || ddl_class.SelectedValue.StartsWith("F") || tb_empno.Text != "")
            {
                record_meal_log("ALERT。未選擇晚餐。姓名：" + tb_name.Text + "，班別：" + ddl_class.SelectedValue, "meal");
                Response.Write("<script language='javascript'>confirm('請選擇晚餐');</script>");
                return;
            }
        }
        #endregion
        //條件(e)---------------------------------------------------------------------------------
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        string sql_str = null;
        string str_my_class = null; //日夜班
        string str_name = null;
        for (int i = 0; i < Convert.ToInt32(ddl_quantity.SelectedValue); i++)
        {
            sql_str = "INSERT INTO [dbo].[PM_Opuser_Work] ([work_no],[make_us],[make_date],[card_no],[class],[Scheduling],[breakfast],[lunch],[dinner],[Vegetarian],[OP_State],[modify_user],[remarks]) " +
                      "VALUES (@my_work_no, @my_name, @my_make_date, @my_card_no, @my_class, @my_scheduling, @my_breakfast, @my_lunch, @my_dinner, @my_vegetarian,'1', @my_modify_user, @my_remarks )"; //找常日班

            cmd = new SqlCommand(sql_str, Conn);
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 6);
            cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@my_make_date", SqlDbType.DateTime);
            cmd.Parameters.Add("@my_card_no", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@my_scheduling", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@my_breakfast", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@my_lunch", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@my_dinner", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@my_vegetarian", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@my_remarks", SqlDbType.VarChar, 50);

            if (tb_empno.Text == "") //工號
            {
                cmd.Parameters["@my_work_no"].Value = Session["id"].ToString();
            }
            else
            {
                cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
            }
            //210308：如數量大於1，需重新編輯名稱，否則無法通過主索引的條件。BY PEGGY
            str_name = tb_name.Text; //姓名
            if (Convert.ToInt32(ddl_quantity.SelectedValue) > 1)
            {
                if (i != 0)
                {
                    str_name = str_name + "-" + i;
                }
            }
            cmd.Parameters["@my_name"].Value = str_name; //姓名
            cmd.Parameters["@my_make_date"].Value = tb_meal_date.Text; //日期
            cmd.Parameters["@my_card_no"].Value = lb_card_no.Text; //卡號
            if (ddl_class.SelectedValue.Contains("F")) //警衛人員額外判斷日夜班
            {
                if (Convert.ToDateTime(lb_date.Text).Hour < 19 && Convert.ToDateTime(lb_date.Text).Hour > 6)
                {
                    str_my_class = "D";
                }
                else
                {
                    str_my_class = "N";
                }
            }
            else
            {
                str_my_class = ddl_class.SelectedValue.Substring(0, 1);
            }
            cmd.Parameters["@my_class"].Value = str_my_class;//日夜班
            cmd.Parameters["@my_scheduling"].Value = ddl_class.SelectedValue; //班別
            cmd.Parameters["@my_breakfast"].Value = rbl_breakfast.SelectedValue; //早餐
            cmd.Parameters["@my_lunch"].Value = rbl_lunch.SelectedValue; //午餐
            cmd.Parameters["@my_dinner"].Value = rbl_dinner.SelectedValue; //晚餐
            if (cbl_vegetarian.Items[0].Selected)
            {
                cmd.Parameters["@my_vegetarian"].Value = "Y"; //素食
            }
            else
            {
                cmd.Parameters["@my_vegetarian"].Value = "";
            }
            if (tb_empno.Text == "")
            {
                cmd.Parameters["@my_modify_user"].Value = ""; //修改人
            }
            else
            {
                cmd.Parameters["@my_modify_user"].Value = str_s[1];
            }
            cmd.Parameters["@my_remarks"].Value = tb_remark.Text; //備註

            cmd.ExecuteNonQuery();
        }
        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
	DB_meal_personal(); //個人明細
        record_meal_log("SUBMIT。提交。", "meal");
        Response.Write("<script language='javascript'>confirm('提交完成');</script>");
    }

    protected void gv_meal_personal_RowCreated(object sender, GridViewRowEventArgs e)	
    {	
        if (e.Row.RowState == DataControlRowState.Edit || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate))	
        {	
            //早餐	
            DropDownList ddl_2 = new DropDownList();	
            ddl_2.ID = "ddl_2";	
            ddl_2.EnableViewState = false;	
            ddl_2.Items.Add(new ListItem("Y", "Y"));	
            ddl_2.Items.Add(new ListItem("N", ""));	
            e.Row.Cells[8].Controls.Add(ddl_2);	
            //午餐	
            DropDownList ddl_3 = new DropDownList();	
            ddl_3.ID = "ddl_3";	
            ddl_3.EnableViewState = false;	
            ddl_3.Items.Add(new ListItem("Y", "Y"));	
            ddl_3.Items.Add(new ListItem("N", ""));	
            e.Row.Cells[9].Controls.Add(ddl_3);	
            //晚餐	
            DropDownList ddl_4 = new DropDownList();	
            ddl_4.ID = "ddl_4";	
            ddl_4.EnableViewState = false;	
            ddl_4.Items.Add(new ListItem("Y", "Y"));	
            ddl_4.Items.Add(new ListItem("N", ""));	
            e.Row.Cells[10].Controls.Add(ddl_4);	
            //素食	
            DropDownList ddl_5 = new DropDownList();	
            ddl_5.ID = "ddl_5";	
            ddl_5.EnableViewState = false;	
            ddl_5.Items.Add(new ListItem("Y", "Y"));	
            ddl_5.Items.Add(new ListItem("N", ""));	
            e.Row.Cells[11].Controls.Add(ddl_5);	
        }	
    }	
    //取消確認	
    protected void gv_meal_personal_RowDataBound(object sender, GridViewRowEventArgs e)	
    {	
        string[] arr_auth = DB_authority(Session["ac"].ToString(), "personal");	
        DateTime dt_make_date;	
        DateTime dt_now = DateTime.Now;	
        if (e.Row.RowType == DataControlRowType.Header)	
        {	
            e.Row.Cells[2].Visible = false;	
        }	
        if (e.Row.RowType == DataControlRowType.DataRow)	
        {	
            Button btn_edit = (Button)e.Row.Cells[0].FindControl("btn_edit");	
            Button btn_update_state = (Button)e.Row.Cells[1].FindControl("btn_update_state");	
            btn_update_state.OnClientClick = "javascript:return confirm('確定取消?')";	
            //狀態顯示(s)----------------------------------	
            e.Row.Cells[2].Visible = false; //sys_id	
            if (e.Row.Cells[12].Text == "取消")	
            {	
                e.Row.Attributes.Add("style", "color:#8E9EAB");	
                btn_edit.Enabled = false; //編輯	
                btn_update_state.Enabled = false;    //取消	
            }	
            if (gv_meal_personal.EditIndex == e.Row.RowIndex)	
            {	
                TextBox tb = e.Row.Cells[6].Controls[0] as TextBox;	
                dt_make_date = Convert.ToDateTime(tb.Text);	
            }	
            else	
            {	
                dt_make_date = Convert.ToDateTime(e.Row.Cells[6].Text);	
            }	
            if (DateTime.Compare(dt_now,dt_make_date) > 0)	
            {	
                if (dt_limit(arr_auth)[0] == false)//訂餐時間限制	
                {	
                    btn_edit.Enabled = false;	
                    btn_update_state.Enabled = false;	
                }	
            }	
            //狀態顯示(e)----------------------------------	
            //編輯狀態(s)-----------------------------------------------	
            if (gv_meal_personal.EditIndex == e.Row.RowIndex)	
            {	
                //確認更新	
                Button u_btn = (Button)e.Row.Cells[0].FindControl("btn_update");	
                u_btn.OnClientClick = "javascript:return confirm('確定儲存?')";	
                string str_tmp = null;	
                int tmp = 0;	
                DropDownList ddl_22 = (DropDownList)e.Row.FindControl("ddl_2");	
                DropDownList ddl_33 = (DropDownList)e.Row.FindControl("ddl_3");	
                DropDownList ddl_44 = (DropDownList)e.Row.FindControl("ddl_4");	
                DropDownList ddl_55 = (DropDownList)e.Row.FindControl("ddl_5");	
                for (int i = 3; i < e.Row.Cells.Count; i++) //第0格為編輯按鈕，第1格為取消按鈕	
                {	
                    TextBox tb = null;	
                    tb = e.Row.Cells[i].Controls[0] as TextBox;	
                    tmp = 0;	
                    if (tb.Text == "" && tb != null)	
                    {	
                        tmp = 1;	
                    }	
                    if (i == 8) //早餐	
                    {	
                        ddl_22.SelectedIndex = tmp;	
                        e.Row.Cells[i].Controls.Clear();	
                        e.Row.Cells[i].Controls.Add(ddl_22);	
                        str_tmp += "早餐：" + tmp + "，";	
                    }	
                    else if (i == 9) //午餐	
                    {	
                        ddl_33.SelectedIndex = tmp;	
                        e.Row.Cells[i].Controls.Clear();	
                        e.Row.Cells[i].Controls.Add(ddl_33);	
                        str_tmp += "午餐：" + tmp + "，";	
                    }	
                    else if (i == 10) //晚餐	
                    {	
                        ddl_44.SelectedIndex = tmp;	
                        e.Row.Cells[i].Controls.Clear();	
                        e.Row.Cells[i].Controls.Add(ddl_44);	
                        str_tmp += "晚餐：" + tmp + "，";	
                    }	
                    else if (i == 11) //素食	
                    {	
                        ddl_55.SelectedIndex = tmp;	
                        e.Row.Cells[i].Controls.Clear();	
                        e.Row.Cells[i].Controls.Add(ddl_55);	
                        str_tmp += "素食：" + tmp + "，";	
                    }	
                    else if (i == 14) //備註	
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
                if (DateTime.Compare(dt_now, dt_make_date) >= 0)	
                {	
                    if (dt_limit(arr_auth)[1] == false)//訂餐時間限制	
                    {	
                        ddl_22.Enabled = false;	
                        ddl_33.Enabled = false;	
                        ddl_55.Enabled = false;	
                    }	
                    else	
                    {	
                    }	
                }	
                record_meal_log("UPDATE。載入更新個人資料訂餐。" + str_tmp, "meal");	
            }	
            //編輯狀態(e)---------------------------------------------- -	
        }	
    }	
    //編輯	
    protected void gv_meal_personal_RowEditing(object sender, GridViewEditEventArgs e)	
    {	
        gv_meal_personal.EditIndex = e.NewEditIndex;	
        DB_meal_personal();	
    }	
    //離開編輯	
    protected void gv_meal_personal_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)	
    {	
        gv_meal_personal.EditIndex = -1;	
        DB_meal_personal();	
    }	
    //更新資料表[PM_Opuser_Work]：抓早午晚餐及備住的欄位更新	
    protected void gv_meal_personal_RowUpdating(object sender, GridViewUpdateEventArgs e)	
    {	
        string str_id = gv_meal_personal.Rows[e.RowIndex].Cells[2].Text;	
        DropDownList my_ddl_breakfast, my_ddl_lunch, my_ddl_dinner, my_ddl_vegetarian;	
        TextBox my_t_remarks, my_t_make_date, my_t_sys_id, my_t_make_us;	
        my_ddl_breakfast = (DropDownList)gv_meal_personal.Rows[e.RowIndex].Cells[8].FindControl("ddl_2"); //早餐	
        my_ddl_lunch = (DropDownList)gv_meal_personal.Rows[e.RowIndex].Cells[9].FindControl("ddl_3"); //午餐	
        my_ddl_dinner = (DropDownList)gv_meal_personal.Rows[e.RowIndex].Cells[10].FindControl("ddl_4"); //晚餐	
        my_ddl_vegetarian = (DropDownList)gv_meal_personal.Rows[e.RowIndex].Cells[10].FindControl("ddl_5"); //素食	
        my_t_make_us = (TextBox)gv_meal_personal.Rows[e.RowIndex].Cells[5].Controls[0]; //姓名	
        my_t_make_date = (TextBox)gv_meal_personal.Rows[e.RowIndex].Cells[6].Controls[0]; //日期	
        my_t_remarks = (TextBox)gv_meal_personal.Rows[e.RowIndex].Cells[14].Controls[0]; //備註	
        string[] str_s = Session["OK"].ToString().Split('-');	
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);	
        Conn.Open();	
        string sql_cmd = "update [dbo].[PM_Opuser_Work] set sys_date = getdate(), breakfast = @my_breakfast, lunch = @my_lunch, dinner = @my_dinner, vegetarian = @my_vegetarian,  " +	
            " op_state = 2, modify_user = @my_modify_user, remarks = @my_remarks " +	
            " where sys_id = @my_sys_id";	
        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);	
        cmd.Parameters.Add("@my_breakfast", SqlDbType.VarChar, 1);	
        cmd.Parameters["@my_breakfast"].Value = my_ddl_breakfast.SelectedValue; //早餐	
        cmd.Parameters.Add("@my_lunch", SqlDbType.VarChar, 1);	
        cmd.Parameters["@my_lunch"].Value = my_ddl_lunch.SelectedValue; //午餐	
        cmd.Parameters.Add("@my_dinner", SqlDbType.VarChar, 1);	
        cmd.Parameters["@my_dinner"].Value = my_ddl_dinner.SelectedValue; //晚餐	
        cmd.Parameters.Add("@my_vegetarian", SqlDbType.VarChar, 1);	
        cmd.Parameters["@my_vegetarian"].Value = my_ddl_vegetarian.SelectedValue; //素食	
        cmd.Parameters.Add("@my_remarks", SqlDbType.VarChar, 50);	
        cmd.Parameters["@my_remarks"].Value = my_t_remarks.Text; //備註	
        cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 10);	
        cmd.Parameters["@my_modify_user"].Value = str_s[1]; //修改人	
        cmd.Parameters.Add("@my_sys_id", SqlDbType.Int);	
        cmd.Parameters["@my_sys_id"].Value = str_id; //id	
        cmd.ExecuteNonQuery();	
        cmd.Cancel();	
        if (Conn.State == ConnectionState.Open)	
        {	
            Conn.Close();	
            Conn.Dispose();	
        }	
        record_meal_log("UPDATE。更新個人訂餐。姓名：" + my_t_make_us.Text + "，日期：" + my_t_make_date.Text + "，" +	
            "早餐：" + my_ddl_breakfast.SelectedIndex + "，午餐：" + my_ddl_lunch.SelectedIndex + "，晚餐：" + my_ddl_dinner.SelectedIndex + "素食：" + my_ddl_vegetarian.SelectedIndex,	
            "meal");	
        gv_meal_personal.EditIndex = -1;	
        DB_meal_personal();	
    }	
    //更新資料表[PM_Opuser_Work]：狀態改為取消	
    protected void gv_meal_personal_RowCommand(object sender, GridViewCommandEventArgs e)	
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
            cmd.Parameters["@my_sys_id"].Value = gv_meal_personal.Rows[row.DataItemIndex].Cells[2].Text;	
            cmd.ExecuteNonQuery();	
            cmd.Cancel();	
            if (Conn.State == ConnectionState.Open)	
            {	
                Conn.Close();	
                Conn.Dispose();	
            }	
            record_meal_log("UPDATE。取消訂餐。姓名：" + gv_meal_personal.Rows[row.DataItemIndex].Cells[5].Text + "，日期：" + gv_meal_personal.Rows[row.DataItemIndex].Cells[6].Text + "。", "meal");	
            gv_meal_personal.EditIndex = -1;	
            DB_meal_personal();	
        }	
    }

    //刪除---暫無開放此功能
    protected void gv_meal_personal_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;
        SqlCommand Deletecmd = new SqlCommand("delete from [PM_Opuser_Work] where [sys_id] = @my_sys_id", Conn);
        Deletecmd.Parameters.Add("@my_sys_id", SqlDbType.VarChar, 20);
        Deletecmd.Parameters["@my_sys_id"].Value = gv_meal_personal.DataKeys[e.RowIndex].Value; //id
        Deletecmd.ExecuteNonQuery();
        if (dr != null)
        {
            Deletecmd.Cancel();
            //----關閉DataReader之前，一定要先「取消」SqlCommand
            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        record_meal_log("Delete。刪除資料。序號：" + gv_meal_personal.Rows[e.RowIndex].Cells[1].Text + "，工號：" + gv_meal_personal.Rows[e.RowIndex].Cells[3].Text + "姓名：" + gv_meal_personal.Rows[e.RowIndex].Cells[4].Text + "。日期：" + gv_meal_personal.Rows[e.RowIndex].Cells[5].Text, "meal");
        Response.Write("<script language='javascript'>alert('刪除資料完成! 姓名："+ gv_meal_personal.Rows[e.RowIndex].Cells[4].Text + "，日期：" + gv_meal_personal.Rows[e.RowIndex].Cells[5].Text + "');</script>");
        DB_meal_personal();
    }

    //讀取資料表[WPA51、MHE_user]：找HRS是否常日班，是則帶資料；否則撈[MHE_user]，帶班別資料
    //事件呼叫：PageLoad
    public Boolean DBInit()
    {
        string str_empno = null; //工號
        Boolean shift = false; //輪班
        Boolean err = false; //查無資料
        string dept = null;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string sql_str = null;
        //常日(s)------------------------------------------------------------------------------------------------------------------------
        sql_str = "SELECT a.pa51004,a.pa51023,b.pa11003 FROM EHRS.hrs_mis.dbo.WPA51 AS a INNER JOIN EHRS.hrs_mis.dbo.WPA11 AS b ON a.PA51014 = b.PA11002 " +
            "WHERE a.PA51011 = 1 and pa51002 = @my_empno"; //找常日班

        cmd = new SqlCommand(sql_str, Conn);
        if (tb_empno.Text == "")
        {
            str_empno = Session["id"].ToString(); //工號
        }
        else
        {
            str_empno = tb_empno.Text;
        }
        cmd.Parameters.Add("@my_empno", SqlDbType.VarChar, 10);
        cmd.Parameters["@my_empno"].Value = str_empno;
        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                tb_name.Text = dr[0].ToString();
                lb_card_no.Text = dr[1].ToString();
                dept = dr[2].ToString();
            }
            if (str_empno == "103157" || str_empno == "103168" || str_empno == "104099") //警衛人員
            {
                tb_class.Text = "FF";
                ddl_class.SelectedValue = "FF";
            }
            else
            {
                tb_class.Text = "DD";
                ddl_class.SelectedValue = "DD"; //常日班
            }
            if (dept.Contains("輪班"))
            {
                shift = true;
            }
        }
        else
        {
            err = true;
        }
        
        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }
        //常日(e)------------------------------------------------------------------------------------------------------------------------
        //輪班(s)------------------------------------------------------------------------------------------------------------------------
        if (shift == true)
        {
            sql_str = "SELECT make_us,Scheduling FROM [dbo].[MHE_user] where work_no = @my_work_no"; //輪班班別

            cmd = new SqlCommand(sql_str, Conn);
            cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 10);
            if (tb_empno.Text == "")
            {
                cmd.Parameters["@my_work_no"].Value = Session["id"].ToString(); //工號
            }
            else
            {
                cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
            }

            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tb_name.Text = dr[0].ToString();
                    tb_class.Text = dr[1].ToString();
                    ddl_class.SelectedValue = dr[1].ToString();
                }
                err = false;
            }
            else
            {
                err = true;
            }
            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
        }
        //輪班(e)------------------------------------------------------------------------------------------------------------------------
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        if (err)
        {
            record_meal_log("ALERT。查無此人，工號：" + tb_empno.Text, "meal");
            Response.Write("<script language='javascript'>confirm('查無此人');</script>");
        }
        else
        {
            if (tb_empno.Text != "")
            {
                tb_class.Visible = false;
                ddl_class.Visible = true;
                lb_remark.Visible = true;
                tb_remark.Visible = true;
            }
        }
        return err;
    }

    //讀取資料表[PM_Opuser_Work]：判斷工號有無重複
    //事件呼叫：btn_submit_Click
    public Boolean DB_empno()
    {
        Boolean result = false;
        int count;
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;

        string sql_str = null;
        sql_str = "SELECT count(work_no) FROM [dbo].[PM_Opuser_Work] where work_no = @my_work_no and make_date = @my_make_date and make_us = @my_make_us";

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 6);
        cmd.Parameters.Add("@my_make_date", SqlDbType.Date);
        cmd.Parameters.Add("@my_make_us", SqlDbType.VarChar, 10);
        if (tb_empno.Text == "")
        {
            cmd.Parameters["@my_work_no"].Value = Session["id"].ToString(); //工號
        }
        else
        {
            cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
        }
        cmd.Parameters["@my_make_date"].Value = tb_meal_date.Text;
        cmd.Parameters["@my_make_us"].Value = tb_name.Text;
        count = (int)cmd.ExecuteScalar();

        if (count > 0)
        {
            result = true;
        }
        
        cmd.Cancel();
        
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }
        return result;
    }

    //讀取資料表[PM_Opuser_Work]：找出近?天個人訂餐資料
    //事件呼叫：Page_Load
    public void DB_meal_personal()
    {
        int day_2 = 2; //昨天
        
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string sql_str = null;
        sql_str = " SELECT sys_id,convert(varchar(20),[sys_date],20) as 修改時間 ,[work_no] as 工號 ,[make_us] as 姓名 ,convert(varchar(20),[make_date],23) as 工作日期 ,[Scheduling] as 排班 , " +	
            " [breakfast] as 早餐 ,[lunch] as 午餐 ,[dinner] as 晚餐 ,[Vegetarian] as 素食, " +	
            " case when op_state = '1' and modify_user<> '' then '代訂'  when op_state = '1' and modify_user = '' then '訂餐' when op_state = '0' then '取消' when op_state = '2' then '修改' end as '狀態' ,[modify_user] as 修改人, remarks as 備註 " +	
            " FROM [dbo].[PM_Opuser_Work] where DATEDIFF(DAY, make_date, GETDATE()- " + day_2 + " ) < 0 and work_no = @my_work_no" +	
            " ORDER BY make_date desc";

        cmd = new SqlCommand(sql_str, Conn);
        cmd.Parameters.Add("@my_work_no", SqlDbType.VarChar, 6);
        if (tb_empno.Text != "")
        {
            cmd.Parameters["@my_work_no"].Value = tb_empno.Text;
        }
        else
        {
            cmd.Parameters["@my_work_no"].Value = Session["id"].ToString(); //工號
        }
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            gv_meal_personal.DataSource = dr;
            gv_meal_personal.DataBind();
        }
	else	
        {	
            gv_meal_personal.DataSource = null;	
            gv_meal_personal.DataBind();	
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

    //訂餐時間限制
    //事件呼叫：Page_Load、btn_submit_Click
    public List<Boolean> dt_limit(string[] arr_auth)
    {
	List<Boolean> list_result = new List<Boolean>();
        DateTime dt_now = DateTime.Now;
        DateTime dt_date = DateTime.Now.Date;
        DateTime dt_am_7 = dt_date.AddHours(7).AddMinutes(30);
        DateTime dt_pm_7 = dt_date.AddHours(19).AddMinutes(30);
        DateTime dt_am_9 = dt_date.AddHours(9).AddMinutes(15);
	DateTime dt_am_10 = dt_date.AddHours(10).AddMinutes(30);
        DateTime dt_pm_9 = dt_date.AddHours(21).AddMinutes(15);
        DateTime dt_pm_1 = dt_date.AddHours(13).AddMinutes(00);
        DateTime[] dt_arr = new DateTime[] { dt_am_7, dt_am_9, dt_am_10, dt_pm_1, dt_pm_7, dt_pm_9 };	
        string str_empno = Session["id"].ToString();
        Boolean dt_result = true;
	Boolean dt_result_gv = true;
        Boolean auth_result = false;
        if (arr_auth[0] == "99" || arr_auth[0] == "10" || arr_auth[0] == "12" || arr_auth[0] == "21" || arr_auth[0] == "22")
        {
            auth_result = true;
        }
	if (str_empno == "103157" || str_empno == "103168" || str_empno == "104099") //警衛人員	
        {	
            dt_result = true;	
            dt_result_gv = true;	
        }
        if (DateTime.Compare(dt_arr[0], dt_now) > 0) //7:30
        {
            dt_result = false;
        }
        else
        {
            if (DateTime.Compare(dt_arr[1], dt_now) > 0) //9:15
            {
                btn_submit.Enabled = true;
            }
            else
            {
                if (DateTime.Compare(dt_arr[2], dt_now) > 0) //10:30
                {
                    btn_submit.Enabled = true;	
                    rbl_breakfast.Enabled = false;	
                    rbl_lunch.Enabled = false;	
                    dt_result = true;	
                    dt_result_gv = false;
                }
                else
                {
                    if (DateTime.Compare(dt_arr[3], dt_now) > 0) //13:00	
                    {	
                        if (auth_result == true) //超過一點不能訂早午餐	
                        {	
                            btn_submit.Enabled = true;	
                            rbl_breakfast.Enabled = false;	
                            rbl_lunch.Enabled = false;	
                            dt_result = true;	
                        }	
                        else	
                        {	
                            dt_result = false;	
                        }	
                        dt_result_gv = false;	
                    }	
                    else	
                    {	
                        if (DateTime.Compare(dt_arr[4], dt_now) > 0) //19:15	
                        {	
                            dt_result = false;	
                            dt_result_gv = false;	
                        }	
                        else	
                        {	
                            if (DateTime.Compare(dt_arr[5], dt_now) > 0) //21:15	
                            {	
                                btn_submit.Enabled = true;	
                            }	
                            else	
                            {	
                                dt_result = false;	
                                dt_result_gv = false;	
                            }	
                        }	
                    }
                }
            }
        }
	list_result.Add(dt_result);	
        list_result.Add(dt_result_gv);	
        return list_result;
    }
    
}