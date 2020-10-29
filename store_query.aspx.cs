using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;

using System.IO; //檔案讀寫


public partial class store_query : System.Web.UI.Page
{
    string btn_txt = null; //紀錄按鈕按下的文字
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!Page.IsPostBack)
        {
            //第一次執行本程式
            DBInit_Pager();

            counter(); //訪客計數
        }
    }

    //點擊類別，尋找dbo.hr_store相對應的資料
    protected void btn_class_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        if (btn.Text == "全部")
        {
            DBInit_Pager();
            return;
        }
        else
        {
            btn_txt = btn.Text;
        }

        DB_class(btn_txt);
    }

    //讀取資料庫並分頁
    //事件呼叫：Page_Load
    protected void DBInit_Pager([Optional] string str)
    {
        lb_pages.Visible = true;
        lb_total_page.Visible = true;

        Boolean haveRec = false;

        lb_pages.Text = null;

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand("SELECT  count(id) FROM [dbo].[hr_store]", Conn); //查詢資料筆數
        int RecordCount = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();

        int PageSize = 10; //每頁顯示10筆紀錄

        //撈不到紀錄---------------------------------------
        if (RecordCount == 0)
        {
            Response.Write("抱歉！無法找到您需要的紀錄！");
            Conn.Close();
            Response.End();
        }//------------------------------------------------

        //總共區分幾頁-------------------------------------------------------------------------------
        int TotalPage = 0; //記錄總頁數。搜尋到所有記錄需幾頁完成。
        if (RecordCount % PageSize > 0) // 「%」除法，傳回餘數
        {
            TotalPage = ((RecordCount / PageSize) + 1); //「/」除法，傳回整數。 如無法整除，加1頁顯示
        }
        else
        {
            TotalPage = (RecordCount / PageSize);
        }
        lb_total_page.Text = "共計" + TotalPage + "頁";
        //--------------------------------------------------------------------------------------------

        #region  //底下if判別式，是用來防呆，防止一些例外狀況。
        int p; //目前在第幾頁

        if (Request["p"] == null)
        {
            p = 1; //有問題，強制跳回第一頁（p=1）。
        }

        if (IsNumeric(Request["p"]) == false)
        {   // 或是寫成 int.TryParse()方法來檢測是否為整數？
            p = 1;
        }
        else
        {
            p = Convert.ToInt32(Request["p"]);
        }

        if ((p <= 0) || (p > TotalPage))
        {   //p必是整數。且需要大於零、比「資料的總頁數」要少
            p = 1;
        }

        //if (btnflag == true)
        //{
        //    p = 1; //如果按下回首頁按紐，就跳回第一頁
        //}

        #endregion

        int NowPageCount = 0; //目前這頁的記錄
        if (p > 0)
        {
            NowPageCount = (p - 1) * PageSize;
        }

        //DataReader---------------------------------------------------------------------------------------
        SqlDataReader dr = null;
        String SqlStr;
        if (str != null)
        {
            SqlStr = str;
        }
        else
        {
            SqlStr = "Select Right('000' + Cast(id as varchar),3) as ID, class as 屬性, name as 名稱, address as 地址,phone as 電話,modify_time as 修改時間 from (select ROW_NUMBER() OVER(ORDER BY modify_time desc) AS 'RowNo', * from hr_store) as t Where t.RowNo Between @Page1 and @Page2";
        }

        //新增ROWNUMBER資料欄 照順序排列

        SqlCommand cmd_store = new SqlCommand(SqlStr, Conn);
        cmd_store.Parameters.AddWithValue("@Page1", (NowPageCount + 1));
        cmd_store.Parameters.AddWithValue("@Page2", (NowPageCount + PageSize));

        dr = cmd_store.ExecuteReader();

        if (dr != null)
        {
            haveRec = true;
            gv_store.DataSource = dr;
            gv_store.DataBind();
        }

        cmd_store.Cancel();
        dr.Close();
        Conn.Close();
        //--------------------------------------------------------------------------------------------------

        #region  // 畫面下方的分頁功能
        if (haveRec)
        {
            if (TotalPage > 0)
            {

                //** 可以把檔名刪除，只留下 ?P=  即可！一樣會運作，但IE 11會出現 JavaScript錯誤。**
                //** 抓到目前網頁的「檔名」。 System.IO.Path.GetFileName(Request.PhysicalPath) **


                //「每十頁」一間隔，分頁功能====================================================================================
                int block_page = 0;
                block_page = p / 10; //取除法中的商

                if (block_page > 0)
                {
                    lb_pages.Text += "&nbsp;&nbsp;&nbsp;<a href='store_query.aspx?p=" + (((block_page - 1) * 10) + 9) + "'> [前十頁]  </a> &nbsp;&nbsp;";
                }

                //有傳來「頁數(p)」，而且頁數正確（大於零），出現<上一頁>功能==================
                if (p > 1)
                {
                    lb_pages.Text += "<a href = 'store_query.aspx?p=" + (p - 1) + "' >[上一頁] </a > &nbsp;";
                }
                //===========================================================================================

                for (int K = 0; K <= 10; K++)
                {
                    if ((block_page * 10 + K) <= TotalPage)
                    {   //--- Pages 資料的總頁數。共需「幾頁」來呈現所有資料？
                        if (((block_page * 10) + K) == p)
                        {   //--- p 就是「目前在第幾頁」
                            lb_pages.Text += "[<b>" + p + "</b>]" + "&nbsp;&nbsp;&nbsp;";
                        }
                        else
                        {
                            if (((block_page * 10) + K) != 0)
                            {
                                lb_pages.Text += "<a href='store_query.aspx?p=" + (block_page * 10 + K) + "'>" + (block_page * 10 + K) + "</a>";
                                lb_pages.Text += "&nbsp;&nbsp;&nbsp;";
                            }
                        }
                    }
                }

                //有傳來「頁數(p)」，而且頁數正確，出現<下一頁>功能==================
                if (p < TotalPage)
                {
                    lb_pages.Text += "&nbsp; <a href='store_query.aspx?p=" + (p + 1) + "'>[下一頁]</a>";
                }
                //====================================================================

                if ((block_page < (TotalPage / 10)) & (TotalPage >= (((block_page + 1) * 10) + 1)))
                {
                    lb_pages.Text += "&nbsp;&nbsp;<a href='store_query.aspx?p=" + ((block_page + 1) * 10 + 1) + "'>  [後十頁]  </a>";
                }
                //==================================================================================================================
            }
        }

        #endregion

    }

    

    //GridView綁定連結內容
    protected void gv_store_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string[] str_s = Session["OK"].ToString().Split('-');
        string[] arr_auth = DB_authority(str_s[1]);

        e.Row.Cells[0].Visible = false;
        e.Row.Cells[6].Visible = false; //修改日期

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //201029_Betty：近七天項目新增new圖示。BY PEGGY
            DateTime dt_modify_time = DateTime.Parse(e.Row.Cells[6].Text);
            DateTime dt_now_start = DateTime.Now.AddDays(-14);
            DateTime dt_now = DateTime.Now;
            if (DateTime.Compare(dt_modify_time, dt_now_start) >= 0 && DateTime.Compare(dt_now, dt_modify_time) >= 0)
            {
                Image img_new = new Image();
                img_new.ImageUrl = "../img/store_new.png";
                img_new.Width = 30;
                e.Row.Cells[3].Controls.Add(img_new);
            }

            //超連結
            HyperLink hyper = new HyperLink();
            hyper.Text = e.Row.Cells[3].Text;
            hyper.ForeColor = System.Drawing.Color.FromArgb(76, 114, 108);
            hyper.NavigateUrl = "../store_detail.aspx?p=" + Request.QueryString["p"] + "&id=" + e.Row.Cells[1].Text;
            e.Row.Cells[3].Controls.Add(hyper);

            //根據權限顯示「Edit」按鈕
            if (arr_auth[0] == "99" || arr_auth[0] == "10")
            {
                e.Row.Cells[0].Visible = true; //Edit

                LinkButton lbtn_edit = new LinkButton();
                lbtn_edit = (LinkButton)e.Row.FindControl("lbtn_edit");

                lbtn_edit.PostBackUrl = "../store_edit.aspx?p=" + Request.QueryString["p"] + "&id=" + e.Row.Cells[1].Text + "&s=e";

            }
        }

        //因「Edit」整欄為隱藏，故需增加Header的Visible = true
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (arr_auth[0] == "99" || arr_auth[0] == "10")
            {
                e.Row.Cells[0].Visible = true; //Edit
            }
        }
    }

    //搜尋店名
    protected void tb_search_name_TextChanged(object sender, EventArgs e)
    {
        DB_name();
    }

    // IsNumeric Function，檢查是否為整數型態？ return true or false
    static bool IsNumeric(object Expression)
    {
        // Variable to collect the Return value of the TryParse method.
        bool isNum;
        // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
        double retNum;
        // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
        // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
        isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    //讀取類別資料
    //事件呼叫：PageLoad、btn_Class_Click
    public void DB_class(string btn_txt)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;
        string sqlstr = "Select Right('000' + Cast(id as varchar),3) as ID, class as 屬性, name as 名稱, address as 地址,phone as 電話,modify_time as 修改日期 from HR_store where class like '%' + @my_class + '%' order by id desc"; //200804：新增「修改人」查詢欄位。BY PEGGY
        SqlCommand cmd = new SqlCommand(sqlstr, Conn);
        cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_class"].Value = btn_txt;

        dr = cmd.ExecuteReader();

        gv_store.DataSource = dr;
        gv_store.DataBind();

        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
        lb_pages.Visible = false;
        lb_total_page.Visible = false;
    }

    //讀取店名字串的資料
    //事件呼叫：PageLoad、tb_search_name_TextChanged
    public void DB_name()
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;
        string sqlstr = "Select Right('000' + Cast(id as varchar),3) as ID, class as 屬性, name as 名稱, address as 地址,phone as 電話,modify_time as 修改日期 from HR_store where name like '%' + @my_name + '%' order by id desc"; //200804：新增「修改人」查詢欄位。BY PEGGY
        SqlCommand cmd = new SqlCommand(sqlstr, Conn);

        if (tb_search_name.Text != "")
        {
            cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_name"].Value = tb_search_name.Text;
        }
        else
        {
            DBInit_Pager();
            return;
        }
        dr = cmd.ExecuteReader();

        gv_store.DataSource = dr;
        gv_store.DataBind();

        if (dr != null)
        {
            cmd.Cancel();
            dr.Close();
        }

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
        lb_pages.Visible = false;
        lb_total_page.Visible = false;
    }

    //讀取權限資料表，抓取個別權限(回傳字串陣列為符合公版格式)
    //事件呼叫：gv_store_RowDataBound
    private string[] DB_authority(string name)
    {
        string[] arr_auth = new string[1]; //修改：新增欄位權限

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();
        SqlDataReader dr = null;

        SqlCommand cmd = new SqlCommand("select store from dbo.hr_authority where name = @my_name", Conn);
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

    //訪客計數：文字檔讀寫加一紀錄人次
    //事件呼叫：PageLoad
    public void counter()
    {
        //讀取檔案 (務必修改這個檔案的權限，需要「寫入」的權限)
        StreamReader sr = new StreamReader(Server.MapPath("counter_store.txt"));

        //把檔案內, 原本的訪客人數[加一]
        string visitors = sr.ReadLine();
        sr.Close();
        sr.Dispose();

        lb_counter.Text = visitors;
    }


    
}