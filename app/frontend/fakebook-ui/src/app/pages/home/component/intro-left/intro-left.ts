import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-intro-left',
  imports: [CommonModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './intro-left.html',
  styleUrl: './intro-left.scss',
})
export class IntroLeft {
  avatars = ['👩🏻', '👨🏿', '👱🏼‍♀️', '👨🏽'];
}
