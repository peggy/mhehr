<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="store_detail.aspx.cs" Inherits="store_detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- 圖片FancyBox放大顯示 --%>
    <link href="packages/fancybox-master/dist/jquery.fancybox.min.css" rel="stylesheet" />
    <script src="packages/jQuery.3.5.1/Content/Scripts/jquery-3.5.1.min.js"></script>
    <script src="packages/fancybox-master/dist/jquery.fancybox.min.js"></script>
    <style type="text/css">
        hr {
            width: 30%;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container text-center">
        <div class="row justify-content-center" style="padding-top: 20px;">
            <h2 style="font-family: 'Playfair Display'">MHE - 特約商店</h2>
        </div>
        <hr />
        <div class="row justify-content-center" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <h2>
                <asp:Label ID="lb_store_name" runat="server" Text="特約商店"></asp:Label></h2>
        </div>

        <%-- 201014_Betty：新增上一頁按鈕。BY PEGGY --%>
        <div class="row" style="padding-bottom: 20px; font-family: 微軟正黑體">
            <div class="col-12 text-right">
                <asp:Button ID="btn_back" runat="server" Text="上一頁" Class="btn btn-outline-dark btn-sm" OnClick="btn_back_Click"  />
            </div>
        </div>

        <div class="row justify-content-center" style="font-family: 微軟正黑體">
            <div class="col-12">
                <asp:DetailsView ID="dv_store_detail" runat="server" Class=" table table-bordered table-hover">
                    <FieldHeaderStyle HorizontalAlign="Left" BackColor="Silver" Width="100px"/>
                     <Fields>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        優惠內容
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%-- 201014_Betty：圖片放大顯示。BY PEGGY --%>
                                       <asp:HyperLink ID="hy_img" runat="server" data-fancybox data-caption="">
                                            <asp:Image ID="img_store_detail" runat="server" Width="600px" />
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                    <RowStyle HorizontalAlign="Left" />
                </asp:DetailsView>
            </div>
        </div>
    </div>

</asp:Content>

