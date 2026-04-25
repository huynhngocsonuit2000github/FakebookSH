import { Component } from '@angular/core';
import { StoryList } from './component/story-list/story-list';
import { PostCreate } from './component/post-create/post-create';
import { Post } from './component/post/post';

@Component({
  selector: 'app-feed',
  imports: [StoryList, PostCreate, Post],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed {}
