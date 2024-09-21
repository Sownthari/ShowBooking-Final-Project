import { Component } from '@angular/core';
import { UserApiService } from '../user-api.service';
import { jwtDecode } from 'jwt-decode';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-booking-history',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './booking-history.component.html',
  styleUrl: './booking-history.component.css'
})
export class BookingHistoryComponent {
  bookings: any[] = []; // Array to store user bookings
  errorMessage: string = '';
  userId: any = 0;

  constructor(private userApiService: UserApiService) {
    var token = localStorage.getItem('token');
    if(token!=null){
      var decodedToken:any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    }
  }

  ngOnInit(): void {
    this.fetchUserBookings();
    
  }

  // Function to fetch bookings
  fetchUserBookings(): void {
    this.userApiService.getBookings(this.userId).subscribe(
      (data: any[]) => {
        this.bookings = data;
      },
      (error) => {
        this.errorMessage = 'Failed to load bookings. Please try again later.';
        console.error('Error fetching bookings:', error);
      }
    );
  }

  // Function to format the date
  formatDate(dateString: string): string {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  }

  // Function to format the time
  formatTime(timeString: string): string {
    return timeString.slice(0, 5); // Remove seconds part
  }
}
