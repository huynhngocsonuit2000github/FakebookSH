import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Store } from '@ngrx/store';
import { CreatePostRequest } from '../../../../core/models/feed.models';
import { selectAuthUser } from '../../../../state/auth/auth.reducer';

@Component({
  selector: 'app-post-create-modal',
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './post-create-modal.html',
  styleUrl: './post-create-modal.scss',
})
export class PostCreateModal {
  private store = inject(Store);

  @Output() close = new EventEmitter<void>();
  @Output() create = new EventEmitter<CreatePostRequest>();

  user$ = this.store.select(selectAuthUser);
  content = '';
  image = '';
  hashtagInput = '';
  location = '';
  feeling = '';
  visibility: 'friends' | 'public' = 'friends';
  hashtags: string[] = [];
  avatar = '/assets/design/1.avatar.webp';

  addHashtag(): void {
    const normalizedTag = this.normalizeHashtag(this.hashtagInput);

    if (!normalizedTag || this.hashtags.includes(normalizedTag)) {
      this.hashtagInput = '';
      return;
    }

    this.hashtags = [...this.hashtags, normalizedTag];
    this.hashtagInput = '';
  }

  removeHashtag(tag: string): void {
    this.hashtags = this.hashtags.filter((currentTag) => currentTag !== tag);
  }

  handleHashtagEnter(event: Event): void {
    event.preventDefault();
    this.addHashtag();
  }

  onClose(): void {
    this.close.emit();
  }

  submitPost(): void {
    if (!this.content.trim()) {
      return;
    }

    this.create.emit({
      content: this.content.trim(),
      visibility: this.visibility,
      feeling: this.feeling.trim() || null,
      location: this.location.trim() || null,
      hashtags: [...this.hashtags],
      image: this.image.trim() || null,
    });

    this.resetForm();
    this.onClose();
  }

  private normalizeHashtag(value: string): string {
    const normalizedValue = value.trim().replace(/\s+/g, '');

    if (!normalizedValue) {
      return '';
    }

    return normalizedValue.startsWith('#') ? normalizedValue : `#${normalizedValue}`;
  }

  private resetForm(): void {
    this.content = '';
    this.image = '';
    this.hashtagInput = '';
    this.location = '';
    this.feeling = '';
    this.visibility = 'friends';
    this.hashtags = [];
  }
}
