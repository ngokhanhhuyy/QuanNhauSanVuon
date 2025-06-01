import type {
  MenuItemListSortedByFields,
  MenuItemListRequestDto,
  MenuItemUpsertRequestDto
} from "@shared/dtos";
import { z } from "zod";
import { useErrorFactory } from "@/errors/errorFactories";

// Utils.
const errorFactory = useErrorFactory();

// Schema.
const listSchema = z.object({
  sortedByAscending: z.boolean().default(true),
  sortedByField: z
    .enum(["name", "defaultAmount", "defaultVatPercentage", "createdDateTime"])
    .default("createdDateTime"),
  page: z.number().int().min(1).default(1),
  resultsPerPage: z.number().int().min(5).max(15).default(15)
});

const upsertSchema = z.object({
  name: z.string().min(1).max(150),
  defaultAmount: z.number().int().min(1),
  defaultVatPercentage: z.number().int().min(0).max(100),
  unit: z.string().min(1),
  thumbnailUrl: z.string().max(255).nullable().transform(value => value || null),
  categoryId: z.number().int().min(1).nullable()
});

// Factory.
const menuItemRequestDtoFactory = {
  createList(requestParams: Record<string, string>): MenuItemListRequestDto {
    try {
      return listSchema.parse(requestParams);
    } catch (error) {
      if (error instanceof z.ZodError) {
        throw errorFactory.createValidationError(error.issues);
      }

      throw error;
    }
  },

  createUpsert(requestBody: object): MenuItemUpsertRequestDto {
    try {
      return upsertSchema.parse(requestBody);
    } catch (error) {
      if (error instanceof z.ZodError) {
        throw errorFactory.createValidationError(error.issues);
      }

      throw error;
    }
  }
}

export default menuItemRequestDtoFactory;