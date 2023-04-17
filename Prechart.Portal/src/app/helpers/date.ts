import { Injectable } from "@angular/core";
import * as moment from "moment";

@Injectable({ providedIn: 'root' })
export class DateHelper {

  formatDate(date: any) {
    return moment(date).local().format('yyyy-MM-DD');
  }

}
