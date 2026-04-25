import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SuggestedFriends } from '../suggested-friends/suggested-friends';
import { TrendingNow } from '../trending-now/trending-now';
import { SponsorList } from '../sponsor-list/sponsor-list';
import { ContactList } from '../contact-list/contact-list';

@Component({
  selector: 'app-right-sidebar',
  imports: [CommonModule, SuggestedFriends, TrendingNow, SponsorList, ContactList],
  templateUrl: './right-sidebar.html',
  styleUrl: './right-sidebar.scss',
})
export class RightSidebar {}
