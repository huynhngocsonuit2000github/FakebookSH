import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  imports: [CommonModule],
  templateUrl: './footer.html',
  styleUrl: './footer.scss',
})
export class Footer {
  productItems = [
    { label: 'Features', link: '#' },
    { label: 'Community', link: '#' },
    { label: 'Why Nova', link: '#' },
  ];

  companyItems = [
    { label: 'About Us', link: '#' },
    { label: 'Careers', link: '#' },
    { label: 'Contact Us', link: '#' },
  ];
}
