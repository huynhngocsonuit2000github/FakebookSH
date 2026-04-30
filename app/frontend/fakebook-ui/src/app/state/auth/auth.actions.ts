import { createActionGroup, emptyProps, props } from '@ngrx/store';
import { AuthUser, LoginRequest, LoginResponse, RegisterRequest, RegisterResponse } from './auth.models';

export const AuthActions = createActionGroup({
  source: 'Auth',
  events: {
    Login: props<{ request: LoginRequest }>(),
    'Login Success': props<{ response: LoginResponse }>(),
    'Login Failure': props<{ error: string }>(),

    Register: props<{ request: RegisterRequest }>(),
    'Register Success': props<{ response: RegisterResponse }>(),
    'Register Failure': props<{ error: string }>(),

    'Load Me': emptyProps(),
    'Load Me Success': props<{ user: AuthUser }>(),
    'Load Me Failure': props<{ error: string }>(),

    Logout: emptyProps(),
  },
});
