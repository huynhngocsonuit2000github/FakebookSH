import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeatureCard, FeatureItem } from '../feature-card/feature-card';

@Component({
  selector: 'app-features-section',
  standalone: true,
  imports: [CommonModule, FeatureCard],
  templateUrl: './features-section.html',
  styleUrl: './features-section.scss',
})
export class FeaturesSection {
  features: FeatureItem[] = [
    {
      icon: 'auto_awesome',
      title: 'A calmer feed',
      description:
        'Signal over noise. Smart grouping and gentle defaults so your timeline never shouts.',
    },
    {
      icon: 'chat_bubble_outline',
      title: 'Real conversations',
      description:
        'Threaded chats, presence, reactions, and shared media — built for closeness, not metrics.',
    },
    {
      icon: 'shield_outlined',
      title: 'You own your space',
      description:
        'Granular privacy, per-post audiences, and one-tap “show me less” that actually works.',
    },
    {
      icon: 'bolt',
      title: 'Fast everywhere',
      description: 'Loads in a blink on every device. Designed mobile-first, polished on desktop.',
    },
    {
      icon: 'groups_2',
      title: 'Find your people',
      description: 'Discover communities and creators around what you actually care about.',
    },
    {
      icon: 'public',
      title: 'Open by default',
      description: 'Share publicly, in groups, or just with friends. You decide who sees what.',
    },
  ];
}
