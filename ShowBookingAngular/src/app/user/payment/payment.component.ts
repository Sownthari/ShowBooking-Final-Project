import { Component, Input, OnInit } from '@angular/core';

import { UserApiService } from '../user-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements OnInit {
  paymentData: any = {};
  paymentDetails = {
    name: '',
    cardNumber: '',
    expiry: '',
    cvv: '',
    upiId: ''
  };
  paymentStatus: boolean = false;

  constructor(private route: ActivatedRoute,private userApiService: UserApiService,private router:Router) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.paymentData.totalAmount = params['totalAmount'];
      this.paymentData.seatIds = params['seatIds'] ? params['seatIds'].split(',') : [];
      this.paymentData.userId = params['userId'];
      this.paymentData.showId = params['showId'];
      this.paymentData.movieName = params['movieName'];
      this.paymentData.theatreName = params['theatreName'];
      this.paymentData.showTime = params['showTime'];
      this.paymentData.showDate = params['showDate'];
      this.paymentData.count = params['count'];

      console.log('Received payment data:', this.paymentData);
    });
  }

  processPayment() {
    this.userApiService.makePayment(this.paymentData).subscribe(
      (response) => {
        this.router.navigate(['/payment-success',this.paymentData.totalAmount]);
      }
    )
  }
}