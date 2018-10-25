import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginViewModel } from '../viewmodels/login.viewmodel';
import { Observable } from 'rxjs';
import { AuthorizedUserInfoViewModel } from 'src/app/viewmodels/authorizeduser.viewmodel';
import { Response, Http } from '@angular/http';
import { map } from 'rxjs/operators';
import { Headers, RequestOptions } from '@angular/http';

interface IAuthService {
  login(userdata: LoginViewModel);
}

@Injectable({
  providedIn: 'root'
})
export class AuthService implements IAuthService {
  private readonly authUrl = 'http://localhost:4442/api/auth/login';
  private readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
      'Authorization': 'my-auth-token'
    })
  };

  constructor(private http: HttpClient) {
  }

  login(userdata: LoginViewModel) {
    return this.http.post(this.authUrl, JSON.stringify(userdata), this.httpOptions)
    .pipe(map(user => {
      if (user && user.token) {
        localStorage.setItem('currentUser', JSON.stringify(user));
      }
    }));
  }

  private getRequestOptions() {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    headers.append('Authorization', 'Bearer ' + localStorage.getItem('token'));
    return headers;
}
}
