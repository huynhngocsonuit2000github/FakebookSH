import { Component } from '@angular/core';
import { IntroLeft } from './component/intro-left/intro-left';
import { IntroRight } from './component/intro-right/intro-right';

@Component({
  selector: 'app-home',
  imports: [IntroLeft, IntroRight],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {}
