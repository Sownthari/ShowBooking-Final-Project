import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrganizerStatisticsComponent } from './organizer-statistics.component';

describe('OrganizerStatisticsComponent', () => {
  let component: OrganizerStatisticsComponent;
  let fixture: ComponentFixture<OrganizerStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrganizerStatisticsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(OrganizerStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
