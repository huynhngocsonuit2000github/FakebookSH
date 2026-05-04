import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { FeedService } from '../../core/services/feed.service';
import { FeedPost } from '../../core/models/feed.models';
import { Post } from '../feed/component/post/post';

@Component({
  selector: 'app-saved',
  imports: [CommonModule, Post],
  templateUrl: './saved.html',
  styleUrl: './saved.scss',
})
export class Saved implements OnInit {
  readonly posts$: Observable<FeedPost[]>;
  loading = false;
  error: string | null = null;

  constructor(private feedService: FeedService) {
    this.posts$ = this.feedService.savedPosts$;
  }

  ngOnInit(): void {
    this.loadSavedPosts();
  }

  loadSavedPosts(): void {
    this.loading = true;
    this.error = null;

    this.feedService.getSavedPosts().subscribe({
      next: () => {
        this.loading = false;
      },
      error: () => {
        this.error = 'Cannot load saved posts right now.';
        this.loading = false;
      },
    });
  }

  toggleReaction(postId: string): void {
    this.feedService.toggleReaction(postId).subscribe({
      next: () => {
        this.error = null;
      },
      error: () => {
        this.error = 'Cannot update reaction right now.';
      },
    });
  }

  addComment(event: { postId: string; text: string }): void {
    this.feedService.addComment(event.postId, { text: event.text }).subscribe({
      next: () => {
        this.error = null;
      },
      error: () => {
        this.error = 'Cannot add comment right now.';
      },
    });
  }

  toggleSavedPost(postId: string): void {
    this.feedService.toggleSavedPost(postId).subscribe({
      next: () => {
        this.error = null;
      },
      error: () => {
        this.error = 'Cannot update saved post right now.';
      },
    });
  }

  trackByPostId(_: number, post: FeedPost): string {
    return post.id;
  }
}
