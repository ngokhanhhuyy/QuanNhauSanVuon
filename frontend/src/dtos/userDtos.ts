declare global {
  type UserDto = {
    id: number;
    userName: string;
    role: RoleDto;
  };
}

export { };