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
  measurementSystem: string;
}

