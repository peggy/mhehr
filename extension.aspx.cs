using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data;

using NPOI.XSSF.UserModel;   //-- XSSF 用來產生Excel 2007檔案（.xlsx）
using NPOI.SS.UserModel; 
using System.IO;
using NPOI.SS.Util;
using NPOI.SS.Formula.Functions;
using System.Activities.Expressions;

public partial class extension : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)//第一次執行本程式
        {
            string[] str_arr = DB_init_dept(lb_1_1, lb_1_2, lb_1_3, lb_1_4, lb_1_5, lb_1_6,lb_1_7, lb_2_1, lb_2_2, lb_2_3, lb_2_4, lb_3_1, lb_3_2, lb_3_3, lb_3_4, lb_3_5,  lb_4_1, lb_4_2, lb_4_3); //讀取部門
            DB_init_gv(str_arr, gv_1_1, gv_1_2, gv_1_3, gv_1_4, gv_1_5, gv_1_6,gv_1_7, gv_2_1, gv_2_2, gv_2_3, gv_2_4, gv_3_1, gv_3_2, gv_3_3, gv_3_4, gv_3_5, gv_4_1, gv_4_2, gv_4_3);
            record_extension_log("extension_load_log", "load");
        }
    }

    //讀取資料表-部門，並填入Lable文字
    //事件呼叫：PageLoad
    private string[] DB_init_dept(params System.Web.UI.WebControls.Label[] label_group)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        string[] str_dept = new string[20];
        int i = 0;

        string sql_str = "select distinct(dept),id_dept from IT_extension order by id_dept ";
        SqlCommand cmd = new SqlCommand(sql_str, Conn);
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
            {
                str_dept[i] = dr[0].ToString();
                i++;
            }
        }

        for (int j = 0; j < label_group.Length; j++)
        {
            label_group[j].Text = str_dept[j];
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

        return str_dept;
    }

    //讀取資料表-GridView：根據DB_init_dept部門綁定GridView
    //事件呼叫：PageLoad
    private void DB_init_gv(string[] str_dept, params GridView[] gridview_group)
    {
        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        for (int i = 0; i < gridview_group.Length; i++)
        {
            string sql_str = "SELECT ext as 分機, ext_phone as 手機簡碼, ext_name as 名稱  FROM [dbo].[IT_extension] where dept = @my_dept order by id_name";
            SqlCommand cmd = new SqlCommand(sql_str, Conn);

            cmd.Parameters.Add("@my_dept", SqlDbType.VarChar, 10);
            cmd.Parameters["@my_dept"].Value = str_dept[i]; //部門

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                gridview_group[i].DataSource = dr;
                gridview_group[i].DataBind();
            }

            if (dr != null)
            {
                cmd.Cancel();
                dr.Close();
            }
        }

        if (Conn.State == ConnectionState.Open)
        {
            Conn.Close();
        }
    }

    //匯出Excel
    protected void btn_export_Click(object sender, EventArgs e)
    {
        //紀錄record_extension_user_log.txt
        record_extension_log("extension_user_log", "匯出。格式：" + ddl_export.SelectedValue);

        if (ddl_export.SelectedValue == "請選擇")
        {
            Response.Write("<script language='javascript'>alert('請選擇欲匯出Excel之格式'); </script>");
            return;
        }
        else
        {
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            IWorkbook workbook = new XSSFWorkbook(); //產生Excel檔
            XSSFSheet worksheet = (XSSFSheet)workbook.CreateSheet("分機表"); // 新增Sheet

            //for 橫向
            Label[] lb_gp_1 = new Label[] { lb_1_1, lb_1_2, lb_1_3, lb_1_4, lb_1_5, lb_1_6,lb_1_7 };
            Label[] lb_gp_2 = new Label[] { lb_2_1, lb_2_2, lb_2_3, lb_2_4 };
            Label[] lb_gp_3 = new Label[] { lb_3_1, lb_3_2, lb_3_3, lb_3_4, lb_3_5 };
            Label[] lb_gp_4 = new Label[] { lb_4_1, lb_4_2, lb_4_3 };

            GridView[] gv_gp_1 = new GridView[] { gv_1_1, gv_1_2, gv_1_3, gv_1_4, gv_1_5, gv_1_6,gv_1_7 };
            GridView[] gv_gp_2 = new GridView[] { gv_2_1, gv_2_2, gv_2_3, gv_2_4 };
            GridView[] gv_gp_3 = new GridView[] { gv_3_1, gv_3_2, gv_3_3, gv_3_4, gv_3_5 };
            GridView[] gv_gp_4 = new GridView[] { gv_4_1, gv_4_2, gv_4_3 };
            //for 直向
            Label[] lb_gp_5 = new Label[] { lb_1_1, lb_1_2, lb_1_3, lb_1_4 ,lb_2_1,lb_1_5,lb_3_2,lb_3_3,lb_3_5,lb_4_1,lb_1_7};
            Label[] lb_gp_6 = new Label[] { lb_3_4,lb_2_2,lb_2_3,lb_2_4,lb_3_1,lb_4_2};

            GridView[] gv_gp_5 = new GridView[] { gv_1_1, gv_1_2, gv_1_3, gv_1_4 , gv_2_1, gv_1_5, gv_3_2, gv_3_3, gv_3_5, gv_4_1, gv_1_7};
            GridView[] gv_gp_6 = new GridView[] { gv_3_4,gv_2_2,gv_2_3,gv_2_4,gv_3_1, gv_4_2 };

            int row_gp_1 = 0, row_gp_2 = 0, row_gp_3 = 0, row_gp_4 = 0, row_gp_5 = 0, row_gp_6 = 0;
            int row_gp_max = 0;
            int col_gp_max = 0;
            int add_row = 0;

            if (ddl_export.SelectedValue == "橫向")
            {
                col_gp_max = 12; //共12欄
                add_row = 10; //預留列數
                //計算群組的所有列數
                for (int i = 0; i < gv_gp_1.Length; i++) //注意最大列(暫定第一群組的列數)
                {
                    if (i < gv_gp_1.Length)
                    {
                        row_gp_1 += gv_gp_1[i].Rows.Count;
                    }
                    if (i < gv_gp_2.Length)
                    {
                        row_gp_2 += gv_gp_2[i].Rows.Count;
                    }
                    if (i < gv_gp_3.Length)
                    {
                        row_gp_3 += gv_gp_3[i].Rows.Count;
                    }
                    if (i < gv_gp_4.Length)
                    {
                        row_gp_4 += gv_gp_4[i].Rows.Count;
                    }
                }

                //找出最大列數
                int[] row_gp_int = new int[] { row_gp_1, row_gp_2, row_gp_3, row_gp_4 };
                for (int i = 0; i < row_gp_int.Length; i++)
                {
                    if (row_gp_int[i] > row_gp_max)
                    {
                        row_gp_max = row_gp_int[i];

                    }
                }
            }
            else if (ddl_export.SelectedValue == "直向")
            {
                col_gp_max = 6; //共6欄
                add_row = 15; //預留列數(標題、部門)
                //計算群組的所有列數
                for (int i = 0; i < gv_gp_5.Length; i++) //注意最大列(暫定第一群組的列數)
                {
                    if (i < gv_gp_5.Length)
                    {
                        row_gp_5 += gv_gp_5[i].Rows.Count;
                    }
                    if (i < gv_gp_6.Length)
                    {
                        row_gp_6 += gv_gp_6[i].Rows.Count;
                    }
                }

                //找出最大列數
                int[] row_gp_int_2 = new int[] {row_gp_5, row_gp_6};
                for (int i = 0; i < row_gp_int_2.Length; i++)
                {
                    if (row_gp_int_2[i] > row_gp_max)
                    {
                        row_gp_max = row_gp_int_2[i];

                    }
                }
            }
            
            int[] cell_gp1 = new int[] { 0, 1, 2 };
            int[] cell_gp2 = new int[] { 3, 4, 5 };
            int[] cell_gp3 = new int[] { 6, 7, 8 };
            int[] cell_gp4 = new int[] { 9, 10, 11 };

            //style(s)----------------------------------------------------------------
            #region
            ICellStyle style = workbook.CreateCellStyle();
            XSSFFont font = (XSSFFont)workbook.CreateFont();
            XSSFFont font1 = (XSSFFont)workbook.CreateFont();
            font.FontName = "標楷體"; //字型
            font.Boldweight = (short)FontBoldWeight.Bold; //粗體
            font1.FontName = "標楷體"; //字型
            font1.Boldweight = (short)FontBoldWeight.Bold; //粗體
            font1.FontHeightInPoints = 16; //字體大小

            XSSFCellStyle style20 = (XSSFCellStyle)workbook.CreateCellStyle(); //標題
            XSSFCellStyle style21 = (XSSFCellStyle)workbook.CreateCellStyle(); //標題
            XSSFCellStyle style30 = (XSSFCellStyle)workbook.CreateCellStyle();//內容
            XSSFCellStyle style31 = (XSSFCellStyle)workbook.CreateCellStyle();
            XSSFCellStyle style32 = (XSSFCellStyle)workbook.CreateCellStyle();
            XSSFCellStyle style33 = (XSSFCellStyle)workbook.CreateCellStyle();
            XSSFCellStyle style34 = (XSSFCellStyle)workbook.CreateCellStyle();

            style20.SetFont(font1); //字型
            style20.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style20.Alignment = HorizontalAlignment.Center;              //水平置中

            style21.SetFont(font); //字型
            style21.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium; //框線-底
            style21.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;    //框線-上
            style21.BorderLeft = NPOI.SS.UserModel.BorderStyle.Double;   //框線-左
            style21.BorderRight = NPOI.SS.UserModel.BorderStyle.Double;  //框線-右
            style21.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style21.Alignment = HorizontalAlignment.Center;              //水平置中

            style30.SetFont(font); //字型
            style30.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium; //框線-底
            style30.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;    //框線-上
            style30.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;   //框線-左
            style30.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;  //框線-右
            style30.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style30.Alignment = HorizontalAlignment.Center;              //水平置中

            style31.SetFont(font); //字型
            style31.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin; //框線-底
            style31.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;    //框線-上
            style31.BorderLeft = NPOI.SS.UserModel.BorderStyle.Double;   //框線-左
            style31.BorderRight = NPOI.SS.UserModel.BorderStyle.Double;  //框線-右
            style31.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style31.Alignment = HorizontalAlignment.Center;              //水平置中

            style32.SetFont(font); //字型
            style32.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin; //框線-底
            style32.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;    //框線-上
            style32.BorderLeft = NPOI.SS.UserModel.BorderStyle.Double;   //框線-左
            style32.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;  //框線-右
            style32.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style32.Alignment = HorizontalAlignment.Center;              //水平置中

            style33.SetFont(font); //字型
            style33.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin; //框線-底
            style33.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;    //框線-上
            style33.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;   //框線-左
            style33.BorderRight = NPOI.SS.UserModel.BorderStyle.Double;  //框線-右
            style33.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style33.Alignment = HorizontalAlignment.Center;              //水平置中

            style34.SetFont(font); //字型
            style34.BorderBottom = NPOI.SS.UserModel.BorderStyle.Medium; //框線-底
            style34.BorderTop = NPOI.SS.UserModel.BorderStyle.Medium;    //框線-上
            style34.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;   //框線-左
            style34.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;  //框線-右
            style34.VerticalAlignment = VerticalAlignment.Center;        //垂直置中
            style34.Alignment = HorizontalAlignment.Center;              //水平置中
            #endregion
            //style(e)----------------------------------------------------------------
            XSSFCell cell0,cell1,cell;
            IRow u_row;
            XSSFDataFormat format = (XSSFDataFormat)workbook.CreateDataFormat();
            //設定標題
            u_row = worksheet.CreateRow(0);
            u_row = worksheet.CreateRow(1);
            for (int j = 0; j < col_gp_max; j++)
            {
                cell0 = (XSSFCell)worksheet.GetRow(0).CreateCell(j);
                cell0.CellStyle = style20;

                cell = (XSSFCell)worksheet.GetRow(1).CreateCell(j);
                cell.CellStyle = style30;

                if (j == 0)
                {
                    cell0.SetCellValue("明徽能源通訊錄");
                    worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, col_gp_max-1));
                }
                if (j % 3 == 0)
                {
                    cell.SetCellValue("分機");
                }
                else if(j % 3 == 1)
                {
                    cell.SetCellValue("手機簡碼");
                    cell.CellStyle = style21;
                }
                else if (j % 3 == 2)
                {
                    cell.SetCellValue("名稱");
                }
            }
            worksheet.GetRow(0).HeightInPoints = 20; //列高
            worksheet.GetRow(1).HeightInPoints = 18; //列高 

            for (int i = 2; i < row_gp_max + add_row; i++) //預留部門位置(暫定10列)
            {
                u_row = worksheet.CreateRow(i);
                for (int j = 0; j < col_gp_max; j++)
                {
                    cell = (XSSFCell)worksheet.GetRow(i).CreateCell(j);
                    if (j % 3 == 2)
                    {
                        worksheet.SetColumnWidth(j, 20 * 256); //欄寬
                        cell.CellStyle = style32;
                    }
                    else if (j % 3 == 1)
                    {
                        worksheet.SetColumnWidth(j, 13 * 256); //欄寬
                        cell.CellStyle = style31;
                    }
                    else if (j % 3 == 0)
                    {
                        worksheet.SetColumnWidth(j, 13 * 256); //欄寬
                        cell.CellStyle = style33;
                    }
                }
                worksheet.GetRow(i).HeightInPoints = 18; //列高
            }

            u_row = worksheet.CreateRow(row_gp_max + add_row);
            u_row = worksheet.CreateRow(row_gp_max + add_row+1);
            for (int i = row_gp_max + add_row; i <= row_gp_max + add_row + 1; i++)
            {
                for (int j = 0; j < col_gp_max; j++)
                {
                    cell1 = (XSSFCell)worksheet.GetRow(row_gp_max + add_row).CreateCell(j);
                    cell1.CellStyle = style34;

                    cell = (XSSFCell)worksheet.GetRow(row_gp_max + add_row + 1).CreateCell(j);
                    cell.CellStyle = style34;
                
                    if (j == 0)
                    {
                        cell1.SetCellValue("公司代表號：05-5519968");
                        cell.SetCellValue("公司FAX：05-5519268");
                    }
                }
                worksheet.GetRow(i).HeightInPoints = 18; //列高
            }

            worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(row_gp_max + add_row, row_gp_max + add_row, 0, col_gp_max - 1));
            worksheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(row_gp_max + add_row + 1, row_gp_max + add_row + 1, 0, col_gp_max - 1));

            if (ddl_export.SelectedValue == "橫向")
            {
                excel_value(workbook, worksheet, gv_gp_1, lb_gp_1, cell_gp1);
                excel_value(workbook, worksheet, gv_gp_2, lb_gp_2, cell_gp2);
                excel_value(workbook, worksheet, gv_gp_3, lb_gp_3, cell_gp3);
                excel_value(workbook, worksheet, gv_gp_4, lb_gp_4, cell_gp4);

            }
            else if (ddl_export.SelectedValue == "直向")
            {
                excel_value(workbook, worksheet, gv_gp_5, lb_gp_5, cell_gp1);
                excel_value(workbook, worksheet, gv_gp_6, lb_gp_6, cell_gp2);
            }


            //20201110：文字格式，雙框線

            MemoryStream MS = new MemoryStream();   //==需要 System.IO命名空間
            workbook.Write(MS);

            //輸出檔名
            Response.AddHeader("Content-Disposition", "attachment; filename=明徽能源分機表" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
            Response.BinaryWrite(MS.ToArray());

            //釋放資源
            workbook = null;
            MS.Close();
            MS.Dispose();

            Response.Flush();
            Response.End();
        }
    }

    public void excel_value(IWorkbook workbook, XSSFSheet worksheet, GridView[] gv, Label[] lb, int[] cell_arr)
    {
        IDataFormat dataformat = workbook.CreateDataFormat();
        ICellStyle style = workbook.CreateCellStyle();
        XSSFFont font = (XSSFFont)workbook.CreateFont();
        font.FontName = "標楷體"; //字型
        font.Boldweight = (short)FontBoldWeight.Bold; //粗體

        XSSFCellStyle style1 = (XSSFCellStyle)workbook.CreateCellStyle(); //黃底置中
        XSSFCellStyle style12 = (XSSFCellStyle)workbook.CreateCellStyle(); 

        style1.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.LightYellow.Index; //藍色底的儲存格
        style1.FillPattern = FillPattern.SolidForeground;  //==底圖（紋路）
        style1.VerticalAlignment = VerticalAlignment.Center;
        style1.Alignment = HorizontalAlignment.Center;
        style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Medium;
        style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Medium;
        style1.SetFont(font); //字型

        XSSFCell cell;
        int row_tmp = 0;
        for (int ii = 0; ii < gv.Length; ii++)
        {
            cell = (XSSFCell)worksheet.GetRow(row_tmp+2).CreateCell(cell_arr[0]);
            cell.SetCellValue(lb[ii].Text);  //插入資料值。
            CellRangeAddress cra = new CellRangeAddress(row_tmp+2, row_tmp+2, cell_arr[0], cell_arr[2]);
            worksheet.AddMergedRegion(cra);
            cell.CellStyle = style1; //底色
            row_tmp++;
            for (int row = 0; row < gv[ii].Rows.Count; row++)
            {
                for (int col = 0; col < cell_arr.Length; col++) //抓不到gv[ii].Columns.Count的值，用cell_arr代替
                {
                    cell = (XSSFCell)worksheet.GetRow(row_tmp+2).GetCell(cell_arr[col]);
                    cell.SetCellValue(gv[ii].Rows[row].Cells[col].Text.ToString().Replace("&nbsp;",""));
                }
                row_tmp++;
            }
        }
    }

}
