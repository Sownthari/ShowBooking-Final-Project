import { Component } from '@angular/core';
import { TheatreApiService } from '../theatre-api.service';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-admin-profile',
  standalone: true,
  imports: [],
  templateUrl: './admin-profile.component.html',
  styleUrl: './admin-profile.component.css'
})
export class AdminProfileComponent {

  adminDetails: any = {}; 
  theatreDetails: any = {};
  userId: number = 0;

  constructor(private theatreService: TheatreApiService, private router: Router) {
    const token = localStorage.getItem('token');
    if(token != null){
      const decodedToken: any = jwtDecode(token);
      console.log(decodedToken)
      this.userId = decodedToken.UserId;
    } 
  }

  ngOnInit(): void {
    this.fetchAdminDetails();
    this.fetchTheatreDetails();
  }

  fetchAdminDetails(): void {
    this.theatreService.getUser(this.userId).subscribe({
      next: (details) => {
        this.adminDetails = details;
      },
      error: (err) => {
        console.error('Error fetching admin details', err);
      }
    });
  }

  fetchTheatreDetails(): void {
    this.theatreService.getTheatre(this.userId).subscribe({
      next: (details) => {
        this.theatreDetails = details;
      },
      error: (err) => {
        console.error('Error fetching theatre details', err);
      }
    });
  }

  editProfile(): void {
    this.router.navigate(['/editAdminProfile', this.userId]);
  }

  changePassword(): void {
    this.router.navigate(['/change-password']);
  }
}
