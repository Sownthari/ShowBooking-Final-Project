import { Component } from '@angular/core';
import { TheatreApiService } from '../theatre-api.service';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-theatre-home',
  standalone: true,
  imports: [],
  templateUrl: './theatre-home.component.html',
  styleUrl: './theatre-home.component.css'
})
export class TheatreHomeComponent {

  theatreName: string = 'Grand Cinema';
  userId: number = 0;

  constructor(private theatreService: TheatreApiService) { 
    const token = localStorage.getItem('token')
    if(token != null){
      const decodedToken:any = jwtDecode(token);
      this.userId = decodedToken.UserId;
    }
  }

  ngOnInit(): void {
    this.fetchTheatreName();
  }

  fetchTheatreName(): void {
    this.theatreService.getTheatre(this.userId).subscribe(
      (response) => {
        this.theatreName = response.theatreName;
      }
    )
  }
}
