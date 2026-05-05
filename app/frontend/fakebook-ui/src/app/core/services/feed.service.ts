import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { AddCommentRequest, CreatePostRequest, FeedPage, FeedPost } from '../models/feed.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FeedService {
  private readonly feedPageSize = 5;
  private readonly baseUrl = `${environment.bffBaseUrl}/feed`;
  private readonly postsSubject = new BehaviorSubject<FeedPost[]>([]);
  private readonly ownPostsSubject = new BehaviorSubject<FeedPost[]>([]);
  private readonly savedPostsSubject = new BehaviorSubject<FeedPost[]>([]);
  private readonly hasMoreFeedPostsSubject = new BehaviorSubject<boolean>(true);
  private nextFeedCursor: string | null = null;

  posts$ = this.postsSubject.asObservable();
  ownPosts$ = this.ownPostsSubject.asObservable();
  savedPosts$ = this.savedPostsSubject.asObservable();
  hasMoreFeedPosts$ = this.hasMoreFeedPostsSubject.asObservable();

  constructor(private http: HttpClient) {}

  getFeed(): Observable<FeedPage> {
    this.nextFeedCursor = null;
    this.hasMoreFeedPostsSubject.next(true);

    return this.http
      .get<FeedPage>(`${this.baseUrl}?limit=${this.feedPageSize}`)
      .pipe(tap((page) => this.applyFeedPage(page, false)));
  }

  getMoreFeedPosts(): Observable<FeedPage> {
    if (!this.hasMoreFeedPostsSubject.value || !this.nextFeedCursor) {
      return of({
        posts: this.postsSubject.value,
        nextCursor: this.nextFeedCursor,
        hasMore: false,
      });
    }

    return this.http
      .get<FeedPage>(`${this.baseUrl}?limit=${this.feedPageSize}&cursor=${encodeURIComponent(this.nextFeedCursor)}`)
      .pipe(tap((page) => this.applyFeedPage(page, true)));
  }

  getOwnPosts(): Observable<FeedPost[]> {
    return this.http
      .get<FeedPost[]>(`${this.baseUrl}/me/posts`)
      .pipe(tap((posts) => this.ownPostsSubject.next(posts)));
  }

  getSavedPosts(): Observable<FeedPost[]> {
    return this.http
      .get<FeedPost[]>(`${this.baseUrl}/saved`)
      .pipe(tap((posts) => this.savedPostsSubject.next(posts)));
  }

  createPost(request: CreatePostRequest): Observable<FeedPost> {
    return this.http.post<FeedPost>(`${this.baseUrl}/posts`, request).pipe(
      tap((post) => {
        this.postsSubject.next([post, ...this.postsSubject.value]);
        this.ownPostsSubject.next([post, ...this.ownPostsSubject.value]);
      }),
    );
  }

  toggleReaction(postId: string): Observable<FeedPost> {
    return this.http
      .post<FeedPost>(`${this.baseUrl}/posts/${postId}/reaction`, {})
      .pipe(tap((post) => this.updatePostAcrossSubjects(post)));
  }

  addComment(postId: string, request: AddCommentRequest): Observable<FeedPost> {
    return this.http
      .post<FeedPost>(`${this.baseUrl}/posts/${postId}/comments`, request)
      .pipe(tap((post) => this.updatePostAcrossSubjects(post)));
  }

  toggleSavedPost(postId: string): Observable<FeedPost> {
    return this.http
      .post<FeedPost>(`${this.baseUrl}/posts/${postId}/saved`, {})
      .pipe(tap((post) => this.updateSavedState(post)));
  }

  private updatePostAcrossSubjects(updatedPost: FeedPost): void {
    this.postsSubject.next(this.replacePost(this.postsSubject.value, updatedPost));
    this.ownPostsSubject.next(this.replacePost(this.ownPostsSubject.value, updatedPost));
    this.savedPostsSubject.next(this.replacePost(this.savedPostsSubject.value, updatedPost));
  }

  private updateSavedState(updatedPost: FeedPost): void {
    this.postsSubject.next(this.replacePost(this.postsSubject.value, updatedPost));
    this.ownPostsSubject.next(this.replacePost(this.ownPostsSubject.value, updatedPost));

    this.savedPostsSubject.next(
      updatedPost.isSaved
        ? this.upsertPost(this.savedPostsSubject.value, updatedPost)
        : this.savedPostsSubject.value.filter((post) => post.id !== updatedPost.id),
    );
  }

  private replacePost(posts: FeedPost[], updatedPost: FeedPost): FeedPost[] {
    return posts.map((post) => (post.id === updatedPost.id ? updatedPost : post));
  }

  private upsertPost(posts: FeedPost[], updatedPost: FeedPost): FeedPost[] {
    const existingPost = posts.some((post) => post.id === updatedPost.id);

    return existingPost ? this.replacePost(posts, updatedPost) : [updatedPost, ...posts];
  }

  private applyFeedPage(page: FeedPage, append: boolean): void {
    this.nextFeedCursor = page.nextCursor;
    this.hasMoreFeedPostsSubject.next(page.hasMore);

    this.postsSubject.next(append ? [...this.postsSubject.value, ...page.posts] : page.posts);
  }
}
