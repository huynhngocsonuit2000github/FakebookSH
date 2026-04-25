import { CommonModule, DecimalPipe } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { PostComment } from '../post-comment/post-comment';
import { FeedPost } from '../../feed';

interface CommentData {
  avatar: string;
  name: string;
  username: string;
  time: string;
  text: string;
  likes: number;
}

@Component({
  selector: 'app-post',
  imports: [MatIconModule, DecimalPipe, CommonModule, PostComment],
  templateUrl: './post.html',
  styleUrl: './post.scss',
})
export class Post {
  @Input() post: FeedPost = {
    author: 'Unknown',
    username: 'unknown',
    avatar: '/assets/design/4.post-comment-avatar.jpg',
    time: '0m',
    visibility: 'public',
    content: '',
    hashtags: [],
    image: '',
    likeCount: 0,
    shareCount: 0,
    comments: [],
  };

  liked = false;
  showComment = false;

  toggleLike() {
    this.liked = !this.liked;
    this.post.likeCount += this.liked ? 1 : -1;
  }

  toggleComment() {
    this.showComment = !this.showComment;
  }
}
