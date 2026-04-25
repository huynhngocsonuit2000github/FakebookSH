import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuggestedFriends } from './suggested-friends';

describe('SuggestedFriends', () => {
  let component: SuggestedFriends;
  let fixture: ComponentFixture<SuggestedFriends>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuggestedFriends],
    }).compileComponents();

    fixture = TestBed.createComponent(SuggestedFriends);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
