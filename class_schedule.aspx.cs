using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.SS.Util;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text.RegularExpressions;

public partial class class_schedule : class_login
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Excel綁定Gridview(s)-------------------------------------------------------------
        string filename = null;
        if (tb_cs_start_date.Text == "")
        {
            filename = DateTime.Now.ToString("yyyy");
        }
        else
        {
            filename = tb_cs_start_date.Text.Substring(0, 4);
        }
        lb_calendar_title.Text = filename + "年行事曆";
        string path = Server.MapPath("calendar_class_schedule") + "\\" + filename + ".xlsx";
        if (File.Exists(path))
        {
            Excel_hodily_gv(path);
        }
        else
        {
            if (ddl_daily_shift.SelectedValue == "常日")
            {
                Response.Write("<script language='javascript'>alert('無法載入行事曆!'); </script>");
                record_class_schedule_log("ALERT。無法載入行事曆。班表匯出。");
            }
            gv_calendar.DataSource = null;
            gv_calendar.DataBind();
        }
        record_class_schedule_log("LOAD。班表匯入");
        //Excel綁定Gridview(e)-------------------------------------------------------------
    }

    //班別選擇，顯示常日/輪班部門
    protected void ddl_daily_shift_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_dept_all.Checked = false;
        if (ddl_daily_shift.SelectedValue == "請選擇")
        {
            cb_dept_all.Visible = false;//全選
            cbl_daily_dept.Visible = false;//常日
            cbl_shift_dept_1.Visible = false;//輪班-產線
            cbl_shift_dept_2.Visible = false;//輪班-工程師
            lb_dept_daily_remark.Visible = false; //常日備註
            lb_dept_shift_remark.Visible = false; //輪班備註
        }
        else
        {
            cb_dept_all.Visible = true;
            if (ddl_daily_shift.SelectedValue == "常日")
            {
                cbl_daily_dept.Visible = true;
                lb_dept_daily_remark.Visible = true;
                cbl_shift_dept_1.Visible = false;
                cbl_shift_dept_2.Visible = false;
                lb_dept_shift_remark.Visible = false;
                rbl_shift.SelectedValue = null;
                ddl_shift_start_day.SelectedIndex = 0;
            }
            else if (ddl_daily_shift.Items[2].Selected) //輪班工程師
            {
                cbl_daily_dept.Visible = false;
                lb_dept_daily_remark.Visible = false;
                cbl_shift_dept_1.Visible = true;
                cbl_shift_dept_2.Visible = false;
                lb_dept_shift_remark.Visible = true;
            }
            else if (ddl_daily_shift.Items[3].Selected) //輪班產線
            {
                cbl_daily_dept.Visible = false;
                lb_dept_daily_remark.Visible = false;
                cbl_shift_dept_1.Visible = false;
                cbl_shift_dept_2.Visible = true;
                lb_dept_shift_remark.Visible = true;
            }
        }
        //避免部門全選的BUG
        for (int i = 0; i < cbl_daily_dept.Items.Count; i++)
        {
            cbl_daily_dept.Items[i].Selected = false;
        }
        for (int i = 0; i < cbl_shift_dept_1.Items.Count; i++)
        {
            cbl_shift_dept_1.Items[i].Selected = false;
        }
        for (int i = 0; i < cbl_shift_dept_2.Items.Count; i++)
        {
            cbl_shift_dept_2.Items[i].Selected = false;
        }
    }

    //匯出Excel按鈕
    protected void btn_export_Click(object sender, EventArgs e)
    {
        export_excel();
    }


    //部門全選
    protected void cb_dept_all_CheckedChanged(object sender, EventArgs e)
    {
        Boolean select = true; //常日
       
        if (cb_dept_all.Checked)
        {
            select = true;
        }
        else
        {
            select = false;
        }

        if (ddl_daily_shift.Items[1].Selected)
        {
            for (int i = 0; i < cbl_daily_dept.Items.Count; i++)
            {
                cbl_daily_dept.Items[i].Selected = select;
            }            
        }
        if (ddl_daily_shift.Items[2].Selected)
        {
            for (int i = 0; i < cbl_shift_dept_1.Items.Count; i++)
            {
                cbl_shift_dept_1.Items[i].Selected = select;
            }
        }
        if (ddl_daily_shift.Items[3].Selected)
        {
            for (int i = 0; i < cbl_shift_dept_2.Items.Count; i++)
            {
                cbl_shift_dept_2.Items[i].Selected = select;
            }
        }
    }

    //上傳行事曆
    protected void btn_import_Click(object sender, EventArgs e)
    {

        String savePath = Server.MapPath("calendar_class_schedule");
        string strExpression = "^\\d{0,4}$"; //正規表達式：數字四位
        Regex reg = new Regex(strExpression);

        if (fu_import.HasFile)
        {
            string filename = fu_import.FileName; //上傳的完整檔名，不含路徑
            string file_ext_name = System.IO.Path.GetExtension(filename); //副檔名 

            if (!reg.IsMatch(Path.GetFileNameWithoutExtension(filename)))
            {
                Response.Write("<script language='javascript'>alert('上傳失敗，檔名必須為西元年');</script>");
                return;
            }

            if ((file_ext_name == ".xls") || (file_ext_name == ".xlsx"))
            {
                savePath = System.IO.Path.Combine(savePath, filename);
                fu_import.SaveAs(savePath);
                Response.Write("<script language='javascript'>alert('上傳成功，檔名：" + filename + "');</script>");
            }
            else
            {
                Response.Write("<script language='javascript'>alert('上傳失敗! 請選擇附檔名為 xls 或 xlsx 的檔案'); </script>");
            }
        }
        else
        {
            Response.Write("<script language='javascript'>alert('請先挑選檔案再上傳');</script>");
        }
    }

    //讀取資料表文中人員個資
    public List<string> DB_person()
    {
        List<string> list_result = new List<string>();

        //取得選取的部門
        string str_dept = null;
        if (ddl_daily_shift.SelectedValue == "常日")
        {
            for (int i = 0; i < cbl_daily_dept.Items.Count; i++)
            {
                if (i != 2) //排除人事課
                {
                    if (cbl_daily_dept.Items[i].Selected)
                    {
                        str_dept += cbl_daily_dept.Items[i].Value + ",";
                    }
                }
            }
        }
        else if (ddl_daily_shift.Items[2].Selected) //輪班工程師
        {
            for (int i = 0; i < cbl_shift_dept_1.Items.Count; i++)
            {
                if (cbl_shift_dept_1.Items[i].Selected)
                {
                    str_dept += cbl_shift_dept_1.Items[i].Value + ",";
                }
            }
        }
        else if (ddl_daily_shift.Items[3].Selected) //輪班產線
        {
            for (int i = 0; i < cbl_shift_dept_2.Items.Count; i++)
            {
                if (cbl_shift_dept_2.Items[i].Selected)
                {
                    str_dept += cbl_shift_dept_2.Items[i].Value + ",";
                }
            }
        }

        SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["HRConnectionString"].ConnectionString);
        Conn.Open();

        SqlDataReader dr = null;
        SqlCommand cmd = null;
        string sql_str = null;
        if (ddl_daily_shift.SelectedValue == "常日")
        {
            sql_str = "select pa11003 ,pa51002 ,pa51004 FROM EHRS.hrs_mis.dbo.WPA51 AS a INNER JOIN EHRS.hrs_mis.dbo.WPA11 AS b ON a.PA51014 = b.PA11002 " +
            "where PA51011 = '1' "; //用參數方式無法取得資料

            if (cbl_daily_dept.Items[2].Selected) //人事課須排除警衛、沈祝琴
            {
                sql_str += "and (pa51014 in (" + str_dept.Substring(0, str_dept.Length - 1) + ") or (pa51014 = 13 and pa51135 in ('13A','01C','13B') and pa51002 <> '102037'))"; //課長、人事、總務
            }
            else
            {
                sql_str += "and pa51014 in (" + str_dept.Substring(0, str_dept.Length - 1) + ")";
            }
        }
        else if (ddl_daily_shift.SelectedValue.Contains("輪班"))
        {
            sql_str = "select pa11003,pa51002,pa51004 FROM EHRS.hrs_mis.dbo.WPA51 as a inner join [dbo].[MHE_user] as b on a.pa51002 = b.work_no " +
                "inner join EHRS.hrs_mis.dbo.WPA11 as c on a.PA51014 = c.PA11002 where a.pa51011 = 1 and b.Scheduling = '" + rbl_shift.SelectedValue + "' " +
                "and pa51014 in (" + str_dept.Substring(0, str_dept.Length - 1) + ")"; //用參數方式無法取得資料
        }
        cmd = new SqlCommand(sql_str, Conn);

        dr = cmd.ExecuteReader();
        if (dr.HasRows)
        {
            //資料行
            while (dr.Read())
            {
                list_result.Add(dr[0].ToString()); //部門名稱
                list_result.Add(dr[1].ToString()); //工號
                list_result.Add(dr[2].ToString()); //姓名
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
        return list_result;

    }

    //讀取行事曆綁定GridView：國定假日
    //事件呼叫：PageLoad
    public void Excel_hodily_gv(string path)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        ISheet sheet = null;
        IWorkbook wb = null;

        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        wb = new XSSFWorkbook(fs);
        sheet = wb.GetSheetAt(0);
        IRow row = null;

        if (sheet != null)
        {
            //表頭(s)-----------------------------------------------
            row = sheet.GetRow(0);
            if (row != null)
            {
                for (int m = 0; m < row.LastCellNum; m++)
                {
                    string cell_value = row.GetCell(m).ToString();
                    dt.Columns.Add(cell_value);
                }
            }
            //表頭(e)-----------------------------------------------

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                System.Data.DataRow dr = dt.NewRow();
                row = sheet.GetRow(i);
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum - 1; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            string cell_value = row.GetCell(j).ToString();
                            dr[j] = cell_value;
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        gv_calendar.DataSource = dt;
        gv_calendar.DataBind();

        fs.Dispose();
        fs.Close();
    }

    //讀取行事曆：國定假日
    //事件呼叫：btn_export_Click
    public Tuple<List<string>, List<string>> Excel_holiday(string path)
    {
        List<string> list_dt_h = new List<string>();
        List<string> list_str_h = new List<string>();

        ISheet sheet = null;
        IWorkbook wb = null;
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        wb = new XSSFWorkbook(fs);
        sheet = wb.GetSheetAt(0);
        IRow row = null;

        if (sheet != null)
        {
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                row = sheet.GetRow(i);
                if (row != null)
                {
                    list_dt_h.Add(row.GetCell(0).ToString()); //取得i列第0欄資料：國定假日日期
                    list_str_h.Add(row.GetCell(1).ToString());//取得i列第1欄資料：國定假日日期說明補班
                }
            }
        }

        fs.Dispose();
        fs.Close();

        return new Tuple<List<string>, List<string>>(list_dt_h, list_str_h);
    }

    //匯出
    //事件呼叫：btn_export_Click
    public void export_excel()
    {

        //條件判斷(s)---------------------------------------------------------------------------
        #region
        string str_err = null;
        int cbl_select = 0;
        int rbl_select = 0;
        //班別
        if (ddl_daily_shift.SelectedValue == "請選擇")
        {
            Response.Write("<script language='javascript'>alert('請選擇班別');</script>");
            record_class_schedule_log("ALERT。班別未選擇，班表匯出。");
            return;
        }
        else if (ddl_daily_shift.SelectedValue.Contains("輪班"))
        {
            //輪班組別
            for (int k = 0; k < rbl_shift.Items.Count; k++)
            {
                if (rbl_shift.Items[k].Selected)
                {
                    rbl_select++;
                }
            }
            if (rbl_select == 0)
            {
                Response.Write("<script language='javascript'>alert('請選擇輪班組別');</script>");
                record_class_schedule_log("ALERT。輪班組別未選擇，班表匯出。");
                return;
            }

            if (ddl_shift_start_day.SelectedValue == "請選擇")
            {
                Response.Write("<script language='javascript'>alert('請選擇當月第一天');</script>");
                record_class_schedule_log("ALERT。當月第一天未選擇，班表匯出。");
                return;
            }
        }

        //日期
        if (tb_cs_start_date.Text == "" | tb_cs_end_date.Text == "")
        {
            Response.Write("<script language='javascript'>alert('請輸入日期');</script>");
            record_class_schedule_log("ALERT。日期未輸入，班表匯出。");
            return;
        }
        else if (Convert.ToDateTime(tb_cs_end_date.Text) < Convert.ToDateTime(tb_cs_start_date.Text))
        {
            Response.Write("<script language='javascript'>alert('錯誤! 開始日期必須小於結束日期');</script>");
            record_class_schedule_log("ALERT。開始日期必須小於結束日期，班表匯出。");
            return;
        }
        //部門
        if (ddl_daily_shift.SelectedValue == "常日")
        {
            for (int k = 0; k < cbl_daily_dept.Items.Count; k++)
            {
                if (cbl_daily_dept.Items[k].Selected)
                {
                    cbl_select++;
                }
            }
        }
        else if (ddl_daily_shift.Items[2].Selected) //輪班工程師
        {
            for (int k = 0; k < cbl_shift_dept_1.Items.Count; k++)
            {
                if (cbl_shift_dept_1.Items[k].Selected)
                {
                    cbl_select++;
                }
            }
        }
        else if (ddl_daily_shift.Items[3].Selected) //輪班產線
        {
            for (int k = 0; k < cbl_shift_dept_2.Items.Count; k++)
            {
                if (cbl_shift_dept_2.Items[k].Selected)
                {
                    cbl_select++;
                }
            }
        }
        if (cbl_select == 0)
        {
            Response.Write("<script language='javascript'>alert('請選擇部門');</script>");
            record_class_schedule_log("ALERT。部門未選擇，班表匯出。");
            return;
        }
        #endregion
        //條件判斷(e)---------------------------------------------------------------------------

        string dt_now = DateTime.Now.ToString("yyMMdd");
        //Response.Clear();
        //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        IWorkbook workbook = new XSSFWorkbook(); //產生Excel檔
        XSSFSheet worksheet = (XSSFSheet)workbook.CreateSheet(dt_now); // 新增Sheet
        IRow u_row = null;

        //第1、2列填值與日期(s)-------------------------------------------------------------
        #region
        u_row = worksheet.CreateRow(0);
        u_row = worksheet.CreateRow(1);
        XSSFCell cell0, cell1, cell2, cell3;
        cell0 = (XSSFCell)worksheet.GetRow(0).CreateCell(0);
        cell0.SetCellValue("日期");
        cell1 = (XSSFCell)worksheet.GetRow(1).CreateCell(0);
        cell1.SetCellValue("部門名稱");
        cell1 = (XSSFCell)worksheet.GetRow(1).CreateCell(1);
        cell1.SetCellValue("員工代號");
        cell1 = (XSSFCell)worksheet.GetRow(1).CreateCell(2);
        cell1.SetCellValue("員工姓名");

        int date_col = 3, title_col = 3;
        DateTime dt_cs_start = Convert.ToDateTime(tb_cs_start_date.Text);
        DateTime dt_cs_end = Convert.ToDateTime(tb_cs_end_date.Text);

        while (dt_cs_start <= dt_cs_end)
        {
            cell0 = (XSSFCell)worksheet.GetRow(0).CreateCell(date_col);
            cell0.SetCellValue(dt_cs_start.ToString("yyyy/MM/dd"));
            dt_cs_start = dt_cs_start.AddDays(1);
            date_col = date_col + 2;

            cell1 = (XSSFCell)worksheet.GetRow(1).CreateCell(title_col);
            cell1.SetCellValue("區分");
            title_col++;
            cell1 = (XSSFCell)worksheet.GetRow(1).CreateCell(title_col);
            cell1.SetCellValue("班表");
            title_col++;
        }
        #endregion
        //第1、2列填值與日期(e)-------------------------------------------------------------



        //讀取DB人員資料並第三列寫值、判斷星期填班別(s)--------------------------------------
        List<string> list_p_result = DB_person();

        if (ddl_daily_shift.SelectedValue == "常日")
        {
            //讀取國定假日行事曆，需先判斷有無檔案
            List<string> list_dt_h;
            List<string> list_str_h;
            string file_path = Server.MapPath("calendar_class_schedule") + "\\" + tb_cs_start_date.Text.Substring(0, 4) + ".xlsx";
            if (File.Exists(file_path))
            {
                var db_result = Excel_holiday(file_path);
                list_dt_h = db_result.Item1;
                list_str_h = db_result.Item2;
            }
            else
            {
                Response.Write("<script language='javascript'>alert('找不到排班年份的行事曆'); </script>");
                str_err = "ALERT。找不到排班年分的行事曆，班表匯出。";
                record_class_schedule_log(str_err);
                return;
            }

            int list_col = 0;
            for (int i = 2; i <= (list_p_result.Count / 3) + 1; i++) //總資料筆數/3欄=列數
            {
                u_row = worksheet.CreateRow(i);//列
                for (int j = 0; j < 3; j++) //此人員名稱僅3欄
                {
                    cell0 = (XSSFCell)worksheet.GetRow(i).CreateCell(j);
                    cell0.SetCellValue(list_p_result[list_col]);
                    list_col++;
                }

                dt_cs_start = Convert.ToDateTime(tb_cs_start_date.Text);
                date_col = 3;
                while (dt_cs_start <= dt_cs_end)
                {
                    cell2 = (XSSFCell)worksheet.GetRow(i).CreateCell(date_col);
                    cell2.SetCellValue(dt_cs_start.DayOfWeek.ToString("d")); //DayOfWeek 0-日、1-一、2-二、3-三、4-四、5-五、6-六
                    if (dt_cs_start.DayOfWeek.ToString("d") == "0") //例休
                    {
                        cell2.SetCellValue(5);
                    }
                    else if (dt_cs_start.DayOfWeek.ToString("d") == "6") //排休
                    {
                        cell2.SetCellValue(3);
                    }
                    else
                    {
                        cell2.SetCellValue(1);
                    }

                    for (int k = 0; k < list_dt_h.Count; k++) //國定假日
                    {
                        if (dt_cs_start == Convert.ToDateTime(list_dt_h[k]))
                        {
                            if (list_str_h[k].Contains("補班"))
                            {
                                cell2.SetCellValue(1);
                            }
                            else
                            {
                                cell2.SetCellValue(6);
                            }
                        }
                    }
                    cell3 = (XSSFCell)worksheet.GetRow(i).CreateCell(date_col + 1);
                    cell3.SetCellValue("001");

                    dt_cs_start = dt_cs_start.AddDays(1);
                    date_col = date_col + 2;
                }
            }
        }
        else if (ddl_daily_shift.SelectedValue.Contains("輪班"))
        {
            int count = 0;
            int list_col = 0;
            int[] arr_shift = new int[] { 1, 1, 1, 1, 3, 5, 1, 1, 1, 3, 5, 1, 1, 1, 3, 5, 1, 1, 1, 3, 5, 1, 1, 1, 3, 5, 1, 1, 1, 1, 5, 1, 1, 1, 1, 3 };
            for (int i = 2; i <= (list_p_result.Count / 3) + 1; i++) //總資料筆數/3欄=列數
            {
                u_row = worksheet.CreateRow(i);//列
                for (int j = 0; j < 3; j++) //此人員名稱僅3欄
                {
                    cell0 = (XSSFCell)worksheet.GetRow(i).CreateCell(j);
                    cell0.SetCellValue(list_p_result[list_col]);
                    list_col++;
                }

                dt_cs_start = Convert.ToDateTime(tb_cs_start_date.Text);
                date_col = 3;
                while (dt_cs_start <= dt_cs_end)
                {
                    cell2 = (XSSFCell)worksheet.GetRow(i).CreateCell(date_col);
                    if (ddl_shift_start_day.SelectedValue == "1")
                    {
                        cell2.SetCellValue(arr_shift[count]);
                    }
                    else if (ddl_shift_start_day.SelectedValue == "2")
                    {
                        cell2.SetCellValue(arr_shift[count + 6]);
                    }
                    else if (ddl_shift_start_day.SelectedValue == "3")
                    {
                        cell2.SetCellValue(arr_shift[count + 12]);
                    }
                    else if (ddl_shift_start_day.SelectedValue == "4")
                    {
                        cell2.SetCellValue(arr_shift[count + 18]);
                    }
                    else if (ddl_shift_start_day.SelectedValue == "5")
                    {
                        cell2.SetCellValue(arr_shift[count + 24]);
                    }
                    else if (ddl_shift_start_day.SelectedValue == "6")
                    {
                        cell2.SetCellValue(arr_shift[count + 30]);
                    }

                    if ((count + 1) % 6 == 0)
                    {
                        count = 0;
                    }
                    else
                    {
                        count++;
                    }

                    cell3 = (XSSFCell)worksheet.GetRow(i).CreateCell(date_col + 1);
                    if (rbl_shift.SelectedValue.Contains("D") && (cbl_shift_dept_1.SelectedValue == "1712" || cbl_shift_dept_1.SelectedValue == "1722")) //日班工程師(因文中沒有判斷設備課或廠務課，所以漏給日班廠務)
                    {
                        cell3.SetCellValue("022");
                    }
                    else if (rbl_shift.SelectedValue.Contains("N") && (cbl_shift_dept_1.SelectedValue == "1712" || cbl_shift_dept_1.SelectedValue == "1722")) //夜班工程師(因文中沒有判斷設備課或廠務課，所以漏給夜班廠務)
                    {
                        cell3.SetCellValue("023");
                    }
                    else if (rbl_shift.SelectedValue.Contains("D") && (cbl_shift_dept_2.SelectedValue == "1732" || cbl_shift_dept_2.SelectedValue == "1742" || cbl_shift_dept_2.SelectedValue == "1802")) //日班產線
                    {
                        cell3.SetCellValue("009");
                    }
                    else if (rbl_shift.SelectedValue.Contains("N") && (cbl_shift_dept_2.SelectedValue == "1732" || cbl_shift_dept_2.SelectedValue == "1742" || cbl_shift_dept_2.SelectedValue == "1802")) //夜班產線
                    {
                        cell3.SetCellValue("010");
                    }

                    dt_cs_start = dt_cs_start.AddDays(1);
                    date_col = date_col + 2;
                }
                count = 0;
            }
        }
        //讀取DB人員資料並第三列寫值、判斷星期填班別(e)---------------------------------------

        MemoryStream MS = new MemoryStream(); //System.IO
        workbook.Write(MS);

        Response.AddHeader("Content-Disposition", "attachment; filename=" + dt_now + ".xlsx");//輸出檔名
        Response.BinaryWrite(MS.ToArray());

        //釋放資源
        workbook = null;
        MS.Close();
        MS.Dispose();

        Response.Flush();
        Response.End();

        record_class_schedule_log("EXPORT。已匯出，班別：" + ddl_daily_shift.SelectedValue + "日期區間：" + tb_cs_start_date.Text + " - " + tb_cs_end_date.Text + "。班表匯出。");
    }


}

