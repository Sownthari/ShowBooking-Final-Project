import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TheatreApiService } from '../theatre-api.service';

@Component({
  selector: 'app-add-screen',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './add-screen.component.html',
  styleUrls: ['./add-screen.component.css']
})
export class AddScreenComponent {

  theatreId: number = 0;
  screenId: number = 0;
  screenName: string = '';
  seatingCapacity: number = 0;
  screenType: string = '';
  
  ticketTypes: { ticketTypeName: string; price: number }[] = [];
  seats: { seatRow: string; seatColumn: number; ticketType: string }[] = [];

  constructor(private router: Router,private theatreApiService: TheatreApiService,private route: ActivatedRoute) {}

  ngOnInit(){
    this.theatreId = this.route.snapshot.params['theatreId'];
    this.screenId = this.route.snapshot.params['screenId'];
    if (this.screenId) {
      this.loadScreenData(this.screenId);
    }
  }

  loadScreenData(screenId: number){
    this.theatreApiService.getScreen(screenId).subscribe(data => {
      this.screenName = data.screenName;
      this.seatingCapacity = data.seatingCapacity;
      this.screenType = data.screenType;
      this.ticketTypes = data.ticketTypes;
      this.seats = data.seats;
    });
  }

  addTicketType(): void {
    this.ticketTypes.push({ ticketTypeName: '', price: 0 });
  }

  removeTicketType(index: number): void {
    this.ticketTypes.splice(index, 1);
  }

  addSeat(): void {
    this.seats.push({ seatRow: '', seatColumn: 0, ticketType: '' });
  }

  removeSeat(index: number): void {
    this.seats.splice(index, 1);
  }

  submitForm(): void {

    if (this.screenId) {
      const screenData = {
        screenID: this.screenId,
        screenName: this.screenName,
        seatingCapacity: this.seatingCapacity,
        screenType: this.screenType,
        ticketTypes: this.ticketTypes,
        seats: this.seats
      };
      console.log(screenData)
      
      this.theatreApiService.updateScreen(screenData).subscribe(
        (response) => {
          console.log('Screen updated successfully:', response);
          this.router.navigate(['/screens',this.screenId]);
        },
        (error) => {
          console.error('Error updating screen:', error);
        }
      );
    }

    else{
      const screenData = {
        theatreID: this.theatreId,
        screenName: this.screenName,
        seatingCapacity: this.seatingCapacity,
        screenType: this.screenType,
        ticketTypes: this.ticketTypes,
        seats: this.seats
      };

      console.log('Screen Data:', screenData);
      this.theatreApiService.addScreen(screenData).subscribe(
        (response) => {
          alert('Screen added successfully:');
          
          this.router.navigate(['/screens', this.theatreId]);
        },
        (error) => {
          console.error('Error adding screen:', error);
          
        }
      );

      this.router.navigate(['/screens',this.theatreId]);
    } 
  }

  cancel(){
    this.router.navigate(["/screens", this.screenId]);
  }

}

  
