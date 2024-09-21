import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./admin/header/header.component";
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { jwtDecode } from 'jwt-decode';
import { HeaderUserComponent } from './user/header-user/header-user.component';
import { AuthService } from './auth.service';
import { SuperHeaderComponent } from "./super-admin/super-header/super-header.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, FormsModule, CommonModule, HeaderUserComponent, SuperHeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'TicketBooking';

  currentRoute: string = '';

  constructor(private router: Router, private authService: AuthService) {

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });
  }

  showHeader(): boolean {

    return !(this.currentRoute.includes('/login') || this.currentRoute.includes('/register'));
  }

  isRole(role: string): boolean {
    return this.authService.getUserRole() === role;
  }
}