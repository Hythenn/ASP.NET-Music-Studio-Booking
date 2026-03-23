<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Music_Studio_Booking.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero" style="height: 40vh;">
        <div class="hero-content">
            <h1>My Profile</h1>
            <p>Manage your account and active studio sessions.</p>
        </div>
    </section>

    <section class="staff-section">
        <div class="container">
            <div class="profile-card">
                <div style="margin-bottom: 2rem;">
                    <h2>Account Information</h2>
                    <div class="profile-row">
                        <span class="profile-label">Name</span>
                        <span class="profile-value">
                            <asp:Label ID="lblName" runat="server" /></span>
                    </div>
                    <div class="profile-row">
                        <span class="profile-label">User ID</span>
                        <span class="profile-value">
                            <asp:Label ID="lblUserId" runat="server" /></span>
                    </div>
                    <div class="profile-row">
                        <span class="profile-label">Email</span>
                        <span class="profile-value">
                            <asp:Label ID="lblEmail" runat="server" /></span>
                    </div>
                    <div class="profile-actions">
                        <a class="profile-home-link" href="Home.aspx">Home</a>
                        <a class="profile-logout-link" href="Logout.aspx">Logout</a>
                    </div>
                </div>

                <hr style="border: 0; border-top: 1px solid rgba(255,215,0,0.1); margin: 2rem 0;" />

                <h2>Your Bookings</h2>
                <asp:Label ID="lblMessage" runat="server" CssClass="status-msg" Visible="false" />

                <asp:GridView ID="gvUserBookings" runat="server" AutoGenerateColumns="False"
                    DataKeyNames="BookingID" OnRowDeleting="gvUserBookings_RowDeleting"
                    GridLines="None" CssClass="bookings-grid">
                    <Columns>
                        <asp:BoundField DataField="StudioRoom" HeaderText="Studio" />

                        <asp:BoundField DataField="BookingDate" HeaderText="Date" DataFormatString="{0:MMM dd, yyyy}" />

                        <asp:BoundField DataField="BookingTime" HeaderText="Selected Time Slots" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Delete"
                                    OnClientClick="return confirm('Cancel this booking?');"
                                    CssClass="cancel-link">Cancel</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </section>
</asp:Content>
