import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminApiService } from '../admin-api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-movies-list',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './movies-list.component.html',
  styleUrl: './movies-list.component.css'
})
export class MoviesListComponent {
  movies: any = [];

  constructor(private adminApiService: AdminApiService, private router: Router) {}

  ngOnInit() {
    this.fetchMovies();
  }

  fetchMovies() {
    this.adminApiService.getMovies().subscribe(
      (movies: any[]) => {
        this.movies = movies;
      },
      error => {
        console.error('Error fetching movies:', error);
      }
    );
  }

  convertDuration(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;
    return `${hours}h ${remainingMinutes}m`;
  }

  addMovie(){
    this.router.navigate(['/add-movie']);
  }
}
