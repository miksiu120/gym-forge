import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../services/account-service/account-service.service';
import { AccountDetailsDto } from '../../interfaces/account.interfaces';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'page-profile',
  imports: [RouterModule, DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  constructor(private accountService: AccountService) {}
  accountDetails: AccountDetailsDto = {} as AccountDetailsDto;
  loading = true;
  loadFailed = false;

  ngOnInit(): void {
    this.accountService.getAccountDetails().subscribe({
      next: (response) => {
        this.accountDetails = response;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.loadFailed = true;
      }
    });
  }
}
