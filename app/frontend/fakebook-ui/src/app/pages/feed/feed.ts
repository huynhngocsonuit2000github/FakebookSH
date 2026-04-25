import { Component } from '@angular/core';
import { StoryList } from './component/story-list/story-list';
import { PostCreate } from './component/post-create/post-create';

@Component({
  selector: 'app-feed',
  imports: [StoryList, PostCreate],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed {}
