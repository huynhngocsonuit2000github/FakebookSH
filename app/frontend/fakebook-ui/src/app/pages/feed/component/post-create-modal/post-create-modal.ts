import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-post-create-modal',
  imports: [FormsModule, MatIconModule],
  templateUrl: './post-create-modal.html',
  styleUrl: './post-create-modal.scss',
})
export class PostCreateModal {
  @Output() close = new EventEmitter<void>();
  @Output() create = new EventEmitter<string>();

  content = '';
  avatar = '/assets/design/1.avatar.webp';

  onClose() {
    this.close.emit();
  }

  submitPost() {
    if (!this.content.trim()) return;

    this.create.emit(this.content.trim());
    this.content = '';
    this.onClose();
  }
}
