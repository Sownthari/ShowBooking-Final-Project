import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [],
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.css'
})
export class PaymentSuccessComponent {
  amount: number = 0;
  constructor(private route: ActivatedRoute, private router: Router){
    this.amount = this.route.snapshot.params['amount'];
  }

  goToHome(){
    this.router.navigate(['/home']);
  }


}
