import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserApiService } from '../user/user-api.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  user:any = {
    'firstName':'',
    'lastName': '',
    'email': '',
    'passwordHash': '',
    'phoneNumber': ''
  }

  constructor(private userApiService: UserApiService, private router: Router){}
  register(){
    this.userApiService.register(this.user).subscribe(
      (response) => {
        alert("Registered successfully");
        this.router.navigate(['/login']);
      },
      (error) => {
        const errorMessage = error.error?.details || "An unknown error occurred.";
        alert(errorMessage);
      }
    )
  }
}
