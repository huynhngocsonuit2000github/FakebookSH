import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

interface SidebarMenuItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-left-sidebar',
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './left-sidebar.html',
  styleUrl: './left-sidebar.scss',
})
export class LeftSidebar {
  menuItems: SidebarMenuItem[] = [
    { label: 'Home', route: '/feed', icon: 'home' },
    { label: 'Profile', route: '/profile', icon: 'person' },
    { label: 'Friends', route: '/friends', icon: 'group' },
    { label: 'Saved', route: '/saved', icon: 'bookmark' },
    { label: 'Messages', route: '/messages', icon: 'chat' },
    { label: 'Notifications', route: '/notifications', icon: 'notifications' },
    { label: 'Explore', route: '/explore', icon: 'explore' },
    { label: 'Media', route: '/media', icon: 'image' },
    { label: 'Settings', route: '/settings', icon: 'settings' },
  ];
}
