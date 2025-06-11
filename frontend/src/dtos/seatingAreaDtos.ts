declare global {
  type SeatingAreaDto = {
    id: number;
    name: string;
    color: string;
    takenUpPositions: PointDto[];
    seatings: SeatingMinimalDto[];
  };
}

export { };