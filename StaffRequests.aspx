<%@ Page Title="Staff - Booking Requests" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true"
    CodeBehind="StaffRequests.aspx.cs" Inherits="Music_Studio_Booking.StaffRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Booking Requests</h1>
            <p>Review, accept, or decline incoming studio booking requests.</p>
        </div>
    </section>

    <section class="staff-section">
        <div class="container">
            <div class="staff-header">
                <h2>Pending Requests</h2>
                <p>Latest requests appear on top. Click a card to view full details.</p>
            </div>

            <!--SHOW IF NO RESULTS FOUND FROM DATABASE -->
            <asp:Panel ID="PanelEmpty" runat="server" CssClass="empty-state" Visible="false"></asp:Panel>

            <asp:Repeater ID="rptGroups" runat="server">
                <ItemTemplate>
                    <div class="schedule-day-header" style="margin-top: 30px; border-bottom: 2px solid #ffd700; padding-bottom: 5px;">
                        <h3 style="color: #ffd700;"><%# Eval("GroupName") %></h3>
                    </div>

                    <div class="requests-list">
                        <asp:Repeater ID="rptInner" runat="server" DataSource='<%# Eval("Bookings") %>' OnItemCommand="rptRequests_ItemCommand">
                            <ItemTemplate>
                                <div class="request-card <%# Eval("StatusCss") %>">
                                    <div class="request-main" onclick="this.parentElement.classList.toggle('expanded');">
                                        <h3 class="request-title">
                                            <%# Eval("CustomerEmail") %> &bull; <%# Eval("StudioName") %>
                            </h3>
                                        <p class="request-summary">
                                            <strong>Date:</strong> <%# Eval("DateDisplay") %> | <strong>Time:</strong> <%# Eval("BookingTime") %>
                                        </p>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>
