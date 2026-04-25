import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SuggestedFriends } from '../suggested-friends/suggested-friends';
import { TrendingNow } from '../trending-now/trending-now';

interface SuggestedFriend {
  name: string;
  mutualFriends: number;
  avatar: string;
}

@Component({
  selector: 'app-right-sidebar',
  imports: [CommonModule, SuggestedFriends, TrendingNow],
  templateUrl: './right-sidebar.html',
  styleUrl: './right-sidebar.scss',
})
export class RightSidebar {}
