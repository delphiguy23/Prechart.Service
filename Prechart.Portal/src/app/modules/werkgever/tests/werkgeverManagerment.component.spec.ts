import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormBuilder, FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

import { WerkgeverManagementComponent } from 'src/app/modules/werkgever/werkgeverManagement.component';
import { ServiceHelper } from '../../../helpers/service';
import { AccountService } from '../../../shared/account/services/account.service';

describe('WerkgeverManagementComponent', () => {
  let fixture: ComponentFixture<WerkgeverManagementComponent>;
  let component: WerkgeverManagementComponent;
  let element: any;

  const getMockFn = jest.fn();
  let service: ServiceHelper;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        HttpClientModule,
        RouterTestingModule,
        FormsModule,
      ],
      declarations: [
        WerkgeverManagementComponent,
      ],
      providers: [AccountService, FormBuilder],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WerkgeverManagementComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
    service = TestBed.inject(ServiceHelper);

    service.get = getMockFn;
  });

  it('should create the Werkgever Management page', () => {
    expect(component).toBeTruthy();
  });

  it('should render the Werkgever Management page correctly', () => {
    expect(element).toMatchSnapshot();
  });

  //it('should have the Werkgever Management table', () => {
  //  const wrapper = fixture.debugElement.query(By.css(".search"));
  //  expect(wrapper).toBeTruthy();
  //  expect(element).toMatchSnapshot();
  //});

});
