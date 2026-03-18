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
                    <asp:TextBox runat="server" type="text" id="signupName"> </asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="signupEmail">Email</label>
                    <asp:TextBox runat="server" type="email" id="signupEmail"> </asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="signupPassword">Password</label>
                    <asp:TextBox runat="server"  type="password" id="signupPassword"> </asp:TextBox>
                </div>
                <div class="form-group">
                    <label for="confirmPassword">Confirm Password</label>
                    <asp:TextBox runat="server" type="password" id="signupconfirmPassword"> </asp:TextBox>
                </div>
                <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn-gold" OnClick="btnSignUp_Click" />

                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
            </form>
            <p>Already have an account? <a href="login.aspx">Login here</a></p>
        </div>
    </div>
</section>
</asp:Content>
