<%@ Page Title="" Language="C#" MasterPageFile="~/Navigation.Master" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="Music_Studio_Booking.WebForm7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <section class="page-hero">
       <div class="hero-content">
           <h1>Book Your Studio</h1>
           <p>Select your preferred studio and time</p>
       </div>
   </section>

   <section class="form-section">
       <div class="container">
           <div class="form-container">
               <h2>Studio Booking Form</h2>
               <form id="bookingForm">
                   <div class="form-group">
                       <label for="studio">Select Studio</label>
                       <select id="studio" name="studio" required>
                           <option value="">Choose a studio</option>
                           <option value="studio-a">Studio A – Modern Recording Room</option>
                           <option value="studio-b">Studio B – Vocal Booth & Mixing Gear</option>
                           <option value="studio-c">Studio C – Large Band Room</option>
                       </select>
                   </div>
                   <div class="form-group">
                       <label for="date">Date</label>
                       <input type="date" id="date" name="date" required>
                   </div>
                   <div class="form-group">
                       <label for="time">Time</label>
                       <select id="time" name="time" required>
                           <option value="">Select time</option>
                           <option value="9:00">9:00 AM</option>
                           <option value="10:00">10:00 AM</option>
                           <option value="11:00">11:00 AM</option>
                           <option value="12:00">12:00 PM</option>
                           <option value="13:00">1:00 PM</option>
                           <option value="14:00">2:00 PM</option>
                           <option value="15:00">3:00 PM</option>
                           <option value="16:00">4:00 PM</option>
                           <option value="17:00">5:00 PM</option>
                           <option value="18:00">6:00 PM</option>
                       </select>
                   </div>
                   <button type="submit" class="btn-gold">Confirm Booking</button>
               </form>
           </div>
       </div>
   </section>
</asp:Content>
