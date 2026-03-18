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
                <form id="contactForm">
                    <div class="form-group">
                        <label for="name">Name</label>
                        <input type="text" id="name" name="name" required>
                    </div>
                    <div class="form-group">
                        <label for="email">Email</label>
                        <input type="email" id="email" name="email" required>
                    </div>
                    <div class="form-group">
                        <label for="message">Message</label>
                        <textarea id="message" name="message" rows="5" required></textarea>
                    </div>
                    <button type="submit" class="btn-gold">Send Message</button>
                </form>
            </div>
            <div class="contact-image">
                <img src="https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?ixlib=rb-4.0.3&auto=format&fit=crop&w=600&q=80" alt="Recording Studio">
            </div>
        </div>
    </div>
</section>
</asp:Content>
