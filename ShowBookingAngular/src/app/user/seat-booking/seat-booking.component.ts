import { Component } from '@angular/core';
import { UserApiService } from '../user-api.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { PaymentComponent } from '../payment/payment.component';

@Component({
  selector: 'app-seat-booking',
  standalone: true,
  imports: [FormsModule, CommonModule, PaymentComponent],
  templateUrl: './seat-booking.component.html',
  styleUrl: './seat-booking.component.css'
})
export class SeatBookingComponent {
  showId: number = 0;
  screenType: string = '';
  show : any = {};
  purchaseDetails: any;
  constructor(private userApiService: UserApiService, private router: Router, private route: ActivatedRoute){}

  ngOnInit():void{
    this.showId = this.route.snapshot.params['showId'];
    this.screenType = this.route.snapshot.params['screenType'];

    this.getshow(this.showId)
  }

  getshow(showId : number){
    this.userApiService.getShowSeats(showId).subscribe(
      (response) => {
        this.show = response;
        console.log(this.show)
      }
    )
  }

  selectedSeats: any[] = [];
  totalPrice: number = 0;
  rows: any = {};

  toggleSeatSelection(seat: any) {
    const seatIndex = this.selectedSeats.findIndex(s => s.seatId === seat.seatId);

    if (seatIndex === -1) {
      this.selectedSeats.push(seat);
      this.totalPrice += seat.price;
    } else {
      this.selectedSeats.splice(seatIndex, 1);
      this.totalPrice -= seat.price;
    }
    console.log(this.selectedSeats);
  }

  getSeatRows(seats: any[]) {
    const rows: any = {};
    seats.forEach(seat => {
      if (!rows[seat.seatRow]) {
        rows[seat.seatRow] = [];
      }
      rows[seat.seatRow].push(seat);
    });
    this.rows = rows;
  }

  buySeats() {
    const token = localStorage.getItem('token');
    if(token){
      const decodedToken: any = jwtDecode(token);
      const userId = decodedToken.UserId; 
      const seatIds = this.selectedSeats.map(seat => seat.seatId);

      this.purchaseDetails = {
        userId: userId,
        totalAmount: this.totalPrice,
        seatIds: seatIds,
        showId: this.showId
      };
      console.log('Purchase Details:', this.purchaseDetails);
    }

  }
  goToPayment() {
    this.buySeats();
    this.router.navigate(['/payment'], {
      queryParams: {
        totalAmount: this.purchaseDetails.totalAmount,
        seatIds: this.purchaseDetails.seatIds.join(','),
        userId: this.purchaseDetails.userId,
        showId: this.purchaseDetails.showId,
        showTime: this.show.showTime,
        showDate: this.show.showDate,
        movieName: this.show.movieName,
        theatreName: this.show.theatreName,
        count: this.selectedSeats.length
      }
    });
  }
  
}


