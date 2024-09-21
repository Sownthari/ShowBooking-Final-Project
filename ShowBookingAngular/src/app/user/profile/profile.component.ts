import { Component } from '@angular/core';
import { UserApiService } from '../user-api.service';
import { Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [RouterModule, FormsModule, CommonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {

  Details: any = {}; 
  userId: number = 0;

  constructor(private userApiService: UserApiService, private router: Router) {
    const token = localStorage.getItem('token');
    if(token != null){
      const decodedToken: any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    } 
  }

  ngOnInit(): void {
    this.fetchUserDetails();
  }

  fetchUserDetails(): void {
    this.userApiService.getUser(this.userId).subscribe({
      next: (details) => {
        this.Details = details;
      },
      error: (err) => {
        console.error('Error fetching admin details', err);
      }
    });
  }
}
