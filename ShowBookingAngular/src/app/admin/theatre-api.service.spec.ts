import { TestBed } from '@angular/core/testing';

import { TheatreApiService } from './theatre-api.service';

describe('TheatreApiService', () => {
  let service: TheatreApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TheatreApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
