<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="OnlineJobPortalProject.Admin.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="background-image: url('../Images/bg.jpg'); width: 100%; height: 720px; background-repeat: no-repeat; background-size: cover; background-attachment: fixed;">
        <div class="container-fluid pt-4 pb-4">
            <div>
                <asp:label ID="lblMsg" runat="server"></asp:label>

            </div>

            <h3 class="text-center">User List/Details</h3>
            <div>


                <div class="row mb-3 pt-sm-3 ">
                    <div class="col-md-12">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-hover table-bordered" HeaderStyle-HorizontalAlign="Center"
                            EmptyDataText="No record to display..!" AutoGenerateColumns="False" AllowPaging="True"
                            PageSize="5" OnPageIndexChanging="GridView1_PageIndexChanging"
                            DataKeyNames="UserId" OnRowDeleting="GridView1_RowDeleting">
                            <columns>

                                <asp:BoundField DataField="Sr.No" HeaderText="Sr.No">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Name" HeaderText="User Name">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Email" HeaderText="Email">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Mobile" HeaderText="Mobile">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Country" HeaderText="Country">
                                    <itemstyle horizontalalign="Center" />
                                </asp:BoundField>



                                <asp:CommandField CausesValidation="false" HeaderText="Delete" ShowDeleteButton="true"
                                    DeleteImageUrl="../assets/img/icon/trashIcon.png" ButtonType="Image">
                                    <controlstyle height="25px" width="25px" />
                                    <itemstyle horizontalalign="Center" />
                                </asp:CommandField>

                            </columns>
                            <headerstyle backcolor="#7200cf" forecolor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
