import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import moment from 'moment';
import { ActivatedRoute, Router } from '@angular/router';
import { UserApiService } from '../user-api.service';

@Component({
  selector: 'app-movie-shows',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './movie-shows.component.html',
  styleUrls: ['./movie-shows.component.css']
})
export class MovieShowsComponent {
  movieId: number = 0;
  movieName: string = '';
  shows: any[] = [];
  selectedScreen: number | undefined;
  selectedDate: string;

  selectedPriceRanges: { [key: string]: boolean } = {
    '₹0 - ₹100': false,
    '₹100 - ₹200': false,
    '₹200 - ₹300': false,
    '₹300 - ₹400': false,
    '₹400 - ₹500': false,
  };

  selectedTimeOptions: { [key: string]: boolean } = {
    'Morning (8 AM - 12 PM)': false,
    'Afternoon (12 PM - 4 PM)': false,
    'Evening (4 PM - 8 PM)': false,
    'Night (8 PM - 12 AM)': false,
  };

  priceRanges = Object.keys(this.selectedPriceRanges);
  timeOptions = Object.keys(this.selectedTimeOptions);
  next7Days: any[] = [];

  minPrice: number | null = null;
  maxPrice: number | null = null;
  minTime: string | null = null;
  maxTime: string | null = null;

  showPriceOptions = false;
  showTimeOptions = false;

  constructor(private route: ActivatedRoute,private userApiService: UserApiService, private router: Router) {
    this.selectedDate = new Date().toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.movieId = this.route.snapshot.params['movieId'];
    this.movieName = this.route.snapshot.params['movieName'];
    this.getScreens();
    this.getNext7Days();
  }

  getNext7Days(): void {
    const today = new Date();
    this.next7Days = Array.from({ length: 7 }, (_, i) => {
      const nextDate = new Date(today);
      nextDate.setDate(today.getDate() + i);
      return {
        day: nextDate.toLocaleString('en-US', { weekday: 'short' }),
        date: nextDate.getDate(),
        month: nextDate.toLocaleString('en-US', { month: 'short' }), 
        dateValue: nextDate.toISOString().split('T')[0],
      };
    });
  }

  selectDate(day: any): void {
    this.selectedDate = day.dateValue; // Set the selected date
    console.log('Selected Date:', this.selectedDate);
    this.applyFilters();
  }

  // Handle filter application
  applyFilters(): void {
    this.updatePriceRange();
    this.updateTimeRange();

    const selectedPriceRanges = Object.keys(this.selectedPriceRanges).filter(range => this.selectedPriceRanges[range]);
    const selectedTimeOptions = Object.keys(this.selectedTimeOptions).filter(time => this.selectedTimeOptions[time]);

    this.callApi(this.selectedDate, selectedPriceRanges, selectedTimeOptions);
  }

  // Update the selected price range and calculate min/max price
  updatePriceRange(): void {
    const selectedPrices = Object.keys(this.selectedPriceRanges)
      .filter(range => this.selectedPriceRanges[range])
      .map(range => this.parsePriceRange(range));

    if (selectedPrices.length > 0) {
      const allMinPrices = selectedPrices.map(price => price.min);
      const allMaxPrices = selectedPrices.map(price => price.max);
      this.minPrice = Math.min(...allMinPrices);
      this.maxPrice = Math.max(...allMaxPrices);
    } else {
      this.minPrice = null;
      this.maxPrice = null;
    }
  }

  // Update the selected time options and calculate min/max time
  updateTimeRange(): void {
    const selectedTimes = Object.keys(this.selectedTimeOptions)
      .filter(time => this.selectedTimeOptions[time])
      .map(time => this.parseTimeOption(time));

    if (selectedTimes.length > 0) {
      const allMinTimes = selectedTimes.map(time => time.min);
      const allMaxTimes = selectedTimes.map(time => time.max);
      this.minTime = this.formatTime(Math.min(...allMinTimes));
      this.maxTime = this.formatTime(Math.max(...allMaxTimes));
    } else {
      this.minTime = null;
      this.maxTime = null;
    }
  }

  parsePriceRange(range: string): { min: number; max: number } {
    const cleanRange = range.replace(/[₹,\s]/g, '');
    const [minStr, maxStr] = cleanRange.split('-').map(str => str.trim());

    const min = Number(minStr);
    const max = Number(maxStr);

    if (isNaN(min) || isNaN(max)) {
      console.error('Invalid price range:', range);
      return { min: 0, max: 0 };
    }

    return { min, max };
  }


  parseTimeOption(time: string): { min: number; max: number } {
    switch (time) {
      case 'Morning (8 AM - 12 PM)':
        return { min: 480, max: 720 }; // 8 AM = 480 minutes, 12 PM = 720 minutes
      case 'Afternoon (12 PM - 4 PM)':
        return { min: 720, max: 960 }; // 12 PM = 720 minutes, 4 PM = 960 minutes
      case 'Evening (4 PM - 8 PM)':
        return { min: 960, max: 1200 }; // 4 PM = 960 minutes, 8 PM = 1200 minutes
      case 'Night (8 PM - 12 AM)':
        return { min: 1200, max: 1440 }; // 8 PM = 1200 minutes, 12 AM = 1440 minutes
      default:
        return { min: 0, max: 0 };
    }
  }

  formatTime(minutes: number): string {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return `${hours.toString().padStart(2, '0')}:${mins.toString().padStart(2, '0')}`;
  }

  callApi(date: string, priceRanges: string[], timeOptions: string[]): void {
    var filters = {
      "showDate": date,
      "movieID": this.movieId,
      "minPrice":this.minPrice,
      "maxPrice":this.maxPrice,
      "minShowTime":this.minTime,
      "maxShowTime":this.maxTime,
      "screenType": "2D" || null,
      "city": localStorage.getItem('location')
    }
    this.userApiService.getFilteredShow(filters).subscribe(
      (response) => {
        this.shows = response;
        console.log(this.shows)
      }
    )
  }

  getScreens(): void {
    var filters = {
      "showDate": new Date(),
      "movieID": this.movieId,
      "minPrice":null,
      "maxPrice":null,
      "minShowTime":null,
      "maxShowTime":null,
      "screenType": "2D",
      "city": localStorage.getItem('location')
    }
    this.userApiService.getFilteredShow(filters).subscribe(
      (response) => {
        this.shows = response;
        console.log(this.shows)
      }
    )
  }
  bookTickets(showId: any, screenType: string) {
    this.router.navigate(['/show-seat',showId, screenType]);
  }

  convertToDate(timeString: string): Date {
    const [hours, minutes, seconds] = timeString.split(':');
    const date = new Date();
    date.setHours(+hours);
    date.setMinutes(+minutes);
    date.setSeconds(+seconds);
    return date;
  }

  

togglePriceFilter() {
    this.showPriceOptions = !this.showPriceOptions; // Toggle price options visibility
    if (this.showTimeOptions) { // Close time options if open
      this.showTimeOptions = false;
    }
  }

  toggleTimeFilter() {
    this.showTimeOptions = !this.showTimeOptions; // Toggle time options visibility
    if (this.showPriceOptions) { // Close price options if open
      this.showPriceOptions = false;
    }
  }

}
