import { TaxDetails } from "./taxDetails";

export class Employee {
  public significantAchternaam: string;
  public voorletter: string;
  public sofinummer: string;
  public klant: string;
  public loonheffingsnummer: string;
  public adresBinnenland: any;
  public adresBuitenland: any;
  public taxDetails: TaxDetails;
  public taxCumulative: any;
}
