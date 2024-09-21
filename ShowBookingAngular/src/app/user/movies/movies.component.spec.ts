import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AvailableMoviesComponent } from './movies.component';



describe('MoviesComponent', () => {
  let component: AvailableMoviesComponent;
  let fixture: ComponentFixture<AvailableMoviesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailableMoviesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AvailableMoviesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
