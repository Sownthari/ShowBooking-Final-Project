import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormRecord, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminApiService } from '../admin-api.service';

@Component({
  selector: 'app-add-theatre',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './add-theatre.component.html',
  styleUrl: './add-theatre.component.css'
})
export class AddTheatreComponent {
  theatre = {
    userId: 0,
    theatreName: '',
    theatreAddress: '',
    city: '',
    state: ''
  };

  constructor(private router:Router,private route: ActivatedRoute,private adminApiService: AdminApiService){}
  ngOnInit(){
    this.theatre.userId = this.route.snapshot.params['userId'];
  }

  createTheatre() {
    this.adminApiService.createTheatre(this.theatre).subscribe(
      (response) => {
        alert("Theatre created successfully");
        this.router.navigate(['/admin-organizers']);
      }
    )
  }
}
