<%@ Page Title="Staff - Schedule Overview" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="StaffSchedule.aspx.cs" Inherits="Music_Studio_Booking.StaffSchedule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Booking Schedule</h1>
            <p>High-level view of all accepted bookings across studios.</p>
        </div>
    </section>

    <section class="staff-section">
        <div class="container">
            <div class="staff-header">
                <h2>Upcoming Schedule</h2>
                <p>Grouped by date so staff can quickly see who is in which studio.</p>
            </div>


            <!--SHOW IF NO RESULTS FOUND FROM DATABASE -->
            <asp:Panel ID="PanelEmptySchedule" runat="server" CssClass="empty-state" Visible="false"></asp:Panel>

            <asp:Repeater ID="rptSchedule" runat="server" OnItemDataBound="rptSchedule_ItemDataBound">
                <ItemTemplate>
                    <div class="schedule-day-card">
                        <div class="schedule-day-header">
                            <h3><%# Eval("DateDisplay") %></h3>
                            <span class="schedule-count"><%# Eval("CountLabel") %></span>
                        </div>
                        <div class="schedule-bookings">
                            <asp:Repeater ID="rptBookings" runat="server">
                                <ItemTemplate>
                                    <div class="schedule-booking-row">
                                        <div class="schedule-time">
                                            <%# Eval("TimeRange") %>
                                        </div>
                                        <div class="schedule-main">
                                            <strong><%# Eval("StudioName") %></strong>
                                            <span class="schedule-customer"><%# Eval("CustomerName") %></span>
                                        </div>
                                        <div class="schedule-notes">
                                            <%# string.IsNullOrEmpty(Eval("SelectedInstruments").ToString()) ? "No instruments" : Eval("SelectedInstruments").ToString() %>
                                            &nbsp;&bull;&nbsp;
                                            <strong><%# Eval("TotalPriceDisplay") %></strong>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>
</asp:Content>
