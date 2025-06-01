
type FailureFieldPath = (string | number)[];
type Failure = {
  code: string;
  ruleValue: any;
}

export type ValidationFailure = Failure & { fieldPath: FailureFieldPath };
export type OperationFailure = Failure & { fieldPath?: FailureFieldPath };

export class ValidationError extends Error {
  public readonly failures: ValidationFailure[];

  constructor(failures: ValidationFailure[]) {
    super();
    this.failures = failures;
  }
}

export class OperationError extends Error {
  public readonly failure: OperationFailure;

  constructor(failure: OperationFailure) {
    super();
    this.failure = failure;
  }

  static notFound(fieldPath: string | number | (string | number)[]): OperationError {
    return new OperationError({
      code: "NotFound",
      fieldPath: Array.isArray(fieldPath) ? fieldPath : [fieldPath],
      ruleValue: undefined
    })
  }
}

export class DuplicatedError extends Error {
  public readonly fieldNames: string[];

  constructor(fieldNames: string[]) {
    super();
    this.fieldNames = fieldNames;
  }
}