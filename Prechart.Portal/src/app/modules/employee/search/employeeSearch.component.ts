import { AfterContentChecked, ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ServiceHelper } from '../../../helpers/service';
import { Dropdown } from '../../../shared/models/dropdown';
import { Employee } from './models/employee';
import { DateHelper } from '../../../helpers/date';
import { Table } from 'primeng/table';

@Component({
  selector: 'psp-employee-search',
  templateUrl: './employeeSearch.component.html',
  providers: [MessageService]
})


export class EmployeeSearchComponent implements OnInit, AfterContentChecked {
  @ViewChild(Table, { static: false }) employeesTable: Table;
  routeData: any;
  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private service: ServiceHelper,
    private dateHelper: DateHelper,
    private router: Router,
  ) {
    this.routeData = this.router.getCurrentNavigation()?.extras.state;
  }

  progressVisible: boolean;

  employeesList: Employee[];

  taxCumulative: any;

  cols: any[];
  colsDetails: any[];

  werkgeverList: Dropdown[];
  ddlWerkgever: Dropdown;

  visibleSidebarDetails: any;
  employeeDetailsModel: Employee;

  isShowDdlWerkgever: boolean = true;
  werkgeverTaxNo: any;

  ngOnInit() {
    if (this.routeData) {
      this.werkgeverOnChange(this.routeData.taxno);
      this.isShowDdlWerkgever = false;
      this.werkgeverTaxNo = this.routeData.taxno;
    } else {
      var werkgeverOptions: any[] = [];
      this.service.get('werkgever', 'werkgever/all').subscribe((result) => {
        if (result) {
          for (let employer of result) {
            werkgeverOptions.push({ value: employer.fiscaalNummer, label: employer.naam });
          }
          this.werkgeverList = werkgeverOptions;
        }
      });
    }
  }

  ngAfterContentChecked() {
    this.changeDetectorRef.detectChanges();
  }

  werkgeverOnChange(event: any) {
    var taxnoValue: any;
    if (event.value != undefined) {
      taxnoValue = event.value.value
    } else {
      taxnoValue = event;
    }

    this.progressVisible = true;
    this.service.get('person', `person/werkgever/${taxnoValue}`).subscribe((result) => {

      if (result) {
        this.cols = [
          { field: 'personnummer', header: 'Personeelsnummer' },
          { field: 'significantAchternaam', header: 'Achternaam' },
          { field: 'voorletter', header: 'Voorletter' },
          { field: 'geboortedatum', header: 'Geboortedatum' },
          { field: 'klant', header: 'Klant' },
          { field: 'loonheffingsnummer', header: 'Loonheffingsnummer' },
        ];

        var personList: any[] = [];
        for (let person of result) {
          personList.push({
            significantAchternaam: person.significantAchternaam,
            voorletter: person.voorletter,
            sofinummer: person.sofiNr,
            geboortedatum: this.dateHelper.formatDate(person.geboortedatum),
            klant: person.werkgever[0].klant,
            loonheffingsnummer: person.werkgever[0].loonheffingsNr,
            adresBinnenland: person.adresBinnenland,
            adresBuitenland: person.adresBuitenland,
            taxDetails: person.taxPaymentDetails,
            personnummer: person.taxPaymentDetails[0].personNr,
          });
        }

        this.employeesList = personList;
        this.progressVisible = false;
      }
    });
  }

  onRowExpand(event: any) {
    this.taxCumulative = null;
    this.service.get('person', `person/cumulative/${event.data.sofinummer}`).subscribe((result) => {
      if (result) {
        this.taxCumulative = result;
        }
    });
  }

  viewSidebarDetails(details: any) {
    this.employeeDetailsModel = details;    
    this.visibleSidebarDetails = true;
  }

  clear(table: Table) {
    table.clear();
  }

  applyfilterGlobal($event: any) {
    this.employeesTable.filterGlobal(($event.target as HTMLInputElement).value, "contains");  
  }
}
