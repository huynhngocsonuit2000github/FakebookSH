import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { AuthActions } from '../../../state/auth/auth.actions';
import { Store } from '@ngrx/store';
import { selectAuthUser } from '../../../state/auth/auth.reducer';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-menu',
  imports: [MatIconModule, RouterModule, CommonModule],
  templateUrl: './user-menu.html',
  styleUrl: './user-menu.scss',
})
export class UserMenu {
  private store = inject(Store);
  user$;

  constructor() {
    this.user$ = this.store.select(selectAuthUser);
  }

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }
}
