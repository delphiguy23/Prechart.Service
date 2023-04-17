import { AfterContentChecked, ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ServiceHelper } from '../../../helpers/service';
import { Dropdown } from '../../../shared/models/dropdown';
import { Employee } from './models/employee';
import { DateHelper } from '../../../helpers/date';
import { Daywage } from './models/daywage';

@Component({
  selector: 'psp-employee-daywage',
  templateUrl: './employeeDaywage.component.html',
  providers: [MessageService]
})

export class EmployeeDaywageComponent implements OnInit, AfterContentChecked {
  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private service: ServiceHelper,
    private dateHelper: DateHelper,
    private router: Router,
  ) {
  }

  ddlNameDisabled: boolean = true;
  showNameSpinner: boolean = false;

  personNames: any[] = [];
  ddlName: any[];
  selectedPerson: any;

  personData: any;
  personDaywageData: any;

  calculateDaywageDialog: boolean = false;
  maxDate: any = new Date();
  calculateSickleaveDate: any = new Date();

  progressVisible: boolean;

  werkgeverList: Dropdown[];
  ddlWerkgever: Dropdown;

  calculatePersonId: any = "";
  calculateTaxNumber: any = "";

  taxDetailsDialogData: any;
  taxDetailsDialog: boolean = false;

  ngOnInit() {
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

  ngAfterContentChecked() {
    this.changeDetectorRef.detectChanges();
  }

  werkgeverOnChange(event: any) {
    this.ddlNameDisabled = true;
    this.showNameSpinner = true;

    this.service.get('person', `person/werkgever/${event.value.value}`).subscribe((result) => {      
      var personList: any[] = [];
      if (result != null) {
        for (let person of result) {
          personList.push({
            significantAchternaam: person.significantAchternaam,
            voorletter: person.voorletter,
            sofinummer: person.sofiNr,
            geboortedatum: this.dateHelper.formatDate(person.geboortedatum),
            klant: person.werkgever[0].klant,
            loonheffingsnummer: person.werkgever[0].loonheffingsNr,
            personnummer: person.taxPaymentDetails[0].personNr,
            personId: person.id,
            taxNumber: person.taxPaymentDetails[0].taxNo,
          });
        }
        console.log(personList.length);

        this.personNames = personList;
        this.ddlNameDisabled = false;
        this.showNameSpinner = false;
      }
    });
  }

  search() {
    this.service.get('person', `person/id/${this.selectedPerson.personId}`).subscribe((result) => {
      if (result != null) {
        this.personData = result;
        this.getPersonDaywageDetails(this.selectedPerson.personId, this.selectedPerson.loonheffingsnummer);
      }
    });
  }

  hideDialog() {
    this.calculateDaywageDialog = false;
    this.taxDetailsDialog = false;
  }

  openCalculateDaywage(personId: any, taxNumber: any) {
    this.calculateDaywageDialog = true;
    this.calculatePersonId = personId;
    this.calculateTaxNumber = taxNumber;
  }

  openTaxDetails(taxDetails: any) {
    this.taxDetailsDialogData = null;
    this.taxDetailsDialog = true;
    this.taxDetailsDialogData = taxDetails;
  }

  getPersonDaywageDetails(personId: any, taxNumber: any) {
    this.personDaywageData = null;

    this.service.get('person', `daywage/${personId}/${taxNumber}`).subscribe((result) => {
      if (result) {
        this.personDaywageData = result;
      }
    });
  }

  calculateDaywage() {
    this.progressVisible = true;
    var daywageData = new Daywage();
    daywageData.personId = this.selectedPerson.personId;
    daywageData.taxNumber = this.selectedPerson.loonheffingsnummer;
    daywageData.startOfSickLeave = this.dateHelper.formatDate(this.calculateSickleaveDate);

    this.service.post('person', `daywage/calculate/`, daywageData).subscribe((result) => {
      if (result) {
        this.hideDialog();
        this.getPersonDaywageDetails(this.selectedPerson.personId, this.selectedPerson.loonheffingsnummer);
        this.progressVisible = false;
      }
    });
  }

  reloadPage(taxNumber: any, personId: any) {
    this.router.navigate(['/employee/daywage/'], { state: { taxno: taxNumber, personId: personId } });
  }
}
