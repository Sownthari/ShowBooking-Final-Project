import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserApiService } from '../user-api.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-edit-user',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.css'
})
export class EditUserComponent {
  userDetails: any = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: ''
  };
  userId: number = 0;

  constructor(private userApiService: UserApiService,private router: Router){
    var token = localStorage.getItem('token');
    if(token !=null){
      var decodedToken:any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    }
  }

  ngOnInit(){
    this.getUserDetails();
  }

  getUserDetails(){
    this.userApiService.getUser(this.userId).subscribe(
      (response) => {
        this.userDetails = response;
      }
    )
  }

  updateUserDetails() {
    
    this.userApiService.updateUser(this.userDetails).subscribe(
      (response) => {
        alert("Updated successfully");
        this.router.navigate(['/profile'])
      }
    )
    
  }
}
