import { PrismaClient } from "@prisma/client";
import { usePrismaClientErrorHandler } from "@/errors/prismaErrorHandler";

export abstract class BaseService {
  protected prisma = new PrismaClient();
  protected prismaErrorHandler = usePrismaClientErrorHandler();
}