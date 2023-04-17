import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { AccountService } from '../account/services/account.service';

@Component({
  selector: 'psp-menu',
  templateUrl: './menu.component.html',
})
export class MenuComponent implements OnInit {
  title = 'Menu';
  user: string;

  constructor(private accountService: AccountService) {
    this.accountService.user.subscribe(x => this.user = x);
  }

  menuItems: MenuItem[];

  ngOnInit() {
    this.menuItems = [
      //{
      //  label: 'DTR',
      //  routerLink: '/timerecord'
      //},
      {
        label: 'Belastingen',
        items: [{
          label: 'Belasting Tabellen Wit/Groen',
          items: [
            {
              label: 'Uploaden',
              routerLink: '/belastingen/upload'
            },
            {
              label: 'Zoek',
              routerLink: '/belastingen/search'
            }
          ]
        }]
      },
      {
        label: 'Employee',
        items: [
          {          
            label: 'Uploaden',
            routerLink: '/employee/upload'
          },
          {
            label: 'Zoek',
            routerLink: '/employee/search'
          },
          {
            label: 'Daywage',
            routerLink: '/employee/daywage'
          }
        ],
      }, {
        label: 'Werkgever',
        items: [
          {
            label: 'Beheer',
            routerLink: '/werkgever/management'
          },
          {
            label: 'Collectieve Aangifte ',
            routerLink: '/werkgever/collective'
          },
        ],
      },
    ];
  }

  logout() {
    this.accountService.logout();
  }
}
