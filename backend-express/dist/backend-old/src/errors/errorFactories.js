import { ValidationError } from "@shared/errors";
export function useErrorFactory() {
    return {
        createValidationError(issues) {
            const failures
            for (const issue of issues) {
                let ruleValue = 0;
                switch (issue.code) {
                    case "too_small":
                        ruleValue = issue.minimum;
                        break;
                    case "too_big":
                        ruleValue = issue.maximum;
                        break;
                }
                failures.push({
                    fieldPath: issue.path,
                    code: issue.code,
                    ruleValue,
                });
            }
            return new ValidationError(failures);
        }
    };
}
