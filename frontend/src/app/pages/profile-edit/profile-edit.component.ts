import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RouterLinkActive } from '@angular/router';
import { AccountService } from '../../services/account-service/account-service.service';
import { AccountDetailsDto } from '../../interfaces/account.interfaces';

@Component({
  selector: 'app-profile-edit',
  imports: [RouterModule, RouterLinkActive],
  templateUrl: './profile-edit.component.html',
  styleUrl: './profile-edit.component.scss',
})
export class ProfileEditComponent {
  
  accountDetails: AccountDetailsDto = {} as AccountDetailsDto;
  constructor(private accountService: AccountService) {
  }
  ngOnInit() {
    this.accountService.getAccountDetails().subscribe({
      next: (response) => {
        console.log(response);
        this.accountDetails = response;

        console.log('DETAILS:', this.accountDetails);
      },
    });
  }
}
