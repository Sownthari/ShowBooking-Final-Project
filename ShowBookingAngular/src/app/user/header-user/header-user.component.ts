import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-header-user',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header-user.component.html',
  styleUrl: './header-user.component.css'
})
export class HeaderUserComponent {

  selectedCity: string = '';

  name: string| null = 'Guest';

  constructor() {}

  ngOnInit(): void {

    this.selectedCity = localStorage.getItem('location') || 'Select Location';
    var token = localStorage.getItem('token');
    if(token!=null){
      var decodedToken:any = jwtDecode(token);
    }
    this.name = decodedToken.nameid;
    console.log(this.name);
  }

  logout(): void {

    localStorage.removeItem('token');
    localStorage.removeItem('location');
    window.location.reload();
    window.location.href = '/login';
  }
}
