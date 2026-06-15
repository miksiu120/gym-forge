export interface CreateUserDto {
  nickname: string;
  email: string;
  name: string;
  surname: string;
  password: string;
  confirmPassword: string;
  measurementValue?: string;
}

export interface LoginUserDto {
  nickname: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  nickname: string;
  message: string;
}

export enum MeasurementSystem {
  imperial = 'imperial',
  metric = 'metric',
}

export interface AccountDetailsDto {
  nickname: string;
  name?: string;
  surname?: string;
  email: string;
  weight?: number;
  height?: number;
  role: string;
  createdAt: Date;
  birthDay?: Date;
  description?: string;
  measurementSystem: MeasurementSystem;
}

export interface UpdateAccountDto {
  email: string;
  name?: string | null;
  surname?: string | null;
  weight?: number | null;
  height?: number | null;
  birthDay?: string | null;
  description?: string | null;
  measurementSystem: MeasurementSystem;
}
