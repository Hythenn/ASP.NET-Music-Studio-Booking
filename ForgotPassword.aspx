<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Music_Studio_Booking.WebForm8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<section class="form-section">
    <div class="container">
        <div class="form-container">
            <h2>Reset Password</h2>
            
            <div class="form-group">
                <label>Enter your Registered Email</label>
                <asp:TextBox ID="txtResetEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="email@example.com"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>Security Answer</label>
                <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="form-control" placeholder="Your answer from signup"></asp:TextBox>
            </div>

            <div class="form-group">
                <label>New Password</label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Minimum 6 characters"></asp:TextBox>
            </div>

            <asp:Button ID="btnReset" runat="server" Text="Update Password" CssClass="btn-gold" OnClick="btnReset_Click" />
            <br />
            <asp:Label ID="lblResetStatus" runat="server" CssClass="status-message"></asp:Label>
            <p><a href="Login.aspx">Back to Login</a></p>
        </div>
    </div>
</section>
</asp:Content>
