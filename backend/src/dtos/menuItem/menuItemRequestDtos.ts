import type { MenuItemListRequestDto } from "@shared/dtos";
import { z } from "zod";
import { createValidationError } from "@/errors/errorFactories";
import { useRequestParsingUtils } from "@/utils/requestParsingUtils";

// Utils.
const requestParsingUtils = useRequestParsingUtils();

// Schema.
const schema = z.object({
  sortedByField: z
    .enum([
      "name",
      "defaultAmount",
      "defaultVatPercentage",
      "createdDateTime"
    ]).default("name"),
  sortedByAscending: z.boolean().default(true),
  page: z.number().min(1).max(Number.MAX_SAFE_INTEGER).default(10),
  resultsPerPage: z.number().min(5).max(50).default(10)
});

function createList(params: URLSearchParams): MenuItemListRequestDto {
  try {
    const parsedData = schema.parse(requestParsingUtils.searchParamsToObject(params));
    return {
      sortedByField: parsedData.sortedByField,
      sortedByAscending: parsedData.sortedByAscending,
      page: parsedData.page,
      resultsPerPage: parsedData.resultsPerPage
    };
  } catch (error) {
    if (error instanceof z.ZodError) {
      throw createValidationError(error.issues);
    }

    throw error;
  }
}

export { createList as createMenuItemListRequestDto };