import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-comment-item',
  imports: [CommonModule, MatIconModule],
  templateUrl: './comment-item.html',
  styleUrl: './comment-item.scss',
})
export class CommentItem {
  @Input() avatar = '';
  @Input() name = '';
  @Input() username = '';
  @Input() time = '';
  @Input() text = '';
  @Input() likes = 0;
}
