<%@ Page Title="Book a Studio" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Music_Studio_Booking.WebForm7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Book Your Studio</h1>
            <p>Select your preferred studio and time</p>
        </div>
    </section>

    <section class="form-section">
        <div class="container">
            <div class="form-container">
                <h2>Studio Booking Form</h2>

                <div class="form-group">
                    <label for="ddlStudio">Select Studio</label>
                    <asp:DropDownList ID="ddlStudio" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="CalculateTotal">
                        <asp:ListItem Value="">Choose a studio</asp:ListItem>
                        <asp:ListItem Value="studio-a">Studio A – Modern Recording Room (P300/hour)</asp:ListItem>
                        <asp:ListItem Value="studio-b">Studio B – Vocal Booth & Mixing Gear (P600/hour)</asp:ListItem>
                        <asp:ListItem Value="studio-c">Studio C – Large Band Room (P900/hour)</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="form-group">
                    <label for="txtDate">Date</label>
                    <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control" required="required"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>Select Time Slots</label>
                    <div class="time-grid">
                        <asp:CheckBoxList ID="cblTime" runat="server"
                            RepeatLayout="Flow"
                            RepeatDirection="Horizontal"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="CalculateTotal">
                            <asp:ListItem Value="09:00">9:00 AM</asp:ListItem>
                            <asp:ListItem Value="10:00">10:00 AM</asp:ListItem>
                            <asp:ListItem Value="11:00">11:00 AM</asp:ListItem>
                            <asp:ListItem Value="12:00">12:00 PM</asp:ListItem>
                            <asp:ListItem Value="13:00">1:00 PM</asp:ListItem>
                            <asp:ListItem Value="14:00">2:00 PM</asp:ListItem>
                            <asp:ListItem Value="15:00">3:00 PM</asp:ListItem>
                            <asp:ListItem Value="16:00">4:00 PM</asp:ListItem>
                            <asp:ListItem Value="17:00">5:00 PM</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </div>

                <div class="form-group">
                    <label>Optional Instrument Rentals</label>
                    <div class="instrument-grid">
                        <asp:CheckBoxList ID="cblInstruments" runat="server"
                            CssClass="instrument-grid"
                            RepeatLayout="Table"
                            RepeatColumns="1"
                            RepeatDirection="Vertical"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="CalculateTotal">
                        </asp:CheckBoxList>
                    </div>
                </div>

                <div class="price-summary">
                    <h3>Current Total:
                        <asp:Label ID="lblTotalPriceDisplay" runat="server" Text="P0.00"></asp:Label></h3>
                </div>

                <asp:Button ID="btnRequestBooking" runat="server" Text="Request Booking" CssClass="btn-gold" OnClick="btnRequestBooking_Click" />

                <div style="margin-top: 20px; text-align: center;">
                    <asp:Label ID="lblStatus" runat="server" CssClass="status-message"></asp:Label>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
