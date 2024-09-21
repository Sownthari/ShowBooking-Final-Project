import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  private baseUrl = 'https://localhost:7025/api/Statistics/api';

  constructor(private http: HttpClient) {}

  // Get top performing movies
  getTopPerformingMovies(startDate?: Date, endDate?: Date, genre?: string): Observable<any> {
    let params: any = {};
    if (startDate) params.startDate = startDate.toISOString();
    if (endDate) params.endDate = endDate.toISOString();
    if (genre) params.genre = genre;

    return this.http.get(`${this.baseUrl}/admin/top-performing-movies`, { params });
  }

  // Get revenue by theatre
  getRevenueByTheatre(startDate?: Date, endDate?: Date, city?: string): Observable<any> {
    let params: any = {};
    if (startDate) params.startDate = startDate.toISOString();
    if (endDate) params.endDate = endDate.toISOString();
    if (city) params.city = city;

    return this.http.get(`${this.baseUrl}/admin/revenue-by-theatre`, { params });
  }

  // Get occupancy rate by theatre screen
  getOccupancyRate(theatreId: number, startDate?: Date, endDate?: Date, screenType?: string): Observable<any> {
    let params: any = {};
    if (startDate) params.startDate = startDate.toISOString();
    if (endDate) params.endDate = endDate.toISOString();
    if (screenType) params.screenType = screenType;

    return this.http.get(`${this.baseUrl}/theatre/occupancy-rate/${theatreId}`, { params });
  }

  // Get ticket sales trend
  getTicketSalesTrend(theatreId: number, period: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/theatre/ticket-sales-trend/${theatreId}?period=${period}`);
  }
} 
