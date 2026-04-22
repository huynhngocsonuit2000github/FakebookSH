import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-whyfb',
  imports: [CommonModule, MatCardModule, MatIconModule],
  templateUrl: './whyfb.html',
  styleUrl: './whyfb.scss',
})
export class Whyfb {
  testimonials = [
    {
      name: 'Maya Chen',
      role: 'Product designer',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Maya',
      quote:
        "Nova is the first feed in years that doesn't make me feel weird after twenty minutes.",
    },
    {
      name: 'Theo Nakamura',
      role: 'Photographer',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Theo',
      quote: 'Finally a place where my work is the loudest thing on the page.',
    },
    {
      name: 'Priya Shah',
      role: 'Community lead',
      avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Priya',
      quote: 'We moved our whole community here. Engagement is up, anxiety is down.',
    },
  ];
}
