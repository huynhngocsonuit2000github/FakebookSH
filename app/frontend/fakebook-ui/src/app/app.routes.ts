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
