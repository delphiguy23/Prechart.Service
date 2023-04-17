import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Record } from '../timerecord/models/record';
import { AccountService } from '../../shared/account/services/account.service';

@Component({
  selector: 'psp-timerecord',
  templateUrl: './timerecord.component.html',
  styleUrls: ['./timerecord.component.css'],
})


export class TimeRecordComponent {
  title = 'Daily Time Record';

  time = new Date();
  employeeId = 1;
    headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private accountService: AccountService
  ) {
    setInterval(() => {
      this.time = new Date();
    }, 1000);

    if (this.accountService.userValue) {
      this.headers = new HttpHeaders({
        'Authorization': `Bearer ${this.accountService.userValue}`,
        'Content-Type': 'application/json'
      });
    }
  }

  save(recordType: number) {
      var recordData = new Record();
      recordData.employeeId = this.employeeId;
      recordData.recordType = recordType;

      this.http.post<Record>('http://localhost:5100/platform/service/api/employee/timesheet', recordData, { headers: this.headers }).subscribe((result) => {
        if (result) {
          this.http.get(`http://localhost:5100/platform/service/api/employee/timesheet/${this.employeeId}`, { headers: this.headers }).subscribe(data => {
            console.log(data);
          });
        } else {
          console.log("fail");
        }
      });
  }
}
