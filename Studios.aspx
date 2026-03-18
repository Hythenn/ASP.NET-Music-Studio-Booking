<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Studios.aspx.cs" Inherits="Music_Studio_Booking.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="page-hero">
        <div class="hero-content">
            <h1>Our Studios</h1>
            <p>Explore our professional recording spaces</p>
        </div>
    </section>

    <section class="studios-section">
        <div class="container">
            <div class="studios-intro">
                <h2>Our Studio Rooms</h2>
                <p>Choose from our range of professional recording spaces, each designed to meet different creative needs. All rooms feature excellent soundproofing, quality acoustics, and professional recording capabilities.</p>
            </div>

            <div class="studios-grid">
                <div class="studio-card">
                    <img src="https://images.unsplash.com/photo-1598488035139-bdbb2231ce04?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80" alt="Basic Studio">
                    <div class="studio-category">Basic Studio</div>
                    <h3>Studio A – Solo Practice Room</h3>
                    <div class="studio-details">
                        <p><strong>📐 Dimensions:</strong> 3m x 3m to 4m x 4m</p>
                        <p><strong>🎵 Ideal for:</strong> solo practice, vocal recording, or small jam sessions</p>
                        <p><strong>🔧 Features:</strong> basic soundproofing and essential equipment</p>
                        <p><strong>💰 Perfect for:</strong> affordable option for students</p>
                    </div>
                    <a href="booking.aspx" class="btn btn-primary">Book Now</a>
                </div>
                <div class="studio-card">
                    <img src="https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80" alt="Standard Studio">
                    <div class="studio-category">Standard Studio</div>
                    <h3>Studio B – Band Rehearsal Room</h3>
                    <div class="studio-details">
                        <p><strong>📐 Dimensions:</strong> 5m x 5m to 6m x 6m</p>
                        <p><strong>🎵 Ideal for:</strong> band rehearsals and music recording</p>
                        <p><strong>🔧 Features:</strong> improved acoustics and better recording equipment</p>
                        <p><strong>💰 Perfect for:</strong> balanced option for musicians who need more space</p>
                    </div>
                    <a href="booking.aspx" class="btn btn-primary">Book Now</a>
                </div>
                <div class="studio-card">
                    <img src="https://images.unsplash.com/photo-1516450360452-9312f5e86fc7?ixlib=rb-4.0.3&auto=format&fit=crop&w=400&q=80" alt="Premium Studio">
                    <div class="studio-category">Premium Studio</div>
                    <h3>Studio C – Professional Recording Suite</h3>
                    <div class="studio-details">
                        <p><strong>📐 Dimensions:</strong> 7m x 7m to 8m x 10m</p>
                        <p><strong>🎵 Ideal for:</strong> professional-level recording environment</p>
                        <p><strong>🔧 Features:</strong> advanced soundproofing and acoustic treatment</p>
                        <p><strong>💰 Perfect for:</strong> suitable for full band recordings, music production, and professional sessions</p>
                    </div>
                    <a href="booking.aspx" class="btn btn-primary">Book Now</a>
                    
                </div>
            </div>

            <div class="instruments-section">
                <h2>Instruments & Equipment for Rent</h2>
                <p>Enhance your session with our well-maintained instruments and professional equipment. All instruments are well-maintained and available for rent during studio bookings.</p>

                <div class="instruments-grid">
                    <div class="instrument-item">
                        <h4>🎸 Electric Guitar</h4>
                        <p>suitable for rock, pop, and recording sessions</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🎸 Bass Guitar</h4>
                        <p>ideal for rhythm and band rehearsals</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🥁 Drum Set</h4>
                        <p>full acoustic drum kit for band practice</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🎹 Keyboard / MIDI Keyboard</h4>
                        <p>useful for music production and composing</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🎤 Studio Microphones</h4>
                        <p>condenser and dynamic microphones for recording</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🔊 Guitar Amplifiers</h4>
                        <p>amplifiers for electric guitars and bass</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🎧 Audio Interface</h4>
                        <p>connects instruments to recording software</p>
                    </div>
                    <div class="instrument-item">
                        <h4>🎧 Studio Headphones</h4>
                        <p>for monitoring during recording sessions</p>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
