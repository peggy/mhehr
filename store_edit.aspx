<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="store_edit.aspx.cs" Inherits="store_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        hr {
            width: 30%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <h2 style="font-family: 'Playfair Display'">特約</h2>
        </div>
        <hr />
        <div class="row justify-content-center" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>
                <asp:Label ID="lb_title" runat="server">商家資料</asp:Label></h2>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px; padding-bottom: 10px">
            <div class="col text-left" style="padding-left: 100px;">
                <asp:Button ID="btn_insert" runat="server" Class="btn btn-outline-secondary btn-sm" Text="新增" OnClick="btn_insert_Click" />
                &nbsp;
                <asp:Button ID="btn_delete" runat="server" Class="btn btn-outline-secondary btn-sm" Text="刪除" OnClick="btn_delete_Click" />
                &nbsp;
                <asp:Button ID="btn_store" runat="server" Class="btn btn-outline-secondary btn-sm" Text="儲存" OnClick="btn_store_Click" />
            </div>
            <div class="col text-right">
                <%-- 201014_Betty：新增「上一頁」按鈕。BY PEGGY --%>
                <asp:Button ID="btn_back" runat="server" Class="btn btn-outline-secondary btn-sm" Text="上一頁" OnClick="btn_back_Click" />
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 30px">
            <div class="col-12 col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_id" runat="server" Text="序號："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_id" runat="server" Class="form-control w-25" ReadOnly="True"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12 col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_class" runat="server" Text="屬性："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:DropDownList ID="ddl_class" runat="server" Class="form-control w-25">
                    <asp:ListItem Value="請選擇">請選擇</asp:ListItem>
                    <asp:ListItem Value="醫療">醫療</asp:ListItem>
                    <asp:ListItem Value="餐飲">餐飲</asp:ListItem>
                    <asp:ListItem Value="娛樂">娛樂</asp:ListItem>
                    <asp:ListItem Value="運動">運動</asp:ListItem>
                    <asp:ListItem Value="住宿">住宿</asp:ListItem>
                    <asp:ListItem Value="皮件">皮件</asp:ListItem>
                    <asp:ListItem Value="車輛">車輛</asp:ListItem>
                    <asp:ListItem Value="文教">文教</asp:ListItem>
                    <asp:ListItem Value="科技">科技</asp:ListItem>
                    <asp:ListItem Value="幼兒園">幼兒園</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_name" runat="server" Text="名稱："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_name" runat="server" Class="form-control w-75"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_address" runat="server" Text="地址："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_address" runat="server" Class="form-control w-75"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_phone" runat="server" Text="電話："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_phone" runat="server" Class="form-control w-25"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_expiry_date" runat="server" Text="到期日："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_expiry_date" runat="server" Class="form-control w-25"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_modify_user" runat="server" Text="修改人："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_modify_user" runat="server" Class="form-control w-25" ReadOnly="True"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_modify_time" runat="server" Text="修改時間："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:TextBox ID="tb_modify_time" runat="server" Class="form-control w-25" ReadOnly="True"></asp:TextBox>
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體; padding-top: 10px">
            <div class="col-12  col-lg-2 col-sm-2 text-right">
                <asp:Label ID="lb_file" runat="server" Text="附加檔案："></asp:Label>
            </div>
            <div class="col-12 col-lg-10 col-sm-10 text-left">
                <asp:FileUpload ID="fu_file" runat="server" Class="btn btn-light" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_file" runat="server" Class="btn btn-secondary btn-sm" Text="上傳" OnClick="btn_file_Click" />
            </div>
        </div>
    </div>
</asp:Content>

