<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Music_Studio_Booking.WebForm4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Contact Us</h1>
            <p>Get in touch with our team</p>
        </div>
    </section>

    <section class="contact-section">
        <div class="container">
            <div class="contact-content">
                <div class="contact-form">
                    <h2>Send us a message</h2>
                    <div class="form-group">
                        <label for="name">Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Your Name" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="email">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="Your Email" required></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="message">Message</label>
                        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" placeholder="How can we help?" required></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" CssClass="btn-gold" OnClick="btnSendMessage_Click" />
                    <br />
                    <asp:Label ID="lblStatus" runat="server" CssClass="status-message"></asp:Label>
                </div>
                <div class="contact-image">
                    <img src="https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80" alt="Recording Studio">
                </div>
            </div>
        </div>
    </section>
</asp:Content>
