import { Component } from '@angular/core';
import { IntroLeft } from './component/intro-left/intro-left';
import { IntroRight } from './component/intro-right/intro-right';
import { FeaturesSection } from './component/features-section/features-section';
import { Community } from './component/community/community';
import { Whyfb } from './component/whyfb/whyfb';

@Component({
  selector: 'app-home',
  imports: [IntroLeft, IntroRight, FeaturesSection, Community, Whyfb],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {}
