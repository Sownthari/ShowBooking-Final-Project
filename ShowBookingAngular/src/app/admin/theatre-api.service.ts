import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TheatreApiService {

  private apiUrl = 'https://localhost:7025/api/Theatre';
  private apiUserUrl = 'https://localhost:7025/api/User'; 
  private apiScreenUrl = 'https://localhost:7025/api/Screen';
  private apiShowUrl = 'https://localhost:7025/api/TheatreShow';
  private apiMovieUrl = 'https://localhost:7025/api/Movie';

  constructor(private http: HttpClient) { }

  getTheatre(userId:number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${userId}`);
  }
  getUser(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUserUrl}/${userId}`);
  }

  updateTheatreDetails(theatreDetails: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update`, theatreDetails);
  }

  updateAdminDetails(adminDetails: any): Observable<any> {
    return this.http.put(`${this.apiUserUrl}/update`, adminDetails);
  }

  getScreens(theatreId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiScreenUrl}/theatre/${theatreId}`);
  }

  getScreen(screenId: number): Observable<any> {
    return this.http.get<any[]>(`${this.apiScreenUrl}/${screenId}`);
  }

  addScreen(screen: any): Observable<any> {
    return this.http.post<any>(`${this.apiScreenUrl}`, screen);
  }

  updateScreen(screen: any): Observable<any> {
    return this.http.put<any>(`${this.apiScreenUrl}/update`, screen);
  }

  getShowsByDate(theatreId:number, date:string): Observable<any> {
    return this.http.get<any[]>(`${this.apiShowUrl}/theatre/${theatreId}/${date}`);
  }

  getMovies(theatreId:number):Observable<any>{
    return this.http.get<any[]>(`${this.apiMovieUrl}/theatre/${theatreId}`);
  }
  
  getScreensName(theatreId:number):Observable<any>{
    return this.http.get<any[]>(`${this.apiScreenUrl}/screens/${theatreId}`);
  }

  addShow(show: any):Observable<any>{
    return this.http.post<any>(`${this.apiShowUrl}`,show);
  }

  showDetails(showId: number):Observable<any>{
    return this.http.get<any>(`${this.apiShowUrl}/${showId}`);
  }

  updateShowStatus(showId:number,status:string):Observable<any>{
    return this.http.put<any>(`${this.apiShowUrl}/${showId}`,status);
  }

  movieStatus(movieId: number, theatreId: number):Observable<any>{
    return this.http.put<any>(`${this.apiMovieUrl}/update/${movieId}/${theatreId}`,{});
  }
  
} 
