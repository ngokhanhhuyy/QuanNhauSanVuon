import type { Table } from "@prisma/client";

declare global {
  type TableBasicResponseDto = {
    id: number;
    name: string;
    positionX: number;
    positionY: number;
  };

  type TableDetailResponseDto = {
    order:
  }
}