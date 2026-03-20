<%@ Page Title="Staff - Booking Requests" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="StaffRequests.aspx.cs" Inherits="Music_Studio_Booking.StaffRequests" %>
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

            <asp:Panel ID="PanelEmpty" runat="server" CssClass="empty-state" Visible="false">
                <h3>No booking requests right now</h3>
                <p>New requests will show up here as soon as customers submit them.</p>
            </asp:Panel>

            <asp:Repeater ID="rptRequests" runat="server" OnItemCommand="rptRequests_ItemCommand">
                <HeaderTemplate>
                    <div class="requests-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="request-card <%# Eval("StatusCss") %>">
                        <div class="request-main" onclick="this.parentElement.classList.toggle('expanded');">
                            <div class="request-meta">
                                <span class="request-time"><%# Eval("RequestedAtDisplay") %></span>
                                <span class="request-status-badge"><%# Eval("Status") %></span>
                            </div>
                            <h3 class="request-title">
                                <%# Eval("CustomerName") %> • <%# Eval("StudioName") %>
                            </h3>
                            <p class="request-summary">
                                <%# Eval("DateDisplay") %> • <%# Eval("TimeRange") %>
                            </p>
                        </div>
                        <div class="request-details">
                            <div class="request-detail-grid">
                                <div>
                                    <h4>Customer</h4>
                                    <p><strong>Name:</strong> <%# Eval("CustomerName") %></p>
                                    <p><strong>Email:</strong> <%# Eval("CustomerEmail") %></p>
                                    <p><strong>Phone:</strong> <%# Eval("CustomerPhone") %></p>
                                </div>
                                <div>
                                    <h4>Booking</h4>
                                    <p><strong>Studio:</strong> <%# Eval("StudioName") %></p>
                                    <p><strong>Date:</strong> <%# Eval("DateDisplay") %></p>
                                    <p><strong>Time:</strong> <%# Eval("TimeRange") %></p>
                                </div>
                                <div>
                                    <h4>Notes</h4>
                                    <p><%# Eval("Notes") %></p>
                                </div>
                            </div>
                            <div class="request-actions">
                                <asp:Button runat="server"
                                            CssClass="btn-request btn-accept"
                                            Text="Accept"
                                            CommandName="Accept"
                                            CommandArgument='<%# Eval("Id") %>' />
                                <asp:Button runat="server"
                                            CssClass="btn-request btn-decline"
                                            Text="Decline"
                                            CommandName="Decline"
                                            CommandArgument='<%# Eval("Id") %>' />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>

