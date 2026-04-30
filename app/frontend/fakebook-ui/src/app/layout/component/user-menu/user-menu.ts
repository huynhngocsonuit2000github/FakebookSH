import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { AuthActions } from '../../../state/auth/auth.actions';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-user-menu',
  imports: [MatIconModule, RouterModule],
  templateUrl: './user-menu.html',
  styleUrl: './user-menu.scss',
})
export class UserMenu {
  private store = inject(Store);

  logout(): void {
    this.store.dispatch(AuthActions.logout());
  }
}
