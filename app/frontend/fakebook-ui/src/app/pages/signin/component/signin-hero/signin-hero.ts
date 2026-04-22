import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-signin-hero',
  imports: [CommonModule, MatIconModule],
  templateUrl: './signin-hero.html',
  styleUrl: './signin-hero.scss',
})
export class SigninHero {
  users = [
    'https://api.dicebear.com/7.x/avataaars/svg?seed=User1',
    'https://api.dicebear.com/7.x/avataaars/svg?seed=User2',
    'https://api.dicebear.com/7.x/avataaars/svg?seed=User3',
    'https://api.dicebear.com/7.x/avataaars/svg?seed=User4',
  ];
}
