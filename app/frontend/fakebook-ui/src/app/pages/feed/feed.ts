import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { StoryList } from './component/story-list/story-list';
import { PostCreate } from './component/post-create/post-create';
import { Post } from './component/post/post';
import { RightSidebar } from './component/right-sidebar/right-sidebar';
import { FeedService } from '../../core/services/feed.service';
import { CreatePostRequest, FeedPost } from '../../core/models/feed.models';

@Component({
  selector: 'app-feed',
  imports: [CommonModule, StoryList, PostCreate, Post, RightSidebar],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed implements OnInit, AfterViewInit, OnDestroy {
  readonly posts$: Observable<FeedPost[]>;
  readonly hasMorePosts$: Observable<boolean>;

  @ViewChild('feedScrollAnchor') private feedScrollAnchor?: ElementRef<HTMLDivElement>;

  loading = false;
  loadingMore = false;
  hasMore = true;
  error: string | null = null;
  private intersectionObserver?: IntersectionObserver;
  private hasMoreSubscription?: Subscription;

  constructor(private feedService: FeedService) {
    this.posts$ = this.feedService.posts$;
    this.hasMorePosts$ = this.feedService.hasMoreFeedPosts$;
  }

  ngOnInit(): void {
    this.hasMoreSubscription = this.hasMorePosts$.subscribe((hasMore) => {
      this.hasMore = hasMore;
    });

    this.loadFeed();
  }

  ngAfterViewInit(): void {
    if (!this.feedScrollAnchor) {
      return;
    }

    this.intersectionObserver = new IntersectionObserver(
      (entries) => {
        if (entries.some((entry) => entry.isIntersecting)) {
          this.loadMoreFeed();
        }
      },
      { rootMargin: '240px 0px' },
    );

    this.intersectionObserver.observe(this.feedScrollAnchor.nativeElement);
  }

  ngOnDestroy(): void {
    this.intersectionObserver?.disconnect();
    this.hasMoreSubscription?.unsubscribe();
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

  loadMoreFeed(): void {
    if (this.loading || this.loadingMore || !this.hasMore || this.error) {
      return;
    }

    this.loadingMore = true;

    this.feedService.getMoreFeedPosts().subscribe({
      next: () => {
        this.loadingMore = false;
      },
      error: () => {
        this.error = 'Cannot load more posts right now.';
        this.loadingMore = false;
      },
    });
  }

  createPost(request: CreatePostRequest): void {
    this.feedService.createPost(request).subscribe({
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
