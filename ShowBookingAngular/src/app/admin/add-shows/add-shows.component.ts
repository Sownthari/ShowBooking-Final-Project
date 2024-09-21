import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TheatreApiService } from '../theatre-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-shows',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-shows.component.html',
  styleUrl: './add-shows.component.css'
})
export class AddShowsComponent {

  theatreId : number = 0;
  movies: any[] = [];
  screens: any[] = [];

  selectedMovieId: number = 0;
  selectedScreenId: number = 0;
  showName: string = '';
  description: string = '';
  showDate: Date = new Date();
  showTime: string = '';
  durationMinutes: number = 0;

  constructor(private router: Router,private theatreApiService: TheatreApiService,private route: ActivatedRoute) {}

  ngOnInit():void {
    this.theatreId = this.route.snapshot.params['theatreId'];
    this.getMovies(this.theatreId);
    this.getScreens(this.theatreId);
  }

  getMovies(theatreId:number){
    this.theatreApiService.getMovies(theatreId).subscribe(
      (response) => {
        this.movies = response;
      }
    )
  }

  getScreens(theatreId: number){
    this.theatreApiService.getScreensName(theatreId).subscribe(
      (response) => {
        this.screens = response;
      }
    )
  }

  submitForm() {
    const newShow = {
      movieID: this.selectedMovieId,
      screenID: this.selectedScreenId,
      showName: this.showName,
      description: this.description,
      showDate: this.showDate,
      showTime: this.showTime,
      durationMinutes: this.durationMinutes,
      status: "active"
    };
    this.theatreApiService.addShow(newShow).subscribe(response => {
        alert('Show added successfully');
        this.router.navigate(['/shows',this.theatreId]);
      });
    }
    cancel() {
      this.router.navigate(['/shows', this.theatreId]);
    }
  }

