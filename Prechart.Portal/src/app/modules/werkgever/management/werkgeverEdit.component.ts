import { Component, Input } from '@angular/core';

@Component({
  selector: 'psp-werkgever-edit',
  templateUrl: './werkgeverEdit.component.html',
})

export class WerkgeverEditComponent {
  constructor() { }

  @Input() model: any;

}
