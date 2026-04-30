import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  AuthUser,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
} from '../../state/auth/auth.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly baseUrl = 'https://localhost:7000/api/bff/auth';

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, request);
  }

  register(request: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.baseUrl}/register`, request);
  }

  getMe(): Observable<AuthUser> {
    return this.http.get<AuthUser>(`${this.baseUrl}/me`);
  }
}
