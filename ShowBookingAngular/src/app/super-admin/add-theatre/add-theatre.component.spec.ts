import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddTheatreComponent } from './add-theatre.component';

describe('AddTheatreComponent', () => {
  let component: AddTheatreComponent;
  let fixture: ComponentFixture<AddTheatreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddTheatreComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddTheatreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
