import { Inhouding } from "./inhouding";
import { PremieBedrag } from "./premieBedrag";
import { Werkgever } from "./werkgever";

export class Berekeningen {
  public inhoudingParameters: Inhouding;
  public premieBedragParameters: PremieBedrag;
  public whkWerkgeverParameters: Werkgever;
  public klantId: number;
  public werkgever: number;
  public employeeId: number;
  public premieBedragAlgemeenWerkloosheIdsFondsLaagHoog: string;
  public isPremieBedragUitvoeringsFondsvoordeOverheId: boolean;
  public premieBedragWetArbeIdsOngeschikheIdLaagHoog: string;
  public payee: string;
}
