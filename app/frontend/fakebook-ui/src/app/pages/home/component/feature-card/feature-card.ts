import { Component, Input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-feature-card',
  imports: [MatIconModule, MatCardModule],
  templateUrl: './feature-card.html',
  styleUrl: './feature-card.scss',
})
export class FeatureCard {
  @Input({ required: true }) item!: FeatureItem;
}

export interface FeatureItem {
  icon: string;
  title: string;
  description: string;
}
