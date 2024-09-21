import { RouterModule, Routes } from '@angular/router';
import { TheatreHomeComponent } from './admin/theatre-home/theatre-home.component';
import { AdminProfileComponent } from './admin/admin-profile/admin-profile.component';
import { EditProfileComponent } from './admin/edit-profile/edit-profile.component';
import { ScreenComponent } from './admin/screen/screen.component';
import { AddScreenComponent } from './admin/add-screen/add-screen.component';
import { ShowsComponent } from './admin/shows/shows.component';
import { AddShowsComponent } from './admin/add-shows/add-shows.component';
import { ShowDetailsComponent } from './admin/show-details/show-details.component';
import { SeatBookingComponent } from './user/seat-booking/seat-booking.component';
import { LoginComponent } from './login/login.component';
import { roleGuard } from './role.guard';
import { authGuard } from './auth.guard';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './user/home/home.component';
import { MovieDetailsComponent } from './user/movie-details/movie-details.component';
import { MovieShowsComponent } from './user/movie-shows/movie-shows.component';
import { OrganizersComponent } from './super-admin/organizers/organizers.component';
import { AddOrganizerComponent } from './super-admin/add-organizer/add-organizer.component';
import { PaymentComponent } from './user/payment/payment.component';
import { AddTheatreComponent } from './super-admin/add-theatre/add-theatre.component';
import { MoviesComponent } from './admin/movies/movies.component';
import { MoviesListComponent } from './super-admin/movies-list/movies-list.component';
import { AddMovieComponent } from './super-admin/add-movie/add-movie.component';
import { YourshowComponent } from './user/yourshow/yourshow.component';
import { BookingHistoryComponent } from './user/booking-history/booking-history.component';
import { ProfileComponent } from './user/profile/profile.component';
import { EditUserComponent } from './user/edit-user/edit-user.component';
import { ChangePasswordComponent } from './user/change-password/change-password.component';
import { SuperHeaderComponent } from './super-admin/super-header/super-header.component';
import { PaymentSuccessComponent } from './user/payment-success/payment-success.component';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { StatisticsComponent } from './super-admin/statistics/statistics.component';
import { OrganizerStatisticsComponent } from './admin/organizer-statistics/organizer-statistics.component';
import { AvailableMoviesComponent } from './user/movies/movies.component';



export const routes: Routes = [
    
    { path: 'organizer', component: TheatreHomeComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'adminProfile', component: AdminProfileComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'editAdminProfile/:id', component: EditProfileComponent, canActivate: [roleGuard], data: {requiredRole: '2'} },
    { path: 'screens/:id', component: ScreenComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'add-screen/:theatreId', component: AddScreenComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'edit-screen/:screenId', component: AddScreenComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'shows/:id', component: ShowsComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'add-show/:theatreId', component: AddShowsComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'show-details/:showId', component: ShowDetailsComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'show-seat/:showId/:screenType',component: SeatBookingComponent,canActivate: [authGuard]},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: HomeComponent, canActivate: [authGuard]},
    { path: 'movie/:movieId', component: MovieDetailsComponent, canActivate: [authGuard]},
    { path: 'movie-shows/:movieId/:movieName',component: MovieShowsComponent, canActivate: [authGuard]},
    { path: 'admin-organizers', component: OrganizersComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'add-organizer', component: AddOrganizerComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'payment', component: PaymentComponent, canActivate: [authGuard] },
    { path: 'add-theatre/:userId', component: AddTheatreComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'movies/:id', component: MoviesComponent, canActivate: [roleGuard], data: {requiredRole: '2'}},
    { path: 'movies-list', component: MoviesListComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'add-movie', component: AddMovieComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'list-your-show', component: YourshowComponent, canActivate: [authGuard]},
    { path: 'booking-history', component: BookingHistoryComponent, canActivate: [authGuard]},
    { path: 'profile', component: ProfileComponent, canActivate: [authGuard]},
    { path: 'update-user', component: EditUserComponent, canActivate: [authGuard]},
    { path: 'change-password', component: ChangePasswordComponent, canActivate: [authGuard]},
    { path: 'payment-success/:amount', component: PaymentSuccessComponent, canActivate: [authGuard]},
    { path: 'forbidden', component: ForbiddenComponent},
    { path: 'available-movies', component: AvailableMoviesComponent},
    { path: 'admin-statistics', component: StatisticsComponent, canActivate: [roleGuard], data: {requiredRole: '1'}},
    { path: 'organizer-statistics/:id', component: OrganizerStatisticsComponent, canActivate: [roleGuard], data: {requiredRole: '2'}}
];