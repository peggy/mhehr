<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="meal_query.aspx.cs" Inherits="meal_query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .container-fluid {
            padding-left: 10%;
            padding-right: 10%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <div class="col text-center">
                <h2>訂餐</h2>
            </div>
        </div>
        <hr style="width: 50%" />
        <div class="row justify-content-center" style="padding-bottom: 20px">
            <div class="col text-center">
                <h2>查詢</h2>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-bottom: 20px">	
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right"></div>	
            <div class="col-12 col-lg-3 col-sm-12 text-left">	
                <asp:RadioButtonList ID="rbl_meal_state" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" CellPadding="10" OnSelectedIndexChanged="rbl_meal_state_SelectedIndexChanged">	
                    <asp:ListItem Selected="True">&nbsp;&nbsp;已訂餐人員</asp:ListItem>	
                    <asp:ListItem>&nbsp;&nbsp;未訂餐人員</asp:ListItem>	
                </asp:RadioButtonList>	
            </div>	
            <div class="col-12 col-lg-7 col-sm-12 text-lg-right"></div>	
        </div>
        <div class="row justify-content-center" style="padding-bottom: 10px; padding-left: 2%; padding-right: 2%">
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right"></div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_make_date" runat="server" Text="日期："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-left">
                <asp:TextBox ID="tb_make_date" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_dept" runat="server" Text="部門："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-left">
                <asp:DropDownList ID="ddl_dept" runat="server"  Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem>客人</asp:ListItem>
                    <asp:ListItem>總務</asp:ListItem>
                    <asp:ListItem>守衛室</asp:ListItem>
                    <asp:ListItem>事務所</asp:ListItem>
                    <asp:ListItem>生產部</asp:ListItem>
                    <asp:ListItem>製程課</asp:ListItem>
                    <asp:ListItem>設備課</asp:ListItem>
                    <asp:ListItem>廠務課</asp:ListItem>
                    <asp:ListItem>品管課</asp:ListItem>
                    <asp:ListItem>倉管課</asp:ListItem>
                    <asp:ListItem>物料課</asp:ListItem>
                    <asp:ListItem>製造課</asp:ListItem>
                </asp:DropDownList>
            </div>
	    <div class="col-12 col-lg-1 col-sm-12 text-lg-right">	
                <asp:Label ID="lb_class" runat="server" Text="班別："></asp:Label>	
            </div>	
            <div class="col-12 col-lg-2 col-sm-12 text-left">	
                <asp:DropDownList ID="ddl_class" runat="server" Class="form-control">	
                    <asp:ListItem Value="%">請選擇</asp:ListItem>	
                    <asp:ListItem>D</asp:ListItem>	
                    <asp:ListItem>N</asp:ListItem>	
                </asp:DropDownList>	
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-lg-right"></div>
        </div>
        <div class="row justify-content-center" style="padding-bottom: 10px; padding-left: 2%; padding-right: 2%" id="yes_meal_state" runat="server">
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right"></div>
	    <div class="col-12 col-lg-1 col-sm-12 text-lg-right">	
                <asp:Label ID="lb_work_no" runat="server" Text="工號："></asp:Label>	
            </div>	
            <div class="col-12 col-lg-2 col-sm-12 text-left">	
                <asp:TextBox ID="tb_work_no" runat="server" Class="form-control"></asp:TextBox>	
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_op_state" runat="server" Text="狀態："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-left">
                <asp:DropDownList ID="ddl_op_state" runat="server" Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem Value="1">訂餐</asp:ListItem>
                    <asp:ListItem Value="代訂">代訂</asp:ListItem>
                    <asp:ListItem Value="2">修改</asp:ListItem>
                    <asp:ListItem Value="0">取消</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 col-lg-1 col-sm-12 text-lg-right">
                <asp:Label ID="lb_scheduling" runat="server" Text="排班："></asp:Label>
            </div>
            <div class="col-12 col-lg-2 col-sm-12  text-left">
                <asp:DropDownList ID="ddl_scheduling" runat="server" Class="form-control">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                    <asp:ListItem>DD</asp:ListItem>
                    <asp:ListItem>DA</asp:ListItem>
                    <asp:ListItem>DB</asp:ListItem>
                    <asp:ListItem>DC</asp:ListItem>
                    <asp:ListItem>NA</asp:ListItem>
                    <asp:ListItem>NB</asp:ListItem>
                    <asp:ListItem>NC</asp:ListItem>
                    <asp:ListItem>FF</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 col-lg-2 col-sm-12 text-lg-right"></div>
        </div>
        <div class="row justify-content-center" style="padding-bottom: 50px">
            <div class="col-12 col-lg-12  text-center">
                <asp:Button ID="btn_query" runat="server" Text="查詢" Class="btn btn-dark" OnClick="btn_query_Click" />&nbsp;&nbsp;&nbsp;	
                <asp:Button ID="btn_cancel_update" runat="server" Text="刪除取消" Class="btn btn-primary" OnClick="btn_cancel_update_Click" />
            </div>
        </div>


        <div class="row justify-content-center" style="padding-bottom: 20px">
            <div class="col text-center">
                <asp:GridView ID="gv_meal" runat="server" Class="table table-sm table-hover table-responsive-sm table-bordered" HeaderStyle-BackColor="Silver" HeaderStyle-ForeColor="White" OnRowDataBound="gv_meal_RowDataBound" OnRowUpdating="gv_meal_RowUpdating" ShowFooter="True" OnRowEditing="gv_meal_RowEditing" OnRowCancelingEdit="gv_meal_RowCancelingEdit" OnRowCreated="gv_meal_RowCreated" OnRowCommand="gv_meal_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <EditItemTemplate>
                                <asp:Button ID="btn_update" runat="server" class="btn btn-sm btn-outline-dark" CommandName="Update" Text="儲存" />
                                &nbsp;
                                <asp:Button ID="btn_cancel" runat="server" class="btn btn-sm btn-outline-dark" CommandName="Cancel" Text="離開" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Button ID="btn_edit" runat="server" class="btn btn-sm btn-outline-dark" CommandName="Edit" Text="修改" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_update_state" runat="server" class="btn btn-sm btn-secondary"  Text="取消" CommandName="update_state" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
<HeaderStyle BackColor="Silver" ForeColor="White"></HeaderStyle>
                </asp:GridView>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-bottom: 20px;padding-left:20%;padding-right:20%">	
            <div class="col text-center">	
                <asp:GridView ID="gv_no_meal" runat="server" Class="table table-sm table-hover table-responsive-sm table-bordered"  HeaderStyle-BackColor="Silver" HeaderStyle-ForeColor="White"></asp:GridView>	
                 </div>	
        </div>	

    </div>
</asp:Content>

