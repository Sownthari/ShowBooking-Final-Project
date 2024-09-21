import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminApiService } from '../admin-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-organizer',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-organizer.component.html',
  styleUrl: './add-organizer.component.css'
})
export class AddOrganizerComponent {
  organizer = {
    firstname: '',
    lastname: '',
    email: '',
    phoneNumber: '',
    passwordHash: 'temp'
  };

  constructor(private adminApiService: AdminApiService, private router: Router){}

  createOrganizer() {
    this.adminApiService.createOrganizer(this.organizer).subscribe(
      (response) => {
        alert("Organizer created successfully");
        this.router.navigate(['/admin-organizers']);

      }
    )
  }
}
