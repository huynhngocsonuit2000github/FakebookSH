import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

interface TrendingTopic {
  tag: string;
  posts: string;
}

@Component({
  selector: 'app-trending-now',
  imports: [CommonModule, MatIconModule],
  templateUrl: './trending-now.html',
  styleUrl: './trending-now.scss',
})
export class TrendingNow {
  topics: TrendingTopic[] = [
    { tag: '#designsystems', posts: '12.4k posts' },
    { tag: '#weekendread', posts: '8.1k posts' },
    { tag: '#novalaunch', posts: '5.7k posts' },
    { tag: '#slowinternet', posts: '3.2k posts' },
    { tag: '#homecooking', posts: '2.9k posts' },
  ];
}
