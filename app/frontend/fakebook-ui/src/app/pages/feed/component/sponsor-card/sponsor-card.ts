import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-sponsor-card',
  imports: [CommonModule],
  templateUrl: './sponsor-card.html',
  styleUrl: './sponsor-card.scss',
})
export class SponsorCard {
  @Input() title = 'Bring your community to Nova';
  @Input() description = 'Free tools for creators and groups.';
  @Input() imageUrl = '';
  @Input() imageAlt = 'Sponsored banner';
}
