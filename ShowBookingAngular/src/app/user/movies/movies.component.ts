import { Component, OnInit } from '@angular/core';
import { UserApiService } from '../user-api.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css'
})
export class AvailableMoviesComponent implements OnInit {
  location: any = localStorage.getItem('location');
  languages: string[] = ['English', 'Hindi', 'Spanish', 'French', 'Tamil'];
  genres: string[] = ['Action', 'Comedy', 'Drama', 'Thriller', 'Family','Horror'];

  constructor(private userApiService: UserApiService, private router: Router){}

  selectedLanguages: string[] = [];
  selectedGenres: string[] = [];

  showLanguageFilter: boolean = false;  
  showGenreFilter: boolean = false;    
  movies:any[] = [];
  filteredMovies: any[] = [];

  

  ngOnInit(): void {
    this.getMovies();
    this.applyFilters();
  }

  // Toggle language filter visibility
  toggleLanguageFilter(): void {
    this.showLanguageFilter = !this.showLanguageFilter;
  }

  // Toggle genre filter visibility
  toggleGenreFilter(): void {
    this.showGenreFilter = !this.showGenreFilter;
  }

  onLanguageChange(event: any): void {
    const language = event.target.value;
    if (event.target.checked) {
      this.selectedLanguages.push(language);
    } else {
      this.selectedLanguages = this.selectedLanguages.filter(l => l !== language);
    }
    this.applyFilters();
  }

  onGenreChange(event: any): void {
    const genre = event.target.value;
    if (event.target.checked) {
      this.selectedGenres.push(genre);
    } else {
      this.selectedGenres = this.selectedGenres.filter(g => g !== genre);
    }
    this.applyFilters();
  }

  applyFilters(): void {
    this.filteredMovies = this.movies.filter(movie =>
      (this.selectedLanguages.length === 0 || this.selectedLanguages.includes(movie.language)) &&
      (this.selectedGenres.length === 0 || this.selectedGenres.includes(movie.genre))
    );
  }

  getMovies(){
    this.userApiService.getMovies(this.location).subscribe(
      (response) => {
        this.movies = response;
        this.filteredMovies = this.movies;
      }
    )
  }

  moveMovie(movieId:number){
    console.log("movie")
    this.router.navigate(['/movie',movieId]);
  }
}