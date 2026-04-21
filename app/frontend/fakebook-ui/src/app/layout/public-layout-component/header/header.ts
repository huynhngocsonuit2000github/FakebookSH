import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MenuItem } from '../../models/share-model';

@Component({
  selector: 'app-header',
  imports: [MatButtonModule, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.scss',
})
export class Header {
  menus: MenuItem[] = [
    { label: 'Features', link: '#' },
    { label: 'Community', link: '#' },
    { label: 'Why Nova', link: '#' },
  ];
}
