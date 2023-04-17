import { AfterContentChecked, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { ConfirmationService } from 'primeng/api';
import { DateHelper } from '../../helpers/date';
import { ServiceHelper } from '../../helpers/service';
import { Werkgever } from './models/werkgever';
import { WhkPremies } from './models/whkPremies';

@Component({
  selector: 'psp-werkgever-management',
  templateUrl: './werkgeverManagement.component.html',
  providers: [MessageService, ConfirmationService],
})

export class WerkgeverManagementComponent implements OnInit, AfterContentChecked {
  constructor(
    private messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private service: ServiceHelper,
    public formBuilder: FormBuilder,
    private router: Router,
    private confirmationService: ConfirmationService,
    private dateHelper: DateHelper,
  ) {
    this.router.routeReuseStrategy.shouldReuseRoute = () => {
      return false;
    };
  }

  werkgeverCols: any[];
  werkgevers: Werkgever[];

  colsWhkPremies: any[];

  werkgeverEditDialog: boolean;
  werkgeverModel: Werkgever;

  whkPremieEditDialog: boolean;
  whkPremieModel: WhkPremies;

  ngOnInit() {    
    this.service.get('werkgever', 'werkgever/all').subscribe((result) => {
     
      if (result) {
        this.werkgeverCols = [
          { field: 'naam', header: 'Naam' },
          { field: 'sector', header: 'Sector' },
          { field: 'fiscaalNummer', header: 'Fiscaalnummer' },
          { field: 'loonheffingenExtentie', header: 'Loonheffingen Extentie' },
          { field: 'omzetbelastingExtentie', header: 'Omzetbelasting Extentie' },
          { field: 'datumActiefVanaf', header: 'Datum Actief Vanaf' },
          { field: 'datumActiefTot', header: 'Datum Actief Tot' },
          { field: 'klantName', header: 'Klantnaam' },
        ];

        this.colsWhkPremies = [
          { field: 'wgaVastWerkgever', header: 'WGA Vast Werkgever %' },
          { field: 'wgaVastWerknemer', header: 'WGA Vast Werknemer %' },
          { field: 'flexWerkgever', header: 'Flex Werkgever %' },
          { field: 'flexWerknemer', header: 'Flex Werknemer %' },
          { field: 'zwFlex', header: 'ZW Flex %' },
          { field: 'totaal', header: 'Totaal %' },
          { field: 'actiefVanaf', header: 'Actief Vanaf' },
          { field: 'actiefTot', header: 'Actief Tot' },
        ];

        var werkgeverList: any[] = [];
        for (let werkgever of result) {

          var whkPremieList: any[] = [];
          for (let premie of werkgever.whkPremies) {
            whkPremieList.push({
                id: premie.id,
                werkgeverId: werkgever.id,
                wgaVastWerkgever: premie.wgaVastWerkgever,
                wgaVastWerknemer: premie.wgaVastWerknemer,
                flexWerkgever: premie.flexWerkgever,
                flexWerknemer: premie.flexWerknemer,
                zwFlex: premie.zwFlex,
                totaal: premie.totaal,
                actiefVanaf: this.dateHelper.formatDate(premie.actiefVanaf),              
                actiefTot: this.dateHelper.formatDate(premie.actiefTot),
                dateCreated: premie.dateCreated,
                dateLastModified: premie.dateLastModified,
                sqlId: premie.sqlId,
                actief: premie.actief,
            });
          }

          werkgeverList.push({
            id: werkgever.id,
            naam: werkgever.naam,
            sector: werkgever.sector,
            fiscaalNummer: werkgever.fiscaalNummer,
            loonheffingenExtentie: werkgever.loonheffingenExtentie,
            omzetbelastingExtentie: werkgever.omzetbelastingExtentie,
            datumActiefVanaf: this.dateHelper.formatDate(werkgever.datumActiefVanaf),
            datumActiefTot: this.dateHelper.formatDate(werkgever.datumActiefTot),
            klantName: werkgever.klant.klantName,
            dateCreated: werkgever.dateCreated,
            dateLastModified: werkgever.dateLastModified,
            actief: werkgever.actief,
            klant: werkgever.klant,
            whkPremies: whkPremieList,
          });    
        }
        this.werkgevers = werkgeverList;
      }      
    });
  }

  ngAfterContentChecked() {
    this.changeDetectorRef.detectChanges();
  }

  werkgeverEdit(werkgever: any) {
    this.werkgeverModel = werkgever;
    this.werkgeverEditDialog = true;
  }

  hideDialog() {
    this.werkgeverEditDialog = false;
    this.reloadPage();
  }

  deleteWerkgever(werkgever: any) {
    this.confirmationService.confirm({
      message: 'Weet je zeker dat je werkgever ' + werkgever.naam + ' wilt verwijderen?',
      header: 'Bevestigen',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        werkgever.actief = false;
        this.werkgeverModel = werkgever;
        this.saveWerkgever();
        this.reloadPage();
      }
    });
  }

  saveWerkgever() {
    this.service.post('werkgever', 'werkgever/upsert/werkgever', this.werkgeverModel).subscribe((result) => {
        if (result) {
          this.setMessage('success', 'Succes', 'Succes opslaan');
          this.werkgeverEditDialog = false;
          this.reloadPage();
        } else {
          this.setMessage('error', 'Fout', 'Fout opslaan');
        }
      },
      (error) => {
          this.werkgeverEditDialog = false;
          this.setMessage('error', 'Fout', 'Fout opslaan');
        }
    );
  }

  whkPremieCreate(werkgeverId: any) {
    this.whkPremieModel = new WhkPremies;
    this.whkPremieModel.werkgeverId = werkgeverId;
    this.whkPremieModel.sqlId = 0;
    this.whkPremieModel.actief = true;
    this.whkPremieEditDialog = true;
  }

  whkPremieEdit(whkPremie: any) {
    this.whkPremieModel = whkPremie;
    this.whkPremieEditDialog = true;
  }

  deleteWhkPremie(whkPremie: any) {
    this.confirmationService.confirm({
      message: 'Weet je zeker dat je wilt verwijderen?',
      header: 'Bevestigen',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        whkPremie.actief = false;
        this.whkPremieModel = whkPremie;
        this.saveWhkPremie();
        this.reloadPage();
      }
    });
  }

  saveWhkPremie() {
    this.service.post('werkgever', 'werkgever/upsert/whk', this.whkPremieModel).subscribe((result) => {
      if (result) {
        this.setMessage('success', 'Succes', 'Succes opslaan');
        this.whkPremieEditDialog = false;
        this.reloadPage();
      } else {
        this.setMessage('error', 'Fout', 'Fout opslaan');
      }
    },
      (error) => {
        this.werkgeverEditDialog = false;
        this.setMessage('error', 'Fout', 'Fout opslaan');
      }
    );
  }

  setMessage(serverity: string, summary: string, message: string) {
    this.messageService.add({ key: 'tr', severity: serverity, summary: summary, detail: message, life: 10000 });
  }

  reloadPage() {
    this.router.navigateByUrl('/werkgever/management');
  }
}
