import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminApiService {

  private apiUserUrl = 'https://localhost:7025/api/User'; 
  private apiTheatreUrl = 'https://localhost:7025/api/Theatre';
  private apiMovieUrl = 'https://localhost:7025/api/Movie';

  constructor(private http: HttpClient) { }

  getOrganizers():Observable<any>{
    return this.http.get<any>(`${this.apiUserUrl}/organisers`);
  }

  createOrganizer(organizer: any):Observable<any>{
    return this.http.post<any>(`${this.apiUserUrl}/CreateOrganizer`,organizer);
  }

  createTheatre(theatre:any):Observable<any>{
    return this.http.post<any>(`${this.apiTheatreUrl}`,theatre);
  }

  getMovies(){
    return this.http.get<any[]>(this.apiMovieUrl);
  }

  addMovie(movie: any){
    return this.http.post<any>(`${this.apiMovieUrl}/addMovie`,movie);
  }

  createMapping(theatreId:number, movieId: number){
    return this.http.post<any>(`${this.apiMovieUrl}/mapmovie/${theatreId}/${movieId}`,{});
  }
}
