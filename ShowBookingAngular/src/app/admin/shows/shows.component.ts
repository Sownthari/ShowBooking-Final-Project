import { Component, OnInit } from '@angular/core';
import { TheatreApiService } from '../theatre-api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-shows',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './shows.component.html',
  styleUrl: './shows.component.css'
})

export class ShowsComponent implements OnInit {
  shows: any[] = [];
  selectedDate: string = '';
  theatreId:number = 0;

  constructor(private theatreApiService: TheatreApiService,private route:ActivatedRoute, private router: Router) {
    this.theatreId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
    const today = new Date();
    this.selectedDate = today.toISOString().split('T')[0];
    this.fetchShows();
  }

  fetchShows() {
    this.theatreApiService.getShowsByDate(this.theatreId,this.selectedDate).subscribe((data) => {

      this.shows = data.map((screen: any) => ({
        ...screen,
        shows: screen.shows.map((show: { showTime: string; }) => ({
          ...show,
          showTime: this.convertToDate(show.showTime)
        }))
      }));
    });
  }
  
  convertToDate(timeString: string): Date {
    const [hours, minutes, seconds] = timeString.split(':');
    const date = new Date();
    date.setHours(+hours);
    date.setMinutes(+minutes);
    date.setSeconds(+seconds);
    return date;
  }

  openAddShowModal() {
    this.router.navigate(['/add-show',this.theatreId]); 
  }

  details(showId: number) {
    console.log(showId);
    this.router.navigate(['/show-details',showId]);
  }
}