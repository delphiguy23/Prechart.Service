import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

import { CardModule } from 'primeng/card';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { PanelModule } from 'primeng/panel';
import { InputNumberModule } from 'primeng/inputnumber';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { CheckboxModule } from 'primeng/checkbox';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';

import { BelastingenSearchComponent } from 'src/app/modules/belastingen/search/belastingenSearch.component';
import { ServiceHelper } from '../../../../helpers/service';
import { AccountService } from '../../../../shared/account/services/account.service';

describe('BelastingenSearchComponent', () => {
  let fixture: ComponentFixture<BelastingenSearchComponent>;
  let component: BelastingenSearchComponent;
  let element: any;

  const getMockFn = jest.fn();
  let service: ServiceHelper;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        HttpClientModule,
        RouterTestingModule,
        CardModule,
        DropdownModule,
        InputTextModule,
        PanelModule,
        InputNumberModule,
        BrowserAnimationsModule,
        ButtonModule,
        FormsModule,
        CalendarModule,
        CheckboxModule,
        DialogModule,
        ConfirmDialogModule,
        ToastModule,
      ],
      declarations: [
        BelastingenSearchComponent,
      ],
      providers: [AccountService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BelastingenSearchComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
    service = TestBed.inject(ServiceHelper);

    getMockFn.mockResolvedValue({
      result: [2022, 2021],
    });

    getMockFn.mockResolvedValue({
      result: [
        {
          "id": 1,
          "woonlandbeginselCode": "NL",
          "woonlandbeginselBenaming": "Nederland",
          "woonlandbeginselBelastingCode": 2,
          "active": true
        },
        {
          "id": 2,
          "woonlandbeginselCode": "BE",
          "woonlandbeginselBenaming": "België",
          "woonlandbeginselBelastingCode": 2,
          "active": true
        },
      ],
    });

    getMockFn.mockResolvedValue({
      result: [
        {
          id: 1,
          naam: 'WerkgeverOne',
          fiscaalNummer: '123456789',
          loonheffingenExtentie: 'L01',
        }, {
          id: 1,
          naam: 'WerkgeverTwo',
          fiscaalNummer: '234567891',
          loonheffingenExtentie: 'L02',
        },
      ],
    });

    service.get = getMockFn;
  });

  it('should create the Search page', () => {
    expect(component).toBeTruthy();
  });

  it('should render the Search page correctly', () => {
    expect(element).toMatchSnapshot();
  });

  it('should have the Search field', () => {
    const wrapper = fixture.debugElement.query(By.css(".search"));
    expect(wrapper).toBeTruthy();
    expect(element).toMatchSnapshot();
  });

  it('should have country dropdown', () => {
    const wrapper = fixture.debugElement.query(By.css(".p-dropdown-label"));
    expect(wrapper).toBeTruthy();
    expect(element).toMatchSnapshot();
  });
});
