export interface LoginRequest {
  emailOrUserName: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  userName: string;
  password: string;
}

export interface AuthUser {
  userId: string;
  email: string;
  userName: string;
  firstName: string;
  lastName: string;
}

export interface AuthResponse {
  userId: string;
  email: string;
  userName: string;
  firstName: string;
  lastName: string;
  accessToken: string;
  accessTokenExpiresAtUtc: string;
  refreshToken: string;
  refreshTokenExpiresAtUtc: string;
}

export type LoginResponse = AuthResponse;
export type RegisterResponse = AuthResponse;

export interface AuthState {
  user: AuthUser | null;
  accessToken: string | null;
  refreshToken: string | null;
  loading: boolean;
  error: string | null;
}
