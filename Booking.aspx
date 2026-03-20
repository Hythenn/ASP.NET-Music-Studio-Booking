<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Music_Studio_Booking.WebForm7" %>

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
                <form id="bookingForm">
                    <div class="form-group">
                        <label for="studio">Select Studio</label>
                        <asp:DropDownList ID="ddlStudio" runat="server" CssClass="form-control" required="required">
                            <asp:ListItem Value="">Choose a studio</asp:ListItem>
                            <asp:ListItem Value="studio-a">Studio A – Modern Recording Room</asp:ListItem>
                            <asp:ListItem Value="studio-b">Studio B – Vocal Booth & Mixing Gear</asp:ListItem>
                            <asp:ListItem Value="studio-c">Studio C – Large Band Room</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label for="date">Date</label>
                        <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control" required="required"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="time">Time</label>
                        <asp:DropDownList ID="ddlTime" runat="server" CssClass="form-control" required="required">
                            <asp:ListItem Value="">Select time</asp:ListItem>
                            <asp:ListItem Value="9:00">9:00 AM</asp:ListItem>
                            <asp:ListItem Value="10:00">10:00 AM</asp:ListItem>
                            <asp:ListItem Value="11:00">11:00 AM</asp:ListItem>
                            <asp:ListItem Value="12:00">12:00 PM</asp:ListItem>
                            <asp:ListItem Value="13:00">1:00 PM</asp:ListItem>
                            <asp:ListItem Value="14:00">2:00 PM</asp:ListItem>
                            <asp:ListItem Value="15:00">3:00 PM</asp:ListItem>
                            <asp:ListItem Value="16:00">4:00 PM</asp:ListItem>
                            <asp:ListItem Value="17:00">5:00 PM</asp:ListItem>
                            <asp:ListItem Value="18:00">6:00 PM</asp:ListItem>
                            <asp:ListItem Value="19:00">7:00 PM</asp:ListItem>
                            <asp:ListItem Value="20:00">8:00 PM</asp:ListItem>
                            <asp:ListItem Value="21:00">9:00 PM</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>Optional Instrument Rentals</label>
                        <div class="instrument-grid">
                            <asp:CheckBoxList ID="cblInstruments" runat="server" CssClass="form-control-checkbox" RepeatDirection="Vertical">
                                <asp:ListItem Value="Electric Guitar">🎸 Electric Guitar</asp:ListItem>
                                <asp:ListItem Value="Bass Guitar">🎸 Bass Guitar</asp:ListItem>
                                <asp:ListItem Value="Drum Set">🥁 Drum Set</asp:ListItem>
                                <asp:ListItem Value="Keyboard">🎹 Keyboard / MIDI Keyboard</asp:ListItem>
                                <asp:ListItem Value="Studio Microphones">🎤 Studio Microphones</asp:ListItem>
                                <asp:ListItem Value="Guitar Amplifiers">🔊 Guitar Amplifiers</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>

                    <asp:Button ID="btnRequestBooking" runat="server" Text="Request Booking" CssClass="btn-gold" OnClick="btnRequestBooking_Click" />
                    <asp:Label ID="lblStatus" runat="server" CssClass="status-message"></asp:Label>
                </form>
            </div>
        </div>
    </section>
</asp:Content>
