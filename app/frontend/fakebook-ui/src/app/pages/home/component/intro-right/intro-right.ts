import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-intro-right',
  imports: [MatCardModule, MatIconModule],
  templateUrl: './intro-right.html',
  styleUrl: './intro-right.scss',
})
export class IntroRight {}
