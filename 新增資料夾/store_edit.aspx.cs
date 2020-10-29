using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class store_edit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }
        btn_delete.Attributes.Add("onclick ", "return confirm( '確定要刪除嗎?');"); //確定按下「刪除」按鈕 跳出提示訊息：確定即刪除；取消即返回

        string[] str_s = Session["OK"].ToString().Split('-');

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        SqlDataReader dr = null;

        if (!IsPostBack)//第一次執行本程式
        {
            //連結資料庫 讀取資料(s)--------------------------------------------------------------------------------

            Conn.Open();
            SqlCommand search_cmd = new SqlCommand("select Right('000' + Cast(id as varchar),3) as ID,class,name,address,phone,Convert(varchar(10),expiry_date,23),modify_user,modify_time from dbo.hr_store where id = @my_id", Conn);
            search_cmd.Parameters.Add("@my_id", SqlDbType.VarChar, 10);
            search_cmd.Parameters["@my_id"].Value = Request.QueryString["id"];

            dr = search_cmd.ExecuteReader();

            if (dr.HasRows)
            {
                //有資料。編輯
                while (dr.Read())
                {
                    tb_id.Text = dr[0].ToString();
                    ddl_class.SelectedValue = dr[1].ToString();
                    tb_name.Text = dr[2].ToString();
                    tb_address.Text = dr[3].ToString();
                    tb_phone.Text = dr[4].ToString();
                    tb_expiry_date.Text = dr[5].ToString();
                    tb_modify_user.Text = dr[6].ToString();
                    tb_modify_time.Text = dr[7].ToString();
                }
            }
            else
            {
                tb_id.Text = Request.QueryString["id"]; //序號
                tb_modify_time.Text = DateTime.Now.ToString("yyyy-MM-dd tt hh:mm:ss"); //修改時間
                tb_modify_user.Text = str_s[1];

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
            //連結資料庫 讀取資料(s)--------------------------------------------------------------------------------
        }
    }

    //上一頁
    //201014_Betty：新增上一頁按鈕。BY PEGGY
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>history.go(-2);</script>");
    }

    //新增：點擊「新增」帶入預設值 
    //[預設值：導覽列點擊編輯頁面、edit畫面點擊新增按鈕皆帶入]
    protected void btn_insert_Click(object sender, EventArgs e)
    {
        string str_id = null;
        //設定 新增模式 預設值
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand cmd = new SqlCommand("select MAX(id) from dbo.HR_store", Conn); //取最大值
        int id = (int)cmd.ExecuteScalar(); //回傳第一筆資料：筆數
        cmd.Cancel();

        str_id = (id + 1).ToString();
        str_id = string.Format("{0:000}", Convert.ToInt32(str_id));

        Response.Redirect("/store_edit.aspx?ID=" + str_id + "&s=i");
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
            sql_cmd = "INSERT INTO [dbo].[HR_store]" +
               "([id],[class],[name],[address],[phone],[expiry_date],[modify_user],[modify_time]) " +
               "Values (@my_id,@my_class,@my_name,@my_address,@my_phone,@my_expiry_date,@my_modify_user,GETDATE()) ";
        }
        else if (Request.QueryString["s"] == "e") //編輯
        {
            sql_cmd = "update [dbo].[HR_store] set [class] = @my_class,[name] = @my_name, [address] = @my_address, [phone] = @my_phone, " +
                "[expiry_date] = @my_expiry_date, [modify_user] = @my_modify_user, [modify_time] = GETDATE() where [id] = @my_id  ";
        }

        SqlCommand cmd = new SqlCommand(sql_cmd, Conn);

        cmd.Parameters.Add("@my_id", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_id"].Value = tb_id.Text; //序號

        if (ddl_class.SelectedValue == "請選擇")
        {
            Response.Write("<script language='javascript'>alert('請選擇屬性!'); </script>");
            return;
        }
        else
        {
            cmd.Parameters.Add("@my_class", SqlDbType.VarChar, 20);
            cmd.Parameters["@my_class"].Value = ddl_class.SelectedValue; //屬性
        }
        
        cmd.Parameters.Add("@my_name", SqlDbType.VarChar, 20);
        cmd.Parameters["@my_name"].Value = tb_name.Text.Trim(' '); //名稱

        cmd.Parameters.Add("@my_address", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_address"].Value = tb_address.Text.Trim(' '); //地址

        cmd.Parameters.Add("@my_phone", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_phone"].Value = tb_phone.Text.Trim(' '); //電話

        cmd.Parameters.Add("@my_expiry_date", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_expiry_date"].Value = tb_expiry_date.Text.Trim(' '); //到期日

        cmd.Parameters.Add("@my_modify_user", SqlDbType.VarChar, 200);
        cmd.Parameters["@my_modify_user"].Value = str_s[1]; //修改人

        cmd.ExecuteNonQuery();

        cmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        if (Request.QueryString["s"] == "i") //新增
        {
            Response.Write("<script language='javascript'>alert('新增完成! 序號：" + Request.QueryString["id"] + "'); location.href='../store_edit.aspx?ID=" + Request.QueryString["id"] + "&s=e'; </script>"); //跳出訊息框並跳轉至另一頁面  
        }
        else if (Request.QueryString["s"] == "e") //編輯
        {
            Response.Write("<script language='javascript'>alert('修改完成! 序號：" + tb_id.Text + "'); </script>");
        }
    }

    //刪除：點擊「刪除」刪掉一筆資料並返回query畫面
    //[在PageLoad事件 會先判斷是否有點擊按鈕 並 跳出訊息框 確認是否要刪除]
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlCommand Deletecmd = new SqlCommand("delete from [HR_store] where [id] = @my_id", Conn);
        Deletecmd.Parameters.Add("@my_id", SqlDbType.VarChar, 10);
        Deletecmd.Parameters["@my_id"].Value = Request.QueryString["id"].TrimStart('0');

        Deletecmd.ExecuteNonQuery();

        Deletecmd.Cancel();

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
            Conn.Dispose();
        }

        //刪除該序號檔案(s)-----------------------------------------------------------------------------------
        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("images_store"));

        FileInfo[] file_arr = dir.GetFiles(); //.GetFiles() 從目前的目錄，取回檔案清單。
       
        foreach (FileInfo fi in file_arr)    //fi 個別的檔案。
        {
            
            if (fi.Name.Remove(fi.Name.Length - 4) == Request.QueryString["id"]) //去除副檔名，比對id 刪除檔案
            {
                fi.Delete(); //刪除檔案
            }
        }
        //刪除該序號檔案(e)-----------------------------------------------------------------------------------

        Response.Redirect("~/store_query.aspx");
    }


    //上傳：點擊「上傳」檢查檔案附檔名(jpg、png)、重新命名(序號)上傳
    protected void btn_file_Click(object sender, EventArgs e)
    {
        string path = Server.MapPath("images_store"); //路徑需更改

        if (fu_file.HasFile)
        {
            string file_name = fu_file.FileName;
            string file_ext_name = System.IO.Path.GetExtension(file_name);

            if ((file_ext_name == ".jpg") || (file_ext_name == ".png"))
            {
                file_name = Request.QueryString["id"] + file_ext_name;
                string save_path = Path.Combine(path, file_name);
                fu_file.SaveAs(save_path);

                Response.Write("<script language='javascript'>alert('上傳成功! 檔名：" + file_name + " '); </script>");
            }
            else
            {
                Response.Write("<script language='javascript'>alert('上傳失敗! 請選擇附檔名為 jpg 或 png 的檔案'); </script>");
            }
        }
        else
        {
            Response.Write("<script language='javascript'>alert('上傳失敗! 請先選擇檔案再上傳! '); </script>");
        }


    }
}