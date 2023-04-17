import { Component, Input } from '@angular/core';

@Component({
  selector: 'psp-whkpremie-edit',
  templateUrl: './whkPremieEdit.component.html',
})

export class WhkPremieEditComponent {
  constructor() { }

  @Input() model: any;

}
