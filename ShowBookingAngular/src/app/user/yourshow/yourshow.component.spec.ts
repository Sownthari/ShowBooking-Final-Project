import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YourshowComponent } from './yourshow.component';

describe('YourshowComponent', () => {
  let component: YourshowComponent;
  let fixture: ComponentFixture<YourshowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YourshowComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(YourshowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
