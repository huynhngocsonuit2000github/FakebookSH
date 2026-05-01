import { createFeature, createReducer, createSelector, on } from '@ngrx/store';
import { AuthActions } from './auth.actions';
import { AuthState } from './auth.models';

export const initialAuthState: AuthState = {
  user: null,
  accessToken: localStorage.getItem('accessToken'),
  refreshToken: localStorage.getItem('refreshToken'),
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
    user: {
      userId: response.userId,
      email: response.email,
      userName: response.userName,
      firstName: response.firstName,
      lastName: response.lastName,
    },
    accessToken: response.accessToken,
    refreshToken: response.refreshToken,
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
    user: {
      userId: response.userId,
      email: response.email,
      userName: response.userName,
      firstName: response.firstName,
      lastName: response.lastName,
    },
    accessToken: response.accessToken,
    refreshToken: response.refreshToken,
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
    loading: false,
    error: null,
  })),

  on(AuthActions.loadMeFailure, (state, { error }) => ({
    ...state,
    user: null,
    loading: false,
    error,
  })),

  on(AuthActions.restoreAuth, (state) => ({
    ...state,
    loading: true,
    error: null,
  })),

  on(AuthActions.restoreAuthSuccess, (state, { accessToken, refreshToken, user }) => ({
    ...state,
    user,
    accessToken,
    refreshToken,
    loading: false,
    error: null,
  })),

  on(AuthActions.restoreAuthFailure, () => ({
    user: null,
    accessToken: null,
    refreshToken: null,
    loading: false,
    error: null,
  })),

  on(AuthActions.logout, () => ({
    user: null,
    accessToken: null,
    refreshToken: null,
    loading: false,
    error: null,
  })),
);

export const authFeature = createFeature({
  name: 'auth',
  reducer,

  extraSelectors: ({ selectAccessToken }) => ({
    selectIsAuthenticated: createSelector(selectAccessToken, (accessToken) => !!accessToken),
  }),
});

export const {
  name: authFeatureKey,
  reducer: authReducer,

  selectAuthState,
  selectUser: selectAuthUser,
  selectAccessToken,
  selectRefreshToken,
  selectLoading: selectAuthLoading,
  selectError: selectAuthError,
  selectIsAuthenticated,
} = authFeature;
