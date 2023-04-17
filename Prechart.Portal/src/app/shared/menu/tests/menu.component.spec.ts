import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { MenubarModule } from 'primeng/menubar';

import { MenuComponent } from 'src/app/shared/menu/menu.component';
import { AccountService } from 'src/app/shared/account/services/account.service'
import { HttpClientModule } from '@angular/common/http';

describe('MenuComponent', () => {
  let fixture: ComponentFixture<MenuComponent>;
  let component: MenuComponent;
  let element: any;
  let constructoMock: any;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MenubarModule,
        HttpClientModule,
      ],
      declarations: [
        MenuComponent,
      ],
      providers: [AccountService],
    }).compileComponents();    
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MenuComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
  });

  it('should create the Menu page', () => {    
    expect(component).toBeTruthy();
  });

  it('should render the Menu page correctly', () => {    
    expect(element).toMatchSnapshot();
  });

  it('should render the menu items correctly', () => {
    let element = fixture.debugElement.query(By.css('.p-menuitem-text'));
    let span = element.nativeElement;
    
    expect(span.innerHTML).toContain('Belastingen');
    expect(element).toMatchSnapshot();
  });

});
