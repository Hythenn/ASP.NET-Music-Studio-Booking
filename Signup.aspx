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
                        <label for="signupName">Full Name</label>
                        <asp:TextBox runat="server" type="text" ID="signupName"> </asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="signupEmail">Email</label>
                        <asp:TextBox runat="server" type="email" ID="signupEmail"> </asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="signupPassword">Password</label>
                        <asp:TextBox runat="server" type="password" ID="signupPassword"> </asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label for="confirmPassword">Confirm Password</label>
                        <asp:TextBox runat="server" type="password" ID="signupconfirmPassword"> </asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label for="ddlSecurityQuestion">Security Question</label>
                        <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-control">
                            <asp:ListItem Value="What was the name of your first pet?">What was the name of your first pet?</asp:ListItem>
                            <asp:ListItem Value="What is your mother's maiden name?">What is your mother's maiden name?</asp:ListItem>
                            <asp:ListItem Value="What was the name of your elementary school?">What was the name of your elementary school?</asp:ListItem>
                            <asp:ListItem Value="In what city were you born?">In what city were you born?</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label for="signupAnswer">Security Answer</label>
                        <asp:TextBox runat="server" ID="signupAnswer" CssClass="form-control" placeholder="Answer here..."></asp:TextBox>
                    </div>
                    <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="btn-gold" OnClick="btnSignUp_Click" />

                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                </form>
                <p>Already have an account? <a href="login.aspx">Login here</a></p>
            </div>
        </div>
    </section>
</asp:Content>
