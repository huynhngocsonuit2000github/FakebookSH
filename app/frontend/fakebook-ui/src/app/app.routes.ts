import { Routes } from '@angular/router';
import { PublicLayout } from './layout/public-layout/public-layout';
import { MainLayout } from './layout/main-layout/main-layout';

export const routes: Routes = [
  //   Public page
  {
    path: '',
    component: PublicLayout,
    children: [{ path: '', loadComponent: () => import('./pages/home/home').then((m) => m.Home) }],
  },

  // Main pages, after the user logged in, will be here, with main layout
  {
    path: 'feed',
    component: MainLayout,
    children: [{ path: '', loadComponent: () => import('./pages/feed/feed').then((m) => m.Feed) }],
  },
  {
    path: 'profile',
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/profile/profile').then((m) => m.Profile) },
    ],
  },
  {
    path: 'friends',
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/friends/friends').then((m) => m.Friends) },
    ],
  },
  {
    path: 'saved',
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/saved/saved').then((m) => m.Saved) },
    ],
  },
  {
    path: 'messages',
    component: MainLayout,
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/messages/messages').then((m) => m.Messages),
      },
    ],
  },
  {
    path: 'notifications',
    component: MainLayout,
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/notifications/notifications').then((m) => m.Notifications),
      },
    ],
  },
  {
    path: 'explore',
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/explore/explore').then((m) => m.Explore) },
    ],
  },
  {
    path: 'media',
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/media/media').then((m) => m.Media) },
    ],
  },
  {
    path: 'settings',
    component: MainLayout,
    children: [
      {
        path: '',
        loadComponent: () => import('./pages/settings/settings').then((m) => m.Settings),
      },
    ],
  },

  // Signin/signup page, with public layout
  {
    path: 'signin',
    loadComponent: () => import('./pages/signin/signin').then((m) => m.Signin),
  },
  {
    path: 'signup',
    loadComponent: () => import('./pages/signup/signup').then((m) => m.Signup),
  },
];
