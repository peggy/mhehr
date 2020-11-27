<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage_home.master" AutoEventWireup="true" CodeFile="extension_edit.aspx.cs" Inherits="extension_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=yes" />
    <style type="text/css">
        hr {
            width: 30%;
        }

        .container-fluid {
            padding-left: 20%;
            padding-right: 20%;
        }

        .col {
            padding-top: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row justify-content-center" style="padding-top: 20px;">
        <h2>
            <asp:Label ID="lb_title" runat="server" Text="明徽能源分機表"></asp:Label></h2>
    </div>
    <hr />
    <div class="row justify-content-center " style="padding-top: 1px; padding-bottom: 10px">
        <div class="col-12 text-center">
            <h2>編輯介面</h2>
        </div>
    </div>

    <div class="container-fluid">

        <div class="row justify-content-center">
            <div class="col">
            <asp:Button ID="btn_insert" runat="server" Text="新增" class="btn btn-outline-dark" OnClick="btn_insert_Click" />
            </div>
        </div>
        <div class="row justify-content-center text-center">
            <div class="col text-center">人員序號</div>
            <div class="col text-center">部門序號</div>
            <div class="col text-center">分機</div>
            <div class="col text-center">類別</div>
        </div>

        <div class="row justify-content-center text-center">
            <div class="col text-center">
                <asp:TextBox ID="tb_id_name" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col text-center">
                <asp:TextBox ID="tb_id_dept" runat="server" Class="form-control"></asp:TextBox>
            </div>
            <div class="col text-center">
                <asp:TextBox ID="tb_ext" runat="server" Class="form-control"></asp:TextBox>
            </div>

            <div class="col text-center">
                <asp:TextBox ID="tb_class" runat="server" Class="form-control"></asp:TextBox>
            </div>


        </div>

        <div class="row justify-content-center">
            <div class="col text-center">姓名</div>
            <div class="col text-center">部門</div>
            <div class="col text-center">手機簡碼</div>
            <div class="col text-center"></div>
        </div>

        <div class="row justify-content-center">
            <div class="col text-center">
                <asp:TextBox ID="tb_ext_name" runat="server" Class="form-control" AutoPostBack="True" OnTextChanged="tb_ext_name_TextChanged"></asp:TextBox>
            </div>
            <div class="col text-center">
                <asp:DropDownList ID="ddl_dept" runat="server" Class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
                    <asp:ListItem Value="%">請選擇</asp:ListItem>
                </asp:DropDownList>
                <asp:RadioButtonList ID="rbl_dept" runat="server" RepeatDirection="Horizontal" Visible="False" AutoPostBack="True" Class="form-control border-0" OnSelectedIndexChanged="rbl_dept_SelectedIndexChanged">
                    <asp:ListItem>&nbsp;常日&nbsp;&nbsp;</asp:ListItem>
                    <asp:ListItem>&nbsp;輪班</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Label ID="lb_remark" runat="server" Text="備註：類別 常日=B；輪班=無" Font-Size="Small" ForeColor="Gray" Visible="False"></asp:Label>
            </div>
            <div class="col text-center">
                <asp:TextBox ID="tb_ext_phone" runat="server" Class="form-control" AutoPostBack="True" OnTextChanged="tb_ext_phone_TextChanged"></asp:TextBox>
            </div>
            <div class="col text-center"></div>
        </div>
        <div class="row justify-content-center">
            <div class="col">
                <asp:GridView ID="gv_extension_edit" runat="server" class="table table-hover table-bordered text-center" DataKeyNames="ext_name" OnRowCancelingEdit="gv_extension_edit_RowCancelingEdit" OnRowDataBound="gv_extension_edit_RowDataBound" OnRowDeleting="gv_extension_edit_RowDeleting" OnRowEditing="gv_extension_edit_RowEditing" OnRowUpdating="gv_extension_edit_RowUpdating">
                <Columns>
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:Button ID="btn_update" runat="server" CommandName="Update" Text="更新" />
                            &nbsp;<asp:Button ID="btn_cancel" runat="server" CommandName="cancel" Text="取消" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Button ID="btn_edit" runat="server" CommandName="Edit" Text="編輯" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_delete" runat="server" CommandName="Delete" Text="刪除" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
            
        </div>
    </div>

</asp:Content>

