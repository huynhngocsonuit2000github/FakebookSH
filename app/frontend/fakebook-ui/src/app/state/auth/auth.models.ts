export interface LoginRequest {
  emailOrUserName: string;
  password: string;
}

export interface AuthUser {
  userId: string;
  email: string;
  userName: string;
}

export interface LoginResponse {
  userId: string;
  email: string;
  userName: string;
  accessToken: string;
  accessTokenExpiresAtUtc: string;
  refreshToken: string;
  refreshTokenExpiresAtUtc: string;
}

export interface AuthState {
  user: AuthUser | null;
  accessToken: string | null;
  refreshToken: string | null;
  loading: boolean;
  error: string | null;
}
