<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="extension.aspx.cs" Inherits="extension" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes" />
    <style type="text/css">
        hr {
            width: 30%;
        }

        .row {
            font-family: 微軟正黑體;
            padding-left: 3%;
            padding-right: 3%;
            font-weight: bold;
        }

        .table {
            font-family: 微軟正黑體;
            table-layout: fixed;
            font-weight: bold;
        }

        .table-warning {
            padding-top: 2px;
            padding-bottom: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid">


        <div class="row justify-content-center" style="padding-top: 20px;">
            <h2>
                <asp:Label ID="lb_title" runat="server" Text="明徽能源分機表"></asp:Label></h2>
        </div>
        <hr />
        <div class="row justify-content-center " style="padding-top: 1px; padding-bottom: 10px">
            <div class="col text-center text-danger">
                <h6>公司代表號：05-5519968&nbsp;&nbsp;&nbsp;傳真：05-5519268&nbsp;&nbsp;&nbsp;統編：53129645</h6>
            </div>
        </div>

        <div class="row justify-content-center">
            <div class="col text-right" style="padding-bottom: 20px">
                <asp:DropDownList ID="ddl_export" runat="server" Class="btn btn-outline-secondary" Style="border-color: black;">
                    <asp:ListItem>請選擇</asp:ListItem>
                    <asp:ListItem>橫向</asp:ListItem>
                    <asp:ListItem>直向</asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btn_export" runat="server" Class="btn btn-sm btn-outline-secondary" Text="匯出Excel" OnClick="btn_export_Click" />
            </div>
        </div>

        <div class="row justify-content-center  text-center">
            <div class="col-12 col-md-6 col-lg-3  text-center">
                <asp:Label ID="lb_1_1" runat="server" Width="100%" Class="table-warning border"></asp:Label>
                <asp:GridView ID="gv_1_1" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_2" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_2" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_3" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_3" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_4" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_4" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_5" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_5" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_6" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_6" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_1_7" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_1_7" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
            </div>
            <div class="col-12 col-md-6 col-lg-3  text-center">
                <asp:Label ID="lb_2_1" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_2_1" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_2_2" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_2_2" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_2_3" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_2_3" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_2_4" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_2_4" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
            </div>
            <div class="col-12 col-md-6 col-lg-3  text-center">
                <asp:Label ID="lb_3_1" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_3_1" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_3_2" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_3_2" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_3_3" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_3_3" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_3_4" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_3_4" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_3_5" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_3_5" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
            </div>
            <div class="col-12 col-md-6 col-lg-3  text-center">
                <asp:Label ID="lb_4_1" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_4_1" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_4_2" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_4_2" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
                <asp:Label ID="lb_4_3" runat="server" Width="100%" Class="table-warning table-bordered"></asp:Label>
                <asp:GridView ID="gv_4_3" runat="server" Class="table table-bordered table-sm table-hover text-center"></asp:GridView>
            </div>
        </div>

    </div>
</asp:Content>

