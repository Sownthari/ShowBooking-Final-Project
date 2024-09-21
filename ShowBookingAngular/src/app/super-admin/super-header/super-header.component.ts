import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-super-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './super-header.component.html',
  styleUrl: './super-header.component.css'
})
export class SuperHeaderComponent {

  name:string | null = 'Guest';
  ngOnInit(){
    var token = localStorage.getItem('token');
    if(token!=null){
      var decodedToken:any = jwtDecode(token);
    }
    this.name = decodedToken.nameid;
  }
  logout(){

      localStorage.removeItem('token');
      localStorage.removeItem('location');
      window.location.reload();
      window.location.href = '/login';
  }
}
