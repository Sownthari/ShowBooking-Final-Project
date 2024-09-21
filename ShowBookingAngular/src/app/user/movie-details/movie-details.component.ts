import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserApiService } from '../user-api.service';

@Component({
  selector: 'app-movie-details',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './movie-details.component.html',
  styleUrl: './movie-details.component.css'
})
export class MovieDetailsComponent {
  movieId: number = 0;
  movie: any = {};
  constructor(private route:ActivatedRoute, private userApiService: UserApiService,private router: Router) { }

  ngOnInit(): void {
    this.movieId = +this.route.snapshot.paramMap.get('movieId')!;
    this.getBook(this.movieId);
  }

  bookTickets(movieId: number) {
    this.router.navigate(['/movie-shows',this.movieId,this.movie.movieName]);
    // Logic for booking tickets
    console.log(`Booking tickets for ${this.movie.movieName}`);
  }

  getBook(movieId:number){
    this.userApiService.getMovieDetails(movieId).subscribe(
      (response) => {
        this.movie = response;
      }
    )
  }

}
