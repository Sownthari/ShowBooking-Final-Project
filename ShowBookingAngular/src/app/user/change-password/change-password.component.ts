import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserApiService } from '../user-api.service';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent {
  passwordDetails = {
    oldPassword: '',
    newPassword: '',
    userId: 0
  };
  role:string = '0';

  constructor(private userApiService: UserApiService, private router: Router){
    var token = localStorage.getItem('token');
    if(token){
      var decodedToken:any = jwtDecode(token);
      this.passwordDetails.userId = decodedToken.UserId;
    }
  }

  changePassword() {
    this.userApiService.changePassword(this.passwordDetails).subscribe(
      (response) => {
        alert(" Password changed successfully");
        localStorage.clear();
      }
    )
  
  }
}
