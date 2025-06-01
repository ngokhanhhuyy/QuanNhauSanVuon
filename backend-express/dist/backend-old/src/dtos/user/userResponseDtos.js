const userResponseDtoFactory = {
    createDetail(user) {
        return {
            id: user.id,
            userName: user.userName,
            canCreateUser: user.permission?.canCreateUser ?? false,
            canResetUserPassword: user.permission?.canResetUserPassword ?? false,
            canDeleteUser: user.permission?.canDeleteUser ?? false
        };
    }
};
export default userResponseDtoFactory;
