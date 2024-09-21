import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TheatreApiService } from '../theatre-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-screen',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './screen.component.html',
  styleUrl: './screen.component.css'
})
export class ScreenComponent implements OnInit {
  screens: any[] = [];
  theatreId:number = 0;
  userId: number = 0;

  constructor(private theatreApiService: TheatreApiService, private router:Router, private route: ActivatedRoute) {
    this.theatreId = this.route.snapshot.params['theatreId'];
    var token = localStorage.getItem('token');
    if(token !=null){
      var decodedToken:any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    }
  }

  ngOnInit(): void {
    this.getTheatreAndScreens();
  }

  getTheatreAndScreens(): void {
    this.theatreApiService.getTheatre(this.userId).subscribe(
      (theatre) => {
        this.theatreId = theatre.theatreID;
        this.getScreens();
      },
      (error) => {
        console.error('Error fetching theatre details', error);
      }
    );
  }

  getScreens(): void {
    this.theatreApiService.getScreens(this.theatreId).subscribe(
      (data) => {
        this.screens = data;
      },
      (error) => {
        console.error('Error fetching screens', error);
      }
    );
  }

  editScreen(screenId: number): void {
    this.router.navigate(['/edit-screen', screenId]);
    console.log('Edit screen with ID:', screenId);
  }

  addScreen(): void {
    this.router.navigate(['/add-screen',this.theatreId]);
    console.log('Add a new screen');
  }
}

