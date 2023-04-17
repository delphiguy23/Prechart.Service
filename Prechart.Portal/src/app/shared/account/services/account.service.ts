import {Injectable} from '@angular/core';
import {Router} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable, take} from 'rxjs';
import {map} from 'rxjs/operators';
import {Login} from '../models/login';
import {UrlHelper} from '../../../helpers/url';
import {User} from "../models/user";
import {JwtHelperService} from "@auth0/angular-jwt";

@Injectable({providedIn: 'root'})
export class AccountService {
  private userSubject: BehaviorSubject<string>;
  public user: Observable<any>;
  private tokens: any;
  private isJwtExpired: boolean;
  private jwtHelper: JwtHelperService
  private currentToken: any;

  constructor(
    private router: Router,
    private http: HttpClient,
    private urlHelper: UrlHelper
  ) {
    this.isJwtExpired = false;
    this.jwtHelper = new JwtHelperService();
    this.userSubject = new BehaviorSubject<string>(sessionStorage.getItem('userTokens') || '');
    this.user = this.userSubject.asObservable();
    this.userSubject.subscribe(value => this.currentToken = value);
  }

  public get userValue(): any {
    try {
      if (this.currentToken) {
        this.tokens = JSON.parse(this.currentToken);
        this.isJwtExpired = this.jwtHelper.isTokenExpired(this.tokens.bearerToken, 60 * 30);

        if (this.isJwtExpired) {
          this.refresh(this.tokens);

          let newToken = sessionStorage.getItem('userTokens');

          if (newToken) {
            this.tokens = JSON.parse(this.currentToken)
            return this.tokens.bearerToken
          }
        }

        return this.tokens.bearerToken;
      }
    } catch {
      return ""
    }
  }

  login(username: any, password: any) {
    var bodyValidate: Login = {
      username: username,
      password: password
    };

    var urlValidate = this.urlHelper.format('user', 'users/validate');

    return this.http.post<any>(urlValidate, bodyValidate, {responseType: 'text' as 'json'})
      .pipe(map(user => {
        sessionStorage.setItem('userTokens', user);
        this.userSubject.next(user);
        return user;
      }));
  }

  refresh(tokens: User) {
    var urlRefresh = this.urlHelper.format('user', 'users/refresh');

    this.http.post<any>(urlRefresh, tokens, {responseType: 'text' as 'json'}).subscribe((user) => {
      sessionStorage.removeItem('userTokens');
      this.userSubject.next('');

      sessionStorage.setItem('userTokens', user);
      this.userSubject.next(user);
    });
  }

  logout() {
    sessionStorage.removeItem('userTokens');
    this.userSubject.next('');
    this.router.navigate(['/login']);
  }
}
