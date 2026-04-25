import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-contact',
  imports: [CommonModule],
  templateUrl: './contact.html',
  styleUrl: './contact.scss',
})
export class Contact {
  @Input() name = '';
  @Input() avatarUrl = '';
  @Input() fallbackEmoji = '👤';
  @Input() online = true;
}
