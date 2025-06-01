import { PrismaClient } from "@prisma/client";
import { usePrismaClientErrorHandler } from "@/errors/prismaErrorHandler";
export class BaseService {
    prisma = new PrismaClient();
    prismaErrorHandler = usePrismaClientErrorHandler();
}
