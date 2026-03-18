<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Music_Studio_Booking.WebForm5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
    <div class="hero-content">
        <h1>Login</h1>
        <p>Access your account</p>
    </div>
</section>

<section class="form-section">
    <div class="container">
        <div class="form-container">
            <h2>Login to your account</h2>
            <form id="loginForm">
                <div class="form-group">
                    <label for="loginEmail">Email</label>
                    <input type="email" id="loginEmail" name="email" required>
                </div>
                <div class="form-group">
                    <label for="loginPassword">Password</label>
                    <input type="password" id="loginPassword" name="password" required>
                </div>
                <button type="submit" class="btn-gold">Login</button>
            </form>
            <p>Don't have an account? <a href="signup.html">Sign up here</a></p>
        </div>
    </div>
</section>
</asp:Content>
