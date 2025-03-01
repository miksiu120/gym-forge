export interface CreateUserDto {
  nickname: string;
  email: string;
  password: string;
  confirmPassword: string;
  weight?: number;
  height?: number;
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
