import { Component } from '@angular/core';
import { IntroLeft } from './component/intro-left/intro-left';
import { IntroRight } from './component/intro-right/intro-right';
import { FeaturesSection } from './component/features-section/features-section';
import { Community } from './component/community/community';

@Component({
  selector: 'app-home',
  imports: [IntroLeft, IntroRight, FeaturesSection, Community],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {}
