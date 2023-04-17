import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { By } from '@angular/platform-browser';

import { MessageService } from 'primeng/api';
import { FileUploadModule } from 'primeng/fileupload';
import { CardModule } from 'primeng/card';
import { ToastModule } from 'primeng/toast';

import { BelastingenUploadComponent } from 'src/app/modules/belastingen/upload/belastingenUpload.component';
import { ServiceHelper } from '../../../../helpers/service';
import { AccountService } from '../../../../shared/account/services/account.service';
import { HttpClientModule } from '@angular/common/http';
import { RouterTestingModule } from '@angular/router/testing';

const uploadDocumentMock = jest.fn();

describe('BelastingenUploadComponent', () => {
  let fixture: ComponentFixture<BelastingenUploadComponent>;
  let component: BelastingenUploadComponent;
  let element: any;

  const getMockFn = jest.fn();
  let service: ServiceHelper;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        FileUploadModule,
        CardModule,
        ToastModule,
        HttpClientModule,
        RouterTestingModule,
      ],
      declarations: [
        BelastingenUploadComponent,
      ],
      providers: [
        MessageService,
        AccountService
      ],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BelastingenUploadComponent);
    component = fixture.componentInstance;
    element = fixture.nativeElement;
    fixture.detectChanges();
    service = TestBed.inject(ServiceHelper);

    service.get = getMockFn;
  });

  it('should create the Upload page', () => {  
    expect(component).toBeTruthy();
  });

  it('should render the Upload page correctly', () => {
    expect(element).toMatchSnapshot();
  });

  it('should have the Upload field', () => {
    const wrapper = fixture.debugElement.query(By.css("p-fileupload"));
    expect(wrapper).toBeTruthy();
    expect(element).toMatchSnapshot();
  });
});
