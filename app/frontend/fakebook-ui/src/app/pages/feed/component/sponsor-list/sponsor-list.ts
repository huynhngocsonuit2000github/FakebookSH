import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { SponsorCard } from '../sponsor-card/sponsor-card';

interface Sponsor {
  title: string;
  description: string;
  imageUrl: string;
  imageAlt: string;
}

@Component({
  selector: 'app-sponsor-list',
  imports: [CommonModule, SponsorCard],
  templateUrl: './sponsor-list.html',
  styleUrl: './sponsor-list.scss',
})
export class SponsorList {
  sponsors: Sponsor[] = [
    {
      title: 'Bring your community to Nova',
      description: 'Free tools for creators and groups.',
      imageUrl: '',
      imageAlt: 'Colorful sponsored banner',
    },
    {
      title: 'Plan better weekends',
      description: 'Find local events, food spots, and quick getaways near you.',
      imageUrl: '',
      imageAlt: 'Weekend planning sponsored banner',
    },
  ];
}
