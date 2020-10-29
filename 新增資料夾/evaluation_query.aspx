<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="evaluation_query.aspx.cs" Inherits="evaluation_query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MHEHR考核查詢介面</title>
   
    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            /*font-family: Microsoft YaHei, Tahoma, Helvetica, Arial, "\5b8b\4f53", sans-serif;*/
            font-family: "Mocrosoft JhengHei UI","Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }
        .container {
            max-width: 1500px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center">
        <div class="row  justify-content-center text-center" style="padding-top: 20px; padding-bottom: 10px; font-family: 微軟正黑體">
            <h2>考核</h2>
        </div>

        <div class="row justify-content-center border">
            <div class="col-fixed col-sm text-center" style="padding-top: 15px; padding-bottom: 20px">
                <h3>查詢條件</h3>

                <asp:Table ID="tb_e_query" runat="server" Class="table table-hover table-borderless" Font-Names="微軟正黑體" Width="800px" HorizontalAlign="Center">
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" >
                            <asp:Label ID="lb_ep_a_state" runat="server" Text="狀態："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" colspan="3" Class="text-left">
                            <asp:RadioButtonList ID="rbl_ep_a_state" runat="server" Class="table-less"  style="padding-left:20px" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="未考核">&nbsp;尚未考核&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="已考核">&nbsp;考核完畢&nbsp;&nbsp;&nbsp;</asp:ListItem>
                                <asp:ListItem Value="結案">&nbsp;結案</asp:ListItem>
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                      <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right">
                            <asp:Label ID="lb_e_empno" runat="server" Text="工號："></asp:Label>
                        </asp:TableCell>
                           <asp:TableCell runat="server" Class="text-right ">
                            <asp:TextBox ID="tb_e_empno" runat="server" Class="form-control" Width="200px"></asp:TextBox>
                         </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right">
                            <asp:Label ID="lb_e_opening" runat="server" Text="職稱："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left">
                            <asp:DropDownList ID="ddl_e_opening" runat="server" class="btn-outline-secondary rounded"  Width="200px" Height="30px">
                                 <asp:ListItem Value="%">請選擇</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-right ">
                            <asp:Label ID="lb_e_dept" runat="server" Text="部門："  Width="50px" Visible="False"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left ">
                            <asp:DropDownList ID="ddl_e_dept" runat="server" class="btn-outline-secondary rounded" Width="200px" Height="30px" Visible="False">
                                 <asp:ListItem Value="%">請選擇</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right ">
                            <asp:Label ID="lb_e_start_date" runat="server" Text="考核開始日："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-right ">
                            <asp:TextBox ID="tb_e_start_date_1" runat="server" Class="form-control" Width="200px"></asp:TextBox>
                         </asp:TableCell>
                         <asp:TableCell runat="server" Class="text-center " >
                              ~
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-left ">
                            <asp:TextBox ID="tb_e_start_date_2" runat="server" Class="form-control" Width="200px"></asp:TextBox>
                        </asp:TableCell>
                         
                        
                    </asp:TableRow>

                     <asp:TableRow runat="server">
                        <asp:TableCell runat="server" Class="text-right" Width="150px">
                            <asp:Label ID="lb_e_end_date" runat="server" Text="考核結束日："></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell runat="server" Class="text-right" Width="200px" >
                           <asp:TextBox ID="tb_e_end_date_1" runat="server" Class="form-control" Width="200px"></asp:TextBox>
                         </asp:TableCell>
                         <asp:TableCell runat="server" Class="text-center" Width="50px" >
                             ~ 
                        </asp:TableCell>
                         <asp:TableCell runat="server" Class="text-left" Width="200px">
                            <asp:TextBox ID="tb_e_end_date_2" runat="server" Class="form-control" Width="200px"></asp:TextBox>
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
            <div class="col text-left col-12 col-lg-12 col-sm-12">
                <asp:GridView ID="gv_evaluation" runat="server" Class="table table-hover table-bordered"   OnRowDataBound="gv_Candidates_RowDataBound" HeaderStyle-BackColor="#999999" HeaderStyle-ForeColor="White">
                </asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>

