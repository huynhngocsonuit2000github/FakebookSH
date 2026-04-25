import { Component } from '@angular/core';
import { StoryList } from './component/story-list/story-list';

@Component({
  selector: 'app-feed',
  imports: [StoryList],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed {}
