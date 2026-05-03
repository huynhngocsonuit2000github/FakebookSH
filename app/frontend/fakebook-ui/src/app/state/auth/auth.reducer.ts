import { createFeature, createReducer, createSelector, on } from '@ngrx/store';
import { AuthActions } from './auth.actions';
import { AuthState } from './auth.models';

export const initialAuthState: AuthState = {
  user: null,
  initialized: false,
  loading: false,
  error: null,
};

const reducer = createReducer(
  initialAuthState,

  on(AuthActions.login, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.loginSuccess, (state, { response }) => ({
    ...state,
    user: response,
    initialized: true,
    loading: false,
    error: null,
  })),

  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(AuthActions.register, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.registerSuccess, (state, { response }) => ({
    ...state,
    user: response,
    initialized: true,
    loading: false,
    error: null,
  })),

  on(AuthActions.registerFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
  })),

  on(AuthActions.loadMe, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.loadMeSuccess, (state, { user }) => ({
    ...state,
    user,
    initialized: true,
    loading: false,
    error: null,
  })),

  on(AuthActions.loadMeFailure, (state, { error }) => ({
    ...state,
    user: null,
    initialized: true,
    loading: false,
    error,
  })),

  on(AuthActions.restoreAuth, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.restoreAuthSuccess, (state, { user }) => ({
    ...state,
    user,
    initialized: true,
    loading: false,
    error: null,
  })),

  on(AuthActions.restoreAuthFailure, () => ({
    user: null,
    initialized: true,
    loading: false,
    error: null,
  })),

  on(AuthActions.logout, () => ({
    user: null,
    initialized: true,
    loading: false,
    error: null,
  })),
);

export const authFeature = createFeature({
  name: 'auth',
  reducer,

  extraSelectors: ({ selectUser }) => ({
    selectIsAuthenticated: createSelector(selectUser, (user) => !!user),
  }),
});

export const {
  name: authFeatureKey,
  reducer: authReducer,

  selectAuthState,
  selectUser: selectAuthUser,
  selectInitialized: selectAuthInitialized,
  selectLoading: selectAuthLoading,
  selectError: selectAuthError,
  selectIsAuthenticated,
} = authFeature;
