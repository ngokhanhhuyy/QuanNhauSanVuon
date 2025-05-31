import { ValidationError, ValidationFailure } from "@shared/errors";
import type { z } from "zod";

export function createValidationError(issues: z.ZodIssue[]): ValidationError {
  const failures: ValidationFailure[] = [];
  for (const issue of issues) {
    let ruleValue: any = 0;
    switch (issue.code) {
      case "too_small":
        ruleValue = issue.minimum;
        break;
      case "too_big":
        ruleValue = issue.maximum;
        break;
    }

    failures.push({
      fieldPath: issue.path as (string | number)[],
      code: issue.code,
      ruleValue,
    });
  }

  return new ValidationError(failures);
}
