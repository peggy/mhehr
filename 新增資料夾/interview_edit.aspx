<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="interview_edit.aspx.cs" Inherits="interview_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MHEHR邀約編輯介面</title>
    
    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            font-family: "Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }

        .container {
            max-width: 1500px;
        }

        hr {
            width: 30%;
        }

        .table > tbody > tr > td {
            vertical-align: middle;
        }

        .btn {
            font-family: '微軟正黑體';
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center border">
        <div class="row justify-content-center" style="padding-top: 20px; font-family: 微軟正黑體">
            <h3>招募</h3>
        </div>
        <hr />
        <div class="row justify-content-center" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <h3>個人資料</h3>
        </div>

        <div class="row justify-content-center">
            <div class="col-fixed col-sm text-left" style="height: 50px; padding-left: 20px;">
                <asp:Button ID="btn_insert" runat="server" Class="btn btn-outline-secondary btn-sm" Text="新增" OnClick="btn_insert_Click" Visible="False" />
                &nbsp;
                       <asp:Button ID="btn_delete" runat="server" Class="btn btn-outline-secondary btn-sm" Text="刪除" OnClick="btn_delete_Click" Visible="False" />
                &nbsp;
                        <asp:Button ID="btn_store" runat="server" Class="btn btn-outline-secondary btn-sm" Text="儲存" OnClick="btn_store_Click" />
            </div>

            <div class="col-fixed col-sm text-right" style="height: 50px; padding-right: 20px;">
                <asp:LinkButton ID="lbtn_query" runat="server" Font-Names="微軟正黑體" PostBackUrl="~/interview_query.aspx">查詢畫面</asp:LinkButton>
            </div>
        </div>

        <%--Table 新增 & 編輯  --%>
        <div class="row justify-content-center">
            <div class="col-auto text-left">
                <asp:Table ID="tb_candidates" class="table table-bordered table-sm" runat="server" Font-Names="微軟正黑體">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" class="text-center" colspan="11" Width="1450px">
                                    邀約
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Width="80px">建立時間</asp:TableCell>
                        <asp:TableCell runat="server" Width="100px">序號</asp:TableCell>
                        <asp:TableCell runat="server" Width="130px">職缺</asp:TableCell>
                        <asp:TableCell runat="server" Width="100px">姓名</asp:TableCell>
                        <asp:TableCell runat="server" Width="130px">電話</asp:TableCell>
                        <asp:TableCell runat="server" Width="140px">履歷代碼</asp:TableCell>
                        <asp:TableCell runat="server" Width="120px">履歷來源</asp:TableCell>
                        <asp:TableCell runat="server" Width="100px">邀約狀態 </asp:TableCell>
                        <asp:TableCell runat="server" Width="180px">備註 </asp:TableCell>
                        <asp:TableCell runat="server" Width="80px">修改日期 </asp:TableCell>
                        <asp:TableCell runat="server" Width="80px">修改人 </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Label ID="lb_c_create_time" runat="server" Text="" Width="80px" Font-Size="X-Small"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:Label ID="lb_c_id" runat="server" Text="" Width="100px"></asp:Label>
                        </asp:TableCell>

                        <%--200803_Betty：新增「廠務人員」職缺。BY PEGGY  --%>
                        <%--200803_Betty：新增「會計人員」職缺。BY PEGGY  --%>
                        <%--201020_家瑋：新增「品管工程師」職缺。BY PEGGY  --%>
                        <asp:TableCell runat="server">
                            <asp:DropDownList ID="ddl_c_opening" runat="server" Width="130px" required>
                                <asp:ListItem Value="請選擇">請選擇</asp:ListItem>
                                <asp:ListItem Value="業務代表">業務代表</asp:ListItem>
                                <asp:ListItem Value="會計人員">會計人員</asp:ListItem>
                                <asp:ListItem Value="清潔人員">清潔人員</asp:ListItem>
                                <asp:ListItem Value="廠務人員">廠務人員</asp:ListItem>
                                 <asp:ListItem Value="目檢人員">目檢人員</asp:ListItem>
                                <asp:ListItem Value="併裝人員">併裝人員</asp:ListItem>
                                <asp:ListItem Value="產線技術員">產線技術員</asp:ListItem>
                                <asp:ListItem Value="設備工程師">設備工程師</asp:ListItem>
                                <asp:ListItem Value="製程工程師">製程工程師</asp:ListItem>
                                <asp:ListItem Value="品管工程師">品管工程師</asp:ListItem>
                                <asp:ListItem Value="安環工程師">安環工程師</asp:ListItem>
                                <asp:ListItem Value="繪圖工程師">繪圖工程師</asp:ListItem>
                                <asp:ListItem Value="廠區護理師">廠區護理師</asp:ListItem>
                                <asp:ListItem Value="廠務值班人員">廠務值班人員</asp:ListItem>
                                <asp:ListItem Value="品檢員(IPQC)">品檢員(IPQC)</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_c_name" runat="server" Width="100px" required></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_c_phone" runat="server" Width="130px" required></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_c_resume_no" runat="server" Width="140px" required title="只能輸入數字" pattern="^[0-9]*$" OnTextChanged="tb_c_resume_no_TextChanged" AutoPostBack="True"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:RadioButtonList ID="rbl_c_resume_source" runat="server" class="table-borderless" Width="120px" required="required">
                                <asp:ListItem Value="主動應徵">&nbsp;主動應徵</asp:ListItem>
                                <asp:ListItem Value="公司邀約">&nbsp;公司邀約</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:RadioButtonList ID="rbl_c_state" runat="server" class="table-borderless" Width="100px" required="required">
                                <asp:ListItem Value="成功">&nbsp;成功</asp:ListItem>
                                <asp:ListItem Value="失敗">&nbsp;失敗</asp:ListItem>
                                <asp:ListItem Value="無效">&nbsp;無效</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_c_remark" runat="server" Width="180px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:Label ID="lb_c_modify_time" runat="server" Text="Label" Width="80px" Font-Size="X-Small"></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_c_modify_user" runat="server" ReadOnly="true" Width="80px" ForeColor="#999999"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>

        <div class="row justify-content-center">
            <div class="col-auto text-left">
                <asp:Table ID="tb_interview" class="table table-bordered table-sm" runat="server" Font-Names="微軟正黑體" Visible="False" Width="1450px">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" colspan="3" class="text-center" Width="1450px">
                                    面試
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">時間</asp:TableCell>
                        <asp:TableCell runat="server">狀態</asp:TableCell>
                        <asp:TableCell runat="server">備註</asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <div class="input-group date table-borderless" id="dtp_i_time" style="border-style:none">
                                    <asp:TextBox ID="tb_i_time" runat="server" Width="200px"></asp:TextBox>
                                    <span class="input-group-addon" style="padding-left: 5px; padding-top:5px">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:RadioButtonList ID="rbl_i_state" runat="server" class="table-borderless" required="required" AutoPostBack="True" Width="300px">
                                <asp:ListItem Value="錄取成功">&nbsp;錄取成功</asp:ListItem>
                                <asp:ListItem Value="錄取失敗" Enabled="True">&nbsp;錄取失敗</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_i_remark" runat="server" TextMode="MultiLine" Width="450px" Height="50px"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
            </div>
        </div>

        <div class="row justify-content-center">
            <div class="col-auto text-left">
                <asp:Table ID="tb_reportfor" class="table table-bordered table-sm" runat="server" Font-Names="微軟正黑體" Visible="False" Width="1450px">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" class="text-center" colspan="11" Width="1450px">
                                    報到
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">時間</asp:TableCell>
                        <asp:TableCell runat="server">狀態</asp:TableCell>
                        <asp:TableCell runat="server">工號</asp:TableCell>
                        <asp:TableCell runat="server">備註</asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">                         
                                <div class="input-group date table-borderless" id="dtp_r_time">
                                    <asp:TextBox ID="tb_r_time" runat="server"></asp:TextBox>
                                    <span class="input-group-addon" style="padding-left: 5px; padding-top:5px">
                                        <span class="glyphicon glyphicon-calendar"></span>
                                    </span>
                                </div>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:RadioButtonList ID="rbl_r_state" runat="server" class="table-borderless" required="required" AutoPostBack="True" Width="200px">
                                <asp:ListItem Value="報到成功">&nbsp;報到成功</asp:ListItem>
                                <asp:ListItem Value="報到失敗">&nbsp;報到失敗</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_r_empno" runat="server" Width="200px"></asp:TextBox>
                        </asp:TableCell>

                        <asp:TableCell runat="server">
                            <asp:TextBox ID="tb_r_remark" runat="server" TextMode="MultiLine" Width="500px" Height="50px"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </div>
    <script type='text/javascript'>
        $(function () {
            $('#dtp_i_time').datetimepicker({
                format: 'YYYY-MM-DD HH:mm',
            });
        });

        $(function () {
            $('#dtp_r_time').datetimepicker({
                format: 'YYYY-MM-DD HH:mm',
            });
        });
    </script>
</asp:Content>

