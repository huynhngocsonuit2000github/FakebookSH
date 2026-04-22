import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SigninHero } from './signin-hero';

describe('SigninHero', () => {
  let component: SigninHero;
  let fixture: ComponentFixture<SigninHero>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SigninHero],
    }).compileComponents();

    fixture = TestBed.createComponent(SigninHero);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
