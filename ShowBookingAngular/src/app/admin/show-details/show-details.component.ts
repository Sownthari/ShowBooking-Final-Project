import { Component, Input } from '@angular/core';
import { TheatreApiService } from '../theatre-api.service';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-show-details',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './show-details.component.html',
  styleUrl: './show-details.component.css'
})
export class ShowDetailsComponent {
  showId : number = 0;
  show: any = {
    showName: '',
    showDate: '',
    showTime: '',
    durationMinutes: 0,
    description: '',
    movieName: '',
    screenName: '',
    status: '',
    ticketTypeSeats: []
  };
  displayDetails: boolean[] = [];

  constructor(private theatreApiService: TheatreApiService,private route: ActivatedRoute) {
    this.displayDetails = new Array(this.show.ticketTypeSeats.length).fill(false);
  }

  ngOnInit():void {

    this.showId = this.route.snapshot.params['showId'];
    this.showDetails();
  }

  showDetails(){
    this.theatreApiService.showDetails(this.showId).subscribe(
      (response) => {
        this.show = response;
        this.show.showTime = this.convertToDate(this.show.showTime);
      }
    )
  }

  changeShowStatus() {
    // this.theatreApiService.changeStatus(this.show.showID).subscribe(
    //   (response) => {
    //     // Handle the status change
    //     console.log('Show status updated successfully', response);
    //     // Optionally update the UI to reflect the status change
    //   },
    //   (error) => {
    //     console.error('Error updating show status', error);
    //   }
    // );
  }

  toggleTicketDetails(index: number) {
    // Toggle the visibility of the clicked ticket type
    this.displayDetails[index] = !this.displayDetails[index];
  }

  convertToDate(timeString: string): Date {
    const [hours, minutes, seconds] = timeString.split(':');
    const date = new Date();
    date.setHours(+hours);
    date.setMinutes(+minutes);
    date.setSeconds(+seconds);
    return date;
  }
}
