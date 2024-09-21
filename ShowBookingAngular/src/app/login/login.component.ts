import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { User } from '../User';
import { Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  user: User = {
    email: '',
    password: '',
  };

  selectedCity: string = '';
  cities: string[] = [
    'Chennai',
    'Coimbatore',
    'Madurai',
    'Salem',
    'Erode',
    'Tirupur'
  ];

  constructor(private authService: AuthService, private router: Router) { }

  login() {
    if (this.user.email && this.user.password) {
      this.authService.login(this.user).subscribe(
        data => {
          const token = data?.token;
          if (token) {
            localStorage.setItem('token', token);
            localStorage.setItem('location', this.selectedCity.toLowerCase());
            const decodedToken: any = jwtDecode(token);
            alert("User logged in successfully");
            // Role-based redirection
            if (decodedToken.role === '2') {
              this.router.navigate(['/organizer']);
            } else if (decodedToken.role === '1') {
              this.router.navigate(['/admin-organizers']);
            } else if (decodedToken.role === '3') {
              this.requestGoogleAuth();
            } else {
              this.router.navigate(['/']);
            }
          } else {
            console.log('No token found in API response.');
            alert("Failed to retrieve token.");
          }
        },
        error => {
          console.error('Login error:', error);
          alert("Invalid login details or server error");
        }
      );
    } else {
      alert("Please enter email and password.");
    }
  }

  requestGoogleAuth() {
    const clientId = '409629890577-hggl4ugs06gtt6rca1p08slvgakrhbq9.apps.googleusercontent.com';
    const redirectUri = 'http://localhost:4200/home';
    const scope = 'https://www.googleapis.com/auth/calendar.events';
    const authUrl = `https://accounts.google.com/o/oauth2/auth?client_id=${clientId}&redirect_uri=${redirectUri}&scope=${scope}&response_type=code&access_type=offline&prompt=consent`;
    
    window.location.href = authUrl;
  }

  
}
