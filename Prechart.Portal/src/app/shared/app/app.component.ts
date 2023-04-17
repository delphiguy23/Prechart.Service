import { Component } from '@angular/core';
import { AccountService } from '../account/services/account.service';

@Component({
  selector: 'psp-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'Portal';

  user: string;

  constructor(private accountService: AccountService) {
    this.accountService.user.subscribe(x => this.user = x);
  }

  logout() {
    this.accountService.logout();
  }
}
