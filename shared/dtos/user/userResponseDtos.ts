export type UserDetailResponseDto = {
  id: number;
  userName: string;
  canCreateUser: boolean;
  canResetUserPassword: boolean;
  canDeleteUser: boolean;
};