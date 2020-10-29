<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="evaluation_edit.aspx.cs" Inherits="evaluation_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MHEHR考核人員資料</title>
    
    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            /*font-family: Microsoft YaHei, Tahoma, Helvetica, Arial, "\5b8b\4f53", sans-serif;*/
            font-family: "Mocrosoft JhengHei UI","Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }

        .container {
            max-width: 1400px;
        }

        hr {
            width: 30%;
        }

        .col-xl-1 {
            text-align: right;
        }

        .col-xl-3 {
            text-align: left;
        }

        /*.fontstyle {
            font-weight: bold;
        }*/

        h1,h2,h3,h4,h5,h6{
            color:black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center">
        <div class="row justify-content-center" style="padding-top: 20px; font-family: 微軟正黑體">
            <h2>考核</h2>
        </div>
        <hr />
        <div class="row justify-content-center" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>
                <asp:Label ID="lb_title" runat="server" Text="人員資料"></asp:Label></h2>
        </div>

        <div class="row justify-content-center">
            <div class="col text-left" style="height: 60px; padding-left: 100px;">
                <asp:Button ID="btn_insert" runat="server" Class="btn btn-outline-secondary btn-sm" Text="新增" OnClick="btn_insert_Click" Visible="False" />
                &nbsp;
                <asp:Button ID="btn_delete" runat="server" Class="btn btn-outline-secondary btn-sm" Text="刪除" OnClick="btn_delete_Click" Visible="False" />
                &nbsp;
                <asp:Button ID="btn_store" runat="server" Class="btn btn-outline-secondary btn-sm" Text="儲存" OnClick="btn_store_Click" Visible="False" />
            </div>
             <div class="col text-right">
                <%-- 201019_Betty：新增「上一頁」按鈕。BY PEGGY --%>
                <asp:Button ID="btn_back" runat="server" Class="btn btn-outline-secondary btn-sm" Text="上一頁" OnClick="btn_back_Click" />
            </div>
        </div>

        <div class="row justify-content-center" style="width: 1250px">
            <div class="col-12 col-xl-2 col-sm-6 text-right">序號：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_id" runat="server" ReadOnly="True" ></asp:TextBox>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-right">工號：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_empno" runat="server" AutoPostBack="True" OnTextChanged="tb_e_empno_TextChanged" required></asp:TextBox>
            </div>

            <div class="col-12 col-xl-2 col-sm-6 text-right">項目：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:DropDownList ID="ddl_e_change" runat="server" Width="200px" Height="30px">
                    <asp:ListItem Value="請選擇">請選擇</asp:ListItem>
                    <asp:ListItem Value="升遷">升遷</asp:ListItem>
                    <asp:ListItem Value="職務異動">職務異動</asp:ListItem>
                    <asp:ListItem Value="試用期延長">試用期延長</asp:ListItem>
                </asp:DropDownList>
            </div>

        </div>



        <div class="row justify-content-center" style="width: 1250px; padding-top: 10px">
            <div class="col-12 col-xl-2 col-sm-6 text-right">姓名：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_name" runat="server" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-right">職稱：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_opening" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </div>

            <div class="col-12 col-xl-2 col-sm-6 text-right">部門：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_dept" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </div>

        </div>

        <div class="row justify-content-center" style="width: 1250px; padding-top: 10px">
            <div class="col-12 col-xl-2 col-sm-6 text-right">考核開始日：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_start_date" runat="server" AutoPostBack="True" OnTextChanged="tb_e_start_date_TextChanged" required></asp:TextBox>
            </div>

            <div class="col-12 col-xl-2 col-sm-6 text-right">考核結束日：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                 <asp:TextBox ID="tb_e_end_date" runat="server"></asp:TextBox>
            </div>

            <div class="col-12 col-xl-2 col-sm-6 text-right">單位主管：</div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_manager" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="width: 1250px; padding-top: 10px" id="e_time">
            <div class="col-12 col-xl-2 col-sm-6 text-right">
                <asp:Label ID="lb_e_create_time" runat="server" Text="建立時間：" Visible="False"></asp:Label>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_create_time" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-right">
                <asp:Label ID="lb_e_modify_time" runat="server" Text="修改時間：" Visible="False"></asp:Label>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-left">
                <asp:TextBox ID="tb_e_modify_time" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-right">    
                <asp:Label ID="lb_e_modify_user" runat="server" Text="修改人：" Visible="False"></asp:Label>
            </div>
            <div class="col-12 col-xl-2 col-sm-6 text-left" style="padding-bottom: 50px">
                <asp:TextBox ID="tb_e_modify_user" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
            </div>
        </div>

    </div>


    <div class="container text-center border" style="padding-top: 10px">
        <div class="row justify-content-center" style="padding-top: 30px; padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>考核項目</h2>
        </div>

        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border text-left bg-light " style="padding-top: 10px; padding-left: 10px;">
                <b>1. 工作能力</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_ability_5" runat="server" GroupName="rb_e_ability"/>
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_ability_4" runat="server" GroupName="rb_e_ability" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_ability_3" runat="server" GroupName="rb_e_ability" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_ability_2" runat="server" GroupName="rb_e_ability" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_ability_1" runat="server" GroupName="rb_e_ability" />
            </div>
        </div>
        <br />
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border text-left bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>2. 工作態度(含積極度、配合度)</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attitude_5" runat="server" GroupName="rb_e_attitude" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attitude_4" runat="server" GroupName="rb_e_attitude" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attitude_3" runat="server" GroupName="rb_e_attitude" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attitude_2" runat="server" GroupName="rb_e_attitude" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attitude_1" runat="server" GroupName="rb_e_attitude" />
            </div>
        </div>
        <br />
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border text-left bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>3. 應對進退</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_response_5" runat="server" GroupName="rb_e_response" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_response_4" runat="server" GroupName="rb_e_response" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_response_3" runat="server" GroupName="rb_e_response" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_response_2" runat="server" GroupName="rb_e_response" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_response_1" runat="server" GroupName="rb_e_response" />
            </div>
        </div>
        <br />
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border text-left bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>4. 人際關係</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_relationship_5" runat="server" GroupName="rb_e_relationship" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_relationship_4" runat="server" GroupName="rb_e_relationship" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_relationship_3" runat="server" GroupName="rb_e_relationship" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_relationship_2" runat="server" GroupName="rb_e_relationship" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_relationship_1" runat="server" GroupName="rb_e_relationship" />
            </div>
        </div>
        <br />
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border text-left bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>5. 出勤狀況</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attendance_5" runat="server" GroupName="rb_e_Attendance" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attendance_4" runat="server" GroupName="rb_e_Attendance" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attendance_3" runat="server" GroupName="rb_e_Attendance" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attendance_2" runat="server" GroupName="rb_e_Attendance" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_attendance_1" runat="server" GroupName="rb_e_Attendance" />
            </div>
        </div>

    </div>

    <br />
    <br />

    <div class="container text-center border" style="padding-top: 10px">
        <div class="row justify-content-center" style="padding-top: 30px; font-family: 微軟正黑體">
            <h2>考核等級</h2>
        </div>
        <br />

        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>總考核</b>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border">特優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">優</div>
            <div class="col-10 col-xl-2 col-sm-2 border">普通</div>
            <div class="col-10 col-xl-2 col-sm-2 border">差</div>
            <div class="col-10 col-xl-2 col-sm-2 border">特差</div>
        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_level_5" runat="server" GroupName="rb_e_level" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_level_4" runat="server" GroupName="rb_e_level" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_level_3" runat="server" GroupName="rb_e_level" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_level_2" runat="server" GroupName="rb_e_level" />
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border">
                <asp:RadioButton ID="rb_ep_level_1" runat="server" GroupName="rb_e_level" />
            </div>
        </div>
        <br />
        <div class="row justify-content-center" style="height: 40px;">
            <div class="col-10 col-xl-10 col-sm-10 text-left" style="padding-left: 10px;">
                <asp:Label ID="lb_ep_remark2" runat="server" class="text-secondary text-left" Style="font-size: small" Text="備註：通過標準之等級應在普通以上。"></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>綜合評比   </b><asp:Label ID="lb_comprehensive" runat="server" Text="* 必填" ForeColor="Red" Font-Size="Small"></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border" style="padding-top: 10px; padding-left: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_Comprehensive" runat="server" Width="800px" Height="80px" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <br />
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border border-dark  bg-dark text-white" style="padding-top: 10px; padding-left: 10px">
                <p style="font-size: larger">試用期通過與否</p>
            </div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-3 col-sm-3 border border-dark text-center ">通過</div>
            <div class="col-10 col-xl-3 col-sm-3 border border-dark text-center ">延長至六個月</div>
            <div class="col-10 col-xl-4 col-sm-4 border border-dark text-center ">未通過</div>

        </div>
        <div class="row justify-content-center" style="height: 30px;">
            <div class="col-10 col-xl-3 col-sm-3 border border-dark text-center">
                <asp:RadioButton ID="rb_ep_pass_3" runat="server" GroupName="rb_e_pass" />
            </div>
            <div class="col-10 col-xl-3 col-sm-3 border border-dark text-center">
                <asp:RadioButton ID="rb_ep_pass_2" runat="server" GroupName="rb_e_pass" />
            </div>
            <div class="col-10 col-xl-4 col-sm-4 border border-dark text-center">
                <asp:RadioButton ID="rb_ep_pass_1" runat="server" GroupName="rb_e_pass" />
            </div>
        </div>
        <br />

        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border text-center bg-light">填寫人</div>
            <div class="col-10 col-xl-3 col-sm-3 border text-center bg-light">填寫時間</div>
            <div class="col-10 col-xl-2 col-sm-2 border text-center bg-light">人事確認</div>
            <div class="col-10 col-xl-3 col-sm-3 border text-center bg-light">確認時間</div>
        </div>
        <div class="row justify-content-center" style="height: 30px">
            <div class="col-10 col-xl-2 col-sm-2 border text-center" style="padding-top: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_res_user" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="col-10 col-xl-3 col-sm-3 border text-center" style="padding-top: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_res_time" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="col-10 col-xl-2 col-sm-2 border text-center" style="padding-top: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_res_check_user" runat="server" Width="100px" ReadOnly="True"></asp:TextBox>
            </div>
            <div class="col-10 col-xl-3 col-sm-3 border text-center" style="padding-top: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_res_check_time" runat="server" Width="200px" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
        <br />

        <br />
        
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border bg-light" style="padding-top: 10px; padding-left: 10px">
                <b>人事註記</b>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-10 col-xl-10 col-sm-10 border " style="padding-top: 10px; padding-left: 10px; padding-bottom: 10px">
                <asp:TextBox ID="tb_ep_hr_remark" runat="server" Width="800px" Height="80px" TextMode="MultiLine" Visible="False"></asp:TextBox>
            </div>
        </div>
        <br />

        <div class="row justify-content-center" >
            <div class="col text-center" style="height: 60px; padding-left: 100px;">
                <asp:Button ID="btn_submit" runat="server" Class="btn btn-secondary" Text="提交" OnClick="btn_submit_Click" Visible="False" />
                <asp:Button ID="btn_hr_check" runat="server" Class="btn btn-secondary" Text="人事確認" OnClick="btn_hr_check_Click" Visible="False"  />
            </div>
        </div>
        <br />
    </div>

</asp:Content>

