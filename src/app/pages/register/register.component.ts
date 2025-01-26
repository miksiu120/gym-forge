import { Component } from '@angular/core';
import { RouterModule, RouterLinkActive } from '@angular/router';
@Component({
    selector: 'page-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.scss'],
    imports: [RouterModule, RouterLinkActive]
})
export class RegisterComponent {
    constructor() { }
}