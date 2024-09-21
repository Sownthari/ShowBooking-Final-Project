import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserApiService } from '../user-api.service';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  code : any = ''
  movies : any[] = []

  constructor(private userApiService: UserApiService,private router:Router){}

  location = localStorage.getItem('location');
  ngOnInit(){
    const token = localStorage.getItem('token');
    if(token !=null){
      const decodedtoken:any = jwtDecode(token)
      this.handleAuthCode(decodedtoken.UserId);
    }
    if(this.location){
      this.getMovies(this.location);
    }
    
  }
  getMovies(location:string){
    this.userApiService.getMovies(location).subscribe(
      (response) => {
        this.movies = response
      }
    )
  }
  getMovie(movieId:number){
    console.log("movie")
    this.router.navigate(['/movie',movieId]);
  }

  private handleAuthCode(userId:any) {
    this.code = this.getQueryParameter('code');
    
    if (this.code) {
      var c = {
        'code': this.code,
        'userId': userId
      }
       this.userApiService.auth(c).subscribe((response: any) => {
           console.log('Response from backend:', response);
         });
    }
  }

  private getQueryParameter(name: string): string | null {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
  }
}
