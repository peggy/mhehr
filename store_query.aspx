<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="store_query.aspx.cs" Inherits="store_query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container text-center">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <h2 style="font-family: 'Playfair Display'">MHE</h2>
        </div>
        <hr />
        <div class="row justify-content-center" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>
                <asp:Label ID="lb_title" runat="server">特約商店</asp:Label></h2>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體">
            <div class="col-12 col-lg-2 col-xl-2">
                <asp:Table ID="tb_query" runat="server" class="table table-sm table-borderless text-center" >
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <b style="font-family: 'Playfair Display'; font-size: 20px">class</b>
                            <br />
                            <asp:Button ID="btn_class_1" runat="server" Text="全部" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_2" runat="server" Text="醫療" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_3" runat="server" Text="餐飲" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_4" runat="server" Text="娛樂" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_5" runat="server" Text="運動" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_6" runat="server" Text="住宿" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_7" runat="server" Text="皮件" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_8" runat="server" Text="車輛" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_9" runat="server" Text="文教" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_10" runat="server" Text="科技" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_11" runat="server" Text="幼兒園" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <%-- 201021_Betty：新增「團購」項目。BY PEGGY --%>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">
                            <asp:Button ID="btn_class_12" runat="server" Text="團購活動" Class="btn btn-sm btn-outline-light" Style="color: black" OnClick="btn_class_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div class="col-12 col-lg-10 col-xl-10">
                <div class="row">
                    <div class="col-12 col-sm-3 col-lg-3"></div>
                    <div class="col-12 col-sm-3 col-lg-3"></div>
                    <div class="col-12 col-sm-3 col-lg-3"></div>
                    <div class="col-12 col-sm-3 col-lg-3">
                        <div class="form-inline md-form form-sm">
                            <asp:TextBox ID="tb_search_name" runat="server" class="form-control form-control-sm" placeholder="Search Store Name" aria-label="Search" AutoPostBack="True" OnTextChanged="tb_search_name_TextChanged"></asp:TextBox>
                            <i class="glyphicon glyphicon-search" aria-hidden="true" style="padding-left: 5px"></i>
                        </div>
                    </div>
                </div>
                <br />
                <asp:GridView ID="gv_store" runat="server" class="table table-hover table-bordered rounded text-left" Style="padding-top: 50px;" HeaderStyle-BackColor="Silver" HeaderStyle-ForeColor="White" OnRowDataBound="gv_store_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lb_gv_edit" runat="server" Text="Edit"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtn_edit" runat="server" Text="Edit" Class="btn btn-outline-secondary btn-sm"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                <HeaderStyle BackColor="Silver" ForeColor="White"></HeaderStyle>
                </asp:GridView>
                <!-- 頁數 -->
                <br />
                <br />
                <asp:Label ID="lb_pages" runat="server" Font-Names="微軟正黑體"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lb_total_page" runat="server" Font-Names="微軟正黑體"></asp:Label>
                <br />
                
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col text-right">
                <asp:Label ID="lb_counter_name" runat="server" Text="瀏覽次數：" style="font-size:10px" ForeColor="Gray"></asp:Label>
                <asp:Label ID="lb_counter" runat="server" style="font-size:10px" Font-Names="微軟正黑體"  ForeColor="Gray" ></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

