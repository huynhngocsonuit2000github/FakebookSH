import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { StoryList } from './component/story-list/story-list';
import { PostCreate } from './component/post-create/post-create';
import { Post } from './component/post/post';

interface CommentData {
  avatar: string;
  name: string;
  username: string;
  time: string;
  text: string;
  likes: number;
}

export interface FeedPost {
  author: string;
  username: string;
  avatar: string;
  time: string;
  visibility: string;
  content: string;
  hashtags: string[];
  image: string;
  likeCount: number;
  shareCount: number;
  comments: CommentData[];
}

@Component({
  selector: 'app-feed',
  imports: [CommonModule, StoryList, PostCreate, Post],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed {
  posts: FeedPost[] = [
    {
      author: 'Jordan Park',
      username: 'jordanp',
      avatar: 'https://i.pravatar.cc/100?img=12',
      time: '4h',
      visibility: 'public',
      content:
        'Sunrise from the cabin. No notifications, no dashboards — just coffee and a really stubborn fire.',
      hashtags: ['#weekend', '#outdoors'],
      image: '/assets/design/5.post-1-background.jpg',
      likeCount: 1284,
      shareCount: 31,
      comments: [
        {
          avatar: 'https://i.pravatar.cc/100?img=47',
          name: 'Emma Carter',
          username: 'emmacarter',
          time: '15 min',
          text: 'Loved the cabin view — this is exactly the kind of weekend reset I need.',
          likes: 12,
        },
        {
          avatar: 'https://i.pravatar.cc/100?img=32',
          name: 'Noah Ellis',
          username: 'noahellis',
          time: '17 min',
          text: 'That fire looks perfect. Wish I was there too.',
          likes: 2,
        },
      ],
    },
    {
      author: 'Mia Reynolds',
      username: 'miareynolds',
      avatar: 'https://i.pravatar.cc/100?img=25',
      time: '1d',
      visibility: 'friends',
      content:
        'Just wrapped up a day hike and the sunset from the ridge was unreal. This place is a dream.',
      hashtags: ['#hiking', '#sunset', '#nature'],
      image: '/assets/design/6.post-2-background.jpg',
      likeCount: 2264,
      shareCount: 13,
      comments: [
        {
          avatar: 'https://i.pravatar.cc/100?img=18',
          name: 'Liam Brooks',
          username: 'liambrooks',
          time: '2h',
          text: 'That view is incredible. Need the trail details!',
          likes: 5,
        },
      ],
    },
    {
      author: 'Sofia Almeida',
      username: 'sofiaa',
      avatar: 'https://i.pravatar.cc/100?img=56',
      time: '3d',
      visibility: 'public',
      content:
        'Coffee, a warm campfire, and a fresh morning breeze. These are the small moments that matter.',
      hashtags: ['#camping', '#coffeetime'],
      image: '/assets/design/7.post-3-background.jpg',
      likeCount: 984,
      shareCount: 19,
      comments: [
        {
          avatar: 'https://i.pravatar.cc/100?img=14',
          name: 'Ava Brooks',
          username: 'avabrooks',
          time: '4h',
          text: 'This whole vibe is goals. Love the calm energy.',
          likes: 8,
        },
        {
          avatar: 'https://i.pravatar.cc/100?img=11',
          name: 'Ethan Gray',
          username: 'ethangray',
          time: '1h',
          text: 'Can you share the coffee blend? Looks amazing.',
          likes: 0,
        },
      ],
    },
  ];
}
