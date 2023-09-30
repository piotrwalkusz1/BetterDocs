import { AnimeWhereUniqueInput } from "../anime/AnimeWhereUniqueInput";

export type SeasonUpdateInput = {
  anime?: AnimeWhereUniqueInput | null;
  name?: string;
};
