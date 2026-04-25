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
      image: 'https://hadlyn.vn/wp-content/uploads/2026/02/nen-canva-2026-02-27T152459.343.png',
      avatar:
        'https://admin.vov.gov.vn/UploadFolder/KhoTin/Images/UploadFolder/VOVVN/Images/sites/default/files/styles/large/public/2023-12/2_16.jpeg.jpg',
    },
    {
      name: 'Jordan',
      image: 'https://hadlyn.vn/wp-content/uploads/2026/02/nen-canva-2026-02-27T152459.343.png',
      avatar:
        'https://admin.vov.gov.vn/UploadFolder/KhoTin/Images/UploadFolder/VOVVN/Images/sites/default/files/styles/large/public/2023-12/2_16.jpeg.jpg',
    },
    {
      name: 'Sofia',
      image: 'https://hadlyn.vn/wp-content/uploads/2026/02/nen-canva-2026-02-27T152459.343.png',
      avatar:
        'https://admin.vov.gov.vn/UploadFolder/KhoTin/Images/UploadFolder/VOVVN/Images/sites/default/files/styles/large/public/2023-12/2_16.jpeg.jpg',
    },
    {
      name: 'Theo',
      image: 'https://hadlyn.vn/wp-content/uploads/2026/02/nen-canva-2026-02-27T152459.343.png',
      avatar:
        'https://admin.vov.gov.vn/UploadFolder/KhoTin/Images/UploadFolder/VOVVN/Images/sites/default/files/styles/large/public/2023-12/2_16.jpeg.jpg',
    },
    {
      name: 'Priya',
      image: 'https://hadlyn.vn/wp-content/uploads/2026/02/nen-canva-2026-02-27T152459.343.png',
      avatar:
        'https://admin.vov.gov.vn/UploadFolder/KhoTin/Images/UploadFolder/VOVVN/Images/sites/default/files/styles/large/public/2023-12/2_16.jpeg.jpg',
    },
  ];
}
