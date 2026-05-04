import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { AddCommentRequest, CreatePostRequest, FeedPost } from '../models/feed.models';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class FeedService {
  private readonly baseUrl = `${environment.bffBaseUrl}/feed`;
  private readonly postsSubject = new BehaviorSubject<FeedPost[]>([]);
  private readonly ownPostsSubject = new BehaviorSubject<FeedPost[]>([]);
  private readonly savedPostsSubject = new BehaviorSubject<FeedPost[]>([]);

  posts$ = this.postsSubject.asObservable();
  ownPosts$ = this.ownPostsSubject.asObservable();
  savedPosts$ = this.savedPostsSubject.asObservable();

  constructor(private http: HttpClient) {}

  getFeed(): Observable<FeedPost[]> {
    return this.http
      .get<FeedPost[]>(this.baseUrl)
      .pipe(tap((posts) => this.postsSubject.next(posts)));
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
}
