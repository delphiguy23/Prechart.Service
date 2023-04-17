import { Component, Input } from "@angular/core";

@Component({
  selector: 'psp-employee-daywage-tax-details',
  templateUrl: './employeeTaxDetails.component.html',
})

export class EmployeeDaywageTaxDetailsComponent {
  constructor(
  ) { }
  @Input() model: any;  
}
