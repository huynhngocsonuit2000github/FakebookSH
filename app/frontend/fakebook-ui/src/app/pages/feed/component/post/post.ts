import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { PostComment } from '../post-comment/post-comment';
import { FeedPost } from '../../../../core/models/feed.models';

@Component({
  selector: 'app-post',
  imports: [MatIconModule, DecimalPipe, CommonModule, FormsModule, PostComment],
  templateUrl: './post.html',
  styleUrl: './post.scss',
})
export class Post {
  @Input() post: FeedPost = {
    id: '',
    authorId: '',
    author: 'Unknown',
    username: 'unknown',
    avatar: '/assets/design/4.post-comment-avatar.jpg',
    time: '0m',
    visibility: 'public',
    content: '',
    feeling: null,
    location: null,
    hashtags: [],
    image: null,
    likeCount: 0,
    shareCount: 0,
    isLiked: false,
    isSaved: false,
    comments: [],
  };

  @Output() reactionToggled = new EventEmitter<string>();
  @Output() savedToggled = new EventEmitter<string>();
  @Output() commentAdded = new EventEmitter<{ postId: string; text: string }>();

  showComment = false;
  newComment = '';

  toggleLike() {
    this.reactionToggled.emit(this.post.id);
  }

  toggleComment() {
    this.showComment = !this.showComment;
  }

  toggleSaved() {
    this.savedToggled.emit(this.post.id);
  }

  submitComment() {
    const text = this.newComment.trim();

    if (!text) {
      return;
    }

    this.commentAdded.emit({ postId: this.post.id, text });
    this.newComment = '';
    this.showComment = true;
  }
}
