export enum MeasurementSystem {
  Repetitive = 'repetetive',
  Timed = 'timed',
}

export interface Exercise {
  name: string;
  description?: string;
  sets: number;
  type: MeasurementSystem;
  repetitions?: number | null;
  duration?: number | null;
  tempo?: number;
}
