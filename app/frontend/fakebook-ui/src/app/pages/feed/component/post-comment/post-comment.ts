import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommentItem } from '../comment-item/comment-item';

interface CommentData {
  avatar: string;
  name: string;
  username: string;
  time: string;
  text: string;
  likes: number;
}

@Component({
  selector: 'app-post-comment',
  imports: [CommonModule, CommentItem],
  templateUrl: './post-comment.html',
  styleUrl: './post-comment.scss',
})
export class PostComment {
  @Input() comments: CommentData[] = [];
}
