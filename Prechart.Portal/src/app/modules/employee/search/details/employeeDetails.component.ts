import { Component, Input, OnInit } from "@angular/core";

@Component({
  selector: 'psp-employee-details',
  templateUrl: './employeeDetails.component.html',
})

export class EmployeeDetailsComponent implements OnInit {
  constructor(
  ) { }
  @Input() model: any;

  hasForeignAddress: boolean = false;

  ngOnInit() {
    if (this.model?.adresBuitenland) {
      this.hasForeignAddress = true;
    }
  }
}
