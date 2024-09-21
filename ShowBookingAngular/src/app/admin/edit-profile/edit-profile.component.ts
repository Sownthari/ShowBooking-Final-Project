import { Component, OnInit } from '@angular/core';
import { TheatreApiService } from '../theatre-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-edit-profile',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.css'
})
export class EditProfileComponent implements OnInit{
    adminDetails: any = {
      firstName: '',
      lastName: '',
      email: '',
      phoneNumber: ''
    };
  
    theatreDetails: any = {
      theatreName: '',
      theatreAddress: '',
      city: '',
      state: ''
    };
    userId: any = 0;
  
    constructor(private theatreService: TheatreApiService, private router: Router) {
      var token = localStorage.getItem('token');
    if(token !=null){
      var decodedToken:any = jwtDecode(token);
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
        error: (err) => console.error('Error fetching admin details', err)
      });
    }
  
    fetchTheatreDetails(): void {
      this.theatreService.getTheatre(this.userId).subscribe({
        next: (details) => {
          this.theatreDetails = details;
        },
        error: (err) => console.error('Error fetching theatre details', err)
      });
    }
  
    saveDetails(): void {
      const adminUpdate$ = this.theatreService.updateAdminDetails(this.adminDetails);
      const theatreUpdate$ = this.theatreService.updateTheatreDetails(this.theatreDetails);
  
      adminUpdate$.subscribe({
        next: () => {
          theatreUpdate$.subscribe({
            next: () => {
              alert('Details updated successfully!')
              this.router.navigate(['/adminProfile'])},
            error: (err) => console.error('Error updating theatre details', err)
          });
        },
        error: (err) => console.error('Error updating admin details', err)
      });
    }

    cancelEdit(){
      this.adminDetails = null;
      this.theatreDetails = null;
      this.router.navigate(['/adminProfile']);
    }
}
