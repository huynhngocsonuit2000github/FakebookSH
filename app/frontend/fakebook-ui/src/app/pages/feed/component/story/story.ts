import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-story',
  imports: [],
  templateUrl: './story.html',
  styleUrl: './story.scss',
})
export class Story {
  @Input() story!: StoryModel;

  constructor() {
    console.log(this.story);
  }
}

export interface StoryModel {
  name: string;
  image: string;
  avatar: string;
}
