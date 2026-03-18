<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Music_Studio_Booking.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
    <div class="hero-content">
        <h1>Sign Up</h1>
        <p>Create your account</p>
    </div>
</section>

<section class="form-section">
    <div class="container">
        <div class="form-container">
            <h2>Join Music Studio Booking</h2>
            <form id="signupForm">
                <div class="form-group">
                    <label for="signupName">Name</label>
                    <input type="text" id="signupName" name="name" required>
                </div>
                <div class="form-group">
                    <label for="signupEmail">Email</label>
                    <input type="email" id="signupEmail" name="email" required>
                </div>
                <div class="form-group">
                    <label for="signupPassword">Password</label>
                    <input type="password" id="signupPassword" name="password" required>
                </div>
                <div class="form-group">
                    <label for="confirmPassword">Confirm Password</label>
                    <input type="password" id="confirmPassword" name="confirmPassword" required>
                </div>
                <button type="submit" class="btn-gold">Sign Up</button>
            </form>
            <p>Already have an account? <a href="login.html">Login here</a></p>
        </div>
    </div>
</section>
</asp:Content>
