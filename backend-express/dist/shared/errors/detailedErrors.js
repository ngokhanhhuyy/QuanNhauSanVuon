export class ValidationError extends Error {
    failures;
    constructor(failures) {
        super();
        this.failures = failures;
    }
}
export class OperationError extends Error {
    failure;
    constructor(failure) {
        super();
        this.failure = failure;
    }
    static notFound(fieldPath) {
        return new OperationError({
            code: "NotFound",
            fieldPath: Array.isArray(fieldPath) ? fieldPath : [fieldPath],
            ruleValue: undefined
        });
    }
}
export class DuplicatedError extends Error {
    fieldNames;
    constructor(fieldNames) {
        super();
        this.fieldNames = fieldNames;
    }
}
