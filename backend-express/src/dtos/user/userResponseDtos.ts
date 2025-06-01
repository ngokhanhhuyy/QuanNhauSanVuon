import type { User, UserPermission } from "@prisma/client";
import type { UserDetailResponseDto } from "@shared/dtos";

type PermissionIncludedUser = User & { permission: UserPermission | null };

const userResponseDtoFactory = {
  createDetail(user: PermissionIncludedUser): UserDetailResponseDto {
    return {
      id: user.id,
      userName: user.userName,
      canCreateUser: user.permission?.canCreateUser ?? false,
      canResetUserPassword: user.permission?.canResetUserPassword ?? false,
      canDeleteUser: user.permission?.canDeleteUser ?? false
    }
  }
}

export default userResponseDtoFactory;