declare global {
  type SeatingMinimalDto = {
    id: number;
    name: string;
    position: PointDto;
  };

  type SeatingUpsertDto = {
    id: number | null;
    name: string;
    position: PointDto;
    areaId: number | null;
    hasBeenChanged: boolean;
    hasBeenDeleted: boolean;
  };
}

export { };