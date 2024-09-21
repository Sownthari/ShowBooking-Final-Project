import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminApiService } from '../admin-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizers',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './organizers.component.html',
  styleUrl: './organizers.component.css'
})
export class OrganizersComponent {

    users: any[] = [];
  
    constructor(private adminApiService: AdminApiService,private router: Router) {}
  
    ngOnInit(): void {
      this.getUsersWithRole();
    }
  
    getUsersWithRole() {
      this.adminApiService.getOrganizers().subscribe(
        (response) => {
          this.users = response;
        }
      )
    }
  
    addTheatre(user: any) {
      if (!user.hasTheatre) {
        this.router.navigate(['/add-theatre',user.userID])
      }
    }
    createNewOrganizer() {
      this.router.navigate(['add-organizer'])
    }

    addMovie(user:any){
      this.adminApiService.createMapping(user.theatreID, user.selectedMovie).subscribe(
        (response) => {
          console.log("Creating mapping successfully",response);
          this.getUsersWithRole();
        }
      )
    }
}
