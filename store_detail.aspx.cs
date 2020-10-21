using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class store_detail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OK"] == null)
        {
            Response.Redirect("login.aspx");
        }
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlDataReader dr = null;
        SqlCommand searchclasscmd = new SqlCommand("SELECT Right('000' + Cast(id as varchar),3) as ID, [class] as 屬性,[name] as 店名,[address] as 地址,[phone] as 電話,[expiry_date] as 到期日 FROM[dbo].[hr_store] where [id] = @my_id;", Conn);
        searchclasscmd.Parameters.Add("@my_id", SqlDbType.VarChar, 5);
        searchclasscmd.Parameters["@my_id"].Value = Request.QueryString["id"];

        dr = searchclasscmd.ExecuteReader();

        dv_store_detail.DataSource = dr;
        dv_store_detail.DataBind();
        lb_store_name.Text = dv_store_detail.Rows[2].Cells[1].Text; //標題字

        if (dr != null)
        {
            searchclasscmd.Cancel();

            dr.Close();
        }
        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }

        //檢查含序號的檔案是否存在目錄底下，配合png或jpg兩種格式
        DirectoryInfo dir = new DirectoryInfo(Server.MapPath("images_store"));
        FileInfo[] file_arr = dir.GetFiles();
        string filename = null;

        foreach(FileInfo fi in dir.GetFiles("*" + dv_store_detail.Rows[0].Cells[1].Text + "*"))
        {
                filename = fi.Name; 
        }

        //附件圖檔，取得店名作為檔名
        Image img = (Image)dv_store_detail.FindControl("img_store_detail");
        img.ImageUrl = "~/images_store/" + filename;

        //配合圖片FacncyBox放大顯示，需加入超連結
        HyperLink hy_img = (HyperLink)dv_store_detail.FindControl("hy_img");
        hy_img.NavigateUrl = "~/images_store/" + filename;
    }

    //上一頁
    //201014_Betty：新增上一頁按鈕。BY PEGGY
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Response.Write("<script language=javascript>history.go(-2);</script>");
    }
}