import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { map, take } from 'rxjs';

import { selectAuthUser, selectIsAuthenticated } from '../../state/auth/auth.reducer';

export const authGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);

  const isAuthenticated$ = store.select(selectIsAuthenticated);

  return isAuthenticated$.pipe(
    take(1),
    map((isAuthenticated) => {
      if (isAuthenticated) {
        return true;
      }

      router.navigate(['/signin']);
      return false;
    }),
  );
};
