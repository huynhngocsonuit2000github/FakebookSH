import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MenuItem } from '../../models/share-model';
import { RouterModule } from '@angular/router';
import { UserMenu } from '../user-menu/user-menu';

@Component({
  selector: 'app-header',
  imports: [MatButtonModule, CommonModule, MatIconModule, RouterModule, UserMenu],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  menus: MenuItem[] = [
    { label: 'Features', link: '#' },
    { label: 'Community', link: '#' },
    { label: 'Why Fakebook', link: '#' },
  ];

  userMenuOpen = false;

  toggleUserMenu() {
    this.userMenuOpen = !this.userMenuOpen;
  }

  @HostListener('document:click', ['$event.target'])
  onDocumentClick(target: EventTarget | null) {
    if (!(target instanceof HTMLElement)) {
      return;
    }

    const clickedInside = target.closest('.user-menu');
    if (!clickedInside) {
      this.userMenuOpen = false;
    }
  }
}
