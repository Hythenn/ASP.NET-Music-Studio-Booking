<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="StaffInstruments.aspx.cs" Inherits="Music_Studio_Booking.StaffInstruments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="staff-section">
        <div class="container">
            <div class="staff-header">
                <h2>Inventory Management</h2>
                <p>Add new equipment or update current stock levels.</p>
            </div>

            <div class="form-group" style="flex: 2; margin-bottom: 0;">
                <label>Instrument Name</label>
                <asp:TextBox ID="txtNewName" runat="server" CssClass="form-control" placeholder="e.g. Violin"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfdtxtNewName" runat="server" ErrorMessage="Required"
                    ControlToValidate="txtNewName" ValidationGroup="AddGear" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group" style="flex: 1; margin-bottom: 0;">
                <label>Price (P)</label>
                <asp:TextBox ID="txtNewPrice" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfdtxtNewPrice" runat="server" ErrorMessage="Required"
                    ControlToValidate="txtNewPrice" ValidationGroup="AddGear" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group" style="flex: 1; margin-bottom: 0;">
                <label>Total Stock</label>
                <asp:TextBox ID="txtNewQty" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfdtxtNewQty" runat="server" ErrorMessage="Required"
                    ControlToValidate="txtNewQty" ValidationGroup="AddGear" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <asp:Button ID="btnAddInstrument" runat="server" Text="Add Gear" CssClass="btn-gold"
                OnClick="btnAddInstrument_Click" ValidationGroup="AddGear" Style="width: auto; height: 50px;" />

            <div class="schedule-day-card">
                <asp:GridView ID="gvInstruments" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="InstrumentID" CssClass="bookings-grid" GridLines="None"
                    OnRowEditing="gvInstruments_RowEditing"
                    OnRowCancelingEdit="gvInstruments_RowCancelingEdit"
                    OnRowUpdating="gvInstruments_RowUpdating"
                    OnRowDeleting="gvInstruments_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="InstrumentName" HeaderText="Instrument" ControlStyle-CssClass="form-control" />
                        <asp:BoundField DataField="RentalPrice" HeaderText="Price (P)" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="AvailableQuantity" HeaderText="In Stock" />
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True"
                            EditImageUrl="~/images/edit.png" DeleteImageUrl="~/images/delete.png"
                            ControlStyle-CssClass="cancel-link" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>
