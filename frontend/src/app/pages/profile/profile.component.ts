import { Component } from '@angular/core';
import { AccountService } from '../../services/account-service/account-service.service';
import { AccountDetailsDto } from '../../interfaces/account.interfaces';
import { response } from 'express';
@Component({
  selector: 'page-profile',
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  constructor(private accountService: AccountService) {}

  accountDetails: AccountDetailsDto = {} as AccountDetailsDto;

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
