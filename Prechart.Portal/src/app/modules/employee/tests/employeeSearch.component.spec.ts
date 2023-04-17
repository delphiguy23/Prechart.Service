import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

import { EmployeeSearchComponent } from 'src/app/modules/employee/search/employeeSearch.component';
import { ServiceHelper } from '../../../helpers/service';
import { AccountService } from '../../../shared/account/services/account.service';
import { SidebarModule } from 'primeng/sidebar';
import { EmployeeDetailsComponent } from '../search/details/employeeDetails.component';
import { DividerModule } from 'primeng/divider';
import { Panel, PanelModule } from 'primeng/panel';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('EmployeeSearchComponent', () => {
  let fixture: ComponentFixture<EmployeeSearchComponent>;
  let component: EmployeeSearchComponent;
  let element: any;

  const getMockFn = jest.fn();
  let service: ServiceHelper;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        HttpClientModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        FormsModule,
        SidebarModule,
        DividerModule,
        PanelModule,
        DropdownModule,
      ],
      declarations: [
        EmployeeSearchComponent,
        EmployeeDetailsComponent,
      ],
      providers: [AccountService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeSearchComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
    service = TestBed.inject(ServiceHelper);

    service.get = getMockFn;
  });

  it('should create the Employee Search page', () => {
    expect(component).toBeTruthy();
  });

  it('should render the Employee Search page correctly', () => {
    expect(element).toMatchSnapshot();
  });

});
