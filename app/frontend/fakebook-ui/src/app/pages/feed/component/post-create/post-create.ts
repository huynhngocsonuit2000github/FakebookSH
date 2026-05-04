import { Component, EventEmitter, Output } from '@angular/core';
import { PostCreateModal } from '../post-create-modal/post-create-modal';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-post-create',
  imports: [PostCreateModal, CommonModule],
  templateUrl: './post-create.html',
  styleUrl: './post-create.scss',
})
export class PostCreate {
  @Output() postCreated = new EventEmitter<string>();

  username = 'Alex';
  avatar = '/assets/design/1.avatar.webp';
  showCreatePostModal = false;

  openModal() {
    this.showCreatePostModal = true;
  }

  closeModal() {
    this.showCreatePostModal = false;
  }

  createPost(content: string) {
    this.postCreated.emit(content);
    this.closeModal();
  }
}
