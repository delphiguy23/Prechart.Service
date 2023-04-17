import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AccountService } from "../shared/account/services/account.service";
import { UrlHelper } from "./url";
import {User} from "../shared/account/models/user";

@Injectable({ providedIn: 'root' })
export class ServiceHelper {
  headers: HttpHeaders;
  constructor(
    private http: HttpClient,
    private accountService: AccountService,
    private urlHelper: UrlHelper)
  {
    if (this.accountService.userValue) {
      var token = this.accountService.userValue;

      this.headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`,
      });
    }
  }

  get(port: string, path: string) {
    var url = this.urlHelper.format(port, path);
    return this.http.get<any>(url, { headers: this.headers });
  }

  post(port: string, path: string, object: any) {
    var url = this.urlHelper.format(port, path);
    return this.http.post<any>(url, object, { headers: this.headers });
  }

  validateUser(port: string, path: string, object: any, options: any) {
    var url = this.urlHelper.format(port, path);
    return this.http.post<any>(url, object, options);
  }
}
