export type PropertyErrorDetail = {
  propertyPathElements: string | number;
  errorMessage: string;
}

class DetailIncludedError extends Error {
  public readonly details: PropertyErrorDetail[];

  constructor(details: PropertyErrorDetail[]) {
    super();
    this.details = details;
  }
}

// Exceptions representing request error
export class ValidationError extends DetailIncludedError {}
export class DuplicatedError extends DetailIncludedError {}
export class OperationError extends DetailIncludedError {}
export class NotFoundError extends DetailIncludedError {}
export class AuthenticationError extends Error {}
export class AuthorizationError extends Error {}
export class ConcurrencyError extends Error {}
export class InternalServerError extends Error {}
export class UndefinedError extends Error {}
export class ConnectionError extends Error {}
export class FileTooLargeError extends Error {}
