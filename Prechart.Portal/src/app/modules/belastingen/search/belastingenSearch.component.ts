import { AfterContentChecked, Component } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { Dropdown } from 'src/app/shared/models/dropdown';
import { Inhouding } from './models/inhouding';
import { PremieBedrag } from './models/premieBedrag';
import { ServiceHelper } from '../../../helpers/service';
import { Werkgever } from './models/werkgever';
import { Berekeningen } from './models/berekeningen';
import { DateHelper } from '../../../helpers/date';

@Component({
  selector: 'psp-search',
  templateUrl: './belastingenSearch.component.html',
})

export class BelastingenSearchComponent implements AfterContentChecked {
  isVisible = false;
  progressVisible = false;

  loontijdvakList: Dropdown[];
  loontijdvak: Dropdown;
  loontijdvakDescription: string;

  woondlandBeginselList: Dropdown[];
  woondlandBeginsel: Dropdown;

  inkomenGroen: any = 0;
  inkomenWit: any = 0;
  geboortedatum: any = new Date("2000-01-01");
  maxBirthdate: any = new Date();
  basisDagen: number = 1;
  basisDagenParam: number = 1;
  algemeneHeffingsKortingIndicator: boolean;
  taxRecords: any[] = [];

  taxRecordResult: any;
  socialeVerzekeringen: any;
  resultaatWhk: any;

  ahkIndicatorDescription: string;
  inhoudingTypeDescription: string;

  highLowList: Dropdown[];
  algemeenWerkloosheidsFonds: number = 1;
  algemeenWerkloosheidsFondsValue: any = 0;

  highLowUfoList: Dropdown[];
  wetArbeidsOngeschikheid: number = 1;
  wetArbeidsOngeschikheidValue: any = 0;

  verzekeringList: Dropdown[];
  ziektekostenVerzekeringsWet: number = 1;
  ziektekostenVerzekeringsWetValue: any = 0;

  klantList: Dropdown[];
  klant: Dropdown;
  werkgeverList: Dropdown[];
  werkgever: Dropdown;
  employeeList: Dropdown[];
  employee: Dropdown;
  isDisableBasisDagen: boolean = false;

  procesDatum: any = new Date();
  maxDate: any;
  minDate: any;

  constructor(
    private changeDetectorRef: ChangeDetectorRef,
    private service: ServiceHelper,
    private dateHelper: DateHelper,
  )
  {
    this.loontijdvakList = [
      { value: 1, label: 'Dag' },
      { value: 2, label: 'Week' },
      { value: 3, label: 'Vier Wekelijks' },
      { value: 4, label: 'Maand' },
      { value: 5, label: 'Kwartaal' },
    ];

    this.highLowList = [
      { value: 1, label: 'Hoog' },
      { value: 2, label: 'Laag' },
    ];

    this.highLowUfoList = [
      { value: 1, label: 'Hoog' },
      { value: 2, label: 'Laag' },
      { value: 3, label: 'Ufo' }
    ];

    this.verzekeringList = [
      { value: 1, label: 'Werkgever' },
      { value: 2, label: 'Werknemer' },
    ];

    this.klantList = [
      { value: 1, label: 'Klant 1' },
    ];

    this.employeeList = [
      { value: 1, label: 'Employee 1' },
    ];

    this.getTaxYears();
    this.getCountries();
    this.getWerkgevers();
  }

  ngAfterContentChecked() {
    this.changeDetectorRef.detectChanges();
  }

  getTaxYears() {
    this.service.get('belastingen','berekenen/jaar').subscribe((result) => {        
      if (result) {
        var maxYear = result[0];
        var minYear = result[result.length - 1];

        var strMaxDate = maxYear + "-12-31";
        var strMinDate = minYear + "-01-01";

        this.maxDate = new Date(strMaxDate);
        this.minDate = new Date(strMinDate);
      }
    });
  }

  getCountries() {
    var countryOptions: any[] = [];
    this.service.get('belastingen','berekenen/allewoonlandbeginsel').subscribe((result) => {
      if (result) {
        for (let country of result) {
          countryOptions.push({ value: country.id, label: country.woonlandbeginselBenaming });
        }
        this.woondlandBeginselList = countryOptions;
      }
    });
  }

  getWerkgevers() {
    var werkgeverOptions: any[] = [];
    this.service.get('werkgever', 'werkgever/all').subscribe((result) => {
      if (result) {
        for (let employer of result) {
          werkgeverOptions.push({ value: employer.fiscaalNummer, label: employer.naam });
        }
        this.werkgeverList = werkgeverOptions;
      }
    });
  }

  getInhoudingTypeDescription(type: any) {
    switch (type) {
      case 'White':
        this.inhoudingTypeDescription = 'Wit';
        break;
      case 'Green':
        this.inhoudingTypeDescription = 'Groen';
        break;
      case 'Both':
        this.inhoudingTypeDescription = 'Beide';
        break;
      default:
        this.inhoudingTypeDescription = '';
        break;
    }
  }

  search() {
    this.reset();
    this.progressVisible = true;

    if (this.loontijdvak.value == 1) {
      this.basisDagenParam = this.basisDagen;
    }

    var inhoudingBody: Inhouding = {
      woondlandBeginselId: this.woondlandBeginsel.value,
      procesDatum: this.dateHelper.formatDate(this.procesDatum),
      loontijdvak: this.loontijdvak.value,
      inkomenWit: this.inkomenWit,
      inkomenGroen: this.inkomenGroen,
      algemeneHeffingsKortingIndicator: this.algemeneHeffingsKortingIndicator,
      basisDagen: this.basisDagenParam,
      geboortedatum: this.dateHelper.formatDate(this.geboortedatum),
    };

    var premieBedragBody: PremieBedrag = {
      loonSocialVerzekeringen: this.inkomenWit,
      loonZiektekostenVerzekeringsWet: (this.inkomenWit + this.inkomenGroen),
      socialeVerzekeringenDatum: new Date(),
    };

    var werkgeverBody: Werkgever = {
      taxno: this.werkgever.value,
      inclusiveDate: new Date(),
    };

    var berekeningenBody: Berekeningen = {
      inhoudingParameters: inhoudingBody,
      premieBedragParameters: premieBedragBody,
      whkWerkgeverParameters: werkgeverBody,
      werkgever: this.werkgever.value,
      klantId: 1,
      employeeId: 1,
      premieBedragAlgemeenWerkloosheIdsFondsLaagHoog: 'Hoog',
      isPremieBedragUitvoeringsFondsvoordeOverheId: false,
      premieBedragWetArbeIdsOngeschikheIdLaagHoog: 'Hoog',
      payee: 'Werkgever',
    };

    this.service.post('belastingen', 'berekeningen/', berekeningenBody).subscribe((berekeningenResult) => {
      if (berekeningenResult) {
        this.socialeVerzekeringen = berekeningenResult.premieBedrag;
        this.resultaatWhk = berekeningenResult;

        if (berekeningenResult.inhouding) {
          this.taxRecordResult = berekeningenResult.inhouding;
          this.ahkIndicatorDescription = this.taxRecordResult.algemeneHeffingsKortingIndicator ? 'Waar' : 'Niet Waar';
          this.getInhoudingTypeDescription(this.taxRecordResult.inhoudingType);
          this.loontijdvakDescription = this.loontijdvak.label;
          this.algemeenWerkloosheidsFondsValue = this.socialeVerzekeringen.premieBedragAlgemeenWerkloosheidsFondsHoog;
          this.algemeenWerkloosheidsFonds = 1;
          this.wetArbeidsOngeschikheidValue = this.socialeVerzekeringen.premieBedragWetArbeidsOngeschikheidHoog;
          this.wetArbeidsOngeschikheid = 1;
          this.ziektekostenVerzekeringsWetValue = this.socialeVerzekeringen.premieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage;
          this.ziektekostenVerzekeringsWet = 1;
        }
        this.progressVisible = false;
      }
      else {
        this.isVisible = true;
        this.taxRecordResult = null;
        this.socialeVerzekeringen = null;
        this.resultaatWhk = null;
      }
    },
    (error) => {
      this.isVisible = true;
      this.progressVisible = false;
      }
    );
  }

  reset() {
    this.isVisible = false;
    this.taxRecordResult = null;
    this.socialeVerzekeringen = null;
    this.resultaatWhk = null;
  }

  loontijdvakOnChange(event: any) {
    var selected = event.value;
    this.isDisableBasisDagen = false;

    switch (selected.value) {
      case 2: this.basisDagen = 5;
        break;
      case 3: this.basisDagen = 20;
        break;
      case 4: this.basisDagen = 21.75;
        break;
      case 5: this.basisDagen = 65;
        break;
      default: this.basisDagen = 1;
        break;
    }

    if (selected.value != 1) {
      this.isDisableBasisDagen = true;
      this.basisDagenParam = 1;
    } else {
      this.basisDagenParam = this.basisDagen;
    }
  }

  werkloosheidsFondsOnChange(event: any) {
    if (event.value === 1) {
      this.algemeenWerkloosheidsFondsValue = this.socialeVerzekeringen.premieBedragAlgemeenWerkloosheidsFondsHoog;
    } else if (event.value) {
      this.algemeenWerkloosheidsFondsValue = this.socialeVerzekeringen.premieBedragAlgemeenWerkloosheidsFondsLaag;
    } else {
      this.algemeenWerkloosheidsFondsValue = 0;
    }
  }

  wetArbeidsOngeschikheidOnChange(event: any) {
    switch (event.value) {
      case 1: this.wetArbeidsOngeschikheidValue = this.socialeVerzekeringen.premieBedragWetArbeidsOngeschikheidHoog;
        break;
      case 2: this.wetArbeidsOngeschikheidValue = this.socialeVerzekeringen.premieBedragWetArbeidsOngeschikheidLaag;
        break;
      case 3: this.wetArbeidsOngeschikheidValue = this.socialeVerzekeringen.premieBedragUitvoeringsFondsvoordeOverheid;
        break;
      default: this.wetArbeidsOngeschikheidValue = 0;
        break;
    }
  }

  ziektekostenVerzekeringsWetOnChange(event: any) {
    if (event.value === 1) {
      this.ziektekostenVerzekeringsWetValue = this.socialeVerzekeringen.premieBedragZiektekostenVerzekeringsWetWerkgeversbijdrage;
    } else if (event.value) {
      this.ziektekostenVerzekeringsWetValue = this.socialeVerzekeringen.premieBedragZiektekostenVerzekeringsWetWerknemersbijdrage;
    } else {
      this.ziektekostenVerzekeringsWetValue = 0;
    }
  }
}
