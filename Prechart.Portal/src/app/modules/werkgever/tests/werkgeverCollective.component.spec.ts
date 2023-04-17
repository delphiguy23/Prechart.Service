import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { BrowserModule, By } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

import { WerkgeverCollectiveComponent } from 'src/app/modules/werkgever/collective/werkgeverCollective.component';
import { ServiceHelper } from '../../../helpers/service';
import { AccountService } from '../../../shared/account/services/account.service';
import { PanelModule } from 'primeng/panel';
import { AccordionModule } from 'primeng/accordion';
import { DropdownModule } from 'primeng/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('WerkgeverCollectiveComponent', () => {
  let fixture: ComponentFixture<WerkgeverCollectiveComponent>;
  let component: WerkgeverCollectiveComponent;
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
        PanelModule,
        AccordionModule,
        DropdownModule,
      ],
      declarations: [
        WerkgeverCollectiveComponent,
      ],
      providers: [AccountService],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WerkgeverCollectiveComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
    service = TestBed.inject(ServiceHelper);

    service.get = getMockFn;
  });

  it('should create the Werkgever Collective page', () => {
    expect(component).toBeTruthy();
  });

  it('should render the Werkgever Collective page correctly', () => {
    expect(element).toMatchSnapshot();
  });

  //it('should have the Werkgever Management table', () => {
  //  const wrapper = fixture.debugElement.query(By.css(".search"));
  //  expect(wrapper).toBeTruthy();
  //  expect(element).toMatchSnapshot();
  //});

});
