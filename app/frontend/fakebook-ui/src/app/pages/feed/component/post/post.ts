import { CommonModule, DecimalPipe } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { PostComment } from '../post-comment/post-comment';

@Component({
  selector: 'app-post',
  imports: [MatIconModule, DecimalPipe, CommonModule, PostComment],
  templateUrl: './post.html',
  styleUrl: './post.scss',
})
export class Post {
  liked = false;
  likeCount = 1284;

  comments = [
    {
      avatar: '/assets/design/4.post-comment-avatar.jpg',
      name: 'Emma Carter',
      username: 'emmacarter',
      time: '15 min',
      text: 'Loved the cabin view — this is exactly the kind of weekend reset I need.',
      likes: 12,
    },
    {
      avatar: '/assets/design/4.post-comment-avatar.jpg',
      name: 'Noah Ellis',
      username: 'noahellis',
      time: '17 min',
      text: 'That fire looks perfect. Wish I was there too.',
      likes: 2,
    },
    {
      avatar: '/assets/design/4.post-comment-avatar.jpg',
      name: 'Mia Reynolds',
      username: 'miareynolds',
      time: '3 min',
      text: 'The light and colors are unreal — great shot!',
      likes: 0,
    },
    {
      avatar: '/assets/design/4.post-comment-avatar.jpg',
      name: 'Liam Brooks',
      username: 'liambrooks',
      time: '1 min',
      text: 'That trail looks like a hidden gem. I need the location.',
      likes: 5,
    },
  ];

  toggleLike() {
    this.liked = !this.liked;
    this.likeCount += this.liked ? 1 : -1;
  }

  showComment = false;

  toggleComment() {
    this.showComment = !this.showComment;
  }
}
