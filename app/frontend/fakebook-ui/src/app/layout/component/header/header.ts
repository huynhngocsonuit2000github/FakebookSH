import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MenuItem } from '../../models/share-model';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-header',
  imports: [MatButtonModule, CommonModule, RouterModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  menus: MenuItem[] = [
    { label: 'Features', link: '#' },
    { label: 'Community', link: '#' },
    { label: 'Why Fakebook', link: '#' },
  ];
}
