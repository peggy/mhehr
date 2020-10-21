<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="interview_query.aspx.cs" Inherits="interview_query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MHEHR邀約查詢介面</title>

    <script type="text/javascript">
</script>

    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            /*font-family: Microsoft YaHei, Tahoma, Helvetica, Arial, "\5b8b\4f53", sans-serif;*/
            font-family: "Mocrosoft JhengHei UI","Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }

        .container {
            max-width: 1500px;
            /*align-items: center;
            justify-content: center;*/
            /*background-color: #eee;*/
        }

        .table > tbody > tr > td {
            vertical-align: middle;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center">
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>招募</h2>
        </div>
        <div class="row justify-content-center border">
            <div class="col-fixed col-sm text-center" style="padding-top: 10px; padding-bottom: 10px">
                <div style="font-size: 25px">
                    <h3>查詢條件</h3>
                </div>
                <asp:Table ID="tb_query" class="table table-borderless table-hover" runat="server" Font-Names="微軟正黑體" Width="800px" HorizontalAlign="Center">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="100px">
                            <asp:Label ID="lb_a_state" runat="server" Text="狀態："></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server" Class="text-left" Width="200px">
                            <asp:DropDownList ID="ddl_a_state" runat="server">
                                <asp:ListItem Value="%">請選擇</asp:ListItem>
                                <asp:ListItem Value="邀約成功">邀約成功</asp:ListItem>
                                <asp:ListItem Value="邀約失敗">邀約失敗</asp:ListItem>
                                <asp:ListItem Value="邀約無效">邀約無效</asp:ListItem>
                                <asp:ListItem Value="錄取成功">錄取成功</asp:ListItem>
                                <asp:ListItem Value="錄取失敗">錄取失敗</asp:ListItem>
                                <asp:ListItem Value="報到成功">報到成功</asp:ListItem>
                                <asp:ListItem Value="報到失敗">報到失敗</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>

                        <asp:TableCell runat="server" Class="text-right" Width="100px">
                            <asp:Label ID="lb_a_progress" runat="server" Text="進度："></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server" Class="text-left" Width="250px">
                            <asp:RadioButtonList ID="rbl_a_progress" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="進行中">&nbsp;進行中&nbsp;&nbsp;&nbsp;</asp:ListItem>

                                <asp:ListItem Value="結案">&nbsp;結案</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="100px">
                            <asp:Label ID="lb_c_opening" runat="server" Text="職缺："></asp:Label>
                        </asp:TableCell>

                        <%--200803_Betty：新增「廠務人員」職缺。BY PEGGY  --%>
                        <%--200930_Betty：新增「會計人員」職缺。BY PEGGY  --%>
                        <%--201020_家瑋：新增「品管工程師」職缺。BY PEGGY  --%>
                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="200px">
                            <asp:DropDownList ID="ddl_c_opening" runat="server">
                                <asp:ListItem Value="%">請選擇</asp:ListItem>
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
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="100px">
                            <asp:Label ID="lb_c_name" runat="server" Text="應徵者姓名："></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="200px">
                            <asp:TextBox ID="tb_c_name" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="100px">
                            <asp:Label ID="Label1" runat="server" Text="履歷代碼："></asp:Label>
                        </asp:TableCell>

                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="200px">
                            <asp:TextBox ID="tb_c_resume_no" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="200px">
                            <asp:Label ID="lb_c_create_time" runat="server" Text="修改時間："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="500px">
                            <asp:TextBox ID="tb_c_modify_time_1" runat="server"></asp:TextBox>
                            ~ 
                                        <asp:TextBox ID="tb_c_modify_time_2" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="200px">
                            <asp:Label ID="lb_i_time" runat="server" Text="面試時間："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="500px">
                            <asp:TextBox ID="tb_i_time_1" runat="server"></asp:TextBox>
                            ~ 
                                        <asp:TextBox ID="tb_i_time_2" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="200px">
                            <asp:Label ID="lb_r_time" runat="server" Text="報到時間："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left" colspan="3" Width="500px">
                            <asp:TextBox ID="tb_r_time_1" runat="server"></asp:TextBox>
                            ~ 
                                        <asp:TextBox ID="tb_r_time_2" runat="server"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server" Class="text-center">
                        <asp:TableCell runat="server" colspan="4" Width="500px">
                            <asp:Button ID="btn_query" runat="server" Class="btn btn-secondary btn" Text="查詢" OnClick="btn_query_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

            </div>
        </div>

        <%-- 查詢結果表格 --%>
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom:20px">
            <h3>查詢結果</h3>
        </div>

        <div class="row justify-content-center">
        </div>

        <div class="row justify-content-center">
            <div class="col text-left col-12 col-lg-12 col-sm-12">
                <asp:GridView ID="gv_Candidates" runat="server" Class="table table-hover table-bordered"   OnRowDataBound="gv_Candidates_RowDataBound">
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

