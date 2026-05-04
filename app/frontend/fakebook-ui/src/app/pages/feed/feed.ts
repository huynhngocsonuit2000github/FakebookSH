import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryList } from './component/story-list/story-list';
import { PostCreate } from './component/post-create/post-create';
import { Post } from './component/post/post';
import { RightSidebar } from './component/right-sidebar/right-sidebar';
import { FeedService } from '../../core/services/feed.service';
import { FeedPost } from '../../core/models/feed.models';

@Component({
  selector: 'app-feed',
  imports: [CommonModule, StoryList, PostCreate, Post, RightSidebar],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed implements OnInit {
  readonly posts$: Observable<FeedPost[]>;

  loading = false;
  error: string | null = null;

  constructor(private feedService: FeedService) {
    this.posts$ = this.feedService.posts$;
  }

  ngOnInit(): void {
    this.loadFeed();
  }

  loadFeed(): void {
    this.loading = true;
    this.error = null;

    this.feedService.getFeed().subscribe({
      next: () => {
        this.loading = false;
      },
      error: () => {
        this.error = 'Cannot load feed right now.';
        this.loading = false;
      },
    });
  }

  createPost(content: string): void {
    this.feedService.createPost({ content, visibility: 'friends' }).subscribe({
      next: () => {
        this.error = null;
      },
      error: () => {
        this.error = 'Cannot create post right now.';
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
