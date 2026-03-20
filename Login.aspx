<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Music_Studio_Booking.WebForm5" %>

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
                            <asp:TextBox runat="server" type="email" ID="loginEmail"> </asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="loginPassword">Password</label>
                            <asp:TextBox runat="server" type="password" ID="loginPassword" TextMode="Password">
                            </asp:TextBox>
                        </div>
                        <asp:Button runat="server" ID="btnlogin" type="submit" class="btn-gold" OnClick="btnlogin_Click"
                            Text="Login" />
                        <asp:Label ID="lblLoginError" runat="server" ForeColor="Red"></asp:Label>
                    </form>
                    <p>Don't have an account? <a href="Signup.aspx">Sign up here</a></p>
                </div>


                <div style="margin-top: 10px;">
                    <a href="ForgotPassword.aspx" style="color: #ffc107; font-size: 0.9em;">Forgot Password?</a>
                </div>
            </div>

        </section>
    </asp:Content>