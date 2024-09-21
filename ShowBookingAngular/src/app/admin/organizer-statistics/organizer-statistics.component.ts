import { Component } from '@angular/core';
import { StatisticsService } from '../../super-admin/statistics.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-organizer-statistics',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './organizer-statistics.component.html',
  styleUrl: './organizer-statistics.component.css'
})
export class OrganizerStatisticsComponent {
  occupancyRates: any[] = [];
  theatreId: number = 0;
  startDate?: Date;
  endDate?: Date;
  screenType?: string;
  salesData: any[] = [];
  period: string = 'daily'; 


  constructor(private statisticsService: StatisticsService, private route: ActivatedRoute) {
    this.theatreId = this.route.snapshot.params['id'];
  }

  ngOnInit(): void {
  }

  fetchOccupancyRate(): void {
    this.statisticsService.getOccupancyRate(this.theatreId, this.startDate, this.endDate, this.screenType).subscribe((data: any[]) => {
      this.occupancyRates = data;
    });
  }
  fetchTicketSalesTrend(): void {
    
    this.statisticsService.getTicketSalesTrend(this.theatreId, this.period).subscribe((data: any[]) => {
      this.salesData = data;
    });
  }
}
