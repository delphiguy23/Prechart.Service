import { AfterContentChecked, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DateHelper } from '../../../helpers/date';
import { ServiceHelper } from '../../../helpers/service';
import { Dropdown } from '../../../shared/models/dropdown';

@Component({
  selector: 'psp-werkgever-collective',
  templateUrl: './werkgeverCollective.component.html',
})

export class WerkgeverCollectiveComponent implements OnInit, AfterContentChecked {
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
  ddlWerkgever: any;
  werkgeverList: Dropdown[];
  werkgever: any;
  collectives: any[];

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

    if (this.routeData) {
      this.werkgeverOnChange(this.routeData.taxno);
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

    this.service.get('werkgever', `werkgever/${taxnoValue}`).subscribe((result) => {
      result.datumActiefVanaf = this.dateHelper.formatDate(result.datumActiefVanaf);
      result.datumActiefTot = this.dateHelper.formatDate(result.datumActiefTot);
      if (result) { 
        this.werkgever = result;
        this.collectives = result.collectieve;
      }
    });
  }

  goToEmployee() {
    this.router.navigate(['/werkgever/employees/'], { state: { taxno: this.ddlWerkgever.value } });
  }

}
