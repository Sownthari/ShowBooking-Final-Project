import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserApiService {

  constructor(private http: HttpClient) { }

  private apiShowUrl = 'https://localhost:7025/api/TheatreShow';
  private apiMovieUrl = 'https://localhost:7025/api/Movie';
  private apiBookingUrl = 'https://localhost:7025/api/TheatreBookings';
  private apiAuthUrl  = 'https://localhost:7025/api/Auth/google';
  private apiUserUrl = 'https://localhost:7025/api/User';
  

  getShowSeats(showId: number):Observable<any>{
    return this.http.get<any>(`${this.apiShowUrl}/show/${showId}`);
  }

  getMovies(location: string):Observable<any>{
    return this.http.get<any[]>(`${this.apiMovieUrl}/location/${location}`);
  }

  getMovieDetails(movieId: number):Observable<any>{
    return this.http.get<any>(`${this.apiMovieUrl}/movie/${movieId}`);
  }

  getFilteredShow(filter:any):Observable<any>{
    return this.http.post<any>(`${this.apiShowUrl}/filtered`,filter);
  }

  makePayment(data:any):Observable<any>{
    return this.http.post<any>(`${this.apiBookingUrl}`,data);
  }

  auth(code:any):Observable<any>{
    return this.http.post<any>(this.apiAuthUrl,code);
  }

  getBookings(userId: any):Observable<any>{
    return this.http.get<any[]>(`${this.apiBookingUrl}/user/${userId}`);
  }

  getUser(userId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUserUrl}/${userId}`);
  }

  updateUser(user: any): Observable<any> {
    return this.http.put(`${this.apiUserUrl}/update`, user);
  }

  changePassword(user: any): Observable<any>{
    return this.http.put<any>(`${this.apiUserUrl}/updatePassword`,user);
  }
  register(user: any): Observable<any>{
    return this.http.post<any>(`${this.apiUserUrl}/register`,user);
  } 




}
