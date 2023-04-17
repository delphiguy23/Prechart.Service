import { Klant } from "./klant";
import { WhkPremies } from "./whkPremies";

export class Werkgever {
  public id: string;
  public klant: Klant;
  public naam: string;
  public sector:number;
  public fiscaalNummer: string;
  public loonheffingenExtentie: string;
  public omzetbelastingExtentie: string;
  public whkPremies: WhkPremies[];
  public datumActiefVanaf: any;
  public datumActiefTot: any;
  public dateCreated: any;
  public dateLastModified: any;
  public actief: boolean;
}
