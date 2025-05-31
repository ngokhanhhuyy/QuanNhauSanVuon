import { PrismaClient } from "@prisma/client";
import {
  createMenuItemListResponseDto,
  createMenuItemDetailResponseDto
} from "@/dtos/menuItem/menuItemResponseDtos";
import type {
  MenuItemListSortedByFields,
  MenuItemListRequestDto,
  MenuItemListResponseDto,
  MenuItemDetailResponseDto
} from "@shared/dtos";
import { NotFoundError } from "@shared/errors";
import type { IMenuItemService } from "@shared/services/menuItemService";

type ListRequestDto = MenuItemListRequestDto;
type ListResponseDto = MenuItemListResponseDto;

export class MenuItemService implements IMenuItemService {
  private _prisma = new PrismaClient();

  public async getListAsync(requestDto?: ListRequestDto): Promise<ListResponseDto> {
    // Default request values.
    const {
      sortedByAscending = true,
      sortedByField = "name",
      page = 1,
      resultsPerPage = 15
    } = requestDto ?? { };

    // Compute result count.
    const resultCount = await this._prisma.menuItem.count();
    if (!resultCount) {
      return createMenuItemListResponseDto();
    }

    // Compute field and direction to sort.
    const sortedByConditions: SortedByConditions<MenuItemListSortedByFields> = {
      name: undefined,
      defaultAmount: undefined,
      defaultVatPercentage: undefined,
      createdDateTime: undefined
    };

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
    const menuItems = await this._prisma.menuItem.findMany({
      include: { category: true },
      orderBy: sortedByConditions,
      skip: resultsPerPage * (page - 1),
      take: resultsPerPage
    });

    const pageCount = Math.ceil(resultCount / resultsPerPage);
    return createMenuItemListResponseDto({ pageCount, items: menuItems });
  }

  public async getDetailAsync(id: number): Promise<MenuItemDetailResponseDto> {
    const menuItem = await this._prisma.menuItem.findFirst({
      where: { id },
      include: { category: true }
    });

    if (!menuItem) {
      throw new NotFoundError();
    }

    return createMenuItemDetailResponseDto(menuItem);
  }
}