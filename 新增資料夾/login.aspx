<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>MHEHR登入介面</title>
    
    <style type="text/css">
        /* 變更預設字型 */
        body, button, input, select, textarea, h1, h2, h3, h4, h5, h6 {
            /*font-family: Microsoft YaHei, Tahoma, Helvetica, Arial, "\5b8b\4f53", sans-serif;*/
            font-family: "Helvetica Neue", Helvetica, Arial, "微軟正黑體", "微软雅黑", "メイリオ", "맑은 고딕", sans-serif;
        }

        .bg {
            background-image: url("../img/bg_masterpage.jpg");
            background-size: cover;
        }

        .wrap-headline {
            position: relative;
            padding-top: 20%;
            padding-bottom: 20%;
            width: 1080px;
            height: 880px;
        }

        h2 {
            font-size: 1.5rem;
        }

        hr {
            width: 30%;
        }

        .btn {
            font-family: '微軟正黑體';
            color: gray;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="bg">
        <div class="container">
            <div class="row">
                <div class="wrap-headline">
                    <h1 class="text-center" style="font-family: 'Playfair Display'">M H E</h1>
                    <h2 class="text-center" style="font-family: 'Microsoft JhengHei'">登 入 介 面</h2>
                    <hr>
                    <ul class="list-inline list-unstyled text-center">
                        <li class="list-inline-item">
                            <asp:Label ID="lb_login_id" runat="server" Text="帳號：" Font-Names="微軟正黑體" ForeColor="#666666" Font-Bold="True"></asp:Label>
                        </li>
                        <li class="list-inline-item">
                            <asp:TextBox ID="tb_login_id" class="form-control" runat="server" BorderStyle="Solid"></asp:TextBox>
                        </li>
                    </ul>

                    <ul class="list-inline list-unstyled text-center">
                        <li class="list-inline-item">
                            <asp:Label ID="lb_login_pass" runat="server" Text="密碼：" Font-Names="微軟正黑體" ForeColor="#666666" Font-Bold="True"></asp:Label>
                        </li>
                        <li class="list-inline-item">
                            <asp:TextBox ID="tb_login_pass" class="form-control" runat="server" BorderStyle="Solid" TextMode="Password"></asp:TextBox>
                        </li>
                    </ul>

                    <ul class="list-inline list-unstyled text-center" style="padding-top: 10px">
                        <li class="list-inline-item">
                            <asp:Button ID="btn_login_login" class="btn btn-outline-secondary btn-sm" runat="server" Text="登入" OnClick="btn_login_login_Click" />
                        </li>
                        <li class="list-inline-item">
                            <asp:Button ID="btn_login_cancel" class="btn btn-outline-secondary btn-sm" runat="server" Text="取消" />
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

