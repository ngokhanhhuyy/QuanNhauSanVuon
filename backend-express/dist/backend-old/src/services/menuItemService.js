import { Prisma } from "@prisma/client";
import { responseDtoFactory } from "@/dtos";
import { NotFoundError, OperationError } from "@shared/errors";
import { BaseService } from "./baseService";
export class MenuItemService extends BaseService {
    async getListAsync(requestDto) {
        // Default request values.
        const { sortedByAscending = true, sortedByField = "name", page = 1, resultsPerPage = 15 } = requestDto ?? {};
        // Compute result count.
        const resultCount = await this.prisma.menuItem.count();
        if (!resultCount) {
            return responseDtoFactory.menuItem.createList(0, []);
        }
        // Compute field and direction to sort.
        const sortedByConditions = {};
        switch (sortedByField) {
            case "name":
                sortedByConditions.name = sortedByAscending ? "asc" : "desc";
                break;
            case "defaultAmount":
                sortedByConditions.defaultAmount = sortedByAscending ? "asc" : "desc";
                break;
            case "defaultVatPercentage":
                sortedByConditions.defaultVatPercentage = sortedByAscending ? "asc" : "desc";
                break;
            case "createdDateTime":
                sortedByConditions.createdDateTime = sortedByAscending ? "asc" : "desc";
                break;
        }
        // Fetch the entities from the database.
        const menuItems = await this.prisma.menuItem.findMany({
            include: { category: true },
            orderBy: sortedByConditions,
            skip: resultsPerPage * (page - 1),
            take: resultsPerPage
        });
        const pageCount = Math.ceil(resultCount / resultsPerPage);
        return responseDtoFactory.menuItem.createList(pageCount, menuItems);
    }
    async getDetailAsync(id) {
        const menuItem = await this.prisma.menuItem.findFirst({
            where: { id },
            include: { category: true }
        });
        if (!menuItem) {
            throw new NotFoundError();
        }
        return responseDtoFactory.menuItem.createDetail(menuItem);
    }
    async createAsync(requestDto) {
        try {
            const menuItem = await this.prisma.menuItem.create({
                data: {
                    name: requestDto.name,
                    defaultAmount: requestDto.defaultAmount,
                    defaultVatPercentage: requestDto.defaultVatPercentage,
                    unit: requestDto.unit,
                    thumbnailUrl: requestDto.thumbnailUrl,
                    createdDateTime: new Date(),
                    categoryId: requestDto.categoryId
                }
            });
            return menuItem.id;
        }
        catch (error) {
            if (error instanceof Prisma.PrismaClientKnownRequestError) {
                const handledResult = this.prismaErrorHandler.handle(error);
                if (handledResult.isForeignKeyConstraintError) {
                    throw OperationError.notFound("categoryId");
                }
            }
            throw error;
        }
    }
    async updateAsync(id, requestDto) {
        try {
            await this.prisma.menuItem.update({
                where: { id },
                data: {
                    name: requestDto.name,
                    defaultAmount: requestDto.defaultAmount,
                    defaultVatPercentage: requestDto.defaultVatPercentage,
                    unit: requestDto.unit,
                    thumbnailUrl: requestDto.thumbnailUrl,
                    lastUpdatedDateTime: new Date(),
                    categoryId: requestDto.categoryId
                }
            });
        }
        catch (error) {
            if (error instanceof Prisma.PrismaClientKnownRequestError) {
                const handledResult = this.prismaErrorHandler.handle(error);
                if (handledResult.isNotFoundError) {
                    throw new NotFoundError();
                }
                if (handledResult.isForeignKeyConstraintError) {
                    throw OperationError.notFound("categoryId");
                }
            }
            throw error;
        }
    }
    async deleteAsync(id) {
        try {
            await this.prisma.menuItem.delete({ where: { id } });
        }
        catch (error) {
            if (error instanceof Prisma.PrismaClientKnownRequestError) {
                const handledResult = this.prismaErrorHandler.handle(error);
                if (handledResult.isNotFoundError) {
                    throw new NotFoundError();
                }
            }
            throw error;
        }
    }
}
