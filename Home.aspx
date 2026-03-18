<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Music_Studio_Booking.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="home" class="hero">
    <div class="hero-content">
        <h1>Welcome to G-Studio</h1>
        <p class="hero-subtitle">Your Student-Built Music Studio Booking Platform</p>
        <p class="hero-description">Created by Mapúa students, for students. Book professional recording studios, rent instruments, and bring your musical ideas to life with our affordable, creator-focused platform.</p>
        <div class="hero-buttons">
            <a href="studios.aspx" class="btn btn-primary">Explore Studios</a>
            <a href="booking.aspx" class="btn btn-secondary">Book Now</a>
        </div>
    </div>
</section>

<section class="features">
    <div class="container">
        <h2>Why Choose G-Studio?</h2>
        <div class="features-grid">
            <div class="feature-item">
                <h3>🎓 Student-Built Platform</h3>
                <p>Created by Mapúa students for students – we understand your needs and budget constraints.</p>
            </div>
            <div class="feature-item">
                <h3>💰 Student-Friendly Pricing</h3>
                <p>Affordable rates designed specifically for students, musicians, and emerging artists.</p>
            </div>
            <div class="feature-item">
                <h3>🎵 Professional Equipment</h3>
                <p>Access to high-quality recording studios and instruments for your creative projects.</p>
            </div>
            <div class="feature-item">
                <h3>👥 Community Focused</h3>
                <p>Join a growing community of student artists, bands, and content creators.</p>
            </div>
            <div class="feature-item">
                <h3>📅 Easy Booking</h3>
                <p>Simple online booking system with real-time availability and instant confirmation.</p>
            </div>
            <div class="feature-item">
                <h3>🎯 Creator Support</h3>
                <p>Perfect for music production, podcasting, content creation, and band rehearsals.</p>
            </div>
        </div>
    </div>
</section>

<section id="studios" class="featured-studios">
    <div class="container">
        <h2>Featured Studios</h2>
        <div class="studios-grid">
            <div class="studio-card">
                <img src="https://via.placeholder.com/300x200?text=Studio+A" alt="Studio A">
                <h3>Studio A – Modern Recording Room</h3>
                <p>Equipped with the latest recording technology for high-quality audio production.</p>
            </div>
            <div class="studio-card">
                <img src="https://via.placeholder.com/300x200?text=Studio+B" alt="Studio B">
                <h3>Studio B – Vocal Booth & Mixing Gear</h3>
                <p>Perfect for vocal recordings and mixing sessions with professional gear.</p>
            </div>
            <div class="studio-card">
                <img src="https://via.placeholder.com/300x200?text=Studio+C" alt="Studio C">
                <h3>Studio C – Large Band Room</h3>
                <p>Spacious room ideal for band recordings and live sessions.</p>
            </div>
        </div>
        <a href="Studios.aspx" class="btn-gold">View All Studios</a>
    </div>
</section>

<section class="how-it-works">
    <div class="container">
        <h2>How It Works</h2>
        <div class="steps-grid">
            <div class="step-card">
                <h3>1. Choose Your Studio</h3>
                <p>Browse and select from our range of professional studios.</p>
            </div>
            <div class="step-card">
                <h3>2. Pick Date & Time</h3>
                <p>Select your preferred date and time slot.</p>
            </div>
            <div class="step-card">
                <h3>3. Confirm Your Booking</h3>
                <p>Complete the booking and get confirmation instantly.</p>
            </div>
        </div>
    </div>
</section>
</asp:Content>
