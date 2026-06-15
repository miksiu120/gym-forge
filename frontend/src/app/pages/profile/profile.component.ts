import { DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { AccountService } from '../../services/account-service/account-service.service';
import { AccountDetailsDto } from '../../interfaces/account.interfaces';
import { RouterModule, RouterLinkActive } from '@angular/router';
@Component({
  selector: 'page-profile',
  imports: [RouterModule, RouterLinkActive, DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent {
  constructor(private accountService: AccountService) {}
  accountDetails: AccountDetailsDto = {} as AccountDetailsDto;

  ngOnInit() {
    this.accountService.getAccountDetails().subscribe({
      next: (response) => {
        this.accountDetails = response;
      },
    });
  }
}
