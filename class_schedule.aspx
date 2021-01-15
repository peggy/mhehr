<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="class_schedule.aspx.cs" Inherits="class_schedule" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>排班班表</title>
    <script src="packages/footable/js/footable.min.js"></script>
    <link href="packages/footable/css/footable.bootstrap.min.css" rel="stylesheet" />
    <style type="text/css">
        .container-fluid {
            padding-left: 15%;
            padding-right: 15%;
        }
        .auto-style1 {
            left: -184px;
            top: -863px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container-fluid">
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
            <h2>
                <asp:Label ID="lb_title" runat="server" Text="排班班表"></asp:Label></h2>
        </div>
        <div class="row justify-content-center" style="padding-top: 20px;">
            <div class="col-12 col-lg-1"></div>
            <div class="col-12 col-lg-11">
                <asp:Label ID="lb_class_title" runat="server" Text="請選擇班別："></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
            <div class="col-12 col-lg-1"></div>
            <div class="col-12 col-lg-2">
                <asp:DropDownList ID="ddl_daily_shift" runat="server" Class="btn btn-sm btn-outline-secondary" OnSelectedIndexChanged="ddl_daily_shift_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem>請選擇</asp:ListItem>
                    <asp:ListItem>常日</asp:ListItem>
                    <asp:ListItem>輪班工程師</asp:ListItem>
                    <asp:ListItem>輪班產線</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-12 col-lg-3 text-left">
                <asp:RadioButtonList ID="rbl_shift" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="DA">&nbsp;DA&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem Value="DB">&nbsp;DB&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem Value="DC">&nbsp;DC&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem Value="NA">&nbsp;NA&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem Value="NB">&nbsp;NB&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem Value="NC">&nbsp;NC&nbsp;&nbsp;</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="col-12 col-lg-5 text-left">
                <asp:Label ID="lb_shift_start_day" runat="server" Text="當月第一天為： "></asp:Label>
                <asp:DropDownList ID="ddl_shift_start_day" runat="server" Class="btn btn-sm btn-outline-secondary">
                    <asp:ListItem>請選擇</asp:ListItem>
                    <asp:ListItem Value="1">1 上班</asp:ListItem>
                    <asp:ListItem Value="2">2 上班</asp:ListItem>
                    <asp:ListItem Value="3">3 上班</asp:ListItem>
                    <asp:ListItem Value="4">4 上班</asp:ListItem>
                    <asp:ListItem Value="5">5 休息</asp:ListItem>
                    <asp:ListItem Value="6">6 休息</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="row justify-content-center" style="padding-top: 20px;">
            <div class="col-12 col-lg-1"></div>
            <div class="col-12 col-lg-11">
                <asp:Label ID="Label2" runat="server" Text="請選擇部門："></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center">
            <div class="col-12 col-lg-1 "></div>
            <div class="col-12 col-lg-2 " style="padding-top: 20px;">
                <asp:CheckBox ID="cb_dept_all" runat="server" Text="全選" AutoPostBack="True" OnCheckedChanged="cb_dept_all_CheckedChanged" Visible="False" />
               
            </div>
            <div class="col-12 col-lg-9 text-left">
                <asp:CheckBoxList ID="cbl_daily_dept" runat="server" RepeatDirection="Horizontal" CellPadding="5" CellSpacing="10" RepeatColumns="6" Visible="False">
                    <asp:ListItem Value="11">&nbsp;11 業務課</asp:ListItem>
                    <asp:ListItem Value="12">&nbsp;12 會計課</asp:ListItem>
                    <asp:ListItem Value="13">&nbsp;13 人事課</asp:ListItem>
                    <asp:ListItem Value="14">&nbsp;14 資訊課</asp:ListItem>
                    <asp:ListItem Value="15">&nbsp;15 物料課</asp:ListItem>
                    <asp:ListItem Value="16">&nbsp;16 安環課</asp:ListItem>
                    <asp:ListItem Value="1701">&nbsp;1701 生產部</asp:ListItem>
                    <asp:ListItem Value="1711">&nbsp;1711 工程課</asp:ListItem>
                    <asp:ListItem Value="1721">&nbsp;1721 製程課</asp:ListItem>
                    <asp:ListItem Value="1731">&nbsp;1731 製造課</asp:ListItem>
                    <asp:ListItem Value="1741">&nbsp;1741 倉管課</asp:ListItem>
                    <asp:ListItem Value="1801">&nbsp;1801 品管課</asp:ListItem>
                </asp:CheckBoxList>
                 <asp:CheckBoxList ID="cbl_shift_dept_1" runat="server" RepeatDirection="Horizontal" CellPadding="5" CellSpacing="10" RepeatColumns="5" Visible="False">
                    <asp:ListItem Value="1712">&nbsp;1712 工程課輪班</asp:ListItem>
                    <asp:ListItem Value="1722">&nbsp;1722 製程課輪班</asp:ListItem>
                </asp:CheckBoxList>
                 <asp:CheckBoxList ID="cbl_shift_dept_2" runat="server" RepeatDirection="Horizontal" CellPadding="5" CellSpacing="10" RepeatColumns="5" Visible="False">
                    <asp:ListItem Value="1732">&nbsp;1732 製造課輪班</asp:ListItem>
                    <asp:ListItem Value="1742">&nbsp;1742 倉管課輪班</asp:ListItem>
                    <asp:ListItem Value="1802">&nbsp;1802 品管課輪班</asp:ListItem>
                </asp:CheckBoxList>
                <asp:Label ID="lb_dept_daily_remark" runat="server" Text="*人事課已排除警衛人員" ForeColor="Gray" Visible="False" Font-Size="Small"></asp:Label>
                <asp:Label ID="lb_dept_shift_remark" runat="server" Text="*工程課輪班包含設備課及廠務課人員" ForeColor="Gray" Visible="False" Font-Size="Small"></asp:Label>
            </div>
        </div>

        <div class="row justify-content-center" style="padding-top: 2%;">
            <div class="col-12 col-lg-1"></div>
            <div class="col-12 col-lg-11">
                <asp:Label ID="lb_date_title" runat="server" Text="請選擇日期區間："></asp:Label>
            </div>
        </div>
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
            <div class="col-12 col-lg-1 text-left">
                <asp:Label ID="lb_cs_start_date" runat="server" Text="開始日期"></asp:Label>
            </div>
            <div class="col-12 col-lg-3 text-left">
                <asp:TextBox ID="tb_cs_start_date" runat="server" Class="form-control" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-1"></div>
            <div class="col-12 col-lg-1 text-left">
                <asp:Label ID="lb_cs_end_date" runat="server" Text="結束日期"></asp:Label>
            </div>
            <div class="auto-style1">
                <asp:TextBox ID="tb_cs_end_date" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col-12 col-lg-1"></div>
        </div>
        <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
            <asp:Button ID="btn_export" runat="server" Text="匯出" Class="btn btn-sm btn-outline-secondary" OnClick="btn_export_Click" />
        </div>
        <div class="border">
            <div class="row justify-content-center">
                <div class="col text-center" style="padding-top: 1%;">
                    <h5><asp:Label ID="lb_calendar_title" runat="server" Text=""></asp:Label></h5>
                </div>
            </div>
            <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
                <div class="col text-right">
                    <asp:Label ID="lb_import" runat="server" Text="上傳行事曆：" Style="padding-top: 5px;"></asp:Label>
                    <asp:FileUpload ID="fu_import" runat="server" Class="btn btn-sm btn-light" />
                    <asp:Button ID="btn_import" runat="server" Text="上傳" Class="btn btn-sm btn-outline-secondary" OnClick="btn_import_Click"/>
                </div>
            </div>
            <div class="row justify-content-center" style="padding-top: 20px; padding-bottom: 20px">
                <div class="col" style="padding-left:5%; padding-right:5%">
                    <asp:GridView ID="gv_calendar" runat="server" Class="table table-bordered table-hover"></asp:GridView>
                </div>
            </div>
        </div>

    </div>
</asp:Content>

