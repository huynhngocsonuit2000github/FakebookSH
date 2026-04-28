import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MenuItem } from '../../models/share-model';

@Component({
  selector: 'app-public-header',
  imports: [MatButtonModule, CommonModule],
  templateUrl: './public-header.html',
  styleUrl: './public-header.scss',
})
export class PublicHeader {
  menus: MenuItem[] = [
    { label: 'Features', link: '#' },
    { label: 'Community', link: '#' },
    { label: 'Why Nova', link: '#' },
  ];
}
