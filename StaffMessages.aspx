<%@ Page Title="Staff - Contact Messages" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true"
    CodeBehind="StaffMessages.aspx.cs" Inherits="Music_Studio_Booking.StaffMessages" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Contact Messages</h1>
            <p>Messages submitted by users through the Contact page.</p>
        </div>
    </section>

    <section class="staff-section">
        <div class="container">
            <div class="staff-header">
                <h2>Inbox</h2>
            </div>

            <asp:Panel ID="PanelEmpty" runat="server" CssClass="empty-state" Visible="false">
                <h3>No messages yet</h3>
                <p>Messages from the Contact page will appear here.</p>
            </asp:Panel>

            <asp:Repeater ID="rptMessages" runat="server">
                <HeaderTemplate>
                    <div class="requests-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="request-card request-pending">
                        <div class="request-main" onclick="this.parentElement.classList.toggle('expanded');">
                            <div class="request-meta">
                                <span class="request-time"><%# Eval("SubmittedAtDisplay") %></span>
                            </div>
                            <h3 class="request-title">
                                <%# Eval("SenderName") %> &bull; <%# Eval("SenderEmail") %>
                            </h3>
                            <p class="request-summary">
                                <%# Eval("Preview") %>
                            </p>
                        </div>
                        <div class="request-details">
                            <div class="request-detail-grid">
                                <div>
                                    <h4>Sender</h4>
                                    <p><strong>Name:</strong> <%# Eval("SenderName") %></p>
                                    <p><strong>Email:</strong> <%# Eval("SenderEmail") %></p>
                                </div>
                                <div>
                                    <h4>Message</h4>
                                    <p><%# Eval("UserMessage") %></p>
                                </div>
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
