import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IntroLeft } from './intro-left';

describe('IntroLeft', () => {
  let component: IntroLeft;
  let fixture: ComponentFixture<IntroLeft>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IntroLeft],
    }).compileComponents();

    fixture = TestBed.createComponent(IntroLeft);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
