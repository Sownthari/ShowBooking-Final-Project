import { Component } from '@angular/core';
import { AddScreenComponent } from "../add-screen/add-screen.component";
import { RouterModule } from '@angular/router';
import { TheatreApiService } from '../theatre-api.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [AddScreenComponent, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  userId: number = 0;
  theatreId: number = 0;
  name: string | null = 'Guest';
  constructor(private theatreApiService: TheatreApiService){
    var token = localStorage.getItem('token');
    if(token !=null){
      var decodedToken:any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    }
  }
  ngOnInit(){
    var token = localStorage.getItem('token');
    if(token!=null){
      var decodedToken:any = jwtDecode(token);
    }
    this.name = decodedToken.nameid;
    this.theatreApiService.getTheatre(this.userId).subscribe(
      (response) => {
        this.theatreId = response.theatreID
      }
    )
  }
  logout(): void {

    localStorage.removeItem('token');
    localStorage.removeItem('location');
    window.location.reload();
    window.location.href = '/login';
  }
}
