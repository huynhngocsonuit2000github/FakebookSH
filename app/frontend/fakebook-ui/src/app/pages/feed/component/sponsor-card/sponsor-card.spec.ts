import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SponsorCard } from './sponsor-card';

describe('SponsorCard', () => {
  let component: SponsorCard;
  let fixture: ComponentFixture<SponsorCard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SponsorCard],
    }).compileComponents();

    fixture = TestBed.createComponent(SponsorCard);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
