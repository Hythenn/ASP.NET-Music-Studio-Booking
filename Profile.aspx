<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Music_Studio_Booking.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Profile</h1>
            <p>Basic account details.</p>
        </div>
    </section>

    <section class="staff-section">
        <div class="container">
            <div class="profile-card">
                <h2>Account Details</h2>

                <div class="profile-row">
                    <span class="profile-label">Name</span>
                    <span class="profile-value"><asp:Label ID="lblName" runat="server" /></span>
                </div>

                <div class="profile-row">
                    <span class="profile-label">Email</span>
                    <span class="profile-value"><asp:Label ID="lblEmail" runat="server" /></span>
                </div>

                <div class="profile-row">
                    <span class="profile-label">User ID</span>
                    <span class="profile-value"><asp:Label ID="lblUserId" runat="server" /></span>
                </div>

                <div class="profile-actions">
                    <a class="profile-home-link" href="Home.aspx">Back to Home</a>
                    <a class="profile-logout-link" href="Logout.aspx">Logout</a>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

