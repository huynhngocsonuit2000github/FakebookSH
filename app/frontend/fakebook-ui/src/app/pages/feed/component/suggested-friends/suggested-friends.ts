import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

interface SuggestedFriend {
  name: string;
  mutualFriends: number;
  avatar: string;
}

@Component({
  selector: 'app-suggested-friends',
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './suggested-friends.html',
  styleUrl: './suggested-friends.scss',
})
export class SuggestedFriends {
  friends: SuggestedFriend[] = [
    {
      name: 'Sofia Almeida',
      mutualFriends: 8,
      avatar: '/assets/design/8.suggested-fiend-avatar.jpg',
    },
    {
      name: 'Theo Nakamura',
      mutualFriends: 21,
      avatar: '/assets/design/8.suggested-fiend-avatar.jpg',
    },
    {
      name: 'Priya Shah',
      mutualFriends: 3,
      avatar: '/assets/design/8.suggested-fiend-avatar.jpg',
    },
    {
      name: "Liam O'Connor",
      mutualFriends: 15,
      avatar: '/assets/design/8.suggested-fiend-avatar.jpg',
    },
  ];
}
