import { Component } from '@angular/core';
import { Story, StoryModel } from '../story/story';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-story-list',
  imports: [Story, CommonModule],
  templateUrl: './story-list.html',
  styleUrl: './story-list.scss',
})
export class StoryList {
  stories: StoryModel[] = [
    {
      name: 'Maya',
      image: '/assets/design/2.story-background.png',
      avatar: '/assets/design/3.story-avatar.jpg',
    },
    {
      name: 'Jordan',
      image: '/assets/design/2.story-background.png',
      avatar: '/assets/design/3.story-avatar.jpg',
    },
    {
      name: 'Sofia',
      image: '/assets/design/2.story-background.png',
      avatar: '/assets/design/3.story-avatar.jpg',
    },
    {
      name: 'Theo',
      image: '/assets/design/2.story-background.png',
      avatar: '/assets/design/3.story-avatar.jpg',
    },
    {
      name: 'Priya',
      image: '/assets/design/2.story-background.png',
      avatar: '/assets/design/3.story-avatar.jpg',
    },
  ];
}
