export * from "./detailedErrors";
export class AuthenticationError extends Error { }
export class AuthorizationError extends Error { }
export class NotFoundError extends Error { }
export class ConcurrencyError extends Error { }