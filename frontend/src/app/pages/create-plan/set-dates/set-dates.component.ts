import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateTrainingPlan, TrainingUnit } from '../../../interfaces/traning.interfaces';

interface SchedulePreviewItem {
  name: string;
  startTime: string;
}

@Component({
  selector: 'component-set-dates',
  imports: [ReactiveFormsModule, DatePipe],
  templateUrl: './set-dates.component.html',
  styleUrl: './set-dates.component.scss',
})
export class SetDatesComponent {
  @Input() units: TrainingUnit[] = [];
  @Input() saving = false;
  @Output() savePlan = new EventEmitter<CreateTrainingPlan>();

  readonly weekdays = [
    { value: 1, label: 'Monday' },
    { value: 2, label: 'Tuesday' },
    { value: 3, label: 'Wednesday' },
    { value: 4, label: 'Thursday' },
    { value: 5, label: 'Friday' },
    { value: 6, label: 'Saturday' },
    { value: 0, label: 'Sunday' },
  ];

  readonly scheduleForm;
  validationMessage = '';

  constructor(private readonly formBuilder: FormBuilder) {
    const today = this.toDateInput(new Date());
    const fourWeeksLater = new Date();
    fourWeeksLater.setDate(fourWeeksLater.getDate() + 27);

    this.scheduleForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      from: [today, Validators.required],
      to: [this.toDateInput(fourWeeksLater), Validators.required],
      cycleWeeks: [1, [Validators.required, Validators.min(1), Validators.max(2)]],
      slots: this.formBuilder.array<FormGroup>([]),
    });

    this.scheduleForm.valueChanges.subscribe(() => {
      this.validationMessage = '';
    });
  }

  get slots(): FormArray<FormGroup> {
    return this.scheduleForm.controls.slots;
  }

  get cycleWeeks(): number {
    return Number(this.scheduleForm.controls.cycleWeeks.value ?? 1);
  }

  ngOnInit(): void {
    this.rebuildSlots();
  }

  setCycleWeeks(weeks: number): void {
    this.scheduleForm.controls.cycleWeeks.setValue(weeks);
    this.slots.controls.forEach((slot) => {
      if (Number(slot.controls['cycleWeek'].value) > weeks) {
        slot.controls['cycleWeek'].setValue(1);
      }
    });
  }

  getPreview(): SchedulePreviewItem[] {
    return this.buildOccurrences().slice(0, 6).map((unit) => ({
      name: unit.name,
      startTime: unit.startTime!,
    }));
  }

  getOccurrenceCount(): number {
    return this.buildOccurrences().length;
  }

  unitLabel(index: number): string {
    return index < 26 ? String.fromCharCode(65 + index) : String(index + 1);
  }

  submit(): void {
    const value = this.scheduleForm.getRawValue();

    if (this.scheduleForm.invalid) {
      this.scheduleForm.markAllAsTouched();
      this.validationMessage = 'Complete the plan name and schedule for every training day.';
      return;
    }

    if (value.from! > value.to!) {
      this.validationMessage = 'The end date cannot be earlier than the start date.';
      return;
    }

    const occurrences = this.buildOccurrences();
    if (occurrences.length === 0) {
      this.validationMessage = 'This date range does not contain any selected training day. Extend the plan or change the schedule.';
      return;
    }

    this.savePlan.emit({
      name: value.name!.trim(),
      from: new Date(`${value.from}T00:00:00`).toISOString(),
      to: new Date(`${value.to}T23:59:59`).toISOString(),
      trainingUnits: occurrences,
    });
  }

  private rebuildSlots(): void {
    this.slots.clear();
    this.units.forEach((_, index) => {
      this.slots.push(this.formBuilder.group({
        cycleWeek: [index % this.cycleWeeks + 1, Validators.required],
        dayOfWeek: [this.defaultDay(index), Validators.required],
        time: ['18:00', Validators.required],
      }));
    });
  }

  private buildOccurrences(): TrainingUnit[] {
    const value = this.scheduleForm.getRawValue();
    if (!value.from || !value.to || this.slots.length !== this.units.length || value.from > value.to) {
      return [];
    }

    const from = new Date(`${value.from}T00:00:00`);
    const to = new Date(`${value.to}T23:59:59`);
    const cycleStart = this.startOfWeek(from);
    const occurrences: TrainingUnit[] = [];

    for (const date = new Date(from); date <= to; date.setDate(date.getDate() + 1)) {
      const daysFromCycleStart = Math.floor((this.localDateValue(date) - this.localDateValue(cycleStart)) / 86_400_000);
      const cycleWeek = Math.floor(daysFromCycleStart / 7) % this.cycleWeeks + 1;

      this.units.forEach((unit, index) => {
        const slot = this.slots.at(index).getRawValue();
        if (Number(slot['cycleWeek']) !== cycleWeek || Number(slot['dayOfWeek']) !== date.getDay()) return;

        const [hours, minutes] = String(slot['time']).split(':').map(Number);
        const startTime = new Date(date);
        startTime.setHours(hours, minutes, 0, 0);
        occurrences.push({
          ...unit,
          exercises: unit.exercises.map((exercise) => ({ ...exercise })),
          startTime: startTime.toISOString(),
        });
      });
    }

    return occurrences.sort((a, b) => a.startTime!.localeCompare(b.startTime!));
  }

  private startOfWeek(date: Date): Date {
    const monday = new Date(date);
    const offset = date.getDay() === 0 ? -6 : 1 - date.getDay();
    monday.setDate(date.getDate() + offset);
    monday.setHours(0, 0, 0, 0);
    return monday;
  }

  private defaultDay(index: number): number {
    const days = [1, 3, 5, 2, 4, 6, 0];
    return days[index % days.length];
  }

  private localDateValue(date: Date): number {
    return Date.UTC(date.getFullYear(), date.getMonth(), date.getDate());
  }

  private toDateInput(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
