import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MenubarModule } from 'primeng/menubar';
import { CalendarModule } from 'primeng/calendar';
import { InputSwitchModule } from 'primeng/inputswitch';
import { InputNumberModule } from 'primeng/inputnumber';
import { FileUploadModule } from 'primeng/fileupload';
import { ToastModule } from 'primeng/toast';
import { ProgressBarModule } from 'primeng/progressbar';
import { TableModule } from 'primeng/table';
import { DividerModule } from 'primeng/divider';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { PanelModule } from 'primeng/panel';
import { PasswordModule } from 'primeng/password';
import { CheckboxModule } from 'primeng/checkbox';
import { SelectButtonModule } from 'primeng/selectbutton';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { SidebarModule } from 'primeng/sidebar';
import { FieldsetModule } from 'primeng/fieldset';
import { AccordionModule } from 'primeng/accordion';
import { TabViewModule } from 'primeng/tabview';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

import { AppComponent } from './app.component';
import { MenuComponent } from '../menu/menu.component';
import { AuthGuard } from 'src/app/helpers/auth.guard';
import { HomeComponent } from '../home/home.component';
import { TimeRecordComponent } from 'src/app/modules/timerecord/timerecord.component';
import { BelastingenUploadComponent } from '../../modules/belastingen/upload/belastingenUpload.component';
import { BelastingenSearchComponent } from '../../modules/belastingen/search/belastingenSearch.component';
import { LoginComponent } from 'src/app/shared/account/login/login.component';
import { EmployeeUploadComponent } from '../../modules/employee/upload/employeeUpload.component';
import { EmployeeSearchComponent } from '../../modules/employee/search/employeeSearch.component';
import { WerkgeverManagementComponent } from '../../modules/werkgever/werkgeverManagement.component';
import { WerkgeverEditComponent } from '../../modules/werkgever/management/werkgeverEdit.component';
import { WhkPremieEditComponent } from '../../modules/werkgever/management/whkPremieEdit.component';
import { WerkgeverCollectiveComponent } from '../../modules/werkgever/collective/werkgeverCollective.component';
import { EmployeeDetailsComponent } from '../../modules/employee/search/details/employeeDetails.component';
import { EmployeeTaxDetailsComponent } from '../../modules/employee/search/details/employeeTaxDetails.component';
import { EmployeeDaywageComponent } from '../../modules/employee/daywage/employeeDaywage.component';
import { EmployeeDaywageTaxDetailsComponent } from '../../modules/employee/daywage/details/employeeTaxDetails.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'timerecord', component: TimeRecordComponent, canActivate: [AuthGuard] },
  {
    path: 'belastingen', children: [
      { path: 'upload', component: BelastingenUploadComponent, canActivate: [AuthGuard] },
      { path: 'search', component: BelastingenSearchComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'employee', children: [
      { path: 'upload', component: EmployeeUploadComponent, canActivate: [AuthGuard] },
      { path: 'search', component: EmployeeSearchComponent, canActivate: [AuthGuard] },
      { path: 'daywage', component: EmployeeDaywageComponent, canActivate: [AuthGuard] },
    ]
  },
  {
    path: 'werkgever', children: [
      { path: 'management', component: WerkgeverManagementComponent, canActivate: [AuthGuard] },
      { path: 'collective', component: WerkgeverCollectiveComponent, canActivate: [AuthGuard] },
      { path: 'employees', component: EmployeeSearchComponent, canActivate: [AuthGuard] },
    ]
  },
  { path: '**', redirectTo: '' }
];

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    LoginComponent,
    TimeRecordComponent,
    BelastingenUploadComponent,
    BelastingenSearchComponent,
    EmployeeUploadComponent,
    EmployeeSearchComponent,
    WerkgeverManagementComponent,
    WerkgeverEditComponent,
    WhkPremieEditComponent,
    EmployeeDetailsComponent,
    EmployeeTaxDetailsComponent,
    WerkgeverCollectiveComponent,
    EmployeeDaywageComponent,
    EmployeeDaywageTaxDetailsComponent,
  ],
  imports: [
    RouterModule.forRoot(appRoutes, { onSameUrlNavigation: 'reload' }),
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    NgbModule,
    BrowserAnimationsModule,
    HttpClientModule,
    CardModule,
    ButtonModule,
    MenubarModule,
    CalendarModule,
    InputSwitchModule,
    InputNumberModule,
    FileUploadModule,
    ToastModule,
    ProgressBarModule,
    TableModule,
    DividerModule,
    InputTextModule,
    DropdownModule,
    MessagesModule,
    MessageModule,
    PanelModule,
    PasswordModule,
    CheckboxModule,
    SelectButtonModule,
    DialogModule,
    ConfirmDialogModule,
    SidebarModule,
    FieldsetModule,
    AccordionModule,
    TabViewModule,
    AutoCompleteModule,
    ProgressSpinnerModule,
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
