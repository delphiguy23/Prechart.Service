import { RouterTestingModule } from '@angular/router/testing';
import { TestBed } from '@angular/core/testing';

import { MenubarModule } from 'primeng/menubar';

import { AppComponent } from '../app.component';
import { MenuComponent } from '../../menu/menu.component';
import { AccountService } from '../../account/services/account.service';
import { HttpClientModule } from '@angular/common/http';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MenubarModule,
        HttpClientModule
      ],
      declarations: [
        AppComponent,
        MenuComponent
      ],
      providers: [AccountService],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'Prechart.Service.Portal'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('Portal');
  });
});
