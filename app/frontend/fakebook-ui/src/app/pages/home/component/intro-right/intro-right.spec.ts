import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IntroRight } from './intro-right';

describe('IntroRight', () => {
  let component: IntroRight;
  let fixture: ComponentFixture<IntroRight>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IntroRight],
    }).compileComponents();

    fixture = TestBed.createComponent(IntroRight);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
