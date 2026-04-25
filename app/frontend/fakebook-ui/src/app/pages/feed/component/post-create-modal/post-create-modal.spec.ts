import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostCreateModal } from './post-create-modal';

describe('PostCreateModal', () => {
  let component: PostCreateModal;
  let fixture: ComponentFixture<PostCreateModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PostCreateModal],
    }).compileComponents();

    fixture = TestBed.createComponent(PostCreateModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
