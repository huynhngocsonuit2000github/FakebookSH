import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { CreatePostRequest } from '../../../../core/models/feed.models';
import { PostCreateModal } from '../post-create-modal/post-create-modal';

@Component({
  selector: 'app-post-create',
  imports: [PostCreateModal, CommonModule],
  templateUrl: './post-create.html',
  styleUrl: './post-create.scss',
})
export class PostCreate {
  @Output() postCreated = new EventEmitter<CreatePostRequest>();

  username = 'Alex';
  avatar = '/assets/design/1.avatar.webp';
  showCreatePostModal = false;

  openModal(): void {
    this.showCreatePostModal = true;
  }

  closeModal(): void {
    this.showCreatePostModal = false;
  }

  createPost(request: CreatePostRequest): void {
    this.postCreated.emit(request);
    this.closeModal();
  }
}
