import { Routes } from '@angular/router';
import { PublicLayout } from './layout/public-layout/public-layout';
import { MainLayout } from './layout/main-layout/main-layout';
import { authGuard } from './core/guards/auth.guard';
import { unauthGuard } from './core/guards/auth.unguard';

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
    canActivate: [authGuard],
    component: MainLayout,
    children: [{ path: '', loadComponent: () => import('./pages/feed/feed').then((m) => m.Feed) }],
  },
  {
    path: 'profile',
    canActivate: [authGuard],
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/profile/profile').then((m) => m.Profile) },
    ],
  },
  {
    path: 'friends',
    canActivate: [authGuard],
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/friends/friends').then((m) => m.Friends) },
    ],
  },
  {
    path: 'saved',
    canActivate: [authGuard],
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/saved/saved').then((m) => m.Saved) },
    ],
  },
  {
    path: 'messages',
    canActivate: [authGuard],
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
    canActivate: [authGuard],
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
    canActivate: [authGuard],
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/explore/explore').then((m) => m.Explore) },
    ],
  },
  {
    path: 'media',
    canActivate: [authGuard],
    component: MainLayout,
    children: [
      { path: '', loadComponent: () => import('./pages/media/media').then((m) => m.Media) },
    ],
  },
  {
    path: 'settings',
    canActivate: [authGuard],
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
    canActivate: [unauthGuard],
    loadComponent: () => import('./pages/signin/signin').then((m) => m.Signin),
  },
  {
    path: 'signup',
    canActivate: [unauthGuard],
    loadComponent: () => import('./pages/signup/signup').then((m) => m.Signup),
  },
];
