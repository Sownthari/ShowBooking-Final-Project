import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieShowsComponent } from './movie-shows.component';

describe('MovieShowsComponent', () => {
  let component: MovieShowsComponent;
  let fixture: ComponentFixture<MovieShowsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieShowsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MovieShowsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
