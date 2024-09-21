import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TheatreApiService } from '../theatre-api.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-movies',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './movies.component.html',
  styleUrl: './movies.component.css'
})
export class MoviesComponent {


  selectedTab: string = 'active'; // Default selected tab
  activeMovies: any[] = [];
  inactiveMovies: any[] = [];
  theatreId: number = 0;

  constructor(private theatreApiService: TheatreApiService, private route: ActivatedRoute){
    this.theatreId = this.route.snapshot.params['id'];
  }

  ngOnInit() {
    this.filterMovies();
  }

  selectTab(tab: string) {
    this.selectedTab = tab;
  }

  isActiveTab(tab: string) {
    return this.selectedTab === tab;
  }

  filterMovies() {
    this.theatreApiService.getMovies(this.theatreId).subscribe(movies => {
      this.activeMovies = movies.filter((movie: { isActive: any; }) => movie.isActive);
      this.inactiveMovies = movies.filter((movie: { isActive: any; }) => !movie.isActive);
    });
  }
  makeInactive(movie: any) {
    this.theatreApiService.movieStatus(movie.movieID, this.theatreId).subscribe(
      (response)=> {
        this.filterMovies();
      }
    )

  }
}
