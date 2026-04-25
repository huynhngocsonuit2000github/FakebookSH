import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SuggestedFriends } from '../suggested-friends/suggested-friends';

interface SuggestedFriend {
  name: string;
  mutualFriends: number;
  avatar: string;
}

@Component({
  selector: 'app-right-sidebar',
  imports: [CommonModule, SuggestedFriends],
  templateUrl: './right-sidebar.html',
  styleUrl: './right-sidebar.scss',
})
export class RightSidebar {}
