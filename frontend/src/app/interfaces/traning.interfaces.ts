export enum MeasurementSystem {
  Repetitive = 'repetitive',
  Timed = 'timed',
}

export interface Exercise {
  id?: number;
  name: string;
  description?: string;
  sets: number;
  type: MeasurementSystem;
  repetitions?: number | null;
  duration?: number | null;
  tempo?: string | null;
  setResults?: ExerciseSetResult[];
}

export interface TrainingUnit {
  id?: number;
  name: string;
  startTime?: string;
  completedAt?: string | null;
  notes?: string | null;
  exercises: Exercise[];
}

export interface CreateTrainingPlan {
  name: string;
  from: string;
  to: string;
  trainingUnits: TrainingUnit[];
}

export interface TrainingPlanSummary extends CreateTrainingPlan {
  id: number;
}

export interface ExerciseSetResult {
  setNumber: number;
  weight?: number | null;
  repetitions?: number | null;
  duration?: number | null;
  rpe?: number | null;
}

export interface DueTrainingUnit {
  planId: number;
  planName: string;
  trainingUnit: TrainingUnit;
}

export interface CompleteTrainingUnit {
  notes?: string | null;
  exercises: Array<{
    exerciseId: number;
    sets: ExerciseSetResult[];
  }>;
}

export interface TrainingStatisticsSession {
  planName: string;
  trainingName: string;
  completedAt: string;
  exerciseCount: number;
  setCount: number;
}

export interface TrainingStatistics {
  completedSessions: number;
  completedSets: number;
  totalVolume: number;
  lastCompletedAt?: string | null;
  recentSessions: TrainingStatisticsSession[];
  weightProgress: TrainingWeightProgress[];
}

export interface TrainingWeightProgress {
  exerciseName: string;
  points: TrainingWeightPoint[];
}

export interface TrainingWeightPoint {
  completedAt: string;
  weight: number;
}
