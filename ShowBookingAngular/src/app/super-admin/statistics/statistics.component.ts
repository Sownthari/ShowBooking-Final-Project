import { Component } from '@angular/core';
import { StatisticsService } from '../statistics.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent {
  movies: any[] = [];
  theatres: any[] = [];
  startDate?: Date;
  endDate?: Date;
  startDate1?: Date;
  endDate1?: Date;
  genre?: string;
  city?:string;

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit(): void {
    this.fetchTopMovies();
    this.fetchRevenueByTheatre();
  }

  fetchTopMovies(): void {
    this.statisticsService.getTopPerformingMovies(this.startDate, this.endDate, this.genre).subscribe((data: any[]) => {
      this.movies = data;
    });
  }

  fetchRevenueByTheatre(): void {
    this.statisticsService.getRevenueByTheatre(this.startDate1, this.endDate1, this.city).subscribe((data: any[]) => {
      this.theatres = data;
    });
  }
}
