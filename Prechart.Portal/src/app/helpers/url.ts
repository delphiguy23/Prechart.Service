import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";

@Injectable({ providedIn: 'root' })
export class UrlHelper {
  format(subUrl: string, path: string) {
    var getUrl = this.getUrl(subUrl);

    if (environment.production) {
      var host = environment.apiUrl.replace('[[subdomain]]', getUrl);
      return `${host}${path}`;
    }
    else {
      var host = environment.apiUrl.replace('[[port]]', getUrl);
      return `${host}${path}`;
    }
  }

  getUrl(domain: string) {
    var value: string = '';
    switch (domain) {
      case 'document': value = environment.document;
        break;
      case 'belastingen': value = environment.belastingen;
        break;
      case 'user': value = environment.user;
        break;
      case 'loonheffings': value = environment.loonheffings;
        break;
      case 'person': value = environment.person;
        break;
      case 'werkgever': value = environment.werkgever;
        break;
      default: value = '0000';
        break;
    }
    return value;
  }
}
