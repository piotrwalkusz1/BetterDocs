import { Anime } from "../anime/Anime";

export type Season = {
  anime?: Anime | null;
  createdAt: Date;
  id: string;
  name: string;
  updatedAt: Date;
};
