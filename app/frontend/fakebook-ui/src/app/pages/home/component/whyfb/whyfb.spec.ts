import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Whyfb } from './whyfb';

describe('Whyfb', () => {
  let component: Whyfb;
  let fixture: ComponentFixture<Whyfb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Whyfb],
    }).compileComponents();

    fixture = TestBed.createComponent(Whyfb);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
