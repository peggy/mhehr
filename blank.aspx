<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="blank.aspx.cs" Inherits="blank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            /*font-family: Microsoft YaHei, Tahoma, Helvetica, Arial, "\5b8b\4f53", sans-serif;*/
            font-family: "Mocrosoft JhengHei UI","Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }

        body {
            margin: 0px;
            padding: 0px;
            background:url('../img/bg_masterpage.jpg') center center fixed no-repeat;
            background-size: cover;
        }

        .wrap-headline {
            position: relative;
            padding-top: 30%;
            padding-bottom: 20%;
            width: 1280px;
            height: 880px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="container">
            <div class="row">
                <div class="wrap-headline text-center">

                    <asp:Label ID="lb_txt" runat="server" Text="" ForeColor="Gray"></asp:Label>
                </div>
            </div>
        </div>
</asp:Content>

