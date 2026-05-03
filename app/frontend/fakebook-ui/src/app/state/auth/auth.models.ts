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

export type LoginResponse = AuthUser;
export type RegisterResponse = AuthUser;

export interface AuthState {
  user: AuthUser | null;
  initialized: boolean;
  loading: boolean;
  error: string | null;
}
