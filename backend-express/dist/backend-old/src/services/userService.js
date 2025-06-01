import { responseDtoFactory } from "@/dtos";
import { NotFoundError } from "@shared/errors";
/**
 * Creates a service to perform user-related operations.
 *
 * @param prisma An instance of the {@link PrismaClient} class.
 * @returns An object containing the APIs to perform the operations.
 */
export function createUserService(prisma) {
    return {
        /**
         * Retrieves the details of a specific user, specified by the id of the user.
         *
         * @param id The id of the target user.
         * @returns A {@link Promise} representing the asynchronous operation, which result is an
         * object containing the details of the target user.
         * @example getDetailAsync(1);
         *
         * @throws {NotFoundError} Throws when the user with the specified id doesn't exist or has
         * already been deleted.
         */
        async getDetailAsync(id) {
            const user = await prisma.user.findUnique({
                where: { id },
                include: { permission: true }
            });
            if (!user) {
                throw new NotFoundError();
            }
            return responseDtoFactory.user.createDetail(user);
        }
    };
}
