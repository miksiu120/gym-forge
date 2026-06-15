import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AccountDetailsDto, MeasurementSystem, UpdateAccountDto } from '../../interfaces/account.interfaces';
import { AccountService } from '../../services/account-service/account-service.service';

@Component({
  selector: 'app-profile-edit',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './profile-edit.component.html',
  styleUrl: './profile-edit.component.scss',
})
export class ProfileEditComponent implements OnInit {
  readonly MeasurementSystem = MeasurementSystem;
  accountDetails: AccountDetailsDto | null = null;
  loading = true;
  saving = false;
  errorMessage = '';

  readonly profileForm: FormGroup;

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly accountService: AccountService,
    private readonly router: Router,
  ) {
    this.profileForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email, Validators.maxLength(160)]],
      name: ['', [Validators.maxLength(80)]],
      surname: ['', [Validators.maxLength(80)]],
      birthday: [''],
      weight: [null as number | null, [Validators.min(1), Validators.max(500)]],
      height: [null as number | null, [Validators.min(50), Validators.max(300)]],
      measurementSystem: [MeasurementSystem.metric, Validators.required],
      description: ['', [Validators.maxLength(500)]],
    });
  }

  ngOnInit(): void {
    this.accountService.getAccountDetails().subscribe({
      next: (details) => {
        this.accountDetails = details;
        this.profileForm.patchValue({
          email: details.email,
          name: details.name ?? '',
          surname: details.surname ?? '',
          birthday: details.birthDay ? String(details.birthDay).slice(0, 10) : '',
          weight: details.weight ?? null,
          height: details.height ?? null,
          measurementSystem: details.measurementSystem ?? MeasurementSystem.metric,
          description: details.description ?? '',
        });
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.errorMessage = 'Profile details could not be loaded. Please try again later.';
      },
    });
  }

  save(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    const value = this.profileForm.getRawValue();
    const payload: UpdateAccountDto = {
      email: value.email!.trim(),
      name: value.name?.trim() || null,
      surname: value.surname?.trim() || null,
      birthDay: value.birthday || null,
      weight: value.weight,
      height: value.height,
      measurementSystem: value.measurementSystem as MeasurementSystem,
      description: value.description?.trim() || null,
    };

    this.accountService.updateAccount(payload).subscribe({
      next: () => this.router.navigate(['/profile']),
      error: (error) => {
        this.saving = false;
        this.errorMessage = error.error?.message ?? 'Profile could not be saved. Please review the form.';
      },
    });
  }
}
