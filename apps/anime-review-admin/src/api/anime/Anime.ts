import { Season } from "../season/Season";

export type Anime = {
  createdAt: Date;
  episodesCount: number;
  id: string;
  name: string;
  seasons?: Array<Season>;
  updatedAt: Date;
};
