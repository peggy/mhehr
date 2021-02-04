using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class blank : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label lb = (Label)Master.FindControl("lb_login_state");
        if (!Page.IsPostBack)
        {
            if (Session["OK"] != null)
            {
                //判斷Session是否同一人登入(s)-----------------------------------------------------------
                if (DB_login_log(Session["ac"].ToString(), "insert"))
                {
                    Response.Write("<script language='javascript'>localStorage.setItem('logged_in', 'true');</script>");
                    Response.Write("<script language='javascript'>alert('錯誤!請關閉所有網頁再重新登入')</script>");
                    lb.Text = "1";
                }
                //判斷Session是否同一人登入(e)-----------------------------------------------------------
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
    }
}