import { Component, Input } from "@angular/core";

@Component({
  selector: 'psp-employee-tax-details',
  templateUrl: './employeeTaxDetails.component.html',
})

export class EmployeeTaxDetailsComponent {
  constructor(
  ) { }
  @Input() model: any;  
}
