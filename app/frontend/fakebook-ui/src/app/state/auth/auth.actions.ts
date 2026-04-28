import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { AuthUser, LoginRequest, LoginResponse } from './auth.models';

export const AuthActions = createActionGroup({
  source: 'Auth',
  events: {
    Login: props<{ request: LoginRequest }>(),
    'Login Success': props<{ response: LoginResponse }>(),
    'Login Failure': props<{ error: string }>(),

    'Load Me': emptyProps(),
    'Load Me Success': props<{ user: AuthUser }>(),
    'Load Me Failure': props<{ error: string }>(),

    Logout: emptyProps(),
  },
});
